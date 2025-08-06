using System;
using System.Data;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.OLS
{
    public partial class UserControl_TaoPolicyGanQuyen : UserControl
    {
        private OLSManager _olsManager = new OLSManager();

        public UserControl_TaoPolicyGanQuyen()
        {
            InitializeComponent();
            LoadPolicies();
            LoadUsers();
            SetupInitialState();
        }

        private void SetupInitialState()
        {
            // Initially disable buttons
            btnThem.Enabled = false;
            btnGan.Enabled = false;
        }

        private void LoadPolicies()
        {
            try
            {
                var dt = _olsManager.Get_OLSPolicies();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cboName.DataSource = dt;
                    cboName.DisplayMember = "POLICY_NAME";
                    cboName.ValueMember = "POLICY_NAME";
                    cboName.SelectedIndex = -1; // No selection initially
                }
                else
                {
                    MessageBox.Show("Không có policy nào được tìm thấy!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách policy: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                var dt = _olsManager.GetUsers();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cbo_User.DataSource = dt;
                    cbo_User.DisplayMember = "USERNAME";
                    cbo_User.ValueMember = "USERNAME";
                    cbo_User.SelectedIndex = -1; // No selection initially
                }
                else
                {
                    MessageBox.Show("Không có user nào được tìm thấy!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách user: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string policy = txtName1.Text.Trim();
                string column = txtNameColumn.Text.Trim();

                if (string.IsNullOrEmpty(policy) || string.IsNullOrEmpty(column))
                {
                    MessageBox.Show("Vui lòng nhập đủ tên policy và cột OLS!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validation for policy name format
                if (!IsValidPolicyName(policy))
                {
                    MessageBox.Show("Tên policy chỉ được chứa chữ cái, số và dấu gạch dưới!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmation
                var result = MessageBox.Show($"Bạn có chắc chắn muốn tạo policy '{policy}' với cột '{column}'?",
                                           "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Disable button during operation
                    btnThem.Enabled = false;
                    btnThem.Text = "Creating...";

                    if (_olsManager.Create_OLSPolicy(policy, column))
                    {
                        MessageBox.Show("Tạo policy thành công!", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear form and reload policies
                        txtName1.Clear();
                        txtNameColumn.Clear();
                        LoadPolicies();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi tạo policy! Policy có thể đã tồn tại.", "Lỗi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo policy: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnThem.Text = "Create Policy";
                ValidateCreateForm(null, null); // Re-validate form
            }
        }

        private void btnGan_Click(object sender, EventArgs e)
        {
            try
            {
                string policy = cboName.SelectedValue?.ToString();
                string user = cbo_User.SelectedValue?.ToString();

                if (string.IsNullOrEmpty(policy) || string.IsNullOrEmpty(user))
                {
                    MessageBox.Show("Vui lòng chọn đủ Policy và User để gán quyền quản lý!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmation
                var result = MessageBox.Show($"Bạn có chắc chắn muốn gán quyền quản lý policy '{policy}' cho user '{user}'?",
                                           "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Disable button during operation
                    btnGan.Enabled = false;
                    btnGan.Text = "Assigning...";

                    if (_olsManager.GrantPolicyManager(policy, user))
                    {
                        MessageBox.Show($"Gán quyền quản lý policy '{policy}' cho user '{user}' thành công!", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset selections
                        cboName.SelectedIndex = -1;
                        cbo_User.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi gán quyền! User có thể đã có quyền này.", "Lỗi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gán quyền: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGan.Text = "Assign Rights";
                ValidateAssignForm(null, null); // Re-validate form
            }
        }

        private bool IsValidPolicyName(string policyName)
        {
            foreach (char c in policyName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }
            return true;
        }

        public void RefreshData()
        {
            LoadPolicies();
            LoadUsers();
        }
    }
}
