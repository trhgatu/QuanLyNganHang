using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormActivateAccount : Form
    {
        private DataGridView dgvFrozenAccounts;
        private TextBox txtSearch;
        private Button btnActivate;

        private readonly AccountDataAccess accountDataAccess = new AccountDataAccess();

        public FormActivateAccount()
        {
            InitializeComponent();
            LoadFrozenAccounts();
        }

        private void InitializeComponent()
        {
            this.Text = "🧊 Kích hoạt tài khoản đóng băng";
            this.Size = new Size(820, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Font font = new Font("Segoe UI", 10F);

            Label lblSearch = new Label
            {
                Text = "🔍 Tìm kiếm:",
                Location = new Point(30, 20),
                AutoSize = true,
                Font = font
            };

            txtSearch = new TextBox
            {
                Location = new Point(120, 18),
                Width = 300,
                Font = font
            };
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnActivate = new Button
            {
                Text = "✅ Kích hoạt",
                Location = new Point(440, 16),
                Width = 120,
                Height = 32,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnActivate.FlatAppearance.BorderSize = 0;
            btnActivate.Click += btnActivate_Click;

            dgvFrozenAccounts = new DataGridView
            {
                Location = new Point(30, 70),
                Width = 740,
                Height = 370,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = font,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 28 }
            };

            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnActivate);
            Controls.Add(dgvFrozenAccounts);
        }

        private void LoadFrozenAccounts()
        {
            try
            {
                var dt = accountDataAccess.GetFrozenAccounts();

                // ✅ Giải mã dữ liệu nhạy cảm
                foreach (DataRow row in dt.Rows)
                {
                    row["phone"] = EncryptionHelper.TryDecryptHybrid(row["phone"]?.ToString());
                    row["email"] = EncryptionHelper.TryDecryptHybrid(row["email"]?.ToString());
                    row["address"] = EncryptionHelper.TryDecryptHybrid(row["address"]?.ToString());
                    row["id_number"] = EncryptionHelper.TryDecryptHybrid(row["id_number"]?.ToString());
                }

                dgvFrozenAccounts.DataSource = dt;
                if (dgvFrozenAccounts.Columns.Contains("account_id"))
                {
                    dgvFrozenAccounts.Columns["account_id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi tải tài khoản đóng băng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (dgvFrozenAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn tài khoản cần kích hoạt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int accountId = Convert.ToInt32(dgvFrozenAccounts.SelectedRows[0].Cells["account_id"].Value);

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn kích hoạt lại tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                bool success = accountDataAccess.ActivateAccount(accountId);
                if (success)
                {
                    MessageBox.Show("✅ Kích hoạt tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFrozenAccounts();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("❌ Không thể kích hoạt tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi kích hoạt tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvFrozenAccounts.DataSource is DataTable dt)
            {
                string keyword = txtSearch.Text.Replace("'", "''"); // tránh lỗi SQL injection
                dt.DefaultView.RowFilter = $"account_number LIKE '%{keyword}%' OR customer_name LIKE '%{keyword}%'";
            }
        }
    }
}
