using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class ReportsContent : BaseContent
    {
        private ReportsDataAccess reportsDataAccess;

        public ReportsContent(Panel contentPanel) : base(contentPanel)
        {
            reportsDataAccess = new ReportsDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("📊 BÁO CÁO & THỐNG KÊ", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                // Statistics đơn giản
                LoadSimpleStatistics();

                // Action panel cơ bản
                CreateSimpleActionPanel();

                // Report tabs đơn giản
                CreateSimpleReportTabs();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void LoadSimpleStatistics()
        {
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Báo cáo hôm nay", "8", DashboardConstants.Colors.Info),
                ("Báo cáo tự động", "3", DashboardConstants.Colors.Success),
                ("Báo cáo lỗi", "1", DashboardConstants.Colors.Danger)
            });
            ContentPanel.Controls.Add(statsPanel);
        }

        private void CreateSimpleActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Tạo báo cáo", DashboardConstants.Colors.Success, (Action)ShowCreateReportForm),
                ("Xuất Excel", DashboardConstants.Colors.Primary, (Action)ShowExportForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void CreateSimpleReportTabs()
        {
            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 250),
                Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 270),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Chỉ 3 tab cơ bản theo yêu cầu đồ án
            tabControl.TabPages.Add(CreateUserReportTab());
            tabControl.TabPages.Add(CreateSecurityReportTab());
            tabControl.TabPages.Add(CreateSystemReportTab());

            ContentPanel.Controls.Add(tabControl);
        }

        private TabPage CreateUserReportTab()
        {
            TabPage tab = new TabPage("👥 Báo cáo Người dùng");

            DataGridView grid = DashboardUIFactory.CreateDataGrid();
            grid.Location = new Point(20, 20);
            grid.Size = new Size(tab.Width - 40, tab.Height - 40);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Columns cho user report
            grid.Columns.Add("UserName", "Tên đăng nhập");
            grid.Columns.Add("LoginCount", "Số lần đăng nhập");
            grid.Columns.Add("LastLogin", "Lần cuối");
            grid.Columns.Add("Status", "Trạng thái");

            // Sample data
            grid.Rows.Add("admin", "45", "25/07/2025 08:30", "Active");
            grid.Rows.Add("user1", "23", "24/07/2025 16:45", "Active");
            grid.Rows.Add("user2", "12", "23/07/2025 14:20", "Inactive");

            // Configure columns
            grid.Columns["UserName"].Width = 150;
            grid.Columns["LoginCount"].Width = 120;
            grid.Columns["LastLogin"].Width = 150;
            grid.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateSecurityReportTab()
        {
            TabPage tab = new TabPage("🔐 Báo cáo Bảo mật");

            DataGridView grid = DashboardUIFactory.CreateDataGrid();
            grid.Location = new Point(20, 20);
            grid.Size = new Size(tab.Width - 40, tab.Height - 40);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Columns cho security report
            grid.Columns.Add("EventType", "Loại sự kiện");
            grid.Columns.Add("UserName", "Người dùng");
            grid.Columns.Add("DateTime", "Thời gian");
            grid.Columns.Add("Result", "Kết quả");

            // Sample security events
            grid.Rows.Add("Login Failed", "unknown_user", DateTime.Now.AddHours(-2).ToString("dd/MM/yyyy HH:mm"), "FAILED");
            grid.Rows.Add("Permission Denied", "user1", DateTime.Now.AddHours(-1).ToString("dd/MM/yyyy HH:mm"), "BLOCKED");
            grid.Rows.Add("Password Changed", "admin", DateTime.Now.AddMinutes(-30).ToString("dd/MM/yyyy HH:mm"), "SUCCESS");

            // Configure columns
            grid.Columns["EventType"].Width = 150;
            grid.Columns["UserName"].Width = 120;
            grid.Columns["DateTime"].Width = 140;
            grid.Columns["Result"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Color code results
            foreach (DataGridViewRow row in grid.Rows)
            {
                string result = row.Cells["Result"].Value?.ToString();
                if (result == "FAILED" || result == "BLOCKED")
                {
                    row.Cells["Result"].Style.BackColor = Color.FromArgb(255, 200, 200);
                    row.Cells["Result"].Style.ForeColor = Color.Red;
                }
                else if (result == "SUCCESS")
                {
                    row.Cells["Result"].Style.BackColor = Color.FromArgb(200, 255, 200);
                    row.Cells["Result"].Style.ForeColor = Color.Green;
                }
            }

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateSystemReportTab()
        {
            TabPage tab = new TabPage("🖥️ Báo cáo Hệ thống");

            // Simple system stats
            Panel statsPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(tab.Width - 40, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = DashboardConstants.Colors.Light,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = "📊 THỐNG KÊ HỆ THỐNG",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(200, 30)
            };

            string[] systemStats = {
                "🔧 Database: Oracle 19c - Hoạt động bình thường",
                "💾 Dung lượng sử dụng: 2.3GB / 10GB",
                "👥 Người dùng online: 3/15",
                "📈 CPU: 35% | RAM: 60% | Disk: 23%",
                "🕐 Uptime: 15 ngày 8 giờ 23 phút",
                "🔄 Backup cuối: 24/07/2025 02:00 AM"
            };

            for (int i = 0; i < systemStats.Length; i++)
            {
                Label statLabel = new Label
                {
                    Text = systemStats[i],
                    Location = new Point(20, 60 + (i * 20)),
                    Size = new Size(statsPanel.Width - 40, 20),
                    Font = new Font("Segoe UI", 10)
                };
                statsPanel.Controls.Add(statLabel);
            }

            statsPanel.Controls.Add(titleLabel);
            tab.Controls.Add(statsPanel);

            return tab;
        }

        // Action methods đơn giản
        private void ShowCreateReportForm() => ShowMessage("Tạo báo cáo mới");
        private void ShowExportForm() => ShowMessage("Xuất báo cáo Excel");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu báo cáo đã được làm mới!");
        }
    }
}