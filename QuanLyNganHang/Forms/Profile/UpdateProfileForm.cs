using QuanLyNganHang.Core;
using System;
using System.Drawing;
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

        public UpdateProfileForm()
        {
            this.Text = "CẬP NHẬT THÔNG TIN CÁ NHÂN";
            this.Font = new Font("Segoe UI", 11F);
            this.Size = new Size(460, 350);
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
                Text = "Nhập thông tin mới",
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
                TextAlign = ContentAlignment.MiddleRight
            };
            txtFullName = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY),
                Size = new Size(inputW, 30)
            };

            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(startX, startY + lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtEmail = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + lineH),
                Size = new Size(inputW, 30)
            };

            Label lblPhone = new Label
            {
                Text = "Số điện thoại:",
                Location = new Point(startX, startY + 2 * lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtPhone = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 2 * lineH),
                Size = new Size(inputW, 30)
            };

            Label lblAddress = new Label
            {
                Text = "Địa chỉ:",
                Location = new Point(startX, startY + 3 * lineH),
                Size = new Size(labelW, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            txtAddress = new TextBox
            {
                Location = new Point(startX + labelW + 5, startY + 3 * lineH),
                Size = new Size(inputW, 30)
            };

            groupProfile.Controls.AddRange(new Control[] {
                lblFullName, txtFullName, lblEmail, txtEmail, lblPhone, txtPhone, lblAddress, txtAddress
            });

            // Button chỉnh nằm giữa, to rõ, bo góc và màu sắc đẹp
            btnSave = new Button
            {
                Text = "💾 Lưu",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(80, 260),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "✖ Hủy",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(110, 38),
                Location = new Point(230, 260),
                BackColor = Color.FromArgb(136, 136, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnSave.Click += BtnSave_Click;

            // Xử lý Enter/Hủy bằng phím
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(groupProfile);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadUserData()
        {
            txtFullName.Text = SessionContext.FullName;
            txtEmail.Text = SessionContext.Email;
            txtPhone.Text = SessionContext.Phone;
            txtAddress.Text = SessionContext.Address;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Bạn có thể thêm validate ở đây trước khi đóng form
            // ...

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
