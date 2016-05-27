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
    public enum ApplicationUpdateEventType
    {
        NothingChanged,
        ApplicationVersionChanged,
        ReportTemplateVersionChanged,
    }

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
            t.Start();
        }

        public void Time_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            var applicationUpdateEventType = CheckUpdates();

            switch (applicationUpdateEventType)
            {
                case ApplicationUpdateEventType.ApplicationVersionChanged:
                    MessageBox.Show("发现新版本，请下载新版本使用。");
                    break;

                case ApplicationUpdateEventType.ReportTemplateVersionChanged:
                    break;

                case ApplicationUpdateEventType.NothingChanged:
                    break;
                default:
                    break;
            }
        }

        public string ReportTemplateVersion
        {
            get { return currentApplicationVersion.ReportTemplateVersion; }
        }

        public string LatestVersionDownloadUrl
        {
            get; private set;
        }

        public ApplicationUpdateEventType CheckUpdates()
        {
            var latestVersion = GetLatestVersionFromServer();
            if (latestVersion == null) return ApplicationUpdateEventType.NothingChanged;

            Log.InfoFormat("ApplicationIdentifier={0}, ApplicationVersion={1}, DownloadUrl={2}, ReportTemplateVersion={3}", latestVersion.ApplicationIdentifier, latestVersion.ApplicationVersion, latestVersion.DownloadUrl, latestVersion.ReportTemplateVersion);
            if (currentApplicationVersion.ApplicationVersion != latestVersion.ApplicationVersion)
            {
                LatestVersionDownloadUrl = latestVersion.DownloadUrl;
                return ApplicationUpdateEventType.ApplicationVersionChanged;
            }
            else if (currentApplicationVersion.ReportTemplateVersion != latestVersion.ReportTemplateVersion)
            {
                // update report files
                ReportTemplateFileProvider.UpdateReportTemplateFiles(latestVersion.ReportTemplateVersion);

                // update version file
                currentApplicationVersion.ReportTemplateVersion = latestVersion.ReportTemplateVersion;
                ClientApplicationVersionExtension.ToFile(applicationVersionFilePath, currentApplicationVersion);
                return ApplicationUpdateEventType.ReportTemplateVersionChanged;
            }

            return ApplicationUpdateEventType.NothingChanged;
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
