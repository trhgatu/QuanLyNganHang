using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

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
            WHEN su.username IS NULL THEN 'Chưa có tài khoản'
            WHEN su.status = 1 THEN 'Hoạt động'
            WHEN su.status = 0 THEN 'Bị khóa'
            ELSE 'Không xác định'
        END as Status,
        e.email,
        e.phone,
        b.branch_name as Branch,
        e.oracle_user AS OracleUser,
        r.role_name AS Role
    FROM ADMIN_NGANHANG.employees e
    LEFT JOIN ADMIN_NGANHANG.system_users su ON e.employee_id = su.employee_id
    LEFT JOIN ADMIN_NGANHANG.employee_roles er ON e.employee_id = er.employee_id
    LEFT JOIN ADMIN_NGANHANG.roles r ON er.role_id = r.role_id
    LEFT JOIN ADMIN_NGANHANG.branches b ON e.branch_id = b.branch_id
    GROUP BY 
        e.employee_id, su.username, e.full_name, su.status, 
        su.last_login, e.email, e.phone, b.branch_name, e.oracle_user, r.role_name
    ORDER BY e.employee_id
";


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
        COUNT(CASE WHEN r.role_code IN ('SUPER_ADMIN', 'ADMIN') THEN 1 END) AS TotalAdmin,
        COUNT(CASE WHEN r.role_code NOT IN ('SUPER_ADMIN', 'ADMIN') THEN 1 END) AS TotalEmployee,
        COUNT(CASE WHEN su.status = 1 THEN 1 END) AS ActiveUsers,
        COUNT(CASE WHEN su.status = 0 THEN 1 END) AS LockedUsers
    FROM 
        ADMIN_NGANHANG.employees e
    JOIN 
        ADMIN_NGANHANG.system_users su ON e.employee_id = su.employee_id
    LEFT JOIN 
        ADMIN_NGANHANG.employee_roles er ON e.employee_id = er.employee_id
    LEFT JOIN 
        ADMIN_NGANHANG.roles r ON er.role_id = r.role_id";


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

        public DataRow GetUserById(int employeeId)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string query = @"
            SELECT 
                e.full_name, e.email, e.phone, e.position,
                e.oracle_user, e.branch_id,
                su.username, su.password_hash,
                er.role_id
            FROM employees e
            LEFT JOIN system_users su ON e.employee_id = su.employee_id
            LEFT JOIN employee_roles er ON e.employee_id = er.employee_id
            WHERE e.employee_id = :empId";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("empId", employeeId);

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }

        public bool UpdateUserInfo(int employeeId, string username, string password, string oracleUser, string fullName, string email, string phone, string position, int branchId, int roleId)
        {
            using (var conn = Database.Get_Connect())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    string updateEmp = @"UPDATE ADMIN_NGANHANG.employees SET
                                    full_name = :fullName,
                                    email = :email,
                                    phone = :phone,
                                    position = :position,
                                    branch_id = :branchId,
                                    oracle_user = :oracleUser
                                 WHERE employee_id = :employeeId";

                    var cmdEmp = new OracleCommand(updateEmp, conn);
                    cmdEmp.Transaction = tran;
                    cmdEmp.Parameters.Add("fullName", fullName);
                    cmdEmp.Parameters.Add("email", email);
                    cmdEmp.Parameters.Add("phone", phone);
                    cmdEmp.Parameters.Add("position", position);
                    cmdEmp.Parameters.Add("branchId", branchId);
                    cmdEmp.Parameters.Add("oracleUser", string.IsNullOrEmpty(oracleUser) ? DBNull.Value : (object)oracleUser);
                    cmdEmp.Parameters.Add("employeeId", employeeId);
                    cmdEmp.ExecuteNonQuery();

                    string checkUser = "SELECT COUNT(*) FROM ADMIN_NGANHANG.system_users WHERE employee_id = :employeeId";
                    var cmdCheck = new OracleCommand(checkUser, conn);
                    cmdCheck.Transaction = tran;
                    cmdCheck.Parameters.Add("employeeId", employeeId);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (count > 0)
                    {
                        string updateUser = @"UPDATE ADMIN_NGANHANG.system_users SET username = :username
                                      {0}
                                      WHERE employee_id = :employeeId";

                        string passwordClause = "";
                        if (!string.IsNullOrEmpty(password))
                        {
                            passwordClause = ", password_hash = :passwordHash";
                        }

                        updateUser = string.Format(updateUser, passwordClause);
                        var cmdUser = new OracleCommand(updateUser, conn);
                        cmdUser.Transaction = tran;
                        cmdUser.Parameters.Add("username", username);
                        if (!string.IsNullOrEmpty(password))
                            cmdUser.Parameters.Add("passwordHash", HashHelper.HashPassword(password));
                        cmdUser.Parameters.Add("employeeId", employeeId);
                        cmdUser.ExecuteNonQuery();
                    }
                    else
                    {
                        string insertUser = @"INSERT INTO ADMIN_NGANHANG.system_users (user_id, username, password_hash, employee_id, status, created_date)
                                      VALUES (seq_system_user_id.NEXTVAL, :username, :passwordHash, :employeeId, 1, SYSDATE)";
                        var cmdInsert = new OracleCommand(insertUser, conn);
                        cmdInsert.Transaction = tran;
                        cmdInsert.Parameters.Add("username", username);
                        cmdInsert.Parameters.Add("passwordHash", HashHelper.HashPassword(password));
                        cmdInsert.Parameters.Add("employeeId", employeeId);
                        cmdInsert.ExecuteNonQuery();
                    }

                    var cmdDeleteRole = new OracleCommand("DELETE FROM ADMIN_NGANHANG.employee_roles WHERE employee_id = :eid", conn);
                    cmdDeleteRole.Transaction = tran;
                    cmdDeleteRole.Parameters.Add("eid", employeeId);
                    cmdDeleteRole.ExecuteNonQuery();

                    var cmdInsertRole = new OracleCommand("INSERT INTO ADMIN_NGANHANG.employee_roles (employee_id, role_id) VALUES (:eid, :rid)", conn);
                    cmdInsertRole.Transaction = tran;
                    cmdInsertRole.Parameters.Add("eid", employeeId);
                    cmdInsertRole.Parameters.Add("rid", roleId);
                    cmdInsertRole.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                    return false;
                }
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
