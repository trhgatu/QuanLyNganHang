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
        AuthorizationManager authorization;
        public AuthorizationForm()
        {

            InitializeComponent();
            CenterToScreen();
            try
            {
                CenterToScreen();
                authorization = new AuthorizationManager();
                Load_User();
                Load_Roles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi form load: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        void set_Color_Checkbox_user()
        {
            if (cb_select_us.Checked)
                cb_select_us.ForeColor = Color.Green;
            else
                cb_select_us.ForeColor = Color.Red;
            if (cb_insert_us.Checked)
                cb_insert_us.ForeColor = Color.Green;
            else
                cb_insert_us.ForeColor = Color.Red;
            if (cb_update_us.Checked)
                cb_update_us.ForeColor = Color.Green;
            else
                cb_update_us.ForeColor = Color.Red;
            if (cb_delete_us.Checked)
                cb_delete_us.ForeColor = Color.Green;
            else
                cb_delete_us.ForeColor = Color.Red;
        }

        void set_Color_Checkbox_role()
        {
            if (cb_select_ro.Checked)
                cb_select_ro.ForeColor = Color.Green;
            else
                cb_select_ro.ForeColor = Color.Red;
            if (cb_insert_ro.Checked)
                cb_insert_ro.ForeColor = Color.Green;
            else
                cb_insert_ro.ForeColor = Color.Red;
            if (cb_update_ro.Checked)
                cb_update_ro.ForeColor = Color.Green;
            else
                cb_update_ro.ForeColor = Color.Red;
            if (cb_delete_ro.Checked)
                cb_delete_ro.ForeColor = Color.Green;
            else
                cb_delete_ro.ForeColor = Color.Red;
        }

        void set_color_grant_user()
        {
            if (cb_user_pro.Checked)
                cb_user_pro.ForeColor = Color.Green;
            else
                cb_user_pro.ForeColor = Color.Red;
            if (cb_user_fun.Checked)
                cb_user_fun.ForeColor = Color.Green;
            else
                cb_user_fun.ForeColor = Color.Red;

            if (cb_user_pk.Checked)
                cb_user_pk.ForeColor = Color.Green;
            else
                cb_user_pk.ForeColor = Color.Red;
        }
        void set_color_grant_role()
        {
            if (cb_roles_pro.Checked)
                cb_roles_pro.ForeColor = Color.Green;
            else
                cb_roles_pro.ForeColor = Color.Red;
            if (cb_roles_fun.Checked)
                cb_roles_fun.ForeColor = Color.Green;
            else
                cb_roles_fun.ForeColor = Color.Red;
            if (cb_roles_pk.Checked)
                cb_roles_pk.ForeColor = Color.Green;
            else
                cb_roles_pk.ForeColor = Color.Red;
        }

        void set_label_table()
        {
            string t = "Table: ";
            if (cmb_table.SelectedItem != null)
            {
                t += cmb_table.SelectedItem.ToString();
            }
            lbl_table_roles.Text = t;
            lbl_table_user.Text = t;
        }

        void set_text_button(string user, string role)
        {
            int kq = authorization.Get_Roles_User_Check(user, role);
            if (kq == 1)
            {
                btn_Grant_Revoke_Role.Text = "Revoke Role";

            }
            else if (kq == 0)
            {
                btn_Grant_Revoke_Role.Text = "Grant Role";
            }

        }

        void Load_User()
        {
            DataTable table = authorization.Get_User();

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
        }


        void Load_Roles()
        {
            DataTable table = authorization.Get_Roles();

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
        }


        void Clear_ComboBox()
        {
            cmb_procedure.Items.Clear();
            cmb_function.Items.Clear();
            cmb_package.Items.Clear();
            cmb_table.Items.Clear();
        }

        void Select_Combobox()
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

        void Load_pro_user(string userowner)
        {
            Clear_ComboBox();

            DataTable proTable = authorization.Get_Procedure_User(userowner, "PROCEDURE");
            foreach (DataRow row in proTable.Rows)
            {
                cmb_procedure.Items.Add(row[0].ToString());
            }

            DataTable funTable = authorization.Get_Procedure_User(userowner, "FUNCTION");
            foreach (DataRow row in funTable.Rows)
            {
                cmb_function.Items.Add(row[0].ToString());
            }

            DataTable packTable = authorization.Get_Procedure_User(userowner, "PACKAGE");
            foreach (DataRow row in packTable.Rows)
            {
                cmb_package.Items.Add(row[0].ToString());
            }

            DataTable tableTable = authorization.Get_Table_User(userowner);
            foreach (DataRow row in tableTable.Rows)
            {
                cmb_table.Items.Add(row[0].ToString());
            }

            Select_Combobox();
        }


        void Load_Roles_User(string user)
        {
            dtg_roles.DataSource = authorization.Get_Roles_User(user);
        }

        void Load_Grant_User(string user)
        {
            dtg_grant.DataSource = authorization.Get_Grant_User(user);
        }

        void Load_Grant_Roles(string roles)
        {
            dgv_grant_roles.DataSource = authorization.Get_Grant_User(roles);
        }

        void Load_Grant_Table_User(string user, string userschema, string table)
        {
            DataTable grantTable = authorization.Get_Grant(user, userschema, table);
            if (grantTable == null) return;

            bool select = false, insert = false, update = false, delete = false;

            foreach (DataRow row in grantTable.Rows)
            {
                string privilege = row[0].ToString().ToUpper();
                if (privilege == "SELECT") select = true;
                else if (privilege == "INSERT") insert = true;
                else if (privilege == "UPDATE") update = true;
                else if (privilege == "DELETE") delete = true;
            }

            cb_select_us.Checked = select;
            cb_insert_us.Checked = insert;
            cb_update_us.Checked = update;
            cb_delete_us.Checked = delete;
        }


        void Load_Grant_Table_Roles(string roles, string userschema, string table)
        {
            DataTable grantTable = authorization.Get_Grant(roles, userschema, table);
            if (grantTable == null) return;

            bool select = false, insert = false, update = false, delete = false;

            foreach (DataRow row in grantTable.Rows)
            {
                string privilege = row[0].ToString().ToUpper();
                if (privilege == "SELECT") select = true;
                else if (privilege == "INSERT") insert = true;
                else if (privilege == "UPDATE") update = true;
                else if (privilege == "DELETE") delete = true;
            }

            cb_select_ro.Checked = select;
            cb_insert_ro.Checked = insert;
            cb_update_ro.Checked = update;
            cb_delete_ro.Checked = delete;
        }


        bool Load_Execute(string user_roles, string userschema, string pro)
        {
            DataTable grantTable = authorization.Get_Grant(user_roles, userschema, pro);
            if (grantTable == null) return false;

            foreach (DataRow row in grantTable.Rows)
            {
                string privilege = row[0].ToString().ToUpper();
                if (privilege == "EXECUTE") return true;
            }

            return false;
        }

        private void cmb_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userowner = cmb_user.SelectedItem.ToString();
            Load_pro_user(userowner);
        }

        private void cmb_username_SelectedIndexChanged(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem.ToString();
            string userschema = cmb_user.SelectedItem.ToString();
            Load_Roles_User(user);
            Load_Grant_User(user);
            lbl_user.Text = "User: " + user;

            if (cmb_table.SelectedItem != null)
            {
                string table = cmb_table.SelectedItem.ToString();
                Load_Grant_Table_User(user, userschema, table);
                set_Color_Checkbox_user();
            }
            if (cmb_procedure.SelectedItem != null)
            {
                string procedure = cmb_procedure.SelectedItem.ToString();
                cb_user_pro.Checked = Load_Execute(user, userschema, procedure);
            }
            if (cmb_function.SelectedItem != null)
            {
                string function = cmb_function.SelectedItem.ToString();
                cb_user_fun.Checked = Load_Execute(user, userschema, function);
            }
            if (cmb_package.SelectedItem != null)
            {
                string package = cmb_package.SelectedItem.ToString();
                cb_user_pk.Checked = Load_Execute(user, userschema, package);
            }
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                set_text_button(user, role);
            }
            set_color_grant_user();
        }

        private void cmb_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userschema = cmb_user.SelectedItem.ToString();
            set_label_table();
            string table = cmb_table.SelectedItem.ToString();
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                Load_Grant_Table_User(user, userschema, table);
                set_Color_Checkbox_user();
            }
            if (cmb_roles.SelectedItem != null)
            {
                string roles = cmb_roles.SelectedItem.ToString();
                Load_Grant_Table_Roles(roles, userschema, table);
                set_Color_Checkbox_role();
            }
        }

        private void cmb_roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userschema = cmb_user.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();
            Load_Grant_Roles(role);

            if (cmb_table.SelectedItem != null)
            {
                string table = cmb_table.SelectedItem.ToString();
                Load_Grant_Table_Roles(role, userschema, table);
                set_Color_Checkbox_role();
            }
            if (cmb_procedure.SelectedItem != null)
            {
                string procedure = cmb_procedure.SelectedItem.ToString();
                cb_roles_pro.Checked = Load_Execute(role, userschema, procedure);
            }
            if (cmb_function.SelectedItem != null)
            {
                string function = cmb_function.SelectedItem.ToString();
                cb_roles_fun.Checked = Load_Execute(role, userschema, function);
            }
            if (cmb_package.SelectedItem != null)
            {
                string package = cmb_package.SelectedItem.ToString();
                cb_roles_pk.Checked = Load_Execute(role, userschema, package);
            }
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                set_text_button(user, role);
            }
            set_color_grant_role();
        }

        private void cmb_procedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string procedure = cmb_procedure.SelectedItem.ToString();
            string userschema = cmb_user.SelectedItem.ToString();
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_pro.Checked = Load_Execute(user, userschema, procedure);
            }
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_pro.Checked = Load_Execute(role, userschema, procedure);
            }

            set_color_grant_role();
            set_color_grant_user();
        }

        private void cmb_function_SelectedIndexChanged(object sender, EventArgs e)
        {
            string function = cmb_function.SelectedItem.ToString();
            string userschema = cmb_user.SelectedItem.ToString();
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_fun.Checked = Load_Execute(user, userschema, function);
            }
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_fun.Checked = Load_Execute(role, userschema, function);
            }
            set_color_grant_role();
            set_color_grant_user();
        }

        private void cmb_package_SelectedIndexChanged(object sender, EventArgs e)
        {
            string package = cmb_package.SelectedItem.ToString();
            string userschema = cmb_user.SelectedItem.ToString();
            if (cmb_username.SelectedItem != null)
            {
                string user = cmb_username.SelectedItem.ToString();
                cb_user_pk.Checked = Load_Execute(user, userschema, package);
            }
            if (cmb_roles.SelectedItem != null)
            {
                string role = cmb_roles.SelectedItem.ToString();
                cb_roles_pk.Checked = Load_Execute(role, userschema, package);
            }
            set_color_grant_user();
            set_color_grant_role();
        }
        bool Grant_Revoke_pro(string user_roles, string schema, string pro_tab, string type_pro_tab, string type_user, string type, bool grant_revoke)
        {
            if (pro_tab.Equals(""))
            {
                MessageBox.Show("Mục " + type_pro_tab + " Trống!!!");
                return false;
            }
            if (user_roles.Equals(""))
            {
                MessageBox.Show("Mục " + type_user + " Trống!!!");
                return false;
            }

            int dk;
            DialogResult res;
            if (grant_revoke)
            {
                dk = 1;
                res = MessageBox.Show(
                    "Bạn muốn gán quyền " + type + " " + type_pro_tab + ": " + pro_tab +
                    " cho " + type_user + ": " + user_roles + " không??",
                    "Thông báo", MessageBoxButtons.YesNo);
            }
            else
            {
                dk = 0;
                res = MessageBox.Show(
                    "Bạn muốn hủy quyền " + type + " " + type_pro_tab + ": " + pro_tab +
                    " cho " + type_user + ": " + user_roles + " không??",
                    "Thông báo", MessageBoxButtons.YesNo);
            }
            if (res == DialogResult.Yes)
            {
                if (authorization.Grant_Revoke_Pro(user_roles, schema, pro_tab, type, dk))
                {
                    if (grant_revoke)
                        MessageBox.Show("Gán quyền thành công");
                    else
                        MessageBox.Show("Thu hồi quyền thành công");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        bool Grant_Revoke_Role(string user, string role, int dk)
        {
            if (user.Equals(""))
            {
                MessageBox.Show("Mục User trống!!");
                return false;
            }

            if (role.Equals(""))
            {
                MessageBox.Show("Mục Role trống!!");
                return false;
            }

            DialogResult res;
            if (dk == 1)
            {
                res = MessageBox.Show(
                    "Bạn muốn gán User: " + user + " vào Role: " + role + " không?",
                    "Thông báo", MessageBoxButtons.YesNo);
            }
            else
            {
                res = MessageBox.Show(
                    "Bạn muốn gỡ User: " + user + " khỏi Role: " + role + " không?",
                    "Thông báo", MessageBoxButtons.YesNo);
            }

            if (res == DialogResult.Yes)
            {
                if (authorization.Grant_Revoke_Role(user, role, dk))
                {
                    if (dk == 1)
                        MessageBox.Show("Gán nhóm quyền thành công");
                    else
                        MessageBox.Show("Thu hồi nhóm quyền thành công");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        void Grant_Revoke_Pro_User(ComboBox cmb, CheckBox c, string pro)
        {
            bool check = c.Checked;
            string procedure = cmb.SelectedItem.ToString();
            string user = cmb_username.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            if (Grant_Revoke_pro(user, schema, procedure, "User", pro, "Execute", check))
            {
                set_color_grant_user();
                Load_Grant_User(user);
            }
            else
            {
                c.Checked = !check;
            }
        }
        void Grant_Revoke_Pro_Role(ComboBox cmb, CheckBox c, string pro)
        {
            bool check = c.Checked;
            string procedure = cmb.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();
            if (Grant_Revoke_pro(role, schema, procedure, "Role", pro, "Execute", check))
            {
                set_color_grant_role();
                Load_Grant_Roles(role);
            }
            else
            {
                c.Checked = !check;
            }
        }
        void Grant_Revoke_Table_User(CheckBox c, string type)
        {
            bool check = c.Checked;
            string table = cmb_table.SelectedItem.ToString();
            string user = cmb_username.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();
            if (Grant_Revoke_pro(user, schema, table, "User", "Table", type, check))
            {
                set_Color_Checkbox_user();
            }
            else
            {
                c.Checked = !check;
            }
        }
        void Grant_Revoke_Table_Role(CheckBox c, string type)
        {
            bool check = c.Checked;
            string table = cmb_table.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();
            string schema = cmb_user.SelectedItem.ToString();

            if (Grant_Revoke_pro(role, schema, table, "Role", "Table", type, check))
            {
                set_Color_Checkbox_role();
            }
            else
            {
                c.Checked = !check;
            }
        }

        private void cb_user_pro_CheckedChanged(object sender, EventArgs e)
        {

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

        private void cb_user_pk_Click(object sender, EventArgs e)
        {
            Grant_Revoke_Pro_User(cmb_package, cb_user_pk, "Package");
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

        private void btn_Grant_Revoke_Role_Click(object sender, EventArgs e)
        {
            string user = cmb_username.SelectedItem.ToString();
            string role = cmb_roles.SelectedItem.ToString();
            int dk;
            if (btn_Grant_Revoke_Role.Text.Equals("Grant"))
            {
                dk = 1;
            }
            else
            {
                dk = 0;
            }

            if (Grant_Revoke_Role(user, role, dk))
            {
                Load_Roles_User(user);
                set_text_button(user, role);
            }
        }
    }
}
