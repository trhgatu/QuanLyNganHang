using QuanLyNganHang.Forms.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang
{
    public partial class DashboardForm : Form
    {
        private HeaderManager headerManager;
        private MenuManager menuManager;
        private ContentManager contentManager;
        private Panel footerPanel;

        public DashboardForm()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            // Form properties
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Hệ thống Quản lý Ngân hàng - Dashboard";
            this.BackColor = DashboardConstants.Colors.Background;

            // Initialize managers
            headerManager = new HeaderManager();
            menuManager = new MenuManager();
            contentManager = new ContentManager();

            // Create UI components
            CreateHeader();
            CreateMenu();
            CreateContent();
            CreateFooter();

            // Subscribe to events
            menuManager.MenuItemClicked += OnMenuItemClicked;

            // Load default content
            contentManager.LoadContent("UserManagement");
        }

        private void CreateHeader()
        {
            var header = headerManager.CreateHeader(this, GetCurrentUser(), GetCurrentRole());
            this.Controls.Add(header);
        }

        private void CreateMenu()
        {
            var menu = menuManager.CreateMenu(this);
            this.Controls.Add(menu);
        }

        private void CreateContent()
        {
            var content = contentManager.CreateContentPanel(this);
            this.Controls.Add(content);
        }

        private void CreateFooter()
        {
            footerPanel = new Panel
            {
                Size = new Size(this.Width, DashboardConstants.Sizes.FooterHeight),
                BackColor = DashboardConstants.Colors.Dark,
                Dock = DockStyle.Bottom
            };

            Label footerLabel = new Label
            {
                Text = DashboardConstants.Texts.AppVersion,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(400, DashboardConstants.Sizes.FooterHeight),
                Location = new Point(20, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label statusLabel = new Label
            {
                Text = $"🟢 {DashboardConstants.Texts.SystemStatus}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGreen,
                AutoSize = false,
                Size = new Size(250, DashboardConstants.Sizes.FooterHeight),
                Location = new Point(this.Width - 270, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            footerPanel.Controls.AddRange(new Control[] { footerLabel, statusLabel });
            this.Controls.Add(footerPanel);
        }

        private void OnMenuItemClicked(object sender, string menuKey)
        {
            if (menuKey == "Logout")
            {
                HandleLogout();
            }
            else
            {
                contentManager.LoadContent(menuKey);
                menuManager.SetSelectedMenu(menuKey);
            }
        }

        private void HandleLogout()
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    headerManager?.Dispose();
                    Database.Close_Connect();
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetCurrentUser()
        {
            // TODO: Implement actual user retrieval from session/database
            return "Nguyễn Văn A";
        }

        private string GetCurrentRole()
        {
            // TODO: Implement actual role retrieval from session/database
            return "Quản trị viên";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            headerManager?.Dispose();
            base.OnFormClosed(e);
        }
    }
}