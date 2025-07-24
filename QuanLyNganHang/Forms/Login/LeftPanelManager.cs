using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Login
{
    public class LeftPanelManager
    {
        private Panel leftPanel;

        public Panel CreateLeftPanel(Form parentForm)
        {
            leftPanel = new Panel
            {
                Size = new Size(LoginConstants.Sizes.LeftPanelWidth, parentForm.Height),
                Location = new Point(0, 0),
                BackColor = LoginConstants.Colors.Primary
            };

            CreateLogo();
            CreateTitleLabels();
            CreateSecurityPanel();

            return leftPanel;
        }

        private void CreateLogo()
        {
            PictureBox logo = new PictureBox
            {
                Size = new Size(LoginConstants.Sizes.LogoSize, LoginConstants.Sizes.LogoSize),
                Location = new Point(150, 80),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = LoginUIFactory.CreateBankIcon(LoginConstants.Sizes.LogoSize, LoginConstants.Sizes.LogoSize)
            };

            leftPanel.Controls.Add(logo);
        }

        private void CreateTitleLabels()
        {
            Label titleLabel = new Label
            {
                Text = LoginConstants.Texts.BankName,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(350, 40),
                Location = new Point(25, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label subtitleLabel = new Label
            {
                Text = LoginConstants.Texts.SystemTitle,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightBlue,
                AutoSize = false,
                Size = new Size(350, 30),
                Location = new Point(25, 245),
                TextAlign = ContentAlignment.MiddleCenter
            };

            leftPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });
        }

        private void CreateSecurityPanel()
        {
            Panel securityPanel = new Panel
            {
                Location = new Point(25, 350),
                Size = new Size(350, 200),
                BackColor = LoginConstants.Colors.Secondary
            };

            Label securityTitle = new Label
            {
                Text = LoginConstants.Texts.SecurityTitle,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            for (int i = 0; i < LoginConstants.SecurityFeatures.Length; i++)
            {
                Label feature = new Label
                {
                    Text = LoginConstants.SecurityFeatures[i],
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.LightBlue,
                    Location = new Point(20, 55 + (i * 25)),
                    AutoSize = true
                };
                securityPanel.Controls.Add(feature);
            }

            securityPanel.Controls.Add(securityTitle);
            leftPanel.Controls.Add(securityPanel);
        }
    }
}
