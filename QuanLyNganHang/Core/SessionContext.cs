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
        public static string Email { get; set; }
        public static string Phone { get; set; }
        public static string Address { get; set; }
        public static int BranchId { get; set; }
        public static string BranchCode { get; set; }
        public static string BranchName { get; set; }
        public static string BranchAddress { get; set; }
        public static string BranchPhone { get; set; }
        public static int BankId { get; set; }
        public static string BankCode { get; set; }
        public static string BankName { get; set; }
        public static string BankAddress { get; set; }
        public static string BankPhone { get; set; }
        public static string BankEmail { get; set; }
        public static void Clear()
        {
            OracleUser = null;
            FullName = null;
            RoleName = null;
            EmployeeId = 0;
            Position = null;
            Email = null;
            Phone = null;
            Address = null;

            BranchId = 0;
            BranchCode = null;
            BranchName = null;
            BranchAddress = null;
            BranchPhone = null;

            BankId = 0;
            BankCode = null;
            BankName = null;
            BankAddress = null;
            BankPhone = null;
            BankEmail = null;
        }
    }
}
