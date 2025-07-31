using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class Create_User
    {
        public int Pro_CheckUser(string UserName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string Function = "sys.pkg_cruser.fun_check_account";

                    OracleCommand cmd = new OracleCommand
                    {
                        Connection = conn,
                        CommandText = Function,
                        CommandType = CommandType.StoredProcedure
                    };

                    OracleParameter resultParam = new OracleParameter
                    {
                        ParameterName = "@kq",
                        OracleDbType = OracleDbType.Int16,
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(resultParam);

                    OracleParameter userParam = new OracleParameter
                    {
                        ParameterName = "@username",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = UserName.ToUpper(),
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(userParam);

                    cmd.ExecuteNonQuery();

                    if (resultParam.Value != DBNull.Value)
                    {
                        return int.Parse(resultParam.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi gọi chạy hàm ở Package!!\n" + ex.Message);
            }

            return -1;
        }

        /// <summary>
        /// Gọi procedure tạo hoặc cập nhật tài khoản oracle (tùy theo user đã tồn tại hay chưa)
        /// Trả về true nếu thành công, false nếu lỗi
        /// </summary>
        public bool Pro_CreateUser(string UserName, string Password)
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
                        CommandText = "PKG_CRUSER.PRO_CRUSER",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("USERNAME", OracleDbType.Varchar2).Value = UserName.ToUpper();
                    cmd.Parameters.Add("PASS", OracleDbType.Varchar2).Value = Password;

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi khi tạo user Oracle:\n" + ex.Message);
                return false;
            }
        }
    }
}
