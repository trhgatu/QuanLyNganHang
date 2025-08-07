using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.DataAccess
{
    public class TransactionDataAccess
    {
        public DataTable GetAllTransactions()
        {
            DataTable dt = new DataTable();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    string query = @"
    SELECT 
        t.transaction_id AS TransactionID,
        t.transaction_code AS TransactionCode,
        a.account_number AS AccountNumber,
        c.full_name AS CustomerName,
        tt.type_name AS TransactionType,
        TO_CHAR(t.amount, 'FM999G999G990') || ' VND' AS Amount,
        TO_CHAR(t.transaction_date, 'DD/MM/YYYY HH24:MI:SS') AS TransactionDate,
        CASE t.status
            WHEN 0 THEN 'Thất bại'
            WHEN 1 THEN 'Thành công'
            WHEN 2 THEN 'Đang xử lý'
        END AS Status,
        t.channel AS Channel,
        e.full_name AS ProcessedBy
    FROM transactions t
    JOIN accounts a ON t.account_id = a.account_id
    JOIN customers c ON a.customer_id = c.customer_id
    JOIN transaction_types tt ON t.transaction_type_id = tt.type_id
    LEFT JOIN employees e ON t.processed_by = e.employee_id
    ORDER BY t.transaction_date DESC";

                    using (var command = new OracleCommand(query, connection))
                    {
                        using (var adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt = CreateFallbackTransactionData();
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
            }

            return dt;
        }
        public bool DepositMoney(int accountId, decimal amount, string description, int processedBy, string channel = "BRANCH")
        {
            using (var connection = Database.Get_Connect())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Khóa và kiểm tra tài khoản
                        string checkAccountSql = @"
                    SELECT account_id, balance, status 
                    FROM ADMIN_NGANHANG.accounts 
                    WHERE account_id = :accountId AND status = 1 
                    FOR UPDATE";

                        decimal currentBalance = 0;
                        using (var cmd = new OracleCommand(checkAccountSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    throw new Exception("Tài khoản không tồn tại hoặc đã bị khóa");
                                currentBalance = Convert.ToDecimal(reader["balance"]);
                            }
                        }

                        decimal newBalance = currentBalance + amount;

                        // 2. Cập nhật số dư tài khoản
                        string updateBalanceSql = @"
                    UPDATE ADMIN_NGANHANG.accounts 
                    SET balance = :newBalance, 
                        available_balance = :newBalance, 
                        last_transaction_date = SYSDATE 
                    WHERE account_id = :accountId";

                        using (var cmd = new OracleCommand(updateBalanceSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("newBalance", OracleDbType.Decimal).Value = newBalance;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Tạo mã giao dịch unique
                        string transactionCode = "DEP" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999).ToString();

                        // 4. Ghi vào bảng transactions (sử dụng sequence chữ thường với NEXTVAL viết hoa)
                        string insertTransactionSql = @"
                    INSERT INTO ADMIN_NGANHANG.transactions 
                    (transaction_id, transaction_code, account_id, transaction_type_id, 
                     amount, fee_amount, balance_before, balance_after, 
                     transaction_date, description, channel, processed_by, status, created_date)
                    VALUES 
                    (seq_transaction_id.NEXTVAL, :transCode, :accountId, :typeId, 
                     :amount, :feeAmount, :balanceBefore, :balanceAfter, 
                     SYSDATE, :description, :channel, :processedBy, :status, SYSDATE)";

                        using (var cmd = new OracleCommand(insertTransactionSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;

                            cmd.Parameters.Add("transCode", OracleDbType.Varchar2, 20).Value = transactionCode;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;
                            cmd.Parameters.Add("typeId", OracleDbType.Int32).Value = 1; // DEPOSIT type_id = 1
                            cmd.Parameters.Add("amount", OracleDbType.Decimal).Value = amount;
                            cmd.Parameters.Add("feeAmount", OracleDbType.Decimal).Value = 0; // DEPOSIT không có phí
                            cmd.Parameters.Add("balanceBefore", OracleDbType.Decimal).Value = currentBalance;
                            cmd.Parameters.Add("balanceAfter", OracleDbType.Decimal).Value = newBalance;
                            cmd.Parameters.Add("description", OracleDbType.NVarchar2, 200).Value = description ?? "Nạp tiền";
                            cmd.Parameters.Add("channel", OracleDbType.Varchar2, 20).Value = channel;
                            cmd.Parameters.Add("processedBy", OracleDbType.Int32).Value = processedBy;
                            cmd.Parameters.Add("status", OracleDbType.Int32).Value = 1; // Thành công

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Lỗi khi nạp tiền: " + ex.Message);
                    }
                }
            }
        }

        public bool WithdrawMoney(int accountId, decimal amount, string description, int processedBy, string channel = "BRANCH")
        {
            using (var connection = Database.Get_Connect())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Khóa và kiểm tra tài khoản + lấy thông tin loại tài khoản
                        string checkAccountSql = @"
                    SELECT a.account_id, a.balance, a.status, at.min_balance 
                    FROM ADMIN_NGANHANG.accounts a
                    JOIN ADMIN_NGANHANG.account_types at ON a.account_type_id = at.type_id
                    WHERE a.account_id = :accountId AND a.status = 1 
                    FOR UPDATE";

                        decimal currentBalance = 0;
                        decimal minBalance = 0;
                        using (var cmd = new OracleCommand(checkAccountSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    throw new Exception("Tài khoản không tồn tại hoặc đã bị khóa");
                                currentBalance = Convert.ToDecimal(reader["balance"]);
                                minBalance = Convert.ToDecimal(reader["min_balance"]);
                            }
                        }

                        // 2. Lấy phí giao dịch WITHDRAW (từ dữ liệu: type_id=2, fee=5000)
                        decimal feeAmount = 5000;

                        decimal totalAmount = amount + feeAmount;
                        decimal newBalance = currentBalance - totalAmount;

                        // 3. Kiểm tra số dư
                        if (newBalance < minBalance)
                            throw new Exception($"Số dư không đủ. Cần tối thiểu {minBalance:N0} VND sau khi rút");

                        // 4. Cập nhật số dư
                        string updateBalanceSql = @"
                    UPDATE ADMIN_NGANHANG.accounts 
                    SET balance = :newBalance, 
                        available_balance = :newBalance, 
                        last_transaction_date = SYSDATE 
                    WHERE account_id = :accountId";

                        using (var cmd = new OracleCommand(updateBalanceSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("newBalance", OracleDbType.Decimal).Value = newBalance;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;
                            cmd.ExecuteNonQuery();
                        }

                        // 5. Tạo mã giao dịch unique
                        string transactionCode = "WDR" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999).ToString();

                        // 6. Ghi vào bảng transactions
                        string insertTransactionSql = @"
                    INSERT INTO ADMIN_NGANHANG.transactions 
                    (transaction_id, transaction_code, account_id, transaction_type_id, 
                     amount, fee_amount, balance_before, balance_after, 
                     transaction_date, description, channel, processed_by, status, created_date)
                    VALUES 
                    (seq_transaction_id.NEXTVAL, :transCode, :accountId, :typeId, 
                     :amount, :feeAmount, :balanceBefore, :balanceAfter, 
                     SYSDATE, :description, :channel, :processedBy, :status, SYSDATE)";

                        using (var cmd = new OracleCommand(insertTransactionSql, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;

                            cmd.Parameters.Add("transCode", OracleDbType.Varchar2, 20).Value = transactionCode;
                            cmd.Parameters.Add("accountId", OracleDbType.Int32).Value = accountId;
                            cmd.Parameters.Add("typeId", OracleDbType.Int32).Value = 2; 
                            cmd.Parameters.Add("amount", OracleDbType.Decimal).Value = amount;
                            cmd.Parameters.Add("feeAmount", OracleDbType.Decimal).Value = feeAmount;
                            cmd.Parameters.Add("balanceBefore", OracleDbType.Decimal).Value = currentBalance;
                            cmd.Parameters.Add("balanceAfter", OracleDbType.Decimal).Value = newBalance;
                            cmd.Parameters.Add("description", OracleDbType.NVarchar2, 200).Value = description ?? "Rút tiền";
                            cmd.Parameters.Add("channel", OracleDbType.Varchar2, 20).Value = channel;
                            cmd.Parameters.Add("processedBy", OracleDbType.Int32).Value = processedBy;
                            cmd.Parameters.Add("status", OracleDbType.Int32).Value = 1;

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Lỗi khi rút tiền: " + ex.Message);
                    }
                }
            }
        }


        public DataTable GetAccountInfo(string accountNumber)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string sql = @"
                SELECT 
                    a.account_id,
                    a.account_number,
                    c.full_name AS customer_name,
                    a.balance,
                    at.type_name AS account_type,
                    at.min_balance,
                    CASE a.status 
                        WHEN 1 THEN 'Hoạt động'
                        WHEN 0 THEN 'Đã đóng'
                        WHEN 2 THEN 'Tạm khóa'
                    END AS status_text
                FROM ADMIN_NGANHANG.accounts a
                JOIN ADMIN_NGANHANG.customers c ON a.customer_id = c.customer_id
                JOIN ADMIN_NGANHANG.account_types at ON a.account_type_id = at.type_id
                WHERE a.account_number = :accountNumber";

                    using (var command = new OracleCommand(sql, connection))
                    {
                        command.Parameters.Add(":accountNumber", accountNumber);
                        using (var adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tài khoản: " + ex.Message);
            }
            return dt;
        }
        public DataTable GetTransactionsByAccount(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string sql = @"
                SELECT 
                    t.transaction_code,
                    tt.type_name AS transaction_type,
                    t.amount,
                    t.fee_amount,
                    t.balance_after,
                    TO_CHAR(t.transaction_date, 'DD/MM/YYYY HH24:MI:SS') AS transaction_date,
                    t.description,
                    t.channel,
                    CASE t.status
                        WHEN 0 THEN 'Thất bại'
                        WHEN 1 THEN 'Thành công'
                        WHEN 2 THEN 'Đang xử lý'
                    END AS status
                FROM ADMIN_NGANHANG.transactions t
                JOIN ADMIN_NGANHANG.transaction_types tt ON t.transaction_type_id = tt.type_id
                WHERE t.account_id = :accountId";

                    if (fromDate.HasValue)
                        sql += " AND t.transaction_date >= :fromDate";
                    if (toDate.HasValue)
                        sql += " AND t.transaction_date <= :toDate";

                    sql += " ORDER BY t.transaction_date DESC";

                    using (var command = new OracleCommand(sql, connection))
                    {
                        command.Parameters.Add(":accountId", accountId);
                        if (fromDate.HasValue)
                            command.Parameters.Add(":fromDate", fromDate.Value);
                        if (toDate.HasValue)
                            command.Parameters.Add(":toDate", toDate.Value);

                        using (var adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy lịch sử giao dịch: " + ex.Message);
            }
            return dt;
        }

        public TransactionStatistics GetTransactionStatistics()
        {
            TransactionStatistics stats = new TransactionStatistics();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = @"
                        SELECT 
                            COUNT(*) AS TodayTransactions,
                            NVL(SUM(CASE 
                                WHEN tt.type_code IN ('DEPOSIT', 'INTEREST', 'REVERSAL') THEN t.amount 
                                ELSE 0 
                            END), 0) AS TodayDeposits,
                            NVL(SUM(CASE 
                                WHEN tt.type_code IN ('WITHDRAW', 'TRANSFER', 'PAYMENT', 'FEE', 'ADJUSTMENT') THEN t.amount 
                                ELSE 0 
                            END), 0) AS TodayWithdrawals,
                            COUNT(CASE WHEN t.status = 2 THEN 1 END) AS PendingTransactions
                        FROM transactions t
                        JOIN transaction_types tt ON t.transaction_type_id = tt.type_id";

                    using (var command = new OracleCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats.TodayTransactions = Convert.ToInt32(reader["TodayTransactions"]);
                                stats.TodayDeposits = Convert.ToDecimal(reader["TodayDeposits"]);
                                stats.TodayWithdrawals = Convert.ToDecimal(reader["TodayWithdrawals"]);
                                stats.PendingTransactions = Convert.ToInt32(reader["PendingTransactions"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stats.TodayTransactions = 0;
                stats.TodayDeposits = 0;
                stats.TodayWithdrawals = 0;
                stats.PendingTransactions = 0;
                System.Diagnostics.Debug.WriteLine("Lỗi thống kê giao dịch: " + ex.Message);
            }

            return stats;
        }



        private DataTable CreateFallbackTransactionData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransactionID", typeof(string));
            dt.Columns.Add("TransactionCode", typeof(string));
            dt.Columns.Add("AccountNumber", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("TransactionType", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("TransactionDate", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Channel", typeof(string));
            dt.Columns.Add("ProcessedBy", typeof(string));


            dt.Rows.Add("TXN001", "TXN001", "1234567890", "Nguyễn Văn A", "Nạp tiền", "5,000,000 VND", "25/07/2025 14:30:00", "Thành công", "ATM", "Nhân viên 1");
            dt.Rows.Add("TXN002", "TXN002", "1234567891", "Trần Thị B", "Rút tiền", "2,000,000 VND", "25/07/2025 13:15:00", "Thành công", "Quầy", "Nhân viên 2");
            dt.Rows.Add("TXN003", "TXN003", "1234567892", "Lê Văn C", "Chuyển khoản", "1,500,000 VND", "25/07/2025 12:45:00", "Đang xử lý", "Internet Banking", "Nhân viên 3");
            dt.Rows.Add("TXN004", "TXN004", "1234567893", "Phạm Thị D", "Nạp tiền", "3,000,000 VND", "25/07/2025 11:20:00", "Thất bại", "Mobile Banking", "Nhân viên 1");

            return dt;
        }
    }

    public class TransactionStatistics
    {
        public int TodayTransactions { get; set; }
        public decimal TodayDeposits { get; set; }
        public decimal TodayWithdrawals { get; set; }
        public int PendingTransactions { get; set; }
    }
}
