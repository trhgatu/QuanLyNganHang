using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public class ContentManager
    {
        private Panel contentPanel;

        public Panel CreateContentPanel(Form parentForm)
        {
            contentPanel = new Panel
            {
                Location = new Point(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.HeaderHeight),
                Size = new Size(parentForm.Width - DashboardConstants.Sizes.MenuPanelWidth,
                               parentForm.Height - DashboardConstants.Sizes.HeaderHeight - DashboardConstants.Sizes.FooterHeight),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Padding = new Padding(20),
                AutoScroll = true
            };

            return contentPanel;
        }

        public void LoadContent(string contentType)
        {
            contentPanel.Controls.Clear();

            switch (contentType)
            {
                case "UserManagement":
                    LoadUserManagement();
                    break;
                case "CustomerManagement":
                    LoadCustomerManagement();
                    break;
                case "AccountManagement":
                    LoadAccountManagement();
                    break;
                case "TransactionManagement":
                    LoadTransactionManagement();
                    break;
                case "PermissionManagement":
                    LoadPermissionManagement();
                    break;
                case "AuditLog":
                    LoadAuditLog();
                    break;
                case "Reports":
                    LoadReports();
                    break;
                case "Settings":
                    LoadSettings();
                    break;
                default:
                   LoadReports();
                    break;
            }
        }

        private void LoadUserManagement()
        {
            var title = DashboardUIFactory.CreateTitle("👥 QUẢN LÝ NGƯỜI DÙNG HỆ THỐNG", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng Admin", "5", DashboardConstants.Colors.Danger),
                ("Tổng Nhân viên", "25", DashboardConstants.Colors.Info),
                ("Đang hoạt động", "28", DashboardConstants.Colors.Success),
                ("Bị khóa", "2", DashboardConstants.Colors.Warning)
            });
            contentPanel.Controls.Add(statsPanel);

            var actionPanel = CreateActionPanel(new[]
            {
                ("Thêm Admin", DashboardConstants.Colors.Danger, (Action)(() => ShowMessage("Thêm Admin"))),
                ("Profile", DashboardConstants.Colors.Info, (Action)(() => ShowProfileForm())),
                ("Thêm Nhân viên", DashboardConstants.Colors.Success, (Action)(() => ShowMessage("Thêm Nhân viên"))),
                ("Phân quyền", DashboardConstants.Colors.Warning, (Action)(() => ShowMessage("Phân quyền")))
            });
            contentPanel.Controls.Add(actionPanel);

            AddDataGrid(new[] { "ID", "Username", "FullName", "Role", "Status", "LastLogin" },
                       new[] { "ID", "Tên đăng nhập", "Họ tên", "Vai trò", "Trạng thái", "Đăng nhập cuối" });
        }

        private void LoadCustomerManagement()
        {
            var title = DashboardUIFactory.CreateTitle("👤 QUẢN LÝ KHÁCH HÀNG", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng KH", "1,234", DashboardConstants.Colors.Info),
                ("KH VIP", "89", DashboardConstants.Colors.Warning),
                ("KH Thường", "1,145", DashboardConstants.Colors.Success),
                ("KH Bị khóa", "15", DashboardConstants.Colors.Danger)
            });
            contentPanel.Controls.Add(statsPanel);

            AddDataGrid(new[] { "CustomerID", "FullName", "IDCard", "Phone", "Email", "Address", "Status" },
                       new[] { "Mã KH", "Họ tên", "CMND/CCCD", "Điện thoại", "Email", "Địa chỉ", "Trạng thái" });
        }

        private void LoadAccountManagement()
        {
            var title = DashboardUIFactory.CreateTitle("🏦 QUẢN LÝ TÀI KHOẢN NGÂN HÀNG", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng TK", "2,456", DashboardConstants.Colors.Info),
                ("TK Hoạt động", "2,398", DashboardConstants.Colors.Success),
                ("TK Đóng băng", "58", DashboardConstants.Colors.Warning),
                ("TK Đã đóng", "12", DashboardConstants.Colors.Danger)
            });
            contentPanel.Controls.Add(statsPanel);

            AddDataGrid(new[] { "AccountNumber", "CustomerName", "AccountType", "Balance", "Status", "OpenDate" },
                       new[] { "Số TK", "Chủ TK", "Loại TK", "Số dư", "Trạng thái", "Ngày mở" });
        }

        private void LoadTransactionManagement()
        {
            var title = DashboardUIFactory.CreateTitle("💰 QUẢN LÝ GIAO DỊCH TÀI CHÍNH", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var statsPanel = CreateStatsPanel(new[]
            {
                ("GD hôm nay", "1,456", DashboardConstants.Colors.Info),
                ("Tổng tiền vào", "15.8 tỷ", DashboardConstants.Colors.Success),
                ("Tổng tiền ra", "12.3 tỷ", DashboardConstants.Colors.Danger),
                ("GD chờ duyệt", "23", DashboardConstants.Colors.Warning)
            });
            contentPanel.Controls.Add(statsPanel);

            AddDataGrid(new[] { "TransactionID", "TransactionType", "AccountNumber", "Amount", "DateTime", "Status", "Employee" },
                       new[] { "Mã GD", "Loại GD", "Số TK", "Số tiền", "Thời gian", "Trạng thái", "NV thực hiện" });
        }

        private void LoadPermissionManagement()
        {
            var title = DashboardUIFactory.CreateTitle("🔐 QUẢN LÝ PHÂN QUYỀN & KIỂM SOÁT TRUY CẬP", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var tabControl = DashboardUIFactory.CreateTabControl(
                new Point(20, 80),
                new Size(contentPanel.Width - 40, contentPanel.Height - 100)
            );

            var tabs = new[]
            {
                ("DAC - Discretionary Access Control", "Quản lý quyền truy cập dữ liệu theo nhân viên"),
                ("MAC - Mandatory Access Control", "Gắn nhãn bảo mật cho dữ liệu nhạy cảm"),
                ("RBAC - Role-Based Access Control", "Phân quyền theo vai trò (Admin vs Nhân viên"),
                ("VPD - Virtual Private Database", "Giới hạn dữ liệu theo người dùng"),
                ("OLS - Oracle Label Security", "Kiểm soát truy cập dữ liệu bằng nhãn")
            };

            foreach (var (tabTitle, description) in tabs)
            {
                tabControl.TabPages.Add(DashboardUIFactory.CreateTabPage(tabTitle, description));
            }

            contentPanel.Controls.Add(tabControl);
        }

        private void LoadAuditLog()
        {
            var title = DashboardUIFactory.CreateTitle("📋 NHẬT KÝ AUDIT & GIÁM SÁT", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var tabControl = DashboardUIFactory.CreateTabControl(
                new Point(20, 80),
                new Size(contentPanel.Width - 40, contentPanel.Height - 100)
            );

            // Standard Auditing Tab
            var standardTab = DashboardUIFactory.CreateTabPage("Standard Auditing");
            var standardDgv = DashboardUIFactory.CreateDataGrid();
            standardDgv.Size = new Size(tabControl.Width - 20, tabControl.Height - 60);
            standardDgv.Location = new Point(10, 10);
            standardDgv.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "LogID", HeaderText = "ID" },
                new DataGridViewTextBoxColumn { Name = "UserName", HeaderText = "Người dùng" },
                new DataGridViewTextBoxColumn { Name = "Action", HeaderText = "Hành động" },
                new DataGridViewTextBoxColumn { Name = "DateTime", HeaderText = "Thời gian" },
                new DataGridViewTextBoxColumn { Name = "IPAddress", HeaderText = "IP Address" }
            });
            standardTab.Controls.Add(standardDgv);

            // Fine-Grained Auditing Tab
            var fgaTab = DashboardUIFactory.CreateTabPage("Fine-Grained Auditing");
            var fgaDgv = DashboardUIFactory.CreateDataGrid();
            fgaDgv.Size = new Size(tabControl.Width - 20, tabControl.Height - 60);
            fgaDgv.Location = new Point(10, 10);
            fgaDgv.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "LogID", HeaderText = "ID" },
                new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "Bảng" },
                new DataGridViewTextBoxColumn { Name = "Operation", HeaderText = "Thao tác" },
                new DataGridViewTextBoxColumn { Name = "UserName", HeaderText = "Người dùng" },
                new DataGridViewTextBoxColumn { Name = "DateTime", HeaderText = "Thời gian" },
                new DataGridViewTextBoxColumn { Name = "Details", HeaderText = "Chi tiết" }
            });
            fgaTab.Controls.Add(fgaDgv);

            tabControl.TabPages.AddRange(new TabPage[] { standardTab, fgaTab });
            contentPanel.Controls.Add(tabControl);
        }

        private void LoadReports()
        {
            var title = DashboardUIFactory.CreateTitle("📊 BÁO CÁO & THỐNG KÊ", contentPanel.Width);
            contentPanel.Controls.Add(title);

            // Create report buttons grid
            CreateReportButtons();
            CreateChartPlaceholder();
        }

        private void LoadSettings()
        {
            var title = DashboardUIFactory.CreateTitle("⚙️ CÀI ĐẶT HỆ THỐNG", contentPanel.Width);
            contentPanel.Controls.Add(title);

            var tabControl = DashboardUIFactory.CreateTabControl(
                new Point(20, 80),
                new Size(contentPanel.Width - 40, contentPanel.Height - 100)
            );

            // Security Settings Tab
            var securityTab = DashboardUIFactory.CreateTabPage("🔐 Bảo mật");
            var securitySettings = new[]
            {
                "🔐 Cấu hình mã hóa AES-256",
                "🔑 Chính sách mật khẩu mạnh",
                "⏰ Thời gian phiên làm việc",
                "🛡️ Cấu hình tường lửa ứng dụng",
                "📱 Xác thực đa yếu tố (2FA/MFA)"
            };

            for (int i = 0; i < securitySettings.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = securitySettings[i],
                    Location = new Point(20, 20 + i * 35),
                    Size = new Size(400, 30),
                    Font = new Font("Segoe UI", 11)
                };
                securityTab.Controls.Add(lbl);
            }

            // Database Settings Tab
            var dbTab = DashboardUIFactory.CreateTabPage("🗄️ Cơ sở dữ liệu");
            var dbSettings = new[]
            {
                "🔗 Cấu hình kết nối Oracle Database",
                "🔄 Backup và Recovery tự động",
                "📊 Tối ưu hóa hiệu suất query",
                "🧹 Dọn dẹp dữ liệu cũ (Archiving)"
            };

            for (int i = 0; i < dbSettings.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = dbSettings[i],
                    Location = new Point(20, 20 + i * 35),
                    Size = new Size(400, 30),
                    Font = new Font("Segoe UI", 11)
                };
                dbTab.Controls.Add(lbl);
            }

            tabControl.TabPages.AddRange(new TabPage[] { securityTab, dbTab });
            contentPanel.Controls.Add(tabControl);
        }

        #region Helper Methods

        private Panel CreateStatsPanel((string title, string value, Color color)[] stats)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, DashboardConstants.Sizes.StatCardHeight),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - (stats.Length - 1) * 15) / stats.Length;

            for (int i = 0; i < stats.Length; i++)
            {
                Panel statCard = DashboardUIFactory.CreateStatCard(stats[i].title, stats[i].value, stats[i].color, width);
                statCard.Location = new Point(i * (width + 15), 0);
                panel.Controls.Add(statCard);
            }

            return panel;
        }

        private Panel CreateActionPanel((string text, Color color, Action action)[] actions)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 200),
                Size = new Size(contentPanel.Width - 40, DashboardConstants.Sizes.ActionPanelHeight),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - (actions.Length - 1) * 15) / actions.Length;

            for (int i = 0; i < actions.Length; i++)
            {
                Button btn = DashboardUIFactory.CreateActionButton(actions[i].text, actions[i].color, actions[i].action, width);
                btn.Location = new Point(i * (width + 15), 10);
                btn.Click += ActionButton_Click;
                panel.Controls.Add(btn);
            }

            return panel;
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.Tag is Action action)
                {
                    action.Invoke();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện thao tác: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDataGrid(string[] columnNames, string[] columnHeaders)
        {
            DataGridView dgv = DashboardUIFactory.CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(contentPanel.Width - 40, contentPanel.Height - 320);

            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns.Add(columnNames[i], columnHeaders[i]);
            }

            contentPanel.Controls.Add(dgv);
        }

        private void CreateReportButtons()
        {
            Panel reportPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, 250),
                BackColor = DashboardConstants.Colors.Light,
                Padding = new Padding(20)
            };

            var reportButtons = new[]
            {
                ("📈 Thống kê khách hàng", new Point(20, 20), DashboardConstants.Colors.Info),
                ("💹 Thống kê giao dịch", new Point(220, 20), DashboardConstants.Colors.Success),
                ("👥 Thống kê nhân viên", new Point(420, 20), DashboardConstants.Colors.Warning),
                ("🔍 Nhật ký hệ thống", new Point(620, 20), DashboardConstants.Colors.Danger),
                ("💰 Báo cáo tài chính", new Point(20, 100), DashboardConstants.Colors.Primary),
                ("🛡️ Báo cáo bảo mật", new Point(220, 100), DashboardConstants.Colors.Secondary),
                ("📋 Báo cáo tùy chỉnh", new Point(420, 100), DashboardConstants.Colors.Success),
                ("📤 Xuất báo cáo", new Point(620, 100), DashboardConstants.Colors.Info)
            };

            foreach (var (text, location, color) in reportButtons)
            {
                Button btn = DashboardUIFactory.CreateActionButton(text, color, () => ShowMessage($"Đang mở {text}..."), 180);
                btn.Location = location;
                btn.Click += ActionButton_Click;
                reportPanel.Controls.Add(btn);
            }

            contentPanel.Controls.Add(reportPanel);
        }

        private void CreateChartPlaceholder()
        {
            Panel chartPanel = new Panel
            {
                Location = new Point(20, 350),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 370),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label chartLabel = new Label
            {
                Text = "📊 Biểu đồ thống kê và báo cáo sẽ hiển thị tại đây",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            chartPanel.Controls.Add(chartLabel);
            contentPanel.Controls.Add(chartPanel);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show($"🔧 {message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowProfileForm()
        {
            try
            {
                Profile_GUI profileForm = new Profile_GUI();
                profileForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form Profile: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
