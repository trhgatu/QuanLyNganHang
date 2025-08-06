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
                    string sql = "SELECT branch_id, branch_name, branch_code FROM ADMIN_NGANHANG.branches WHERE status = 1";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text; // Thay đổi thành Text

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy danh sách chi nhánh: " + ex.Message);
            }
            return new DataTable(); // Trả về DataTable rỗng thay vì null
        }

    }
}
