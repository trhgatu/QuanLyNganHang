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
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new OracleCommand("ADMIN_NGANHANG.pkg_user_management.pro_get_user_by_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;

                    var refCursor = new OracleParameter("Result", OracleDbType.RefCursor)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(refCursor);

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
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new OracleCommand("ADMIN_NGANHANG.pkg_user_management.pro_update_user", conn))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;
                            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                            cmd.Parameters.Add("p_password_hash", OracleDbType.Varchar2).Value = HashHelper.HashPassword(password);
                            cmd.Parameters.Add("p_oracle_user", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(oracleUser) ? DBNull.Value : (object)oracleUser;
                            cmd.Parameters.Add("p_full_name", OracleDbType.Varchar2).Value = fullName;
                            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                            cmd.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = phone;
                            cmd.Parameters.Add("p_position", OracleDbType.Varchar2).Value = position;
                            cmd.Parameters.Add("p_branch_id", OracleDbType.Int32).Value = branchId;
                            cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;

                            var p_success = new OracleParameter("p_success", OracleDbType.Int32)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(p_success);

                            var p_error_msg = new OracleParameter("p_error_msg", OracleDbType.Varchar2, 4000)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(p_error_msg);

                            cmd.ExecuteNonQuery();

                            int success = (p_success.Value == DBNull.Value) ? 0 : Convert.ToInt32(((OracleDecimal)p_success.Value).ToInt32());
                            string errMsg = p_error_msg.Value?.ToString();

                            if (success == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                MessageBox.Show("Lỗi khi cập nhật user: " + errMsg);
                                return false;
                            }
                        }
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
    }

    public class UserStatistics
    {
        public int TotalAdmin { get; set; }
        public int TotalEmployee { get; set; }
        public int ActiveUsers { get; set; }
        public int LockedUsers { get; set; }
    }
}
