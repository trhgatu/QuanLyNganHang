using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.UserManagement
{
    partial class CreateUserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "🛠️ Tạo người dùng mới";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);

            Label title = new Label()
            {
                Text = "👤 Thông tin người dùng mới",
                Font = new Font("Segoe UI Semibold", 16),
                ForeColor = Color.Teal,
                AutoSize = true,
                Location = new Point(200, 20)
            };
            this.Controls.Add(title);

            GroupBox infoBox = new GroupBox()
            {
                Text = "📋 Nhập thông tin",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(620, 330),
                Location = new Point(40, 60),
                BackColor = Color.White
            };
            this.Controls.Add(infoBox);

            // Input fields
            string[] labels = {
        "Tên đăng nhập:", "Mật khẩu:", "Họ tên:", "Email:",
        "Số điện thoại:", "Vị trí:", "Vai trò:", "Chi nhánh:"
    };
            Control[] inputs = {
        txt_username = new TextBox(),
        txt_password = new TextBox() { UseSystemPasswordChar = true },
        txt_fullname = new TextBox(),
        txt_email = new TextBox(),
        txt_phone = new TextBox(),
        txt_position = new TextBox(),
        cb_role = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList },
        cb_branch = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList }
    };

            for (int i = 0; i < labels.Length; i++)
            {
                int top = 30 + (i * 35);
                Label lbl = new Label()
                {
                    Text = labels[i],
                    Location = new Point(20, top + 4),
                    Size = new Size(110, 25)
                };
                Control input = inputs[i];
                input.Location = new Point(140, top);
                input.Size = new Size(440, 28);
                infoBox.Controls.Add(lbl);
                infoBox.Controls.Add(input);
            }

            // Create button
            btn_createuser = new Button()
            {
                Text = "➕ Tạo người dùng",
                Size = new Size(200, 45),
                Location = new Point(240, 400),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn_createuser.FlatAppearance.BorderSize = 0;
            btn_createuser.Click += new EventHandler(this.btn_createuser_Click);
            this.Controls.Add(btn_createuser);
        }


        #endregion

        private System.Windows.Forms.Label lbl_CreateUser;
        private System.Windows.Forms.Label lbl_username;
        private System.Windows.Forms.Label lbl_password;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button btn_createuser;
        private System.Windows.Forms.TextBox txt_fullname;
        private System.Windows.Forms.Label lbl_fullname;
        private System.Windows.Forms.ComboBox cb_role;
        private System.Windows.Forms.ComboBox cb_branch;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.TextBox txt_phone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_position;
        private System.Windows.Forms.Label label5;
    }
}