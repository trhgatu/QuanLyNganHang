using System;
using System.Drawing;
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

        public ChangePasswordForm()
        {
            this.Text = "ĐỔI MẬT KHẨU";
            this.Font = new Font("Segoe UI", 11F);
            this.Size = new Size(460, 315);
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
                Text = "Nhập thông tin đổi mật khẩu",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 77, 130),
                Location = new Point(18, 18),
                Size = new Size(400, 180),
                BackColor = Color.White,
                Padding = new Padding(16, 24, 16, 16)
            };

            int labelW = 120;
            int inputW = 200;
            int startX = 20;
            int startY = 34;
            int lineH = 38;

            Label lblOld = new Label
            {
                Text = "Mật khẩu cũ:",
                Location = new Point(startX, startY),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtOldPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY),
                Size = new Size(inputW, 30),
                PasswordChar = '●'
            };

            Label lblNew = new Label
            {
                Text = "Mật khẩu mới:",
                Location = new Point(startX, startY + lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtNewPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + lineH),
                Size = new Size(inputW, 30),
                PasswordChar = '●'
            };

            Label lblConfirm = new Label
            {
                Text = "Nhập lại mới:",
                Location = new Point(startX, startY + 2 * lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtConfirmNewPassword = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 2 * lineH),
                Size = new Size(inputW, 30),
                PasswordChar = '●'
            };

            groupPanel.Controls.AddRange(new Control[]
            {
                lblOld, txtOldPassword,
                lblNew, txtNewPassword,
                lblConfirm, txtConfirmNewPassword
            });

            // Button căn đều, màu đồng bộ
            btnSave = new Button
            {
                Text = "🔒 Lưu",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(95, 220),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "✖ Hủy",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(235, 220),
                BackColor = Color.FromArgb(136, 136, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(groupPanel);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOldPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmNewPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ các trường!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                MessageBox.Show("Mật khẩu mới nhập lại không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            // TODO: Xử lý đổi mật khẩu thực tế ở đây
            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
