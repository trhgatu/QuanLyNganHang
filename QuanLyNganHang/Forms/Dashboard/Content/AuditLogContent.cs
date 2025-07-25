using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class AuditLogContent : BaseContent
    {
        public AuditLogContent(Panel contentPanel) : base(contentPanel) { }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("📋 NHẬT KÝ AUDIT & GIÁM SÁT", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                // Thêm statistics panel
                LoadAuditStatistics();

                // Thêm action panel
                CreateAuditActionPanel();

                CreateAuditTabControl();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void LoadAuditStatistics()
        {
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Logs hôm nay", "247", DashboardConstants.Colors.Info),
                ("Logs thành công", "231", DashboardConstants.Colors.Success),
                ("Logs thất bại", "16", DashboardConstants.Colors.Danger),
                ("FGA Policies", "8", DashboardConstants.Colors.Warning)
            });
            ContentPanel.Controls.Add(statsPanel);
        }

        private void CreateAuditActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Tìm kiếm log", DashboardConstants.Colors.Info, (Action)ShowSearchForm),
                ("Xuất báo cáo", DashboardConstants.Colors.Success, (Action)ShowExportForm),
                ("Cấu hình audit", DashboardConstants.Colors.Primary, (Action)ShowConfigForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void CreateAuditTabControl()
        {
            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 300),
                Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // 2 tab theo yêu cầu với UI cải thiện
            tabControl.TabPages.Add(CreateStandardAuditTab());
            tabControl.TabPages.Add(CreateFGATab());

            ContentPanel.Controls.Add(tabControl);
        }

        private TabPage CreateStandardAuditTab()
        {
            TabPage tab = new TabPage("📊 Standard Auditing");

            // Thêm description label
            Label descLabel = new Label
            {
                Text = "📋 Audit chuẩn Oracle - Theo dõi đăng nhập, DDL, DML operations",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Info,
                Location = new Point(20, 15),
                Size = new Size(tab.Width - 40, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tab.Controls.Add(descLabel);

            // Thêm filter panel
            Panel filterPanel = CreateFilterPanel();
            filterPanel.Location = new Point(20, 45);
            filterPanel.Size = new Size(tab.Width - 40, 50);
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tab.Controls.Add(filterPanel);

            // DataGrid với UI cải thiện
            DataGridView grid = DashboardUIFactory.CreateDataGrid();
            grid.Location = new Point(20, 105);
            grid.Size = new Size(tab.Width - 40, tab.Height - 125);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Cấu hình columns đẹp hơn
            ConfigureStandardAuditGrid(grid);

            // Thêm context menu
            AddStandardAuditContextMenu(grid);

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateFGATab()
        {
            TabPage tab = new TabPage("🔍 Fine-Grained Auditing");

            // Description
            Label descLabel = new Label
            {
                Text = "🎯 FGA - Audit chi tiết các truy vấn trên dữ liệu nhạy cảm",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Warning,
                Location = new Point(20, 15),
                Size = new Size(tab.Width - 40, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tab.Controls.Add(descLabel);

            // FGA Policies info panel
            Panel policiesPanel = CreatePoliciesInfoPanel();
            policiesPanel.Location = new Point(20, 45);
            policiesPanel.Size = new Size(tab.Width - 40, 60);
            policiesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tab.Controls.Add(policiesPanel);

            // DataGrid
            DataGridView grid = DashboardUIFactory.CreateDataGrid();
            grid.Location = new Point(20, 115);
            grid.Size = new Size(tab.Width - 40, tab.Height - 135);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Cấu hình FGA columns
            ConfigureFGAGrid(grid);

            // Context menu cho FGA
            AddFGAContextMenu(grid);

            tab.Controls.Add(grid);
            return tab;
        }

        #region UI HELPER METHODS

        private Panel CreateFilterPanel()
        {
            Panel filterPanel = new Panel
            {
                BackColor = DashboardConstants.Colors.Light,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Date filter
            Label fromLabel = new Label
            {
                Text = "Từ ngày:",
                Location = new Point(10, 15),
                Size = new Size(60, 20),
                Font = new Font("Segoe UI", 9)
            };

            DateTimePicker fromDate = new DateTimePicker
            {
                Location = new Point(75, 12),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short
            };

            Label toLabel = new Label
            {
                Text = "Đến:",
                Location = new Point(210, 15),
                Size = new Size(40, 20),
                Font = new Font("Segoe UI", 9)
            };

            DateTimePicker toDate = new DateTimePicker
            {
                Location = new Point(255, 12),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short
            };

            // User filter
            Label userLabel = new Label
            {
                Text = "User:",
                Location = new Point(390, 15),
                Size = new Size(40, 20),
                Font = new Font("Segoe UI", 9)
            };

            ComboBox userCombo = new ComboBox
            {
                Location = new Point(435, 12),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            userCombo.Items.AddRange(new object[] { "Tất cả", "admin", "user1", "user2" });
            userCombo.SelectedIndex = 0;

            // Search button
            Button searchBtn = new Button
            {
                Text = "🔍 Tìm",
                Location = new Point(550, 10),
                Size = new Size(80, 30),
                BackColor = DashboardConstants.Colors.Info,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            searchBtn.FlatAppearance.BorderSize = 0;
            searchBtn.Click += (s, e) => SearchAuditLogs(fromDate.Value, toDate.Value, userCombo.Text);

            filterPanel.Controls.AddRange(new Control[] {
                fromLabel, fromDate, toLabel, toDate, userLabel, userCombo, searchBtn
            });

            return filterPanel;
        }

        private Panel CreatePoliciesInfoPanel()
        {
            Panel policiesPanel = new Panel
            {
                BackColor = Color.FromArgb(250, 250, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label policiesLabel = new Label
            {
                Text = "📋 Active FGA Policies: CUSTOMER_ACCESS_POLICY | ACCOUNT_BALANCE_POLICY | TRANSACTION_AMOUNT_POLICY",
                Location = new Point(10, 10),
                Size = new Size(policiesPanel.Width - 20, 40),
                Font = new Font("Segoe UI", 9),
                ForeColor = DashboardConstants.Colors.Primary,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            policiesPanel.Controls.Add(policiesLabel);
            return policiesPanel;
        }

        private void ConfigureStandardAuditGrid(DataGridView grid)
        {
            // Clear existing columns
            grid.Columns.Clear();

            // Add columns with proper widths
            grid.Columns.Add("LogID", "ID");
            grid.Columns.Add("UserName", "Người dùng");
            grid.Columns.Add("Action", "Hành động");
            grid.Columns.Add("ObjectName", "Đối tượng");
            grid.Columns.Add("Time", "Thời gian");
            grid.Columns.Add("IPAddress", "IP Address");
            grid.Columns.Add("Status", "Trạng thái");

            // Configure column widths
            grid.Columns["LogID"].Width = 60;
            grid.Columns["UserName"].Width = 100;
            grid.Columns["Action"].Width = 100;
            grid.Columns["ObjectName"].Width = 120;
            grid.Columns["Time"].Width = 140;
            grid.Columns["IPAddress"].Width = 130;
            grid.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Add sample data with more realistic entries
            grid.Rows.Add("1001", "admin", "LOGIN", "SYSTEM", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "192.168.1.100", "SUCCESS");
            grid.Rows.Add("1002", "user1", "SELECT", "CUSTOMERS", DateTime.Now.AddMinutes(-5).ToString("dd/MM/yyyy HH:mm:ss"), "192.168.1.105", "SUCCESS");
            grid.Rows.Add("1003", "user2", "INSERT", "ACCOUNTS", DateTime.Now.AddMinutes(-10).ToString("dd/MM/yyyy HH:mm:ss"), "192.168.1.110", "SUCCESS");
            grid.Rows.Add("1004", "user1", "UPDATE", "TRANSACTIONS", DateTime.Now.AddMinutes(-15).ToString("dd/MM/yyyy HH:mm:ss"), "192.168.1.105", "FAILED");
            grid.Rows.Add("1005", "admin", "DELETE", "LOGS", DateTime.Now.AddMinutes(-20).ToString("dd/MM/yyyy HH:mm:ss"), "192.168.1.100", "SUCCESS");

            // Color code status column
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells["Status"].Value?.ToString() == "FAILED")
                {
                    row.Cells["Status"].Style.BackColor = Color.FromArgb(255, 200, 200);
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                }
                else if (row.Cells["Status"].Value?.ToString() == "SUCCESS")
                {
                    row.Cells["Status"].Style.BackColor = Color.FromArgb(200, 255, 200);
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                }
            }
        }

        private void ConfigureFGAGrid(DataGridView grid)
        {
            // Clear existing columns
            grid.Columns.Clear();

            // Add FGA-specific columns
            grid.Columns.Add("LogID", "ID");
            grid.Columns.Add("PolicyName", "Policy");
            grid.Columns.Add("TableName", "Bảng");
            grid.Columns.Add("UserName", "User");
            grid.Columns.Add("Operation", "Thao tác");
            grid.Columns.Add("SQLText", "SQL Statement");
            grid.Columns.Add("Time", "Thời gian");

            // Configure column widths
            grid.Columns["LogID"].Width = 60;
            grid.Columns["PolicyName"].Width = 150;
            grid.Columns["TableName"].Width = 100;
            grid.Columns["UserName"].Width = 100;
            grid.Columns["Operation"].Width = 80;
            grid.Columns["SQLText"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns["Time"].Width = 140;

            // Add sample FGA data
            grid.Rows.Add("FGA001", "CUSTOMER_ACCESS_POLICY", "CUSTOMERS", "user1", "SELECT", "SELECT * FROM CUSTOMERS WHERE customer_id = ?", DateTime.Now.AddMinutes(-2).ToString("dd/MM/yyyy HH:mm:ss"));
            grid.Rows.Add("FGA002", "ACCOUNT_BALANCE_POLICY", "ACCOUNTS", "user2", "SELECT", "SELECT balance FROM ACCOUNTS WHERE account_id = ?", DateTime.Now.AddMinutes(-8).ToString("dd/MM/yyyy HH:mm:ss"));
            grid.Rows.Add("FGA003", "TRANSACTION_AMOUNT_POLICY", "TRANSACTIONS", "admin", "SELECT", "SELECT * FROM TRANSACTIONS WHERE amount > 10000000", DateTime.Now.AddMinutes(-12).ToString("dd/MM/yyyy HH:mm:ss"));
            grid.Rows.Add("FGA004", "CUSTOMER_ACCESS_POLICY", "CUSTOMERS", "user1", "UPDATE", "UPDATE CUSTOMERS SET status = ? WHERE customer_id = ?", DateTime.Now.AddMinutes(-18).ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private void AddStandardAuditContextMenu(DataGridView grid)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("🔍 Xem chi tiết", null, (s, e) => ViewAuditDetails(grid)),
                new ToolStripMenuItem("📋 Copy thông tin", null, (s, e) => CopyAuditInfo(grid)),
                new ToolStripMenuItem("🔗 Trace session", null, (s, e) => TraceSession(grid)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });
            grid.ContextMenuStrip = contextMenu;
        }

        private void AddFGAContextMenu(DataGridView grid)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("📝 Xem SQL đầy đủ", null, (s, e) => ViewFullSQL(grid)),
                new ToolStripMenuItem("⚠️ Đánh dấu nghi ngờ", null, (s, e) => MarkSuspicious(grid)),
                new ToolStripMenuItem("📧 Gửi cảnh báo", null, (s, e) => SendAlert(grid)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });
            grid.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region EVENT HANDLERS

        private void SearchAuditLogs(DateTime fromDate, DateTime toDate, string user)
        {
            ShowMessage($"Tìm kiếm logs từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy} cho user: {user}");
            // TODO: Implement actual search logic
        }

        // Context menu actions
        private void ViewAuditDetails(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                var row = grid.SelectedRows[0];
                string logId = row.Cells[0].Value?.ToString();
                ShowMessage($"Xem chi tiết audit log ID: {logId}");
            }
        }

        private void CopyAuditInfo(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // Copy row data to clipboard
                ShowMessage("Đã copy thông tin audit vào clipboard");
            }
        }

        private void TraceSession(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                var row = grid.SelectedRows[0];
                string user = row.Cells["UserName"].Value?.ToString();
                ShowMessage($"Trace session của user: {user}");
            }
        }

        private void ViewFullSQL(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                var row = grid.SelectedRows[0];
                string sql = row.Cells["SQLText"].Value?.ToString();

                // Create a popup to show full SQL
                Form sqlForm = new Form
                {
                    Text = "SQL Statement Chi tiết",
                    Size = new Size(600, 400),
                    StartPosition = FormStartPosition.CenterParent
                };

                TextBox sqlTextBox = new TextBox
                {
                    Text = sql,
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    ReadOnly = true,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 10)
                };

                sqlForm.Controls.Add(sqlTextBox);
                sqlForm.ShowDialog();
            }
        }

        private void MarkSuspicious(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                var row = grid.SelectedRows[0];
                row.DefaultCellStyle.BackColor = Color.Yellow;
                ShowMessage("Đã đánh dấu hoạt động nghi ngờ");
            }
        }

        private void SendAlert(DataGridView grid)
        {
            ShowMessage("Gửi cảnh báo bảo mật đến administrator");
        }

        // Action panel methods
        private void ShowSearchForm() => ShowMessage("Mở form tìm kiếm audit logs nâng cao");
        private void ShowExportForm() => ShowMessage("Xuất báo cáo audit logs");
        private void ShowConfigForm() => ShowMessage("Cấu hình audit policies");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu audit logs đã được làm mới!");
        }

        #endregion
    }
}
