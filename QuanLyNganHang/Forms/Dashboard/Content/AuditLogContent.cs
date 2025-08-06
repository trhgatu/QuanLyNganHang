using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class AuditLogContent : BaseContent
    {
        private AuditLogDataAccess dataAccess;
        private ComboBox userCombo;
        private DataGridView standardAuditGrid;
        private DataGridView fgaGrid;
        private DataGridView triggerGrid;
        private TabControl tabControl;
        private Control todayCard, successCard, failedCard, alertCard;
        private string currentUser;
        private CheckBox chkSelect, chkInsert, chkUpdate, chkDelete;


        public AuditLogContent(Panel contentPanel) : base(contentPanel)
        {
            dataAccess = new AuditLogDataAccess();
            currentUser = GetLoggedInOracleUser();
        }
        private string GetLoggedInOracleUser()
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new OracleCommand("SELECT USER FROM DUAL", conn))
                {
                    return cmd.ExecuteScalar()?.ToString()?.ToUpper();
                }
            }
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();
                ContentPanel.AutoScroll = true;

                var title = DashboardUIFactory.CreateTitle("📋 NHẬT KÝ AUDIT & GIÁM SÁT", ContentPanel.Width);
                title.Location = new Point(0, 0);
                title.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                ContentPanel.Controls.Add(title);

                LoadAuditStatistics();
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
            var stats = dataAccess.GetAuditStatistics();

            FlowLayoutPanel statsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(10, 60),
                Size = new Size(ContentPanel.Width - 20, 140),
                Padding = new Padding(5),
                AutoScroll = false,
                WrapContents = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            int cardWidth = 240;

            todayCard = DashboardUIFactory.CreateStatCard("📅 Log hôm nay", stats.TodayLogs.ToString(), DashboardConstants.Colors.Info, cardWidth);
            successCard = DashboardUIFactory.CreateStatCard("✅ Log thành công", stats.SuccessLogs.ToString(), DashboardConstants.Colors.Success, cardWidth);
            failedCard = DashboardUIFactory.CreateStatCard("❌ Log thất bại", stats.FailedLogs.ToString(), DashboardConstants.Colors.Danger, cardWidth);
            alertCard = DashboardUIFactory.CreateStatCard("⚠️ Cảnh báo bảo mật", stats.SecurityAlerts.ToString(), DashboardConstants.Colors.Warning, cardWidth);

            statsPanel.Controls.Add(todayCard);
            statsPanel.Controls.Add(successCard);
            statsPanel.Controls.Add(failedCard);
            statsPanel.Controls.Add(alertCard);

            ContentPanel.Controls.Add(statsPanel);
        }

        private void ShowConfigForm()
        {
            try
            {
                Form configForm = new Form();
                configForm.Text = "Cấu hình Audit Policies";
                configForm.Size = new Size(400, 300);
                configForm.StartPosition = FormStartPosition.CenterParent;

                CheckBox chkLoginAudit = new CheckBox()
                {
                    Text = "Bật audit đăng nhập thất bại",
                    Checked = true,
                    Location = new Point(20, 40),
                    AutoSize = true
                };

                Button btnSave = new Button()
                {
                    Text = "Lưu cấu hình",
                    Location = new Point(20, 80),
                    Size = new Size(120, 30)
                };

                btnSave.Click += (s, e) =>
                {
                    string sql = chkLoginAudit.Checked
                        ? "AUDIT SESSION WHENEVER NOT SUCCESSFUL"
                        : "NOAUDIT SESSION";

                    try
                    {
                        dataAccess.ExecuteAuditConfig(sql);
                        MessageBox.Show("✅ Đã áp dụng cấu hình audit.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi áp dụng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                configForm.Controls.Add(chkLoginAudit);
                configForm.Controls.Add(btnSave);
                configForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi cấu hình: {ex.Message}");
            }
        }


        private void CreateAuditActionPanel()
        {
            FlowLayoutPanel actionPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(10, 190),
                Size = new Size(ContentPanel.Width - 20, 60),
                Padding = new Padding(5),
                AutoScroll = false,
                WrapContents = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            var configBtn = DashboardUIFactory.CreateActionButton("⚙️ Cấu hình audit", DashboardConstants.Colors.Primary, ShowConfigForm, 140);
            var refreshBtn = DashboardUIFactory.CreateActionButton("🔄 Làm mới", DashboardConstants.Colors.Info, RefreshContent, 140);

            actionPanel.Controls.Add(configBtn);
            actionPanel.Controls.Add(refreshBtn);

            ContentPanel.Controls.Add(actionPanel);
        }
        public override void RefreshContent()
        {
            try
            {
                LoadContent();
                ShowMessage("🔄 Giao diện và dữ liệu đã được làm mới!");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi làm mới giao diện: {ex.Message}");
            }
        }


        private void ShowTodayLogs()
        {
            if (userCombo?.SelectedItem == null)
            {
                ShowMessage("Vui lòng chọn user trước!");
                return;
            }

            string user = userCombo.SelectedItem.ToString();
            var dt = dataAccess.GetStandardAuditLogs(user);
            var today = DateTime.Today;
            DataView dv = dt.DefaultView;

            dv.RowFilter = $"TIME_ACTION >= '{today:yyyy-MM-dd}'";
            standardAuditGrid.DataSource = dv;

            if (tabControl != null && tabControl.TabPages.Count > 0)
                tabControl.SelectedIndex = 0;

            ShowMessage($"Đang hiển thị {dv.Count} logs hôm nay.");
        }

        private void ShowSuccessLogs()
        {
            if (userCombo?.SelectedItem == null)
            {
                ShowMessage("Vui lòng chọn user trước!");
                return;
            }

            string user = userCombo.SelectedItem.ToString();
            var dt = dataAccess.GetStandardAuditLogs(user);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "RETURNCODE = 0";
            standardAuditGrid.DataSource = dv;

            if (tabControl != null && tabControl.TabPages.Count > 0)
                tabControl.SelectedIndex = 0;

            ShowMessage($"Đang hiển thị {dv.Count} logs thành công.");
        }

        private void ShowFailedLogs()
        {
            if (userCombo?.SelectedItem == null)
            {
                ShowMessage("Vui lòng chọn user trước!");
                return;
            }

            string user = userCombo.SelectedItem.ToString();
            var dt = dataAccess.GetStandardAuditLogs(user);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "RETURNCODE <> 0";
            standardAuditGrid.DataSource = dv;

            if (tabControl != null && tabControl.TabPages.Count > 0)
                tabControl.SelectedIndex = 0;

            ShowMessage($"Đang hiển thị {dv.Count} logs thất bại.");
        }

        private void ShowSecurityAlerts()
        {
            if (userCombo?.SelectedItem == null)
            {
                ShowMessage("Vui lòng chọn user trước!");
                return;
            }

            string user = userCombo.SelectedItem.ToString();
            var dt = dataAccess.GetStandardAuditLogs(user);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ACTION_NAME LIKE '%DELETE%' OR ACTION_NAME LIKE '%DROP%' OR ACTION_NAME LIKE '%TRUNCATE%'";
            standardAuditGrid.DataSource = dv;

            if (tabControl != null && tabControl.TabPages.Count > 0)
                tabControl.SelectedIndex = 0;

            ShowMessage($"Đang hiển thị {dv.Count} cảnh báo thất bại.");
        }

        private void CreateAuditTabControl()
        {
            tabControl = new TabControl
            {
                Location = new Point(10, 260),
                Size = new Size(ContentPanel.Width - 20, ContentPanel.Height - 280),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            tabControl.TabPages.Add(CreateStandardAuditTab());
            tabControl.TabPages.Add(CreateFGATab());
            tabControl.TabPages.Add(CreateTriggerTab());

            ContentPanel.Controls.Add(tabControl);

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            /* try
             {
                 if (userCombo?.Items.Count > 0)
                 {
                     userCombo.SelectedIndex = 0;
                     SearchAuditLogs(userCombo.SelectedItem.ToString());
                 }
             }
             catch (Exception ex)
             {
                 ShowError($"Lỗi load dữ liệu ban đầu: {ex.Message}");
             }
            */
            try
            {
                if (userCombo?.Items.Count > 0)
                {
                    for (int i = 0; i < userCombo.Items.Count; i++)
                    {
                        if (userCombo.Items[i].ToString().ToUpper() == currentUser)
                        {
                            userCombo.SelectedIndex = i;
                            break;
                        }
                    }

                    SearchAuditLogs(currentUser);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi load dữ liệu ban đầu: {ex.Message}");
            }
        }

        private TabPage CreateStandardAuditTab()
        {
            TabPage tab = new TabPage("📊 Standard Auditing");

            Label descLabel = new Label
            {
                Text = "📋 Audit chuẩn Oracle - Theo dõi đăng nhập, DDL, DML operations",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Info,
                Location = new Point(15, 10),
                AutoSize = true
            };
            tab.Controls.Add(descLabel);

            Panel filterPanel = CreateFilterPanel();
            filterPanel.Location = new Point(15, 35);
            filterPanel.Size = new Size(tab.Width - 30, 50);
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tab.Controls.Add(filterPanel);

            standardAuditGrid = DashboardUIFactory.CreateDataGrid();
            standardAuditGrid.Location = new Point(15, 95);
            standardAuditGrid.Size = new Size(tab.Width - 30, tab.Height - 115);
            standardAuditGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            standardAuditGrid.AllowUserToAddRows = false;
            standardAuditGrid.AllowUserToDeleteRows = false;
            standardAuditGrid.ReadOnly = true;
            standardAuditGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            standardAuditGrid.MultiSelect = false;
            standardAuditGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tab.Controls.Add(standardAuditGrid);

            return tab;
        }

        private TabPage CreateFGATab()
        {
            TabPage tab = new TabPage("🔍 Fine-Grained Auditing");

            Label descLabel = new Label
            {
                Text = "🎯 FGA - Audit chi tiết các truy vấn trên dữ liệu nhạy cảm",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Warning,
                Location = new Point(15, 10),
                AutoSize = true
            };
            tab.Controls.Add(descLabel);

            fgaGrid = DashboardUIFactory.CreateDataGrid();
            fgaGrid.Location = new Point(15, 40);
            fgaGrid.Size = new Size(tab.Width - 30, tab.Height - 60);
            fgaGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            fgaGrid.AllowUserToAddRows = false;
            fgaGrid.AllowUserToDeleteRows = false;
            fgaGrid.ReadOnly = true;
            fgaGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            fgaGrid.MultiSelect = false;
            fgaGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tab.Controls.Add(fgaGrid);

            return tab;
        }

        private TabPage CreateTriggerTab()
        {
            TabPage tab = new TabPage("⚡ Trigger Auditing");

            Label descLabel = new Label
            {
                Text = "⚡ Trigger Audit - Theo dõi các hành động DML được trigger ghi lại",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Primary,
                Location = new Point(15, 10),
                AutoSize = true
            };
            tab.Controls.Add(descLabel);

            triggerGrid = DashboardUIFactory.CreateDataGrid();
            triggerGrid.Location = new Point(15, 40);
            triggerGrid.Size = new Size(tab.Width - 30, tab.Height - 60);
            triggerGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            triggerGrid.AllowUserToAddRows = false;
            triggerGrid.AllowUserToDeleteRows = false;
            triggerGrid.ReadOnly = true;
            triggerGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            triggerGrid.MultiSelect = false;
            triggerGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tab.Controls.Add(triggerGrid);

            return tab;
        }

        private Panel CreateFilterPanel()
        {
            Panel filterPanel = new Panel
            {
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Height = 50
            };

            Label userLabel = new Label
            {
                Text = "👤 User:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            userCombo = new ComboBox
            {
                Location = new Point(65, 12),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };

            userCombo.SelectedIndexChanged += (s, e) =>
            {
                if (userCombo.SelectedItem != null)
                    SearchAuditLogs(userCombo.SelectedItem.ToString());
            };

            try
            {
                var userList = dataAccess.GetUserList();
                userCombo.Items.Clear();
                foreach (DataRow row in userList.Rows)
                {
                    userCombo.Items.Add(row["USERNAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi load danh sách user: {ex.Message}");
            }

            filterPanel.Controls.Add(userLabel);
            filterPanel.Controls.Add(userCombo);
            chkSelect = new CheckBox() { Text = "SELECT", Location = new Point(280, 15), AutoSize = true };
            chkInsert = new CheckBox() { Text = "INSERT", Location = new Point(350, 15), AutoSize = true };
            chkUpdate = new CheckBox() { Text = "UPDATE", Location = new Point(420, 15), AutoSize = true };
            chkDelete = new CheckBox() { Text = "DELETE", Location = new Point(490, 15), AutoSize = true };


            chkSelect.CheckedChanged += (s, e) => ReloadLogsIfUserSelected();
            chkInsert.CheckedChanged += (s, e) => ReloadLogsIfUserSelected();
            chkUpdate.CheckedChanged += (s, e) => ReloadLogsIfUserSelected();
            chkDelete.CheckedChanged += (s, e) => ReloadLogsIfUserSelected();


            filterPanel.Controls.Add(chkSelect);
            filterPanel.Controls.Add(chkInsert);
            filterPanel.Controls.Add(chkUpdate);
            filterPanel.Controls.Add(chkDelete);


            return filterPanel;
        }
        private void ReloadLogsIfUserSelected()
        {
            if (userCombo?.SelectedItem != null)
            {
                SearchAuditLogs(userCombo.SelectedItem.ToString());
            }
        }
        private void SearchAuditLogs(string user)
        {
            try
            {
                ShowMessage($"Đang tải logs cho user {user}...");

                var dt = dataAccess.GetStandardAuditLogs(user);

                var dtFga = dataAccess.GetFineGrainedAuditLogs(user);
                var dtTrigger = dataAccess.GetTriggerAuditLogs(user);

                if (standardAuditGrid != null)
                {
                    DataView dv = dt.DefaultView;


                    List<string> selectedActions = new List<string>();
                    if (chkSelect.Checked) selectedActions.Add("SELECT");
                    if (chkInsert.Checked) selectedActions.Add("INSERT");
                    if (chkUpdate.Checked) selectedActions.Add("UPDATE");
                    if (chkDelete.Checked) selectedActions.Add("DELETE");

                    string filter = string.Join(" OR ", selectedActions.Select(a => $"action_name = '{a}'"));


                    standardAuditGrid.DataSource = dv;
                    FormatStandardAuditGrid();
                }

                if (fgaGrid != null)
                {
                    fgaGrid.DataSource = dtFga;
                    FormatFGAGrid();
                }

                if (triggerGrid != null)
                {
                    triggerGrid.DataSource = null;
                    triggerGrid.Columns.Clear();

                    triggerGrid.DataSource = dtTrigger;

                    if (dtTrigger.Columns.Count > 0)
                        FormatTriggerGrid();
                }

                ShowMessage($"✅ Đã tải {dt.Rows.Count} Standard, {dtFga.Rows.Count} FGA, {dtTrigger.Rows.Count} Trigger logs cho user {user}");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi tải logs: {ex.Message}");
            }
        }

        private void FormatStandardAuditGrid()
        {
            if (standardAuditGrid.DataSource == null) return;

            try
            {
                foreach (DataGridViewColumn col in standardAuditGrid.Columns)
                {
                    switch (col.Name.ToUpper())
                    {
                        case "USERNAME":
                            col.HeaderText = "👤 User";
                            col.Width = 120;
                            break;
                        case "ACTION_NAME":
                            col.HeaderText = "🔧 Action";
                            col.Width = 100;
                            break;
                        case "OBJ_NAME":
                            col.HeaderText = "📋 Object";
                            col.Width = 150;
                            break;
                        case "RETURNCODE":
                            col.HeaderText = "📊 Return Code";
                            col.Width = 100;
                            break;
                        case "TIME_ACTION":
                            col.HeaderText = "⏰ Thời gian";
                            col.Width = 150;
                            col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi format Standard Audit Grid: {ex.Message}");
            }
        }

        private void FormatFGAGrid()
        {
            if (fgaGrid.DataSource == null) return;

            try
            {
                foreach (DataGridViewColumn col in fgaGrid.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi format FGA Grid: {ex.Message}");
            }
        }

        private void FormatTriggerGrid()
        {
            if (triggerGrid.DataSource == null) return;

            try
            {
                foreach (DataGridViewColumn col in triggerGrid.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi format Trigger Grid: {ex.Message}");
            }
        }
        public AuditLogStatistics GetAuditStatistics()
        {
            AuditLogStatistics stats = new AuditLogStatistics();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = @"
                SELECT
                    SUM(CASE WHEN TRUNC(TIME_ACTION) = TRUNC(SYSDATE) THEN 1 ELSE 0 END) AS TodayLogs,
                    SUM(CASE WHEN RETURNCODE = 0 THEN 1 ELSE 0 END) AS SuccessLogs,
                    SUM(CASE WHEN RETURNCODE <> 0 THEN 1 ELSE 0 END) AS FailedLogs,
                    SUM(CASE WHEN ACTION_NAME IN ('DELETE', 'DROP', 'TRUNCATE') THEN 1 ELSE 0 END) AS SecurityAlerts
                FROM AUDIT_LOGS";

                    using (var command = new OracleCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TodayLogs = reader["TodayLogs"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TodayLogs"]);
                            stats.SuccessLogs = reader["SuccessLogs"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SuccessLogs"]);
                            stats.FailedLogs = reader["FailedLogs"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FailedLogs"]);
                            stats.SecurityAlerts = reader["SecurityAlerts"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SecurityAlerts"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stats.TodayLogs = 0;
                stats.SuccessLogs = 0;
                stats.FailedLogs = 0;
                stats.SecurityAlerts = 0;
            }

            return stats;
        }
    }
}
