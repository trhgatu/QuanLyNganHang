using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormSearchCustomerOpenAccount : Form
    {
        private TextBox txtSearch;
        private Button btnSearch, btnClose;
        private Label lblResult;

        private readonly CustomerDataAccess customerDataAccess = new CustomerDataAccess();

        public FormSearchCustomerOpenAccount()
        {
            this.Text = "🔍 Tìm kiếm khách hàng để mở tài khoản";
            this.Size = new Size(580, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            InitUI();
        }

        private void InitUI()
        {
            Font font = new Font("Segoe UI", 10F);

            Label lblTitle = new Label()
            {
                Text = "🧾 Nhập CMND/CCCD hoặc Số điện thoại:",
                Left = 30,
                Top = 30,
                Width = 500,
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Color.Teal
            };
            Controls.Add(lblTitle);

            txtSearch = new TextBox()
            {
                Left = 30,
                Top = 60,
                Width = 390,
                Font = font
            };
            Controls.Add(txtSearch);

            btnSearch = new Button()
            {
                Text = "🔎 Tìm kiếm",
                Left = 430,
                Top = 58,
                Width = 100,
                Height = 32,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            lblResult = new Label()
            {
                Left = 30,
                Top = 110,
                Width = 500,
                Height = 70,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray
            };
            Controls.Add(lblResult);

            btnClose = new Button()
            {
                Text = "❌ Đóng",
                Width = 120,
                Height = 40,
                Left = 220,
                Top = 200,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            Controls.Add(btnClose);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string input = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Vui lòng nhập CMND/CCCD hoặc số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string encryptedInput = EncryptionHelper.EncryptHybrid(input);
                var customerRow = customerDataAccess.FindCustomerByEncryptedInput(encryptedInput);

                if (customerRow != null)
                {
                    string customerId = customerRow["customer_id"].ToString();
                    string customerName = customerRow["full_name"].ToString();

                    if (customerDataAccess.HasActiveAccount(customerId))
                    {
                        lblResult.Text = $"⚠️ Khách hàng \"{customerName}\" đã có tài khoản đang hoạt động.";
                        lblResult.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblResult.Text = $"✅ Khách hàng \"{customerName}\" chưa có tài khoản. Tiến hành mở tài khoản.";
                        lblResult.ForeColor = Color.Green;

                        DialogResult res = MessageBox.Show("Bạn có muốn mở tài khoản cho khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            this.Hide();
                            var openForm = new FormOpenAccount(customerId, customerName);
                            openForm.ShowDialog();
                            this.Close();
                        }
                    }
                }
                else
                {
                    var ask = MessageBox.Show("Khách hàng không tồn tại. Bạn có muốn thêm khách hàng mới?", "Không tìm thấy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ask == DialogResult.Yes)
                    {
                        this.Hide();
                        var addForm = new FormAddEditCustomer(false);
                        addForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        lblResult.Text = "Không tìm thấy khách hàng.";
                        lblResult.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
