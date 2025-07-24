using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyNganHang.Forms.Login
{
    public class ConnectionManager
    {
        private Panel connectionPanel;
        private TextBox txt_Host, txt_Port, txt_Sid;
        private bool isConnectionVisible = false;
        private Button btn_ShowConnection;

        public bool IsVisible => isConnectionVisible;

        public Panel CreateConnectionPanel(Panel parentPanel, Button showConnectionButton)
        {
            btn_ShowConnection = showConnectionButton;

            connectionPanel = new Panel
            {
                Size = new Size(LoginConstants.Sizes.ConnectionPanelWidth, LoginConstants.Sizes.ConnectionPanelHeight),
                Location = new Point(75, 180),
                BackColor = LoginConstants.Colors.Light,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                Padding = new Padding(20)
            };

            CreateConnectionControls();
            parentPanel.Controls.Add(connectionPanel);

            return connectionPanel;
        }

        private void CreateConnectionControls()
        {
            Label connTitle = new Label
            {
                Text = LoginConstants.Texts.ConnectionTitle,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = LoginConstants.Colors.Primary,
                Location = new Point(20, 15),
                AutoSize = true
            };

            // Host
            Label lblHost = LoginUIFactory.CreateLabel("🖥️ Host:", new Point(20, 55));
            txt_Host = LoginUIFactory.CreateTextBox(new Point(20, 80));
            txt_Host.Text = "localhost";

            // Port
            Label lblPort = LoginUIFactory.CreateLabel("🔌 Port:", new Point(240, 55));
            txt_Port = LoginUIFactory.CreateTextBox(new Point(240, 80));
            txt_Port.Size = new Size(150, 30);
            txt_Port.Text = "1521";

            // SID
            Label lblSid = LoginUIFactory.CreateLabel("🗄️ SID:", new Point(20, 120));
            txt_Sid = LoginUIFactory.CreateTextBox(new Point(20, 145));
            txt_Sid.Text = "XE";

            // Buttons
            Button btn_TestConnection = LoginUIFactory.CreateButton("🔍 Kiểm tra kết nối",
                new Point(20, 190), new Size(180, 35), LoginConstants.Colors.Purple,
                btn_TestConnection_Click);

            Button btn_SaveSettings = LoginUIFactory.CreateButton("💾 Lưu cài đặt",
                new Point(220, 190), new Size(150, 35), LoginConstants.Colors.Success,
                btn_SaveSettings_Click);

            Button btn_CloseConnection = LoginUIFactory.CreateButton("✖",
                new Point(400, 15), new Size(30, 30), LoginConstants.Colors.Error,
                (s, e) => Toggle());

            connectionPanel.Controls.AddRange(new Control[] {
                connTitle, lblHost, txt_Host, lblPort, txt_Port, lblSid, txt_Sid,
                btn_TestConnection, btn_SaveSettings, btn_CloseConnection
            });
        }

        public void Toggle()
        {
            isConnectionVisible = !isConnectionVisible;
            connectionPanel.Visible = isConnectionVisible;

            if (isConnectionVisible)
            {
                connectionPanel.BringToFront();
                btn_ShowConnection.Text = "🔙 Quay lại đăng nhập";
            }
            else
            {
                btn_ShowConnection.Text = "⚙️ Cài đặt kết nối";
            }
        }

        private void btn_TestConnection_Click(object sender, EventArgs e)
        {
            if (!ValidateConnectionInputs()) return;

            try
            {
                string testConnectionString = $"Data Source={txt_Host.Text}:{txt_Port.Text}/{txt_Sid.Text};User Id=sys;Password=temp;DBA Privilege=SYSDBA;";
                using (OracleConnection conn = new OracleConnection(testConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Kết nối thành công!\nServer Version: " + conn.ServerVersion,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Kết nối thất bại!\n{ex.Message}", "Lỗi kết nối",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_SaveSettings_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            MessageBox.Show("💾 Đã lưu cài đặt kết nối!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateConnectionInputs()
        {
            if (string.IsNullOrEmpty(txt_Host.Text) ||
                string.IsNullOrEmpty(txt_Port.Text) ||
                string.IsNullOrEmpty(txt_Sid.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin kết nối!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public void SaveConnectionSettings()
        {
            try
            {
                var settings = new[]
                {
                    $"Host={txt_Host.Text}",
                    $"Port={txt_Port.Text}",
                    $"SID={txt_Sid.Text}"
                };
                System.IO.File.WriteAllLines(LoginConstants.ConnectionSettingsFile, settings);
            }
            catch { /* Handle silently */ }
        }

        public void LoadSavedSettings()
        {
            try
            {
                if (System.IO.File.Exists(LoginConstants.ConnectionSettingsFile))
                {
                    var lines = System.IO.File.ReadAllLines(LoginConstants.ConnectionSettingsFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            switch (parts[0])
                            {
                                case "Host": txt_Host.Text = parts[1]; break;
                                case "Port": txt_Port.Text = parts[1]; break;
                                case "SID": txt_Sid.Text = parts[1]; break;
                            }
                        }
                    }
                }
            }
            catch { /* Handle silently */ }
        }

        public string GetHost() => txt_Host.Text;
        public string GetPort() => txt_Port.Text;
        public string GetSid() => txt_Sid.Text;
    }
}
