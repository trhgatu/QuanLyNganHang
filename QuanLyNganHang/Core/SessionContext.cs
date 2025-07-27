using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNganHang.Core
{
    public static class SessionContext
    {
        public static string OracleUser { get; set; }
        public static string FullName { get; set; }
        public static string RoleName { get; set; }
        public static int EmployeeId { get; set; }
        public static string Position { get; set; }
        public static int BranchId { get; set; }
        public static string BranchName { get; set; }

        public static void Clear()
        {
            OracleUser = null;
            FullName = null;
            RoleName = null;
            EmployeeId = 0;
            Position = null;
            BranchId = 0;
            BranchName = null;
        }
    }
}
