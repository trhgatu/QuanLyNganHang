// Các using giữ nguyên
using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
using QuanLyNganHang.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{

    public class FormAddEditCustomer : Form
    {
        private CustomerDataAccess customerDataAccess;
        private TextBox txtFullName, txtIDCard, txtPhone, txtEmail, txtAddress;
        private ComboBox cbStatus;
        private Button btnSave, btnCancel, btnCheckEncrypt;
        private Label lblCustomerCode;

        private bool isEditMode;
        private string customerId;

        public FormAddEditCustomer(bool editMode = false, string id = null)
        {
            Text = editMode ? "Sửa khách hàng" : "Thêm khách hàng mới";
            Size = new Size(520, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            customerDataAccess = new CustomerDataAccess();
            isEditMode = editMode;
            customerId = id;

            InitControls();

            if (isEditMode && !string.IsNullOrEmpty(customerId))
            {
                LoadCustomerData(customerId);
            }
            else
            {
                lblCustomerCode.Text = "KH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        private void InitControls()
        {
            Font font = new Font("Segoe UI Semibold", 10F);
            int leftLabel = 30, leftInput = 160, widthInput = 280, spacing = 40, top = 40;

            GroupBox groupBox = new GroupBox()
            {
                Text = "📝 Thông tin khách hàng",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                BackColor = Color.White,
                Size = new Size(460, 360),
                Left = 20,
                Top = 20,
                Padding = new Padding(10)
            };
            Controls.Add(groupBox);

            void AddControl(string labelText, Control control)
            {
                Label lbl = new Label()
                {
                    Text = labelText,
                    Left = leftLabel,
                    Top = top + 5,
                    Width = 120,
                    Font = font
                };
                control.Left = leftInput;
                control.Top = top;
                control.Width = widthInput;
                control.Font = font;
                control.Height = 28;
                groupBox.Controls.Add(lbl);
                groupBox.Controls.Add(control);
                top += spacing;
            }

            lblCustomerCode = new Label() { ForeColor = Color.MediumBlue, AutoSize = true };
            AddControl("Mã KH:", lblCustomerCode);

            txtFullName = new TextBox();
            AddControl("Họ tên:", txtFullName);

            txtIDCard = new TextBox();
            AddControl("CMND:", txtIDCard);

            txtPhone = new TextBox();
            AddControl("Điện thoại:", txtPhone);

            txtEmail = new TextBox();
            AddControl("Email:", txtEmail);

            txtAddress = new TextBox();
            AddControl("Địa chỉ:", txtAddress);

            cbStatus = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Height = 28 };
            cbStatus.Items.AddRange(new[] { "Hoạt động", "Khóa" });
            cbStatus.SelectedIndex = 0;
            AddControl("Trạng thái:", cbStatus);

            btnSave = new Button()
            {
                Text = "📂 Lưu",
                Width = 120,
                Height = 40,
                Left = 60,
                Top = groupBox.Bottom + 20,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += btnSave_Click;

            btnCancel = new Button()
            {
                Text = "❌ Hủy",
                Width = 120,
                Height = 40,
                Left = 200,
                Top = groupBox.Bottom + 20,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            btnCheckEncrypt = new Button()
            {
                Text = "🧪 Kiểm tra mã hóa",
                Width = 160,
                Height = 40,
                Left = 340,
                Top = groupBox.Bottom + 20,
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9F),
                FlatStyle = FlatStyle.Flat
            };
            btnCheckEncrypt.FlatAppearance.BorderSize = 0;
            btnCheckEncrypt.Click += BtnCheckEncrypt_Click;

            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(btnCheckEncrypt);
        }

        private void LoadCustomerData(string id)
        {
            try
            {
                DataRow row = customerDataAccess.GetCustomerByCode(id);
                if (row != null)
                {
                    lblCustomerCode.Text = row["customer_code"].ToString();
                    txtFullName.Text = row["full_name"].ToString();
                    txtIDCard.Text = EncryptionHelper.DecryptHybrid(row["id_number"].ToString());
                    txtPhone.Text = EncryptionHelper.DecryptHybrid(row["phone"].ToString());
                    txtEmail.Text = EncryptionHelper.DecryptHybrid(row["email"].ToString());
                    txtAddress.Text = EncryptionHelper.DecryptHybrid(row["address"].ToString());
                    cbStatus.SelectedIndex = row["status"].ToString() == "1" ? 0 : 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string code = lblCustomerCode.Text;
                string encryptedIDCard = EncryptionHelper.EncryptHybrid(txtIDCard.Text.Trim());
                string encryptedPhone = EncryptionHelper.EncryptHybrid(txtPhone.Text.Trim());
                string encryptedEmail = EncryptionHelper.EncryptHybrid(txtEmail.Text.Trim());
                string encryptedAddress = EncryptionHelper.EncryptHybrid(txtAddress.Text.Trim());

                bool result = customerDataAccess.SaveCustomer(
                    isEditMode,
                    code,
                    txtFullName.Text.Trim(),
                    encryptedIDCard,
                    encryptedPhone,
                    encryptedEmail,
                    encryptedAddress,
                    cbStatus.SelectedIndex == 0 ? 1 : 0
                );

                if (result)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Không thể lưu dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCheckEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                EncryptDebugger.TestEncryption("CMND", txtIDCard.Text.Trim());
                EncryptDebugger.TestEncryption("Số điện thoại", txtPhone.Text.Trim());
                EncryptDebugger.TestEncryption("Email", txtEmail.Text.Trim());
                EncryptDebugger.TestEncryption("Địa chỉ", txtAddress.Text.Trim());

                MessageBox.Show("✅ Kiểm tra mã hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra mã hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
