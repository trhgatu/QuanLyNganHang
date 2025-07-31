using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNganHang.DataAccess
{
    public class DeleteUser
    {
        public bool Pro_DropUserFull(string username)
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
                        CommandText = "pro_drop_user_full",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username.ToUpper();

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi xóa user Oracle:\n" + ex.Message);
                return false;
            }
        }
    }
}
