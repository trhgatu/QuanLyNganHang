using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.OLS
{
    partial class UserControl_TaoPolicyGanQuyen
    {
        private System.ComponentModel.IContainer components = null;
        public System.Windows.Forms.TextBox txtName1, txtNameColumn;
        public System.Windows.Forms.ComboBox cboName, cbo_User;
        private System.Windows.Forms.Button btnThem, btnGan;
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private GroupBox createPolicyGroupBox;
        private GroupBox assignRightsGroupBox;
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
            InitializeUserControl();
            SetupEventHandlers();
        }

        private void InitializeMainLayout()
        {
            // Main container
            this.mainPanel = new Panel();
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(250, 252, 255);
            this.mainPanel.Padding = new Padding(25);

            // Header Panel
            this.headerPanel = new Panel();
            this.headerPanel.Height = 80;
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Paint += HeaderPanel_Paint;

            // Content Panel - Tăng chiều cao
            this.contentPanel = new Panel();
            this.contentPanel.Height = 250; // Tăng từ 220 lên 250
            this.contentPanel.Dock = DockStyle.Top;
            this.contentPanel.BackColor = Color.Transparent;
            this.contentPanel.Padding = new Padding(0, 15, 0, 15);
        }


        private void InitializeHeader()
        {
            // Title
            this.titleLabel = new Label();
            this.titleLabel.Text = "Policy Management & User Rights Assignment";
            this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.titleLabel.ForeColor = Color.FromArgb(52, 73, 94);
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(20, 25);
            this.titleLabel.BackColor = Color.Transparent;

            this.headerPanel.Controls.Add(this.titleLabel);
        }

        private void InitializeContent()
        {
            // Create Policy Group - Tăng kích thước
            this.createPolicyGroupBox = new GroupBox();
            this.createPolicyGroupBox.Text = "Create New Policy";
            this.createPolicyGroupBox.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.createPolicyGroupBox.ForeColor = Color.FromArgb(39, 174, 96);
            this.createPolicyGroupBox.Size = new Size(400, 200); // Tăng kích thước
            this.createPolicyGroupBox.Location = new Point(20, 20);
            this.createPolicyGroupBox.BackColor = Color.White;

            var lblName1 = new Label();
            lblName1.Text = "Tên Policy:";
            lblName1.Font = new Font("Segoe UI", 10F);
            lblName1.ForeColor = Color.FromArgb(52, 73, 94);
            lblName1.Location = new Point(20, 35); // Điều chỉnh vị trí
            lblName1.AutoSize = true;

            this.txtName1 = new TextBox();
            this.txtName1.Name = "txtName1";
            this.txtName1.Location = new Point(20, 55); // Điều chỉnh vị trí
            this.txtName1.Size = new Size(350, 25); // Điều chỉnh kích thước
            this.txtName1.Font = new Font("Segoe UI", 10F);
            this.txtName1.BorderStyle = BorderStyle.FixedSingle;
            this.txtName1.BackColor = Color.White;

            var lblNameColumn = new Label();
            lblNameColumn.Text = "Tên Cột OLS:";
            lblNameColumn.Font = new Font("Segoe UI", 10F);
            lblNameColumn.ForeColor = Color.FromArgb(52, 73, 94);
            lblNameColumn.Location = new Point(20, 90); // Điều chỉnh vị trí
            lblNameColumn.AutoSize = true;

            this.txtNameColumn = new TextBox();
            this.txtNameColumn.Name = "txtNameColumn";
            this.txtNameColumn.Location = new Point(20, 110); // Điều chỉnh vị trí
            this.txtNameColumn.Size = new Size(350, 25); // Điều chỉnh kích thước
            this.txtNameColumn.Font = new Font("Segoe UI", 10F);
            this.txtNameColumn.BorderStyle = BorderStyle.FixedSingle;
            this.txtNameColumn.BackColor = Color.White;

            this.btnThem = new Button();
            this.btnThem.Text = "Create Policy";
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new Size(150, 35); // Điều chỉnh kích thước
            this.btnThem.Location = new Point(220, 150); // Điều chỉnh vị trí trong GroupBox
            this.btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnThem.BackColor = Color.FromArgb(39, 174, 96);
            this.btnThem.ForeColor = Color.White;
            this.btnThem.FlatStyle = FlatStyle.Flat;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatAppearance.MouseDownBackColor = Color.FromArgb(34, 153, 84);
            this.btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 204, 113);
            this.btnThem.Cursor = Cursors.Hand;

            this.createPolicyGroupBox.Controls.Add(lblName1);
            this.createPolicyGroupBox.Controls.Add(this.txtName1);
            this.createPolicyGroupBox.Controls.Add(lblNameColumn);
            this.createPolicyGroupBox.Controls.Add(this.txtNameColumn);
            this.createPolicyGroupBox.Controls.Add(this.btnThem);

            // Assign Rights Group - Tăng kích thước
            this.assignRightsGroupBox = new GroupBox();
            this.assignRightsGroupBox.Text = "Assign Management Rights";
            this.assignRightsGroupBox.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.assignRightsGroupBox.ForeColor = Color.FromArgb(52, 152, 219);
            this.assignRightsGroupBox.Size = new Size(400, 200); // Tăng kích thước
            this.assignRightsGroupBox.Location = new Point(440, 20); // Điều chỉnh vị trí
            this.assignRightsGroupBox.BackColor = Color.White;

            var lblCboName = new Label();
            lblCboName.Text = "Chọn Policy:";
            lblCboName.Font = new Font("Segoe UI", 10F);
            lblCboName.ForeColor = Color.FromArgb(52, 73, 94);
            lblCboName.Location = new Point(20, 35); // Điều chỉnh vị trí
            lblCboName.AutoSize = true;

            this.cboName = new ComboBox();
            this.cboName.Name = "cboName";
            this.cboName.Location = new Point(20, 55); // Điều chỉnh vị trí
            this.cboName.Size = new Size(350, 25); // Điều chỉnh kích thước
            this.cboName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboName.Font = new Font("Segoe UI", 10F);
            this.cboName.BackColor = Color.White;
            this.cboName.FlatStyle = FlatStyle.Flat;

            var lblUser = new Label();
            lblUser.Text = "Chọn User:";
            lblUser.Font = new Font("Segoe UI", 10F);
            lblUser.ForeColor = Color.FromArgb(52, 73, 94);
            lblUser.Location = new Point(20, 90); // Điều chỉnh vị trí
            lblUser.AutoSize = true;

            this.cbo_User = new ComboBox();
            this.cbo_User.Name = "cbo_User";
            this.cbo_User.Location = new Point(20, 110); // Điều chỉnh vị trí
            this.cbo_User.Size = new Size(350, 25); // Điều chỉnh kích thước
            this.cbo_User.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbo_User.Font = new Font("Segoe UI", 10F);
            this.cbo_User.BackColor = Color.White;
            this.cbo_User.FlatStyle = FlatStyle.Flat;

            this.btnGan = new Button();
            this.btnGan.Text = "Assign Rights";
            this.btnGan.Name = "btnGan";
            this.btnGan.Size = new Size(150, 35); // Điều chỉnh kích thước
            this.btnGan.Location = new Point(220, 150); // Điều chỉnh vị trí trong GroupBox
            this.btnGan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGan.BackColor = Color.FromArgb(52, 152, 219);
            this.btnGan.ForeColor = Color.White;
            this.btnGan.FlatStyle = FlatStyle.Flat;
            this.btnGan.FlatAppearance.BorderSize = 0;
            this.btnGan.FlatAppearance.MouseDownBackColor = Color.FromArgb(41, 128, 185);
            this.btnGan.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 193);
            this.btnGan.Cursor = Cursors.Hand;

            this.assignRightsGroupBox.Controls.Add(lblCboName);
            this.assignRightsGroupBox.Controls.Add(this.cboName);
            this.assignRightsGroupBox.Controls.Add(lblUser);
            this.assignRightsGroupBox.Controls.Add(this.cbo_User);
            this.assignRightsGroupBox.Controls.Add(this.btnGan);

            this.contentPanel.Controls.Add(this.createPolicyGroupBox);
            this.contentPanel.Controls.Add(this.assignRightsGroupBox);
        }


        private void InitializeUserControl()
        {
            // UserControl properties - Tăng kích thước
            this.Size = new Size(920, 400); // Tăng kích thước
            this.MinimumSize = new Size(900, 380);
            this.BackColor = Color.FromArgb(250, 252, 255);
            this.Font = new Font("Segoe UI", 9F);

            // Add panels to main panel
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.headerPanel);

            // Add main panel to UserControl
            this.Controls.Add(this.mainPanel);
        }


        private void SetupEventHandlers()
        {
            this.btnThem.Click += btnThem_Click;
            this.btnGan.Click += btnGan_Click;

            // Add validation events
            this.txtName1.TextChanged += ValidateCreateForm;
            this.txtNameColumn.TextChanged += ValidateCreateForm;
            this.cboName.SelectedIndexChanged += ValidateAssignForm;
            this.cbo_User.SelectedIndexChanged += ValidateAssignForm;
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawLine(pen, 0, panel.Height - 1, panel.Width, panel.Height - 1);
            }
        }

        // Validation methods
        private void ValidateCreateForm(object sender, EventArgs e)
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtName1.Text) &&
                          !string.IsNullOrWhiteSpace(txtNameColumn.Text);
            btnThem.Enabled = isValid;
        }

        private void ValidateAssignForm(object sender, EventArgs e)
        {
            bool isValid = cboName.SelectedIndex >= 0 &&
                          cbo_User.SelectedIndex >= 0;
            btnGan.Enabled = isValid;
        }
    }
}
