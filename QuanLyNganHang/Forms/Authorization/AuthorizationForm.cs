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
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.Forms
{
    public partial class AuthorizationForm : Form
    {
        private AuthorizationManager p;

        public AuthorizationForm()
        {
            InitializeComponent();
            CenterToScreen();
            try
            {
                p = new AuthorizationManager();

                LoadUsers();
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi form load: " + ex.Message);
            }
        }

        private void set_Color_Checkbox_user()
        {
            cb_select_us.ForeColor = cb_select_us.Checked ? Color.Green : Color.Red;
            cb_insert_us.ForeColor = cb_insert_us.Checked ? Color.Green : Color.Red;
            cb_update_us.ForeColor = cb_update_us.Checked ? Color.Green : Color.Red;
            cb_delete_us.ForeColor = cb_delete_us.Checked ? Color.Green : Color.Red;
        }
        private void set_Color_Checkbox_roles()
        {
            cb_select_ro.ForeColor = cb_select_ro.Checked ? Color.Green : Color.Red;
            cb_insert_ro.ForeColor = cb_insert_ro.Checked ? Color.Green : Color.Red;
            cb_update_ro.ForeColor = cb_update_ro.Checked ? Color.Green : Color.Red;
            cb_delete_ro.ForeColor = cb_delete_ro.Checked ? Color.Green : Color.Red;
        }

        private void Set_Color_Grant_User()
        {
            cb_user_pro.ForeColor = cb_user_pro.Checked ? Color.Green : Color.Red;
            cb_user_fun.ForeColor = cb_user_fun.Checked ? Color.Green : Color.Red;
            cb_user_pk.ForeColor = cb_user_pk.Checked ? Color.Green : Color.Red;
        }
        private void Set_Color_Grant_Roles()
        {
            cb_roles_pro.ForeColor = cb_roles_pro.Checked ? Color.Green : Color.Red;
            cb_roles_fun.ForeColor = cb_roles_fun.Checked ? Color.Green : Color.Red;
            cb_roles_pk.ForeColor = cb_roles_pk.Checked ? Color.Green : Color.Red;
        }
        private void Set_Label_Table()
        {
            string tableName = "Table: ";

            if (cmb_table.SelectedItem != null)
            {
                tableName += cmb_table.SelectedItem.ToString();
            }

            lbl_table_roles.Text = tableName;
            lbl_table_user.Text = tableName;
        }
        private void Set_Text_Button(string user, string role)
        {
            int result = p.Get_Roles_User_Check(user, role);

            if (result == 1)
            {
                btn_Grant_Revoke_Role.Text = "Revoke";
            }
            else if (result == 0)
            {
                btn_Grant_Revoke_Role.Text = "Grant";
            }
        }

        private void LoadUsers()
        {
            DataTable table = p.Get_User();
            if (table != null)
            {
                cmb_user.Items.Clear();
                cmb_username.Items.Clear();

                foreach (DataRow row in table.Rows)
                {
                    string username = row[0].ToString();
                    cmb_user.Items.Add(username);
                    cmb_username.Items.Add(username);
                }

                if (cmb_user.Items.Count > 0)
                    cmb_user.SelectedIndex = 0;
                if (cmb_username.Items.Count > 0)
                    cmb_username.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không tải được danh sách người dùng.");
            }
        }

        private void Clear_Combobox()
        {
            cmb_procedure.Items.Clear();
            cmb_function.Items.Clear();
            cmb_package.Items.Clear();
            cmb_table.Items.Clear();
        }

        private void Select_Combobox()
        {
            if (cmb_procedure.Items.Count == 0)
                cmb_procedure.Items.Add("");

            if (cmb_function.Items.Count == 0)
                cmb_function.Items.Add("");

            if (cmb_package.Items.Count == 0)
                cmb_package.Items.Add("");

            if (cmb_table.Items.Count == 0)
                cmb_table.Items.Add("");

            cmb_procedure.SelectedIndex = 0;
            cmb_function.SelectedIndex = 0;
            cmb_package.SelectedIndex = 0;
            cmb_table.SelectedIndex = 0;
        }

        private void LoadRoles()
        {
            DataTable table = p.Get_Roles();
            if (table != null)
            {
                cmb_roles.Items.Clear();

                foreach (DataRow row in table.Rows)
                {
                    cmb_roles.Items.Add(row[0].ToString());
                }

                if (cmb_roles.Items.Count > 0)
                    cmb_roles.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không tải được danh sách Roles.");
            }
        }


        private void SetCheckboxColor(CheckBox cb)
        {
            cb.ForeColor = cb.Checked ? Color.Green : Color.Red;
        }

        private void Load_pro_user(string userowner)
        {
            Clear_Combobox();

            // Load PROCEDURE
            DataTable dtProc = p.Get_Procedure_User(userowner, "PROCEDURE");
            if (dtProc != null)
            {
                foreach (DataRow row in dtProc.Rows)
                {
                    cmb_procedure.Items.Add(row[0].ToString());
                }
            }

            // Load FUNCTION
            DataTable dtFunc = p.Get_Procedure_User(userowner, "FUNCTION");
            if (dtFunc != null)
            {
                foreach (DataRow row in dtFunc.Rows)
                {
                    cmb_function.Items.Add(row[0].ToString());
                }
            }

            // Load PACKAGE
            DataTable dtPack = p.Get_Procedure_User(userowner, "PACKAGE");
            if (dtPack != null)
            {
                foreach (DataRow row in dtPack.Rows)
                {
                    cmb_package.Items.Add(row[0].ToString());
                }
            }

            // Load TABLE
            DataTable dtTable = p.Get_Table_User(userowner);
            if (dtTable != null)
            {
                foreach (DataRow row in dtTable.Rows)
                {
                    cmb_table.Items.Add(row[0].ToString());
                }
            }

            Select_Combobox();
        }

        private void Load_Roles_User(string user)
        {
            dtg_roles.DataSource = p.Get_Roles_User(user);
        }

        private void Load_Grant_User(string user)
        {
            dtg_grant.DataSource = p.Get_Grant_User(user);
        }

        private void Load_Grant_Roles(string roles)
        {
            dgv_grant_roles.DataSource = p.Get_Grant_User(roles);
        }
        private void Load_Grant_Table_User(string user, string userschema, string table)
        {
            DataTable dt = p.Get_Grant(user, userschema, table);

            bool select = false, insert = false, update = false, delete = false;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string privilege = row[0].ToString();

                    if (privilege.Equals("SELECT", StringComparison.OrdinalIgnoreCase))
                        select = true;
                    else if (privilege.Equals("INSERT", StringComparison.OrdinalIgnoreCase))
                        insert = true;
                    else if (privilege.Equals("UPDATE", StringComparison.OrdinalIgnoreCase))
                        update = true;
                    else if (privilege.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
                        delete = true;
                }
            }

            cb_select_us.Checked = select;
            cb_insert_us.Checked = insert;
            cb_update_us.Checked = update;
            cb_delete_us.Checked = delete;
        }

        private void Load_Grant_Table_Roles(string roles, string userschema, string table)
        {
            DataTable dt = p.Get_Grant(roles, userschema, table);

            bool select = false, insert = false, update = false, delete = false;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string privilege = row[0].ToString();

                    if (privilege.Equals("SELECT", StringComparison.OrdinalIgnoreCase))
                        select = true;
                    else if (privilege.Equals("INSERT", StringComparison.OrdinalIgnoreCase))
                        insert = true;
                    else if (privilege.Equals("UPDATE", StringComparison.OrdinalIgnoreCase))
                        update = true;
                    else if (privilege.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
                        delete = true;
                }
            }

            cb_select_ro.Checked = select;
            cb_insert_ro.Checked = insert;
            cb_update_ro.Checked = update;
            cb_delete_ro.Checked = delete;
        }

        private bool Load_Execute(string user_roles, string userschema, string pro)
        {
            DataTable dt = p.Get_Grant(user_roles, userschema, pro);
            bool execute = false;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString().Equals("EXECUTE", StringComparison.OrdinalIgnoreCase))
                    {
                        execute = true;
                        break; // tối ưu: tìm thấy là thoát luôn
                    }
                }
            }

            return execute;
        }

        private void cmb_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userowner = cmb_user.SelectedItem.ToString();
            Load_pro_user(userowner);
        }
        private void cmb_username_SelectedIndexChanged(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem?.ToString() ?? "";
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";

            // Load quyền và vai trò
            Load_Roles_User(user);
            Load_Grant_User(user);
            lbl_user.Text = "User: " + user;

            // Load quyền bảng
            if (cmb_table.SelectedItem != null)
            {
                string table = cmb_table.SelectedItem.ToString();
                Load_Grant_Table_User(user, userschema, table);
                set_Color_Checkbox_user();
            }

            // Load quyền EXECUTE - Procedure
            if (cmb_procedure.SelectedItem != null)
            {
                string procedure = cmb_procedure.SelectedItem.ToString();
                cb_user_pro.Checked = Load_Execute(user, userschema, procedure);
            }

            // Load quyền EXECUTE - Function
            if (cmb_function.SelectedItem != null)
            {
                string function = cmb_function.SelectedItem.ToString();
                cb_user_fun.Checked = Load_Execute(user, userschema, function);
            }

            // Load quyền EXECUTE - Package
            if (cmb_package.SelectedItem != null)
            {
                string package = cmb_package.SelectedItem.ToString();
                isProgrammaticChange = true;
                cb_user_pk.Checked = Load_Execute(user, userschema, package);
                isProgrammaticChange = false;
            }

            // Kiểm tra gán/hủy Role cho User
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                Set_Text_Button(user, role);
            }

            // Hiển thị màu quyền Grant tương ứng
            Set_Color_Grant_User();
        }


        // Event xử lý chọn table thay đổi
        private void cmb_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";
            string table = cmb_table.SelectedItem?.ToString() ?? "";

            // Gán tên bảng hiển thị
            Set_Label_Table();

            // Nếu có user được chọn, load quyền bảng của user
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                Load_Grant_Table_User(user, userschema, table);
                set_Color_Checkbox_user();
            }

            // Nếu có role được chọn, load quyền bảng của role
            if (cmb_roles.SelectedItem != null)
            {
                string roles = cmb_roles.SelectedItem.ToString();
                Load_Grant_Table_Roles(roles, userschema, table);
                set_Color_Checkbox_roles();
            }
        }


        // Event chọn role
        private void cmb_roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";
            string role = cmb_roles.SelectedItem?.ToString() ?? "";

            // Load danh sách quyền Grant từ role
            Load_Grant_Roles(role);

            // Nếu có bảng được chọn, load quyền trên bảng
            if (cmb_table.SelectedItem != null)
            {
                string table = cmb_table.SelectedItem.ToString();
                Load_Grant_Table_Roles(role, userschema, table);
                set_Color_Checkbox_roles();
            }

            // Nếu có procedure được chọn, kiểm tra quyền EXECUTE
            if (cmb_procedure.SelectedItem != null)
            {
                string procedure = cmb_procedure.SelectedItem.ToString();
                cb_roles_pro.Checked = Load_Execute(role, userschema, procedure);
            }

            // Nếu có function được chọn
            if (cmb_function.SelectedItem != null)
            {
                string function = cmb_function.SelectedItem.ToString();
                cb_roles_fun.Checked = Load_Execute(role, userschema, function);
            }

            // Nếu có package được chọn
            if (cmb_package.SelectedItem != null)
            {
                string package = cmb_package.SelectedItem.ToString();
                cb_roles_pk.Checked = Load_Execute(role, userschema, package);
            }

            // Nếu có username được chọn, kiểm tra gán quyền Role cho User
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                Set_Text_Button(user, role);
            }

            // Cập nhật màu checkbox quyền Grant
            Set_Color_Grant_Roles();
        }


        // Event chọn procedure change
        private void cmb_procedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string procedure = cmb_procedure.SelectedItem?.ToString() ?? "";
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";

            // Nếu có user được chọn → kiểm tra quyền EXECUTE
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_pro.Checked = Load_Execute(user, userschema, procedure);
            }

            // Nếu có role được chọn → kiểm tra quyền EXECUTE
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_pro.Checked = Load_Execute(role, userschema, procedure);
            }

            // Cập nhật màu quyền Grant
            Set_Color_Grant_Roles();
            Set_Color_Grant_User();
        }


        private void cmb_function_SelectedIndexChanged(object sender, EventArgs e)
        {
            string function = cmb_function.SelectedItem?.ToString() ?? "";
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";

            // Nếu có user được chọn → kiểm tra quyền EXECUTE
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_fun.Checked = Load_Execute(user, userschema, function);
            }

            // Nếu có role được chọn → kiểm tra quyền EXECUTE
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_fun.Checked = Load_Execute(role, userschema, function);
            }

            // Cập nhật màu checkbox grant tương ứng
            Set_Color_Grant_Roles();
            Set_Color_Grant_User();
        }


        private void cmb_package_SelectedIndexChanged(object sender, EventArgs e)
        {
            string package = cmb_package.SelectedItem?.ToString() ?? "";
            string userschema = cmb_user.SelectedItem?.ToString() ?? "";

            // Nếu có user được chọn → kiểm tra quyền EXECUTE
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_pk.Checked = Load_Execute(user, userschema, package);
            }

            // Nếu có role được chọn → kiểm tra quyền EXECUTE
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_pk.Checked = Load_Execute(role, userschema, package);
            }

            // Cập nhật màu quyền GRANT tương ứng
            Set_Color_Grant_Roles();
            Set_Color_Grant_User();
        }
        private bool Grant_Revoke_pro(
    string user_roles,
    string schema,
    string pro_tab,
    string type_user,
    string type_pro_tab,
    string type,
    bool grant_revoke)
        {
            // Kiểm tra đầu vào rỗng
            if (string.IsNullOrWhiteSpace(pro_tab))
            {
                MessageBox.Show("Mục " + type_pro_tab + " trống!!!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(user_roles))
            {
                MessageBox.Show("Mục " + type_user + " trống!!!");
                return false;
            }

            int dk = grant_revoke ? 1 : 0;

            string actionText = grant_revoke ? "gán quyền" : "hủy quyền";
            string fullMessage = $"Bạn muốn {actionText} {type} {type_pro_tab}: {pro_tab} cho {type_user}: {user_roles} không?";

            DialogResult res = MessageBox.Show(fullMessage, "Thông báo", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                bool result = p.Grant_Revoke_Pro(user_roles, schema, pro_tab, type, dk);

                if (result)
                {
                    MessageBox.Show(grant_revoke ? "Gán quyền thành công" : "Thu hồi quyền thành công");
                    return true;
                }

                return false;
            }

            return false;
        }
        private void Grant_Revoke_Pro_User(ComboBox cmb, CheckBox ckb, string objectType)
        {
            bool isGrant = ckb.Checked;
            string procedure = cmb.SelectedItem?.ToString() ?? "";
            string user = cmb_username.SelectedItem?.ToString() ?? "";
            string schema = cmb_user.SelectedItem?.ToString() ?? "";

            if (Grant_Revoke_pro(user, schema, procedure, "User", objectType, "Execute", isGrant))
            {
                Set_Color_Grant_User();
                Load_Grant_User(user);
            }
            else
            {
                ckb.Checked = !isGrant;
            }
        }
        private void Grant_Revoke_Pro_Role(ComboBox cmb, CheckBox ckb, string objectType)
        {
            bool isGrant = ckb.Checked;
            string procedure = cmb.SelectedItem?.ToString() ?? "";
            string role = cmb_roles.SelectedItem?.ToString() ?? "";
            string schema = cmb_user.SelectedItem?.ToString() ?? "";

            if (Grant_Revoke_pro(role, schema, procedure, "Role", objectType, "Execute", isGrant))
            {
                Set_Color_Grant_Roles();
                Load_Grant_Roles(role);
            }
            else
            {
                ckb.Checked = !isGrant;
            }
        }
        private void Grant_Revoke_Table_User(CheckBox checkbox, string type)
        {
            bool isGrant = checkbox.Checked;

            string table = cmb_table.SelectedItem?.ToString() ?? "";
            string user = cmb_username.SelectedItem?.ToString() ?? "";
            string schema = cmb_user.SelectedItem?.ToString() ?? "";

            if (Grant_Revoke_pro(user, schema, table, "User", "Table", type, isGrant))
            {
                set_Color_Checkbox_user();
            }
            else
            {
                checkbox.Checked = !isGrant; // rollback nếu thất bại
            }
        }

        private void Grant_Revoke_Table_Role(CheckBox checkbox, string type)
        {
            bool isGrant = checkbox.Checked;

            string table = cmb_table.SelectedItem?.ToString() ?? "";
            string role = cmb_roles.SelectedItem?.ToString() ?? "";
            string schema = cmb_user.SelectedItem?.ToString() ?? "";

            if (Grant_Revoke_pro(role, schema, table, "Role", "Table", type, isGrant))
            {
                set_Color_Checkbox_roles();
            }
            else
            {
                checkbox.Checked = !isGrant;
            }
        }


        private void cb_user_pro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_User(cmb_procedure, cb_user_pro, "Procedure");
        }

        private void cb_roles_pro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_Role(cmb_procedure, cb_roles_pro, "Procedure");
        }


        private void cb_user_fun_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_User(cmb_function, cb_user_fun, "Function");
        }

        private void cb_roles_fun_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_Role(cmb_function, cb_roles_fun, "Function");
        }

        private bool isProgrammaticChange = false;

        private async void cb_user_pk_Click(object sender, EventArgs e)
        {
            if (isProgrammaticChange || cb_user_pk.Enabled == false)
                return;

            var ckb = cb_user_pk;
            string procedure = cmb_package.SelectedItem?.ToString() ?? "";
            string user = cmb_username.SelectedItem?.ToString() ?? "";
            string schema = cmb_user.SelectedItem?.ToString() ?? "";
            bool isGrant = ckb.Checked;

            ckb.Enabled = false;

            try
            {
                bool success = await Task.Run(() =>
                {
                    return p.Grant_Revoke_Pro(user, schema, procedure, "Execute", isGrant ? 1 : 0);
                });

                if (success)
                {
                    MessageBox.Show(isGrant ? "Gán quyền thành công" : "Thu hồi quyền thành công");
                    Set_Color_Grant_User();
                    Load_Grant_User(user);
                }
                else
                {
                    // rollback trạng thái checkbox và chặn loop
                    isProgrammaticChange = true;
                    ckb.Checked = !isGrant;
                    isProgrammaticChange = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý: " + ex.Message);
            }
            finally
            {
                ckb.Enabled = true;
            }
        }





        private void cb_roles_pk_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_Role(cmb_package, cb_roles_pk, "Package");
        }
        private void cb_select_us_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_User(cb_select_us, "Select");
        }

        private void cb_insert_us_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_User(cb_insert_us, "Insert");
        }

        private void cb_update_us_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_User(cb_update_us, "Update");
        }

        private void cb_delete_us_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_User(cb_delete_us, "Delete");
        }


        private void cb_select_ro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_Role(cb_select_ro, "Select");
        }

        private void cb_insert_ro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_Role(cb_insert_ro, "Insert");
        }

        private void cb_update_ro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_Role(cb_update_ro, "Update");
        }

        private void cb_delete_ro_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Table_Role(cb_delete_ro, "Delete");
        }
        private bool Grant_Revoke_Role(string user, string role, int dk)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(user))
            {
                MessageBox.Show("Mục User trống!!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Mục Role trống!!");
                return false;
            }
            string action = dk == 1 ? "gán" : "gỡ";
            string message = $"Bạn muốn {action} User: {user} {(dk == 1 ? "vào" : "ra khỏi")} Role: {role} không?";
            DialogResult res = MessageBox.Show(message, "Thông báo", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                if (p.Grant_Revoke_Role(user, role, dk))
                {
                    MessageBox.Show(dk == 1 ? "Gán nhóm quyền thành công" : "Thu hồi nhóm quyền thành công");
                    return true;
                }
                return false;
            }

            return false;
        }

        private void btn_Grant_Revoke_Role_Click(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem?.ToString() ?? "";
            string role = cmb_roles.SelectedItem?.ToString() ?? "";
            bool isGrant = btn_Grant_Revoke_Role.Text.Equals("Grant", StringComparison.OrdinalIgnoreCase);
            int dk = isGrant ? 1 : 0;

            if (Grant_Revoke_Role(user, role, dk))
            {
                Set_Text_Button(user, role); 
                Load_Roles_User(user); 
            }
        }


        private void btn_Grant_Login_Click(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Chưa chọn user.");
                return;
            }

            var confirm = MessageBox.Show($"Bạn muốn cấp quyền đăng nhập cho user '{user}'?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (p.GrantLogin(user))
                    MessageBox.Show("Cấp quyền CREATE SESSION thành công.");
            }
        }
        private void btn_Revoke_Login_Click(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Chưa chọn user.");
                return;
            }

            var confirm = MessageBox.Show($"Bạn muốn thu hồi quyền đăng nhập của user '{user}'?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                if (p.RevokeLogin(user))
                    MessageBox.Show("Đã thu hồi quyền CREATE SESSION thành công.");
            }
        }
        private void btn_Grant_All_Tables_Click(object sender, EventArgs e)
        {
            string username = cmb_username.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng chọn User để gán quyền.");
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn cấp toàn bộ quyền bảng cho user '{username}'?",
                                         "Xác nhận", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                bool success = p.GrantAllTables(username);

                if (success)
                    MessageBox.Show("Đã cấp quyền thành công!");
                else
                    MessageBox.Show("Cấp quyền thất bại!");
            }
        }

        private void cmb_user_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
