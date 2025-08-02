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
        private AuthorizationService _authService;

        public AuthorizationForm()
        {
            InitializeComponent();
            CenterToScreen();
            try
            {
                _authService = new AuthorizationService();

                LoadUsers();
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi form load: " + ex.Message);
            }
        }

        // Load danh sách users vào combobox
        private void LoadUsers()
        {
            var users = _authService.GetUsers();

            cmb_user.Items.Clear();
            cmb_username.Items.Clear();

            cmb_user.Items.AddRange(users.ToArray());
            cmb_username.Items.AddRange(users.ToArray());

            if (cmb_user.Items.Count > 0)
                cmb_user.SelectedIndex = 0;

            if (cmb_username.Items.Count > 0)
                cmb_username.SelectedIndex = 0;
        }

        // Load danh sách roles
        private void LoadRoles()
        {
            var roles = _authService.GetRoles();
            cmb_roles.Items.Clear();
            cmb_roles.Items.AddRange(roles.ToArray());
            if (cmb_roles.Items.Count > 0)
                cmb_roles.SelectedIndex = 0;
        }

        // Load procedures, functions, packages, tables cho user chọn
        private void LoadObjectsForUser(string userOwner)
        {
            ClearComboBoxes();

            AddItemsToComboBox(cmb_procedure, _authService.GetProcedures(userOwner, "PROCEDURE"));
            AddItemsToComboBox(cmb_function, _authService.GetProcedures(userOwner, "FUNCTION"));
            AddItemsToComboBox(cmb_package, _authService.GetProcedures(userOwner, "PACKAGE"));
            AddItemsToComboBox(cmb_table, _authService.GetTables(userOwner));

            SelectComboBoxDefault();
        }

        private void ClearComboBoxes()
        {
            cmb_procedure.Items.Clear();
            cmb_function.Items.Clear();
            cmb_package.Items.Clear();
            cmb_table.Items.Clear();
        }

        private void AddItemsToComboBox(ComboBox cmb, List<string> items)
        {
            if (items.Count == 0)
                cmb.Items.Add("");
            else
                cmb.Items.AddRange(items.ToArray());
        }

        private void SelectComboBoxDefault()
        {
            cmb_procedure.SelectedIndex = 0;
            cmb_function.SelectedIndex = 0;
            cmb_package.SelectedIndex = 0;
            cmb_table.SelectedIndex = 0;
        }

        // Cập nhật màu checkbox dựa vào trạng thái Checked
        private void SetCheckboxColor(CheckBox cb)
        {
            cb.ForeColor = cb.Checked ? Color.Green : Color.Red;
        }
        private void UpdateUserCheckboxColors()
        {
            SetCheckboxColor(cb_select_us);
            SetCheckboxColor(cb_insert_us);
            SetCheckboxColor(cb_update_us);
            SetCheckboxColor(cb_delete_us);
        }
        private void UpdateRoleCheckboxColors()
        {
            SetCheckboxColor(cb_select_ro);
            SetCheckboxColor(cb_insert_ro);
            SetCheckboxColor(cb_update_ro);
            SetCheckboxColor(cb_delete_ro);
        }
        private void UpdateGrantUserColors()
        {
            SetCheckboxColor(cb_user_pro);
            SetCheckboxColor(cb_user_fun);
            SetCheckboxColor(cb_user_pk);
        }
        private void UpdateGrantRoleColors()
        {
            SetCheckboxColor(cb_roles_pro);
            SetCheckboxColor(cb_roles_fun);
            SetCheckboxColor(cb_roles_pk);
        }

        private void UpdateLabelTable()
        {
            if (cmb_table.SelectedItem != null)
            {
                string text = "Table: " + cmb_table.SelectedItem.ToString();
                lbl_table_roles.Text = text;
                lbl_table_user.Text = text;
            }
        }

        // Set text nút Grant/Revoke Role
        private void UpdateGrantRevokeRoleButtonText(string user, string role)
        {
            int check = _authService.GetRolesUserCheck(user, role);
            if (check == 1)
                btn_Grant_Revoke_Role.Text = "Revoke Role";
            else if (check == 0)
                btn_Grant_Revoke_Role.Text = "Grant Role";
        }

        // Load data roles user, grants user/roles...
        private void LoadRolesUser(string user)
        {
            dtg_roles.DataSource = _authService.GetRolesUser(user);
        }
        private void LoadGrantUser(string user)
        {
            dtg_grant.DataSource = _authService.GetGrantUser(user);
        }
        private void LoadGrantRoles(string role)
        {
            dgv_grant_roles.DataSource = _authService.GetGrantUser(role);
        }

        // Load grant cho table user hoặc role
        private void LoadGrantTableForUser(string user, string schema, string table)
        {
            var dt = _authService.GetGrant(user, schema, table);
            if (dt == null) return;

            cb_select_us.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "SELECT");
            cb_insert_us.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "INSERT");
            cb_update_us.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "UPDATE");
            cb_delete_us.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "DELETE");
        }
        private void LoadGrantTableForRole(string role, string schema, string table)
        {
            var dt = _authService.GetGrant(role, schema, table);
            if (dt == null) return;

            cb_select_ro.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "SELECT");
            cb_insert_ro.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "INSERT");
            cb_update_ro.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "UPDATE");
            cb_delete_ro.Checked = dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "DELETE");
        }

        // Kiểm tra Execute grant
        private bool HasExecuteGrant(string principal, string schema, string proc)
        {
            var dt = _authService.GetGrant(principal, schema, proc);
            if (dt == null) return false;

            return dt.AsEnumerable().Any(r => r[0].ToString().ToUpper() == "EXECUTE");
        }
        private bool GrantRevokeExecute(string principal, string schema, string proTab, string objectType, bool grant)
        {
            if (string.IsNullOrEmpty(proTab))
            {
                MessageBox.Show($"{objectType} trống!");
                return false;
            }
            if (string.IsNullOrEmpty(principal))
            {
                MessageBox.Show($"Chưa chọn User hoặc Role!");
                return false;
            }

            var res = MessageBox.Show(
                $"Bạn muốn {(grant ? "gán" : "thu hồi")} quyền Execute {objectType} '{proTab}' cho '{principal}'?",
                "Xác nhận", MessageBoxButtons.YesNo);

            if (res != DialogResult.Yes)
                return false;

            bool result = _authService.GrantRevokePro(principal, schema, proTab, "Execute", grant ? 1 : 0);

            if (result)
                MessageBox.Show($"{(grant ? "Gán" : "Thu hồi")} quyền thành công.");

            return result;
        }

        // Hàm chung cho cấp/thu hồi quyền Table SELECT/INSERT/UPDATE/DELETE
        private bool GrantRevokeTable(string principal, string schema, string table, string privilege, bool grant)
        {
            if (string.IsNullOrEmpty(table))
            {
                MessageBox.Show("Chưa chọn Table!");
                return false;
            }
            if (string.IsNullOrEmpty(principal))
            {
                MessageBox.Show("Chưa chọn User hoặc Role!");
                return false;
            }

            var res = MessageBox.Show(
                $"Bạn muốn {(grant ? "gán" : "thu hồi")} quyền {privilege} cho Table '{table}' đến '{principal}'?",
                "Xác nhận", MessageBoxButtons.YesNo);

            if (res != DialogResult.Yes)
                return false;

            bool result = _authService.GrantRevokePro(principal, schema, table, privilege.ToUpper(), grant ? 1 : 0);


            if (result)
                MessageBox.Show($"{(grant ? "Gán" : "Thu hồi")} quyền thành công.");

            return result;
        }
        private bool GrantRevokeUserRole(string user, string role, bool grant)
        {
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Chưa chọn User!");
                return false;
            }
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Chưa chọn Role!");
                return false;
            }

            var res = MessageBox.Show(
                $"Bạn muốn {(grant ? "gán" : "thu hồi")} Role '{role}' cho User '{user}'?",
                "Xác nhận", MessageBoxButtons.YesNo);

            if (res != DialogResult.Yes)
                return false;

            bool result = _authService.GrantRevokeRole(user, role, grant ? 1 : 0);

            if (result)
                MessageBox.Show($"{(grant ? "Gán" : "Thu hồi")} Role thành công.");

            return result;
        }

        // Event xử lý khi chọn user owner thay đổi
        private void cmb_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userOwner = cmb_user.SelectedItem.ToString();
            LoadObjectsForUser(userOwner);
        }

        // Event xử lý chọn user để load roles, grants, UI
        private void cmb_username_SelectedIndexChanged(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            LoadRolesUser(user);
            LoadGrantUser(user);
            lbl_user.Text = "User: " + user;

            if (cmb_table.SelectedItem != null)
            {
                LoadGrantTableForUser(user, schema, cmb_table.SelectedItem.ToString());
                UpdateUserCheckboxColors();
            }

            if (cmb_procedure.SelectedItem != null)
                cb_user_pro.Checked = HasExecuteGrant(user, schema, cmb_procedure.SelectedItem.ToString());
            if (cmb_function.SelectedItem != null)
                cb_user_fun.Checked = HasExecuteGrant(user, schema, cmb_function.SelectedItem.ToString());
            if (cmb_package.SelectedItem != null)
                cb_user_pk.Checked = HasExecuteGrant(user, schema, cmb_package.SelectedItem.ToString());

            if (cmb_roles.SelectedItem != null)
                UpdateGrantRevokeRoleButtonText(user, cmb_roles.SelectedItem.ToString());

            UpdateGrantUserColors();
        }

        // Event xử lý chọn table thay đổi
        private void cmb_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            string schema = cmb_user.SelectedItem.ToString();
            string table = cmb_table.SelectedItem.ToString();

            UpdateLabelTable();

            if (cmb_username.SelectedItem != null)
            {
                LoadGrantTableForUser(cmb_username.SelectedItem.ToString(), schema, table);
                UpdateUserCheckboxColors();
            }
            if (cmb_roles.SelectedItem != null)
            {
                LoadGrantTableForRole(cmb_roles.SelectedItem.ToString(), schema, table);
                UpdateRoleCheckboxColors();
            }
        }

        // Event chọn role
        private void cmb_roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string schema = cmb_user.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();

            LoadGrantRoles(role);

            if (cmb_table.SelectedItem != null)
            {
                LoadGrantTableForRole(role, schema, cmb_table.SelectedItem.ToString());
                UpdateRoleCheckboxColors();
            }
            if (cmb_procedure.SelectedItem != null)
                cb_roles_pro.Checked = HasExecuteGrant(role, schema, cmb_procedure.SelectedItem.ToString());
            if (cmb_function.SelectedItem != null)
                cb_roles_fun.Checked = HasExecuteGrant(role, schema, cmb_function.SelectedItem.ToString());
            if (cmb_package.SelectedItem != null)
                cb_roles_pk.Checked = HasExecuteGrant(role, schema, cmb_package.SelectedItem.ToString());

            if (cmb_username.SelectedItem != null)
                UpdateGrantRevokeRoleButtonText(cmb_username.SelectedItem.ToString(), role);

            UpdateGrantRoleColors();
        }

        // Event chọn procedure change
        private void cmb_procedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string procedure = cmb_procedure.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            if (cmb_username.SelectedItem != null)
                cb_user_pro.Checked = HasExecuteGrant(cmb_username.SelectedItem.ToString(), schema, procedure);

            if (cmb_roles.SelectedItem != null)
                cb_roles_pro.Checked = HasExecuteGrant(cmb_roles.SelectedItem.ToString(), schema, procedure);

            UpdateGrantUserColors();
            UpdateGrantRoleColors();
        }

        private void cmb_function_SelectedIndexChanged(object sender, EventArgs e)
        {
            string function = cmb_function.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            if (cmb_username.SelectedItem != null)
                cb_user_fun.Checked = HasExecuteGrant(cmb_username.SelectedItem.ToString(), schema, function);
            if (cmb_roles.SelectedItem != null)
                cb_roles_fun.Checked = HasExecuteGrant(cmb_roles.SelectedItem.ToString(), schema, function);

            UpdateGrantUserColors();
            UpdateGrantRoleColors();
        }

        private void cmb_package_SelectedIndexChanged(object sender, EventArgs e)
        {
            string package = cmb_package.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            if (cmb_username.SelectedItem != null)
                cb_user_pk.Checked = HasExecuteGrant(cmb_username.SelectedItem.ToString(), schema, package);
            if (cmb_roles.SelectedItem != null)
                cb_roles_pk.Checked = HasExecuteGrant(cmb_roles.SelectedItem.ToString(), schema, package);

            UpdateGrantUserColors();
            UpdateGrantRoleColors();
        }

        // Click các checkbox thực hiện grant/revoke

        private void cb_user_pro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_user_pro, () =>
                GrantRevokeExecute(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_procedure.SelectedItem?.ToString(), "Procedure", cb_user_pro.Checked),
                () => LoadGrantUser(cmb_username.SelectedItem?.ToString()));
        }

        private void cb_roles_pro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_roles_pro, () =>
                GrantRevokeExecute(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_procedure.SelectedItem?.ToString(), "Procedure", cb_roles_pro.Checked),
                () => LoadGrantRoles(cmb_roles.SelectedItem?.ToString()));
        }

        private void cb_user_fun_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_user_fun, () =>
                GrantRevokeExecute(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_function.SelectedItem?.ToString(), "Function", cb_user_fun.Checked),
                () => LoadGrantUser(cmb_username.SelectedItem?.ToString()));
        }


        private void cb_roles_fun_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_roles_fun, () =>
                GrantRevokeExecute(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_function.SelectedItem?.ToString(), "Function", cb_roles_fun.Checked),
                () => LoadGrantRoles(cmb_roles.SelectedItem?.ToString()));
        }

        private void cb_user_pk_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_user_pk, () =>
                GrantRevokeExecute(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_package.SelectedItem?.ToString(), "Package", cb_user_pk.Checked),
                () => LoadGrantUser(cmb_username.SelectedItem?.ToString()));
        }
        private void cb_roles_pk_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_roles_pk,
                () => GrantRevokeExecute(
                    cmb_roles.SelectedItem?.ToString(),
                    cmb_user.SelectedItem?.ToString(),
                    cmb_package.SelectedItem?.ToString(),
                    "Package",
                    cb_roles_pk.Checked),
                () => LoadGrantRoles(cmb_roles.SelectedItem?.ToString()));
        }



        private void cb_select_us_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_select_us, () =>
                GrantRevokeTable(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Select", cb_select_us.Checked),
                UpdateUserCheckboxColors);
        }

        private void cb_insert_us_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_insert_us, () =>
                GrantRevokeTable(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Insert", cb_insert_us.Checked),
                UpdateUserCheckboxColors);
        }

        private void cb_update_us_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_update_us, () =>
                GrantRevokeTable(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Update", cb_update_us.Checked),
                UpdateUserCheckboxColors);
        }

        private void cb_delete_us_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_delete_us, () =>
                GrantRevokeTable(cmb_username.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Delete", cb_delete_us.Checked),
                UpdateUserCheckboxColors);
        }

        private void cb_select_ro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_select_ro, () =>
                GrantRevokeTable(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Select", cb_select_ro.Checked),
                UpdateRoleCheckboxColors);
        }


        private void cb_insert_ro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_insert_ro, () =>
                GrantRevokeTable(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Insert", cb_insert_ro.Checked),
                UpdateRoleCheckboxColors);
        }

        private void cb_update_ro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_update_ro, () =>
                GrantRevokeTable(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Update", cb_update_ro.Checked),
                UpdateRoleCheckboxColors);
        }

        private void cb_delete_ro_Click(object sender, EventArgs e)
        {
            HandleCheckboxClick(cb_delete_ro, () =>
                GrantRevokeTable(cmb_roles.SelectedItem?.ToString(), cmb_user.SelectedItem?.ToString(), cmb_table.SelectedItem?.ToString(), "Delete", cb_delete_ro.Checked),
                UpdateRoleCheckboxColors);
        }

        private bool isProgrammaticChange = false;

        private void HandleCheckboxClick(CheckBox cb, Func<bool> grantAction, Action onSuccess)
        {
            if (isProgrammaticChange) return;

            bool original = cb.Checked;
            bool result = grantAction();

            if (!result)
            {
                isProgrammaticChange = true;
                cb.Checked = !original;
                isProgrammaticChange = false;
            }
            else
            {
                onSuccess();
            }
        }


        private void btn_Grant_Revoke_Role_Click(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();

            bool grant = btn_Grant_Revoke_Role.Text == "Grant Role";

            if (GrantRevokeUserRole(user, role, grant))
            {
                LoadRolesUser(user);
                UpdateGrantRevokeRoleButtonText(user, role);
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
                if (_authService.GrantLogin(user))
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
                if (_authService.RevokeLogin(user))
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
                bool success = _authService.GrantAllTables(username);

                if (success)
                    MessageBox.Show("Đã cấp quyền thành công!");
                else
                    MessageBox.Show("Cấp quyền thất bại!");
            }
        }


    }
}
