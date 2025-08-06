using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyNganHang.DataAccess
{
    public class OLSManager
    {
        // 1. Lấy danh sách tất cả policy OLS hiện có
        public DataTable Get_OLSPolicies()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_OLS_POLICIES", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách Policy OLS: " + ex.Message);
            }
            return null;
        }

        // 2. Lấy user từ DB cho ComboBox
        public DataTable GetUsers()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_all_users", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy users: " + ex.Message);
            }
            return null;
        }

        // 3. Tạo mới policy OLS
        public bool Create_OLSPolicy(string policyName, string columnName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_create_policy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("colName", OracleDbType.Varchar2).Value = columnName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo Policy OLS: " + ex.Message);
                return false;
            }
        }

        // 4. Gán quyền quản lý policy cho user
        public bool GrantPolicyManager(string policyName, string userName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_grant_policy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = userName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gán quyền quản lý policy lỗi: " + ex.Message);
                return false;
            }
        }

        // 5. Enable policy
        public bool Enable_OLSPolicy(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_enable_policy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enable Policy OLS lỗi: " + ex.Message);
                return false;
            }
        }

        // 6. Disable policy
        public bool Disable_OLSPolicy(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_disable_policy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disable Policy OLS lỗi: " + ex.Message);
                return false;
            }
        }

        // 7. Lấy trạng thái Policy OLS (Enable/Disable)
        public string GetPolicyStatus(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_Status_OLS_POLICIES", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            using (var reader = ((OracleRefCursor)resultParam.Value).GetDataReader())
                            {
                                if (reader.Read())
                                    return reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy trạng thái policy: " + ex.Message);
            }
            return "unknown";
        }

        // 8. Thêm Level
        public bool Create_Level(string policyName, int lvlNum, string shortName, string longName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_create_level", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("lvnum", OracleDbType.Int32).Value = lvlNum;
                        cmd.Parameters.Add("shortName", OracleDbType.Varchar2).Value = shortName;
                        cmd.Parameters.Add("longName", OracleDbType.Varchar2).Value = longName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Level OLS: " + ex.Message);
                return false;
            }
        }

        // 9. Thêm Compartment
        public bool Create_Compartment(string policyName, int compNum, string shortName, string longName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_create_compartment", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("cpnum", OracleDbType.Int32).Value = compNum;
                        cmd.Parameters.Add("shortName", OracleDbType.Varchar2).Value = shortName;
                        cmd.Parameters.Add("longName", OracleDbType.Varchar2).Value = longName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Compartment OLS: " + ex.Message);
                return false;
            }
        }

        // 10. Thêm Group
        public bool Create_Group(string policyName, int groupNum, string shortName, string longName, string parentName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_create_group", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("cpnum", OracleDbType.Int32).Value = groupNum;
                        cmd.Parameters.Add("shortName", OracleDbType.Varchar2).Value = shortName;
                        cmd.Parameters.Add("longName", OracleDbType.Varchar2).Value = longName;
                        cmd.Parameters.Add("parentName", OracleDbType.Varchar2).Value = parentName;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Group OLS: " + ex.Message);
                return false;
            }
        }

        // 11. Lấy danh sách short name Level theo Policy
        public DataTable Get_Levels(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_levels", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy Level OLS: " + ex.Message);
            }
            return null;
        }

        // 12. Lấy danh sách short name Compartment theo Policy
        public DataTable Get_Compartments(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_COMPARTMENTS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách Compartment: " + ex.Message);
            }
            return null;
        }

        // 13. Lấy danh sách short name Group theo Policy
        public DataTable Get_Groups(string policyName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_GROUPS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy Group: " + ex.Message);
            }
            return null;
        }

        // 14. Lấy chi tiết Level theo Policy & ShortName
        public DataTable Get_LevelDetail(string policyName, string shortName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_CTlevels", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("ShortName", OracleDbType.Varchar2).Value = shortName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy chi tiết Level: " + ex.Message);
            }
            return null;
        }

        // 15. Lấy chi tiết Compartment theo Policy & ShortName
        public DataTable Get_CompartmentDetail(string policyName, string shortName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_CTCOMPARTMENTS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("ShortName", OracleDbType.Varchar2).Value = shortName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy chi tiết Compartment: " + ex.Message);
            }
            return null;
        }

        // 16. Lấy chi tiết Group theo Policy & ShortName
        public DataTable Get_GroupDetail(string policyName, string shortName)
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    using (OracleCommand cmd = new OracleCommand("pro_select_CTGROUPS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("policyName", OracleDbType.Varchar2).Value = policyName;
                        cmd.Parameters.Add("ShortName", OracleDbType.Varchar2).Value = shortName;
                        var resultParam = new OracleParameter("v_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(resultParam);
                        cmd.ExecuteNonQuery();
                        if (resultParam.Value != DBNull.Value)
                        {
                            var reader = ((OracleRefCursor)resultParam.Value).GetDataReader();
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy chi tiết Group: " + ex.Message);
            }
            return null;
        }
    }
}
