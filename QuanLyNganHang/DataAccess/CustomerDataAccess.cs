using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.Helpers;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class CustomerDataAccess
    {
        public (int Total, int Vip, int Normal, int Locked) GetCustomerStatistics()
        {
            int total = 0, vip = 0, normal = 0, locked = 0;

            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = @"
                    SELECT 
                        COUNT(*) AS TOTAL,
                        COUNT(CASE WHEN status = 1 THEN 1 END) AS ACTIVE,
                        COUNT(CASE WHEN status = 0 THEN 1 END) AS LOCKED
                    FROM ADMIN_NGANHANG.customers";

                using (var cmd = new OracleCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        total = Convert.ToInt32(reader["TOTAL"]);
                        int active = Convert.ToInt32(reader["ACTIVE"]);
                        locked = Convert.ToInt32(reader["LOCKED"]);
                        vip = (int)(active * 0.075);
                        normal = active - vip;
                    }
                }
            }

            return (total, vip, normal, locked);
        }

        public DataTable GetAllCustomers()
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
                            customer_code AS ""Mã KH"",
                            full_name AS ""Họ tên"",
                            id_number AS ""CMND"",
                            phone AS ""Điện thoại"",
                            email AS ""Email"",
                            address AS ""Địa chỉ"",
                            CASE
                                WHEN status = 1 THEN 'Hoạt động'
                                ELSE 'Khóa'
                            END AS ""Trạng thái""
                        FROM ADMIN_NGANHANG.customers
                        WHERE status = 1";

                    using (var cmd = new OracleCommand(sql, conn))
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
            }

            return dt;
        }

        public DataTable SearchCustomers(string keyword)
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
                            customer_code AS ""Mã KH"",
                            full_name AS ""Họ tên"",
                            id_number AS ""CMND"",
                            phone AS ""Điện thoại"",
                            email AS ""Email"",
                            address AS ""Địa chỉ"",
                            CASE
                                WHEN status = 1 THEN 'Hoạt động'
                                ELSE 'Khóa'
                            END AS ""Trạng thái""
                        FROM ADMIN_NGANHANG.customers
                        WHERE status = 1 AND (
                            LOWER(full_name) LIKE :kw OR
                            id_number LIKE :kw OR
                            phone LIKE :kw
                        )";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":kw", $"%{keyword.ToLower()}%");
                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm khách hàng: {ex.Message}");
            }

            return dt;
        }

        public bool LockCustomer(string customerCode)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "UPDATE ADMIN_NGANHANG.customers SET status = 0 WHERE customer_code = :code";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":code", customerCode);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa khách hàng: {ex.Message}");
            }
        }
        public DataRow GetCustomerByCode(string code)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = "SELECT * FROM ADMIN_NGANHANG.customers WHERE customer_code = :code";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":code", code);
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }
        public bool SaveCustomer(bool isEdit, string code, string fullName, string idCard, string phone, string email, string address, int status)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.BindByName = true;

                if (isEdit)
                {
                    cmd.CommandText = @"
                UPDATE ADMIN_NGANHANG.customers
                SET full_name = :full_name,
                    id_number = :id_number,
                    phone = :phone,
                    email = :email,
                    address = :address,
                    status = :status
                WHERE customer_code = :customer_code";
                }
                else
                {
                    cmd.CommandText = @"
                INSERT INTO ADMIN_NGANHANG.customers
                (customer_id, customer_id_varchar, customer_code, full_name, id_number, phone, email, address, status)
                VALUES (SEQ_CUSTOMER_ID.NEXTVAL, :customer_id_varchar, :customer_code, :full_name, :id_number, :phone, :email, :address, :status)";
                    cmd.Parameters.Add("customer_id_varchar", code);
                }

                cmd.Parameters.Add("customer_code", code);
                cmd.Parameters.Add("full_name", fullName);

                var encryptedId = EncryptionHelper.EncryptRSA(idCard);
                if (encryptedId.Length > 256) throw new Exception("ID mã hóa quá dài.");

                cmd.Parameters.Add("id_number", encryptedId);
                cmd.Parameters.Add("phone", EncryptionHelper.EncryptRSA(phone));
                cmd.Parameters.Add("email", EncryptionHelper.EncryptRSA(email));
                cmd.Parameters.Add("address", EncryptionHelper.EncryptRSA(address));
                cmd.Parameters.Add("status", status);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public DataRow FindCustomerByEncryptedInput(string encryptedInput)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = "SELECT * FROM ADMIN_NGANHANG.customers WHERE id_number = :id OR phone = :phone";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":id", encryptedInput);
                    cmd.Parameters.Add(":phone", encryptedInput);

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }

        public bool HasActiveAccount(string customerId)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = "SELECT COUNT(*) FROM ADMIN_NGANHANG.accounts WHERE customer_id = :cid AND status = 1";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":cid", customerId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

    }
}
