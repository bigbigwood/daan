using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.NetworkInformation;

namespace daan.ui.PrintingApplication.Helper
{
    public static class LocalMachineInfomationProvider
    {

        public static string GetHostname()
        {
            return Environment.MachineName;
        }

        ///<summary>       
        /// 获取本机MAC地址        
        /// </summary>       
        /// <returns>返回当前机器上的所有MAC地址</returns>        
        public static string GetMac()
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
        public static List<String> GetPrinter()
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
