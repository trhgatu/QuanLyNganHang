using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.UserManagement
{
    public partial class CreateUserForm : Form
    {
        OracleConnection conn;
        Create_User u;
        EmployeeDataAccess employeeDataAccess;
        RoleDataAccess roleDataAccess;
        BranchDataAccess branchDataAccess;

        public CreateUserForm()
        {
            CenterToScreen();
            InitializeComponent();
            u = new Create_User();
            employeeDataAccess = new EmployeeDataAccess();
            roleDataAccess = new RoleDataAccess();
            branchDataAccess = new BranchDataAccess();
            LoadComboBoxData();
        }

        private void btn_createuser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_username.Text))
            {
                MessageBox.Show("Chưa điền tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_username.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_password.Text))
            {
                MessageBox.Show("Chưa điền mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_password.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_fullname.Text))
            {
                MessageBox.Show("Chưa điền họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_fullname.Focus();
                return;
            }
            if (cb_role.SelectedItem == null || cb_branch.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chi nhánh và vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txt_username.Text.Trim().ToUpper();
            string password = txt_password.Text.Trim();
            string fullName = txt_fullname.Text.Trim();
            string email = txt_email.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string position = txt_position.Text.Trim();
            int branchId = Convert.ToInt32(cb_branch.SelectedValue);
            int roleId = Convert.ToInt32(cb_role.SelectedValue);
            int check = u.Pro_CheckUser(username);
            bool oracleSuccess = false;

            if (check == 0)
            {
                oracleSuccess = u.Pro_CreateUser(username, password);
                if (!oracleSuccess)
                {
                    MessageBox.Show("Tạo user Oracle thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Tạo user Oracle thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (check == 1)
            {
                DialogResult res = MessageBox.Show($"User {username} đã tồn tại. Có muốn đổi mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    oracleSuccess = u.Pro_CreateUser(username, password);
                    if (oracleSuccess)
                    {
                        MessageBox.Show("Đổi mật khẩu Oracle thành công");
                        Database.Set_Database(Database.Host, Database.Port, Database.Sid, username, password);
                        Database.Connect();
                    }
                    else
                    {
                        MessageBox.Show("Đổi mật khẩu Oracle thất bại");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lỗi không xác định khi kiểm tra user Oracle.");
                return;
            }

            try
            {
                if (EmployeeDataAccess.UsernameExists(username, Database.Get_Connect()))
                {
                    MessageBox.Show("Username đã tồn tại trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool created = EmployeeDataAccess.CreateFullUser(
                    fullName, email, phone, "", position, branchId, username, password, roleId
                );

                if (created)
                {
                    MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show("Đăng ký user và gán role thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu người dùng vào hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            var roles = roleDataAccess.GetAllRoles();
            cb_role.DataSource = roles;
            cb_role.DisplayMember = "role_name";
            cb_role.ValueMember = "role_id";
            var branches = branchDataAccess.GetAllBranches();
            cb_branch.DataSource = branches;
            cb_branch.DisplayMember = "branch_name"; 
            cb_branch.ValueMember = "branch_id";
        }

    }
}