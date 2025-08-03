using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormCustomerSearch : Form
    {
        public string Keyword { get; private set; }

        private TextBox txtKeyword;
        private Button btnSearch, btnCancel;

        public FormCustomerSearch()
        {
            this.Text = "🔍 Tìm kiếm khách hàng";
            this.Size = new Size(420, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Label lbl = new Label()
            {
                Text = "Nhập từ khóa (Tên, CMND, SĐT):",
                Left = 20,
                Top = 20,
                Width = 360,
                Font = new Font("Segoe UI", 10F)
            };

            txtKeyword = new TextBox()
            {
                Left = 20,
                Top = 50,
                Width = 360,
                Font = new Font("Segoe UI", 10F)
            };

            btnSearch = new Button()
            {
                Text = "🔍 Tìm",
                Width = 100,
                Left = 80,
                Top = 90,
                Height = 35,
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            btnCancel = new Button()
            {
                Text = "❌ Hủy",
                Width = 100,
                Left = 200,
                Top = 90,
                Height = 35,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            btnSearch.Click += (s, e) =>
            {
                Keyword = txtKeyword.Text.Trim();
                if (string.IsNullOrWhiteSpace(Keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.DialogResult = DialogResult.OK;
            };

            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lbl);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnCancel);
        }
    }
}
