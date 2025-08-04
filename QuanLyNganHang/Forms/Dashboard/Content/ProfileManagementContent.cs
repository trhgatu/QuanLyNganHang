using QuanLyNganHang.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public class ProfileManagementContent : BaseContent
    {
        public ProfileManagementContent(Panel contentPanel) : base(contentPanel)
        {
        }

        public override void LoadContent()
        {
            try
            {
                ClearContent();

                var title = DashboardUIFactory.CreateTitle("🙍‍♂️ THÔNG TIN CÁ NHÂN", ContentPanel.Width);
                ContentPanel.Controls.Add(title);
                CreateProfilePanel();
                CreateActionPanel();
                CreateActivityLogPanel();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void CreateProfilePanel()
        {
            var panel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(ContentPanel.Width - 40, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(245, 245, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            string fullName = SessionContext.FullName ?? "Chưa cập nhật";
            string userName = SessionContext.OracleUser ?? "Chưa cập nhật";
            string email = SessionContext.Email ?? "Chưa cập nhật";
            string phone = SessionContext.Phone ?? "Chưa cập nhật";
            string position = SessionContext.Position ?? "Chưa cập nhật";
            string address = SessionContext.Address ?? "Chưa cập nhật";
            string branch = $"{SessionContext.BranchCode} - {SessionContext.BranchName}" ?? "Chưa cập nhật";
            string bank = $"{SessionContext.BankCode} - {SessionContext.BankName}" ?? "Chưa cập nhật";

            var lblName = new Label { Text = $"Họ tên: {fullName}", Font = new Font("Segoe UI", 10), Location = new Point(140, 20), AutoSize = true };
            var lblUser = new Label { Text = $"Tài khoản Oracle: {userName}", Font = new Font("Segoe UI", 10), Location = new Point(140, 45), AutoSize = true };
            var lblEmail = new Label { Text = $"Email: {email}", Font = new Font("Segoe UI", 10), Location = new Point(140, 70), AutoSize = true };
            var lblPhone = new Label { Text = $"SĐT: {phone}", Font = new Font("Segoe UI", 10), Location = new Point(140, 95), AutoSize = true };
            var lblPosition = new Label { Text = $"Chức vụ: {position}", Font = new Font("Segoe UI", 10), Location = new Point(140, 120), AutoSize = true };
            var lblAddress = new Label { Text = $"Địa chỉ: {address}", Font = new Font("Segoe UI", 10), Location = new Point(140, 145), AutoSize = true };
            var lblBranch = new Label { Text = $"Chi nhánh: {branch}", Font = new Font("Segoe UI", 10), Location = new Point(140, 170), AutoSize = true };
            var lblBank = new Label { Text = $"Ngân hàng: {bank}", Font = new Font("Segoe UI", 10), Location = new Point(140, 195), AutoSize = true };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblUser);
            panel.Controls.Add(lblEmail);
            panel.Controls.Add(lblPhone);
            panel.Controls.Add(lblPosition);
            panel.Controls.Add(lblAddress);
            panel.Controls.Add(lblBranch);
            panel.Controls.Add(lblBank);

            ContentPanel.Controls.Add(panel);
        }


        private void CreateActionPanel()
        {
            var actionPanel = new Panel
            {
                Location = new Point(20, 240),
                Size = new Size(ContentPanel.Width - 40, 48),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var btnUpdate = DashboardUIFactory.CreateActionButton("Cập nhật", DashboardConstants.Colors.Success, UpdateProfile, 130);
            var btnChangePw = DashboardUIFactory.CreateActionButton("Đổi mật khẩu", DashboardConstants.Colors.Info, ChangePassword, 150);

            btnUpdate.Location = new Point(0, 0);
            btnChangePw.Location = new Point(150, 0);

            actionPanel.Controls.Add(btnUpdate);
            actionPanel.Controls.Add(btnChangePw);

            ContentPanel.Controls.Add(actionPanel);
        }

        private void CreateActivityLogPanel()
        {
            var groupBox = new GroupBox
            {
                Text = "🕒 Lịch sử hoạt động gần đây",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 300),
                Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            DataGridView logGrid = DashboardUIFactory.CreateDataGrid();
            logGrid.Location = new Point(15, 30);
            logGrid.Size = new Size(groupBox.Width - 30, groupBox.Height - 45);
            logGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Dữ liệu mẫu
            logGrid.Columns.Add("Time", "Thời gian");
            logGrid.Columns.Add("Action", "Hành động");
            logGrid.Columns.Add("Status", "Kết quả");

            logGrid.Rows.Add("25/07/2025 08:35", "Đăng nhập", "Thành công");
            logGrid.Rows.Add("24/07/2025 22:12", "Đổi mật khẩu", "Thành công");
            logGrid.Rows.Add("24/07/2025 13:05", "Cập nhật thông tin", "Thành công");
            logGrid.Rows.Add("24/07/2025 08:34", "Đăng nhập", "Thành công");

            logGrid.Columns["Time"].Width = 140;
            logGrid.Columns["Action"].Width = 200;
            logGrid.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            groupBox.Controls.Add(logGrid);

            ContentPanel.Controls.Add(groupBox);
        }

        private void UpdateProfile()
        {
            ShowMessage("Tính năng cập nhật thông tin đang phát triển.");
        }

        private void ChangePassword()
        {
            ShowMessage("Tính năng đổi mật khẩu đang phát triển.");
        }

        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Đã làm mới hồ sơ cá nhân!");
        }
    }
}
