using System;
using System.Windows.Forms;
using CCWin;

namespace daan.ui.PrintingApplication
{
    public partial class MainFormTabImpl : CCSkinMain
    {
        public MainFormTabImpl()
        {
            InitializeComponent();
        }

        private void tab_Settings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FromPrinterSetting setting = new FromPrinterSetting();
            //setting.TopLevel = false;
            //setting.Dock = DockStyle.Fill;
            ////setting.WindowState = 
            //setting.Parent = tab_Settings.SelectedTab;
            //setting.Show();
        }

        private void MainFormTabImpl_Load(object sender, EventArgs e)
        {
        }
    }
}
