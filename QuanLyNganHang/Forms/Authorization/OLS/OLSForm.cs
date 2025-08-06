using System;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.OLS
{
    public partial class OLSForm : UserControl
    {
        private UserControl_TaoPolicyGanQuyen ucTaoPolicy;
        private UserControl_EnableDisablePolicy ucEnableDisable;
        private UserControl_LabelComponent ucLabelComponent;

        public OLSForm()
        {
            InitializeComponent();
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            ucTaoPolicy = new UserControl_TaoPolicyGanQuyen() { Dock = DockStyle.Fill };
            ucEnableDisable = new UserControl_EnableDisablePolicy() { Dock = DockStyle.Fill };
            ucLabelComponent = new UserControl_LabelComponent() { Dock = DockStyle.Fill };
            tabPageTaoPolicy.Controls.Add(ucTaoPolicy);
            tabPageEnableDisable.Controls.Add(ucEnableDisable);
            tabPageLabel.Controls.Add(ucLabelComponent);
        }
    }
}
