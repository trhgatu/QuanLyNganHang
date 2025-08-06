using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormOpenAccount : Form
    {
        private ComboBox cbCustomer, cbAccountType;
        private TextBox txtBalance, txtSearch;
        private Button btnSave, btnCancel, btnSearch;

        private readonly AccountDataAccess accountDataAccess = new AccountDataAccess();
        private readonly CustomerDataAccess customerDataAccess = new CustomerDataAccess();

        public FormOpenAccount()
        {
            this.Text = "📝 Mở tài khoản mới";
            this.Size = new Size(520, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            InitUI();
        }

        public FormOpenAccount(string customerId, string customerName) : this()
        {
            cbCustomer.Items.Add(new ComboBoxItem(customerName + " (" + customerId + ")", customerId));
            cbCustomer.SelectedIndex = 0;
            cbCustomer.Enabled = false;
        }

        private void InitUI()
        {
            Font font = new Font("Segoe UI", 10F);

            var groupBox = new GroupBox
            {
                Text = "📋 Thông tin mở tài khoản",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Color.Teal,
                BackColor = Color.White,
                Size = new Size(460, 260),
                Location = new Point(25, 20)
            };
            Controls.Add(groupBox);

            var lblSearch = new Label { Text = "🔍 CMND/CCCD/SDT:", Left = 20, Top = 35, AutoSize = true, Font = font };
            txtSearch = new TextBox { Left = 160, Top = 30, Width = 180, Font = font };
            btnSearch = new Button
            {
                Text = "Tìm",
                Left = 350,
                Top = 29,
                Width = 60,
                Height = 28,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;

            var lblCustomer = new Label { Text = "👤 Khách hàng:", Left = 20, Top = 75, AutoSize = true, Font = font };
            cbCustomer = new ComboBox
            {
                Left = 160,
                Top = 70,
                Width = 250,
                Font = font,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            var lblType = new Label { Text = "🏦 Loại tài khoản:", Left = 20, Top = 120, AutoSize = true, Font = font };
            cbAccountType = new ComboBox
            {
                Left = 160,
                Top = 115,
                Width = 250,
                Font = font,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbAccountType.Items.Add(new ComboBoxItem("💳 Thanh toán", "1"));
            cbAccountType.Items.Add(new ComboBoxItem("💰 Tiết kiệm", "2"));

            var lblBalance = new Label { Text = "💵 Số dư ban đầu:", Left = 20, Top = 165, AutoSize = true, Font = font };
            txtBalance = new TextBox { Left = 160, Top = 160, Width = 250, Font = font };

            groupBox.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, btnSearch,
                lblCustomer, cbCustomer,
                lblType, cbAccountType,
                lblBalance, txtBalance
            });

            btnSave = new Button
            {
                Text = "💾 Lưu",
                Width = 120,
                Height = 40,
                Left = 80,
                Top = groupBox.Bottom + 20,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += SaveAccount;

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Width = 120,
                Height = 40,
                Left = 260,
                Top = groupBox.Bottom + 20,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += delegate { this.Close(); };

            Controls.AddRange(new Control[] { btnSave, btnCancel });
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string input = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("🔔 Vui lòng nhập CMND/CCCD hoặc số điện thoại!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                cbCustomer.Items.Clear();
                cbCustomer.Enabled = false;

                DataTable allCustomers = customerDataAccess.GetRawCustomers();
                DataRow matchedRow = null;

                foreach (DataRow row in allCustomers.Rows)
                {
                    string decryptedId = EncryptionHelper.TryDecryptHybrid(row["id_number"].ToString());
                    string decryptedPhone = EncryptionHelper.TryDecryptHybrid(row["phone"].ToString());

                    if (string.Equals(decryptedId, input, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(decryptedPhone, input, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedRow = row;
                        break;
                    }
                }

                if (matchedRow == null)
                {
                    MessageBox.Show("❗ Không tìm thấy khách hàng.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string customerId = matchedRow["customer_id"].ToString();
                string customerName = matchedRow["full_name"].ToString();

                if (accountDataAccess.CustomerHasActiveAccount(customerId))
                {
                    MessageBox.Show($"⚠️ Khách hàng \"{customerName}\" đã có tài khoản đang hoạt động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cbCustomer.Items.Add(new ComboBoxItem($"{customerName} ({customerId})", customerId));
                    cbCustomer.SelectedIndex = 0;
                    cbCustomer.Enabled = true;
                    MessageBox.Show($"✅ Khách hàng \"{customerName}\" đủ điều kiện mở tài khoản.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tìm kiếm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAccount(object sender, EventArgs e)
        {
            ComboBoxItem cust = cbCustomer.SelectedItem as ComboBoxItem;
            ComboBoxItem accType = cbAccountType.SelectedItem as ComboBoxItem;

            if (cust == null || accType == null || string.IsNullOrWhiteSpace(txtBalance.Text))
            {
                MessageBox.Show("🔔 Vui lòng nhập đầy đủ thông tin!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtBalance.Text, out decimal balance) || balance < 0)
            {
                MessageBox.Show("❗ Số dư ban đầu không hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!accountDataAccess.CreateAccount(cust.Value, int.Parse(accType.Value), balance))
            {
                MessageBox.Show("❌ Không thể tạo tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("🎉 Tài khoản đã được tạo thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class ComboBoxItem
    {
        public string Text { get; private set; }
        public string Value { get; private set; }

        public ComboBoxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
