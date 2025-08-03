using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class AccountDataAccess
    {
        public DataTable GetAccountStatistics()
        {
            DataTable dt = new DataTable();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string sql = @"SELECT status, COUNT(*) AS count FROM ADMIN_NGANHANG.accounts GROUP BY status";

                    using (var command = new OracleCommand(sql, connection))
                    using (var adapter = new OracleDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thống kê tài khoản: " + ex.Message);
            }

            return dt;
        }

        public DataTable GetAccounts(int? status = null)
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
                            A.account_number,
                            C.full_name AS customer_name,
                            CASE A.account_type_id
                                WHEN 1 THEN 'Thanh toán'
                                WHEN 2 THEN 'Tiết kiệm'
                                ELSE 'Khác'
                            END AS account_type,
                            A.balance,
                            CASE A.status
                                WHEN 1 THEN 'Hoạt động'
                                WHEN 0 THEN 'Đã đóng'
                                WHEN -1 THEN 'Đóng băng'
                                ELSE 'Không rõ'
                            END AS status_text,
                            TO_CHAR(A.opened_date, 'DD-MM-YYYY HH24:MI') AS opened_date
                        FROM ADMIN_NGANHANG.accounts A
                        JOIN ADMIN_NGANHANG.customers C ON A.customer_id = C.customer_id";

                    if (status != null)
                        sql += " WHERE A.status = :status";

                    sql += " ORDER BY A.opened_date DESC";

                    using (var command = new OracleCommand(sql, connection))
                    {
                        if (status != null)
                            command.Parameters.Add(":status", status);

                        using (var adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản: " + ex.Message);
            }

            return dt;
        }
        public DataTable FindActiveAccountsByEncryptedInput(string encryptedInput)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = @"
                    SELECT 
                        a.account_id,
                        a.account_number AS ""Số tài khoản"",
                        a.balance AS ""Số dư"",
                        c.full_name AS ""Tên khách"",
                        c.id_number AS ""CMND/CCCD"",
                        c.phone AS ""SĐT"",
                        c.email AS ""Gmail"",
                        c.address AS ""Địa chỉ""
                    FROM ADMIN_NGANHANG.accounts a
                    JOIN ADMIN_NGANHANG.customers c ON a.customer_id = c.customer_id
                    WHERE (c.id_number = :id OR c.phone = :phone) AND a.status = 1";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":id", encryptedInput);
                        cmd.Parameters.Add(":phone", encryptedInput);

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tìm kiếm tài khoản: " + ex.Message);
            }

            return dt;
        }

        public bool FreezeAccountById(int accountId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "UPDATE ADMIN_NGANHANG.accounts SET status = -1 WHERE account_id = :id";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":id", accountId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đóng băng tài khoản: " + ex.Message);
            }
        }
        public DataTable GetFrozenAccounts()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = @"
                SELECT a.account_id, 
                       a.account_number, 
                       c.full_name AS customer_name, 
                       c.phone, 
                       c.email, 
                       c.address, 
                       a.balance
                FROM ADMIN_NGANHANG.accounts a
                JOIN ADMIN_NGANHANG.customers c ON a.customer_id = c.customer_id
                WHERE a.status = -1";

                    using (var adapter = new OracleDataAdapter(sql, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản đóng băng: " + ex.Message);
            }

            return dt;
        }

        public bool ActivateAccount(int accountId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "UPDATE ADMIN_NGANHANG.accounts SET status = 1 WHERE account_id = :accountId";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":accountId", accountId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kích hoạt tài khoản: " + ex.Message);
            }
        }
        public DataTable GetActiveAccounts()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = @"
                        SELECT 
                            A.account_id,
                            A.account_number,
                            C.full_name AS customer_name,
                            C.phone,
                            C.email,
                            A.balance
                        FROM ADMIN_NGANHANG.accounts A
                        JOIN ADMIN_NGANHANG.customers C ON A.customer_id = C.customer_id
                        WHERE A.status = 1";

                    using (var adapter = new OracleDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản hoạt động: " + ex.Message);
            }
        }

        public bool CloseAccount(int accountId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "UPDATE ADMIN_NGANHANG.accounts SET status = 0 WHERE account_id = :id";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":id", accountId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đóng tài khoản: " + ex.Message);
            }
        }
        public bool CreateAccount(string customerId, int accountTypeId, decimal balance)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string accNum = "ACCT" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    string sql = @"
                INSERT INTO ADMIN_NGANHANG.ACCOUNTS
                (ACCOUNT_ID, ACCOUNT_NUMBER, CUSTOMER_ID, ACCOUNT_TYPE_ID, BALANCE, AVAILABLE_BALANCE, STATUS, OPENED_DATE, BRANCH_ID, PIN_HASH, CREATED_BY)
                VALUES
                (SEQ_ACCOUNT_ID.NEXTVAL, :num, :cid, :type, :bal, :bal, 1, SYSDATE, 1, 'DEFAULT_HASH', 3)";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":num", accNum);
                        cmd.Parameters.Add(":cid", customerId);
                        cmd.Parameters.Add(":type", accountTypeId);
                        cmd.Parameters.Add(":bal", balance);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo tài khoản: " + ex.Message);
            }
        }
        public bool CustomerHasActiveAccount(string customerId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "SELECT COUNT(*) FROM ADMIN_NGANHANG.accounts WHERE customer_id = :cid AND status = 1";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":cid", customerId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tài khoản hoạt động: " + ex.Message);
            }
        }

    }
}
