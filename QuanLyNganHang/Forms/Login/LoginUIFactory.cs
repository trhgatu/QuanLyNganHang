using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Login
{
    public static class LoginUIFactory
    {
        public static Label CreateLabel(string text, Point location, Font font = null, Color? color = null)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = font ?? new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = color ?? LoginConstants.Colors.TextPrimary,
                AutoSize = true
            };
        }

        public static TextBox CreateTextBox(Point location, bool isPassword = false)
        {
            TextBox txt = new TextBox
            {
                Location = location,
                Size = new Size(200, LoginConstants.Sizes.TextBoxHeight),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            if (isPassword)
            {
                txt.UseSystemPasswordChar = true;
            }

            // Add focus effects
            txt.Enter += (s, e) => ((TextBox)s).BackColor = LoginConstants.Colors.FocusBackground;
            txt.Leave += (s, e) => ((TextBox)s).BackColor = Color.White;

            return txt;
        }

        public static Button CreateButton(string text, Point location, Size size, Color backColor,
            EventHandler clickHandler = null, Font font = null)
        {
            Button btn = new Button
            {
                Text = text,
                Size = size,
                Location = location,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = font ?? new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            if (clickHandler != null)
                btn.Click += clickHandler;

            return btn;
        }

        public static CheckBox CreateCheckBox(string text, Point location, EventHandler changeHandler = null)
        {
            CheckBox chk = new CheckBox
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            if (changeHandler != null)
                chk.CheckedChanged += changeHandler;

            return chk;
        }

        public static Bitmap CreateBankIcon(int width, int height)
        {
            Bitmap bankIcon = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bankIcon))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(Color.White), 10, 10, width - 20, height - 20);
                g.FillRectangle(new SolidBrush(LoginConstants.Colors.Primary), 25, 35, width - 50, 30);
                g.DrawString("🏦", new Font("Segoe UI Symbol", 24), Brushes.White, 35, 30);
            }
            return bankIcon;
        }

        public static ProgressBar CreateProgressBar(Point location, Size size)
        {
            return new ProgressBar
            {
                Location = location,
                Size = size,
                Style = ProgressBarStyle.Continuous,
                Visible = false
            };
        }
    }
}
