using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public class HeaderManager
    {
        private Panel headerPanel;
        private Label userInfoLabel;
        private Label timeLabel;
        private Timer timeTimer;

        public Panel CreateHeader(Form parentForm, string currentUser, string currentRole)
        {
            headerPanel = new Panel
            {
                Size = new Size(parentForm.Width, DashboardConstants.Sizes.HeaderHeight),
                Location = new Point(0, 0),
                BackColor = DashboardConstants.Colors.Primary,
                Dock = DockStyle.Top
            };

            CreateTitleLabel();
            CreateUserInfo(currentUser, currentRole);
            CreateTimeLabel();
            StartTimeUpdater();

            return headerPanel;
        }

        private void CreateTitleLabel()
        {
            Label titleLabel = new Label
            {
                Text = DashboardConstants.Texts.AppTitle,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(400, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            headerPanel.Controls.Add(titleLabel);
        }

        private void CreateUserInfo(string currentUser, string currentRole)
        {
            userInfoLabel = new Label
            {
                Text = $"Xin chào, {currentUser} | {currentRole}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(300, 25),
                Location = new Point(headerPanel.Width - 320, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            headerPanel.Controls.Add(userInfoLabel);
        }

        private void CreateTimeLabel()
        {
            timeLabel = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(200, 20),
                Location = new Point(headerPanel.Width - 220, 10),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            headerPanel.Controls.Add(timeLabel);
        }

        private void StartTimeUpdater()
        {
            timeTimer = new Timer { Interval = 1000 };
            timeTimer.Tick += (s, e) => timeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timeTimer.Start();
        }

        public void UpdateUserInfo(string user, string role)
        {
            if (userInfoLabel != null)
            {
                userInfoLabel.Text = $"👋 Xin chào, {user} | {role}";
            }
        }

        public void Dispose()
        {
            timeTimer?.Stop();
            timeTimer?.Dispose();
        }
    }
}
