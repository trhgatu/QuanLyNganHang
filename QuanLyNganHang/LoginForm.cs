using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.Drawing.Drawing2D;

namespace QuanLyNganHang
{
    public partial class LoginForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private Button btn_Exit;
        private CheckBox chk_ShowPassword;
        private CheckBox chk_RememberMe;
        private Label lbl_Status;
        private ProgressBar progressBar;

        // Connection settings panel
        private Panel connectionPanel;
        private Button btn_ShowConnection;
        private bool isConnectionVisible = false;

        // Security features
        private int loginAttempts = 0;
        private const int MAX_LOGIN_ATTEMPTS = 3;
        private Timer lockoutTimer;

        public LoginForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadSavedSettings();
        }

        private void InitializeCustomComponents()
        {
            // Form properties
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(240, 244, 247);
            this.Text = "Đăng nhập - Hệ thống Quản lý Ngân hàng";

            CreateLeftPanel();
            CreateRightPanel();
            CreateConnectionPanel();
        }

        private void CreateLeftPanel()
        {
            leftPanel = new Panel
            {
                Size = new Size(400, this.Height),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(31, 81, 139)
            };

            // Bank logo and info
            PictureBox logo = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(150, 80),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Create a simple bank icon
            Bitmap bankIcon = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(bankIcon))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(Color.White), 10, 10, 80, 80);
                g.FillRectangle(new SolidBrush(Color.FromArgb(31, 81, 139)), 25, 35, 50, 30);
                g.DrawString("🏦", new Font("Segoe UI Symbol", 24), Brushes.White, 35, 30);
            }
            logo.Image = bankIcon;

            Label titleLabel = new Label
            {
                Text = "NGÂN HÀNG QUỐC GIA",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(350, 40),
                Location = new Point(25, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label subtitleLabel = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ CORE BANKING",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(350, 30),
                Location = new Point(25, 245),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Security features info
            Panel securityPanel = new Panel
            {
                Location = new Point(25, 350),
                Size = new Size(350, 200),
                BackColor = Color.FromArgb(41, 98, 159)
            };

            Label securityTitle = new Label
            {
                Text = "🔒 TÍNH NĂNG BẢO MẬT",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            string[] securityFeatures = {
                "🔐 Mã hóa AES 256-bit",
                "🛡️ Xác thực đa lớp",
                "📋 Audit Trail đầy đủ",
                "⏰ Session timeout tự động",
                "🚫 Khóa tài khoản sau 3 lần sai"
            };

            for (int i = 0; i < securityFeatures.Length; i++)
            {
                Label feature = new Label
                {
                    Text = securityFeatures[i],
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.LightBlue,
                    Location = new Point(20, 55 + (i * 25)),
                    AutoSize = true
                };
                securityPanel.Controls.Add(feature);
            }

            securityPanel.Controls.Add(securityTitle);

            leftPanel.Controls.AddRange(new Control[] {
                logo, titleLabel, subtitleLabel, securityPanel
            });

            this.Controls.Add(leftPanel);
        }

        private void CreateRightPanel()
        {
            rightPanel = new Panel
            {
                Size = new Size(600, this.Height),
                Location = new Point(400, 0),
                BackColor = Color.White
            };

            // Login form header
            Label headerLabel = new Label
            {
                Text = "ĐĂNG NHẬP HỆ THỐNG",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 81, 139),
                AutoSize = false,
                Size = new Size(500, 50),
                Location = new Point(50, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label welcomeLabel = new Label
            {
                Text = "Vui lòng nhập thông tin đăng nhập của bạn",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(500, 30),
                Location = new Point(50, 110),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // User credentials panel
            Panel credentialsPanel = new Panel
            {
                Location = new Point(75, 180),
                Size = new Size(450, 200),
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(30)
            };

            // Username
            Label lblUser = CreateLabel("👤 Tên đăng nhập:", new Point(30, 30));
            txt_User = CreateTextBox(new Point(30, 60), false);
            txt_User.Text = ""; // Default for demo

            // Password
            Label lblPassword = CreateLabel("🔒 Mật khẩu:", new Point(30, 100));
            txt_Password = CreateTextBox(new Point(30, 130), true);

            // Show password checkbox
            chk_ShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Location = new Point(30, 170),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            chk_ShowPassword.CheckedChanged += (s, e) => {
                txt_Password.UseSystemPasswordChar = !chk_ShowPassword.Checked;
            };

            credentialsPanel.Controls.AddRange(new Control[] {
                lblUser, txt_User, lblPassword, txt_Password, chk_ShowPassword
            });

            // Buttons panel
            Panel buttonsPanel = new Panel
            {
                Location = new Point(75, 400),
                Size = new Size(450, 80),
                BackColor = Color.Transparent
            };

            btn_Login = new Button
            {
                Text = "🔑 ĐĂNG NHẬP",
                Size = new Size(200, 50),
                Location = new Point(30, 15),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn_Login.FlatAppearance.BorderSize = 0;
            btn_Login.Click += btn_Login_Click;

            btn_Exit = new Button
            {
                Text = "❌ THOÁT",
                Size = new Size(150, 50),
                Location = new Point(250, 15),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn_Exit.FlatAppearance.BorderSize = 0;
            btn_Exit.Click += (s, e) => Application.Exit();

            buttonsPanel.Controls.AddRange(new Control[] { btn_Login, btn_Exit });

            // Connection settings button
            btn_ShowConnection = new Button
            {
                Text = "⚙️ Cài đặt kết nối",
                Size = new Size(200, 35),
                Location = new Point(75, 500),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btn_ShowConnection.FlatAppearance.BorderSize = 0;
            btn_ShowConnection.Click += btn_ShowConnection_Click;

            // Remember me checkbox
            chk_RememberMe = new CheckBox
            {
                Text = "Ghi nhớ thông tin đăng nhập",
                Location = new Point(300, 507),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            // Status label
            lbl_Status = new Label
            {
                Text = "Sẵn sàng đăng nhập...",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(46, 204, 113),
                AutoSize = false,
                Size = new Size(500, 25),
                Location = new Point(75, 550),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(75, 580),
                Size = new Size(450, 10),
                Style = ProgressBarStyle.Continuous,
                Visible = false
            };

            rightPanel.Controls.AddRange(new Control[] {
                headerLabel, welcomeLabel, credentialsPanel, buttonsPanel,
                btn_ShowConnection, chk_RememberMe, lbl_Status, progressBar
            });

            this.Controls.Add(rightPanel);
        }

        private void CreateConnectionPanel()
        {
            connectionPanel = new Panel
            {
                Size = new Size(450, 280),
                Location = new Point(75, 180),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                Padding = new Padding(20)
            };

            Label connTitle = new Label
            {
                Text = "⚙️ CÀI ĐẶT KẾT NỐI ORACLE",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 81, 139),
                Location = new Point(20, 15),
                AutoSize = true
            };

            // Host
            Label lblHost = CreateLabel("🖥️ Host:", new Point(20, 55));
            txt_Host = CreateTextBox(new Point(20, 80), false);
            txt_Host.Text = "localhost"; // Default

            // Port
            Label lblPort = CreateLabel("🔌 Port:", new Point(240, 55));
            txt_Port = CreateTextBox(new Point(240, 80), false);
            txt_Port.Size = new Size(150, 30);
            txt_Port.Text = "1521"; // Default

            // SID
            Label lblSid = CreateLabel("🗄️ SID:", new Point(20, 120));
            txt_Sid = CreateTextBox(new Point(20, 145), false);
            txt_Sid.Text = "XE"; // Default

            // Test connection button
            Button btn_TestConnection = new Button
            {
                Text = "🔍 Kiểm tra kết nối",
                Size = new Size(180, 35),
                Location = new Point(20, 190),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btn_TestConnection.FlatAppearance.BorderSize = 0;
            btn_TestConnection.Click += btn_TestConnection_Click;

            // Save settings button
            Button btn_SaveSettings = new Button
            {
                Text = "💾 Lưu cài đặt",
                Size = new Size(150, 35),
                Location = new Point(220, 190),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btn_SaveSettings.FlatAppearance.BorderSize = 0;
            btn_SaveSettings.Click += btn_SaveSettings_Click;

            // Close button
            Button btn_CloseConnection = new Button
            {
                Text = "✖",
                Size = new Size(30, 30),
                Location = new Point(400, 15),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn_CloseConnection.FlatAppearance.BorderSize = 0;
            btn_CloseConnection.Click += (s, e) => ToggleConnectionPanel();

            connectionPanel.Controls.AddRange(new Control[] {
                connTitle, lblHost, txt_Host, lblPort, txt_Port, lblSid, txt_Sid,
                btn_TestConnection, btn_SaveSettings, btn_CloseConnection
            });

            rightPanel.Controls.Add(connectionPanel);
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true
            };
        }

        private TextBox CreateTextBox(Point location, bool isPassword)
        {
            TextBox txt = new TextBox
            {
                Location = location,
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            if (isPassword)
            {
                txt.UseSystemPasswordChar = true;
            }

            // Add focus effects
            txt.Enter += (s, e) => ((TextBox)s).BackColor = Color.FromArgb(235, 245, 255);
            txt.Leave += (s, e) => ((TextBox)s).BackColor = Color.White;

            return txt;
        }

        // Event handlers
        private void btn_ShowConnection_Click(object sender, EventArgs e)
        {
            ToggleConnectionPanel();
        }

        private void ToggleConnectionPanel()
        {
            isConnectionVisible = !isConnectionVisible;
            connectionPanel.Visible = isConnectionVisible;

            if (isConnectionVisible)
            {
                connectionPanel.BringToFront();
                btn_ShowConnection.Text = "🔙 Quay lại đăng nhập";
            }
            else
            {
                btn_ShowConnection.Text = "⚙️ Cài đặt kết nối";
            }
        }

        private void btn_TestConnection_Click(object sender, EventArgs e)
        {
            string host = txt_Host.Text;
            string port = txt_Port.Text;
            string sid = txt_Sid.Text;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(sid))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin kết nối!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string testConnectionString = $"Data Source={host}:{port}/{sid};User Id=sys;Password=temp;DBA Privilege=SYSDBA;";
                using (OracleConnection conn = new OracleConnection(testConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Kết nối thành công!\nServer Version: " + conn.ServerVersion,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Kết nối thất bại!\n{ex.Message}", "Lỗi kết nối",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_SaveSettings_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            MessageBox.Show("💾 Đã lưu cài đặt kết nối!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Your existing login logic with enhancements
        bool Check_Textbox(string host, string port, string sid, string user, string pass)
        {
            var validations = new[]
            {
                (value: host, message: "Chưa điền thông tin Host", control: txt_Host),
                (value: port, message: "Chưa điền thông tin Port", control: txt_Port),
                (value: sid, message: "Chưa điền thông tin SID", control: txt_Sid),
                (value: user, message: "Chưa điền thông tin User", control: txt_User),
                (value: pass, message: "Chưa điền mật khẩu", control: txt_Password)
            };

            foreach (var validation in validations)
            {
                if (string.IsNullOrEmpty(validation.value))
                {
                    ShowError(validation.message);
                    validation.control.Focus();
                    return false;
                }
            }
            return true;
        }

        void Check_Status(string user)
        {
            string status = Database.Get_Status(user);
            string v;
            if (status == "LOCKED" || status == "LOCKED(TIMED)")
                v = "🔒 Tài khoản bị khóa";
            else if (status == "EXPIRED(GRACE)")
                v = "⚠️ Tài khoản sắp hết hạn";
            else if (status == "EXPIRED & LOCKED(TIMED)")
                v = "🔒 Tài khoản bị khóa do hết hạn";
            else if (status == "EXPIRED")
                v = "⏰ Tài khoản hết hạn";
            else if (status == " ")
                v = "❌ Tài khoản không tồn tại";
            else
                v = "❌ Đăng nhập thất bại!\nKiểm tra lại thông tin đăng nhập";
            string message = v;

            ShowError(message);
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            if (loginAttempts >= MAX_LOGIN_ATTEMPTS)
            {
                ShowError("🚫 Tài khoản tạm thời bị khóa do quá nhiều lần đăng nhập sai!");
                return;
            }

            string host = txt_Host.Text;
            string port = txt_Port.Text;
            string sid = txt_Sid.Text;
            string user = txt_User.Text;
            string pass = txt_Password.Text;

            if (Check_Textbox(host, port, sid, user, pass))
            {
                // Show loading
                ShowLoading(true);
                btn_Login.Enabled = false;

                try
                {
                    await Task.Run(() =>
                    {
                        Database.Set_Database(host, port, sid, user, pass);
                        return Database.Connect();
                    });

                    if (Database.Connect())
                    {
                        OracleConnection c = Database.Get_Connect();

                        // Log successful login
                        LogLoginAttempt(user, true);

                        // Save settings if remember me is checked
                        if (chk_RememberMe.Checked)
                        {
                            SaveLoginCredentials(user);
                        }

                        ShowSuccess($"✅ Đăng nhập thành công!\nServer Version: {c.ServerVersion}");

                        await Task.Delay(1500); // Show success message briefly

                        Database.Close_Connect();
                        this.Hide();

                        DashboardForm dashboardForm = new DashboardForm();
                        dashboardForm.ShowDialog();

                        this.Close();
                    }
                    else
                    {
                        loginAttempts++;
                        LogLoginAttempt(user, false);
                        Check_Status(user);

                        if (loginAttempts >= MAX_LOGIN_ATTEMPTS)
                        {
                            StartLockoutTimer();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"❌ Lỗi kết nối: {ex.Message}");
                }
                finally
                {
                    ShowLoading(false);
                    btn_Login.Enabled = true;
                }
            }
        }

        // UI Helper methods
        private void ShowLoading(bool show)
        {
            progressBar.Visible = show;
            if (show)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                lbl_Status.Text = "🔄 Đang xác thực...";
                lbl_Status.ForeColor = Color.FromArgb(52, 152, 219);
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }
        }

        private void ShowError(string message)
        {
            lbl_Status.Text = message;
            lbl_Status.ForeColor = Color.FromArgb(231, 76, 60);

            // Flash effect
            Timer flashTimer = new Timer { Interval = 200 };
            int flashCount = 0;
            flashTimer.Tick += (s, e) =>
            {
                lbl_Status.Visible = !lbl_Status.Visible;
                flashCount++;
                if (flashCount >= 6)
                {
                    flashTimer.Stop();
                    lbl_Status.Visible = true;
                }
            };
            flashTimer.Start();
        }

        private void ShowSuccess(string message)
        {
            lbl_Status.Text = message;
            lbl_Status.ForeColor = Color.FromArgb(46, 204, 113);
        }

        // Security and persistence methods
        private void StartLockoutTimer()
        {
            lockoutTimer = new Timer { Interval = 300000 }; // 5 minutes
            lockoutTimer.Tick += (s, e) =>
            {
                loginAttempts = 0;
                lockoutTimer.Stop();
                ShowSuccess("🔓 Tài khoản đã được mở khóa. Bạn có thể thử đăng nhập lại.");
            };
            lockoutTimer.Start();
        }

        private void LogLoginAttempt(string username, bool success)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] User: {username}, " +
                                $"Success: {success}, IP: {GetLocalIP()}";
                System.IO.File.AppendAllText("login_log.txt", logEntry + Environment.NewLine);
            }
            catch { /* Log errors silently */ }
        }

        private string GetLocalIP()
        {
            return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
                .AddressList.FirstOrDefault(ip => ip.AddressFamily ==
                System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "Unknown";
        }

        private void SaveConnectionSettings()
        {
            try
            {
                var settings = new[]
                {
                    $"Host={txt_Host.Text}",
                    $"Port={txt_Port.Text}",
                    $"SID={txt_Sid.Text}"
                };
                System.IO.File.WriteAllLines("connection_settings.cfg", settings);
            }
            catch { /* Handle silently */ }
        }

        private void LoadSavedSettings()
        {
            try
            {
                if (System.IO.File.Exists("connection_settings.cfg"))
                {
                    var lines = System.IO.File.ReadAllLines("connection_settings.cfg");
                    foreach (var line in lines)
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            switch (parts[0])
                            {
                                case "Host": txt_Host.Text = parts[1]; break;
                                case "Port": txt_Port.Text = parts[1]; break;
                                case "SID": txt_Sid.Text = parts[1]; break;
                            }
                        }
                    }
                }

                if (System.IO.File.Exists("user_settings.cfg") && chk_RememberMe != null)
                {
                    var userSettings = System.IO.File.ReadAllLines("user_settings.cfg");
                    if (userSettings.Length > 0)
                    {
                        txt_User.Text = userSettings[0];
                        chk_RememberMe.Checked = true;
                    }
                }
            }
            catch { /* Handle silently */ }
        }

        private void SaveLoginCredentials(string username)
        {
            try
            {
                System.IO.File.WriteAllText("user_settings.cfg", username);
            }
            catch { /* Handle silently */ }
        }

        // Form events
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (chk_RememberMe?.Checked == true && !string.IsNullOrEmpty(txt_User?.Text))
            {
                SaveLoginCredentials(txt_User.Text);
            }
            SaveConnectionSettings();
            base.OnFormClosing(e);
        }

        // Allow dragging the form
        private bool mouseDown;
        private Point mouseLocation;

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
