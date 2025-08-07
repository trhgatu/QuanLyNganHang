using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;

namespace QuanLyNganHang.DataAccess
{
    public class EmployeeDataAccess
    {
        public static (
     string FullName, string RoleName, int EmployeeId, string Position,
     string Email, string OracleUser, string Phone, string Address,
     int BranchId, string BranchCode, string BranchName, string BranchAddress, string BranchPhone,
     int BankId, string BankCode, string BankName, string BankAddress, string BankPhone, string BankEmail
 ) GetProfileFull(string oracleUser)
        {
            using (var conn = Database.Get_Connect())
            {
                using (var cmd = new OracleCommand("ADMIN_NGANHANG.PKG_EMPLOYEE.fn_get_profile_full", conn))
                {
                    var resultParam = new OracleParameter
                    {
                        ParameterName = "RETURN_VALUE",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(resultParam);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_oracle_user", OracleDbType.Varchar2).Value = oracleUser.ToUpper();
                    cmd.ExecuteNonQuery();
                    if (resultParam.Value != DBNull.Value && resultParam.Value is OracleRefCursor cursor)
                    {
                        using (var reader = cursor.GetDataReader())
                        {
                            if (reader.Read())
                            {
                                return (
                                    reader.GetString(0), reader.GetString(1), reader.GetInt32(2),
                                    reader.IsDBNull(3) ? null : reader.GetString(3),
                                    reader.IsDBNull(4) ? null : reader.GetString(4),
                                    reader.IsDBNull(5) ? null : reader.GetString(5),
                                    reader.IsDBNull(6) ? null : reader.GetString(6),
                                    reader.IsDBNull(7) ? null : reader.GetString(7),
                                    reader.GetInt32(8), reader.GetString(9), reader.GetString(10),
                                    reader.IsDBNull(11) ? null : reader.GetString(11),
                                    reader.IsDBNull(12) ? null : reader.GetString(12),
                                    reader.GetInt32(13), reader.GetString(14), reader.GetString(15),
                                    reader.IsDBNull(16) ? null : reader.GetString(16),
                                    reader.IsDBNull(17) ? null : reader.GetString(17),
                                    reader.IsDBNull(18) ? null : reader.GetString(18)
                                );
                            }
                        }
                    }
                }
            }
            return ("Không rõ", "Không rõ", 0, null, null, null, null, null, 0, null, null, null, null, 0, null, null, null, null, null);
        }


        public static bool UsernameExists(string username)
        {
            using (var conn = Database.Get_Connect())
            {
                using (var cmd = new OracleCommand("ADMIN_NGANHANG.PKG_EMPLOYEE.fn_check_username_exists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username.ToUpper();

                    var returnParam = new OracleParameter
                    {
                        ParameterName = "RETURN_VALUE",
                        OracleDbType = OracleDbType.Decimal,
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParam);

                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(returnParam.Value) > 0;
                }
            }
            
        }

        public static (bool created, string errorMsg) CreateFullUser(OracleConnection conn, OracleTransaction tran,
    string fullName, string email, string phone, string address,
    string position, int branchId, string username, string password, int roleId)
        {
            try
            {
                using (var cmd = new OracleCommand("ADMIN_NGANHANG.PKG_EMPLOYEE.pro_create_full_user", conn))
                {
                    cmd.Transaction = tran;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_full_name", OracleDbType.Varchar2).Value = fullName;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    cmd.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = phone;
                    cmd.Parameters.Add("p_address", OracleDbType.Varchar2).Value = address;
                    cmd.Parameters.Add("p_position", OracleDbType.Varchar2).Value = position;
                    cmd.Parameters.Add("p_branch_id", OracleDbType.Int32).Value = branchId;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = password;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;

                    var p_success = new OracleParameter("p_success", OracleDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_success);

                    var p_error = new OracleParameter("p_error_msg", OracleDbType.Varchar2, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_error);

                    cmd.ExecuteNonQuery();

                    OracleDecimal oracleDec = (OracleDecimal)p_success.Value;
                    int success = oracleDec.ToInt32();

                    string errMsg = p_error.Value?.ToString();

                    return (success == 1, errMsg);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public static (bool success, string errorMsg) UpdateProfile(string oracleUser, string fullName, string email, string phone, string address)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (var cmd = new OracleCommand("ADMIN_NGANHANG.PKG_EMPLOYEE.pro_update_profile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.Add("p_oracle_user", OracleDbType.Varchar2).Value = oracleUser.ToUpper();
                        cmd.Parameters.Add("p_full_name", OracleDbType.Varchar2).Value = fullName;
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = phone ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_address", OracleDbType.Varchar2).Value = address ?? (object)DBNull.Value;

                        // Output parameters
                        var p_success = new OracleParameter("p_success", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(p_success);

                        var p_error = new OracleParameter("p_error_msg", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(p_error);

                        cmd.ExecuteNonQuery();

                        OracleDecimal oracleDec = (OracleDecimal)p_success.Value;
                        int success = oracleDec.ToInt32();

                        string errMsg = p_error.Value?.ToString();

                        return (success == 1, errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}");
            }
        }
        public static (bool success, string errorMsg) ChangePassword(string oracleUser, string oldPassword, string newPassword)
        {
            try
            {
                // ✅ KHÔNG hash nữa - gửi mật khẩu plaintext để Oracle xử lý
                using (var conn = Database.Get_Connect())
                {
                    using (var cmd = new OracleCommand("ADMIN_NGANHANG.PKG_EMPLOYEE.pro_change_password", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters - plaintext passwords
                        cmd.Parameters.Add("p_oracle_user", OracleDbType.Varchar2).Value = oracleUser.ToUpper();
                        cmd.Parameters.Add("p_old_password", OracleDbType.Varchar2).Value = oldPassword;  // ✅ Plaintext
                        cmd.Parameters.Add("p_new_password", OracleDbType.Varchar2).Value = newPassword;  // ✅ Plaintext

                        // Output parameters
                        var p_success = new OracleParameter("p_success", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(p_success);

                        var p_error = new OracleParameter("p_error_msg", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(p_error);

                        cmd.ExecuteNonQuery();

                        OracleDecimal oracleDec = (OracleDecimal)p_success.Value;
                        int success = oracleDec.ToInt32();

                        string errMsg = p_error.Value?.ToString();

                        return (success == 1, errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}");
            }
        }



    }
}
