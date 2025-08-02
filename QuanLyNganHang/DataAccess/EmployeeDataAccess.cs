using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

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
                FROM ADMIN_NGANHANG.employees e
                JOIN ADMIN_NGANHANG.employee_roles er ON e.employee_id = er.employee_id
                JOIN ADMIN_NGANHANG.roles r ON r.role_id = er.role_id
                JOIN ADMIN_NGANHANG.branches b ON e.branch_id = b.branch_id
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
        public static bool UsernameExists(string username, OracleConnection conn)
        {
            string query = "SELECT COUNT(*) FROM ADMIN_NGANHANG.system_users WHERE UPPER(username) = :username";
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add("username", username.ToUpper());
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        public static bool CreateFullUser(string fullName, string email, string phone, string address, string position, int branchId, string username, string password, int roleId)
        {
            using (var conn = Database.Get_Connect())
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmdGetId = new OracleCommand("SELECT seq_employee_id.NEXTVAL FROM dual", conn);
                        cmdGetId.Transaction = tran;
                        int employeeId = Convert.ToInt32(cmdGetId.ExecuteScalar());

                        string empCode = "EMP" + employeeId.ToString().PadLeft(4, '0');
                        string insertEmp = @"
                    INSERT INTO ADMIN_NGANHANG.employees (
                        employee_id, employee_code, full_name, email, phone, address, position, branch_id, hire_date, salary, status, created_date, oracle_user
                    ) VALUES (
                        :id, :code, :name, :email, :phone, :addr, :pos, :branchId, SYSDATE, 0, 1, SYSDATE, :oracleUser
                    )";

                        var cmdInsertEmp = new OracleCommand(insertEmp, conn);
                        cmdInsertEmp.Transaction = tran;
                        cmdInsertEmp.Parameters.Add("id", employeeId);
                        cmdInsertEmp.Parameters.Add("code", empCode);
                        cmdInsertEmp.Parameters.Add("name", fullName);
                        cmdInsertEmp.Parameters.Add("email", email);
                        cmdInsertEmp.Parameters.Add("phone", phone);
                        cmdInsertEmp.Parameters.Add("addr", address);
                        cmdInsertEmp.Parameters.Add("pos", position);
                        cmdInsertEmp.Parameters.Add("branchId", branchId);
                        cmdInsertEmp.Parameters.Add("oracleUser", username);
                        cmdInsertEmp.ExecuteNonQuery();
                        string insertUser = @"
                    INSERT INTO ADMIN_NGANHANG.system_users (
                        user_id, username, password_hash, employee_id, status, created_date
                    ) VALUES (
                        seq_system_user_id.NEXTVAL, :username, :password_hash, :employee_id, 1, SYSDATE
                    )";

                        var cmdInsertUser = new OracleCommand(insertUser, conn);
                        cmdInsertUser.Transaction = tran;
                        cmdInsertUser.Parameters.Add("username", username.ToUpper());
                        cmdInsertUser.Parameters.Add("password_hash", HashHelper.HashPassword(password));
                        cmdInsertUser.Parameters.Add("employee_id", employeeId);
                        cmdInsertUser.ExecuteNonQuery();

                        string assignRole = "INSERT INTO ADMIN_NGANHANG.employee_roles (employee_id, role_id) VALUES (:eid, :rid)";
                        var cmdAssignRole = new OracleCommand(assignRole, conn);
                        cmdAssignRole.Transaction = tran;
                        cmdAssignRole.Parameters.Add("eid", employeeId);
                        cmdAssignRole.Parameters.Add("rid", roleId);
                        cmdAssignRole.ExecuteNonQuery();

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        System.Windows.Forms.MessageBox.Show("Lỗi khi tạo người dùng đầy đủ: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
