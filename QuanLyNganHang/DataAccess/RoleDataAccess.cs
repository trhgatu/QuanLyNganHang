using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class RoleDataAccess
    {
        public DataTable GetAllRoles()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string sql = "SELECT role_id, role_name FROM ADMIN_NGANHANG.roles WHERE status = 1";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text; 

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi lấy danh sách vai trò: " + ex.Message);
            }
            return new DataTable(); 
        }

        public bool AssignRoleToEmployee(int employeeId, int roleId)
        {
            try
            {
                using (var connection = Database.Get_Connect())
                {
                    string query = "INSERT INTO ADMIN_NGANHANG.employee_roles (employee_id, role_id) VALUES (:employeeId, :roleId)";

                    using (OracleCommand cmd = new OracleCommand(query, connection))
                    {
                        cmd.Parameters.Add(new OracleParameter("employeeId", employeeId));
                        cmd.Parameters.Add(new OracleParameter("roleId", roleId));

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi gán vai trò: " + ex.Message);
                return false;
            }
        }
    }
}
