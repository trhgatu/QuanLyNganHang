using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class CustomerManagementContent : BaseContent
    {
        private CustomerDataAccess customerDataAccess;

        public CustomerManagementContent(Panel contentPanel) : base(contentPanel)
        {
            customerDataAccess = new CustomerDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("QUẢN LÝ KHÁCH HÀNG", ContentPanel.Width);
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
            var stats = customerDataAccess.GetCustomerStatistics();
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng KH", stats.Total.ToString("N0"), DashboardConstants.Colors.Info),
                ("KH VIP", stats.Vip.ToString("N0"), DashboardConstants.Colors.Warning),
                ("KH Thường", stats.Normal.ToString("N0"), DashboardConstants.Colors.Success),
                ("KH Bị khóa", stats.Locked.ToString("N0"), DashboardConstants.Colors.Danger)
            });
            ContentPanel.Controls.Add(statsPanel);
        }

        private void CreateCustomerActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Thêm KH mới", DashboardConstants.Colors.Success, (Action)ShowAddCustomerForm),
                ("Tìm kiếm nâng cao", DashboardConstants.Colors.Primary, (Action)ShowAdvancedSearchForm),
                ("Export dữ liệu", DashboardConstants.Colors.Warning, (Action)ShowExportDataForm),
                ("Import Excel", DashboardConstants.Colors.Info, (Action)ShowImportExcelForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadCustomerDataGrid()
        {
            try
            {
                DataGridView dgv = DashboardUIFactory.CreateDataGrid();
                dgv.Name = "dataGridView1";
                dgv.Location = new Point(20, 300);
                dgv.Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320);
                dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                DataTable customerData = customerDataAccess.GetAllCustomers();

                if (customerData != null && customerData.Rows.Count > 0)
                {
                    dgv.DataSource = customerData;

                    ConfigureCustomerDataGridColumns(dgv);
                    AddCustomerContextMenu(dgv);
                }
                else
                {
                    ShowNoDataMessage(dgv, "Không có dữ liệu khách hàng.");
                }

                ContentPanel.Controls.Add(dgv);
                ContentPanel.Refresh();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách khách hàng: {ex.Message}");
            }
        }

        private void ConfigureCustomerDataGridColumns(DataGridView dgv)
        {
            if (dgv.Columns["Mã KH"] != null)
                dgv.Columns["Mã KH"].Width = 80;
            if (dgv.Columns["Họ tên"] != null)
                dgv.Columns["Họ tên"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgv.Columns["CMND"] != null)
                dgv.Columns["CMND"].Width = 130;
            if (dgv.Columns["Điện thoại"] != null)
                dgv.Columns["Điện thoại"].Width = 120;
            if (dgv.Columns["Email"] != null)
                dgv.Columns["Email"].Width = 200;
            if (dgv.Columns["Địa chỉ"] != null)
                dgv.Columns["Địa chỉ"].Width = 220;
            if (dgv.Columns["Trạng thái"] != null)
                dgv.Columns["Trạng thái"].Width = 100;
        }

        private void AddCustomerContextMenu(DataGridView dgv)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("👁️ Xem chi tiết", null, (s, e) => ShowCustomerDetails(dgv)),
                new ToolStripMenuItem("✏️ Chỉnh sửa", null, (s, e) => EditCustomer(dgv)),
                new ToolStripMenuItem("🗑️ Khóa khách hàng", null, (s, e) => LockCustomer(dgv)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });

            dgv.ContextMenuStrip = contextMenu;
        }

        private void ShowCustomerDetails(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                string code = dgv.SelectedRows[0].Cells["Mã KH"].Value?.ToString();
                if (!string.IsNullOrEmpty(code))
                {
                    var form = new FormCustomerDetails(code);
                    form.ShowDialog();
                }
                else ShowInfo("Không tìm thấy mã khách hàng.");
            }
            else ShowInfo("Vui lòng chọn khách hàng để xem chi tiết.");
        }

        private void EditCustomer(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                string code = dgv.SelectedRows[0].Cells["Mã KH"].Value?.ToString();
                if (!string.IsNullOrEmpty(code))
                {
                    var form = new FormAddEditCustomer(true, code);
                    if (form.ShowDialog() == DialogResult.OK)
                        RefreshContent();
                }
            }
            else ShowInfo("Vui lòng chọn khách hàng để chỉnh sửa.");
        }

        private void LockCustomer(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                string code = dgv.SelectedRows[0].Cells["Mã KH"].Value?.ToString();
                if (!string.IsNullOrEmpty(code))
                {
                    if (ShowConfirmation($"Bạn có chắc muốn khóa khách hàng {code}?"))
                    {
                        bool success = customerDataAccess.LockCustomer(code);
                        if (success)
                        {
                            ShowInfo("Đã khóa khách hàng!");
                            RefreshContent();
                        }
                        else
                        {
                            ShowError("Không thể khóa khách hàng!");
                        }
                    }
                }
            }
            else ShowInfo("Vui lòng chọn khách hàng để khóa.");
        }

        // Action methods
        private void ShowAddCustomerForm()
        {
            var form = new FormAddEditCustomer();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        private void ShowAdvancedSearchForm()
        {
            var form = new FormCustomerSearch();
            if (form.ShowDialog() == DialogResult.OK)
            {
                string keyword = form.Keyword;
                DataTable result = customerDataAccess.SearchCustomers(keyword);

                var dgv = ContentPanel.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;
                if (dgv != null)
                    dgv.DataSource = result;
            }
        }

        private void ShowImportExcelForm() => ShowMessage("Import Excel chưa được cài đặt.");
        private void ShowExportDataForm() => ShowMessage("Export dữ liệu chưa được cài đặt.");

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu khách hàng đã được làm mới!");
        }
    }
}
