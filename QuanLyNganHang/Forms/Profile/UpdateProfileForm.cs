using QuanLyNganHang.Core;
using QuanLyNganHang.DataAccess;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Profile
{
    public partial class UpdateProfileForm : Form
    {
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private Button btnSave;
        private Button btnCancel;
        private Label lblStatus;
        private ProgressBar progressBar;

        public UpdateProfileForm()
        {
            this.Text = "CẬP NHẬT THÔNG TIN CÁ NHÂN";
            this.Font = new Font("Segoe UI", 11F);
            this.Size = new Size(460, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.KeyPreview = true;
            InitializeControls();
            LoadUserData();
        }

        private void InitializeControls()
        {
            var groupProfile = new GroupBox
            {
                Text = "📝 Nhập thông tin mới",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 77, 130),
                Location = new Point(18, 18),
                Size = new Size(400, 225),
                BackColor = Color.White,
                Padding = new Padding(16, 24, 16, 16)
            };

            int labelW = 110;
            int inputW = 220;
            int startX = 20;
            int startY = 35;
            int lineH = 40;

            Label lblFullName = new Label
            {
                Text = "Họ tên:",
                Location = new Point(startX, startY),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtFullName = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY),
                Size = new Size(inputW, 30),
                Font = new Font("Segoe UI", 10)
            };

            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(startX, startY + lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtEmail = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + lineH),
                Size = new Size(inputW, 30),
                Font = new Font("Segoe UI", 10)
            };

            Label lblPhone = new Label
            {
                Text = "Số điện thoại:",
                Location = new Point(startX, startY + 2 * lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtPhone = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 2 * lineH),
                Size = new Size(inputW, 30),
                Font = new Font("Segoe UI", 10)
            };

            Label lblAddress = new Label
            {
                Text = "Địa chỉ:",
                Location = new Point(startX, startY + 3 * lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtAddress = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 3 * lineH),
                Size = new Size(inputW, 50),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            groupProfile.Controls.AddRange(new Control[] {
                lblFullName, txtFullName, lblEmail, txtEmail, lblPhone, txtPhone, lblAddress, txtAddress
            });

            // Status label
            lblStatus = new Label
            {
                Text = "✅ Sẵn sàng cập nhật thông tin",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(18, 250),
                Size = new Size(400, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(18, 275),
                Size = new Size(400, 8),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            // Save button
            btnSave = new Button
            {
                Text = "💾 Lưu thay đổi",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(130, 38),
                Location = new Point(70, 300),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;

            // Cancel button
            btnCancel = new Button
            {
                Text = "✖ Hủy bỏ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(220, 300),
                BackColor = Color.FromArgb(136, 136, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            // Add hover effects
            AddButtonHoverEffects(btnSave, Color.FromArgb(40, 167, 69));
            AddButtonHoverEffects(btnCancel, Color.FromArgb(136, 136, 136));

            // Event handlers
            btnSave.Click += BtnSave_Click;

            // Key handlers
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.AddRange(new Control[] {
                groupProfile, lblStatus, progressBar, btnSave, btnCancel
            });
        }

        private void AddButtonHoverEffects(Button button, Color originalColor)
        {
            button.MouseEnter += (s, e) => button.BackColor = ChangeColorBrightness(originalColor, -0.1f);
            button.MouseLeave += (s, e) => button.BackColor = originalColor;
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

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private void LoadUserData()
        {
            txtFullName.Text = SessionContext.FullName ?? "";
            txtEmail.Text = SessionContext.Email ?? "";
            txtPhone.Text = SessionContext.Phone ?? "";
            txtAddress.Text = SessionContext.Address ?? "";
        }

        private bool ValidateInputs()
        {
            // Validate họ tên (bắt buộc)
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowError("❌ Họ tên không được để trống!");
                txtFullName.Focus();
                return false;
            }

            if (txtFullName.Text.Trim().Length < 2)
            {
                ShowError("❌ Họ tên phải có ít nhất 2 ký tự!");
                txtFullName.Focus();
                return false;
            }

            // Validate email (nếu có nhập)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(txtEmail.Text.Trim(), emailPattern))
                {
                    ShowError("❌ Định dạng email không hợp lệ!");
                    txtEmail.Focus();
                    return false;
                }
            }

            // Validate số điện thoại (nếu có nhập)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                string phonePattern = @"^(0|\+84)[0-9]{8,9}$";
                if (!Regex.IsMatch(txtPhone.Text.Trim(), phonePattern))
                {
                    ShowError("❌ Số điện thoại không hợp lệ! (VD: 0123456789)");
                    txtPhone.Focus();
                    return false;
                }
            }

            return true;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            // Disable controls during processing
            SetControlsEnabled(false);
            ShowInfo("🔄 Đang cập nhật thông tin...");
            progressBar.Visible = true;

            try
            {
                // Prepare data
                string fullName = txtFullName.Text.Trim();
                string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
                string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();

                // Call update method asynchronously
                var result = await Task.Run(() =>
                    EmployeeDataAccess.UpdateProfile(SessionContext.OracleUser, fullName, email, phone, address)
                );

                if (result.success)
                {
                    // Update SessionContext with new data
                    SessionContext.FullName = fullName;
                    SessionContext.Email = email;
                    SessionContext.Phone = phone;
                    SessionContext.Address = address;

                    ShowSuccess("✅ Cập nhật thông tin thành công!");

                    // Auto close after success
                    await Task.Delay(1500);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError($"❌ Cập nhật thất bại: {result.errorMsg}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"❌ Lỗi hệ thống: {ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
                progressBar.Visible = false;
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtFullName.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtPhone.Enabled = enabled;
            txtAddress.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.FromArgb(220, 53, 69);
        }

        private void ShowSuccess(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.FromArgb(40, 167, 69);
        }

        private void ShowInfo(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.FromArgb(23, 162, 184);
        }

        // Handle Enter key in textboxes
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
