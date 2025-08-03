using QuanLyNganHang.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class AccountManagementContent : BaseContent
    {
        private AccountDataAccess accountDataAccess;
        public AccountManagementContent(Panel contentPanel) : base(contentPanel)
        {
            accountDataAccess = new AccountDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("🏦 QUẢN LÝ TÀI KHOẢN NGÂN HÀNG", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                LoadAccountStatistics();
                CreateAccountActionPanel();
                LoadAccountDataGrid();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu tài khoản: {ex.Message}");
            }
        }

        private void LoadAccountStatistics()
        {
            try
            {
                var statTable = accountDataAccess.GetAccountStatistics();

                int total = 0, active = 0, frozen = 0, closed = 0;

                foreach (DataRow row in statTable.Rows)
                {
                    int status = Convert.ToInt32(row["status"]);
                    int count = Convert.ToInt32(row["count"]);
                    total += count;

                    switch (status)
                    {
                        case 1: active = count; break;
                        case -1: frozen = count; break;
                        case 0: closed = count; break;
                    }
                }

                var statsPanel = CreateStatsPanel(new[]
                {
            ("Tổng TK", total.ToString(), DashboardConstants.Colors.Info),
            ("TK Hoạt động", active.ToString(), DashboardConstants.Colors.Success),
            ("TK Đóng băng", frozen.ToString(), DashboardConstants.Colors.Warning),
            ("TK Đã đóng", closed.ToString(), DashboardConstants.Colors.Danger)
        });

                ContentPanel.Controls.Add(statsPanel);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải thống kê: {ex.Message}");
            }
        }


        private void CreateAccountActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Mở TK mới", DashboardConstants.Colors.Success, (Action)ShowOpenAccountForm),
                ("Đóng băng TK", DashboardConstants.Colors.Warning, (Action)ShowFreezeAccountForm),
                ("Kích hoạt TK", DashboardConstants.Colors.Info, (Action)ShowActivateAccountForm),
                ("Đóng TK", DashboardConstants.Colors.Danger, (Action)ShowCloseAccountForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadAccountDataGrid()
        {
            var dgv = CreateDataGrid(
                new[] { "AccountNumber", "CustomerName", "AccountType", "Balance", "Status", "OpenDate" },
                new[] { "Số TK", "Chủ TK", "Loại TK", "Số dư", "Trạng thái", "Ngày mở" });

            try
            {
                DataTable accounts = accountDataAccess.GetAccounts();
                dgv.Rows.Clear();

                foreach (DataRow row in accounts.Rows)
                {
                    string balance = "";
                    if (decimal.TryParse(row["balance"]?.ToString(), out decimal b))
                        balance = b.ToString("N0") + " VNĐ";

                    dgv.Rows.Add(
                        row["account_number"],
                        row["customer_name"],
                        row["account_type"],
                        balance,
                        row["status_text"],
                        row["opened_date"]
                    );
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải danh sách tài khoản: " + ex.Message);
            }

            ContentPanel.Controls.Add(dgv);
        }


        // Action methods
        private void ShowOpenAccountForm()
        {
            var form = new FormOpenAccount();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        private void ShowFreezeAccountForm()
        {
            var form = new FormFreezeAccount();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        private void ShowActivateAccountForm()
        {
            var form = new FormActivateAccount();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        private void ShowCloseAccountForm()
        {
            var form = new FormCloseAccount();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu tài khoản đã được làm mới!");
        }
    }
}
