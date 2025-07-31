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
            this.lbl_CreateUser = new System.Windows.Forms.Label();
            this.lbl_username = new System.Windows.Forms.Label();
            this.lbl_password = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.btn_createuser = new System.Windows.Forms.Button();
            this.txt_fullname = new System.Windows.Forms.TextBox();
            this.lbl_fullname = new System.Windows.Forms.Label();
            this.cb_role = new System.Windows.Forms.ComboBox();
            this.cb_branch = new System.Windows.Forms.ComboBox();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.txt_phone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_position = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_CreateUser
            // 
            this.lbl_CreateUser.AutoSize = true;
            this.lbl_CreateUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CreateUser.Location = new System.Drawing.Point(258, 31);
            this.lbl_CreateUser.Name = "lbl_CreateUser";
            this.lbl_CreateUser.Size = new System.Drawing.Size(293, 42);
            this.lbl_CreateUser.TabIndex = 0;
            this.lbl_CreateUser.Text = "Tạo người dùng";
            // 
            // lbl_username
            // 
            this.lbl_username.AutoSize = true;
            this.lbl_username.Location = new System.Drawing.Point(83, 104);
            this.lbl_username.Name = "lbl_username";
            this.lbl_username.Size = new System.Drawing.Size(76, 16);
            this.lbl_username.TabIndex = 1;
            this.lbl_username.Text = "User name:";
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(83, 142);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(70, 16);
            this.lbl_password.TabIndex = 2;
            this.lbl_password.Text = "Password:";
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(182, 104);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(465, 22);
            this.txt_username.TabIndex = 3;
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(182, 139);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(465, 22);
            this.txt_password.TabIndex = 4;
            // 
            // btn_createuser
            // 
            this.btn_createuser.Location = new System.Drawing.Point(477, 374);
            this.btn_createuser.Name = "btn_createuser";
            this.btn_createuser.Size = new System.Drawing.Size(216, 62);
            this.btn_createuser.TabIndex = 5;
            this.btn_createuser.Text = "Tạo";
            this.btn_createuser.UseVisualStyleBackColor = true;
            this.btn_createuser.Click += new System.EventHandler(this.btn_createuser_Click);
            // 
            // txt_fullname
            // 
            this.txt_fullname.Location = new System.Drawing.Point(182, 174);
            this.txt_fullname.Name = "txt_fullname";
            this.txt_fullname.Size = new System.Drawing.Size(465, 22);
            this.txt_fullname.TabIndex = 6;
            // 
            // lbl_fullname
            // 
            this.lbl_fullname.AutoSize = true;
            this.lbl_fullname.Location = new System.Drawing.Point(83, 180);
            this.lbl_fullname.Name = "lbl_fullname";
            this.lbl_fullname.Size = new System.Drawing.Size(78, 16);
            this.lbl_fullname.TabIndex = 7;
            this.lbl_fullname.Text = "Tên đầy đủ:";
            // 
            // cb_role
            // 
            this.cb_role.FormattingEnabled = true;
            this.cb_role.Location = new System.Drawing.Point(182, 226);
            this.cb_role.Name = "cb_role";
            this.cb_role.Size = new System.Drawing.Size(121, 24);
            this.cb_role.TabIndex = 8;
            // 
            // cb_branch
            // 
            this.cb_branch.FormattingEnabled = true;
            this.cb_branch.Location = new System.Drawing.Point(470, 226);
            this.cb_branch.Name = "cb_branch";
            this.cb_branch.Size = new System.Drawing.Size(121, 24);
            this.cb_branch.TabIndex = 9;
            // 
            // txt_email
            // 
            this.txt_email.Location = new System.Drawing.Point(197, 265);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(465, 22);
            this.txt_email.TabIndex = 10;
            // 
            // txt_phone
            // 
            this.txt_phone.Location = new System.Drawing.Point(197, 306);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(155, 22);
            this.txt_phone.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "role";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(388, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "branch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "phone";
            // 
            // txt_position
            // 
            this.txt_position.Location = new System.Drawing.Point(197, 346);
            this.txt_position.Name = "txt_position";
            this.txt_position.Size = new System.Drawing.Size(496, 22);
            this.txt_position.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Position:";
            // 
            // CreateUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_position);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_phone);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.cb_branch);
            this.Controls.Add(this.cb_role);
            this.Controls.Add(this.lbl_fullname);
            this.Controls.Add(this.txt_fullname);
            this.Controls.Add(this.btn_createuser);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.lbl_username);
            this.Controls.Add(this.lbl_CreateUser);
            this.Name = "CreateUserForm";
            this.Text = "CreateUserForm";
            this.ResumeLayout(false);
            this.PerformLayout();

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