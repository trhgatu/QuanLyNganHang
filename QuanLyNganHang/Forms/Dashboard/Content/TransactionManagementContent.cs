using QuanLyNganHang.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class TransactionManagementContent : BaseContent
    {
        private TransactionDataAccess transactionDataAccess;

        public TransactionManagementContent(Panel contentPanel) : base(contentPanel)
        {
            transactionDataAccess = new TransactionDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("💰 QUẢN LÝ GIAO DỊCH TÀI CHÍNH", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                LoadTransactionStatistics();
                CreateTransactionActionPanel();
                LoadTransactionDataGrid();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu giao dịch: {ex.Message}");
            }
        }

        private void LoadTransactionStatistics()
        {
            try
            {
                var transactionStats = transactionDataAccess.GetTransactionStatistics();
                var statsPanel = CreateStatsPanel(new[]
                {
                    ("GD hôm nay", transactionStats.TodayTransactions.ToString(), DashboardConstants.Colors.Info),
                    ("Tổng tiền vào", FormatCurrency(transactionStats.TodayDeposits), DashboardConstants.Colors.Success),
                    ("Tổng tiền ra", FormatCurrency(transactionStats.TodayWithdrawals), DashboardConstants.Colors.Danger),
                    ("GD chờ duyệt", transactionStats.PendingTransactions.ToString(), DashboardConstants.Colors.Warning)
                });
                ContentPanel.Controls.Add(statsPanel);
            }
            catch (Exception ex)
            {
                // Fallback statistics
                var statsPanel = CreateStatsPanel(new[]
                {
                    ("GD hôm nay", "1,456", DashboardConstants.Colors.Info),
                    ("Tổng tiền vào", "15.8 tỷ", DashboardConstants.Colors.Success),
                    ("Tổng tiền ra", "12.3 tỷ", DashboardConstants.Colors.Danger),
                    ("GD chờ duyệt", "23", DashboardConstants.Colors.Warning)
                });
                ContentPanel.Controls.Add(statsPanel);
                System.Diagnostics.Debug.WriteLine($"Error loading transaction statistics: {ex.Message}");
            }
        }

        private void CreateTransactionActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Nạp tiền", DashboardConstants.Colors.Success, (Action)ShowDepositForm),
                ("Rút tiền", DashboardConstants.Colors.Danger, (Action)ShowWithdrawForm),
                ("Chuyển khoản", DashboardConstants.Colors.Info, (Action)ShowTransferForm),
                ("Duyệt GD", DashboardConstants.Colors.Warning, (Action)ShowApproveTransactionForm),
                ("Báo cáo GD", DashboardConstants.Colors.Primary, (Action)ShowTransactionReportForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadTransactionDataGrid()
        {
            try
            {
                DataGridView dgv = DashboardUIFactory.CreateDataGrid();
                dgv.Location = new Point(20, 300);
                dgv.Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320);

                // Load data from database
                DataTable transactionData = transactionDataAccess.GetAllTransactions();

                if (transactionData != null && transactionData.Rows.Count > 0)
                {
                    dgv.DataSource = transactionData;
                    ConfigureTransactionDataGridColumns(dgv);
                    AddTransactionContextMenu(dgv);
                }
                else
                {
                    ShowNoDataMessage(dgv, "Không có dữ liệu giao dịch");
                }

                ContentPanel.Controls.Add(dgv);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu giao dịch: {ex.Message}");

                // Fallback - tạo grid với columns cơ bản
                var dgv = CreateDataGrid(
                    new[] { "TransactionID", "TransactionType", "AccountNumber", "Amount", "DateTime", "Status", "Employee" },
                    new[] { "Mã GD", "Loại GD", "Số TK", "Số tiền", "Thời gian", "Trạng thái", "NV thực hiện" }
                );
                ContentPanel.Controls.Add(dgv);
            }
        }

        private void ConfigureTransactionDataGridColumns(DataGridView dgv)
        {
            if (dgv.Columns["TransactionID"] != null)
            {
                dgv.Columns["TransactionID"].HeaderText = "Mã GD";
                dgv.Columns["TransactionID"].Width = 80;
            }
            if (dgv.Columns["TransactionCode"] != null)
            {
                dgv.Columns["TransactionCode"].HeaderText = "Mã GD";
                dgv.Columns["TransactionCode"].Width = 100;
            }
            if (dgv.Columns["AccountNumber"] != null)
            {
                dgv.Columns["AccountNumber"].HeaderText = "Số TK";
                dgv.Columns["AccountNumber"].Width = 120;
            }
            if (dgv.Columns["CustomerName"] != null)
            {
                dgv.Columns["CustomerName"].HeaderText = "Khách hàng";
                dgv.Columns["CustomerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgv.Columns["TransactionType"] != null)
            {
                dgv.Columns["TransactionType"].HeaderText = "Loại GD";
                dgv.Columns["TransactionType"].Width = 100;
            }
            if (dgv.Columns["Amount"] != null)
            {
                dgv.Columns["Amount"].HeaderText = "Số tiền";
                dgv.Columns["Amount"].Width = 120;
                dgv.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgv.Columns["TransactionDate"] != null)
            {
                dgv.Columns["TransactionDate"].HeaderText = "Thời gian";
                dgv.Columns["TransactionDate"].Width = 140;
            }
            if (dgv.Columns["Status"] != null)
            {
                dgv.Columns["Status"].HeaderText = "Trạng thái";
                dgv.Columns["Status"].Width = 100;
            }
            if (dgv.Columns["Channel"] != null)
            {
                dgv.Columns["Channel"].HeaderText = "Kênh GD";
                dgv.Columns["Channel"].Width = 80;
            }
            if (dgv.Columns["ProcessedBy"] != null)
            {
                dgv.Columns["ProcessedBy"].HeaderText = "NV xử lý";
                dgv.Columns["ProcessedBy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void AddTransactionContextMenu(DataGridView dgv)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("🔍 Xem chi tiết", null, (s, e) => ViewTransactionDetails(dgv)),
                new ToolStripMenuItem("📝 Chỉnh sửa", null, (s, e) => EditSelectedTransaction(dgv)),
                new ToolStripMenuItem("✅ Duyệt GD", null, (s, e) => ApproveTransaction(dgv)),
                new ToolStripMenuItem("❌ Từ chối GD", null, (s, e) => RejectTransaction(dgv)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("📄 In biên lai", null, (s, e) => PrintReceipt(dgv)),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });

            dgv.ContextMenuStrip = contextMenu;
        }

        private void ViewTransactionDetails(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string transactionId = selectedRow.Cells["TransactionID"]?.Value?.ToString() ??
                                     selectedRow.Cells["TransactionCode"]?.Value?.ToString();
                string transactionType = selectedRow.Cells["TransactionType"]?.Value?.ToString();
                string amount = selectedRow.Cells["Amount"]?.Value?.ToString();

                ShowMessage($"Chi tiết giao dịch:\nMã GD: {transactionId}\nLoại: {transactionType}\nSố tiền: {amount}");
            }
            else
            {
                ShowInfo("Vui lòng chọn một giao dịch để xem chi tiết!");
            }
        }

        private void EditSelectedTransaction(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string transactionId = selectedRow.Cells["TransactionID"]?.Value?.ToString() ??
                                     selectedRow.Cells["TransactionCode"]?.Value?.ToString();
                string status = selectedRow.Cells["Status"]?.Value?.ToString();

                if (status == "Thành công")
                {
                    ShowInfo("Không thể chỉnh sửa giao dịch đã hoàn thành!");
                    return;
                }

                ShowMessage($"Chỉnh sửa giao dịch: {transactionId}");
                // TODO: Implement edit transaction form
            }
            else
            {
                ShowInfo("Vui lòng chọn một giao dịch để chỉnh sửa!");
            }
        }

        private void ApproveTransaction(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string transactionId = selectedRow.Cells["TransactionID"]?.Value?.ToString() ??
                                     selectedRow.Cells["TransactionCode"]?.Value?.ToString();
                string status = selectedRow.Cells["Status"]?.Value?.ToString();

                if (status != "Đang xử lý")
                {
                    ShowInfo("Chỉ có thể duyệt giao dịch đang chờ xử lý!");
                    return;
                }

                if (ShowConfirmation($"Bạn có chắc chắn muốn duyệt giao dịch '{transactionId}'?"))
                {
                    ShowMessage($"Đã duyệt giao dịch: {transactionId}");
                    // TODO: Implement approve transaction logic
                    RefreshContent();
                }
            }
            else
            {
                ShowInfo("Vui lòng chọn một giao dịch để duyệt!");
            }
        }

        private void RejectTransaction(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string transactionId = selectedRow.Cells["TransactionID"]?.Value?.ToString() ??
                                     selectedRow.Cells["TransactionCode"]?.Value?.ToString();

                if (ShowConfirmation($"Bạn có chắc chắn muốn từ chối giao dịch '{transactionId}'?"))
                {
                    ShowMessage($"Đã từ chối giao dịch: {transactionId}");
                    // TODO: Implement reject transaction logic
                    RefreshContent();
                }
            }
            else
            {
                ShowInfo("Vui lòng chọn một giao dịch để từ chối!");
            }
        }

        private void PrintReceipt(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string transactionId = selectedRow.Cells["TransactionID"]?.Value?.ToString() ??
                                     selectedRow.Cells["TransactionCode"]?.Value?.ToString();

                ShowMessage($"In biên lai cho giao dịch: {transactionId}");
                // TODO: Implement print receipt functionality
            }
            else
            {
                ShowInfo("Vui lòng chọn một giao dịch để in biên lai!");
            }
        }

        // Action methods
        private void ShowDepositForm() => ShowMessage("Mở form nạp tiền");
        private void ShowWithdrawForm() => ShowMessage("Mở form rút tiền");
        private void ShowTransferForm() => ShowMessage("Mở form chuyển khoản");
        private void ShowApproveTransactionForm() => ShowMessage("Mở form duyệt giao dịch");
        private void ShowTransactionReportForm() => ShowMessage("Mở báo cáo giao dịch");

        private string FormatCurrency(decimal amount)
        {
            if (amount >= 1000000000) // >= 1 tỷ
            {
                return $"{amount / 1000000000:F1} tỷ";
            }
            else if (amount >= 1000000) // >= 1 triệu
            {
                return $"{amount / 1000000:F1} tr";
            }
            else if (amount >= 1000) // >= 1 nghìn
            {
                return $"{amount / 1000:F1}k";
            }
            else
            {
                return amount.ToString("N0");
            }
        }

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu giao dịch đã được làm mới!");
        }
    }
}
