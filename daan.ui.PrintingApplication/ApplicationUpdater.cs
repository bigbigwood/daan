using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.ui.PrintingApplication.Helper;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication
{
    public class ApplicationUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string applicationVersionFilePath = FilePathHelper.BuildPath(ConfigurationManager.AppSettings.Get("ApplicationVersionFilePath"));
        private readonly bool EnableApplicationUpdater = ConfigurationManager.AppSettings.Get("EnableApplicationUpdater").ToUpper() == "TRUE";
        private static readonly Int32 updateCheckIntervalMinutes = int.Parse(ConfigurationManager.AppSettings.Get("ApplicationUpdaterRefreshMinutes"));
        private ClientApplicationVersion currentApplicationVersion = new ClientApplicationVersion();

        public void Initialize()
        {
            currentApplicationVersion = ClientApplicationVersionExtension.FromFile(applicationVersionFilePath);
            PrintingApp.CurrentApplicationVersion = ClientApplicationVersionExtension.BuildApplicationVersionString(currentApplicationVersion);

            if (EnableApplicationUpdater)
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            var updateCheckIntervalMilliSeconds = updateCheckIntervalMinutes * 60 * 1000;
            var t = new System.Timers.Timer(updateCheckIntervalMilliSeconds);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Time_Elapsed);
            t.AutoReset = true;
            t.Enabled = true;
        }

        public void Time_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            CheckUpdates();
        }

        public string ReportTemplateVersion
        {
            get { return currentApplicationVersion.ReportTemplateVersion; }
        }

        public void CheckUpdates()
        {
            var latestVersion = GetLatestVersionFromServer();
            if (latestVersion == null) return;

            Log.InfoFormat("ApplicationIdentifier={0}, ApplicationVersion={1}, ReportTemplateVersion={2}", latestVersion.ApplicationIdentifier, latestVersion.ApplicationVersion, latestVersion.ReportTemplateVersion);
            if (currentApplicationVersion.ApplicationVersion != latestVersion.ApplicationVersion)
            {
                MessageBox.Show("发现新版本，请下载新版本使用。");
            }
            else if (currentApplicationVersion.ReportTemplateVersion != latestVersion.ReportTemplateVersion)
            {
                // update report files
                ReportTemplateFileProvider.UpdateReportTemplateFiles(latestVersion.ReportTemplateVersion);

                // update version file
                currentApplicationVersion.ReportTemplateVersion = latestVersion.ReportTemplateVersion;
                ClientApplicationVersionExtension.ToFile(applicationVersionFilePath, currentApplicationVersion);
            }
        }

        public ClientApplicationVersion GetLatestVersionFromServer()
        {
            Log.Info("Getting last client appliation version from server.");
            ClientApplicationVersion latestVersion = null;
            var userService = ServiceFactory.GetClientApplicationService();
            var request = new GetLastClientAppVersionsRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
            };
            var response = userService.GetLastClientAppVersions(request);
            if (response.ResultType == ResultTypes.Ok && response.ClientApplicationVersions != null)
            {
                latestVersion = response.ClientApplicationVersions.FirstOrDefault(v => v.ApplicationIdentifier == currentApplicationVersion.ApplicationIdentifier);
            }

            return latestVersion;
        }
    }
}
