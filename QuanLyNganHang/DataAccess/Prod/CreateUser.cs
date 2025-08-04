using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyNganHang.DataAccess
{
    public class Create_User
    {
        public int Pro_CheckUser(OracleConnection conn, OracleTransaction tran, string UserName)
        {
            try
            {
                string Function = "ADMIN_NGANHANG.pkg_cruser.fun_check_account";

                using (OracleCommand cmd = new OracleCommand(Function, conn))
                {
                    cmd.Transaction = tran;
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter resultParam = new OracleParameter
                    {
                        ParameterName = "kq",
                        OracleDbType = OracleDbType.Int16,
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(resultParam);

                    OracleParameter userParam = new OracleParameter
                    {
                        ParameterName = "username",
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
                MessageBox.Show("Lỗi gọi chạy hàm ở Package!!\n" + ex.Message);
            }

            return -1;
        }

        public bool Pro_CreateUser(OracleConnection conn, OracleTransaction tran, string UserName, string Password)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ADMIN_NGANHANG.PKG_CRUSER.PRO_CRUSER", conn))
                {
                    cmd.Transaction = tran;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("USERNAME", OracleDbType.Varchar2).Value = UserName.ToUpper();
                    cmd.Parameters.Add("PASS", OracleDbType.Varchar2).Value = Password;

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo user Oracle:\n" + ex.Message);
                return false;
            }
        }
    }
}
