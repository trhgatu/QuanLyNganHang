using QuanLyNganHang.Core;
using QuanLyNganHang.DataAccess;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Profile
{
    public partial class ChangePasswordForm : Form
    {
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmNewPassword;
        private Button btnSave;
        private Button btnCancel;
        private CheckBox chkShowPassword;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblPasswordStrength;

        public ChangePasswordForm()
        {
            this.Text = "ĐỔI MẬT KHẨU";
            this.Font = new Font("Segoe UI", 11F);
            this.Size = new Size(460, 400); // Tăng height để chứa thêm controls
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.KeyPreview = true;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var groupPanel = new GroupBox
            {
                Text = "🔒 Nhập thông tin đổi mật khẩu",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 77, 130),
                Location = new Point(18, 18),
                Size = new Size(400, 220), // Tăng height để chứa thêm controls
                BackColor = Color.White,
                Padding = new Padding(16, 24, 16, 16)
            };

            int labelW = 120;
            int inputW = 200;
            int startX = 20;
            int startY = 34;
            int lineH = 38;

            // Mật khẩu cũ
            Label lblOld = new Label
            {
                Text = "Mật khẩu cũ:",
                Location = new Point(startX, startY),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtOldPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY),
                Size = new Size(inputW, 30),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 10)
            };

            // Mật khẩu mới
            Label lblNew = new Label
            {
                Text = "Mật khẩu mới:",
                Location = new Point(startX, startY + lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtNewPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + lineH),
                Size = new Size(inputW, 30),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 10)
            };
            txtNewPassword.TextChanged += TxtNewPassword_TextChanged;

            // ✅ Thêm: Chỉ báo độ mạnh mật khẩu
            lblPasswordStrength = new Label
            {
                Text = "Độ mạnh: Chưa nhập",
                Location = new Point(startX + labelW + 5, startY + lineH + 25),
                Size = new Size(inputW, 15),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            // Nhập lại mật khẩu mới
            Label lblConfirm = new Label
            {
                Text = "Nhập lại mới:",
                Location = new Point(startX, startY + 2 * lineH + 20),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtConfirmNewPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 2 * lineH + 20),
                Size = new Size(inputW, 30),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 10)
            };

            // ✅ Thêm: Checkbox hiện mật khẩu
            chkShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Location = new Point(startX + labelW + 5, startY + 3 * lineH + 25),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 9)
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            groupPanel.Controls.AddRange(new Control[]
            {
                lblOld, txtOldPassword,
                lblNew, txtNewPassword, lblPasswordStrength,
                lblConfirm, txtConfirmNewPassword,
                chkShowPassword
            });

            // ✅ Thêm: Status label
            lblStatus = new Label
            {
                Text = "✅ Sẵn sàng đổi mật khẩu",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(18, 250),
                Size = new Size(400, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // ✅ Thêm: Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(18, 275),
                Size = new Size(400, 8),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            // Button căn đều, màu đồng bộ
            btnSave = new Button
            {
                Text = "🔒 Đổi mật khẩu",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(130, 38),
                Location = new Point(85, 300),
                BackColor = Color.FromArgb(31, 77, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "✖ Hủy bỏ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(235, 300),
                BackColor = Color.FromArgb(136, 136, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            // ✅ Thêm: Hover effects
            AddButtonHoverEffects(btnSave, Color.FromArgb(31, 77, 130));
            AddButtonHoverEffects(btnCancel, Color.FromArgb(136, 136, 136));

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.AddRange(new Control[] {
                groupPanel, lblStatus, progressBar, btnSave, btnCancel
            });
        }

        // ✅ Thêm: Button hover effects
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

        // ✅ Thêm: Password strength indicator
        private void TxtNewPassword_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordStrength(txtNewPassword.Text);
        }

        private void UpdatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                lblPasswordStrength.Text = "Độ mạnh: Chưa nhập";
                lblPasswordStrength.ForeColor = Color.Gray;
                return;
            }

            int score = 0;
            string strength = "";
            Color color = Color.Red;

            // Length check
            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;

            // Character variety checks
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]")) score++;

            switch (score)
            {
                case 0:
                case 1:
                    strength = "Rất yếu";
                    color = Color.Red;
                    break;
                case 2:
                case 3:
                    strength = "Yếu";
                    color = Color.Orange;
                    break;
                case 4:
                case 5:
                    strength = "Trung bình";
                    color = Color.FromArgb(255, 193, 7);
                    break;
                case 6:
                    strength = "Mạnh";
                    color = Color.Green;
                    break;
            }

            lblPasswordStrength.Text = $"Độ mạnh: {strength}";
            lblPasswordStrength.ForeColor = color;
        }

        // ✅ Thêm: Show/Hide password functionality
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool showPassword = chkShowPassword.Checked;
            txtOldPassword.UseSystemPasswordChar = !showPassword;
            txtNewPassword.UseSystemPasswordChar = !showPassword;
            txtConfirmNewPassword.UseSystemPasswordChar = !showPassword;
        }

        // ✅ Cải tiến: Enhanced validation
        private bool ValidateInputs()
        {
            // Check empty fields
            if (string.IsNullOrWhiteSpace(txtOldPassword.Text))
            {
                ShowError("❌ Vui lòng nhập mật khẩu cũ!");
                txtOldPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                ShowError("❌ Vui lòng nhập mật khẩu mới!");
                txtNewPassword.Focus();
                return false;
            }

            // ✅ Oracle password validation (ít hạn chế hơn)
            if (txtNewPassword.Text.Length < 3)
            {
                ShowError("❌ Mật khẩu Oracle phải có ít nhất 3 ký tự!");
                txtNewPassword.Focus();
                return false;
            }

            // Check password confirmation
            if (txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                ShowError("❌ Mật khẩu mới nhập lại không đúng!");
                txtConfirmNewPassword.Focus();
                return false;
            }

            // Check if new password is different from old
            if (txtOldPassword.Text == txtNewPassword.Text)
            {
                ShowError("❌ Mật khẩu mới phải khác với mật khẩu cũ!");
                txtNewPassword.Focus();
                return false;
            }

            return true;
        }


        // ✅ Tích hợp: Real change password functionality
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            SetControlsEnabled(false);
            ShowInfo("🔄 Đang đổi mật khẩu...");
            progressBar.Visible = true;

            try
            {
                // ✅ Gọi EmployeeDataAccess.ChangePassword với SHA256 hash
                var result = await Task.Run(() =>
                    EmployeeDataAccess.ChangePassword(
                        SessionContext.OracleUser,
                        txtOldPassword.Text,
                        txtNewPassword.Text
                    )
                );

                if (result.success)
                {
                    ShowSuccess("✅ Đổi mật khẩu thành công!");
                    await Task.Delay(1500);

                    MessageBox.Show("✅ Đổi mật khẩu thành công!\nVui lòng sử dụng mật khẩu mới cho lần đăng nhập tiếp theo.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError($"❌ Đổi mật khẩu thất bại: {result.errorMsg}");
                    this.DialogResult = DialogResult.None;
                }
            }
            catch (Exception ex)
            {
                ShowError($"❌ Lỗi hệ thống: {ex.Message}");
                this.DialogResult = DialogResult.None;
            }
            finally
            {
                SetControlsEnabled(true);
                progressBar.Visible = false;
            }
        }

        // ✅ Helper methods
        private void SetControlsEnabled(bool enabled)
        {
            txtOldPassword.Enabled = enabled;
            txtNewPassword.Enabled = enabled;
            txtConfirmNewPassword.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            chkShowPassword.Enabled = enabled;
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

        // ✅ Enhanced keyboard navigation
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (ActiveControl == txtConfirmNewPassword)
                {
                    BtnSave_Click(btnSave, EventArgs.Empty);
                    return true;
                }
                else
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
