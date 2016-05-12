using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication
{
    public class ApplicationUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string applicationVersionFilePath = ConfigurationManager.AppSettings.Get("ApplicationVersionFilePath");
        private ClientApplicationVersion currentApplicationVersion = new ClientApplicationVersion();

        public void Initialize()
        {
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
            PrintingApp.CurrentApplicationVersion = BuildApplicationVersionString();

            System.Timers.Timer t = new System.Timers.Timer(5000);   //实例化Timer类，设置间隔时间为10000毫秒；   
            t.Elapsed += new System.Timers.ElapsedEventHandler(Time_Elapsed); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 
        }

        public void Time_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            CheckUpdates();
        }  

        public string ReportTemplateVersion
        {
            get { return currentApplicationVersion.ReportTemplateVersion; }
        }

        public string BuildApplicationVersionString()
        {
            return string.Format("{0} v{1}", currentApplicationVersion.ApplicationIdentifier, currentApplicationVersion.ApplicationVersion);
        }



        public void CheckUpdates()
        {
            var latestVersion = GetLatestVersionFromServer();
            Log.InfoFormat("ApplicationIdentifier={0}, ApplicationVersion={1}, ReportTemplateVersion={2}", latestVersion.ApplicationIdentifier, latestVersion.ApplicationVersion, latestVersion.ReportTemplateVersion);
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
                return response.ClientApplicationVersions.First(v => v.ApplicationIdentifier == currentApplicationVersion.ApplicationIdentifier);
            }
            else
            {
                return currentApplicationVersion;
            }
        }
    }
}
