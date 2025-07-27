using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.DataAccess
{
    public class EmployeeDataAccess
    {
        public static (string FullName, string RoleName, int EmployeeId, string Position, int BranchId, string BranchName) GetProfileFull()
        {
            using (var conn = Database.Get_Connect())
            {
                string query = @"
            SELECT e.full_name, r.role_name, e.employee_id, e.position, b.branch_id, b.branch_name
            FROM employees e
            JOIN employee_roles er ON e.employee_id = er.employee_id
            JOIN roles r ON r.role_id = er.role_id
            JOIN branches b ON e.branch_id = b.branch_id
            WHERE UPPER(e.oracle_user) = SYS_CONTEXT('USERENV', 'SESSION_USER')";

                using (var cmd = new OracleCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.IsDBNull(3) ? null : reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetString(5)
                        );
                    }
                }
            }

            return ("Không rõ", "Không rõ", 0, null, 0, null);
        }
    }
}
