using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class SettingsContent : BaseContent
    {
        private SettingsDataAccess settingsDataAccess;

        public SettingsContent(Panel contentPanel) : base(contentPanel)
        {
            settingsDataAccess = new SettingsDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                // Title
                var title = DashboardUIFactory.CreateTitle("⚙️ CÀI ĐẶT HỆ THỐNG & QUẢN TRỊ", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                // Thống kê cơ bản
                LoadSettingsStatistics();

                // Action panel
                CreateSettingsActionPanel();

                // Tab control cho các nhóm cài đặt
                CreateSettingsTabControl();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải cài đặt: {ex.Message}");
            }
        }

        private void LoadSettingsStatistics()
        {
            try
            {
                var stats = settingsDataAccess.GetSettingsStatistics();
                var statsPanel = CreateStatsPanel(new[]
                {
                    ("Tổng thiết lập", stats.TotalSettings.ToString(), DashboardConstants.Colors.Primary),
                    ("Profiles hoạt động", stats.ActiveProfiles.ToString(), DashboardConstants.Colors.Success),
                    ("Cập nhật cuối", stats.LastUpdated.ToString("dd/MM/yyyy"), DashboardConstants.Colors.Info),
                    ("Lần lưu gần nhất", stats.LastSaved.ToString("dd/MM/yyyy"), DashboardConstants.Colors.Warning)
                });
                ContentPanel.Controls.Add(statsPanel);
            }
            catch
            {
                var statsPanel = CreateStatsPanel(new[]
                {
                    ("Tổng thiết lập", "16", DashboardConstants.Colors.Primary),
                    ("Profiles hoạt động", "3", DashboardConstants.Colors.Success),
                    ("Cập nhật cuối", DateTime.Now.ToString("dd/MM/yyyy"), DashboardConstants.Colors.Info),
                    ("Lần lưu gần nhất", DateTime.Now.ToString("dd/MM/yyyy"), DashboardConstants.Colors.Warning)
                });
                ContentPanel.Controls.Add(statsPanel);
            }
        }

        private void CreateSettingsActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Lưu cài đặt", DashboardConstants.Colors.Success, (Action)ShowSaveSettingsForm),
                ("Tải mặc định", DashboardConstants.Colors.Danger, (Action)ShowLoadDefaultsForm),
                ("Import Config", DashboardConstants.Colors.Info, (Action)ShowImportConfigForm),
                ("Export Config", DashboardConstants.Colors.Secondary, (Action)ShowExportConfigForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void CreateSettingsTabControl()
        {
            var tabControl = DashboardUIFactory.CreateTabControl(
                new Point(20, 300),
                new Size(ContentPanel.Width - 40, ContentPanel.Height - 320)
            );

            tabControl.TabPages.Add(CreateSecurityTab());
            tabControl.TabPages.Add(CreateDatabaseTab());
            tabControl.TabPages.Add(CreateUiTab());
            tabControl.TabPages.Add(CreateNotificationsTab());
            tabControl.TabPages.Add(CreateLoggingTab());

            ContentPanel.Controls.Add(tabControl);
        }

        private TabPage CreateSecurityTab()
        {
            var tab = DashboardUIFactory.CreateTabPage("🔐 Bảo mật");
            int y = 20;
            string[] items = {
                "🔑 Chính sách mật khẩu mạnh",
                "⏰ Thời gian timeout phiên",
                "📱 2FA/MFA",
                "🛡️ Quản lý khóa tài khoản"
            };
            foreach (var txt in items)
            {
                tab.Controls.Add(new Label
                {
                    Text = txt,
                    Location = new Point(20, y),
                    Size = new Size(tab.Width - 60, 30),
                    Font = new Font("Segoe UI", 11)
                });
                y += 40;
            }
            return tab;
        }

        private TabPage CreateDatabaseTab()
        {
            var tab = DashboardUIFactory.CreateTabPage("🗄️ Cơ sở dữ liệu");
            int y = 20;
            string[] items = {
                "🔗 Connection String",
                "🔄 Backup & Recovery",
                "📊 Tối ưu hoá query",
                "🧹 Archiving dữ liệu"
            };
            foreach (var txt in items)
            {
                tab.Controls.Add(new Label
                {
                    Text = txt,
                    Location = new Point(20, y),
                    Size = new Size(tab.Width - 60, 30),
                    Font = new Font("Segoe UI", 11)
                });
                y += 40;
            }
            return tab;
        }

        private TabPage CreateUiTab()
        {
            var tab = DashboardUIFactory.CreateTabPage("🎨 Giao diện");
            var themes = new[] { "🌑 Dark", "🌕 Light", "🌈 Colorful" };
            for (int i = 0; i < themes.Length; i++)
            {
                var rb = new RadioButton
                {
                    Text = themes[i],
                    Location = new Point(20, 20 + i * 35),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11)
                };
                tab.Controls.Add(rb);
            }
            return tab;
        }

        private TabPage CreateNotificationsTab()
        {
            var tab = DashboardUIFactory.CreateTabPage("🔔 Thông báo");
            string[] items = {
                "📧 Email Alerts",
                "📱 SMS Alerts",
                "🔔 In-App Notifications",
                "📋 Báo cáo định kỳ"
            };
            int y = 20;
            foreach (var txt in items)
            {
                var chk = new CheckBox
                {
                    Text = txt,
                    Location = new Point(20, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11)
                };
                tab.Controls.Add(chk);
                y += 35;
            }
            return tab;
        }

        private TabPage CreateLoggingTab()
        {
            var tab = DashboardUIFactory.CreateTabPage("📜 Ghi log");
            string[] levels = { "DEBUG", "INFO", "WARN", "ERROR", "FATAL" };
            var cb = new ComboBox
            {
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            cb.Items.AddRange(levels);
            cb.SelectedIndex = 1; // INFO
            tab.Controls.Add(new Label
            {
                Text = "Chọn mức log:",
                Location = new Point(20, 0),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10)
            });
            tab.Controls.Add(cb);
            return tab;
        }

        // Action handlers
        private void ShowSaveSettingsForm() => ShowMessage("Lưu cài đặt...");
        private void ShowLoadDefaultsForm() => ShowMessage("Tải cài đặt mặc định...");
        private void ShowImportConfigForm() => ShowMessage("Import file cấu hình...");
        private void ShowExportConfigForm() => ShowMessage("Export cấu hình...");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Cài đặt đã được làm mới!");
        }
    }
}
