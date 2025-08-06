using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Shared
{
    public class FormEncryptedDataViewer : Form
    {
        public FormEncryptedDataViewer(string cmnd, string phone, string email, string address)
        {
            this.Text = "🔐 Dữ liệu mã hóa trong DB";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font textFont = new Font("Consolas", 9);

            int top = 20;
            int height = 60;
            int spacing = 20;

            void AddSection(string title, string value)
            {
                var lbl = new Label
                {
                    Text = title,
                    Font = labelFont,
                    Location = new Point(20, top),
                    AutoSize = true
                };
                var txt = new TextBox
                {
                    Text = value,
                    Font = textFont,
                    Location = new Point(20, top + 25),
                    Size = new Size(640, height),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.WhiteSmoke
                };
                this.Controls.Add(lbl);
                this.Controls.Add(txt);
                top += height + spacing + 25;
            }

            AddSection("CMND:", cmnd);
            AddSection("SĐT:", phone);
            AddSection("Email:", email);
            AddSection("Địa chỉ:", address);

            var btnOK = new Button
            {
                Text = "Đóng",
                DialogResult = DialogResult.OK,
                Location = new Point(this.Width - 120, this.Height - 80),
                Size = new Size(80, 30)
            };
            this.Controls.Add(btnOK);
        }
    }
}
