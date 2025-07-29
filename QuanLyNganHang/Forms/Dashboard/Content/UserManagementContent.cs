using QuanLyNganHang.DataAccess;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class UserManagementContent : BaseContent
    {
        private UserDataAccess userDataAccess;

        public UserManagementContent(Panel contentPanel) : base(contentPanel)
        {
            userDataAccess = new UserDataAccess();
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("QUẢN LÝ NGƯỜI DÙNG HỆ THỐNG", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                LoadUserStatistics();
                CreateUserActionPanel();
                LoadUserDataGrid();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu người dùng: {ex.Message}");
            }
        }

        private void LoadUserStatistics()
        {
            var userStats = userDataAccess.GetUserStatistics();
            var statsPanel = CreateStatsPanel(new[]
            {
                ("Tổng Admin", userStats.TotalAdmin.ToString(), DashboardConstants.Colors.Danger),
                ("Tổng Nhân viên", userStats.TotalEmployee.ToString(), DashboardConstants.Colors.Info),
                ("Đang hoạt động", userStats.ActiveUsers.ToString(), DashboardConstants.Colors.Success),
                ("Bị khóa", userStats.LockedUsers.ToString(), DashboardConstants.Colors.Warning)
            });
            ContentPanel.Controls.Add(statsPanel);
        } 

        private void CreateUserActionPanel()
        {
            var actionPanel = CreateActionPanel(new[]
            {
                ("Thêm Admin", DashboardConstants.Colors.Danger, (Action)ShowAddAdminForm),
                ("Profile", DashboardConstants.Colors.Info, (Action)ShowProfileForm),
                ("Thêm Nhân viên", DashboardConstants.Colors.Success, (Action)ShowAddEmployeeForm),
                ("Phân quyền", DashboardConstants.Colors.Warning, (Action)ShowPermissionForm),
                ("Làm mới", DashboardConstants.Colors.Info, (Action)RefreshContent)
            });
            ContentPanel.Controls.Add(actionPanel);
        }

        private void LoadUserDataGrid()
        {
            try
            {
                DataGridView dgv = DashboardUIFactory.CreateDataGrid();
                dgv.Location = new Point(20, 300);
                dgv.Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320);
                dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                DataTable userData = userDataAccess.GetAllUsers();

                if (userData != null && userData.Rows.Count > 0)
                {
                    
                    dgv.DataSource = userData;

                    ConfigureUserDataGridColumns(dgv);
                    AddUserContextMenu(dgv);
                }
                else
                {
                    ShowNoDataMessage(dgv, "Không có dữ liệu người dùng");
                }

                ContentPanel.Controls.Add(dgv);
                ContentPanel.Refresh();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }

        private void ConfigureUserDataGridColumns(DataGridView dgv)
        {
            if (dgv.Columns["ID"] != null)
            {
                dgv.Columns["ID"].HeaderText = "ID";
                dgv.Columns["ID"].Width = 60;
            }
            if (dgv.Columns["Username"] != null)
            {
                dgv.Columns["Username"].HeaderText = "Tên đăng nhập";
                dgv.Columns["Username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgv.Columns["FullName"] != null)
            {
                dgv.Columns["FullName"].HeaderText = "Họ tên";
                dgv.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgv.Columns["Role"] != null)
            {
                dgv.Columns["Role"].HeaderText = "Vai trò";
                dgv.Columns["Role"].Width = 100;
            }
            if (dgv.Columns["Status"] != null)
            {
                dgv.Columns["Status"].HeaderText = "Trạng thái";
                dgv.Columns["Status"].Width = 120;
            }
            if (dgv.Columns["LastLogin"] != null)
            {
                dgv.Columns["LastLogin"].HeaderText = "Đăng nhập cuối";
                dgv.Columns["LastLogin"].Width = 150;
            }
        }

        private void AddUserContextMenu(DataGridView dgv)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("✏️ Chỉnh sửa", null, (s, e) => EditSelectedUser(dgv)),
                new ToolStripMenuItem("🗑️ Xóa", null, (s, e) => DeleteSelectedUser(dgv)),
                new ToolStripSeparator(),
                new ToolStripMenuItem("🔄 Làm mới", null, (s, e) => RefreshContent())
            });

            dgv.ContextMenuStrip = contextMenu;
        }

        private void EditSelectedUser(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string userId = selectedRow.Cells["ID"].Value?.ToString();
                string username = selectedRow.Cells["Username"].Value?.ToString();

                ShowMessage($"Chỉnh sửa người dùng: {username} (ID: {userId})");
                // TODO: Implement edit user form
            }
            else
            {
                ShowInfo("Vui lòng chọn một người dùng để chỉnh sửa!");
            }
        }

        private void DeleteSelectedUser(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                string userId = selectedRow.Cells["ID"].Value?.ToString();
                string username = selectedRow.Cells["Username"].Value?.ToString();

                if (ShowConfirmation($"Bạn có chắc chắn muốn xóa người dùng '{username}'?"))
                {
                    ShowMessage($"Xóa người dùng: {username} (ID: {userId})");
                    // TODO: Implement delete user logic
                    RefreshContent();
                }
            }
            else
            {
                ShowInfo("Vui lòng chọn một người dùng để xóa!");
            }
        }

        // Action methods
        private void ShowAddAdminForm() => ShowMessage("Thêm Admin");
        private void ShowAddEmployeeForm() => ShowMessage("Thêm Nhân viên");
        private void ShowPermissionForm() => ShowMessage("Phân quyền");

        private void ShowProfileForm()
        {
            try
            {
                Profile_GUI profileForm = new Profile_GUI();
                profileForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi mở form Profile: {ex.Message}");
            }
        }

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Dữ liệu người dùng đã được làm mới!");
        }
    }
}
