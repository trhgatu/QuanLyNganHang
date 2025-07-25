using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public static class ContentFactory
    {
        public static BaseContent CreateContent(string contentType, Panel contentPanel)
        {
            switch (contentType)
            {
                case "UserManagement":
                    return new UserManagementContent(contentPanel);
                case "CustomerManagement":
                    return new CustomerManagementContent(contentPanel);
                case "AccountManagement":
                    return new AccountManagementContent(contentPanel);
                case "TransactionManagement":
                    return new TransactionManagementContent(contentPanel);
                case "PermissionManagement":
                    return new PermissionManagementContent(contentPanel);
                case "AuditLog":
                    return new AuditLogContent(contentPanel);
                case "Reports":
                    return new ReportsContent(contentPanel);
                case "Settings":
                    return new SettingsContent(contentPanel);
               default:
                   return new UserManagementContent(contentPanel);
            }
        }
    }
}
