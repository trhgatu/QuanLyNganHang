using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (var cmd = new OracleCommand("ADMIN_NGANHANG.pkg_user_management.pro_get_all_users", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var refCursor = new OracleParameter("Result", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(refCursor);

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
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

                    using (var cmd = new OracleCommand("ADMIN_NGANHANG.pkg_user_management.pro_get_user_statistics", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_total_admin", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_total_employee", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_active_users", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_locked_users", OracleDbType.Int32).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        stats.TotalAdmin = Convert.ToInt32(cmd.Parameters["p_total_admin"].Value.ToString());
                        stats.TotalEmployee = Convert.ToInt32(cmd.Parameters["p_total_employee"].Value.ToString());
                        stats.ActiveUsers = Convert.ToInt32(cmd.Parameters["p_active_users"].Value.ToString());
                        stats.LockedUsers = Convert.ToInt32(cmd.Parameters["p_locked_users"].Value.ToString());
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
            try
            {
                string sql = @"
            SELECT 
                e.employee_id,
                e.full_name,
                e.email,
                e.phone,
                e.position,
                e.branch_id,
                su.username,
                su.username as oracle_user,
                er.role_id,
                r.role_name,
                b.branch_name,
                b.branch_code
            FROM ADMIN_NGANHANG.employees e
            LEFT JOIN ADMIN_NGANHANG.system_users su ON e.employee_id = su.employee_id
            LEFT JOIN ADMIN_NGANHANG.employee_roles er ON e.employee_id = er.employee_id
            LEFT JOIN ADMIN_NGANHANG.roles r ON er.role_id = r.role_id
            LEFT JOIN ADMIN_NGANHANG.branches b ON e.branch_id = b.branch_id
            WHERE e.employee_id = :employeeId";

                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin user: " + ex.Message);
            }
        }

        public bool Pro_DropUserById(int employeeId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (OracleCommand cmd = new OracleCommand("ADMIN_NGANHANG.pkg_user_management.pro_drop_user_by_id", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi xóa người dùng theo ID:\n" + ex.Message);
                return false;
            }
        }
        public bool UpdateUserInfo(int employeeId, string username, string password, string oracleUser, string fullName, string email, string phone, string position, int branchId, int roleId)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Cập nhật thông tin employee
                        string updateEmployeeSql = @"
                    UPDATE ADMIN_NGANHANG.employees 
                    SET full_name = :fullName,
                        email = :email,
                        phone = :phone,
                        position = :position,
                        branch_id = :branchId
                    WHERE employee_id = :employeeId";

                        using (var cmd = new OracleCommand(updateEmployeeSql, conn))
                        {
                            cmd.Transaction = tran;
                            cmd.BindByName = true;

                            cmd.Parameters.Add("fullName", OracleDbType.NVarchar2).Value = fullName;
                            cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email ?? (object)DBNull.Value;
                            cmd.Parameters.Add("phone", OracleDbType.Varchar2).Value = phone ?? (object)DBNull.Value;
                            cmd.Parameters.Add("position", OracleDbType.NVarchar2).Value = position ?? (object)DBNull.Value;
                            cmd.Parameters.Add("branchId", OracleDbType.Int32).Value = branchId;
                            cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                            cmd.ExecuteNonQuery();
                        }

                        // 2. Cập nhật system_users (nếu có thay đổi mật khẩu)
                        if (!string.IsNullOrEmpty(password))
                        {
                            string updateUserSql = @"
                        UPDATE ADMIN_NGANHANG.system_users 
                        SET password_hash = :passwordHash
                        WHERE employee_id = :employeeId";

                            using (var cmd = new OracleCommand(updateUserSql, conn))
                            {
                                cmd.Transaction = tran;
                                cmd.BindByName = true;

                                cmd.Parameters.Add("passwordHash", OracleDbType.Varchar2).Value = HashHelper.HashPassword(password);
                                cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 3. Cập nhật oracle user (nếu có)
                        if (!string.IsNullOrEmpty(oracleUser))
                        {
                            string updateOracleUserSql = @"
                        UPDATE ADMIN_NGANHANG.system_users 
                        SET username = :oracleUser
                        WHERE employee_id = :employeeId";

                            using (var cmd = new OracleCommand(updateOracleUserSql, conn))
                            {
                                cmd.Transaction = tran;
                                cmd.BindByName = true;

                                cmd.Parameters.Add("oracleUser", OracleDbType.Varchar2).Value = oracleUser;
                                cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 4. Cập nhật role - xóa role cũ và thêm role mới
                        string deleteRoleSql = "DELETE FROM ADMIN_NGANHANG.employee_roles WHERE employee_id = :employeeId";
                        using (var cmd = new OracleCommand(deleteRoleSql, conn))
                        {
                            cmd.Transaction = tran;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;
                            cmd.ExecuteNonQuery();
                        }

                        string insertRoleSql = @"
                    INSERT INTO ADMIN_NGANHANG.employee_roles (employee_id, role_id, assigned_date)
                    VALUES (:employeeId, :roleId, SYSDATE)";

                        using (var cmd = new OracleCommand(insertRoleSql, conn))
                        {
                            cmd.Transaction = tran;
                            cmd.BindByName = true;
                            cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;
                            cmd.Parameters.Add("roleId", OracleDbType.Int32).Value = roleId;
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Lỗi khi cập nhật thông tin người dùng: " + ex.Message);
                    }
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
