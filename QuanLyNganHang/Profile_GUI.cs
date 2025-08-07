using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang
{
    public partial class Profile_GUI : Form
    {
        private OracleConnection conn;
        private Profile Proc;

        // UI Components - Simplified
        private Panel headerPanel;
        private Panel mainPanel;
        private Label lblTitle;
        private Label lblProfileSelect;
        private ComboBox cmb_profile_name;
        private Button btnRefresh;
        private Button btnClose;
        private Button btnCreateProfile;
        private Button btnDeleteProfile;
        private Button btnEditProfile;
        private DataGridView dgv_profile;
        private Label lblRecordCount;
        private Label lblStatus;
        private ProgressBar progressBar;

        public Profile_GUI()
        {
            InitializeCustomComponents();
            CenterToScreen();

            try
            {
                conn = Database.Get_Connect();
                if (conn == null)
                {
                    ShowStatusMessage("❌ Lỗi: Không thể kết nối cơ sở dữ liệu!", StatusType.Error);
                    return;
                }

                Proc = new Profile(conn);
                Load_Combobox();
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"❌ Lỗi khởi tạo: {ex.Message}", StatusType.Error);
            }
        }

        private void InitializeCustomComponents()
        {
            // Form properties - Simplified
            this.Size = new Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9F);
            this.Text = "🔐 Quản lý Profile Oracle";
            this.MinimumSize = new Size(1000, 600);

            CreateMainPanel();
        }
        private void CreateMainPanel()
        {
            mainPanel = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(this.Width, this.Height - 70),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            CreateControlsPanel();
            CreateDataGridView();
            CreateStatusPanel();

            this.Controls.Add(mainPanel);
        }

        private void CreateControlsPanel()
        {
            Panel controlsPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(mainPanel.Width - 40, 80),
                BackColor = Color.FromArgb(248, 249, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Profile selection
            lblProfileSelect = new Label
            {
                Text = "📋 Chọn Profile:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 15),
                AutoSize = true
            };

            cmb_profile_name = new ComboBox
            {
                Location = new Point(20, 35),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White
            };
            cmb_profile_name.SelectedIndexChanged += cmb_profile_name_SelectedIndexChanged;

            // Buttons in a row
            btnRefresh = CreateSimpleButton("🔄 Làm mới", new Point(290, 35), Color.FromArgb(108, 117, 125));
            btnRefresh.Click += BtnRefresh_Click;
            btnDeleteProfile = CreateSimpleButton("🗑️ Xóa", new Point(620, 35), Color.FromArgb(220, 53, 69));
            btnDeleteProfile.Click += BtnDeleteProfile_Click;

            // Record count
            lblRecordCount = new Label
            {
                Text = "📊 Tổng: 0 bản ghi",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(750, 40),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            controlsPanel.Controls.AddRange(new Control[] {
                lblProfileSelect, cmb_profile_name, btnRefresh, btnCreateProfile,
                btnEditProfile, btnDeleteProfile, lblRecordCount
            });

            mainPanel.Controls.Add(controlsPanel);
        }

        private Button CreateSimpleButton(string text, Point location, Color backColor)
        {
            Button btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 30),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            // Simple hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = ChangeColorBrightness(backColor, -0.1f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;

            return btn;
        }

        private void CreateDataGridView()
        {
            dgv_profile = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(mainPanel.Width - 40, mainPanel.Height - 180),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(52, 73, 94),
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9),
                    Padding = new Padding(8, 5, 8, 5)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(220, 220, 220)
            };

            // Simple alternating row colors
            dgv_profile.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                SelectionBackColor = Color.FromArgb(52, 152, 219)
            };

            dgv_profile.DataBindingComplete += Dgv_profile_DataBindingComplete;

            mainPanel.Controls.Add(dgv_profile);
        }

        private void CreateStatusPanel()
        {
            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, mainPanel.Height - 45),
                Size = new Size(mainPanel.Width - 40, 5),
                Style = ProgressBarStyle.Marquee,
                Visible = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Status label
            lblStatus = new Label
            {
                Text = "✅ Sẵn sàng",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(20, mainPanel.Height - 25),
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            // Close button at bottom
            btnClose = new Button
            {
                Text = "❌ Đóng",
                Size = new Size(100, 30),
                Location = new Point(mainPanel.Width - 140, mainPanel.Height - 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            mainPanel.Controls.AddRange(new Control[] { progressBar, lblStatus, btnClose });
        }

        // Data loading methods remain the same
        private async void Load_Combobox()
        {
            try
            {
                ShowLoading(true, "🔄 Đang tải danh sách profile...");

                cmb_profile_name.Items.Clear();

                await Task.Run(() =>
                {
                    DataTable read = Proc.GetName_Profile();

                    this.Invoke(new Action(() =>
                    {
                        if (read != null && read.Rows.Count > 0)
                        {
                            foreach (DataRow r in read.Rows)
                            {
                                cmb_profile_name.Items.Add(r[0]);
                            }
                            if (cmb_profile_name.Items.Count > 0)
                            {
                                cmb_profile_name.SelectedIndex = 0;
                            }

                            ShowStatusMessage($"✅ Đã tải {read.Rows.Count} profile thành công!", StatusType.Success);
                        }
                        else
                        {
                            ShowStatusMessage("⚠️ Không tìm thấy profile nào!", StatusType.Warning);
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"❌ Lỗi khi tải danh sách profile: {ex.Message}", StatusType.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async void cmb_profile_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_profile_name.SelectedItem == null) return;

            try
            {
                ShowLoading(true, "📊 Đang tải chi tiết profile...");

                string profile = cmb_profile_name.SelectedItem.ToString();

                await Task.Run(() =>
                {
                    DataTable profileData = Proc.Get_Profile(profile);

                    this.Invoke(new Action(() =>
                    {
                        dgv_profile.DataSource = profileData;

                        if (profileData != null && profileData.Rows.Count > 0)
                        {
                            ShowStatusMessage($"✅ Đã tải profile '{profile}' với {profileData.Rows.Count} tham số!", StatusType.Success);
                        }
                        else
                        {
                            ShowStatusMessage($"⚠️ Không có dữ liệu cho profile '{profile}'", StatusType.Warning);
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"❌ Lỗi khi tải chi tiết profile: {ex.Message}", StatusType.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void Dgv_profile_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lblRecordCount.Text = $"📊 Tổng: {dgv_profile.Rows.Count} tham số";
            FormatDataGridColumns();
        }

        private void FormatDataGridColumns()
        {
            if (dgv_profile.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in dgv_profile.Columns)
            {
                switch (col.Name.ToUpper())
                {
                    case "PROFILE":
                        col.HeaderText = "Tên Profile";
                        col.Width = 180;
                        break;
                    case "RESOURCE_NAME":
                        col.HeaderText = "Tên tài nguyên";
                        col.Width = 280;
                        break;
                    case "RESOURCE_TYPE":
                        col.HeaderText = "Loại tài nguyên";
                        col.Width = 150;
                        break;
                    case "LIMIT":
                        col.HeaderText = "Giới hạn";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                }
            }
        }

        // Event handlers - simplified
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            string currentSelection = cmb_profile_name.SelectedItem?.ToString();
            Load_Combobox();

            if (!string.IsNullOrEmpty(currentSelection) && cmb_profile_name.Items.Contains(currentSelection))
            {
                cmb_profile_name.SelectedItem = currentSelection;
            }
        }
        private void BtnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (cmb_profile_name.SelectedItem != null)
            {
                string profileName = cmb_profile_name.SelectedItem.ToString();

                if (profileName.ToUpper() == "DEFAULT")
                {
                    ShowStatusMessage("🚫 Không thể xóa profile DEFAULT!", StatusType.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa profile '{profileName}'?\n\n" +
                    "Cảnh báo: Hành động này không thể hoàn tác!",
                    "Xác nhận xóa Profile",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                );

                if (result == DialogResult.Yes)
                {
                    ShowStatusMessage("🚧 Chức năng xóa profile đang được phát triển", StatusType.Info);
                }
            }
            else
            {
                ShowStatusMessage("⚠️ Vui lòng chọn profile cần xóa!", StatusType.Warning);
            }
        }

        private void ShowLoading(bool show, string message = "")
        {
            progressBar.Visible = show;
            if (show && !string.IsNullOrEmpty(message))
            {
                ShowStatusMessage(message, StatusType.Info);
            }
        }

        private void ShowStatusMessage(string message, StatusType type)
        {
            lblStatus.Text = message;
            Color color;
            switch (type)
            {
                case StatusType.Success:
                    color = Color.FromArgb(40, 167, 69);
                    break;
                case StatusType.Warning:
                    color = Color.FromArgb(255, 193, 7);
                    break;
                case StatusType.Error:
                    color = Color.FromArgb(220, 53, 69);
                    break;
                case StatusType.Info:
                    color = Color.FromArgb(23, 162, 184);
                    break;
                default:
                    color = Color.FromArgb(108, 117, 125);
                    break;
            }
            lblStatus.ForeColor = color;

            // Auto hide after 5 seconds
            Timer hideTimer = new Timer { Interval = 5000 };
            hideTimer.Tick += (s, e) =>
            {
                hideTimer.Stop();
                lblStatus.Text = "✅ Sẵn sàng";
                lblStatus.ForeColor = Color.FromArgb(40, 167, 69);
            };
            hideTimer.Start();
        }

        private Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                dgv_profile.DataSource = null;
                conn?.Close();
            }
            catch { }

            base.OnFormClosing(e);
        }
    }

    public enum StatusType
    {
        Success,
        Warning,
        Error,
        Info
    }
}
