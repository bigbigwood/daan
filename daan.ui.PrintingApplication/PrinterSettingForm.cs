using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CCWin;
using daan.ui.PrintingApplication.Helper;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.ui.PrintingApplication
{
    public partial class PrinterSettingForm : CCSkinMain
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
            var clientApplicationService = ServiceFactory.GetClientApplicationService();
            var request = new UpdateUserInfoRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
                UserInfo = new UserInfo()
                {
                    UserComputerConfig = new UserComputerConfig()
                    {
                        HostMac = mac.Text,
                        HostName = hostname.Text
                    },
                    UserPrinterConfig = new UserPrinterConfig()
                    {
                        A4Printer = cb_A4Printer.SelectedItem.ToString(),
                        A5Printer = cb_A5Printer.SelectedItem.ToString(),
                        BarcodePrinter = cb_BarcodePrinter.SelectedItem.ToString(),
                        PdfPrinter = cb_PDFPrinter.SelectedItem.ToString(),
                    }
                },
            };
            var response = clientApplicationService.UpdateUserInfo(request);
            if (response.ResultType == ResultTypes.Ok)
            {
                PrintingApp.CurrentUserInfo.UserComputerConfig = request.UserInfo.UserComputerConfig;
                PrintingApp.CurrentUserInfo.UserPrinterConfig = request.UserInfo.UserPrinterConfig;
                MessageBox.Show("保存打印机配置成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("保存打印机配置异常！");
            }
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
