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

                var title = DashboardUIFactory.CreateTitle("🔐 PHÂN QUYỀN & KIỂM SOÁT", ContentPanel.Width);
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

            // Chỉ 5 tab theo yêu cầu đề bài
            tabControl.TabPages.Add(CreateDACTab());
            tabControl.TabPages.Add(CreateMACTab());
            tabControl.TabPages.Add(CreateRBACTab());
            tabControl.TabPages.Add(CreateVPDTab());
            tabControl.TabPages.Add(CreateOLSTab());

            ContentPanel.Controls.Add(tabControl);
        }

        private TabPage CreateDACTab()
        {
            TabPage tab = new TabPage("DAC");
            Label info = new Label
            {
                Text = "🔐 Discretionary Access Control\n\nQuản lý quyền truy cập dữ liệu theo người dùng",
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 11)
            };
            tab.Controls.Add(info);
            return tab;
        }

        private TabPage CreateMACTab()
        {
            TabPage tab = new TabPage("MAC");
            Label info = new Label
            {
                Text = "🏷️ Mandatory Access Control\n\nGắn nhãn bảo mật cho dữ liệu nhạy cảm",
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 11)
            };
            tab.Controls.Add(info);
            return tab;
        }

        private TabPage CreateRBACTab()
        {
            TabPage tab = new TabPage("RBAC");
            Label info = new Label
            {
                Text = "👥 Role-Based Access Control\n\nPhân quyền theo vai trò (Admin vs Employee)",
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 11)
            };
            tab.Controls.Add(info);
            return tab;
        }

        private TabPage CreateVPDTab()
        {
            TabPage tab = new TabPage("VPD");
            Label info = new Label
            {
                Text = "🔍 Virtual Private Database\n\nGiới hạn dữ liệu theo người dùng",
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 11)
            };
            tab.Controls.Add(info);
            return tab;
        }

        private TabPage CreateOLSTab()
        {
            TabPage tab = new TabPage("OLS");
            Label info = new Label
            {
                Text = "🏷️ Oracle Label Security\n\nKiểm soát truy cập bằng nhãn Oracle",
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 11)
            };
            tab.Controls.Add(info);
            return tab;
        }

        public override void RefreshContent()
        {
            LoadContent();
        }
    }

}
