using System.Windows.Forms;

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
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_user = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_procedure = new System.Windows.Forms.ComboBox();
            this.cb_user_pro = new System.Windows.Forms.CheckBox();
            this.cb_roles_pro = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_function = new System.Windows.Forms.ComboBox();
            this.cb_user_fun = new System.Windows.Forms.CheckBox();
            this.cb_roles_fun = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_package = new System.Windows.Forms.ComboBox();
            this.cb_user_pk = new System.Windows.Forms.CheckBox();
            this.cb_roles_pk = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_table = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_roles = new System.Windows.Forms.ComboBox();
            this.lbl_table_roles = new System.Windows.Forms.Label();
            this.cb_select_ro = new System.Windows.Forms.CheckBox();
            this.cb_insert_ro = new System.Windows.Forms.CheckBox();
            this.cb_update_ro = new System.Windows.Forms.CheckBox();
            this.cb_delete_ro = new System.Windows.Forms.CheckBox();
            this.btn_Grant_Revoke_Role = new System.Windows.Forms.Button();
            this.dgv_grant_roles = new System.Windows.Forms.DataGridView();
            this.lbl_user = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn_Grant_Login = new System.Windows.Forms.Button();
            this.btn_Revoke_Login = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_username = new System.Windows.Forms.ComboBox();
            this.lbl_table_user = new System.Windows.Forms.Label();
            this.cb_select_us = new System.Windows.Forms.CheckBox();
            this.cb_insert_us = new System.Windows.Forms.CheckBox();
            this.cb_update_us = new System.Windows.Forms.CheckBox();
            this.cb_delete_us = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtg_roles = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.dtg_grant = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_grant_roles)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_roles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_grant)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmb_user);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmb_procedure);
            this.panel1.Controls.Add(this.cb_user_pro);
            this.panel1.Controls.Add(this.cb_roles_pro);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmb_function);
            this.panel1.Controls.Add(this.cb_user_fun);
            this.panel1.Controls.Add(this.cb_roles_fun);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmb_package);
            this.panel1.Controls.Add(this.cb_user_pk);
            this.panel1.Controls.Add(this.cb_roles_pk);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmb_table);
            this.panel1.Location = new System.Drawing.Point(13, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 237);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "User:";
            // 
            // cmb_user
            // 
            this.cmb_user.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_user.FormattingEnabled = true;
            this.cmb_user.Location = new System.Drawing.Point(80, 32);
            this.cmb_user.Name = "cmb_user";
            this.cmb_user.Size = new System.Drawing.Size(140, 28);
            this.cmb_user.TabIndex = 4;
            this.cmb_user.SelectedIndexChanged += new System.EventHandler(this.cmb_user_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Procedure:";
            // 
            // cmb_procedure
            // 
            this.cmb_procedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_procedure.FormattingEnabled = true;
            this.cmb_procedure.Location = new System.Drawing.Point(30, 90);
            this.cmb_procedure.Name = "cmb_procedure";
            this.cmb_procedure.Size = new System.Drawing.Size(215, 28);
            this.cmb_procedure.TabIndex = 5;
            this.cmb_procedure.SelectedIndexChanged += new System.EventHandler(this.cmb_procedure_SelectedIndexChanged);
            // 
            // cb_user_pro
            // 
            this.cb_user_pro.AutoSize = true;
            this.cb_user_pro.Location = new System.Drawing.Point(35, 120);
            this.cb_user_pro.Name = "cb_user_pro";
            this.cb_user_pro.Size = new System.Drawing.Size(98, 24);
            this.cb_user_pro.TabIndex = 13;
            this.cb_user_pro.Text = "Grant user";
            this.cb_user_pro.UseVisualStyleBackColor = true;
            this.cb_user_pro.Click += new System.EventHandler(this.cb_user_pro_Click);
            // 
            // cb_roles_pro
            // 
            this.cb_roles_pro.AutoSize = true;
            this.cb_roles_pro.Location = new System.Drawing.Point(150, 120);
            this.cb_roles_pro.Name = "cb_roles_pro";
            this.cb_roles_pro.Size = new System.Drawing.Size(97, 24);
            this.cb_roles_pro.TabIndex = 14;
            this.cb_roles_pro.Text = "Grant role";
            this.cb_roles_pro.UseVisualStyleBackColor = true;
            this.cb_roles_pro.Click += new System.EventHandler(this.cb_roles_pro_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Function:";
            // 
            // cmb_function
            // 
            this.cmb_function.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_function.FormattingEnabled = true;
            this.cmb_function.Location = new System.Drawing.Point(270, 90);
            this.cmb_function.Name = "cmb_function";
            this.cmb_function.Size = new System.Drawing.Size(225, 28);
            this.cmb_function.TabIndex = 6;
            this.cmb_function.SelectedIndexChanged += new System.EventHandler(this.cmb_function_SelectedIndexChanged);
            // 
            // cb_user_fun
            // 
            this.cb_user_fun.AutoSize = true;
            this.cb_user_fun.Location = new System.Drawing.Point(270, 120);
            this.cb_user_fun.Name = "cb_user_fun";
            this.cb_user_fun.Size = new System.Drawing.Size(98, 24);
            this.cb_user_fun.TabIndex = 15;
            this.cb_user_fun.Text = "Grant user";
            this.cb_user_fun.UseVisualStyleBackColor = true;
            this.cb_user_fun.Click += new System.EventHandler(this.cb_user_fun_Click);
            // 
            // cb_roles_fun
            // 
            this.cb_roles_fun.AutoSize = true;
            this.cb_roles_fun.Location = new System.Drawing.Point(395, 120);
            this.cb_roles_fun.Name = "cb_roles_fun";
            this.cb_roles_fun.Size = new System.Drawing.Size(97, 24);
            this.cb_roles_fun.TabIndex = 16;
            this.cb_roles_fun.Text = "Grant role";
            this.cb_roles_fun.UseVisualStyleBackColor = true;
            this.cb_roles_fun.Click += new System.EventHandler(this.cb_roles_fun_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(520, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Package:";
            // 
            // cmb_package
            // 
            this.cmb_package.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_package.FormattingEnabled = true;
            this.cmb_package.Location = new System.Drawing.Point(520, 90);
            this.cmb_package.Name = "cmb_package";
            this.cmb_package.Size = new System.Drawing.Size(220, 28);
            this.cmb_package.TabIndex = 7;
            this.cmb_package.SelectedIndexChanged += new System.EventHandler(this.cmb_package_SelectedIndexChanged);
            // 
            // cb_user_pk
            // 
            this.cb_user_pk.AutoSize = true;
            this.cb_user_pk.Location = new System.Drawing.Point(515, 120);
            this.cb_user_pk.Name = "cb_user_pk";
            this.cb_user_pk.Size = new System.Drawing.Size(98, 24);
            this.cb_user_pk.TabIndex = 17;
            this.cb_user_pk.Text = "Grant user";
            this.cb_user_pk.UseVisualStyleBackColor = true;
            this.cb_user_pk.Click += new System.EventHandler(this.cb_user_pk_Click);
            // 
            // cb_roles_pk
            // 
            this.cb_roles_pk.AutoSize = true;
            this.cb_roles_pk.Location = new System.Drawing.Point(640, 120);
            this.cb_roles_pk.Name = "cb_roles_pk";
            this.cb_roles_pk.Size = new System.Drawing.Size(97, 24);
            this.cb_roles_pk.TabIndex = 18;
            this.cb_roles_pk.Text = "Grant role";
            this.cb_roles_pk.UseVisualStyleBackColor = true;
            this.cb_roles_pk.Click += new System.EventHandler(this.cb_roles_pk_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(270, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Table:";
            // 
            // cmb_table
            // 
            this.cmb_table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_table.FormattingEnabled = true;
            this.cmb_table.Location = new System.Drawing.Point(270, 190);
            this.cmb_table.Name = "cmb_table";
            this.cmb_table.Size = new System.Drawing.Size(220, 28);
            this.cmb_table.TabIndex = 9;
            this.cmb_table.SelectedIndexChanged += new System.EventHandler(this.cmb_table_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmb_roles);
            this.panel2.Controls.Add(this.lbl_table_roles);
            this.panel2.Controls.Add(this.cb_select_ro);
            this.panel2.Controls.Add(this.cb_insert_ro);
            this.panel2.Controls.Add(this.cb_update_ro);
            this.panel2.Controls.Add(this.cb_delete_ro);
            this.panel2.Controls.Add(this.btn_Grant_Revoke_Role);
            this.panel2.Controls.Add(this.dgv_grant_roles);
            this.panel2.Location = new System.Drawing.Point(765, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(430, 514);
            this.panel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Roles:";
            // 
            // cmb_roles
            // 
            this.cmb_roles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_roles.FormattingEnabled = true;
            this.cmb_roles.Location = new System.Drawing.Point(70, 32);
            this.cmb_roles.Name = "cmb_roles";
            this.cmb_roles.Size = new System.Drawing.Size(340, 28);
            this.cmb_roles.TabIndex = 1;
            this.cmb_roles.SelectedIndexChanged += new System.EventHandler(this.cmb_roles_SelectedIndexChanged);
            // 
            // lbl_table_roles
            // 
            this.lbl_table_roles.AutoSize = true;
            this.lbl_table_roles.Location = new System.Drawing.Point(20, 70);
            this.lbl_table_roles.Name = "lbl_table_roles";
            this.lbl_table_roles.Size = new System.Drawing.Size(47, 20);
            this.lbl_table_roles.TabIndex = 2;
            this.lbl_table_roles.Text = "Table:";
            // 
            // cb_select_ro
            // 
            this.cb_select_ro.AutoSize = true;
            this.cb_select_ro.Location = new System.Drawing.Point(20, 100);
            this.cb_select_ro.Name = "cb_select_ro";
            this.cb_select_ro.Size = new System.Drawing.Size(71, 24);
            this.cb_select_ro.TabIndex = 3;
            this.cb_select_ro.Text = "Select";
            this.cb_select_ro.UseVisualStyleBackColor = true;
            this.cb_select_ro.Click += new System.EventHandler(this.cb_select_ro_Click);
            // 
            // cb_insert_ro
            // 
            this.cb_insert_ro.AutoSize = true;
            this.cb_insert_ro.Location = new System.Drawing.Point(90, 100);
            this.cb_insert_ro.Name = "cb_insert_ro";
            this.cb_insert_ro.Size = new System.Drawing.Size(67, 24);
            this.cb_insert_ro.TabIndex = 4;
            this.cb_insert_ro.Text = "Insert";
            this.cb_insert_ro.UseVisualStyleBackColor = true;
            this.cb_insert_ro.Click += new System.EventHandler(this.cb_insert_ro_Click);
            // 
            // cb_update_ro
            // 
            this.cb_update_ro.AutoSize = true;
            this.cb_update_ro.Location = new System.Drawing.Point(160, 100);
            this.cb_update_ro.Name = "cb_update_ro";
            this.cb_update_ro.Size = new System.Drawing.Size(80, 24);
            this.cb_update_ro.TabIndex = 5;
            this.cb_update_ro.Text = "Update";
            this.cb_update_ro.UseVisualStyleBackColor = true;
            this.cb_update_ro.Click += new System.EventHandler(this.cb_update_ro_Click);
            // 
            // cb_delete_ro
            // 
            this.cb_delete_ro.AutoSize = true;
            this.cb_delete_ro.Location = new System.Drawing.Point(240, 100);
            this.cb_delete_ro.Name = "cb_delete_ro";
            this.cb_delete_ro.Size = new System.Drawing.Size(75, 24);
            this.cb_delete_ro.TabIndex = 6;
            this.cb_delete_ro.Text = "Delete";
            this.cb_delete_ro.UseVisualStyleBackColor = true;
            this.cb_delete_ro.Click += new System.EventHandler(this.cb_delete_ro_Click);
            // 
            // btn_Grant_Revoke_Role
            // 
            this.btn_Grant_Revoke_Role.Location = new System.Drawing.Point(20, 150);
            this.btn_Grant_Revoke_Role.Name = "btn_Grant_Revoke_Role";
            this.btn_Grant_Revoke_Role.Size = new System.Drawing.Size(110, 30);
            this.btn_Grant_Revoke_Role.TabIndex = 8;
            this.btn_Grant_Revoke_Role.Text = "Grant Role";
            this.btn_Grant_Revoke_Role.UseVisualStyleBackColor = true;
            this.btn_Grant_Revoke_Role.Click += new System.EventHandler(this.btn_Grant_Revoke_Role_Click);
            // 
            // dgv_grant_roles
            // 
            this.dgv_grant_roles.AllowUserToAddRows = false;
            this.dgv_grant_roles.AllowUserToDeleteRows = false;
            this.dgv_grant_roles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_grant_roles.BackgroundColor = System.Drawing.Color.White;
            this.dgv_grant_roles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_grant_roles.Location = new System.Drawing.Point(20, 195);
            this.dgv_grant_roles.Name = "dgv_grant_roles";
            this.dgv_grant_roles.ReadOnly = true;
            this.dgv_grant_roles.RowHeadersVisible = false;
            this.dgv_grant_roles.RowHeadersWidth = 51;
            this.dgv_grant_roles.RowTemplate.Height = 24;
            this.dgv_grant_roles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_grant_roles.Size = new System.Drawing.Size(390, 300);
            this.dgv_grant_roles.TabIndex = 2;
            // 
            // lbl_user
            // 
            this.lbl_user.Location = new System.Drawing.Point(0, 0);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(100, 23);
            this.lbl_user.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btn_Grant_Login);
            this.panel3.Controls.Add(this.btn_Revoke_Login);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.cmb_username);
            this.panel3.Controls.Add(this.lbl_table_user);
            this.panel3.Controls.Add(this.cb_select_us);
            this.panel3.Controls.Add(this.cb_insert_us);
            this.panel3.Controls.Add(this.cb_update_us);
            this.panel3.Controls.Add(this.cb_delete_us);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.dtg_roles);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.dtg_grant);
            this.panel3.Location = new System.Drawing.Point(13, 280);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(744, 267);
            this.panel3.TabIndex = 2;
            // 
            // btn_Grant_Login
            // 
            this.btn_Grant_Login.Location = new System.Drawing.Point(260, 20);
            this.btn_Grant_Login.Name = "btn_Grant_Login";
            this.btn_Grant_Login.Size = new System.Drawing.Size(120, 28);
            this.btn_Grant_Login.TabIndex = 30;
            this.btn_Grant_Login.Text = "Cấp quyền login";
            this.btn_Grant_Login.UseVisualStyleBackColor = true;
            this.btn_Grant_Login.Click += new System.EventHandler(this.btn_Grant_Login_Click);
            // 
            // btn_Revoke_Login
            // 
            this.btn_Revoke_Login.Location = new System.Drawing.Point(390, 20);
            this.btn_Revoke_Login.Name = "btn_Revoke_Login";
            this.btn_Revoke_Login.Size = new System.Drawing.Size(140, 28);
            this.btn_Revoke_Login.TabIndex = 31;
            this.btn_Revoke_Login.Text = "Thu hồi quyền login";
            this.btn_Revoke_Login.UseVisualStyleBackColor = true;
            this.btn_Revoke_Login.Click += new System.EventHandler(this.btn_Revoke_Login_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "User:";
            // 
            // cmb_username
            // 
            this.cmb_username.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_username.FormattingEnabled = true;
            this.cmb_username.Location = new System.Drawing.Point(60, 22);
            this.cmb_username.Name = "cmb_username";
            this.cmb_username.Size = new System.Drawing.Size(190, 28);
            this.cmb_username.TabIndex = 19;
            this.cmb_username.SelectedIndexChanged += new System.EventHandler(this.cmb_username_SelectedIndexChanged);
            // 
            // lbl_table_user
            // 
            this.lbl_table_user.AutoSize = true;
            this.lbl_table_user.Location = new System.Drawing.Point(15, 60);
            this.lbl_table_user.Name = "lbl_table_user";
            this.lbl_table_user.Size = new System.Drawing.Size(47, 20);
            this.lbl_table_user.TabIndex = 1;
            this.lbl_table_user.Text = "Table:";
            // 
            // cb_select_us
            // 
            this.cb_select_us.AutoSize = true;
            this.cb_select_us.Location = new System.Drawing.Point(20, 90);
            this.cb_select_us.Name = "cb_select_us";
            this.cb_select_us.Size = new System.Drawing.Size(71, 24);
            this.cb_select_us.TabIndex = 20;
            this.cb_select_us.Text = "Select";
            this.cb_select_us.UseVisualStyleBackColor = true;
            this.cb_select_us.Click += new System.EventHandler(this.cb_select_us_Click);
            // 
            // cb_insert_us
            // 
            this.cb_insert_us.AutoSize = true;
            this.cb_insert_us.Location = new System.Drawing.Point(20, 130);
            this.cb_insert_us.Name = "cb_insert_us";
            this.cb_insert_us.Size = new System.Drawing.Size(67, 24);
            this.cb_insert_us.TabIndex = 21;
            this.cb_insert_us.Text = "Insert";
            this.cb_insert_us.UseVisualStyleBackColor = true;
            this.cb_insert_us.Click += new System.EventHandler(this.cb_insert_us_Click);
            // 
            // cb_update_us
            // 
            this.cb_update_us.AutoSize = true;
            this.cb_update_us.Location = new System.Drawing.Point(20, 170);
            this.cb_update_us.Name = "cb_update_us";
            this.cb_update_us.Size = new System.Drawing.Size(80, 24);
            this.cb_update_us.TabIndex = 22;
            this.cb_update_us.Text = "Update";
            this.cb_update_us.UseVisualStyleBackColor = true;
            this.cb_update_us.Click += new System.EventHandler(this.cb_update_us_Click);
            // 
            // cb_delete_us
            // 
            this.cb_delete_us.AutoSize = true;
            this.cb_delete_us.Location = new System.Drawing.Point(20, 210);
            this.cb_delete_us.Name = "cb_delete_us";
            this.cb_delete_us.Size = new System.Drawing.Size(75, 24);
            this.cb_delete_us.TabIndex = 23;
            this.cb_delete_us.Text = "Delete";
            this.cb_delete_us.UseVisualStyleBackColor = true;
            this.cb_delete_us.Click += new System.EventHandler(this.cb_delete_us_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(260, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 20);
            this.label9.TabIndex = 27;
            this.label9.Text = "Roles";
            // 
            // dtg_roles
            // 
            this.dtg_roles.AllowUserToAddRows = false;
            this.dtg_roles.AllowUserToDeleteRows = false;
            this.dtg_roles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtg_roles.BackgroundColor = System.Drawing.Color.White;
            this.dtg_roles.ColumnHeadersHeight = 29;
            this.dtg_roles.Location = new System.Drawing.Point(260, 90);
            this.dtg_roles.Name = "dtg_roles";
            this.dtg_roles.ReadOnly = true;
            this.dtg_roles.RowHeadersVisible = false;
            this.dtg_roles.RowHeadersWidth = 51;
            this.dtg_roles.RowTemplate.Height = 24;
            this.dtg_roles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtg_roles.Size = new System.Drawing.Size(230, 160);
            this.dtg_roles.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(510, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "Grant";
            // 
            // dtg_grant
            // 
            this.dtg_grant.AllowUserToAddRows = false;
            this.dtg_grant.AllowUserToDeleteRows = false;
            this.dtg_grant.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtg_grant.BackgroundColor = System.Drawing.Color.White;
            this.dtg_grant.ColumnHeadersHeight = 29;
            this.dtg_grant.Location = new System.Drawing.Point(510, 90);
            this.dtg_grant.Name = "dtg_grant";
            this.dtg_grant.ReadOnly = true;
            this.dtg_grant.RowHeadersVisible = false;
            this.dtg_grant.RowHeadersWidth = 51;
            this.dtg_grant.RowTemplate.Height = 24;
            this.dtg_grant.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtg_grant.Size = new System.Drawing.Size(220, 160);
            this.dtg_grant.TabIndex = 25;
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 580);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "AuthorizationForm";
            this.Text = "Authorization Management";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_grant_roles)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_roles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_grant)).EndInit();
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
        private System.Windows.Forms.Button btn_Grant_Login;
        private System.Windows.Forms.Button btn_Revoke_Login;
    }
}