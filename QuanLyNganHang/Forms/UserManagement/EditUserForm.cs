using QuanLyNganHang.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLyNganHang.Forms.UserManagement
{
    public partial class EditUserForm : Form
    {
        private UserDataAccess userDataAccess;
        private BranchDataAccess branchDataAccess;
        private RoleDataAccess roleDataAccess;
        private Label label5;
        private TextBox txt_position;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txt_phone;
        private TextBox txt_email;
        private ComboBox cb_branch;
        private ComboBox cb_role;
        private Label lbl_fullname;
        private TextBox txt_fullname;
        private Button btn_save;
        private TextBox txt_password;
        private TextBox txt_username;
        private Label lbl_password;
        private Label lbl_username;
        private Label label6;
        private TextBox txt_oracleuser;
        private Label lbl_CreateUser;
        private int _employeeId;
        private string _username;
        public EditUserForm(int employeeId, string username)
        {
            InitializeComponent();
            _employeeId = employeeId;
            _username = username;
            userDataAccess = new UserDataAccess();
            roleDataAccess = new RoleDataAccess();
            branchDataAccess = new BranchDataAccess();

        }

        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.txt_position = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_phone = new System.Windows.Forms.TextBox();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.cb_branch = new System.Windows.Forms.ComboBox();
            this.cb_role = new System.Windows.Forms.ComboBox();
            this.lbl_fullname = new System.Windows.Forms.Label();
            this.txt_fullname = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.lbl_password = new System.Windows.Forms.Label();
            this.lbl_username = new System.Windows.Forms.Label();
            this.lbl_CreateUser = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_oracleuser = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(235, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 16);
            this.label5.TabIndex = 35;
            this.label5.Text = "Position:";
            // 
            // txt_position
            // 
            this.txt_position.Location = new System.Drawing.Point(319, 343);
            this.txt_position.Name = "txt_position";
            this.txt_position.Size = new System.Drawing.Size(496, 22);
            this.txt_position.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(268, 306);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "phone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 32;
            this.label3.Text = "email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(510, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 31;
            this.label2.Text = "branch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 16);
            this.label1.TabIndex = 30;
            this.label1.Text = "role";
            // 
            // txt_phone
            // 
            this.txt_phone.Location = new System.Drawing.Point(319, 303);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(155, 22);
            this.txt_phone.TabIndex = 29;
            // 
            // txt_email
            // 
            this.txt_email.Location = new System.Drawing.Point(319, 262);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(465, 22);
            this.txt_email.TabIndex = 28;
            // 
            // cb_branch
            // 
            this.cb_branch.FormattingEnabled = true;
            this.cb_branch.Location = new System.Drawing.Point(592, 223);
            this.cb_branch.Name = "cb_branch";
            this.cb_branch.Size = new System.Drawing.Size(121, 24);
            this.cb_branch.TabIndex = 27;
            // 
            // cb_role
            // 
            this.cb_role.FormattingEnabled = true;
            this.cb_role.Location = new System.Drawing.Point(304, 223);
            this.cb_role.Name = "cb_role";
            this.cb_role.Size = new System.Drawing.Size(121, 24);
            this.cb_role.TabIndex = 26;
            // 
            // lbl_fullname
            // 
            this.lbl_fullname.AutoSize = true;
            this.lbl_fullname.Location = new System.Drawing.Point(205, 177);
            this.lbl_fullname.Name = "lbl_fullname";
            this.lbl_fullname.Size = new System.Drawing.Size(78, 16);
            this.lbl_fullname.TabIndex = 25;
            this.lbl_fullname.Text = "Tên đầy đủ:";
            // 
            // txt_fullname
            // 
            this.txt_fullname.Location = new System.Drawing.Point(304, 171);
            this.txt_fullname.Name = "txt_fullname";
            this.txt_fullname.Size = new System.Drawing.Size(465, 22);
            this.txt_fullname.TabIndex = 24;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(599, 371);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(216, 62);
            this.btn_save.TabIndex = 23;
            this.btn_save.Text = "Lưu";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(304, 136);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(465, 22);
            this.txt_password.TabIndex = 22;
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(304, 101);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(146, 22);
            this.txt_username.TabIndex = 21;
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(205, 139);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(70, 16);
            this.lbl_password.TabIndex = 20;
            this.lbl_password.Text = "Password:";
            // 
            // lbl_username
            // 
            this.lbl_username.AutoSize = true;
            this.lbl_username.Location = new System.Drawing.Point(205, 101);
            this.lbl_username.Name = "lbl_username";
            this.lbl_username.Size = new System.Drawing.Size(76, 16);
            this.lbl_username.TabIndex = 19;
            this.lbl_username.Text = "User name:";
            // 
            // lbl_CreateUser
            // 
            this.lbl_CreateUser.AutoSize = true;
            this.lbl_CreateUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CreateUser.Location = new System.Drawing.Point(380, 28);
            this.lbl_CreateUser.Name = "lbl_CreateUser";
            this.lbl_CreateUser.Size = new System.Drawing.Size(359, 42);
            this.lbl_CreateUser.TabIndex = 18;
            this.lbl_CreateUser.Text = "Chỉnh sửa thông tin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(538, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 16);
            this.label6.TabIndex = 36;
            this.label6.Text = "Oracle user:";
            // 
            // txt_oracleuser
            // 
            this.txt_oracleuser.Location = new System.Drawing.Point(623, 98);
            this.txt_oracleuser.Name = "txt_oracleuser";
            this.txt_oracleuser.Size = new System.Drawing.Size(146, 22);
            this.txt_oracleuser.TabIndex = 37;
            // 
            // EditUserForm
            // 
            this.ClientSize = new System.Drawing.Size(1020, 461);
            this.Controls.Add(this.txt_oracleuser);
            this.Controls.Add(this.label6);
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
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.lbl_username);
            this.Controls.Add(this.lbl_CreateUser);
            this.Name = "EditUserForm";
            this.Load += new System.EventHandler(this.EditUserForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            lbl_CreateUser.Text = $"Chỉnh sửa người dùng: {_username}";
            LoadUserInfo();
        }
        private void LoadComboboxes(DataRow row)
        {
            // Load branches
            var branchTable = branchDataAccess.GetAllBranches();
            cb_branch.DataSource = branchTable;
            cb_branch.DisplayMember = "branch_name";
            cb_branch.ValueMember = "branch_id";

            // Load roles
            var roleTable = roleDataAccess.GetAllRoles();
            cb_role.DataSource = roleTable;
            cb_role.DisplayMember = "role_name";
            cb_role.ValueMember = "role_id";

            cb_role.SelectedValue = Convert.ToInt32(row["role_id"]);
            cb_branch.SelectedValue = Convert.ToInt32(row["branch_id"]);
        }
        private void LoadUserInfo()
        {
            var row = userDataAccess.GetUserById(_employeeId);
            if (row != null)
            {
                txt_fullname.Text = row["full_name"]?.ToString();
                txt_email.Text = row["email"]?.ToString();
                txt_phone.Text = row["phone"]?.ToString();
                txt_position.Text = row["position"]?.ToString();
                txt_oracleuser.Text = row["oracle_user"]?.ToString();

                txt_username.Text = row["username"]?.ToString();
                txt_password.Text = "";

                LoadComboboxes(row);
            }
            else
            {
                MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text;
            string oracleUser = txt_oracleuser.Text.Trim();
            string fullName = txt_fullname.Text.Trim();
            string email = txt_email.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string position = txt_position.Text.Trim();
            int branchId = Convert.ToInt32(cb_branch.SelectedValue);
            int roleId = Convert.ToInt32(cb_role.SelectedValue);

            // Gọi tới DataAccess để cập nhật
            var userDA = new UserDataAccess();

            bool result = userDA.UpdateUserInfo(_employeeId, username, password, oracleUser, fullName, email, phone, position, branchId, roleId);

            if (result)
            {
                MessageBox.Show("Cập nhật người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
