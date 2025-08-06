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
        private TextBox txtEncryptedInfo;
        private readonly string customerCode;
        private readonly CustomerDataAccess customerDataAccess;

        public FormCustomerDetails(string code)
        {
            this.customerCode = code;
            this.customerDataAccess = new CustomerDataAccess();

            this.Text = $"Chi tiết khách hàng - {code}";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.White;
            this.MaximizeBox = false;

            InitUI();
            LoadCustomerData();
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
                    Width = 380,
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

            Label lblEncrypted = new Label()
            {
                Text = "Dữ liệu đã mã hóa:",
                Top = top + 10,
                Left = 30,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true
            };
            txtEncryptedInfo = new TextBox()
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Left = 30,
                Top = top + 40,
                Width = 510,
                Height = 150,
                Font = new Font("Consolas", 9),
                ReadOnly = true
            };

            this.Controls.Add(lblEncrypted);
            this.Controls.Add(txtEncryptedInfo);
        }

        private void LoadCustomerData()
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

                txtEncryptedInfo.Text =
                    $"CMND:    {row["id_number"]}\r\n" +
                    $"SĐT:      {row["phone"]}\r\n" +
                    $"Email:    {row["email"]}\r\n" +
                    $"Địa chỉ:  {row["address"]}";
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
                return EncryptionHelper.DecryptHybrid(value?.ToString());
            }
            catch
            {
                return "(Không giải mã được)";
            }
        }
    }
}
