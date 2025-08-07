using QuanLyNganHang.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class PermissionManagementContent : BaseContent
    {
        public PermissionManagementContent(Panel contentPanel) : base(contentPanel) { }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("PHÂN QUYỀN KIỂM SOÁT", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                CreateSimpleTabControl();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void CreateSimpleTabControl()
        {
            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 80),
                Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 100),
                Font = new Font("Segoe UI", 10)
            };
            tabControl.TabPages.Add(CreateRBACTab());

            ContentPanel.Controls.Add(tabControl);
        }
        private TabPage CreateRBACTab()
        {
            TabPage tab = new TabPage("RBAC");
            var authControl = new AuthorizationForm();
            authControl.Dock = DockStyle.Fill;
            tab.Controls.Add(authControl);
            return tab;
        }

  
        public override void RefreshContent()
        {
            LoadContent();
        }
    }

}
