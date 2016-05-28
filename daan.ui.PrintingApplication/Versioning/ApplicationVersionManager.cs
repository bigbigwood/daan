using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using daan.ui.PrintingApplication.Helper;

namespace daan.ui.PrintingApplication.Versioning
{
    public class ApplicationVersionManager
    {
        private static readonly string LocalReportTemplateVersionFilePath = FilePathHelper.BuildPath(ConfigurationManager.AppSettings.Get("LocalReportTemplateVersionFilePath"));
        public String ApplicationVersion { get; private set; }
        public String ReportTemplateVersion { get; private set; }

        public static ApplicationVersionManager Create()
        {
            return new ApplicationVersionManager()
            {
                ReportTemplateVersion = GetLocalReportTemplateVersion(LocalReportTemplateVersionFilePath),
                ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
            };
        }

        public void UpdateReportTemplateVersion(string version)
        {
            //update report template files and reload.
            ReportTemplateFileProvider.UpdateReportTemplateFiles(version);

            //upgrade template version
            ReportTemplateVersion = version;
            SetLocalReportTemplateVersion(LocalReportTemplateVersionFilePath, version);
        }

        private static string GetLocalReportTemplateVersion (string path)
        {
            var sr = new StreamReader(path, Encoding.Default);
            var lines = new List<string>();
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines.First(l => l.Contains("ReportTemplateVersion")).Substring("ReportTemplateVersion=".Length);
        }

        private static void SetLocalReportTemplateVersion(string path, string version)
        {
            var lines = new List<string>()
                {
                    string.Format("ReportTemplateVersion={0}", version),
                };

            File.Delete(path);
            File.WriteAllLines(path, lines);
        }
    }
}
