using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace daan.ui.PrintingApplication
{
    public partial class PrinterSettingForm : Form
    {
        public PrinterSettingForm()
        {
            InitializeComponent();
        }

        private void FromPrinterSetting_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //var userService = ServiceFactory.GetUserService(ConfigurationManager.AppSettings.Get("UserServiceUrl"));
            //var request = new UpdateUserPrinterConfigRequest() { };
            //var response = userService.UpdateUserPrinterConfig(request);

            //PrinterInterface printerInterface  = new PrinterInterface();
            //printerInterface.PringFile(@"C:\Users\wood\Desktop\porting message 101 reply issue.sql", false);
        }

        private void BindData()
        {
            mac.Text = LocalMachineInfomationProvider.GetMac();
            hostname.Text = LocalMachineInfomationProvider.GetHostname();
            var printers = LocalMachineInfomationProvider.GetPrinter();
            cb_A4Printer.DataSource = new List<String>(printers);
            cb_A5Printer.DataSource = new List<String>(printers);
            cb_BarcodePrinter.DataSource = new List<String>(printers);
            cb_PDFPrinter.DataSource = new List<String>(printers);

            //bind user printer config.
            var userInfo = PrintingApp.CurrentUserInfo;
            cb_A4Printer.SelectedItem = userInfo.UserPrinterConfig.A4Printer;
            cb_A5Printer.SelectedItem = userInfo.UserPrinterConfig.A5Printer;
            cb_BarcodePrinter.SelectedItem = userInfo.UserPrinterConfig.BarcodePrinter;
            cb_PDFPrinter.SelectedItem = userInfo.UserPrinterConfig.PdfPrinter;
        }

    }
}
