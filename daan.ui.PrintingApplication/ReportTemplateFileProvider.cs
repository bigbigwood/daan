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
    public class ReportTemplateFileProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string LocalReportTemplateFilePath = ConfigurationManager.AppSettings.Get("LocalReportTemplateFilePath");
        private static List<ReportTemplateFile> _reportTemplateFileList = new List<ReportTemplateFile>();

        public static void Init(string currentVersion)
        {
            _reportTemplateFileList = GetLocalReportTemplateFiles(currentVersion);
        }

        public static void UpdateReportTemplateFiles(string version)
        {
            var reportTemplateFileList = GetServerReportTemplateFiles();

            //update template in memory
            _reportTemplateFileList = reportTemplateFileList;

            //store file in local machine
            string directoryPath = string.Format(@"{0}\{1}", LocalReportTemplateFilePath, version);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var file in reportTemplateFileList)
            {
                string filePath = string.Format(@"{0}\{1}", directoryPath, file.FileName);
                if (File.Exists(filePath))
                {
                    Log.WarnFormat("Report template file: {0} already exist, the file will be recovery.", filePath);
                    File.Delete(filePath);
                }
                File.Create(filePath);
                File.WriteAllText(filePath, file.FileContent);
            }
        }

        private static List<ReportTemplateFile> GetLocalReportTemplateFiles(string version)
        {
            var temp = new List<ReportTemplateFile>();

            try
            {
                string directoryPath = string.Format(@"{0}\{1}", LocalReportTemplateFilePath, version);
                var files = new DirectoryInfo(directoryPath).GetFiles().ToList();
                temp.AddRange(files.Select(file => new ReportTemplateFile() { FileName = file.Name.Replace(file.Extension, ""), FileContent = File.ReadAllText(file.FullName) }));
            }
            catch (Exception ex)
            {
                Log.Error("Error while loading report templates", ex);
            }

            return temp;
        }

        private static List<ReportTemplateFile> GetServerReportTemplateFiles()
        {
            var serverReportTemplateFiles = new List<ReportTemplateFile>();

            var request = new GetReportTemplatesRequest()
            {
                Username = PrintingApp.UserCredential.UserName,
                Password = PrintingApp.UserCredential.Password,
            };

            var response = ServiceFactory.GetClientApplicationService().GetReportTemplates(request);
            if (response.ResultType == ResultTypes.Ok)
            {
                serverReportTemplateFiles = response.ReportTemplateFiles.ToList();
            }

            return serverReportTemplateFiles;
        }


        public static String GetLocalReportTemplateFileByCode(string reportTemplateCode)
        {
            return _reportTemplateFileList.First(r => r.FileName == reportTemplateCode).FileContent;
        }
    }
}
