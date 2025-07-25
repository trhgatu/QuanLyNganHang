using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class AccountManagementContent : BaseContent
    {
        public AccountManagementContent(Panel contentPanel) : base(contentPanel)
        {
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
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng TK", "2,456", DashboardConstants.Colors.Info),
                ("TK Hoạt động", "2,398", DashboardConstants.Colors.Success),
                ("TK Đóng băng", "58", DashboardConstants.Colors.Warning),
                ("TK Đã đóng", "12", DashboardConstants.Colors.Danger)
            });
            ContentPanel.Controls.Add(statsPanel);
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
            var dgv = CreateDataGrid(new[] { "AccountNumber", "CustomerName", "AccountType", "Balance", "Status", "OpenDate" },
                                   new[] { "Số TK", "Chủ TK", "Loại TK", "Số dư", "Trạng thái", "Ngày mở" });
            ContentPanel.Controls.Add(dgv);
        }

        // Action methods
        private void ShowOpenAccountForm() => ShowMessage("Mở tài khoản mới");
        private void ShowFreezeAccountForm() => ShowMessage("Đóng băng tài khoản");
        private void ShowActivateAccountForm() => ShowMessage("Kích hoạt tài khoản");
        private void ShowCloseAccountForm() => ShowMessage("Đóng tài khoản");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu tài khoản đã được làm mới!");
        }
    }
}
