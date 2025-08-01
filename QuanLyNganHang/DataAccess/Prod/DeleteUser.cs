using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;


namespace QuanLyNganHang.DataAccess
{
    public class DeleteUser
    {
        public bool Pro_DropUserById(int employeeId)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    OracleCommand cmd = new OracleCommand
                    {
                        Connection = conn,
                        CommandText = "pro_drop_user_by_id",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi xóa người dùng theo ID:\n" + ex.Message);
                return false;
            }
        }
    }
}
