using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using daan.ui.PrintingApplication.Helper;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication.Versioning
{
    public class ApplicationUpdateEventInfo
    {
        public ApplicationUpdateEventType Type;
        public ClientApplicationVersion LatestVersion { get; set; }
    }

    public enum ApplicationUpdateEventType
    {
        NothingChanged,
        ApplicationVersionChanged,
        ReportTemplateVersionChanged,
    }

    public class ApplicationUpdateChecker
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly bool EnableApplicationUpdater = ConfigurationManager.AppSettings.Get("EnableApplicationUpdater").ToUpper() == "TRUE";
        private static readonly Int32 UpdateCheckIntervalMinutes = int.Parse(ConfigurationManager.AppSettings.Get("ApplicationUpdaterRefreshMinutes"));

        public void Initialize()
        {
            if (EnableApplicationUpdater)
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            var updateCheckIntervalMilliSeconds = UpdateCheckIntervalMinutes * 60 * 1000;
            var t = new System.Timers.Timer(updateCheckIntervalMilliSeconds);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Time_Elapsed);
            t.AutoReset = true;
            t.Enabled = true;
            t.Start();
        }

        public void Time_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            var applicationUpdateEventInfo = CheckUpdates();

            switch (applicationUpdateEventInfo.Type)
            {
                case ApplicationUpdateEventType.ApplicationVersionChanged:
                    MessageBox.Show(ConstString.DetectNewVersionNotification);
                    break;

                case ApplicationUpdateEventType.ReportTemplateVersionChanged:
                {
                    var applicationVersionManager = PrintingApp.GetVersionManager();
                    applicationVersionManager.UpdateReportTemplateVersion(applicationUpdateEventInfo.LatestVersion.ReportTemplateVersion);
                }
                break;

                case ApplicationUpdateEventType.NothingChanged:
                    break;
                default:
                    break;
            }
        }


        public ApplicationUpdateEventInfo CheckUpdates()
        {
            var nothingChangedEvent = new ApplicationUpdateEventInfo() { Type = ApplicationUpdateEventType.NothingChanged };

            var latestVersion = GetLatestVersionFromServer();
            if (latestVersion == null) return nothingChangedEvent;

            var applicationVersionManager = PrintingApp.GetVersionManager();
            if (applicationVersionManager.ApplicationVersion != latestVersion.ApplicationVersion)
            {
                return new ApplicationUpdateEventInfo()
                {
                    Type = ApplicationUpdateEventType.ApplicationVersionChanged,
                    LatestVersion = latestVersion,
                };
            }
            else if (applicationVersionManager.ReportTemplateVersion != latestVersion.ReportTemplateVersion)
            {
                return new ApplicationUpdateEventInfo()
                {
                    Type = ApplicationUpdateEventType.ReportTemplateVersionChanged,
                    LatestVersion = latestVersion,
                };
            }

            return nothingChangedEvent;
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
                latestVersion = response.ClientApplicationVersions.FirstOrDefault(v => v.ApplicationIdentifier == PrintingApp.ApplicationIdentifier);
            }

            return latestVersion;
        }
    }
}
