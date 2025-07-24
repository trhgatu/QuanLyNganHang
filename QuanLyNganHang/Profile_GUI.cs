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

namespace QuanLyNganHang
{
    public partial class Profile_GUI : Form
    {
        private OracleConnection conn;
        private Profile Proc;

        // UI Components
        private Panel headerPanel;
        private Panel mainPanel;
        private Panel footerPanel;
        private Panel controlPanel;
        private Label lblTitle;
        private Label lblSubtitle;
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
                    ShowStatusMessage("Lỗi: Không thể kết nối cơ sở dữ liệu!", StatusType.Error);
                    return;
                }

                Proc = new Profile(conn);
                Load_Combobox();
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"Lỗi khởi tạo: {ex.Message}", StatusType.Error);
            }
        }

        private void InitializeCustomComponents()
        {
            // Form properties
            this.Size = new Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 247);

            CreateHeaderPanel();
            CreateMainPanel();
            CreateFooterPanel();

            // Enable form dragging
            EnableFormDragging();
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

            // Gradient effect for header
            headerPanel.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(31, 81, 139),
                    Color.FromArgb(41, 98, 159),
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            lblTitle = new Label
            {
                Text = "🔐 QUẢN LÝ PROFILE ORACLE",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(600, 35),
                Location = new Point(30, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblSubtitle = new Label
            {
                Text = "Quản lý các profile bảo mật và giới hạn tài nguyên hệ thống",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(600, 25),
                Location = new Point(30, 45),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Close button in header
            Button btnHeaderClose = new Button
            {
                Text = "✖",
                Size = new Size(40, 40),
                Location = new Point(this.Width - 60, 20),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnHeaderClose.FlatAppearance.BorderSize = 0;
            btnHeaderClose.Click += (s, e) => this.Close();

            // Minimize button
            Button btnMinimize = new Button
            {
                Text = "−",
                Size = new Size(40, 40),
                Location = new Point(this.Width - 105, 20),
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            headerPanel.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, btnHeaderClose, btnMinimize });
            this.Controls.Add(headerPanel);
        }

        private void CreateMainPanel()
        {
            mainPanel = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(this.Width, this.Height - 140),
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            CreateControlPanel();
            CreateDataGridView();
            CreateStatusPanel();

            this.Controls.Add(mainPanel);
        }

        private void CreateControlPanel()
        {
            controlPanel = new Panel
            {
                Location = new Point(30, 20),
                Size = new Size(mainPanel.Width - 60, 100),
                BackColor = Color.FromArgb(248, 249, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add border radius effect
            controlPanel.Paint += (s, e) =>
            {
                var rect = controlPanel.ClientRectangle;
                using (var brush = new SolidBrush(Color.FromArgb(248, 249, 250)))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
                using (var pen = new Pen(Color.FromArgb(220, 223, 230), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
                }
            };

            // Profile selection
            lblProfileSelect = new Label
            {
                Text = "📋 Chọn Profile để xem chi tiết:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 15),
                AutoSize = true
            };

            cmb_profile_name = new ComboBox
            {
                Location = new Point(20, 45),
                Size = new Size(280, 35),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmb_profile_name.SelectedIndexChanged += cmb_profile_name_SelectedIndexChanged;

            // Buttons
            btnRefresh = CreateStyledButton("🔄 Làm mới", new Point(320, 45), new Size(120, 35), Color.FromArgb(52, 152, 219));
            btnRefresh.Click += BtnRefresh_Click;

            btnCreateProfile = CreateStyledButton("➕ Tạo Profile", new Point(460, 45), new Size(140, 35), Color.FromArgb(46, 204, 113));
            btnCreateProfile.Click += BtnCreateProfile_Click;

            btnEditProfile = CreateStyledButton("✏️ Sửa Profile", new Point(620, 45), new Size(130, 35), Color.FromArgb(241, 196, 15));
            btnEditProfile.Click += BtnEditProfile_Click;

            btnDeleteProfile = CreateStyledButton("🗑️ Xóa Profile", new Point(770, 45), new Size(130, 35), Color.FromArgb(231, 76, 60));
            btnDeleteProfile.Click += BtnDeleteProfile_Click;

            // Record count label
            lblRecordCount = new Label
            {
                Text = "Tổng số: 0 bản ghi",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(920, 50),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            controlPanel.Controls.AddRange(new Control[] {
                lblProfileSelect, cmb_profile_name, btnRefresh,
                btnCreateProfile, btnEditProfile, btnDeleteProfile, lblRecordCount
            });

            mainPanel.Controls.Add(controlPanel);
        }

        private Button CreateStyledButton(string text, Point location, Size size, Color backColor)
        {
            Button btn = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            // Hover effects
            btn.MouseEnter += (s, e) => btn.BackColor = ChangeColorBrightness(backColor, -0.1f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;

            return btn;
        }

        private void CreateDataGridView()
        {
            dgv_profile = new DataGridView
            {
                Location = new Point(30, 140),
                Size = new Size(mainPanel.Width - 60, mainPanel.Height - 200),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(52, 73, 94),
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 10),
                    Padding = new Padding(8, 4, 8, 4)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(8, 8, 8, 8)
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ColumnHeadersHeight = 50,
                RowTemplate = { Height = 40 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(220, 220, 220)
            };

            // Alternating row colors
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
                Location = new Point(30, mainPanel.Height - 35),
                Size = new Size(mainPanel.Width - 60, 8),
                Style = ProgressBarStyle.Marquee,
                Visible = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Status label
            lblStatus = new Label
            {
                Text = "Sẵn sàng",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                Location = new Point(30, mainPanel.Height - 20),
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            mainPanel.Controls.AddRange(new Control[] { progressBar, lblStatus });
        }

        private void CreateFooterPanel()
        {
            footerPanel = new Panel
            {
                Size = new Size(this.Width, 60),
                BackColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Bottom
            };

            Label footerLabel = new Label
            {
                Text = "© 2025 Hệ thống Quản lý Ngân hàng - Profile Management v1.0",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(500, 30),
                Location = new Point(30, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };

            btnClose = new Button
            {
                Text = "❌ Đóng",
                Size = new Size(120, 35),
                Location = new Point(this.Width - 150, 12),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            footerPanel.Controls.AddRange(new Control[] { footerLabel, btnClose });
            this.Controls.Add(footerPanel);
        }

        private async void Load_Combobox()
        {
            try
            {
                ShowLoading(true, "Đang tải danh sách profile...");

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
                            cmb_profile_name.SelectedIndex = 0;
                            ShowStatusMessage($"Đã tải {read.Rows.Count} profile thành công!", StatusType.Success);
                        }
                        else
                        {
                            ShowStatusMessage("Không tìm thấy profile nào!", StatusType.Warning);
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"Lỗi khi tải danh sách profile: {ex.Message}", StatusType.Error);
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
                ShowLoading(true, "Đang tải chi tiết profile...");

                string profile = cmb_profile_name.SelectedItem.ToString();

                await Task.Run(() =>
                {
                    DataTable profileData = Proc.Get_Profile(profile);

                    this.Invoke(new Action(() =>
                    {
                        dgv_profile.DataSource = profileData;

                        if (profileData != null && profileData.Rows.Count > 0)
                        {
                            ShowStatusMessage($"Đã tải profile '{profile}' thành công!", StatusType.Success);
                        }
                        else
                        {
                            ShowStatusMessage($"Không có dữ liệu cho profile '{profile}'", StatusType.Warning);
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                ShowStatusMessage($"Lỗi khi tải chi tiết profile: {ex.Message}", StatusType.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void Dgv_profile_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Update record count
            lblRecordCount.Text = $"Tổng số: {dgv_profile.Rows.Count} bản ghi";

            // Format columns
            FormatDataGridColumns();
        }

        private void FormatDataGridColumns()
        {
            if (dgv_profile.Columns.Count == 0) return;

            for (int i = 0; i < dgv_profile.Columns.Count; i++)
            {
                var col = dgv_profile.Columns[i];

                switch (col.Name.ToUpper())
                {
                    case "PROFILE":
                        col.HeaderText = "Tên Profile";
                        col.Width = 180;
                        break;
                    case "RESOURCE_NAME":
                        col.HeaderText = "Tài nguyên";
                        col.Width = 250;
                        break;
                    case "RESOURCE_TYPE":
                        col.HeaderText = "Loại tài nguyên";
                        col.Width = 150;
                        break;
                    case "LIMIT":
                        col.HeaderText = "Giới hạn";
                        col.Width = 200;
                        break;
                    default:
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                }
            }
        }

        // Event handlers
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            string currentSelection = cmb_profile_name.SelectedItem?.ToString();
            Load_Combobox();

            if (!string.IsNullOrEmpty(currentSelection) && cmb_profile_name.Items.Contains(currentSelection))
            {
                cmb_profile_name.SelectedItem = currentSelection;
            }
        }

        private void BtnCreateProfile_Click(object sender, EventArgs e)
        {
            ShowStatusMessage("Chức năng tạo profile đang được phát triển", StatusType.Info);
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            if (cmb_profile_name.SelectedItem != null)
            {
                string profileName = cmb_profile_name.SelectedItem.ToString();
                ShowStatusMessage($"Chức năng chỉnh sửa profile '{profileName}' đang được phát triển", StatusType.Info);
            }
            else
            {
                ShowStatusMessage("Vui lòng chọn profile cần chỉnh sửa!", StatusType.Warning);
            }
        }

        private void BtnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (cmb_profile_name.SelectedItem != null)
            {
                string profileName = cmb_profile_name.SelectedItem.ToString();

                if (profileName.ToUpper() == "DEFAULT")
                {
                    ShowStatusMessage("Không thể xóa profile DEFAULT!", StatusType.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa profile '{profileName}'?\n\nLưu ý: Hành động này không thể hoàn tác!",
                    "Xác nhận xóa Profile",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                );

                if (result == DialogResult.Yes)
                {
                    ShowStatusMessage("Chức năng xóa profile đang được phát triển", StatusType.Info);
                }
            }
            else
            {
                ShowStatusMessage("Vui lòng chọn profile cần xóa!", StatusType.Warning);
            }
        }

        // Utility methods
        private void ShowLoading(bool show, string message = "")
        {
            progressBar.Visible = show;
            if (show)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ShowStatusMessage(message, StatusType.Info);
                }
            }
        }

        private void ShowStatusMessage(string message, StatusType type)
        {
            lblStatus.Text = message;
            Color color;
            switch (type)
            {
                case StatusType.Success:
                    color = Color.FromArgb(46, 204, 113);
                    break;
                case StatusType.Warning:
                    color = Color.FromArgb(241, 196, 15);
                    break;
                case StatusType.Error:
                    color = Color.FromArgb(231, 76, 60);
                    break;
                case StatusType.Info:
                    color = Color.FromArgb(52, 152, 219);
                    break;
                default:
                    color = Color.Black;
                    break;
            }
            lblStatus.ForeColor = color;

            // Auto hide after 5 seconds
            Timer hideTimer = new Timer { Interval = 5000 };
            hideTimer.Tick += (s, e) =>
            {
                hideTimer.Stop();
                lblStatus.Text = "Sẵn sàng";
                lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
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

        private void EnableFormDragging()
        {
            bool mouseDown = false;
            Point mouseLocation = Point.Empty; // Initialize to avoid CS0165

            this.MouseDown += (s, e) =>
            {
                mouseDown = true;
                mouseLocation = e.Location;
            };

            this.MouseMove += (s, e) =>
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        this.Location.X + e.X - mouseLocation.X,
                        this.Location.Y + e.Y - mouseLocation.Y);
                }
            };

            this.MouseUp += (s, e) => mouseDown = false;

            // Also enable dragging from header panel
            headerPanel.MouseDown += (s, e) =>
            {
                mouseDown = true;
                mouseLocation = e.Location;
            };

            headerPanel.MouseMove += (s, e) =>
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        this.Location.X + e.X - mouseLocation.X,
                        this.Location.Y + e.Y - mouseLocation.Y);
                }
            };

            headerPanel.MouseUp += (s, e) => mouseDown = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                dgv_profile.DataSource = null;
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
