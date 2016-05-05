using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using System.Net.NetworkInformation;
using System.Drawing.Printing;

namespace daan.ui.PrinterApplication
{
    public partial class FromPrinterSetting : Form
    {
        public FromPrinterSetting()
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

            PrinterInterface printerInterface  = new PrinterInterface();
            printerInterface.PringFile(@"C:\Users\wood\Desktop\porting message 101 reply issue.sql", false);
        }

        private void BindData()
        {
            mac.Text= GetMac();
            hostname.Text = GetHostname();
            var printers = GetPrinter();
            cb_A4Printer.DataSource = new List<String>(printers);
            cb_A5Printer.DataSource = new List<String>(printers);
            cb_BarcodePrinter.DataSource = new List<String>(printers);
            cb_PDFPrinter.DataSource = new List<String>(printers);

            //bind user printer config.
            var userInfo = PrinterApp.CurrentUserInfo;
            cb_A4Printer.SelectedItem = userInfo.UserPrinterConfig.A4Printer;
            cb_A5Printer.SelectedItem = userInfo.UserPrinterConfig.A5Printer;
            cb_BarcodePrinter.SelectedItem = userInfo.UserPrinterConfig.BarcodePrinter;
            cb_PDFPrinter.SelectedItem = userInfo.UserPrinterConfig.PdfPrinter;
        }

        private string GetHostname()
        {
            return Environment.MachineName;
        }

        ///<summary>       
        /// 获取本机MAC地址        
        /// </summary>       
        /// <returns>返回当前机器上的所有MAC地址</returns>        
        private string GetMac()
        {
            string macAddress = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macAddress = adapter.GetPhysicalAddress().ToString();
                        for (int i = 1; i < 6; i++)
                        {
                            macAddress = macAddress.Insert(3 * i - 1, ":");
                        }
                        break;
                    }
                }

            }
            catch
            {
            }
            return macAddress.Replace(":", "-");
        }

        //取得打印机名称，用逗号隔开
        public List<String> GetPrinter()
        {
            List<String> printers = new List<string>();
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer.ToString());
            }
            return printers;
        }
    }
}
