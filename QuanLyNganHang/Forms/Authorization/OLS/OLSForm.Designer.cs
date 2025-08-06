using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.OLS
{
    partial class OLSForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        internal System.Windows.Forms.TabPage tabPageTaoPolicy;
        internal System.Windows.Forms.TabPage tabPageEnableDisable;
        internal System.Windows.Forms.TabPage tabPageLabel;
        private Panel headerPanel;
        private Label titleLabel;
        private PictureBox iconPictureBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Initialize components
            InitializeHeaderPanel();
            InitializeTabControl();
            InitializeForm();
        }

        private void InitializeHeaderPanel()
        {
            // Header Panel
            this.headerPanel = new Panel();
            this.headerPanel.BackColor = Color.FromArgb(45, 66, 91); // Dark blue
            this.headerPanel.Height = 80;
            this.headerPanel.Dock = DockStyle.Top;

            // Icon
            this.iconPictureBox = new PictureBox();
            this.iconPictureBox.Size = new Size(48, 48);
            this.iconPictureBox.Location = new Point(20, 16);
            this.iconPictureBox.BackColor = Color.Transparent;
            this.iconPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            // Bạn có thể thêm icon từ resources hoặc file
            // this.iconPictureBox.Image = Properties.Resources.security_icon;

            // Title Label
            this.titleLabel = new Label();
            this.titleLabel.Text = "Oracle Label Security (OLS) Management";
            this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.titleLabel.ForeColor = Color.White;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(80, 25);
            this.titleLabel.BackColor = Color.Transparent;

            this.headerPanel.Controls.Add(this.iconPictureBox);
            this.headerPanel.Controls.Add(this.titleLabel);
        }

        private void InitializeTabControl()
        {
            // Tab Control
            this.tabControl = new TabControl();
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Segoe UI", 10F);
            this.tabControl.ItemSize = new Size(150, 40);
            this.tabControl.Padding = new Point(15, 8);
            this.tabControl.BackColor = Color.White;
            this.tabControl.Appearance = TabAppearance.Normal;
            this.tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl.DrawItem += TabControl_DrawItem;

            // Tab Pages
            InitializeTabPages();

            this.tabControl.TabPages.Add(this.tabPageTaoPolicy);
            this.tabControl.TabPages.Add(this.tabPageEnableDisable);
            this.tabControl.TabPages.Add(this.tabPageLabel);
        }

        private void InitializeTabPages()
        {
            // Tab 1: Tạo Policy & Gán quyền
            this.tabPageTaoPolicy = new TabPage("Tạo Policy & Gán quyền");
            this.tabPageTaoPolicy.BackColor = Color.FromArgb(250, 252, 255);
            this.tabPageTaoPolicy.Padding = new Padding(20);
            this.tabPageTaoPolicy.Font = new Font("Segoe UI", 9F);

            // Tab 2: Enable/Disable Policy
            this.tabPageEnableDisable = new TabPage("Enable/Disable Policy");
            this.tabPageEnableDisable.BackColor = Color.FromArgb(250, 252, 255);
            this.tabPageEnableDisable.Padding = new Padding(20);
            this.tabPageEnableDisable.Font = new Font("Segoe UI", 9F);

            // Tab 3: Thành phần OLS
            this.tabPageLabel = new TabPage("Thành phần OLS");
            this.tabPageLabel.BackColor = Color.FromArgb(250, 252, 255);
            this.tabPageLabel.Padding = new Padding(20);
            this.tabPageLabel.Font = new Font("Segoe UI", 9F);
        }

        private void InitializeForm()
        {
            // Form properties
            this.Size = new Size(900, 650);
            this.MinimumSize = new Size(800, 600);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);
            this.Text = "Oracle Label Security Management";

            // Add controls to form
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.headerPanel);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            Rectangle tabRect = e.Bounds;

            // Colors
            Color selectedColor = Color.FromArgb(52, 152, 219); // Blue
            Color normalColor = Color.FromArgb(189, 195, 199);  // Light gray
            Color textColor = e.Index == tabControl.SelectedIndex ? Color.White : Color.FromArgb(44, 62, 80);

            // Fill background
            using (SolidBrush brush = new SolidBrush(e.Index == tabControl.SelectedIndex ? selectedColor : normalColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // Draw text
            string tabText = tabControl.TabPages[e.Index].Text;
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Font tabFont = e.Index == tabControl.SelectedIndex ?
                    new Font("Segoe UI", 9F, FontStyle.Bold) :
                    new Font("Segoe UI", 9F);

                e.Graphics.DrawString(tabText, tabFont, textBrush, tabRect, stringFormat);
            }

            // Draw border for selected tab
            if (e.Index == tabControl.SelectedIndex)
            {
                using (Pen borderPen = new Pen(Color.FromArgb(41, 128, 185), 2))
                {
                    e.Graphics.DrawRectangle(borderPen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
                }
            }
        }
    }
}
