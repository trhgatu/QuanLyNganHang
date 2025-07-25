using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class CustomerManagementContent : BaseContent
    {
        public CustomerManagementContent(Panel contentPanel) : base(contentPanel)
        {
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("👤 QUẢN LÝ KHÁCH HÀNG", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                LoadCustomerStatistics();
                CreateCustomerActionPanel();
                LoadCustomerDataGrid();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu khách hàng: {ex.Message}");
            }
        }

        private void LoadCustomerStatistics()
        {
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng KH", "1,234", DashboardConstants.Colors.Info),
                ("KH VIP", "89", DashboardConstants.Colors.Warning),
                ("KH Thường", "1,145", DashboardConstants.Colors.Success),
                ("KH Bị khóa", "15", DashboardConstants.Colors.Danger)
            });
            ContentPanel.Controls.Add(statsPanel);
        }

        private void CreateCustomerActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Thêm KH mới", DashboardConstants.Colors.Success, (Action)ShowAddCustomerForm),
                ("Import Excel", DashboardConstants.Colors.Info, (Action)ShowImportExcelForm),
                ("Export dữ liệu", DashboardConstants.Colors.Warning, (Action)ShowExportDataForm),
                ("Tìm kiếm nâng cao", DashboardConstants.Colors.Primary, (Action)ShowAdvancedSearchForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadCustomerDataGrid()
        {
            var dgv = CreateDataGrid(new[] { "CustomerID", "FullName", "IDCard", "Phone", "Email", "Address", "Status" },
                                   new[] { "Mã KH", "Họ tên", "CMND/CCCD", "Điện thoại", "Email", "Địa chỉ", "Trạng thái" });
            ContentPanel.Controls.Add(dgv);
        }

        // Action methods
        private void ShowAddCustomerForm() => ShowMessage("Thêm khách hàng mới");
        private void ShowImportExcelForm() => ShowMessage("Import Excel");
        private void ShowExportDataForm() => ShowMessage("Export dữ liệu");
        private void ShowAdvancedSearchForm() => ShowMessage("Tìm kiếm nâng cao");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu khách hàng đã được làm mới!");
        }
    }
}
