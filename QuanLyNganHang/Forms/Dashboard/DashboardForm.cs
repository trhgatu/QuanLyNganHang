using QuanLyNganHang.Core;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Forms.Dashboard;
using System;
using System.Drawing;
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
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Hệ thống Quản lý Ngân hàng - Dashboard";
            this.BackColor = DashboardConstants.Colors.Background;

            headerManager = new HeaderManager();
            try
            {
                var (
    fullName, roleName, employeeId, position,
    email, oracleUser, phone, address,
    branchId, branchCode, branchName, branchAddress, branchPhone,
    bankId, bankCode, bankName, bankAddress, bankPhone, bankEmail
) = EmployeeDataAccess.GetProfileFull(SessionContext.OracleUser);
                SessionContext.FullName = fullName;
                SessionContext.RoleName = roleName;
                SessionContext.EmployeeId = employeeId;
                SessionContext.Position = position;
                SessionContext.Email = email;
                SessionContext.OracleUser = oracleUser;
                SessionContext.Phone = phone;
                SessionContext.Address = address;

                SessionContext.BranchId = branchId;
                SessionContext.BranchCode = branchCode;
                SessionContext.BranchName = branchName;
                SessionContext.BranchAddress = branchAddress;
                SessionContext.BranchPhone = branchPhone;

                SessionContext.BankId = bankId;
                SessionContext.BankCode = bankCode;
                SessionContext.BankName = bankName;
                SessionContext.BankAddress = bankAddress;
                SessionContext.BankPhone = bankPhone;
                SessionContext.BankEmail = bankEmail;

                var headerPanel = headerManager.CreateHeader(this, fullName, roleName);
                this.Controls.Add(headerPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy thông tin profile user.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


           
            
            menuManager = new MenuManager();
            contentManager = new ContentManager();
            CreateMenu();
            CreateContent();
            CreateFooter();
            menuManager.MenuItemClicked += OnMenuItemClicked;
            contentManager.LoadContent("UserManagement");
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
                    SessionContext.Clear(); 
                    Database.Close_Connect(); 
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.ShowDialog();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            headerManager?.Dispose();
            base.OnFormClosed(e);
        }
    }
}