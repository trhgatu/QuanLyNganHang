using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang
{
    public class FormCloseAccount : Form
    {
        private DataGridView dgvAccounts;
        private TextBox txtSearch;
        private Button btnCloseAccount;
        private DataTable accountTable;

        private readonly AccountDataAccess accountDataAccess = new AccountDataAccess();

        public FormCloseAccount()
        {
            InitializeComponent();
            LoadAccountData();
        }

        private void InitializeComponent()
        {
            this.Text = "🗑️ Đóng tài khoản";
            this.Size = new Size(880, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            var lblSearch = new Label
            {
                Text = "🔍 Tìm kiếm:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Location = new Point(110, 15),
                Width = 350
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnCloseAccount = new Button
            {
                Text = "🗑️ Đóng tài khoản",
                Location = new Point(480, 13),
                Size = new Size(160, 32),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCloseAccount.FlatAppearance.BorderSize = 0;
            btnCloseAccount.Click += BtnCloseAccount_Click;

            dgvAccounts = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(820, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false // ẩn khi mở form
            };

            dgvAccounts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            dgvAccounts.EnableHeadersVisualStyles = false;

            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnCloseAccount);
            this.Controls.Add(dgvAccounts);
        }

        private void LoadAccountData()
        {
            try
            {
                accountTable = accountDataAccess.GetActiveAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (accountTable == null) return;

            string filter = $"account_number LIKE '%{txtSearch.Text}%' " +
                            $"OR customer_name LIKE '%{txtSearch.Text}%' " +
                            $"OR phone LIKE '%{txtSearch.Text}%' " +
                            $"OR email LIKE '%{txtSearch.Text}%'";

            try
            {
                DataView view = new DataView(accountTable);
                view.RowFilter = filter;

                dgvAccounts.DataSource = view;
                dgvAccounts.Visible = view.Count > 0;
            }
            catch
            {
                // tránh lỗi nếu nhập ký tự đặc biệt
                dgvAccounts.Visible = false;
            }
        }

        private void BtnCloseAccount_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn một tài khoản để đóng.");
                return;
            }

            var row = dgvAccounts.SelectedRows[0];
            int accountId = Convert.ToInt32(row.Cells["account_id"].Value);
            string accNumber = row.Cells["account_number"].Value.ToString();

            var confirm = MessageBox.Show($"Bạn có chắc muốn đóng tài khoản {accNumber}?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (accountDataAccess.CloseAccount(accountId))
                {
                    MessageBox.Show("✅ Tài khoản đã được đóng thành công.");
                    LoadAccountData();
                    txtSearch.Clear();
                    dgvAccounts.Visible = false;
                }
                else
                {
                    MessageBox.Show("❌ Không thể đóng tài khoản.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
