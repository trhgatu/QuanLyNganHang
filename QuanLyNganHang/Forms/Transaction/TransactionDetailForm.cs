using QuanLyNganHang.Core;
using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Transaction
{
    public partial class TransactionDetailForm : Form
    {
        private TransactionDataAccess transactionDA;
        private string transactionId;
        private DataRow transactionData;

        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;

        public TransactionDetailForm(string transactionId)
        {
            InitializeComponent();
            this.transactionId = transactionId;
            transactionDA = new TransactionDataAccess();
            InitializePrintComponents();
            LoadTransactionDetails();
        }
        private void InitializePrintComponents()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
        }

        private void LoadTransactionDetails()
        {
            try
            {
                DataTable dt = GetTransactionDetails(transactionId);

                if (dt.Rows.Count > 0)
                {
                    transactionData = dt.Rows[0]; // Lưu để in
                    PopulateTransactionInfo(transactionData);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin giao dịch!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin giao dịch: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (transactionData == null)
                {
                    MessageBox.Show("Không có dữ liệu để in!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị Print Preview trước khi in
                if (printPreviewDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in biên lai: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font normalFont = new Font("Arial", 10, FontStyle.Regular);
                Font smallFont = new Font("Arial", 8, FontStyle.Regular);

                // Margins and positions
                int leftMargin = 50;
                int rightMargin = e.PageBounds.Width - 50;
                int currentY = 50;
                int lineHeight = 20;
                int sectionSpacing = 30;

                // Colors
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 123, 255));
                SolidBrush greenBrush = new SolidBrush(Color.FromArgb(40, 167, 69));
                SolidBrush redBrush = new SolidBrush(Color.FromArgb(220, 53, 69));

                // === HEADER SECTION ===
                string bankName = SessionContext.BankName ?? "NGÂN HÀNG ABC";
                string branchName = transactionData["branch_name"]?.ToString() ?? SessionContext.BranchName ?? "Chi nhánh Hà Nội";
                string branchAddress = SessionContext.BranchAddress ?? "123 Phố ABC, Hà Nội";

                // Bank name
                DrawCenteredText(g, bankName, titleFont, blueBrush, leftMargin, rightMargin, currentY);
                currentY += 25;

                // Branch info
                DrawCenteredText(g, branchName, headerFont, blackBrush, leftMargin, rightMargin, currentY);
                currentY += 20;
                DrawCenteredText(g, branchAddress, normalFont, blackBrush, leftMargin, rightMargin, currentY);
                currentY += 20;
                DrawCenteredText(g, "Hotline: 1900-xxx-xxx", normalFont, blackBrush, leftMargin, rightMargin, currentY);
                currentY += sectionSpacing;

                // Title
                DrawCenteredText(g, "BIÊN LAI GIAO DỊCH", titleFont, blueBrush, leftMargin, rightMargin, currentY);
                currentY += sectionSpacing;

                // Separator line
                g.DrawLine(new Pen(Color.Black, 2), leftMargin, currentY, rightMargin, currentY);
                currentY += 20;

                // === TRANSACTION INFO SECTION ===
                g.DrawString("THÔNG TIN GIAO DỊCH", headerFont, blueBrush, leftMargin, currentY);
                currentY += 25;

                // Transaction details
                DrawInfoLine(g, "Mã giao dịch:", transactionData["transaction_code"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Loại giao dịch:", transactionData["transaction_type"].ToString(), normalFont, blueBrush, leftMargin, currentY);
                currentY += lineHeight;

                // Amount with color based on transaction type
                string typeCode = transactionData["type_code"].ToString();
                SolidBrush amountBrush = typeCode == "DEPOSIT" ? greenBrush :
                                       (typeCode == "WITHDRAW" || typeCode == "PAYMENT") ? redBrush : blackBrush;

                string amountText = Convert.ToDecimal(transactionData["amount"]).ToString("N0") + " VND";
                DrawInfoLine(g, "Số tiền:", amountText, normalFont, amountBrush, leftMargin, currentY);
                currentY += lineHeight;

                if (Convert.ToDecimal(transactionData["fee_amount"]) > 0)
                {
                    string feeText = Convert.ToDecimal(transactionData["fee_amount"]).ToString("N0") + " VND";
                    DrawInfoLine(g, "Phí giao dịch:", feeText, normalFont, redBrush, leftMargin, currentY);
                    currentY += lineHeight;
                }

                DrawInfoLine(g, "Thời gian GD:", transactionData["transaction_date"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Trạng thái:", transactionData["status_text"].ToString(), normalFont, greenBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Kênh giao dịch:", transactionData["channel"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += sectionSpacing;

                // === BALANCE INFO ===
                g.DrawString("THÔNG TIN SỐ DƯ", headerFont, blueBrush, leftMargin, currentY);
                currentY += 25;

                string balanceBefore = Convert.ToDecimal(transactionData["balance_before"]).ToString("N0") + " VND";
                string balanceAfter = Convert.ToDecimal(transactionData["balance_after"]).ToString("N0") + " VND";

                DrawInfoLine(g, "Số dư trước GD:", balanceBefore, normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Số dư sau GD:", balanceAfter, normalFont, greenBrush, leftMargin, currentY);
                currentY += sectionSpacing;

                // === ACCOUNT & CUSTOMER INFO ===
                g.DrawString("THÔNG TIN TÀI KHOẢN & KHÁCH HÀNG", headerFont, blueBrush, leftMargin, currentY);
                currentY += 25;

                DrawInfoLine(g, "Số tài khoản:", transactionData["account_number"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Tên khách hàng:", transactionData["customer_name"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                DrawInfoLine(g, "Loại tài khoản:", transactionData["account_type"].ToString(), normalFont, blackBrush, leftMargin, currentY);
                currentY += sectionSpacing;

                // === PROCESSING INFO ===
                g.DrawString("THÔNG TIN XỬ LÝ", headerFont, blueBrush, leftMargin, currentY);
                currentY += 25;

                string processedBy = transactionData["processed_by_name"]?.ToString() ?? "Hệ thống";
                DrawInfoLine(g, "Xử lý bởi:", processedBy, normalFont, blackBrush, leftMargin, currentY);
                currentY += lineHeight;

                string employeeCode = transactionData["processed_by_code"]?.ToString() ?? "N/A";
                DrawInfoLine(g, "Mã nhân viên:", employeeCode, normalFont, blackBrush, leftMargin, currentY);
                currentY += sectionSpacing;

                // === FOOTER ===
                currentY += 30;
                g.DrawLine(new Pen(Color.Black, 1), leftMargin, currentY, rightMargin, currentY);
                currentY += 20;

                DrawCenteredText(g, "Cảm ơn Quý khách đã sử dụng dịch vụ!", normalFont, blackBrush, leftMargin, rightMargin, currentY);
                currentY += 20;

                string printTime = "In lúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                DrawCenteredText(g, printTime, smallFont, blackBrush, leftMargin, rightMargin, currentY);
                currentY += 15;

                string reference = "Số tham chiếu: " + (transactionData["reference_number"]?.ToString() ?? transactionData["transaction_code"].ToString());
                DrawCenteredText(g, reference, smallFont, blackBrush, leftMargin, rightMargin, currentY);

                // Clean up brushes
                blackBrush.Dispose();
                blueBrush.Dispose();
                greenBrush.Dispose();
                redBrush.Dispose();

                // No more pages
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo nội dung in: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DrawCenteredText(Graphics g, string text, Font font, Brush brush, int leftMargin, int rightMargin, int y)
        {
            SizeF textSize = g.MeasureString(text, font);
            int x = leftMargin + (rightMargin - leftMargin - (int)textSize.Width) / 2;
            g.DrawString(text, font, brush, x, y);
        }

        private void DrawInfoLine(Graphics g, string label, string value, Font font, Brush valueBrush, int leftMargin, int y)
        {
            SolidBrush labelBrush = new SolidBrush(Color.Black);
            g.DrawString(label, font, labelBrush, leftMargin, y);
            g.DrawString(value, font, valueBrush, leftMargin + 150, y);
            labelBrush.Dispose();
        }

        private DataTable GetTransactionDetails(string transactionId)
        {
            // Query chi tiết giao dịch với đầy đủ thông tin
            string sql = @"
                SELECT 
                    t.transaction_id,
                    t.transaction_code,
                    t.amount,
                    t.fee_amount,
                    t.balance_before,
                    t.balance_after,
                    TO_CHAR(t.transaction_date, 'DD/MM/YYYY HH24:MI:SS') AS transaction_date,
                    t.description,
                    t.reference_number,
                    t.channel,
                    CASE t.status
                        WHEN 0 THEN 'Thất bại'
                        WHEN 1 THEN 'Thành công'
                        WHEN 2 THEN 'Đang xử lý'
                    END AS status_text,
                    tt.type_name AS transaction_type,
                    tt.type_code,
                    a.account_number,
                    c.full_name AS customer_name,
                    c.phone AS customer_phone,
                    c.email AS customer_email,
                    at.type_name AS account_type,
                    e.full_name AS processed_by_name,
                    e.employee_code AS processed_by_code,
                    b.branch_name,
                    b.branch_code
                FROM ADMIN_NGANHANG.transactions t
                JOIN ADMIN_NGANHANG.transaction_types tt ON t.transaction_type_id = tt.type_id
                JOIN ADMIN_NGANHANG.accounts a ON t.account_id = a.account_id
                JOIN ADMIN_NGANHANG.customers c ON a.customer_id = c.customer_id
                JOIN ADMIN_NGANHANG.account_types at ON a.account_type_id = at.type_id
                LEFT JOIN ADMIN_NGANHANG.employees e ON t.processed_by = e.employee_id
                LEFT JOIN ADMIN_NGANHANG.branches b ON e.branch_id = b.branch_id
                WHERE t.transaction_code = :transactionId OR t.transaction_id = :transactionId";

            using (var connection = Database.Get_Connect())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(sql, connection))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("transactionId", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = transactionId;

                    using (var adapter = new Oracle.ManagedDataAccess.Client.OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private void PopulateTransactionInfo(DataRow row)
        {
            // Thông tin giao dịch
            lblTransactionId.Text = row["transaction_code"].ToString();
            lblTransactionType.Text = row["transaction_type"].ToString();
            lblAmount.Text = Convert.ToDecimal(row["amount"]).ToString("N0") + " VND";
            lblFeeAmount.Text = Convert.ToDecimal(row["fee_amount"]).ToString("N0") + " VND";
            lblTransactionDate.Text = row["transaction_date"].ToString();
            lblStatus.Text = row["status_text"].ToString();
            lblChannel.Text = row["channel"].ToString();
            lblDescription.Text = row["description"].ToString();
            lblReferenceNumber.Text = row["reference_number"]?.ToString() ?? "N/A";

            // Thông tin số dư
            lblBalanceBefore.Text = Convert.ToDecimal(row["balance_before"]).ToString("N0") + " VND";
            lblBalanceAfter.Text = Convert.ToDecimal(row["balance_after"]).ToString("N0") + " VND";

            // Thông tin tài khoản
            lblAccountNumber.Text = row["account_number"].ToString();
            lblAccountType.Text = row["account_type"].ToString();

            // Thông tin khách hàng
            lblCustomerName.Text = row["customer_name"].ToString();
            lblCustomerPhone.Text = row["customer_phone"]?.ToString() ?? "N/A";
            lblCustomerEmail.Text = row["customer_email"]?.ToString() ?? "N/A";

            // Thông tin nhân viên xử lý
            lblProcessedBy.Text = row["processed_by_name"]?.ToString() ?? "Hệ thống";
            lblEmployeeCode.Text = row["processed_by_code"]?.ToString() ?? "N/A";
            lblBranchName.Text = row["branch_name"]?.ToString() ?? "N/A";

            // Đặt màu cho status
            SetStatusColor(row["status_text"].ToString());

            // Đặt màu cho loại giao dịch
            SetTransactionTypeColor(row["type_code"].ToString());
        }

        private void SetStatusColor(string status)
        {
            switch (status)
            {
                case "Thành công":
                    lblStatus.ForeColor = Color.FromArgb(40, 167, 69);
                    break;
                case "Thất bại":
                    lblStatus.ForeColor = Color.FromArgb(220, 53, 69);
                    break;
                case "Đang xử lý":
                    lblStatus.ForeColor = Color.FromArgb(255, 193, 7);
                    break;
            }
        }

        private void SetTransactionTypeColor(string typeCode)
        {
            switch (typeCode)
            {
                case "DEPOSIT":
                    lblAmount.ForeColor = Color.FromArgb(40, 167, 69);
                    break;
                case "WITHDRAW":
                case "PAYMENT":
                    lblAmount.ForeColor = Color.FromArgb(220, 53, 69);
                    break;
                case "TRANSFER":
                    lblAmount.ForeColor = Color.FromArgb(0, 123, 255);
                    break;
                default:
                    lblAmount.ForeColor = Color.Black;
                    break;
            }
        }


        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
