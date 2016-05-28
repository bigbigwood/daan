using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using daan.ui.PrintingApplication.Helper;

namespace daan.ui.PrintingApplication.Versioning
{
    public class ApplicationVersionManager
    {
        
        public String ApplicationVersion { get; private set; }
        public String ReportTemplateVersion { get; private set; }

        public static ApplicationVersionManager Create()
        {
            return new ApplicationVersionManager()
            {
                ReportTemplateVersion = ConfigurationManager.AppSettings.Get("ReportTemplateVersion"),
                ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
            };
        }

        public void UpdateReportTemplateVersion(string version)
        {
            //update report template files and reload.
            ReportTemplateFileProvider.UpdateReportTemplateFiles(version);

            //upgrade template version
            ReportTemplateVersion = version;
            ConfigurationManager.AppSettings.Set("ReportTemplateVersion", version);
        }
    }
}
