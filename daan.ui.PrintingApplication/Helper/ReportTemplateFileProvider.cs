using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication.Helper
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
            Log.InfoFormat("Updating files for report template version: {0}.", version);
            var reportTemplateFileList = GetServerReportTemplateFiles();

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
                File.WriteAllText(filePath, file.FileContent);
            }

            //update template in memory
            _reportTemplateFileList = GetLocalReportTemplateFiles(version);
        }

        private static List<ReportTemplateFile> GetLocalReportTemplateFiles(string version)
        {
            Log.InfoFormat("Loading local report template files. current version is {0}.", version);
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

        private static IEnumerable<ReportTemplateFile> GetServerReportTemplateFiles()
        {
            Log.Info("Getting report template files from server.");
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

            if (!serverReportTemplateFiles.Any())
            {
                Log.Error("Getting 0 report template files from server.");
                throw new Exception("Getting 0 report template files from server.");
            }

            return serverReportTemplateFiles;
        }


        public static String GetLocalReportTemplateFileByCode(string reportTemplateCode)
        {
            var reportTemplateFile = _reportTemplateFileList.FirstOrDefault(r => r.FileName == reportTemplateCode);
            if (reportTemplateFile == null)
            {
                string message = string.Format("Cannot Get report template file by code: {0}", reportTemplateCode);
                Log.ErrorFormat(message);
                throw new Exception(message);
            }
            else
            {
                return reportTemplateFile.FileContent;
            }
        }
    }
}
