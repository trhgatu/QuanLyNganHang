using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyNganHang.DataAccess
{
    public class AuthorizationManager
    {
        public DataTable Get_User()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_user";

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

                        if (resultParam.Value != DBNull.Value)
                        {
                            using (OracleDataReader reader = ((OracleRefCursor)resultParam.Value).GetDataReader())
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                return table;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_select_user");
            }

            return null;
        }


        public DataTable Get_Roles()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_roles";

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

                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            DataTable table = new DataTable();
                            table.Load(reader);
                            return table;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi gọi chạy thủ tục: pro_select_roles");
            }

            return null;
        }

        public DataTable Get_Procedure_User(string userowner, string type)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_procedure_user";

                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter userOwnerParam = new OracleParameter
                        {
                            ParameterName = "userowner",
                            OracleDbType = OracleDbType.Varchar2,
                            Value = userowner.ToUpper(),
                            Direction = ParameterDirection.Input
                        };
                        cmd.Parameters.Add(userOwnerParam);

                        OracleParameter typeParam = new OracleParameter
                        {
                            ParameterName = "pro_type",
                            OracleDbType = OracleDbType.Varchar2,
                            Value = type.ToUpper(),
                            Direction = ParameterDirection.Input
                        };
                        cmd.Parameters.Add(typeParam);

                        OracleParameter resultParam = new OracleParameter
                        {
                            ParameterName = "Result",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();

                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            DataTable table = new DataTable();
                            table.Load(reader);
                            return table;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_select_procedure_user");
            }

            return null;
        }

        public DataTable Get_Table_User(string userowner)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_table";

                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter userOwnerParam = new OracleParameter
                        {
                            ParameterName = "userowner",
                            OracleDbType = OracleDbType.Varchar2,
                            Value = userowner.ToUpper(),
                            Direction = ParameterDirection.Input
                        };
                        cmd.Parameters.Add(userOwnerParam);

                        OracleParameter resultParam = new OracleParameter
                        {
                            ParameterName = "Result",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();

                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            DataTable table = new DataTable();
                            table.Load(reader);
                            return table;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_select_table");
            }

            return null;
        }
        public DataTable Get_Roles_User(string username)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_user_roles";
                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new OracleParameter
                        {
                            ParameterName = "@username",
                            OracleDbType = OracleDbType.Varchar2,
                            Direction = ParameterDirection.Input,
                            Value = username.ToUpper()
                        });

                        var resultParam = new OracleParameter
                        {
                            ParameterName = "@Result",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            OracleDataReader reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var table = new DataTable();
                            table.Load(reader);
                            return table;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi Gọi chạy thủ tục: pro_user_roles");
            }
            return null;
        }

        public int Get_Roles_User_Check(string username, string roles)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_user_roles_check";
                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new OracleParameter("@username", OracleDbType.Varchar2, username.ToUpper(), ParameterDirection.Input));
                        cmd.Parameters.Add(new OracleParameter("@roles", OracleDbType.Varchar2, roles.ToUpper(), ParameterDirection.Input));

                        var resultParam = new OracleParameter("@Result", OracleDbType.Int16, ParameterDirection.Output);
                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            return int.Parse(resultParam.Value.ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi Gọi chạy thủ tục: pro_user_roles_check");
            }
            return -1;
        }

        public DataTable Get_Grant_User(string username)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_grant_user";
                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new OracleParameter("@username", OracleDbType.Varchar2)
                        {
                            Value = username.ToUpper(),
                            Direction = ParameterDirection.Input
                        });

                        OracleParameter resultParam = new OracleParameter("@Result", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        cmd.ExecuteNonQuery();

                        if (resultParam.Value != DBNull.Value)
                        {
                            OracleDataReader reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            DataTable data = new DataTable();
                            data.Load(reader);
                            return data;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi gọi chạy thủ tục: pro_select_grant_user");
            }

            return null;
        }
        public DataTable Get_Grant(string username, string userschema, string tab)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_select_grant";
                    using (OracleCommand cmd = new OracleCommand(procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new OracleParameter("@username", OracleDbType.Varchar2)
                        {
                            Value = username.ToUpper(),
                            Direction = ParameterDirection.Input
                        });
                        cmd.Parameters.Add(new OracleParameter("@userschema", OracleDbType.Varchar2)
                        {
                            Value = userschema.ToUpper(),
                            Direction = ParameterDirection.Input
                        });
                        cmd.Parameters.Add(new OracleParameter("@tab", OracleDbType.Varchar2)
                        {
                            Value = tab.ToUpper(),
                            Direction = ParameterDirection.Input
                        });
                        OracleParameter resultParam = new OracleParameter("@Result", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            DataTable table = new DataTable();
                            table.Load(reader);
                            return table;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_select_grant");
                return null;
            }
            return null;
        }

        public bool Grant_Revoke_Pro(string username, string user_schema, string pro_tab, string type_pro, int dk)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_grant_revoke";

                    OracleCommand cmd = new OracleCommand(procedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName = true; 

                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username.ToUpper();
                    cmd.Parameters.Add("schema_user", OracleDbType.Varchar2).Value = user_schema.ToUpper();
                    cmd.Parameters.Add("pro_tab", OracleDbType.Varchar2).Value = pro_tab.ToUpper();
                    cmd.Parameters.Add("type_pro", OracleDbType.Varchar2).Value = type_pro.ToUpper();
                    cmd.Parameters.Add("dk", OracleDbType.Int16).Value = dk;

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_grant_revoke\n" + ex.Message);
                return false;
            }
        }

        public bool Grant_Revoke_Role(string username, string role, int dk)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "pkg_PhanQuyen.pro_grant_revoke_roles";

                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = procedure;
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter UserName = new OracleParameter();
                    UserName.ParameterName = "username";
                    UserName.OracleDbType = OracleDbType.Varchar2;
                    UserName.Value = username.ToUpper();
                    UserName.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(UserName);

                    OracleParameter Role = new OracleParameter();
                    Role.ParameterName = "roles";
                    Role.OracleDbType = OracleDbType.Varchar2;
                    Role.Value = role.ToUpper();
                    Role.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(Role);

                    OracleParameter DK = new OracleParameter();
                    DK.ParameterName = "dk";
                    DK.OracleDbType = OracleDbType.Int16;
                    DK.Value = dk;
                    DK.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(DK);

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi gọi chạy thủ tục: pro_grant_revoke_roles");
                return false;
            }
        }

        public bool GrantLogin(string username)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                using (var cmd = new OracleCommand("pkg_PhanQuyen.pro_grant_login", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName = true;

                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username.ToUpper();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gán quyền login: " + ex.Message);
                return false;
            }
        }
        public bool RevokeLogin(string username)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                using (var cmd = new OracleCommand("pkg_PhanQuyen.pro_revoke_login", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName = true;

                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username.ToUpper();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thu hồi quyền đăng nhập: " + ex.Message);
                return false;
            }
        }

        public bool GrantAllTables(string username)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    var cmd = new OracleCommand("pkg_PhanQuyen.pro_grant_all_tables", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username.ToUpper();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấp toàn bộ quyền: " + ex.Message);
                return false;
            }
        }




    }
}
