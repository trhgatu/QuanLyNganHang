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
            dt.Columns.Add("Mã KH");
            dt.Columns.Add("Họ tên");
            dt.Columns.Add("CMND");
            dt.Columns.Add("Điện thoại");
            dt.Columns.Add("Email");
            dt.Columns.Add("Địa chỉ");
            dt.Columns.Add("Trạng thái");

            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "SELECT * FROM ADMIN_NGANHANG.customers WHERE status = 1";

                    using (var cmd = new OracleCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string code = reader["customer_code"].ToString();
                            string name = reader["full_name"].ToString();

                            string idEncrypted = reader["id_number"].ToString();
                            string phoneEncrypted = reader["phone"].ToString();
                            string emailEncrypted = reader["email"].ToString();
                            string addressEncrypted = reader["address"].ToString();

                            string status = Convert.ToInt32(reader["status"]) == 1 ? "Hoat dong" : "Khoa";

                            string idNumber = EncryptionHelper.TryDecryptHybrid(idEncrypted);
                            string phone = EncryptionHelper.TryDecryptHybrid(phoneEncrypted);
                            string email = EncryptionHelper.TryDecryptHybrid(emailEncrypted);
                            string address = EncryptionHelper.TryDecryptHybrid(addressEncrypted);

                            dt.Rows.Add(code, name, idNumber, phone, email, address, status);
                        }
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
            DataTable dt = GetAllCustomers();
            string filter = string.Format("[Họ tên] LIKE '%{0}%' OR [CMND] LIKE '%{0}%' OR [Điện thoại] LIKE '%{0}%'", keyword);

            DataRow[] rows = dt.Select(filter);
            DataTable filtered = dt.Clone();

            foreach (var row in rows)
            {
                filtered.ImportRow(row);
            }

            return filtered;
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

        public bool DeleteCustomer(string customerCode)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string sql = "DELETE FROM ADMIN_NGANHANG.customers WHERE customer_code = :code";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(":code", customerCode);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khách hàng: {ex.Message}");
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

                using (var cmd = new OracleCommand())
                {
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
                            (customer_id, customer_code, full_name, id_number, phone, email, address, status)
                            VALUES (SEQ_CUSTOMER_ID.NEXTVAL, :customer_code, :full_name, :id_number, :phone, :email, :address, :status)";
                    }

                    cmd.Parameters.Add("customer_code", code);
                    cmd.Parameters.Add("full_name", fullName);
                    cmd.Parameters.Add("id_number", idCard);
                    cmd.Parameters.Add("phone", phone);
                    cmd.Parameters.Add("email", email);
                    cmd.Parameters.Add("address", address);
                    cmd.Parameters.Add("status", status);

                    return cmd.ExecuteNonQuery() > 0;
                }
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
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }

        public DataRow FindCustomerByInputWithDecryption(string plainInput)
        {
            try
            {
                DataTable rawCustomers = GetRawCustomers();
                foreach (DataRow row in rawCustomers.Rows)
                {
                    string idEncrypted = row["id_number"].ToString();
                    string phoneEncrypted = row["phone"].ToString();

                    string idDecrypted = EncryptionHelper.TryDecryptHybrid(idEncrypted);
                    string phoneDecrypted = EncryptionHelper.TryDecryptHybrid(phoneEncrypted);

                    if (idDecrypted == plainInput || phoneDecrypted == plainInput)
                    {
                        return row;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm khách hàng theo mã hóa hybrid: " + ex.Message);
            }

            return null;
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

        public DataTable GetRawCustomers()
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = "SELECT customer_id, full_name, id_number, phone FROM ADMIN_NGANHANG.customers WHERE status = 1";
                using (var cmd = new OracleCommand(sql, conn))
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
