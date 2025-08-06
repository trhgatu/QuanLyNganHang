using System;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.OLS
{
    public partial class UserControl_LabelComponent : UserControl
    {
        private OLSManager _olsManager = new OLSManager();

        public UserControl_LabelComponent()
        {
            InitializeComponent();
            LoadPolicies();
            LoadThanhPhan();
            SetupInitialState();
        }

        private void LoadPolicies()
        {
            try
            {
                var dt = _olsManager.Get_OLSPolicies();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cboPolicy.DataSource = dt;
                    cboPolicy.DisplayMember = "POLICY_NAME";
                    cboPolicy.ValueMember = "POLICY_NAME";
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

        private void LoadThanhPhan()
        {
            cboThanhPhan.Items.Clear();
            cboThanhPhan.Items.Add("Level");
            cboThanhPhan.Items.Add("Compartment");
            cboThanhPhan.Items.Add("Group");
            cboThanhPhan.SelectedIndex = 0;
        }

        private void SetupInitialState()
        {
            // Initially hide parent field (only show for Group)
            UpdateFieldVisibility("Level");

            // Disable create button initially
            btnCreate.Enabled = false;

            // Add validation events
            txtShortName.TextChanged += ValidateForm;
            cboPolicy.SelectedIndexChanged += ValidateForm;
        }

        private void ValidateForm(object sender, EventArgs e)
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtShortName.Text) &&
                          cboPolicy.SelectedIndex >= 0;

            btnCreate.Enabled = isValid;
        }

        private void cboThanhPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = cboThanhPhan.SelectedItem?.ToString();
            UpdateFieldVisibility(selectedType);

            // Clear fields when changing component type
            ClearFields();
        }

        private void ClearFields()
        {
            txtNumber.Clear();
            txtShortName.Clear();
            txtLongName.Clear();
            cbo_groupParent.SelectedIndex = -1;
            btnCreate.Enabled = false;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string policy = cboPolicy.SelectedValue?.ToString();
                string tp = cboThanhPhan.SelectedItem?.ToString();
                string shortName = txtShortName.Text.Trim();
                string longName = txtLongName.Text.Trim();
                string parent = cbo_groupParent.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(policy))
                {
                    MessageBox.Show("Vui lòng chọn policy!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(shortName))
                {
                    MessageBox.Show("Vui lòng nhập short name!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtNumber.Text, out int number))
                {
                    MessageBox.Show("Vui lòng nhập number hợp lệ!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmation
                var result = MessageBox.Show($"Bạn có chắc chắn muốn tạo {tp} '{shortName}' trong policy '{policy}'?",
                                           "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Disable button during operation
                    btnCreate.Enabled = false;
                    btnCreate.Text = "Creating...";

                    bool success = false;
                    string message = "";

                    switch (tp)
                    {
                        case "Level":
                            success = _olsManager.Create_Level(policy, number, shortName, longName);
                            message = success ? "Thêm level thành công!" : "Lỗi khi tạo level!";
                            break;

                        case "Compartment":
                            success = _olsManager.Create_Compartment(policy, number, shortName, longName);
                            message = success ? "Thêm compartment thành công!" : "Lỗi khi tạo compartment!";
                            break;

                        case "Group":
                            success = _olsManager.Create_Group(policy, number, shortName, longName, parent);
                            message = success ? "Thêm group thành công!" : "Lỗi khi tạo group!";
                            break;
                    }

                    MessageBox.Show(message, success ? "Thành công" : "Lỗi",
                                  MessageBoxButtons.OK,
                                  success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (success)
                    {
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo component: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreate.Text = "Tạo mới";
                ValidateForm(null, null); // Re-validate form
            }
        }
    }
}
