using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormFreezeAccount : Form
    {
        private TextBox txtSearch;
        private Button btnSearch, btnFreeze, btnClose;
        private DataGridView dgvAccounts;
        private string selectedAccountId = null;

        private readonly AccountDataAccess accountDataAccess = new AccountDataAccess();
        private readonly CustomerDataAccess customerDataAccess = new CustomerDataAccess();

        public FormFreezeAccount()
        {
            Text = "❄️ Đóng băng tài khoản khách hàng";
            Size = new Size(900, 550);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            InitUI();
        }

        private void InitUI()
        {
            Font font = new Font("Segoe UI", 10F);

            Label lbl = new Label
            {
                Text = "🔍 Nhập CMND/CCCD hoặc SĐT khách hàng:",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = font
            };
            Controls.Add(lbl);

            txtSearch = new TextBox
            {
                Location = new Point(30, 60),
                Width = 500,
                Font = font
            };
            Controls.Add(txtSearch);

            btnSearch = new Button
            {
                Text = "🔎 Tìm kiếm",
                Location = new Point(540, 58),
                Width = 110,
                Height = 32,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            dgvAccounts = new DataGridView
            {
                Location = new Point(30, 110),
                Width = 820,
                Height = 300,
                Font = font,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White
            };
            dgvAccounts.CellClick += DgvAccounts_CellClick;
            Controls.Add(dgvAccounts);

            btnFreeze = new Button
            {
                Text = "❄️ Đóng băng",
                Location = new Point(230, 430),
                Width = 180,
                Height = 42,
                Enabled = false,
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnFreeze.FlatAppearance.BorderSize = 0;
            btnFreeze.Click += BtnFreeze_Click;
            Controls.Add(btnFreeze);

            btnClose = new Button
            {
                Text = "❌ Đóng",
                Location = new Point(450, 430),
                Width = 180,
                Height = 42,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập CMND/CCCD hoặc SĐT.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy danh sách khách hàng (dạng mã hóa gốc)
                DataTable customers = customerDataAccess.GetRawCustomers();
                string matchedCustomerId = null;

                foreach (DataRow row in customers.Rows)
                {
                    string decryptedId = EncryptionHelper.TryDecryptHybrid(row["id_number"]?.ToString());
                    string decryptedPhone = EncryptionHelper.TryDecryptHybrid(row["phone"]?.ToString());

                    if (decryptedId == keyword || decryptedPhone == keyword)
                    {
                        matchedCustomerId = row["customer_id"].ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(matchedCustomerId))
                {
                    MessageBox.Show("Không tìm thấy khách hàng phù hợp.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvAccounts.DataSource = null;
                    btnFreeze.Enabled = false;
                    selectedAccountId = null;
                    return;
                }

                // ⚠️ Convert từ string sang int để truyền vào đúng kiểu hàm
                DataTable result = accountDataAccess.GetActiveAccountsByCustomerId(Convert.ToInt32(matchedCustomerId));

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy tài khoản hoạt động.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvAccounts.DataSource = null;
                    btnFreeze.Enabled = false;
                    selectedAccountId = null;
                    return;
                }

                foreach (DataRow row in result.Rows)
                {
                    row["phone"] = EncryptionHelper.TryDecryptHybrid(row["phone"]?.ToString());
                    row["email"] = EncryptionHelper.TryDecryptHybrid(row["email"]?.ToString());
                    row["address"] = EncryptionHelper.TryDecryptHybrid(row["address"]?.ToString());
                    row["id_number"] = EncryptionHelper.TryDecryptHybrid(row["id_number"]?.ToString());
                }

                dgvAccounts.DataSource = result;
                dgvAccounts.Columns["account_id"].Visible = false;
                selectedAccountId = null;
                btnFreeze.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAccounts.Rows.Count > 0)
            {
                selectedAccountId = dgvAccounts.Rows[e.RowIndex].Cells["account_id"].Value?.ToString();
                btnFreeze.Enabled = !string.IsNullOrEmpty(selectedAccountId);
            }
        }

        private void BtnFreeze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedAccountId))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần đóng băng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn đóng băng tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                bool success = accountDataAccess.FreezeAccountById(Convert.ToInt32(selectedAccountId));

                if (success)
                {
                    MessageBox.Show("❄️ Đã đóng băng tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnSearch_Click(null, null); // refresh lại danh sách
                }
                else
                {
                    MessageBox.Show("Không thể đóng băng tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đóng băng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
