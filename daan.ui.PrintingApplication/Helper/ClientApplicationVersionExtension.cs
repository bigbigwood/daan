using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication.Helper
{
    class ClientApplicationVersionExtension
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ClientApplicationVersion FromFile(string path)
        {
            var sr = new StreamReader(path, Encoding.Default);
            var lines = new List<string>();
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            var model = new ClientApplicationVersion();
            model.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            model.ApplicationIdentifier = lines.First(l => l.Contains("ApplicationIdentifier")).Substring("ApplicationIdentifier=".Length);
            //model.ApplicationVersion = lines.First(l => l.Contains("ApplicationVersion")).Substring("ApplicationVersion=".Length); ;
            model.ReportTemplateVersion = lines.First(l => l.Contains("ReportTemplateVersion")).Substring("ReportTemplateVersion=".Length);

            return model;
        }

        public static void ToFile(string path, ClientApplicationVersion currentApplicationVersion)
        {
            var lines = new List<string>()
                {
                    //string.Format("ApplicationIdentifier={0}", currentApplicationVersion.ApplicationIdentifier),
                    //string.Format("ApplicationVersion={0}", currentApplicationVersion.ApplicationVersion),
                    string.Format("ReportTemplateVersion={0}", currentApplicationVersion.ReportTemplateVersion),
                };

            File.Delete(path);
            File.WriteAllLines(path, lines);
        }

    }
}
