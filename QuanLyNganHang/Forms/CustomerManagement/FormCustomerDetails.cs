using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class FormCustomerDetails : Form
    {
        private Label lblFullName, lblIDCard, lblPhone, lblEmail, lblAddress;
        private readonly string customerCode;
        private readonly CustomerDataAccess customerDataAccess;

        public FormCustomerDetails(string code)
        {
            this.customerCode = code;
            this.customerDataAccess = new CustomerDataAccess();

            this.Text = $"Thông tin chi tiết - {code}";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.White;
            this.MaximizeBox = false;

            InitUI();
            LoadDecryptedData();
        }

        private void InitUI()
        {
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);
            int top = 30, spacing = 40;

            void AddRow(string labelText, ref Label lbl)
            {
                Label title = new Label()
                {
                    Text = labelText,
                    Left = 30,
                    Top = top,
                    Width = 120,
                    Font = labelFont
                };
                lbl = new Label()
                {
                    Left = 160,
                    Top = top,
                    Width = 280,
                    Font = labelFont,
                    ForeColor = Color.DarkBlue
                };
                this.Controls.Add(title);
                this.Controls.Add(lbl);
                top += spacing;
            }

            AddRow("Họ tên:", ref lblFullName);
            AddRow("CMND:", ref lblIDCard);
            AddRow("Điện thoại:", ref lblPhone);
            AddRow("Email:", ref lblEmail);
            AddRow("Địa chỉ:", ref lblAddress);
        }

        private void LoadDecryptedData()
        {
            try
            {
                DataRow row = customerDataAccess.GetCustomerByCode(customerCode);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblFullName.Text = row["full_name"].ToString();
                lblIDCard.Text = TryDecrypt(row["id_number"]);
                lblPhone.Text = TryDecrypt(row["phone"]);
                lblEmail.Text = TryDecrypt(row["email"]);
                lblAddress.Text = TryDecrypt(row["address"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu khách hàng:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TryDecrypt(object value)
        {
            try
            {
                return EncryptionHelper.DecryptRSA(value?.ToString());
            }
            catch
            {
                return "(Không giải mã được)";
            }
        }
    }
}
