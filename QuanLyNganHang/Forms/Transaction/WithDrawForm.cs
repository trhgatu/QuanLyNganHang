using QuanLyNganHang.Core;
using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Transaction
{
    public partial class WithDrawForm : Form
    {
        private TransactionDataAccess transactionDA;

        public WithDrawForm()
        {
            InitializeComponent();
            transactionDA = new TransactionDataAccess();
            SetupForm();
        }

        private void SetupForm()
        {
            // Kiểm tra session trước khi sử dụng
            if (SessionContext.EmployeeId <= 0)
            {
                MessageBox.Show("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!",
                    "Lỗi phiên làm việc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // Set default values
            txtAmount.Text = "0";
            cmbChannel.SelectedIndex = 0;

            // Hiển thị thông tin nhân viên thực hiện
            this.Text = $"Rút tiền - {SessionContext.FullName} ({SessionContext.Position})";

            // Set tab order
            txtAccountNumber.TabIndex = 0;
            btnSearchAccount.TabIndex = 1;
            txtAmount.TabIndex = 2;
            cmbChannel.TabIndex = 3;
            txtDescription.TabIndex = 4;
            btnWithdraw.TabIndex = 5;
            btnCancel.TabIndex = 6;
        }

        private void TxtAccountNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchAccount_Click(sender, e);
            }
        }

        private void BtnSearchAccount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập số tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountNumber.Focus();
                return;
            }

            try
            {
                DataTable dt = transactionDA.GetAccountInfo(txtAccountNumber.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblCustomerName.Text = "Tên khách hàng: " + row["customer_name"].ToString();
                    lblAccountType.Text = "Loại tài khoản: " + row["account_type"].ToString();
                    lblCurrentBalance.Text = "Số dư hiện tại: " + Convert.ToDecimal(row["balance"]).ToString("N0") + " VND";
                    lblMinBalance.Text = "Số dư tối thiểu: " + Convert.ToDecimal(row["min_balance"]).ToString("N0") + " VND";
                    lblAccountStatus.Text = "Trạng thái: " + row["status_text"].ToString();

                    if (row["status_text"].ToString() != "Hoạt động")
                    {
                        MessageBox.Show("Tài khoản không hoạt động!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnWithdraw.Enabled = false;
                    }
                    else
                    {
                        btnWithdraw.Enabled = true;
                        txtAmount.Focus();
                    }

                    grpAccountInfo.Visible = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAccountInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm tài khoản: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnWithdraw_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            decimal amount = decimal.Parse(txtAmount.Text.Replace(",", ""));
            decimal fee = 5000; // Phí rút tiền
            decimal total = amount + fee;

            // Hiển thị thông tin xác nhận với thông tin nhân viên
            string confirmMessage = $"Xác nhận rút {amount:N0} VND (phí: {fee:N0} VND, tổng: {total:N0} VND) từ tài khoản {txtAccountNumber.Text}?\n\n" +
                                  $"Thực hiện bởi: {SessionContext.FullName}\n" +
                                  $"Chi nhánh: {SessionContext.BranchName}\n" +
                                  $"Chức vụ: {SessionContext.Position}";

            if (MessageBox.Show(confirmMessage, "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                // Lấy account_id từ số tài khoản
                DataTable accountInfo = transactionDA.GetAccountInfo(txtAccountNumber.Text.Trim());
                int accountId = Convert.ToInt32(accountInfo.Rows[0]["account_id"]);

                bool success = transactionDA.WithdrawMoney(
                    accountId,
                    amount,
                    txtDescription.Text,
                    SessionContext.EmployeeId, // Sử dụng EmployeeId từ Session
                    cmbChannel.SelectedItem.ToString()
                );

                if (success)
                {
                    string successMessage = $"Rút tiền thành công!\n\n" +
                                          $"Số tiền: {amount:N0} VND\n" +
                                          $"Phí: {fee:N0} VND\n" +
                                          $"Tổng: {total:N0} VND\n" +
                                          $"Thực hiện bởi: {SessionContext.FullName}\n" +
                                          $"Chi nhánh: {SessionContext.BranchName}";

                    MessageBox.Show(successMessage, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Rút tiền thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực hiện giao dịch: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra session trước khi thực hiện
            if (SessionContext.EmployeeId <= 0)
            {
                MessageBox.Show("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!",
                    "Lỗi phiên làm việc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập số tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountNumber.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text.Replace(",", ""), out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            if (amount < 50000)
            {
                MessageBox.Show("Số tiền rút tối thiểu là 50,000 VND!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            return true;
        }

        private void ClearAccountInfo()
        {
            lblCustomerName.Text = "Tên khách hàng: ";
            lblAccountType.Text = "Loại tài khoản: ";
            lblCurrentBalance.Text = "Số dư hiện tại: ";
            lblMinBalance.Text = "Số dư tối thiểu: ";
            lblAccountStatus.Text = "Trạng thái: ";
            grpAccountInfo.Visible = false;
            btnWithdraw.Enabled = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }
    }
}
