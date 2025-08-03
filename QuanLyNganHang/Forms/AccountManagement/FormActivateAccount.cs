using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
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
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblSearch = new Label
            {
                Text = "🔍 Tìm kiếm:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            txtSearch = new TextBox
            {
                Location = new Point(100, 15),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnActivate = new Button
            {
                Text = "✅ Kích hoạt",
                Location = new Point(420, 13),
                Width = 120,
                Height = 30,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnActivate.Click += btnActivate_Click;

            dgvFrozenAccounts = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 740,
                Height = 370,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 10),
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
                dgvFrozenAccounts.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi tải tài khoản đóng băng: " + ex.Message);
            }
        }


        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (dgvFrozenAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn tài khoản cần kích hoạt.");
                return;
            }

            int accountId = Convert.ToInt32(dgvFrozenAccounts.SelectedRows[0].Cells["account_id"].Value);

            try
            {
                bool success = accountDataAccess.ActivateAccount(accountId);
                if (success)
                {
                    MessageBox.Show("✅ Kích hoạt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("❌ Lỗi khi cập nhật tài khoản: " + ex.Message);
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvFrozenAccounts.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = $"account_number LIKE '%{txtSearch.Text}%' OR customer_name LIKE '%{txtSearch.Text}%'";
            }
        }
    }
}
