using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.DataAccess
{
    public class BranchDataAccess
    {
        public DataTable GetAllBranches()
        {
            try
            {
                using (var conn = Database.Get_Connect())
                {
                    string procedure = "ADMIN_NGANHANG.pkg_Branch.pro_get_all_branches";

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

                        if (resultParam.Value != DBNull.Value && resultParam.Value is OracleRefCursor cursor)
                        {
                            using (var reader = cursor.GetDataReader())
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gọi thủ tục: pro_get_all_branches\n" + ex.Message);
            }
            return null;
        }
    }
}
