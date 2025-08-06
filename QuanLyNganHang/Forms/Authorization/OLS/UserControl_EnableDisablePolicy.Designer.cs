using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.OLS
{
    partial class UserControl_EnableDisablePolicy
    {
        private System.ComponentModel.IContainer components = null;
        public System.Windows.Forms.ComboBox cboName;
        public System.Windows.Forms.Label lb_Status;
        public System.Windows.Forms.RadioButton rdbEnable, rdbDisable;
        private System.Windows.Forms.Button btnCommit;
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel actionPanel;
        private GroupBox policyGroupBox;
        private GroupBox statusGroupBox;
        private Label titleLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            InitializeMainLayout();
            InitializeHeader();
            InitializeContent();
            InitializeActions();
            InitializeUserControl(); // ← Thêm dòng này
            SetupEventHandlers(); // ← Di chuyển xuống cuối để đảm bảo tất cả controls đã được tạo
        }

        private void InitializeMainLayout()
        {
            // Main container
            this.mainPanel = new Panel();
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(250, 252, 255);
            this.mainPanel.Padding = new Padding(25); // Tăng padding từ 20 lên 25

            // Header Panel
            this.headerPanel = new Panel();
            this.headerPanel.Height = 70; // Tăng từ 60 lên 70
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Paint += HeaderPanel_Paint;

            // Content Panel
            this.contentPanel = new Panel();
            this.contentPanel.Height = 200; // Tăng từ 140 lên 200
            this.contentPanel.Dock = DockStyle.Top;
            this.contentPanel.BackColor = Color.Transparent;
            this.contentPanel.Padding = new Padding(0, 15, 0, 15); // Tăng padding

            // Action Panel
            this.actionPanel = new Panel();
            this.actionPanel.Height = 80; // Tăng từ 70 lên 80
            this.actionPanel.Dock = DockStyle.Top;
            this.actionPanel.BackColor = Color.Transparent;
        }

        private void InitializeHeader()
        {
            // Title
            this.titleLabel = new Label();
            this.titleLabel.Text = "Enable/Disable Policy Configuration";
            this.titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.titleLabel.ForeColor = Color.FromArgb(52, 73, 94);
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(20, 18);
            this.titleLabel.BackColor = Color.Transparent;

            this.headerPanel.Controls.Add(this.titleLabel);
        }

        private void InitializeContent()
        {
            // Policy Selection Group - Điều chỉnh kích thước và vị trí
            this.policyGroupBox = new GroupBox();
            this.policyGroupBox.Text = "Policy Selection";
            this.policyGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.policyGroupBox.ForeColor = Color.FromArgb(52, 73, 94);
            this.policyGroupBox.Size = new Size(350, 100); // Tăng kích thước
            this.policyGroupBox.Location = new Point(20, 20);
            this.policyGroupBox.BackColor = Color.White;

            var lblPolicy = new Label();
            lblPolicy.Text = "Chọn Policy:";
            lblPolicy.Font = new Font("Segoe UI", 10F); // Tăng font size
            lblPolicy.ForeColor = Color.FromArgb(52, 73, 94);
            lblPolicy.Location = new Point(20, 35); // Điều chỉnh vị trí
            lblPolicy.AutoSize = true;

            this.cboName = new ComboBox();
            this.cboName.Name = "cboName";
            this.cboName.Location = new Point(20, 60); // Điều chỉnh vị trí
            this.cboName.Size = new Size(310, 30); // Tăng kích thước
            this.cboName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboName.Font = new Font("Segoe UI", 10F); // Tăng font size
            this.cboName.BackColor = Color.White;
            this.cboName.FlatStyle = FlatStyle.Flat;

            this.policyGroupBox.Controls.Add(lblPolicy);
            this.policyGroupBox.Controls.Add(this.cboName);

            // Status Group - Điều chỉnh kích thước và vị trí
            this.statusGroupBox = new GroupBox();
            this.statusGroupBox.Text = "Current Status";
            this.statusGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.statusGroupBox.ForeColor = Color.FromArgb(52, 73, 94);
            this.statusGroupBox.Size = new Size(300, 100); // Tăng kích thước
            this.statusGroupBox.Location = new Point(390, 20); // Điều chỉnh vị trí
            this.statusGroupBox.BackColor = Color.White;

            var lblStatusText = new Label();
            lblStatusText.Text = "Status:";
            lblStatusText.Font = new Font("Segoe UI", 10F); // Tăng font size
            lblStatusText.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatusText.Location = new Point(20, 35); // Điều chỉnh vị trí
            lblStatusText.AutoSize = true;

            this.lb_Status = new Label();
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Location = new Point(20, 60); // Điều chỉnh vị trí
            this.lb_Status.Size = new Size(260, 25); // Tăng kích thước
            this.lb_Status.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // Tăng font size
            this.lb_Status.ForeColor = Color.FromArgb(127, 140, 141);
            this.lb_Status.Text = "No policy selected";

            this.statusGroupBox.Controls.Add(lblStatusText);
            this.statusGroupBox.Controls.Add(this.lb_Status);

            // Action Radio Buttons Panel - Điều chỉnh kích thước và vị trí
            var radioPanel = new Panel();
            radioPanel.Size = new Size(670, 60); // Tăng kích thước
            radioPanel.Location = new Point(20, 140); // Điều chỉnh vị trí
            radioPanel.BackColor = Color.White;
            radioPanel.Paint += RadioPanel_Paint;

            var lblAction = new Label();
            lblAction.Text = "Select Action:";
            lblAction.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // Tăng font size
            lblAction.ForeColor = Color.FromArgb(52, 73, 94);
            lblAction.Location = new Point(20, 20); // Điều chỉnh vị trí
            lblAction.AutoSize = true;

            this.rdbEnable = new RadioButton();
            this.rdbEnable.Name = "rdbEnable";
            this.rdbEnable.Text = "Enable Policy";
            this.rdbEnable.Font = new Font("Segoe UI", 10F); // Tăng font size
            this.rdbEnable.ForeColor = Color.FromArgb(39, 174, 96);
            this.rdbEnable.Location = new Point(200, 18); // Điều chỉnh vị trí
            this.rdbEnable.AutoSize = true;

            this.rdbDisable = new RadioButton();
            this.rdbDisable.Name = "rdbDisable";
            this.rdbDisable.Text = "Disable Policy";
            this.rdbDisable.Font = new Font("Segoe UI", 10F); // Tăng font size
            this.rdbDisable.ForeColor = Color.FromArgb(231, 76, 60);
            this.rdbDisable.Location = new Point(380, 18); // Điều chỉnh vị trí
            this.rdbDisable.AutoSize = true;

            radioPanel.Controls.Add(lblAction);
            radioPanel.Controls.Add(this.rdbEnable);
            radioPanel.Controls.Add(this.rdbDisable);

            this.contentPanel.Controls.Add(this.policyGroupBox);
            this.contentPanel.Controls.Add(this.statusGroupBox);
            this.contentPanel.Controls.Add(radioPanel);
        }

        private void InitializeActions()
        {
            // Commit Button - Điều chỉnh kích thước và vị trí
            this.btnCommit = new Button();
            this.btnCommit.Text = "Apply Changes";
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new Size(180, 45); // Tăng kích thước
            this.btnCommit.Location = new Point(310, 20); // Center button
            this.btnCommit.Font = new Font("Segoe UI", 11F, FontStyle.Bold); // Tăng font size
            this.btnCommit.BackColor = Color.FromArgb(52, 152, 219);
            this.btnCommit.ForeColor = Color.White;
            this.btnCommit.FlatStyle = FlatStyle.Flat;
            this.btnCommit.FlatAppearance.BorderSize = 0;
            this.btnCommit.FlatAppearance.MouseDownBackColor = Color.FromArgb(41, 128, 185);
            this.btnCommit.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 193);
            this.btnCommit.Cursor = Cursors.Hand;

            this.actionPanel.Controls.Add(this.btnCommit);
        }

        private void InitializeUserControl()
        {
            // UserControl properties - Tăng kích thước
            this.Size = new Size(800, 400); // Tăng từ 580x290 lên 800x400
            this.MinimumSize = new Size(750, 350); // Thêm minimum size
            this.BackColor = Color.FromArgb(250, 252, 255);
            this.Font = new Font("Segoe UI", 9F);

            // Add panels to main panel trong thứ tự đúng (Bottom to Top vì dùng Dock.Top)
            this.mainPanel.Controls.Add(this.actionPanel);
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.headerPanel);

            // Add main panel to UserControl
            this.Controls.Add(this.mainPanel);
        }

        private void SetupEventHandlers()
        {
            this.btnCommit.Click += btnCommit_Click;
            this.cboName.SelectedIndexChanged += cboName_SelectedIndexChanged;

            // Add hover effects for radio buttons
            this.rdbEnable.MouseEnter += (s, e) => this.rdbEnable.BackColor = Color.FromArgb(240, 248, 255);
            this.rdbEnable.MouseLeave += (s, e) => this.rdbEnable.BackColor = Color.Transparent;

            this.rdbDisable.MouseEnter += (s, e) => this.rdbDisable.BackColor = Color.FromArgb(255, 240, 240);
            this.rdbDisable.MouseLeave += (s, e) => this.rdbDisable.BackColor = Color.Transparent;
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawLine(pen, 0, panel.Height - 1, panel.Width, panel.Height - 1);
            }
        }

        private void RadioPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        // Method to update status label color based on status
        public void UpdateStatusDisplay(string status)
        {
            if (this.lb_Status != null)
            {
                this.lb_Status.Text = status;

                if (status.ToLower().Contains("enable") || status.ToLower().Contains("true"))
                {
                    this.lb_Status.ForeColor = Color.FromArgb(39, 174, 96); // Green
                }
                else if (status.ToLower().Contains("disable") || status.ToLower().Contains("false"))
                {
                    this.lb_Status.ForeColor = Color.FromArgb(231, 76, 60); // Red
                }
                else
                {
                    this.lb_Status.ForeColor = Color.FromArgb(127, 140, 141); // Gray
                }
            }
        }
    }
}
