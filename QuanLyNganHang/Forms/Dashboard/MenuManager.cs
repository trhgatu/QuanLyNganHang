using QuanLyNganHang.Core.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public class MenuManager
    {
        private Panel menuPanel;
        private Button currentSelectedButton;
        private Form parentForm; 

        public event EventHandler<string> MenuItemClicked;

        public Panel CreateMenu(Form parentForm)
        {
            this.parentForm = parentForm; 

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
                new {Text = "👤 Thông tin cá nhân", Key = "ProfileManagement"},
                new { Text = "👥 Quản lý Nhân viên", Key = "UserManagement" },
                new { Text = "👤 Quản lý Khách hàng", Key = "CustomerManagement" },
                new { Text = "🏦 Quản lý Tài khoản", Key = "AccountManagement" },
                new { Text = "💰 Quản lý Giao dịch", Key = "TransactionManagement" },
                new { Text = "🔐 Phân quyền", Key = "PermissionManagement" },
                new { Text = "📋 Nhật ký Audit", Key = "AuditLog" },
                new { Text = "📊 Báo cáo", Key = "Reports" },
            };

            // ✅ Reverse order để đúng thứ tự hiển thị (do Dock = Top)
            for (int i = menuItems.Length - 1; i >= 0; i--)
            {
                var menuItem = menuItems[i];

                Button menuButton = CreateMenuButton(menuItem.Text, menuItem.Key);
                menuPanel.Controls.Add(menuButton);
            }
        }

        // ✅ Extract method để tái sử dụng
        private Button CreateMenuButton(string text, string key)
        {
            Button menuButton = new Button
            {
                Text = text,
                Size = new Size(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.MenuButtonHeight),
                FlatStyle = FlatStyle.Flat,
                BackColor = DashboardConstants.Colors.Secondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top,
                Tag = key
            };

            menuButton.FlatAppearance.BorderSize = 0;
            menuButton.FlatAppearance.MouseOverBackColor = DashboardConstants.Colors.MenuHover;

            // ✅ Thêm hover effects
            menuButton.MouseEnter += (s, e) =>
            {
                if (menuButton != currentSelectedButton)
                {
                    menuButton.BackColor = DashboardConstants.Colors.MenuHover;
                }
            };

            menuButton.MouseLeave += (s, e) =>
            {
                if (menuButton != currentSelectedButton)
                {
                    menuButton.BackColor = DashboardConstants.Colors.Secondary;
                }
            };

            menuButton.Click += MenuButton_Click;
            return menuButton;
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
                Tag = "Logout"
            };

            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += MenuButton_Click;

            menuPanel.Controls.Add(btnLogout);
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button clickedButton = sender as Button;
                if (clickedButton?.Tag == null) return;

                string menuKey = clickedButton.Tag.ToString();
                if (currentSelectedButton != null)
                    currentSelectedButton.BackColor = DashboardConstants.Colors.Secondary;

                clickedButton.BackColor = DashboardConstants.Colors.MenuHover;
                currentSelectedButton = clickedButton;

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

        // ✅ Method để reset selected state khi logout
        public void ClearSelection()
        {
            if (currentSelectedButton != null)
            {
                currentSelectedButton.BackColor = DashboardConstants.Colors.Secondary;
                currentSelectedButton = null;
            }
        }

        // ✅ Method để disable menu khi đang logout
        public void SetMenuEnabled(bool enabled)
        {
            if (menuPanel != null)
            {
                foreach (Control control in menuPanel.Controls)
                {
                    control.Enabled = enabled;
                }
            }
        }
    }
}
