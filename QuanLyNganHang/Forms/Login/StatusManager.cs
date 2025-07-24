using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Login
{
    public class StatusManager
    {
        private Label lbl_Status;
        private ProgressBar progressBar;

        public StatusManager(Label statusLabel, ProgressBar progressBar)
        {
            this.lbl_Status = statusLabel;
            this.progressBar = progressBar;
        }

        public void ShowLoading(bool show)
        {
            progressBar.Visible = show;
            if (show)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                lbl_Status.Text = "🔄 Đang xác thực...";
                lbl_Status.ForeColor = LoginConstants.Colors.Info;
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }
        }

        public void ShowError(string message)
        {
            lbl_Status.Text = message;
            lbl_Status.ForeColor = LoginConstants.Colors.Error;

            // Flash effect
            Timer flashTimer = new Timer { Interval = 200 };
            int flashCount = 0;
            flashTimer.Tick += (s, e) =>
            {
                lbl_Status.Visible = !lbl_Status.Visible;
                flashCount++;
                if (flashCount >= 6)
                {
                    flashTimer.Stop();
                    lbl_Status.Visible = true;
                }
            };
            flashTimer.Start();
        }

        public void ShowSuccess(string message)
        {
            lbl_Status.Text = message;
            lbl_Status.ForeColor = LoginConstants.Colors.Success;
        }

        public void ShowReady()
        {
            lbl_Status.Text = "Sẵn sàng đăng nhập...";
            lbl_Status.ForeColor = LoginConstants.Colors.Success;
        }
    }
}
