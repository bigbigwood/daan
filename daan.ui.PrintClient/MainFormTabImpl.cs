using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;

namespace daan.ui.PrinterApplication
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
            var setting = new FromPrinterSetting();
            setting.TopLevel = false;
            setting.Dock = DockStyle.Fill;
            setting.FormBorderStyle = FormBorderStyle.None;
            setting.ControlBox = false;
            tab_Settings.TabPages[0].Controls.Add(setting);
            setting.BackColor = setting.Parent.BackColor;
            setting.Show();
        }
    }
}
