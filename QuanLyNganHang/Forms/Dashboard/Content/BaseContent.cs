using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Dashboard.Content
{
    public abstract class BaseContent
    {
        protected Panel ContentPanel { get; private set; }

        public BaseContent(Panel contentPanel)
        {
            ContentPanel = contentPanel;
        }

        public abstract void LoadContent();
        public abstract void RefreshContent();

        protected void ClearContent()
        {
            ContentPanel.Controls.Clear();
        }

        protected Panel CreateStatsPanel((string title, string value, Color color)[] stats)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(ContentPanel.Width - 40, DashboardConstants.Sizes.StatCardHeight),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - (stats.Length - 1) * 15) / stats.Length;

            for (int i = 0; i < stats.Length; i++)
            {
                Panel statCard = DashboardUIFactory.CreateStatCard(stats[i].title, stats[i].value, stats[i].color, width);
                statCard.Location = new Point(i * (width + 15), 0);
                panel.Controls.Add(statCard);
            }

            return panel;
        }

        protected Panel CreateActionPanel((string text, Color color, Action action)[] actions)
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 200),
                Size = new Size(ContentPanel.Width - 40, DashboardConstants.Sizes.ActionPanelHeight),
                BackColor = Color.Transparent
            };

            int width = (panel.Width - (actions.Length - 1) * 15) / actions.Length;

            for (int i = 0; i < actions.Length; i++)
            {
                Button btn = DashboardUIFactory.CreateActionButton(actions[i].text, actions[i].color, actions[i].action, width);
                btn.Location = new Point(i * (width + 15), 10);
                btn.Click += ActionButton_Click;
                panel.Controls.Add(btn);
            }

            return panel;
        }

        protected DataGridView CreateDataGrid(string[] columnNames, string[] columnHeaders)
        {
            DataGridView dgv = DashboardUIFactory.CreateDataGrid();
            dgv.Location = new Point(20, 300);
            dgv.Size = new Size(ContentPanel.Width - 40, ContentPanel.Height - 320);

            for (int i = 0; i < columnNames.Length; i++)
            {
                dgv.Columns.Add(columnNames[i], columnHeaders[i]);
            }

            return dgv;
        }

        protected void ShowNoDataMessage(DataGridView dgv, string message)
        {
            // Create empty DataTable with first column
            var emptyTable = new System.Data.DataTable();
            if (dgv.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    emptyTable.Columns.Add(col.Name, typeof(string));
                }

                var row = emptyTable.NewRow();
                if (emptyTable.Columns.Count > 1)
                    row[1] = message; // Show message in second column
                else
                    row[0] = message; // Show message in first column
                emptyTable.Rows.Add(row);
            }

            dgv.DataSource = emptyTable;
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.Tag is Action action)
                {
                    action.Invoke();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi thực hiện thao tác: {ex.Message}");
            }
        }

        // Utility methods
        protected void ShowMessage(string message)
        {
            MessageBox.Show($"🔧 {message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected bool ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
