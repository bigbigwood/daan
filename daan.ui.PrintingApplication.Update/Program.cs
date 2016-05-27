using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;

namespace daan.ui.PrintingApplication.Update
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");

            string fileName = "";
            string url = "";

            if (args.Length == 4)
            {
                fileName = args[1].Trim();
                url = args[3].Trim();
            }
            else
            {
                throw new Exception("Please input file name and url.");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new AutoUpdateForm();
            form.FillDownloadFileInfo(fileName, url);

            Application.Run(form);
        }
    }
}
