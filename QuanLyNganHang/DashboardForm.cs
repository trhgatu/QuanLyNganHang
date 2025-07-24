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
        private Panel menuPanel;
        private Panel contentPanel;
        private Panel headerPanel;
        private Panel footerPanel;
        private Label userInfoLabel;
        private Button currentSelectedButton;

        // Menu buttons
        private Button btnUserManagement;
        private Button btnCustomerManagement;
        private Button btnAccountManagement;
        private Button btnTransactionManagement;
        private Button btnPermissionManagement;
        private Button btnAuditLog;
        private Button btnReports;
        private Button btnSettings;
        private Button btnLogout;

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
            this.BackColor = Color.FromArgb(240, 244, 247);

            CreateHeaderPanel();
            CreateMenuPanel();
            CreateContentPanel();
            CreateFooterPanel();

            // Load default content
            LoadUserManagement();
        }

        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Size = new Size(this.Width, 80),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(31, 81, 139),
                Dock = DockStyle.Top
            };

            // Logo and title
            Label titleLabel = new Label
            {
                Text = "NGÂN HÀNG QUỐC GIA",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(400, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // User info
            userInfoLabel = new Label
            {
                Text = $"Xin chào, {GetCurrentUser()} | {GetCurrentRole()}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(300, 25),
                Location = new Point(this.Width - 320, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Current time
            Label timeLabel = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(200, 20),
                Location = new Point(this.Width - 220, 10),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Timer timeTimer = new Timer { Interval = 1000 };
            timeTimer.Tick += (s, e) => timeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timeTimer.Start();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, userInfoLabel, timeLabel });
            this.Controls.Add(headerPanel);
        }

        private void CreateMenuPanel()
        {
            menuPanel = new Panel
            {
                Size = new Size(250, this.Height - 120),
                Location = new Point(0, 80),
                BackColor = Color.FromArgb(52, 73, 94),
                Dock = DockStyle.Left
            };

            // Menu items
            var menuItems = new[]
            {
                new { Text = "👥 Quản lý Người dùng", Action = (Action)LoadUserManagement },
                new { Text = "👤 Quản lý Khách hàng", Action = (Action)LoadCustomerManagement },
                new { Text = "🏦 Quản lý Tài khoản", Action = (Action)LoadAccountManagement },
                new { Text = "💰 Quản lý Giao dịch", Action = (Action)LoadTransactionManagement },
                new { Text = "🔐 Phân quyền", Action = (Action)LoadPermissionManagement },
                new { Text = "📋 Nhật ký Audit", Action = (Action)LoadAuditLog },
                new { Text = "📊 Báo cáo", Action = (Action)LoadReports },
                new { Text = "⚙️ Cài đặt", Action = (Action)LoadSettings }
            };

            Button[] menuButtons = new Button[menuItems.Length + 1];

            for (int i = 0; i < menuItems.Length; i++)
            {
                menuButtons[i] = CreateMenuButton(menuItems[i].Text, new Point(0, i * 60), menuItems[i].Action);
                menuPanel.Controls.Add(menuButtons[i]);
            }

            // Logout button
            btnLogout = CreateMenuButton("🚪 Đăng xuất", new Point(0, menuPanel.Height - 60), LogoutAction);
            btnLogout.BackColor = Color.FromArgb(192, 57, 43);
            btnLogout.Dock = DockStyle.Bottom;
            menuPanel.Controls.Add(btnLogout);

            this.Controls.Add(menuPanel);
        }

        private Button CreateMenuButton(string text, Point location, Action clickAction)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(250, 60),
                Location = location,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);

            btn.Click += (s, e) => {
                // Update button selection
                if (currentSelectedButton != null)
                    currentSelectedButton.BackColor = Color.FromArgb(52, 73, 94);

                btn.BackColor = Color.FromArgb(41, 128, 185);
                currentSelectedButton = btn;

                clickAction?.Invoke();
            };

            return btn;
        }

        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                Location = new Point(250, 80),
                Size = new Size(this.Width - 250, this.Height - 120),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Padding = new Padding(20)
            };

            this.Controls.Add(contentPanel);
        }

        private void CreateFooterPanel()
        {
            footerPanel = new Panel
            {
                Size = new Size(this.Width, 40),
                BackColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Bottom
            };

            Label footerLabel = new Label
            {
                Text = "© 2025 Hệ thống Quản lý Ngân hàng - Phiên bản 1.0.0",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(400, 40),
                Location = new Point(20, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label statusLabel = new Label
            {
                Text = "Hệ thống hoạt động bình thường",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGreen,
                AutoSize = false,
                Size = new Size(200, 40),
                Location = new Point(this.Width - 220, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            footerPanel.Controls.AddRange(new Control[] { footerLabel, statusLabel });
            this.Controls.Add(footerPanel);
        }

        // Content loading methods
        private void LoadUserManagement()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("👥 QUẢN LY NGƯỜI DÙNG HỆ THỐNG");
            contentPanel.Controls.Add(titleLabel);

            // Quick stats
            Panel statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng Admin", "5", Color.FromArgb(231, 76, 60)),
                ("Tổng Nhân viên", "25", Color.FromArgb(52, 152, 219)),
                ("Đang hoạt động", "28", Color.FromArgb(46, 204, 113)),
                ("Bị khóa", "2", Color.FromArgb(241, 196, 15))
            });
            contentPanel.Controls.Add(statsPanel);

            // Action buttons
            Panel actionPanel = CreateActionPanel(new[]
            {
                ("Thêm Admin", Color.FromArgb(231, 76, 60)),
                ("Thêm Nhân viên", Color.FromArgb(52, 152, 219)),
                ("Phân quyền", Color.FromArgb(155, 89, 182)),
                ("Đặt lại mật khẩu", Color.FromArgb(241, 196, 15))
            });
            contentPanel.Controls.Add(actionPanel);

            // Data grid
            DataGridView dgv = CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320);

            // Sample data for user management
            dgv.Columns.Add("ID", "ID");
            dgv.Columns.Add("Username", "Tên đăng nhập");
            dgv.Columns.Add("FullName", "Họ tên");
            dgv.Columns.Add("Role", "Vai trò");
            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns.Add("LastLogin", "Đăng nhập cuối");

            contentPanel.Controls.Add(dgv);
        }

        private void LoadCustomerManagement()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("👤 QUẢN LY KHÁCH HÀNG");
            contentPanel.Controls.Add(titleLabel);

            Panel statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng KH", "1,234", Color.FromArgb(52, 152, 219)),
                ("KH VIP", "89", Color.FromArgb(241, 196, 15)),
                ("KH Thường", "1,145", Color.FromArgb(46, 204, 113)),
                ("KH Bị khóa", "15", Color.FromArgb(231, 76, 60))
            });
            contentPanel.Controls.Add(statsPanel);

            Panel actionPanel = CreateActionPanel(new[]
            {
                ("Thêm KH mới", Color.FromArgb(46, 204, 113)),
                ("Import Excel", Color.FromArgb(52, 152, 219)),
                ("Export dữ liệu", Color.FromArgb(155, 89, 182)),
                ("Tìm kiếm nâng cao", Color.FromArgb(241, 196, 15))
            });
            contentPanel.Controls.Add(actionPanel);

            DataGridView dgv = CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320);

            dgv.Columns.Add("CustomerID", "Mã KH");
            dgv.Columns.Add("FullName", "Họ tên");
            dgv.Columns.Add("IDCard", "CMND/CCCD");
            dgv.Columns.Add("Phone", "Điện thoại");
            dgv.Columns.Add("Email", "Email");
            dgv.Columns.Add("Address", "Địa chỉ");
            dgv.Columns.Add("Status", "Trạng thái");

            contentPanel.Controls.Add(dgv);
        }

        private void LoadAccountManagement()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("🏦 QUẢN LY TÀI KHOẢN NGÂN HÀNG");
            contentPanel.Controls.Add(titleLabel);

            Panel statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng TK", "2,456", Color.FromArgb(52, 152, 219)),
                ("TK Hoạt động", "2,398", Color.FromArgb(46, 204, 113)),
                ("TK Đóng băng", "58", Color.FromArgb(241, 196, 15)),
                ("TK Đã đóng", "12", Color.FromArgb(231, 76, 60))
            });
            contentPanel.Controls.Add(statsPanel);

            Panel actionPanel = CreateActionPanel(new[]
            {
                ("Mở TK mới", Color.FromArgb(46, 204, 113)),
                ("Đóng băng TK", Color.FromArgb(241, 196, 15)),
                ("Kích hoạt TK", Color.FromArgb(52, 152, 219)),
                ("Đóng TK", Color.FromArgb(231, 76, 60))
            });
            contentPanel.Controls.Add(actionPanel);

            DataGridView dgv = CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320);

            dgv.Columns.Add("AccountNumber", "Số TK");
            dgv.Columns.Add("CustomerName", "Chủ TK");
            dgv.Columns.Add("AccountType", "Loại TK");
            dgv.Columns.Add("Balance", "Số dư");
            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns.Add("OpenDate", "Ngày mở");

            contentPanel.Controls.Add(dgv);
        }

        private void LoadTransactionManagement()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("💰 QUẢN LY GIAO DỊCH TÀI CHÍNH");
            contentPanel.Controls.Add(titleLabel);

            Panel statsPanel = CreateStatsPanel(new[]
            {
                ("GD hôm nay", "1,456", Color.FromArgb(52, 152, 219)),
                ("Tổng tiền vào", "15.8 tỷ", Color.FromArgb(46, 204, 113)),
                ("Tổng tiền ra", "12.3 tỷ", Color.FromArgb(231, 76, 60)),
                ("GD chờ duyệt", "23", Color.FromArgb(241, 196, 15))
            });
            contentPanel.Controls.Add(statsPanel);

            Panel actionPanel = CreateActionPanel(new[]
            {
                ("Nạp tiền", Color.FromArgb(46, 204, 113)),
                ("Rút tiền", Color.FromArgb(231, 76, 60)),
                ("Chuyển khoản", Color.FromArgb(52, 152, 219)),
                ("Duyệt GD", Color.FromArgb(241, 196, 15))
            });
            contentPanel.Controls.Add(actionPanel);

            DataGridView dgv = CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320);

            dgv.Columns.Add("TransactionID", "Mã GD");
            dgv.Columns.Add("TransactionType", "Loại GD");
            dgv.Columns.Add("AccountNumber", "Số TK");
            dgv.Columns.Add("Amount", "Số tiền");
            dgv.Columns.Add("DateTime", "Thời gian");
            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns.Add("Employee", "NV thực hiện");

            contentPanel.Controls.Add(dgv);
        }

        private void LoadPermissionManagement()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("🔐 QUẢN LY PHÂN QUYỀN & KIỂM SOÁT TRUY CẬP");
            contentPanel.Controls.Add(titleLabel);

            // Permission tabs
            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 100),
                Font = new Font("Segoe UI", 10)
            };

            // DAC Tab
            TabPage dacTab = new TabPage("DAC - Discretionary Access Control");
            dacTab.Controls.Add(new Label { Text = "Quản lý quyền truy cập dữ liệu theo nhân viên", Location = new Point(20, 20), Font = new Font("Segoe UI", 12) });

            // MAC Tab
            TabPage macTab = new TabPage("MAC - Mandatory Access Control");
            macTab.Controls.Add(new Label { Text = "Gắn nhãn bảo mật cho dữ liệu nhạy cảm", Location = new Point(20, 20), Font = new Font("Segoe UI", 12) });

            // RBAC Tab
            TabPage rbacTab = new TabPage("RBAC - Role-Based Access Control");
            rbacTab.Controls.Add(new Label { Text = "Phân quyền theo vai trò (Admin vs Nhân viên)", Location = new Point(20, 20), Font = new Font("Segoe UI", 12) });

            // VPD Tab
            TabPage vpdTab = new TabPage("VPD - Virtual Private Database");
            vpdTab.Controls.Add(new Label { Text = "Giới hạn dữ liệu theo người dùng", Location = new Point(20, 20), Font = new Font("Segoe UI", 12) });

            // OLS Tab
            TabPage olsTab = new TabPage("OLS - Oracle Label Security");
            olsTab.Controls.Add(new Label { Text = "Kiểm soát truy cập dữ liệu bằng nhãn", Location = new Point(20, 20), Font = new Font("Segoe UI", 12) });

            tabControl.TabPages.AddRange(new TabPage[] { dacTab, macTab, rbacTab, vpdTab, olsTab });
            contentPanel.Controls.Add(tabControl);
        }

        private void LoadAuditLog()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("📋 NHẬT KÝ AUDIT & GIẢI TRÌNH");
            contentPanel.Controls.Add(titleLabel);

            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 100),
                Font = new Font("Segoe UI", 10)
            };

            // Standard Auditing
            TabPage standardTab = new TabPage("Standard Auditing");
            DataGridView standardDgv = CreateDataGrid();
            standardDgv.Size = new Size(tabControl.Width - 20, tabControl.Height - 60);
            standardDgv.Location = new Point(10, 10);
            standardDgv.Columns.Add("LogID", "ID");
            standardDgv.Columns.Add("UserName", "Người dùng");
            standardDgv.Columns.Add("Action", "Hành động");
            standardDgv.Columns.Add("DateTime", "Thời gian");
            standardDgv.Columns.Add("IPAddress", "IP Address");
            standardTab.Controls.Add(standardDgv);

            // Fine-Grained Auditing
            TabPage fgaTab = new TabPage("Fine-Grained Auditing");
            DataGridView fgaDgv = CreateDataGrid();
            fgaDgv.Size = new Size(tabControl.Width - 20, tabControl.Height - 60);
            fgaDgv.Location = new Point(10, 10);
            fgaDgv.Columns.Add("LogID", "ID");
            fgaDgv.Columns.Add("TableName", "Bảng");
            fgaDgv.Columns.Add("Operation", "Thao tác");
            fgaDgv.Columns.Add("UserName", "Người dùng");
            fgaDgv.Columns.Add("DateTime", "Thời gian");
            fgaDgv.Columns.Add("Details", "Chi tiết");
            fgaTab.Controls.Add(fgaDgv);

            // Trigger Logs
            TabPage triggerTab = new TabPage("Trigger Logs");
            DataGridView triggerDgv = CreateDataGrid();
            triggerDgv.Size = new Size(tabControl.Width - 20, tabControl.Height - 60);
            triggerDgv.Location = new Point(10, 10);
            triggerDgv.Columns.Add("TriggerName", "Trigger");
            triggerDgv.Columns.Add("TableName", "Bảng");
            triggerDgv.Columns.Add("Operation", "Thao tác");
            triggerDgv.Columns.Add("OldValue", "Giá trị cũ");
            triggerDgv.Columns.Add("NewValue", "Giá trị mới");
            triggerDgv.Columns.Add("DateTime", "Thời gian");
            triggerTab.Controls.Add(triggerDgv);

            tabControl.TabPages.AddRange(new TabPage[] { standardTab, fgaTab, triggerTab });
            contentPanel.Controls.Add(tabControl);
        }

        private void LoadReports()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("📊 BÁO CÁO & THỐNG KÊ");
            contentPanel.Controls.Add(titleLabel);

            // Report categories
            Panel reportPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, 200),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var reportButtons = new[]
            {
                ("📈 Thống kê khách hàng", new Point(20, 20)),
                ("💹 Thống kê giao dịch", new Point(200, 20)),
                ("👥 Thống kê nhân viên", new Point(380, 20)),
                ("🔍 Nhật ký hệ thống", new Point(560, 20)),
                ("💰 Báo cáo tài chính", new Point(20, 100)),
                ("🛡️ Báo cáo bảo mật", new Point(200, 100)),
                ("📋 Báo cáo tùy chỉnh", new Point(380, 100)),
                ("📤 Xuất báo cáo", new Point(560, 100))
            };

            foreach (var (text, location) in reportButtons)
            {
                Button btn = new Button
                {
                    Text = text,
                    Size = new Size(160, 60),
                    Location = location,
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                reportPanel.Controls.Add(btn);
            }

            contentPanel.Controls.Add(reportPanel);

            // Sample chart area
            Panel chartPanel = new Panel
            {
                Location = new Point(20, 300),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label chartLabel = new Label
            {
                Text = "📊 Biểu đồ thống kê sẽ hiển thị tại đây",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = chartPanel.Size,
                TextAlign = ContentAlignment.MiddleCenter
            };

            chartPanel.Controls.Add(chartLabel);
            contentPanel.Controls.Add(chartPanel);
        }

        private void LoadSettings()
        {
            contentPanel.Controls.Clear();

            Label titleLabel = CreateContentTitle("⚙️ CÀI ĐẶT HỆ THỐNG");
            contentPanel.Controls.Add(titleLabel);

            TabControl settingsTab = new TabControl
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 100),
                Font = new Font("Segoe UI", 10)
            };

            // Security Settings
            TabPage securityTab = new TabPage("Bảo mật");
            var securityControls = new[]
            {
                "🔐 Cấu hình mã hóa AES",
                "🔑 Chính sách mật khẩu",
                "⏰ Thời gian phiên làm việc",
                "🛡️ Cấu hình tường lửa",
                "📱 Xác thực 2 lớp (2FA)"
            };

            for (int i = 0; i < securityControls.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = securityControls[i],
                    Location = new Point(20, 20 + i * 40),
                    Size = new Size(300, 30),
                    Font = new Font("Segoe UI", 11)
                };
                securityTab.Controls.Add(lbl);
            }

            // Database Settings
            TabPage dbTab = new TabPage("Cơ sở dữ liệu");
            dbTab.Controls.Add(new Label { Text = "🗄️ Cấu hình kết nối Oracle", Location = new Point(20, 20), Font = new Font("Segoe UI", 11) });
            dbTab.Controls.Add(new Label { Text = "🔄 Backup tự động", Location = new Point(20, 60), Font = new Font("Segoe UI", 11) });
            dbTab.Controls.Add(new Label { Text = "📊 Tối ưu hóa hiệu suất", Location = new Point(20, 100), Font = new Font("Segoe UI", 11) });

            // System Settings
            TabPage systemTab = new TabPage("Hệ thống");
            systemTab.Controls.Add(new Label { Text = "🌐 Cấu hình mạng", Location = new Point(20, 20), Font = new Font("Segoe UI", 11) });
            systemTab.Controls.Add(new Label { Text = "📝 Cấu hình log", Location = new Point(20, 60), Font = new Font("Segoe UI", 11) });
            systemTab.Controls.Add(new Label { Text = "⚡ Cấu hình hiệu suất", Location = new Point(20, 100), Font = new Font("Segoe UI", 11) });

            settingsTab.TabPages.AddRange(new TabPage[] { securityTab, dbTab, systemTab });
            contentPanel.Controls.Add(settingsTab);
        }

        // Helper methods
        private Label CreateContentTitle(string title)
        {
            return new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = false,
                Size = new Size(contentPanel.Width - 40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private Panel CreateStatsPanel((string title, string value, Color color)[] stats)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, 100),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - 60) / stats.Length;

            for (int i = 0; i < stats.Length; i++)
            {
                Panel statCard = new Panel
                {
                    Location = new Point(i * (width + 15), 0),
                    Size = new Size(width, 100),
                    BackColor = stats[i].color,
                    Padding = new Padding(15)
                };

                Label valueLabel = new Label
                {
                    Text = stats[i].value,
                    Font = new Font("Segoe UI", 20, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(statCard.Width - 30, 40),
                    Location = new Point(15, 15),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label titleLabel = new Label
                {
                    Text = stats[i].title,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(statCard.Width - 30, 30),
                    Location = new Point(15, 55),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                statCard.Controls.AddRange(new Control[] { valueLabel, titleLabel });
                panel.Controls.Add(statCard);
            }

            return panel;
        }

        private Panel CreateActionPanel((string text, Color color)[] actions)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 200),
                Size = new Size(contentPanel.Width - 40, 80),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - 60) / actions.Length;

            for (int i = 0; i < actions.Length; i++)
            {
                Button btn = new Button
                {
                    Text = actions[i].text,
                    Location = new Point(i * (width + 15), 10),
                    Size = new Size(width, 60),
                    BackColor = actions[i].color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                panel.Controls.Add(btn);
            }

            return panel;
        }

        private DataGridView CreateDataGrid()
        {
            return new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 10)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private string GetCurrentUser()
        {
            // Replace with actual user retrieval logic
            return "Nguyễn Văn A";
        }

        private string GetCurrentRole()
        {
            // Replace with actual role retrieval logic
            return "Quản trị viên";
        }

        private void LogoutAction()
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                // Show login form
                // LoginForm loginForm = new LoginForm();
                // loginForm.Show();
            }
        }
    }
}
