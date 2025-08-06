using System;
using System.Data;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;

namespace QuanLyNganHang.Forms.OLS
{
    public partial class UserControl_EnableDisablePolicy : UserControl
    {
        private OLSManager _olsManager = new OLSManager();

        public UserControl_EnableDisablePolicy()
        {
            InitializeComponent();
            LoadPolicies();
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

                    // Thêm item đầu tiên
                    cboName.SelectedIndex = -1;
                    UpdateStatusDisplay("Chọn policy để xem trạng thái");
                }
                else
                {
                    UpdateStatusDisplay("Không có policy nào");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách policy: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusDisplay("Lỗi tải dữ liệu");
            }
        }

        private void UpdateStatus()
        {
            try
            {
                string policy = cboName.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(policy))
                {
                    string status = _olsManager.GetPolicyStatus(policy);
                    UpdateStatusDisplay(status);

                    // Enable/disable buttons based on current status
                    EnableActionButtons(true);
                }
                else
                {
                    UpdateStatusDisplay("Chọn policy để xem trạng thái");
                    EnableActionButtons(false);
                }
            }
            catch (Exception ex)
            {
                UpdateStatusDisplay($"Lỗi: {ex.Message}");
                EnableActionButtons(false);
            }
        }

        private void EnableActionButtons(bool enabled)
        {
            rdbEnable.Enabled = enabled;
            rdbDisable.Enabled = enabled;
            btnCommit.Enabled = enabled && (rdbEnable.Checked || rdbDisable.Checked);
        }

        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
            // Reset radio buttons when selection changes
            rdbEnable.Checked = false;
            rdbDisable.Checked = false;
            btnCommit.Enabled = false;
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            try
            {
                string policy = cboName.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(policy))
                {
                    MessageBox.Show("Vui lòng chọn policy!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!rdbEnable.Checked && !rdbDisable.Checked)
                {
                    MessageBox.Show("Vui lòng chọn hành động (Enable/Disable)!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string action = rdbEnable.Checked ? "Enable" : "Disable";
                var result = MessageBox.Show($"Bạn có chắc chắn muốn {action} policy '{policy}'?",
                                           "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Disable button during operation
                    btnCommit.Enabled = false;
                    btnCommit.Text = "Processing...";

                    if (rdbEnable.Checked)
                    {
                        _olsManager.Enable_OLSPolicy(policy);
                    }
                    else
                    {
                        _olsManager.Disable_OLSPolicy(policy);
                    }

                    // Update status and UI
                    UpdateStatus();

                    // Reset radio buttons
                    rdbEnable.Checked = false;
                    rdbDisable.Checked = false;

                    btnCommit.Text = "Apply Changes";

                    MessageBox.Show($"Policy '{policy}' đã được {action.ToLower()} thành công!",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện thao tác: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCommit.Text = "Apply Changes";
                btnCommit.Enabled = rdbEnable.Checked || rdbDisable.Checked;
            }
        }

        // Event handler for radio button changes
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            btnCommit.Enabled = (rdbEnable.Checked || rdbDisable.Checked) &&
                               cboName.SelectedIndex >= 0;
        }

        // Override SetupEventHandlers để thêm event cho radio buttons
        private void SetupAdditionalEventHandlers()
        {
            rdbEnable.CheckedChanged += RadioButton_CheckedChanged;
            rdbDisable.CheckedChanged += RadioButton_CheckedChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupAdditionalEventHandlers();
        }
    }
}
