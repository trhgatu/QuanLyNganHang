using QuanLyNganHang.DataAccess;
using QuanLyNganHang.Forms.Dashboard.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard
{
    public class ContentManager
    {
        private Panel contentPanel;
        private BaseContent currentContent;

        public Panel CreateContentPanel(Form parentForm)
        {
            contentPanel = new Panel
            {
                Location = new Point(DashboardConstants.Sizes.MenuPanelWidth, DashboardConstants.Sizes.HeaderHeight),
                Size = new Size(parentForm.Width - DashboardConstants.Sizes.MenuPanelWidth,
                               parentForm.Height - DashboardConstants.Sizes.HeaderHeight - DashboardConstants.Sizes.FooterHeight),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Padding = new Padding(20),
                AutoScroll = true
            };

            return contentPanel;
        }

        public void LoadContent(string contentType)
        {
            try
            {
                // Create appropriate content based on type
                currentContent = ContentFactory.CreateContent(contentType, contentPanel);

                // Load the content
                currentContent.LoadContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải nội dung: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshCurrentContent()
        {
            currentContent?.RefreshContent();
        }
    }
}
