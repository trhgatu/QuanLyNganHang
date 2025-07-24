using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public static class DashboardUIFactory
    {
        public static Label CreateTitle(string text, int width)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = DashboardConstants.Colors.Dark,
                AutoSize = false,
                Size = new Size(width - 40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        public static Button CreateMenuButton(string text, Action clickAction)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.MenuButtonHeight),
                FlatStyle = FlatStyle.Flat,
                BackColor = DashboardConstants.Colors.Secondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = DashboardConstants.Colors.MenuHover;

            return btn;
        }

        public static Panel CreateStatCard(string title, string value, Color color, int width)
        {
            Panel statCard = new Panel
            {
                Size = new Size(width, DashboardConstants.Sizes.StatCardHeight),
                BackColor = color,
                Padding = new Padding(15)
            };

            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(statCard.Width - 30, 40),
                Location = new Point(15, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(statCard.Width - 30, 30),
                Location = new Point(15, 55),
                TextAlign = ContentAlignment.MiddleCenter
            };

            statCard.Controls.AddRange(new Control[] { valueLabel, titleLabel });
            return statCard;
        }

        public static Button CreateActionButton(string text, Color color, Action action, int width)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, 60),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = action
            };
            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        public static DataGridView CreateDataGrid()
        {
            return new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = DashboardConstants.Colors.Info,
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 10)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = DashboardConstants.Colors.Secondary,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        public static TabControl CreateTabControl(Point location, Size size)
        {
            return new TabControl
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 10)
            };
        }

        public static TabPage CreateTabPage(string title, string description = null)
        {
            TabPage tab = new TabPage(title);

            if (!string.IsNullOrEmpty(description))
            {
                Label desc = new Label
                {
                    Text = description,
                    Location = new Point(20, 20),
                    Font = new Font("Segoe UI", 12),
                    AutoSize = true
                };
                tab.Controls.Add(desc);
            }

            return tab;
        }
    }
}
