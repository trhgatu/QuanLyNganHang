using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public class MenuManager
    {
        private Panel menuPanel;
        private Button currentSelectedButton;

        public event EventHandler<string> MenuItemClicked;

        public Panel CreateMenu(Form parentForm)
        {
            menuPanel = new Panel
            {
                Size = new Size(DashboardConstants.Sizes.MenuPanelWidth, parentForm.Height - 120),
                Location = new Point(0, DashboardConstants.Sizes.HeaderHeight),
                BackColor = DashboardConstants.Colors.Secondary,
                Dock = DockStyle.Left
            };

            CreateMenuItems();
            CreateLogoutButton();

            return menuPanel;
        }

        private void CreateMenuItems()
        {
            var menuItems = new[]
            {
                new { Text = "👥 Quản lý Người dùng", Key = "UserManagement" },
                new { Text = "👤 Quản lý Khách hàng", Key = "CustomerManagement" },
                new { Text = "🏦 Quản lý Tài khoản", Key = "AccountManagement" },
                new { Text = "💰 Quản lý Giao dịch", Key = "TransactionManagement" },
                new { Text = "🔐 Phân quyền", Key = "PermissionManagement" },
                new { Text = "🔐 Quản lý tài khoản Oracle", Key = "OracleAccountManagement" },
                new { Text = "📋 Nhật ký Audit", Key = "AuditLog" },
                new { Text = "📊 Báo cáo", Key = "Reports" },
                new { Text = "⚙️ Cài đặt", Key = "Settings" }
            };

            for (int i = 0; i < menuItems.Length; i++)
            {
                var menuItem = menuItems[i];

                Button menuButton = new Button
                {
                    Text = menuItem.Text,
                    Size = new Size(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.MenuButtonHeight),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = DashboardConstants.Colors.Secondary,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(20, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Dock = DockStyle.Top,
                    Tag = menuItem.Key // String, không phải Action
                };

                menuButton.FlatAppearance.BorderSize = 0;
                menuButton.FlatAppearance.MouseOverBackColor = DashboardConstants.Colors.MenuHover;
                menuButton.Click += MenuButton_Click;

                menuPanel.Controls.Add(menuButton);
            }
        }

        private void CreateLogoutButton()
        {
            Button btnLogout = new Button
            {
                Text = "🚪 Đăng xuất",
                Size = new Size(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.MenuButtonHeight),
                FlatStyle = FlatStyle.Flat,
                BackColor = DashboardConstants.Colors.LogoutButton,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Bottom,
                Tag = "Logout" // String key
            };

            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += MenuButton_Click; // Sử dụng chung event handler

            menuPanel.Controls.Add(btnLogout);
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button clickedButton = sender as Button;
                if (clickedButton?.Tag == null) return;

                string menuKey = clickedButton.Tag.ToString();
                if (menuKey != "Logout")
                {
                    if (currentSelectedButton != null)
                        currentSelectedButton.BackColor = DashboardConstants.Colors.Secondary;

                    clickedButton.BackColor = DashboardConstants.Colors.MenuHover;
                    currentSelectedButton = clickedButton;
                }
                OnMenuItemClicked(menuKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi menu click: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnMenuItemClicked(string menuKey)
        {
            try
            {
                MenuItemClicked?.Invoke(this, menuKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trigger event: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetSelectedMenu(string menuKey)
        {
            try
            {
                foreach (Control control in menuPanel.Controls)
                {
                    if (control is Button btn && btn.Tag?.ToString() == menuKey)
                    {
                        if (currentSelectedButton != null)
                            currentSelectedButton.BackColor = DashboardConstants.Colors.Secondary;

                        btn.BackColor = DashboardConstants.Colors.MenuHover;
                        currentSelectedButton = btn;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting selected menu: {ex.Message}");
            }
        }
    }
}
