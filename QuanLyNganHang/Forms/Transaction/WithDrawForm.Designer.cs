namespace QuanLyNganHang.Forms.Transaction
{
    partial class WithDrawForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.btnSearchAccount = new System.Windows.Forms.Button();
            this.grpAccountInfo = new System.Windows.Forms.GroupBox();
            this.lblWithdrawFee = new System.Windows.Forms.Label();
            this.lblAccountStatus = new System.Windows.Forms.Label();
            this.lblMinBalance = new System.Windows.Forms.Label();
            this.lblCurrentBalance = new System.Windows.Forms.Label();
            this.lblAccountType = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblVND = new System.Windows.Forms.Label();
            this.lblChannel = new System.Windows.Forms.Label();
            this.cmbChannel = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnWithdraw = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAccountInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(560, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "RÚT TIỀN TỪ TÀI KHOẢN";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAccount.Location = new System.Drawing.Point(30, 70);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(100, 23);
            this.lblAccount.TabIndex = 1;
            this.lblAccount.Text = "Số tài khoản:";
            this.lblAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAccountNumber.Location = new System.Drawing.Point(140, 70);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(200, 23);
            this.txtAccountNumber.TabIndex = 2;
            this.txtAccountNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtAccountNumber_KeyDown);
            // 
            // btnSearchAccount
            // 
            this.btnSearchAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSearchAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearchAccount.ForeColor = System.Drawing.Color.White;
            this.btnSearchAccount.Location = new System.Drawing.Point(350, 69);
            this.btnSearchAccount.Name = "btnSearchAccount";
            this.btnSearchAccount.Size = new System.Drawing.Size(80, 25);
            this.btnSearchAccount.TabIndex = 3;
            this.btnSearchAccount.Text = "Tìm kiếm";
            this.btnSearchAccount.UseVisualStyleBackColor = false;
            this.btnSearchAccount.Click += new System.EventHandler(this.BtnSearchAccount_Click);
            // 
            // grpAccountInfo
            // 
            this.grpAccountInfo.Controls.Add(this.lblWithdrawFee);
            this.grpAccountInfo.Controls.Add(this.lblAccountStatus);
            this.grpAccountInfo.Controls.Add(this.lblMinBalance);
            this.grpAccountInfo.Controls.Add(this.lblCurrentBalance);
            this.grpAccountInfo.Controls.Add(this.lblAccountType);
            this.grpAccountInfo.Controls.Add(this.lblCustomerName);
            this.grpAccountInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAccountInfo.Location = new System.Drawing.Point(30, 110);
            this.grpAccountInfo.Name = "grpAccountInfo";
            this.grpAccountInfo.Size = new System.Drawing.Size(540, 140);
            this.grpAccountInfo.TabIndex = 4;
            this.grpAccountInfo.TabStop = false;
            this.grpAccountInfo.Text = "Thông tin tài khoản";
            this.grpAccountInfo.Visible = false;
            // 
            // lblWithdrawFee
            // 
            this.lblWithdrawFee.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblWithdrawFee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblWithdrawFee.Location = new System.Drawing.Point(15, 105);
            this.lblWithdrawFee.Name = "lblWithdrawFee";
            this.lblWithdrawFee.Size = new System.Drawing.Size(250, 20);
            this.lblWithdrawFee.TabIndex = 5;
            this.lblWithdrawFee.Text = "Phí giao dịch: 5,000 VND";
            // 
            // lblAccountStatus
            // 
            this.lblAccountStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAccountStatus.Location = new System.Drawing.Point(270, 65);
            this.lblAccountStatus.Name = "lblAccountStatus";
            this.lblAccountStatus.Size = new System.Drawing.Size(250, 20);
            this.lblAccountStatus.TabIndex = 4;
            this.lblAccountStatus.Text = "Trạng thái: ";
            // 
            // lblMinBalance
            // 
            this.lblMinBalance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMinBalance.Location = new System.Drawing.Point(15, 65);
            this.lblMinBalance.Name = "lblMinBalance";
            this.lblMinBalance.Size = new System.Drawing.Size(250, 20);
            this.lblMinBalance.TabIndex = 3;
            this.lblMinBalance.Text = "Số dư tối thiểu: ";
            // 
            // lblCurrentBalance
            // 
            this.lblCurrentBalance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCurrentBalance.Location = new System.Drawing.Point(270, 45);
            this.lblCurrentBalance.Name = "lblCurrentBalance";
            this.lblCurrentBalance.Size = new System.Drawing.Size(250, 20);
            this.lblCurrentBalance.TabIndex = 2;
            this.lblCurrentBalance.Text = "Số dư hiện tại: ";
            // 
            // lblAccountType
            // 
            this.lblAccountType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAccountType.Location = new System.Drawing.Point(15, 45);
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Size = new System.Drawing.Size(250, 20);
            this.lblAccountType.TabIndex = 1;
            this.lblAccountType.Text = "Loại tài khoản: ";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCustomerName.Location = new System.Drawing.Point(15, 25);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(500, 20);
            this.lblCustomerName.TabIndex = 0;
            this.lblCustomerName.Text = "Tên khách hàng: ";
            // 
            // lblAmount
            // 
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAmount.Location = new System.Drawing.Point(30, 270);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(100, 23);
            this.lblAmount.TabIndex = 5;
            this.lblAmount.Text = "Số tiền rút:";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAmount.Location = new System.Drawing.Point(140, 270);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(200, 23);
            this.txtAmount.TabIndex = 6;
            this.txtAmount.Text = "0";
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtAmount_KeyPress);
            // 
            // lblVND
            // 
            this.lblVND.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVND.Location = new System.Drawing.Point(350, 273);
            this.lblVND.Name = "lblVND";
            this.lblVND.Size = new System.Drawing.Size(40, 20);
            this.lblVND.TabIndex = 7;
            this.lblVND.Text = "VND";
            // 
            // lblChannel
            // 
            this.lblChannel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblChannel.Location = new System.Drawing.Point(30, 310);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(100, 23);
            this.lblChannel.TabIndex = 8;
            this.lblChannel.Text = "Kênh giao dịch:";
            this.lblChannel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbChannel
            // 
            this.cmbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Items.AddRange(new object[] {
            "BRANCH",
            "ATM",
            "ONLINE",
            "MOBILE"});
            this.cmbChannel.Location = new System.Drawing.Point(140, 310);
            this.cmbChannel.Name = "cmbChannel";
            this.cmbChannel.Size = new System.Drawing.Size(200, 23);
            this.cmbChannel.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(30, 350);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 23);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Mô tả:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescription.Location = new System.Drawing.Point(140, 350);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(400, 23);
            this.txtDescription.TabIndex = 11;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblTotal.Location = new System.Drawing.Point(140, 380);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(400, 20);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "Tổng tiền cần có: [Số tiền rút] + 5,000 VND (phí)";
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnWithdraw.Enabled = false;
            this.btnWithdraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWithdraw.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnWithdraw.ForeColor = System.Drawing.Color.White;
            this.btnWithdraw.Location = new System.Drawing.Point(380, 450);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.Size = new System.Drawing.Size(90, 35);
            this.btnWithdraw.TabIndex = 13;
            this.btnWithdraw.Text = "Rút tiền";
            this.btnWithdraw.UseVisualStyleBackColor = false;
            this.btnWithdraw.Click += new System.EventHandler(this.BtnWithdraw_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(480, 450);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // WithDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 550);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnWithdraw);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbChannel);
            this.Controls.Add(this.lblChannel);
            this.Controls.Add(this.lblVND);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.grpAccountInfo);
            this.Controls.Add(this.btnSearchAccount);
            this.Controls.Add(this.txtAccountNumber);
            this.Controls.Add(this.lblAccount);
            this.Controls.Add(this.titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WithDrawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rút tiền";
            this.grpAccountInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Button btnSearchAccount;
        private System.Windows.Forms.GroupBox grpAccountInfo;
        private System.Windows.Forms.Label lblWithdrawFee;
        private System.Windows.Forms.Label lblAccountStatus;
        private System.Windows.Forms.Label lblMinBalance;
        private System.Windows.Forms.Label lblCurrentBalance;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label lblVND;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.ComboBox cmbChannel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnWithdraw;
        private System.Windows.Forms.Button btnCancel;
    }
}
