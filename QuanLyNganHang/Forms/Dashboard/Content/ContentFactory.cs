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
                case "ProfileManagement":
                    return new ProfileManagementContent(contentPanel);
               default:
                   return new UserManagementContent(contentPanel);
            }
        }
    }
}
