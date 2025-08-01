namespace QuanLyNganHang.Forms
{
    partial class AuthorizationForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_roles_pk = new System.Windows.Forms.CheckBox();
            this.cb_user_pk = new System.Windows.Forms.CheckBox();
            this.cb_roles_fun = new System.Windows.Forms.CheckBox();
            this.cb_user_fun = new System.Windows.Forms.CheckBox();
            this.cb_roles_pro = new System.Windows.Forms.CheckBox();
            this.cb_user_pro = new System.Windows.Forms.CheckBox();
            this.cmb_table = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_package = new System.Windows.Forms.ComboBox();
            this.cmb_function = new System.Windows.Forms.ComboBox();
            this.cmb_procedure = new System.Windows.Forms.ComboBox();
            this.cmb_user = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_grant_roles = new System.Windows.Forms.DataGridView();
            this.btn_Grant_Revoke_Role = new System.Windows.Forms.Button();
            this.lbl_user = new System.Windows.Forms.Label();
            this.cb_delete_ro = new System.Windows.Forms.CheckBox();
            this.cb_update_ro = new System.Windows.Forms.CheckBox();
            this.cb_insert_ro = new System.Windows.Forms.CheckBox();
            this.cb_select_ro = new System.Windows.Forms.CheckBox();
            this.lbl_table_roles = new System.Windows.Forms.Label();
            this.cmb_roles = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtg_grant = new System.Windows.Forms.DataGridView();
            this.dtg_roles = new System.Windows.Forms.DataGridView();
            this.cb_delete_us = new System.Windows.Forms.CheckBox();
            this.cb_update_us = new System.Windows.Forms.CheckBox();
            this.cb_insert_us = new System.Windows.Forms.CheckBox();
            this.cb_select_us = new System.Windows.Forms.CheckBox();
            this.cmb_username = new System.Windows.Forms.ComboBox();
            this.lbl_table_user = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_grant_roles)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_grant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_roles)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_roles_pk);
            this.panel1.Controls.Add(this.cb_user_pk);
            this.panel1.Controls.Add(this.cb_roles_fun);
            this.panel1.Controls.Add(this.cb_user_fun);
            this.panel1.Controls.Add(this.cb_roles_pro);
            this.panel1.Controls.Add(this.cb_user_pro);
            this.panel1.Controls.Add(this.cmb_table);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmb_package);
            this.panel1.Controls.Add(this.cmb_function);
            this.panel1.Controls.Add(this.cmb_procedure);
            this.panel1.Controls.Add(this.cmb_user);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 237);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // cb_roles_pk
            // 
            this.cb_roles_pk.AutoSize = true;
            this.cb_roles_pk.Location = new System.Drawing.Point(646, 119);
            this.cb_roles_pk.Name = "cb_roles_pk";
            this.cb_roles_pk.Size = new System.Drawing.Size(94, 20);
            this.cb_roles_pk.TabIndex = 18;
            this.cb_roles_pk.Text = "Grant roles";
            this.cb_roles_pk.UseVisualStyleBackColor = true;
            this.cb_roles_pk.Click += new System.EventHandler(this.cb_roles_pk_Click);
            // 
            // cb_user_pk
            // 
            this.cb_user_pk.AutoSize = true;
            this.cb_user_pk.Location = new System.Drawing.Point(509, 119);
            this.cb_user_pk.Name = "cb_user_pk";
            this.cb_user_pk.Size = new System.Drawing.Size(90, 20);
            this.cb_user_pk.TabIndex = 17;
            this.cb_user_pk.Text = "Grant user";
            this.cb_user_pk.UseVisualStyleBackColor = true;
            this.cb_user_pk.Click += new System.EventHandler(this.cb_user_pk_Click);
            // 
            // cb_roles_fun
            // 
            this.cb_roles_fun.AutoSize = true;
            this.cb_roles_fun.Location = new System.Drawing.Point(397, 118);
            this.cb_roles_fun.Name = "cb_roles_fun";
            this.cb_roles_fun.Size = new System.Drawing.Size(94, 20);
            this.cb_roles_fun.TabIndex = 16;
            this.cb_roles_fun.Text = "Grant roles";
            this.cb_roles_fun.UseVisualStyleBackColor = true;
            this.cb_roles_fun.Click += new System.EventHandler(this.cb_roles_fun_Click);
            // 
            // cb_user_fun
            // 
            this.cb_user_fun.AutoSize = true;
            this.cb_user_fun.Location = new System.Drawing.Point(268, 119);
            this.cb_user_fun.Name = "cb_user_fun";
            this.cb_user_fun.Size = new System.Drawing.Size(90, 20);
            this.cb_user_fun.TabIndex = 15;
            this.cb_user_fun.Text = "Grant user";
            this.cb_user_fun.UseVisualStyleBackColor = true;
            this.cb_user_fun.Click += new System.EventHandler(this.cb_user_fun_Click);
            // 
            // cb_roles_pro
            // 
            this.cb_roles_pro.AutoSize = true;
            this.cb_roles_pro.Location = new System.Drawing.Point(154, 118);
            this.cb_roles_pro.Name = "cb_roles_pro";
            this.cb_roles_pro.Size = new System.Drawing.Size(94, 20);
            this.cb_roles_pro.TabIndex = 14;
            this.cb_roles_pro.Text = "Grant roles";
            this.cb_roles_pro.UseVisualStyleBackColor = true;
            this.cb_roles_pro.Click += new System.EventHandler(this.cb_roles_pro_Click);
            // 
            // cb_user_pro
            // 
            this.cb_user_pro.AutoSize = true;
            this.cb_user_pro.Location = new System.Drawing.Point(35, 119);
            this.cb_user_pro.Name = "cb_user_pro";
            this.cb_user_pro.Size = new System.Drawing.Size(90, 20);
            this.cb_user_pro.TabIndex = 13;
            this.cb_user_pro.Text = "Grant user";
            this.cb_user_pro.UseVisualStyleBackColor = true;
            this.cb_user_pro.CheckedChanged += new System.EventHandler(this.cb_user_pro_CheckedChanged);
            this.cb_user_pro.Click += new System.EventHandler(this.cb_user_pro_Click);
            // 
            // cmb_table
            // 
            this.cmb_table.FormattingEnabled = true;
            this.cmb_table.Location = new System.Drawing.Point(268, 189);
            this.cmb_table.Name = "cmb_table";
            this.cmb_table.Size = new System.Drawing.Size(194, 24);
            this.cmb_table.TabIndex = 9;
            this.cmb_table.SelectedIndexChanged += new System.EventHandler(this.cmb_table_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(265, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Table";
            // 
            // cmb_package
            // 
            this.cmb_package.FormattingEnabled = true;
            this.cmb_package.Location = new System.Drawing.Point(509, 88);
            this.cmb_package.Name = "cmb_package";
            this.cmb_package.Size = new System.Drawing.Size(232, 24);
            this.cmb_package.TabIndex = 7;
            this.cmb_package.SelectedIndexChanged += new System.EventHandler(this.cmb_package_SelectedIndexChanged);
            // 
            // cmb_function
            // 
            this.cmb_function.FormattingEnabled = true;
            this.cmb_function.Location = new System.Drawing.Point(268, 88);
            this.cmb_function.Name = "cmb_function";
            this.cmb_function.Size = new System.Drawing.Size(224, 24);
            this.cmb_function.TabIndex = 6;
            this.cmb_function.SelectedIndexChanged += new System.EventHandler(this.cmb_function_SelectedIndexChanged);
            // 
            // cmb_procedure
            // 
            this.cmb_procedure.FormattingEnabled = true;
            this.cmb_procedure.Location = new System.Drawing.Point(35, 88);
            this.cmb_procedure.Name = "cmb_procedure";
            this.cmb_procedure.Size = new System.Drawing.Size(214, 24);
            this.cmb_procedure.TabIndex = 5;
            this.cmb_procedure.SelectedIndexChanged += new System.EventHandler(this.cmb_procedure_SelectedIndexChanged);
            // 
            // cmb_user
            // 
            this.cmb_user.FormattingEnabled = true;
            this.cmb_user.Location = new System.Drawing.Point(77, 32);
            this.cmb_user.Name = "cmb_user";
            this.cmb_user.Size = new System.Drawing.Size(121, 24);
            this.cmb_user.TabIndex = 4;
            this.cmb_user.SelectedIndexChanged += new System.EventHandler(this.cmb_user_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(506, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Package";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Function";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Prod:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "User:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_grant_roles);
            this.panel2.Controls.Add(this.btn_Grant_Revoke_Role);
            this.panel2.Controls.Add(this.lbl_user);
            this.panel2.Controls.Add(this.cb_delete_ro);
            this.panel2.Controls.Add(this.cb_update_ro);
            this.panel2.Controls.Add(this.cb_insert_ro);
            this.panel2.Controls.Add(this.cb_select_ro);
            this.panel2.Controls.Add(this.lbl_table_roles);
            this.panel2.Controls.Add(this.cmb_roles);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(764, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 514);
            this.panel2.TabIndex = 1;
            // 
            // dgv_grant_roles
            // 
            this.dgv_grant_roles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_grant_roles.Location = new System.Drawing.Point(18, 204);
            this.dgv_grant_roles.Name = "dgv_grant_roles";
            this.dgv_grant_roles.RowHeadersWidth = 51;
            this.dgv_grant_roles.RowTemplate.Height = 24;
            this.dgv_grant_roles.Size = new System.Drawing.Size(408, 307);
            this.dgv_grant_roles.TabIndex = 2;
            // 
            // btn_Grant_Revoke_Role
            // 
            this.btn_Grant_Revoke_Role.Location = new System.Drawing.Point(18, 163);
            this.btn_Grant_Revoke_Role.Name = "btn_Grant_Revoke_Role";
            this.btn_Grant_Revoke_Role.Size = new System.Drawing.Size(75, 23);
            this.btn_Grant_Revoke_Role.TabIndex = 8;
            this.btn_Grant_Revoke_Role.Text = "Grant";
            this.btn_Grant_Revoke_Role.UseVisualStyleBackColor = true;
            this.btn_Grant_Revoke_Role.Click += new System.EventHandler(this.btn_Grant_Revoke_Role_Click);
            // 
            // lbl_user
            // 
            this.lbl_user.AutoSize = true;
            this.lbl_user.Location = new System.Drawing.Point(15, 136);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(36, 16);
            this.lbl_user.TabIndex = 7;
            this.lbl_user.Text = "User";
            // 
            // cb_delete_ro
            // 
            this.cb_delete_ro.AutoSize = true;
            this.cb_delete_ro.Location = new System.Drawing.Point(275, 99);
            this.cb_delete_ro.Name = "cb_delete_ro";
            this.cb_delete_ro.Size = new System.Drawing.Size(69, 20);
            this.cb_delete_ro.TabIndex = 6;
            this.cb_delete_ro.Text = "Delete";
            this.cb_delete_ro.UseVisualStyleBackColor = true;
            this.cb_delete_ro.Click += new System.EventHandler(this.cb_delete_ro_Click);
            // 
            // cb_update_ro
            // 
            this.cb_update_ro.AutoSize = true;
            this.cb_update_ro.Location = new System.Drawing.Point(181, 99);
            this.cb_update_ro.Name = "cb_update_ro";
            this.cb_update_ro.Size = new System.Drawing.Size(74, 20);
            this.cb_update_ro.TabIndex = 5;
            this.cb_update_ro.Text = "Update";
            this.cb_update_ro.UseVisualStyleBackColor = true;
            this.cb_update_ro.Click += new System.EventHandler(this.cb_update_ro_Click);
            // 
            // cb_insert_ro
            // 
            this.cb_insert_ro.AutoSize = true;
            this.cb_insert_ro.Location = new System.Drawing.Point(102, 99);
            this.cb_insert_ro.Name = "cb_insert_ro";
            this.cb_insert_ro.Size = new System.Drawing.Size(61, 20);
            this.cb_insert_ro.TabIndex = 4;
            this.cb_insert_ro.Text = "Insert";
            this.cb_insert_ro.UseVisualStyleBackColor = true;
            this.cb_insert_ro.Click += new System.EventHandler(this.cb_insert_ro_Click);
            // 
            // cb_select_ro
            // 
            this.cb_select_ro.AutoSize = true;
            this.cb_select_ro.Location = new System.Drawing.Point(18, 99);
            this.cb_select_ro.Name = "cb_select_ro";
            this.cb_select_ro.Size = new System.Drawing.Size(67, 20);
            this.cb_select_ro.TabIndex = 3;
            this.cb_select_ro.Text = "Select";
            this.cb_select_ro.UseVisualStyleBackColor = true;
            this.cb_select_ro.Click += new System.EventHandler(this.cb_select_ro_Click);
            // 
            // lbl_table_roles
            // 
            this.lbl_table_roles.AutoSize = true;
            this.lbl_table_roles.Location = new System.Drawing.Point(15, 69);
            this.lbl_table_roles.Name = "lbl_table_roles";
            this.lbl_table_roles.Size = new System.Drawing.Size(43, 16);
            this.lbl_table_roles.TabIndex = 2;
            this.lbl_table_roles.Text = "Table";
            // 
            // cmb_roles
            // 
            this.cmb_roles.FormattingEnabled = true;
            this.cmb_roles.Location = new System.Drawing.Point(66, 32);
            this.cmb_roles.Name = "cmb_roles";
            this.cmb_roles.Size = new System.Drawing.Size(326, 24);
            this.cmb_roles.TabIndex = 1;
            this.cmb_roles.SelectedIndexChanged += new System.EventHandler(this.cmb_roles_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Roles";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.dtg_grant);
            this.panel3.Controls.Add(this.dtg_roles);
            this.panel3.Controls.Add(this.cb_delete_us);
            this.panel3.Controls.Add(this.cb_update_us);
            this.panel3.Controls.Add(this.cb_insert_us);
            this.panel3.Controls.Add(this.cb_select_us);
            this.panel3.Controls.Add(this.cmb_username);
            this.panel3.Controls.Add(this.lbl_table_user);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(13, 282);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(744, 267);
            this.panel3.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(218, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "Roles";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(525, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "Grant";
            // 
            // dtg_grant
            // 
            this.dtg_grant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_grant.Location = new System.Drawing.Point(372, 89);
            this.dtg_grant.Name = "dtg_grant";
            this.dtg_grant.RowHeadersWidth = 51;
            this.dtg_grant.RowTemplate.Height = 24;
            this.dtg_grant.Size = new System.Drawing.Size(368, 175);
            this.dtg_grant.TabIndex = 25;
            // 
            // dtg_roles
            // 
            this.dtg_roles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_roles.Location = new System.Drawing.Point(131, 89);
            this.dtg_roles.Name = "dtg_roles";
            this.dtg_roles.RowHeadersWidth = 51;
            this.dtg_roles.RowTemplate.Height = 24;
            this.dtg_roles.Size = new System.Drawing.Size(227, 175);
            this.dtg_roles.TabIndex = 24;
            // 
            // cb_delete_us
            // 
            this.cb_delete_us.AutoSize = true;
            this.cb_delete_us.Location = new System.Drawing.Point(35, 198);
            this.cb_delete_us.Name = "cb_delete_us";
            this.cb_delete_us.Size = new System.Drawing.Size(69, 20);
            this.cb_delete_us.TabIndex = 23;
            this.cb_delete_us.Text = "Delete";
            this.cb_delete_us.UseVisualStyleBackColor = true;
            this.cb_delete_us.Click += new System.EventHandler(this.cb_delete_us_Click);
            // 
            // cb_update_us
            // 
            this.cb_update_us.AutoSize = true;
            this.cb_update_us.Location = new System.Drawing.Point(35, 162);
            this.cb_update_us.Name = "cb_update_us";
            this.cb_update_us.Size = new System.Drawing.Size(74, 20);
            this.cb_update_us.TabIndex = 22;
            this.cb_update_us.Text = "Update";
            this.cb_update_us.UseVisualStyleBackColor = true;
            this.cb_update_us.Click += new System.EventHandler(this.cb_update_us_Click);
            // 
            // cb_insert_us
            // 
            this.cb_insert_us.AutoSize = true;
            this.cb_insert_us.Location = new System.Drawing.Point(35, 126);
            this.cb_insert_us.Name = "cb_insert_us";
            this.cb_insert_us.Size = new System.Drawing.Size(61, 20);
            this.cb_insert_us.TabIndex = 21;
            this.cb_insert_us.Text = "Insert";
            this.cb_insert_us.UseVisualStyleBackColor = true;
            this.cb_insert_us.Click += new System.EventHandler(this.cb_insert_us_Click);
            // 
            // cb_select_us
            // 
            this.cb_select_us.AutoSize = true;
            this.cb_select_us.Location = new System.Drawing.Point(35, 89);
            this.cb_select_us.Name = "cb_select_us";
            this.cb_select_us.Size = new System.Drawing.Size(67, 20);
            this.cb_select_us.TabIndex = 20;
            this.cb_select_us.Text = "Select";
            this.cb_select_us.UseVisualStyleBackColor = true;
            this.cb_select_us.Click += new System.EventHandler(this.cb_select_us_Click);
            // 
            // cmb_username
            // 
            this.cmb_username.FormattingEnabled = true;
            this.cmb_username.Location = new System.Drawing.Point(77, 20);
            this.cmb_username.Name = "cmb_username";
            this.cmb_username.Size = new System.Drawing.Size(184, 24);
            this.cmb_username.TabIndex = 19;
            this.cmb_username.SelectedIndexChanged += new System.EventHandler(this.cmb_username_SelectedIndexChanged);
            // 
            // lbl_table_user
            // 
            this.lbl_table_user.AutoSize = true;
            this.lbl_table_user.Location = new System.Drawing.Point(13, 57);
            this.lbl_table_user.Name = "lbl_table_user";
            this.lbl_table_user.Size = new System.Drawing.Size(43, 16);
            this.lbl_table_user.TabIndex = 1;
            this.lbl_table_user.Text = "Table";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "User";
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 564);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AuthorizationForm";
            this.Text = "AuthorizationForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_grant_roles)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_grant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_roles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_roles_pk;
        private System.Windows.Forms.CheckBox cb_user_pk;
        private System.Windows.Forms.CheckBox cb_roles_fun;
        private System.Windows.Forms.CheckBox cb_user_fun;
        private System.Windows.Forms.CheckBox cb_roles_pro;
        private System.Windows.Forms.CheckBox cb_user_pro;
        private System.Windows.Forms.ComboBox cmb_table;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb_package;
        private System.Windows.Forms.ComboBox cmb_function;
        private System.Windows.Forms.ComboBox cmb_procedure;
        private System.Windows.Forms.ComboBox cmb_user;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_grant_roles;
        private System.Windows.Forms.Button btn_Grant_Revoke_Role;
        private System.Windows.Forms.Label lbl_user;
        private System.Windows.Forms.CheckBox cb_delete_ro;
        private System.Windows.Forms.CheckBox cb_update_ro;
        private System.Windows.Forms.CheckBox cb_insert_ro;
        private System.Windows.Forms.CheckBox cb_select_ro;
        private System.Windows.Forms.Label lbl_table_roles;
        private System.Windows.Forms.ComboBox cmb_roles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dtg_grant;
        private System.Windows.Forms.DataGridView dtg_roles;
        private System.Windows.Forms.CheckBox cb_delete_us;
        private System.Windows.Forms.CheckBox cb_update_us;
        private System.Windows.Forms.CheckBox cb_insert_us;
        private System.Windows.Forms.CheckBox cb_select_us;
        private System.Windows.Forms.ComboBox cmb_username;
        private System.Windows.Forms.Label lbl_table_user;
        private System.Windows.Forms.Label label7;
    }
}