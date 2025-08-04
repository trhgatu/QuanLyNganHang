using QuanLyNganHang.Core;
using QuanLyNganHang.Forms.Profile;
using System;
using System.Drawing;
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

                var title = DashboardUIFactory.CreateTitle("THÔNG TIN CÁ NHÂN", ContentPanel.Width);
                ContentPanel.Controls.Add(title);

                CreateProfilePanel();
                CreateProfileActionPanel();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void CreateProfilePanel()
        {
            var profileGroup = new GroupBox
            {
                Text = "Thông tin cá nhân",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 90),
                Location = new Point(20, 70),
                Size = new Size(ContentPanel.Width - 40, 280),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Padding = new Padding(10)
            };

            TableLayoutPanel infoTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 9,
                Location = new Point(160, 30),
                Size = new Size(profileGroup.Width - 180, profileGroup.Height - 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = false
            };

            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            for (int i = 0; i < 9; i++)
                infoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            string fullName = SessionContext.FullName ?? "Chưa cập nhật";
            string userName = SessionContext.OracleUser ?? "Chưa cập nhật";
            string email = SessionContext.Email ?? "Chưa cập nhật";
            string phone = SessionContext.Phone ?? "Chưa cập nhật";
            string position = SessionContext.Position ?? "Chưa cập nhật";
            string roleName = SessionContext.RoleName ?? "Chưa cập nhật";
            string address = SessionContext.Address ?? "Chưa cập nhật";
            string branch = $"{SessionContext.BranchCode} - {SessionContext.BranchName}" ?? "Chưa cập nhật";
            string bank = $"{SessionContext.BankCode} - {SessionContext.BankName}" ?? "Chưa cập nhật";

            var labels = new string[]
            {
                "Họ tên:", "Tài khoản Oracle:", "Email:", "SĐT:", "Chức vụ:", "Vai trò:",
                "Địa chỉ:", "Chi nhánh:", "Ngân hàng:"
            };

            var values = new string[]
            {
                fullName, userName, email, phone, position, roleName, address, branch, bank
            };

            for (int i = 0; i < labels.Length; i++)
            {
                var lblTitle = new Label
                {
                    Text = labels[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Anchor = AnchorStyles.Left,
                    AutoSize = true
                };
                var lblValue = new Label
                {
                    Text = values[i],
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    Anchor = AnchorStyles.Left,
                    AutoSize = true
                };

                infoTable.Controls.Add(lblTitle, 0, i);
                infoTable.Controls.Add(lblValue, 1, i);
            }

            profileGroup.Controls.Add(infoTable);
            ContentPanel.Controls.Add(profileGroup);
        }

        private void CreateProfileActionPanel()
        {
            int actionPanelY = 70 + 280 + 10;
            var actionPanel = CreateActionPanel(new[]
            {
                ("Cập nhật thông tin", DashboardConstants.Colors.Success, (Action)ShowUpdateProfileForm),
                ("Đổi mật khẩu", DashboardConstants.Colors.Primary, (Action)ShowChangePasswordForm),
            });
            actionPanel.Location = new Point(20, actionPanelY);
            ContentPanel.Controls.Add(actionPanel);
        }


        private void ShowUpdateProfileForm()
        {
            var form = new UpdateProfileForm();
            if (form.ShowDialog() == DialogResult.OK)
                RefreshContent();
        }

        private void ShowChangePasswordForm()
        {
            using (var form = new ChangePasswordForm())
            {
                form.ShowDialog();
            }
        }
        public override void RefreshContent()
        {
            LoadContent();
            ShowMessage("Đã làm mới hồ sơ cá nhân!");
        }
    }
}
