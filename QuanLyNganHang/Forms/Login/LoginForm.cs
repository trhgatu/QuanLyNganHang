using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.Forms.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang
{
    public partial class LoginForm : Form
    {
        // Managers
        private LeftPanelManager leftPanelManager;
        private ConnectionManager connectionManager;
        private LoginValidator loginValidator;
        private SettingsManager settingsManager;
        private StatusManager statusManager;

        // UI Controls
        private Panel rightPanel;
        private Button btn_Exit, btn_Login, btn_ShowConnection;
        private CheckBox chk_ShowPassword, chk_RememberMe;
        private Label lbl_Status;
        private ProgressBar progressBar;
        private TextBox txt_User, txt_Password;

        // Security
        private int loginAttempts = 0;

        // Form dragging
        private bool mouseDown;
        private Point mouseLocation;

        public LoginForm()
        {
            InitializeComponent();
            InitializeManagers();
            InitializeCustomComponents();
            LoadSavedSettings();
        }

        private void InitializeManagers()
        {
            leftPanelManager = new LeftPanelManager();
            connectionManager = new ConnectionManager();
            loginValidator = new LoginValidator();
            settingsManager = new SettingsManager();
        }

        private void InitializeCustomComponents()
        {
            // Form properties
            this.Size = new Size(LoginConstants.Sizes.FormWidth, LoginConstants.Sizes.FormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = LoginConstants.Colors.Background;
            this.Text = "Đăng nhập - Hệ thống Quản lý Ngân hàng";

            CreateLeftPanel();
            CreateRightPanel();
            CreateConnectionPanel();
        }

        private void CreateLeftPanel()
        {
            var leftPanel = leftPanelManager.CreateLeftPanel(this);
            this.Controls.Add(leftPanel);
        }

        private void CreateRightPanel()
        {
            rightPanel = new Panel
            {
                Size = new Size(LoginConstants.Sizes.RightPanelWidth, this.Height),
                Location = new Point(LoginConstants.Sizes.LeftPanelWidth, 0),
                BackColor = Color.White
            };

            CreateRightPanelContent();
            this.Controls.Add(rightPanel);
        }

        private void CreateRightPanelContent()
        {
            // Header
            Label headerLabel = new Label
            {
                Text = LoginConstants.Texts.LoginTitle,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = LoginConstants.Colors.Primary,
                AutoSize = false,
                Size = new Size(500, 50),
                Location = new Point(50, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label welcomeLabel = new Label
            {
                Text = LoginConstants.Texts.WelcomeMessage,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(500, 30),
                Location = new Point(50, 110),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Credentials panel
            Panel credentialsPanel = CreateCredentialsPanel();

            // Buttons panel
            Panel buttonsPanel = CreateButtonsPanel();

            // Connection settings button
            btn_ShowConnection = LoginUIFactory.CreateButton("⚙️ Cài đặt kết nối",
                new Point(75, 500), new Size(200, 35), LoginConstants.Colors.Info,
                (s, e) => connectionManager.Toggle());

            // Remember me checkbox
            chk_RememberMe = LoginUIFactory.CreateCheckBox("Ghi nhớ thông tin đăng nhập",
                new Point(300, 507));

            // Status and progress
            lbl_Status = new Label
            {
                Text = "Sẵn sàng đăng nhập...",
                Font = new Font("Segoe UI", 10),
                ForeColor = LoginConstants.Colors.Success,
                AutoSize = false,
                Size = new Size(500, 25),
                Location = new Point(75, 550),
                TextAlign = ContentAlignment.MiddleLeft
            };

            progressBar = LoginUIFactory.CreateProgressBar(new Point(75, 580), new Size(450, 10));

            // Initialize status manager
            statusManager = new StatusManager(lbl_Status, progressBar);

            rightPanel.Controls.AddRange(new Control[] {
                headerLabel, welcomeLabel, credentialsPanel, buttonsPanel,
                btn_ShowConnection, chk_RememberMe, lbl_Status, progressBar
            });
        }

        private Panel CreateCredentialsPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(75, 180),
                Size = new Size(450, 200),
                BackColor = LoginConstants.Colors.Light,
                Padding = new Padding(30)
            };

            // Username
            Label lblUser = LoginUIFactory.CreateLabel("👤 Tên đăng nhập:", new Point(30, 30));
            txt_User = LoginUIFactory.CreateTextBox(new Point(30, 60));

            // Password
            Label lblPassword = LoginUIFactory.CreateLabel("🔒 Mật khẩu:", new Point(30, 100));
            txt_Password = LoginUIFactory.CreateTextBox(new Point(30, 130), true);

            // Show password checkbox
            chk_ShowPassword = LoginUIFactory.CreateCheckBox("Hiện mật khẩu", new Point(30, 170),
                (s, e) => txt_Password.UseSystemPasswordChar = !chk_ShowPassword.Checked);

            panel.Controls.AddRange(new Control[] {
                lblUser, txt_User, lblPassword, txt_Password, chk_ShowPassword
            });

            return panel;
        }

        private Panel CreateButtonsPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(75, 400),
                Size = new Size(450, 80),
                BackColor = Color.Transparent
            };

            btn_Login = LoginUIFactory.CreateButton("🔑 ĐĂNG NHẬP",
                new Point(30, 15), new Size(200, LoginConstants.Sizes.ButtonHeight),
                LoginConstants.Colors.Success, btn_Login_Click);

            btn_Exit = LoginUIFactory.CreateButton("❌ THOÁT",
                new Point(250, 15), new Size(150, LoginConstants.Sizes.ButtonHeight),
                LoginConstants.Colors.Error, (s, e) => Application.Exit());

            panel.Controls.AddRange(new Control[] { btn_Login, btn_Exit });
            return panel;
        }

        private void CreateConnectionPanel()
        {
            connectionManager.CreateConnectionPanel(rightPanel, btn_ShowConnection);
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            string host = connectionManager.GetHost();
            string port = connectionManager.GetPort();
            string sid = connectionManager.GetSid();
            string user = txt_User.Text;
            string pass = txt_Password.Text;

            if (!loginValidator.ValidateInputs(host, port, sid, user, pass,
                null, null, null, txt_User, txt_Password))
                return;

            statusManager.ShowLoading(true);
            btn_Login.Enabled = false;

            try
            {
                if (loginValidator.AttemptLogin(host, port, sid, user, pass))
                {
                    OracleConnection c = Database.Get_Connect();

                    if (chk_RememberMe.Checked)
                    {
                        settingsManager.SaveLoginCredentials(user);
                    }

                    statusManager.ShowSuccess($"✅ Đăng nhập thành công!\nServer Version: {c.ServerVersion}");
                    await Task.Delay(1500);

                    this.Hide();
                    DashboardForm dashboardForm = new DashboardForm();
                    dashboardForm.ShowDialog();
                   // Database.Close_Connect();
                    this.Close();
                }
                else
                {
                    string statusMessage = loginValidator.GetUserStatus(user);
                    statusManager.ShowError(statusMessage);

                    loginAttempts++;
                    if (loginAttempts >= LoginConstants.MAX_LOGIN_ATTEMPTS)
                    {
                        HandleMaxAttemptsReached();
                    }
                }
            }
            catch (Exception ex)
            {
                statusManager.ShowError($"❌ Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                statusManager.ShowLoading(false);
                btn_Login.Enabled = true;
            }
        }

        private void HandleMaxAttemptsReached()
        {
            btn_Login.Enabled = false;
            statusManager.ShowError("🔒 Đã vượt quá số lần thử. Vui lòng chờ 5 phút.");

            Timer lockoutTimer = new Timer { Interval = 300000 }; // 5 minutes
            lockoutTimer.Tick += (s, e) =>
            {
                loginAttempts = 0;
                btn_Login.Enabled = true;
                statusManager.ShowReady();
                lockoutTimer.Stop();
            };
            lockoutTimer.Start();
        }

        private void LoadSavedSettings()
        {
            connectionManager?.LoadSavedSettings();

            string savedUsername = settingsManager.LoadSavedUsername();
            if (!string.IsNullOrEmpty(savedUsername))
            {
                txt_User.Text = savedUsername;
                chk_RememberMe.Checked = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (chk_RememberMe?.Checked == true && !string.IsNullOrEmpty(txt_User?.Text))
            {
                settingsManager.SaveLoginCredentials(txt_User.Text);
            }
            connectionManager?.SaveConnectionSettings();
            base.OnFormClosing(e);
        }

        // Form dragging functionality
        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
            mouseLocation = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    this.Location.X + e.X - mouseLocation.X,
                    this.Location.Y + e.Y - mouseLocation.Y);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;
            base.OnMouseUp(e);
        }
    }
}
