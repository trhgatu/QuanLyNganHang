using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.UserManagement
{
    public partial class EditUserForm : Form
    {
        private UserDataAccess userDataAccess;
        private BranchDataAccess branchDataAccess;
        private RoleDataAccess roleDataAccess;

        private Label lbl_CreateUser;

        private Label lbl_username, lbl_password, lbl_fullname, lbl_email, lbl_phone, lbl_position, lbl_oracleuser, lbl_role, lbl_branch;
        private TextBox txt_username, txt_password, txt_fullname, txt_email, txt_phone, txt_position, txt_oracleuser;
        private ComboBox cb_role, cb_branch;

        private Button btn_save;

        private int _employeeId;
        private string _username;

        public EditUserForm(int employeeId, string username)
        {
            InitializeComponent();
            _employeeId = employeeId;
            _username = username;

            userDataAccess = new UserDataAccess();
            branchDataAccess = new BranchDataAccess();
            roleDataAccess = new RoleDataAccess();
            lbl_CreateUser.Text = $"Chỉnh sửa người dùng: {_username}";
            LoadUserInfo();
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Chỉnh sửa người dùng";
            this.Size = new Size(800, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);

            int labelX = 20, controlX = 150;
            int baseY = 70;
            int gapY = 45;

            // Title label
            lbl_CreateUser = new Label
            {
                Text = $"Chỉnh sửa người dùng: {_username}",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 81, 139),
                Location = new Point(labelX, 15),
                AutoSize = true
            };
            this.Controls.Add(lbl_CreateUser);

            // Username
            lbl_username = CreateLabel("Tên đăng nhập", labelX, baseY);
            txt_username = CreateTextBox(controlX, baseY - 3, 200);
            txt_username.ReadOnly = true;
            txt_username.BackColor = Color.LightGray;
            this.Controls.Add(lbl_username);
            this.Controls.Add(txt_username);

            // Password
            lbl_password = CreateLabel("Mật khẩu mới", labelX, baseY + gapY);
            txt_password = CreateTextBox(controlX, baseY + gapY - 3, 200);
            txt_password.UseSystemPasswordChar = true;
            this.Controls.Add(lbl_password);
            this.Controls.Add(txt_password);

            // Fullname
            lbl_fullname = CreateLabel("Họ và tên", labelX, baseY + gapY * 2);
            txt_fullname = CreateTextBox(controlX, baseY + gapY * 2 - 3, 420);
            this.Controls.Add(lbl_fullname);
            this.Controls.Add(txt_fullname);

            // Email
            lbl_email = CreateLabel("Email", labelX, baseY + gapY * 3);
            txt_email = CreateTextBox(controlX, baseY + gapY * 3 - 3, 420);
            this.Controls.Add(lbl_email);
            this.Controls.Add(txt_email);

            // Phone
            lbl_phone = CreateLabel("Số điện thoại", labelX, baseY + gapY * 4);
            txt_phone = CreateTextBox(controlX, baseY + gapY * 4 - 3, 220);
            this.Controls.Add(lbl_phone);
            this.Controls.Add(txt_phone);

            // Position
            lbl_position = CreateLabel("Chức vụ", labelX, baseY + gapY * 5);
            txt_position = CreateTextBox(controlX, baseY + gapY * 5 - 3, 420);
            this.Controls.Add(lbl_position);
            this.Controls.Add(txt_position);

            // Oracle User
            lbl_oracleuser = CreateLabel("Oracle User", labelX, baseY + gapY * 6);
            txt_oracleuser = CreateTextBox(controlX, baseY + gapY * 6 - 3, 220);
            this.Controls.Add(lbl_oracleuser);
            this.Controls.Add(txt_oracleuser);

            // Role ComboBox
            lbl_role = CreateLabel("Vai trò", 400, baseY);
            cb_role = new ComboBox
            {
                Location = new Point(460, baseY - 3),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = this.Font
            };
            this.Controls.Add(lbl_role);
            this.Controls.Add(cb_role);

            // Branch ComboBox
            lbl_branch = CreateLabel("Chi nhánh", 400, baseY + gapY);
            cb_branch = new ComboBox
            {
                Location = new Point(460, baseY + gapY - 3),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = this.Font
            };
            this.Controls.Add(lbl_branch);
            this.Controls.Add(cb_branch);

            // Save button
            btn_save = new Button
            {
                Text = "Lưu",
                Location = new Point(480, baseY + gapY * 7),
                Size = new Size(180, 50),
                BackColor = Color.FromArgb(31, 81, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn_save.FlatAppearance.BorderSize = 0;
            btn_save.Click += btn_save_Click;
            this.Controls.Add(btn_save);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = this.Font
            };
        }

        private TextBox CreateTextBox(int x, int y, int width)
        {
            return new TextBox()
            {
                Location = new Point(x, y),
                Size = new Size(width, 28),
                Font = this.Font
            };
        }

        private void LoadUserInfo()
        {
            try
            {
                var row = userDataAccess.GetUserById(_employeeId);
                if (row != null)
                {
                    txt_username.Text = row["username"]?.ToString();
                    txt_password.Text = "";
                    txt_fullname.Text = row["full_name"]?.ToString();
                    txt_email.Text = row["email"]?.ToString();
                    txt_phone.Text = row["phone"]?.ToString();
                    txt_position.Text = row["position"]?.ToString();
                    txt_oracleuser.Text = row["oracle_user"]?.ToString();

                    LoadComboboxes(row);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboboxes(DataRow row)
        {
            var branches = branchDataAccess.GetAllBranches();
            branches.Columns.Add("display", typeof(string));

            foreach (DataRow br in branches.Rows)
            {
                string name = br["branch_name"].ToString();
                string code = br["branch_code"].ToString();
                br["display"] = $"{name} ({code})";
            }

            cb_branch.DataSource = branches;
            cb_branch.DisplayMember = "display";
            cb_branch.ValueMember = "branch_id";

            var roleTable = roleDataAccess.GetAllRoles();
            cb_role.DataSource = roleTable;
            cb_role.DisplayMember = "role_name";
            cb_role.ValueMember = "role_id";

            if (row["role_id"] != DBNull.Value)
                cb_role.SelectedValue = Convert.ToInt32(row["role_id"]);
            else
                cb_role.SelectedIndex = -1;

            if (row["branch_id"] != DBNull.Value)
                cb_branch.SelectedValue = Convert.ToInt32(row["branch_id"]);
            else
                cb_branch.SelectedIndex = -1;
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txt_fullname.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ và tên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_fullname.Focus();
                    return;
                }

                if (cb_branch.SelectedValue == null || cb_role.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh và vai trò!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate email format
                if (!string.IsNullOrWhiteSpace(txt_email.Text) && !IsValidEmail(txt_email.Text))
                {
                    MessageBox.Show("Định dạng email không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_email.Focus();
                    return;
                }

                // Collect data
                string username = txt_username.Text.Trim();
                string password = txt_password.Text; // Có thể để trống nếu không đổi
                string fullName = txt_fullname.Text.Trim();
                string email = txt_email.Text.Trim();
                string phone = txt_phone.Text.Trim();
                string position = txt_position.Text.Trim();
                string oracleUser = txt_oracleuser.Text.Trim();

                int branchId = Convert.ToInt32(cb_branch.SelectedValue);
                int roleId = Convert.ToInt32(cb_role.SelectedValue);

                // Xác nhận cập nhật
                string confirmMessage = $"Xác nhận cập nhật thông tin người dùng '{username}'?";
                if (!string.IsNullOrEmpty(password))
                {
                    confirmMessage += "\n\nLưu ý: Mật khẩu sẽ được thay đổi!";
                }

                if (MessageBox.Show(confirmMessage, "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                // Thực hiện cập nhật
                var userDA = new UserDataAccess();
                bool result = userDA.UpdateUserInfo(_employeeId, username, password, oracleUser,
                    fullName, email, phone, position, branchId, roleId);

                if (result)
                {
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
