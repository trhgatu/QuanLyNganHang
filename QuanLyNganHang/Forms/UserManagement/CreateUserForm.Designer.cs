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
            // Form properties
            this.Text = "Tạo người dùng mới - Hệ thống quản lý ngân hàng";
            this.Size = new Size(800, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 248, 250);
            this.Font = new Font("Segoe UI", 9F);

            // Header panel với gradient
            Panel headerPanel = new Panel()
            {
                Size = new Size(800, 80),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(41, 128, 185)
            };
            this.Controls.Add(headerPanel);

            // Title với icon
            Label title = new Label()
            {
                Text = "👤 TẠO NGƯỜI DÙNG MỚI",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(50, 25)
            };
            headerPanel.Controls.Add(title);

            // Main container
            Panel mainContainer = new Panel()
            {
                Size = new Size(740, 480),
                Location = new Point(30, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            // Add subtle shadow effect
            mainContainer.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, mainContainer.ClientRectangle,
                    Color.FromArgb(220, 220, 220), ButtonBorderStyle.Solid);
            };
            this.Controls.Add(mainContainer);

            // Left panel for personal info
            GroupBox personalInfoBox = new GroupBox()
            {
                Text = "📋 Thông tin cá nhân",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Size = new Size(340, 280),
                Location = new Point(20, 20),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            mainContainer.Controls.Add(personalInfoBox);

            // Right panel for system info
            GroupBox systemInfoBox = new GroupBox()
            {
                Text = "⚙️ Thông tin hệ thống",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Size = new Size(340, 280),
                Location = new Point(380, 20),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            mainContainer.Controls.Add(systemInfoBox);

            // Personal info controls
            CreateFormField(personalInfoBox, "Họ và tên:", ref txt_fullname, 30, true);
            CreateFormField(personalInfoBox, "Email:", ref txt_email, 80, true);
            CreateFormField(personalInfoBox, "Số điện thoại:", ref txt_phone, 130, true);
            CreateFormField(personalInfoBox, "Địa chỉ:", ref txt_address, 180, false);

            // System info controls
            CreateFormField(systemInfoBox, "Tên đăng nhập:", ref txt_username, 30, true);
            txt_password = CreatePasswordField(systemInfoBox, "Mật khẩu:", 80);
            CreateFormField(systemInfoBox, "Vị trí:", ref txt_position, 130, false);
            CreateComboField(systemInfoBox, "Vai trò:", ref cb_role, 180);

            // Branch selection (full width)
            CreateComboField(mainContainer, "🏢 Chi nhánh:", ref cb_branch, 320, 680);

            // Action buttons
            Panel buttonPanel = new Panel()
            {
                Size = new Size(700, 60),
                Location = new Point(20, 400),
                BackColor = Color.Transparent
            };
            mainContainer.Controls.Add(buttonPanel);

            // Create button
            btn_createuser = new Button()
            {
                Text = "✅ Tạo người dùng",
                Size = new Size(160, 40),
                Location = new Point(270, 10),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn_createuser.FlatAppearance.BorderSize = 0;
            btn_createuser.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
            btn_createuser.Click += new EventHandler(this.btn_createuser_Click);
            buttonPanel.Controls.Add(btn_createuser);

            // Cancel button
            Button btn_cancel = new Button()
            {
                Text = "❌ Hủy bỏ",
                Size = new Size(120, 40),
                Location = new Point(450, 10),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn_cancel.FlatAppearance.BorderSize = 0;
            btn_cancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43);
            btn_cancel.Click += (s, e) => this.Close();
            buttonPanel.Controls.Add(btn_cancel);

            // Footer
            Label footerLabel = new Label()
            {
                Text = "💡 Vui lòng điền đầy đủ thông tin bắt buộc được đánh dấu (*)",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(40, 600)
            };
            this.Controls.Add(footerLabel);
        }

        // Helper method to create form fields
        private void CreateFormField(Control parent, string labelText, ref TextBox textBox, int top, bool required)
        {
            Label label = new Label()
            {
                Text = labelText + (required ? " *" : ""),
                Location = new Point(15, top),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = required ? Color.FromArgb(231, 76, 60) : Color.FromArgb(52, 73, 94)
            };

            textBox = new TextBox()
            {
                Location = new Point(15, top + 22),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Add placeholder effect
            if (required)
            {
                textBox.BackColor = Color.FromArgb(255, 250, 250);
            }

            parent.Controls.Add(label);
            parent.Controls.Add(textBox);
        }

        // Helper method for password field
        private TextBox CreatePasswordField(Control parent, string labelText, int top)
        {
            Label label = new Label()
            {
                Text = labelText + " *",
                Location = new Point(15, top),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(231, 76, 60)
            };

            TextBox textBox = new TextBox()
            {
                Location = new Point(15, top + 22),
                Size = new Size(270, 25),
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(255, 250, 250)
            };

            Button btnShowPassword = new Button()
            {
                Text = "👁",
                Size = new Size(25, 25),
                Location = new Point(290, top + 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(236, 240, 241),
                Font = new Font("Segoe UI", 8F),
                Cursor = Cursors.Hand
            };
            btnShowPassword.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnShowPassword.Click += (s, e) => {
                textBox.UseSystemPasswordChar = !textBox.UseSystemPasswordChar;
                btnShowPassword.Text = textBox.UseSystemPasswordChar ? "👁" : "🙈";
            };

            parent.Controls.Add(label);
            parent.Controls.Add(textBox);
            parent.Controls.Add(btnShowPassword);

            return textBox; // Return textBox instead of using ref
        }



        // Helper method for combo boxes
        private void CreateComboField(Control parent, string labelText, ref ComboBox comboBox, int top, int width = 300)
        {
            Label label = new Label()
            {
                Text = labelText + " *",
                Location = new Point(15, top),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(231, 76, 60)
            };

            comboBox = new ComboBox()
            {
                Location = new Point(15, top + 22),
                Size = new Size(width, 25),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(255, 250, 250)
            };

            parent.Controls.Add(label);
            parent.Controls.Add(comboBox);
        }

        #endregion

        private System.Windows.Forms.Label lbl_CreateUser;
        private System.Windows.Forms.Label lbl_username;
        private System.Windows.Forms.Label lbl_password;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button btn_createuser;
        private System.Windows.Forms.TextBox txt_fullname;
        private System.Windows.Forms.TextBox txt_address;
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
