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
                    string procedure = "ADMIN_NGANHANG.pkg_Role.pro_get_all_roles";

                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter resultParam = new OracleParameter
                        {
                            ParameterName = "Result",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value && resultParam.Value is OracleRefCursor cursor)
                        {
                            using (var reader = cursor.GetDataReader())
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                return table;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi gọi thủ tục: pro_get_all_roles\n" + ex.Message);
            }
            return null;
        }


        public bool AssignRoleToEmployee(int employeeId, int roleId)
        {
            var connection = Database.Get_Connect();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            string query = "INSERT INTO ADMIN_NGANHANG.employee_roles (employee_id, role_id) VALUES (:employeeId, :roleId)";
            using (OracleCommand cmd = new OracleCommand(query, connection))
            {
                cmd.Parameters.Add(new OracleParameter("employeeId", employeeId));
                cmd.Parameters.Add(new OracleParameter("roleId", roleId));
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error assigning role: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
