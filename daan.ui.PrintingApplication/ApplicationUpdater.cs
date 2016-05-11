using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;

namespace daan.ui.PrintingApplication
{
    public class ApplicationUpdater
    {
        private ClientApplicationVersion currentApplicationVersion = new ClientApplicationVersion();

        public void Initialize()
        {
            string applicationVersionFilePath = ConfigurationManager.AppSettings.Get("ApplicationVersionFilePath");
            var sr = new StreamReader(applicationVersionFilePath, Encoding.Default);
            var lines = new List<string>();
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            currentApplicationVersion.ApplicationIdentifier = lines.First(l => l.Contains("ApplicationIdentifier")).Substring("ApplicationIdentifier=".Length);
            currentApplicationVersion.ApplicationVersion = lines.First(l => l.Contains("ApplicationVersion")).Substring("ApplicationVersion=".Length);;
            currentApplicationVersion.ReportTemplateVersion = lines.First(l => l.Contains("ReportTemplateVersion")).Substring("ReportTemplateVersion=".Length); ;
        }

        public void CheckUpdates()
        {
            var latestVersion = GetLatestVersionFromServer();
            if (currentApplicationVersion.ApplicationVersion != latestVersion.ApplicationVersion)
            {
                
            }
            else if (currentApplicationVersion.ReportTemplateVersion != latestVersion.ReportTemplateVersion)
            {
                
            }
        }

        public ClientApplicationVersion GetLatestVersionFromServer()
        {
            var userService = ServiceFactory.GetClientApplicationService();
            var request = new GetLastClientAppVersionsRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
            };
            var response = userService.GetLastClientAppVersions(request);
            if (response.ResultType == ResultTypes.Ok && response.ClientApplicationVersions != null)
            {
                return response.ClientApplicationVersions.First(v => v.ApplicationIdentifier == "ApplicationIdentifier");
            }
            else
            {
                return currentApplicationVersion;
            }
        }
    }
}
