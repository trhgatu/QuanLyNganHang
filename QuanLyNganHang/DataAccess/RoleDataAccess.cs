using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.DataAccess
{
    public class RoleDataAccess
    {

        public DataTable GetAllRoles()
        {
            var connection = Database.Get_Connect();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            string query = "SELECT role_id, role_name FROM ADMIN_NGANHANG.roles WHERE status = 1";

            using (OracleCommand cmd = new OracleCommand(query, connection))
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
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
