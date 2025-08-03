// Các using giữ nguyên
using Oracle.ManagedDataAccess.Client;
using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Helpers;
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
        private Button btnSave, btnCancel;
        private Label lblCustomerCode;

        private bool isEditMode;
        private string customerId;

        public FormAddEditCustomer(bool editMode = false, string id = null)
        {
            this.Text = editMode ? "Sửa khách hàng" : "Thêm khách hàng mới";
            this.Size = new Size(520, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
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
                string newCustomerCode = "KH" + DateTime.Now.ToString("yyyyMMddHHmmss");
                lblCustomerCode.Text = newCustomerCode;
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
            this.Controls.Add(groupBox);

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
                Left = 100,
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
                Left = 260,
                Top = groupBox.Bottom + 20,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            Controls.Add(btnSave);
            Controls.Add(btnCancel);
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
                    txtIDCard.Text = EncryptionHelper.DecryptRSA(row["id_number"].ToString());
                    txtPhone.Text = EncryptionHelper.DecryptRSA(row["phone"].ToString());
                    txtEmail.Text = EncryptionHelper.DecryptRSA(row["email"].ToString());
                    txtAddress.Text = EncryptionHelper.DecryptRSA(row["address"].ToString());
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
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtIDCard.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtIDCard.Text, @"^\d{12}$"))
            {
                MessageBox.Show("CMND/CCCD phải có 12 số.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không đúng định dạng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string code = lblCustomerCode.Text;
                bool result = customerDataAccess.SaveCustomer(
                    isEditMode,
                    code,
                    txtFullName.Text.Trim(),
                    txtIDCard.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtAddress.Text.Trim(),
                    cbStatus.SelectedIndex == 0 ? 1 : 0
                );

                if (result)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
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

    }
}
