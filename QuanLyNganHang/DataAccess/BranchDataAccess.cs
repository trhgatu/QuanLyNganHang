using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.DataAccess
{
    public class BranchDataAccess
    {
        public DataTable GetAllBranches()
        {
            using (var conn = Database.Get_Connect())
            {
                string query = "SELECT branch_id, branch_name FROM branches WHERE status = 1";
                using (var cmd = new OracleCommand(query, conn))
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
