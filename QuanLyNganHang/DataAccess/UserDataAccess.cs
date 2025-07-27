using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class UserDataAccess
    {  
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            try
            {
                var connection = Database.Get_Connect();

                if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = @"
                        SELECT 
                            e.employee_id as ID,
                            su.username as Username,
                            e.full_name as FullName,
                           
                            CASE 
                                WHEN su.status = 1 THEN 'Hoạt động'
                                WHEN su.status = 0 THEN 'Bị khóa'
                                ELSE 'Không xác định'
                            END as Status,
                            TO_CHAR(su.last_login, 'DD/MM/YYYY HH24:MI:SS') as LastLogin,
                            e.email,
                            e.phone,
                            b.branch_name as Branch
                        FROM 
                            employees e
                        LEFT JOIN 
                            system_users su ON e.employee_id = su.employee_id
                        LEFT JOIN 
                            employee_roles er ON e.employee_id = er.employee_id
                        LEFT JOIN 
                            roles r ON er.role_id = r.role_id
                        LEFT JOIN
                            branches b ON e.branch_id = b.branch_id
                        WHERE 
                            su.username IS NOT NULL
                        GROUP BY 
                            e.employee_id, su.username, e.full_name, su.status, 
                            su.last_login, e.email, e.phone, b.branch_name
                        ORDER BY 
                            e.employee_id";

                using (var command = new OracleCommand(query, connection))
                using (var adapter = new OracleDataAdapter(command))
                {
                    adapter.Fill(dt);

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu người dùng: {ex.Message}");
            }

            return dt;
        }

        public UserStatistics GetUserStatistics()
        {
            UserStatistics stats = new UserStatistics();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = @"
                        SELECT 
                            COUNT(CASE WHEN r.role_code IN ('SUPER_ADMIN', 'ADMIN') THEN 1 END) as TotalAdmin,
                            COUNT(CASE WHEN r.role_code NOT IN ('SUPER_ADMIN', 'ADMIN') THEN 1 END) as TotalEmployee,
                            COUNT(CASE WHEN su.status = 1 THEN 1 END) as ActiveUsers,
                            COUNT(CASE WHEN su.status = 0 THEN 1 END) as LockedUsers
                        FROM 
                            employees e
                        JOIN 
                            system_users su ON e.employee_id = su.employee_id
                        LEFT JOIN 
                            employee_roles er ON e.employee_id = er.employee_id
                        LEFT JOIN 
                            roles r ON er.role_id = r.role_id";

                    using (var command = new OracleCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats.TotalAdmin = Convert.ToInt32(reader["TotalAdmin"]);
                                stats.TotalEmployee = Convert.ToInt32(reader["TotalEmployee"]);
                                stats.ActiveUsers = Convert.ToInt32(reader["ActiveUsers"]);
                                stats.LockedUsers = Convert.ToInt32(reader["LockedUsers"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê người dùng: {ex.Message}");
            }

            return stats;
        }

        public bool CreateUser(string username, string password, int employeeId, string profileName = "DEFAULT")
        {
            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string insertUserQuery = @"
                                INSERT INTO system_users (user_id, username, password_hash, employee_id, status, created_date)
                                VALUES (seq_employee_id.NEXTVAL, :username, :password_hash, :employee_id, 1, SYSDATE)";

                            using (var command = new OracleCommand(insertUserQuery, connection))
                            {
                                command.Transaction = transaction;
                                command.Parameters.Add(":username", username);
                                command.Parameters.Add(":password_hash", HashPassword(password));
                                command.Parameters.Add(":employee_id", employeeId);
                                command.ExecuteNonQuery();
                            }
                            if (!string.IsNullOrEmpty(profileName))
                            {
                                string createOracleUserQuery = $"BEGIN pro_create_user('{username}', '{password}', '{profileName}'); END;";
                                using (var command = new OracleCommand(createOracleUserQuery, connection))
                                {
                                    command.Transaction = transaction;
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo người dùng: {ex.Message}");
            }
        }

        public DataTable GetUserPermissions(int userId)
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
                            '1' as ID,
                            'admin' as Username,
                            'Administrator' as FullName,
                            'Admin' as Role,
                            'Hoạt động' as Status,
                            TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS') as LastLogin
                        FROM DUAL
                        UNION ALL
                        SELECT 
                            '2' as ID,
                            'employee1' as Username,
                            'Nhân viên 1' as FullName,
                            'Employee' as Role,
                            'Hoạt động' as Status,
                            TO_CHAR(SYSDATE-1, 'DD/MM/YYYY HH24:MI:SS') as LastLogin
                        FROM DUAL
                        UNION ALL
                        SELECT 
                            '3' as ID,
                            'manager1' as Username,
                            'Quản lý 1' as FullName,
                            'Manager' as Role,
                            'Bị khóa' as Status,
                            TO_CHAR(SYSDATE-2, 'DD/MM/YYYY HH24:MI:SS') as LastLogin
                        FROM DUAL";


                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(":user_id", userId);
                        using (var adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy quyền người dùng: {ex.Message}");
            }

            return dt;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public class UserStatistics
    {
        public int TotalAdmin { get; set; }
        public int TotalEmployee { get; set; }
        public int ActiveUsers { get; set; }
        public int LockedUsers { get; set; }
    }
}
