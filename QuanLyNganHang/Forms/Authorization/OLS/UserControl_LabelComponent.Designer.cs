using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.OLS
{
    partial class UserControl_LabelComponent
    {
        private System.ComponentModel.IContainer components = null;
        public ComboBox cboPolicy, cboThanhPhan, cboChiTiet, cbo_groupParent;
        public TextBox txtNumber, txtShortName, txtLongName;
        private Button btnCreate, btnClose;
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel actionPanel;
        private GroupBox policyGroupBox;
        private GroupBox componentGroupBox;
        private GroupBox detailsGroupBox;
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
            this.headerPanel.Height = 70;
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Paint += HeaderPanel_Paint;

            // Content Panel
            this.contentPanel = new Panel();
            this.contentPanel.Height = 300;
            this.contentPanel.Dock = DockStyle.Top;
            this.contentPanel.BackColor = Color.Transparent;
            this.contentPanel.Padding = new Padding(0, 15, 0, 15);

            // Action Panel
            this.actionPanel = new Panel();
            this.actionPanel.Height = 80;
            this.actionPanel.Dock = DockStyle.Top;
            this.actionPanel.BackColor = Color.Transparent;
        }

        private void InitializeHeader()
        {
            // Title
            this.titleLabel = new Label();
            this.titleLabel.Text = "Oracle Label Security Component Management";
            this.titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.titleLabel.ForeColor = Color.FromArgb(52, 73, 94);
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(20, 20);
            this.titleLabel.BackColor = Color.Transparent;

            this.headerPanel.Controls.Add(this.titleLabel);
        }

        private void InitializeContent()
        {
            // Policy Selection Group
            this.policyGroupBox = new GroupBox();
            this.policyGroupBox.Text = "Policy Configuration";
            this.policyGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.policyGroupBox.ForeColor = Color.FromArgb(52, 73, 94);
            this.policyGroupBox.Size = new Size(380, 80);
            this.policyGroupBox.Location = new Point(20, 20);
            this.policyGroupBox.BackColor = Color.White;

            var lblPolicy = new Label();
            lblPolicy.Text = "Chọn Policy:";
            lblPolicy.Font = new Font("Segoe UI", 9F);
            lblPolicy.ForeColor = Color.FromArgb(52, 73, 94);
            lblPolicy.Location = new Point(20, 35);
            lblPolicy.AutoSize = true;

            this.cboPolicy = new ComboBox();
            this.cboPolicy.Name = "cboPolicy";
            this.cboPolicy.Location = new Point(20, 55);
            this.cboPolicy.Size = new Size(340, 25);
            this.cboPolicy.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPolicy.Font = new Font("Segoe UI", 9F);
            this.cboPolicy.BackColor = Color.White;
            this.cboPolicy.FlatStyle = FlatStyle.Flat;

            this.policyGroupBox.Controls.Add(lblPolicy);
            this.policyGroupBox.Controls.Add(this.cboPolicy);

            // Component Selection Group
            this.componentGroupBox = new GroupBox();
            this.componentGroupBox.Text = "Component Type";
            this.componentGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.componentGroupBox.ForeColor = Color.FromArgb(52, 73, 94);
            this.componentGroupBox.Size = new Size(320, 80);
            this.componentGroupBox.Location = new Point(420, 20);
            this.componentGroupBox.BackColor = Color.White;

            var lblThanhPhan = new Label();
            lblThanhPhan.Text = "Thành phần:";
            lblThanhPhan.Font = new Font("Segoe UI", 9F);
            lblThanhPhan.ForeColor = Color.FromArgb(52, 73, 94);
            lblThanhPhan.Location = new Point(20, 35);
            lblThanhPhan.AutoSize = true;

            this.cboThanhPhan = new ComboBox();
            this.cboThanhPhan.Name = "cboThanhPhan";
            this.cboThanhPhan.Location = new Point(20, 55);
            this.cboThanhPhan.Size = new Size(130, 25);
            this.cboThanhPhan.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboThanhPhan.Font = new Font("Segoe UI", 9F);
            this.cboThanhPhan.BackColor = Color.White;
            this.cboThanhPhan.FlatStyle = FlatStyle.Flat;

            var lblChiTiet = new Label();
            lblChiTiet.Text = "Chi tiết:";
            lblChiTiet.Font = new Font("Segoe UI", 9F);
            lblChiTiet.ForeColor = Color.FromArgb(52, 73, 94);
            lblChiTiet.Location = new Point(170, 35);
            lblChiTiet.AutoSize = true;

            this.cboChiTiet = new ComboBox();
            this.cboChiTiet.Name = "cboChiTiet";
            this.cboChiTiet.Location = new Point(170, 55);
            this.cboChiTiet.Size = new Size(130, 25);
            this.cboChiTiet.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboChiTiet.Font = new Font("Segoe UI", 9F);
            this.cboChiTiet.BackColor = Color.White;
            this.cboChiTiet.FlatStyle = FlatStyle.Flat;

            this.componentGroupBox.Controls.Add(lblThanhPhan);
            this.componentGroupBox.Controls.Add(this.cboThanhPhan);
            this.componentGroupBox.Controls.Add(lblChiTiet);
            this.componentGroupBox.Controls.Add(this.cboChiTiet);

            // Details Group
            this.detailsGroupBox = new GroupBox();
            this.detailsGroupBox.Text = "Component Details";
            this.detailsGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.detailsGroupBox.ForeColor = Color.FromArgb(52, 73, 94);
            this.detailsGroupBox.Size = new Size(720, 160);
            this.detailsGroupBox.Location = new Point(20, 120);
            this.detailsGroupBox.BackColor = Color.White;

            // Number field
            var lblNumber = new Label();
            lblNumber.Text = "Number:";
            lblNumber.Font = new Font("Segoe UI", 9F);
            lblNumber.ForeColor = Color.FromArgb(52, 73, 94);
            lblNumber.Location = new Point(20, 35);
            lblNumber.AutoSize = true;

            this.txtNumber = new TextBox();
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Location = new Point(20, 55);
            this.txtNumber.Size = new Size(120, 25);
            this.txtNumber.Font = new Font("Segoe UI", 9F);
            this.txtNumber.BorderStyle = BorderStyle.FixedSingle;

            // Short name field
            var lblShort = new Label();
            lblShort.Text = "Short Name:";
            lblShort.Font = new Font("Segoe UI", 9F);
            lblShort.ForeColor = Color.FromArgb(52, 73, 94);
            lblShort.Location = new Point(160, 35);
            lblShort.AutoSize = true;

            this.txtShortName = new TextBox();
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Location = new Point(160, 55);
            this.txtShortName.Size = new Size(150, 25);
            this.txtShortName.Font = new Font("Segoe UI", 9F);
            this.txtShortName.BorderStyle = BorderStyle.FixedSingle;

            // Long name field
            var lblLong = new Label();
            lblLong.Text = "Long Name:";
            lblLong.Font = new Font("Segoe UI", 9F);
            lblLong.ForeColor = Color.FromArgb(52, 73, 94);
            lblLong.Location = new Point(330, 35);
            lblLong.AutoSize = true;

            this.txtLongName = new TextBox();
            this.txtLongName.Name = "txtLongName";
            this.txtLongName.Location = new Point(330, 55);
            this.txtLongName.Size = new Size(200, 25);
            this.txtLongName.Font = new Font("Segoe UI", 9F);
            this.txtLongName.BorderStyle = BorderStyle.FixedSingle;

            // Parent field
            var lblParent = new Label();
            lblParent.Text = "Parent Name:";
            lblParent.Font = new Font("Segoe UI", 9F);
            lblParent.ForeColor = Color.FromArgb(52, 73, 94);
            lblParent.Location = new Point(20, 95);
            lblParent.AutoSize = true;

            this.cbo_groupParent = new ComboBox();
            this.cbo_groupParent.Name = "cbo_groupParent";
            this.cbo_groupParent.Location = new Point(20, 115);
            this.cbo_groupParent.Size = new Size(290, 25);
            this.cbo_groupParent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbo_groupParent.Font = new Font("Segoe UI", 9F);
            this.cbo_groupParent.BackColor = Color.White;
            this.cbo_groupParent.FlatStyle = FlatStyle.Flat;

            this.detailsGroupBox.Controls.Add(lblNumber);
            this.detailsGroupBox.Controls.Add(this.txtNumber);
            this.detailsGroupBox.Controls.Add(lblShort);
            this.detailsGroupBox.Controls.Add(this.txtShortName);
            this.detailsGroupBox.Controls.Add(lblLong);
            this.detailsGroupBox.Controls.Add(this.txtLongName);
            this.detailsGroupBox.Controls.Add(lblParent);
            this.detailsGroupBox.Controls.Add(this.cbo_groupParent);

            this.contentPanel.Controls.Add(this.policyGroupBox);
            this.contentPanel.Controls.Add(this.componentGroupBox);
            this.contentPanel.Controls.Add(this.detailsGroupBox);
        }

        private void InitializeActions()
        {
            this.btnCreate = new Button();
            this.btnCreate.Text = "Tạo mới";
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new Size(200, 45);
            this.btnCreate.Location = new Point(300, 20);
            this.btnCreate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCreate.BackColor = Color.FromArgb(149, 165, 166);
            this.btnCreate.ForeColor = Color.White;
            this.btnCreate.FlatStyle = FlatStyle.Flat;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatAppearance.MouseDownBackColor = Color.FromArgb(127, 140, 141);
            this.btnCreate.FlatAppearance.MouseOverBackColor = Color.FromArgb(171, 183, 183);
            this.btnCreate.Cursor = Cursors.Hand;

            this.actionPanel.Controls.Add(this.btnCreate);
        }

        private void InitializeUserControl()
        {
            // UserControl properties
            this.Size = new Size(800, 500);
            this.MinimumSize = new Size(750, 450);
            this.BackColor = Color.FromArgb(250, 252, 255);
            this.Font = new Font("Segoe UI", 9F);

            // Add panels to main panel
            this.mainPanel.Controls.Add(this.actionPanel);
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.headerPanel);

            // Add main panel to UserControl
            this.Controls.Add(this.mainPanel);
        }

        private void SetupEventHandlers()
        {
            this.btnCreate.Click += btnCreate_Click;
            this.cboThanhPhan.SelectedIndexChanged += cboThanhPhan_SelectedIndexChanged;
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawLine(pen, 0, panel.Height - 1, panel.Width, panel.Height - 1);
            }
        }

        // Method to show/hide fields based on component type
        private void UpdateFieldVisibility(string componentType)
        {
            bool showParent = componentType == "Group";

            // Find parent controls in details group
            foreach (Control control in this.detailsGroupBox.Controls)
            {
                if (control.Name == "cbo_groupParent" ||
                    (control is Label && control.Text == "Parent Name:"))
                {
                    control.Visible = showParent;
                }
            }
        }
    }
}
