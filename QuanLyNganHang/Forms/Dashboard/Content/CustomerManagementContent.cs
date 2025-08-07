using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Forms.Shared;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class CustomerManagementContent : BaseContent
    {
        private readonly CustomerDataAccess customerDataAccess;

        public CustomerManagementContent(Panel contentPanel) : base(contentPanel)
        {
            customerDataAccess = new CustomerDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();
                ContentPanel.Controls.Add(DashboardUIFactory.CreateTitle("QUẢN LÝ KHÁCH HÀNG", ContentPanel.Width));

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
                ("Xem mã hóa KH", DashboardConstants.Colors.Danger, (Action)ShowEncryptedView),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadCustomerDataGrid()
        {
            try
            {
                ContentPanel.Controls.RemoveByKey("dataGridView1");

                var dgv = DashboardUIFactory.CreateDataGrid();
                dgv.Name = "dataGridView1";
                dgv.Location = new Point(20, 300);
                dgv.Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320);
                dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                var customerData = customerDataAccess.GetAllCustomers();

                if (customerData?.Rows.Count > 0)
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
                dgv.Columns["CMND"].HeaderText = "🔐 CMND";
            if (dgv.Columns["Điện thoại"] != null)
                dgv.Columns["Điện thoại"].HeaderText = "🔐 Điện thoại";
            if (dgv.Columns["Email"] != null)
                dgv.Columns["Email"].HeaderText = "🔐 Email";
            if (dgv.Columns["Địa chỉ"] != null)
                dgv.Columns["Địa chỉ"].HeaderText = "🔐 Địa chỉ";
            if (dgv.Columns["Trạng thái"] != null)
                dgv.Columns["Trạng thái"].Width = 100;
        }

        private void AddCustomerContextMenu(DataGridView dgv)
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("👁️ Xem chi tiết", null, (s, e) => ShowCustomerDetails(dgv)),
                new ToolStripMenuItem("✏️ Chỉnh sửa", null, (s, e) => EditCustomer(dgv)),
                new ToolStripMenuItem("🗑️ Khóa khách hàng", null, (s, e) => LockCustomer(dgv)),
                new ToolStripMenuItem("❌ Xóa khách hàng", null, (s, e) => DeleteCustomer(dgv)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });
            dgv.ContextMenuStrip = contextMenu;

            // Ensure right-click selects the row
            dgv.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = dgv.HitTest(e.X, e.Y);
                    if (hit.RowIndex >= 0)
                    {
                        dgv.ClearSelection();
                        dgv.Rows[hit.RowIndex].Selected = true;
                    }
                }
            };
        }

        private void ShowCustomerDetails(DataGridView dgv)
        {
            if (TryGetSelectedCustomerCode(dgv, out string code))
                new FormCustomerDetails(code).ShowDialog();
            else
                ShowInfo("Vui lòng chọn khách hàng để xem chi tiết.");
        }

        private void EditCustomer(DataGridView dgv)
        {
            if (TryGetSelectedCustomerCode(dgv, out string code))
            {
                var form = new FormAddEditCustomer(true, code);
                if (form.ShowDialog() == DialogResult.OK)
                    RefreshContent();
            }
            else
                ShowInfo("Vui lòng chọn khách hàng để chỉnh sửa.");
        }

        private void LockCustomer(DataGridView dgv)
        {
            if (TryGetSelectedCustomerCode(dgv, out string code) &&
                ShowConfirmation($"Bạn có chắc muốn khóa khách hàng {code}?"))
            {
                if (customerDataAccess.LockCustomer(code))
                {
                    ShowInfo("Đã khóa khách hàng!");
                    RefreshContent();
                }
                else
                {
                    ShowError("Không thể khóa khách hàng!");
                }
            }
            else ShowInfo("Vui lòng chọn khách hàng để khóa.");
        }

        private void DeleteCustomer(DataGridView dgv)
        {
            if (TryGetSelectedCustomerCode(dgv, out string code) &&
                ShowConfirmation($"Bạn có chắc chắn muốn xóa khách hàng {code}?\nThao tác này không thể hoàn tác."))
            {
                if (customerDataAccess.DeleteCustomer(code))
                {
                    ShowInfo("Đã xóa khách hàng thành công.");
                    RefreshContent();
                }
                else
                {
                    ShowError("Không thể xóa khách hàng.");
                }
            }
            else ShowInfo("Vui lòng chọn khách hàng để xóa.");
        }

        private void ShowEncryptedView()
        {
            if (ContentPanel.Controls.Find("dataGridView1", true).FirstOrDefault() is DataGridView dgv &&
                dgv.SelectedRows.Count > 0)
            {
                string code = dgv.SelectedRows[0].Cells["Mã KH"].Value?.ToString();
                if (!string.IsNullOrEmpty(code))
                {
                    var row = customerDataAccess.GetCustomerByCode(code);
                    if (row != null)
                    {
                        new FormEncryptedDataViewer(
                            row["id_number"].ToString(),
                            row["phone"].ToString(),
                            row["email"].ToString(),
                            row["address"].ToString()
                        ).ShowDialog();
                    }
                }
            }
            else
            {
                ShowInfo("Vui lòng chọn khách hàng để xem dữ liệu mã hóa.");
            }
        }

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
                var result = customerDataAccess.SearchCustomers(form.Keyword);
                if (ContentPanel.Controls.Find("dataGridView1", true).FirstOrDefault() is DataGridView dgv)
                    dgv.DataSource = result;
            }
        }

        private void ShowImportExcelForm() => ShowMessage("Import Excel chưa được cài đặt.");
        private void ShowExportDataForm() => ShowMessage("Export dữ liệu chưa được cài đặt.");

        private bool TryGetSelectedCustomerCode(DataGridView dgv, out string code)
        {
            code = null;
            if (dgv.SelectedRows.Count > 0)
            {
                code = dgv.SelectedRows[0].Cells["Mã KH"].Value?.ToString();
                return !string.IsNullOrEmpty(code);
            }
            return false;
        }

        public override void RefreshContent() => LoadContent();
    }
}
