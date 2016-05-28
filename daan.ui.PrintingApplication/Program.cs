using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using daan.ui.PrintingApplication.Helper;
using log4net;

namespace daan.ui.PrintingApplication
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");

            PrintingApp.Initialize();
            ReportTemplateFileProvider.Initialize(PrintingApp.GetVersionManager().ReportTemplateVersion);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
