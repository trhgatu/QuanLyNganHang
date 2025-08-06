using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.UserManagement
{
    public partial class CreateUserForm : Form
    {
        Create_User u;
        EmployeeDataAccess employeeDataAccess;
        RoleDataAccess roleDataAccess;
        BranchDataAccess branchDataAccess;

        public CreateUserForm()
        {
            
            InitializeComponent();
            CenterToScreen();
            u = new Create_User();
            employeeDataAccess = new EmployeeDataAccess();
            roleDataAccess = new RoleDataAccess();
            branchDataAccess = new BranchDataAccess();
            LoadComboBoxData();
        }

        private void btn_createuser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_username.Text) ||
                string.IsNullOrWhiteSpace(txt_password.Text) ||
                string.IsNullOrWhiteSpace(txt_fullname.Text) ||
                cb_role.SelectedItem == null || cb_branch.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin, chọn chi nhánh và vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txt_username.Text.Trim().ToUpper();
            string password = txt_password.Text.Trim();
            string fullName = txt_fullname.Text.Trim();
            string address = txt_address.Text.Trim();
            string email = txt_email.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string position = txt_position.Text.Trim();
            int branchId = Convert.ToInt32(cb_branch.SelectedValue);
            int roleId = Convert.ToInt32(cb_role.SelectedValue);

            using (var conn = Database.Get_Connect())
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int check = u.Pro_CheckUser(conn, tran, username);

                        bool oracleSuccess = false;

                        if (check == 0)
                        {
                            oracleSuccess = u.Pro_CreateUser(conn, tran, username, password);
                            if (!oracleSuccess)
                            {
                                MessageBox.Show("Tạo USER Oracle thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tran.Rollback();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Tạo USER Oracle thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (check == 1)
                        {
                            DialogResult res = MessageBox.Show($"User {username} đã tồn tại. Có muốn đổi mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo);
                            if (res == DialogResult.Yes)
                            {
                                oracleSuccess = u.Pro_CreateUser(conn, tran, username, password);
                                if (oracleSuccess)
                                {
                                    MessageBox.Show("Đổi mật khẩu Oracle thành công");
                                }
                                else
                                {
                                    MessageBox.Show("Đổi mật khẩu Oracle thất bại");
                                    tran.Rollback();
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
                            tran.Rollback();
                            return;
                        }

                        string passwordHash = HashHelper.HashPassword(password);
                        var (created, errorMsg) = EmployeeDataAccess.CreateFullUser(conn, tran,
                            fullName, email, phone, address, position, branchId, username, passwordHash, roleId);

                        if (created)
                        {
                            tran.Commit();
                            MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            tran.Rollback();
                            MessageBox.Show("Tạo tài khoản thất bại: " + errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Lỗi CỤ THỂ khi tạo user:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void LoadComboBoxData()
        {
            try
            {
                // Load roles
                var roles = roleDataAccess.GetAllRoles();
                if (roles != null && roles.Rows.Count > 0)
                {
                    cb_role.DataSource = roles;
                    cb_role.DisplayMember = "role_name";
                    cb_role.ValueMember = "role_id";
                }

                // Load branches
                var branches = branchDataAccess.GetAllBranches();
                if (branches != null && branches.Rows.Count > 0)
                {
                    branches.Columns.Add("display", typeof(string));

                    foreach (DataRow row in branches.Rows)
                    {
                        string name = row["branch_name"]?.ToString() ?? "";
                        string code = row["branch_code"]?.ToString() ?? "";
                        row["display"] = $"{name} ({code})";
                    }

                    cb_branch.DataSource = branches;
                    cb_branch.DisplayMember = "display";
                    cb_branch.ValueMember = "branch_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
            }
        }


    }
}