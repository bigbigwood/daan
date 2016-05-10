using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Models;
using log4net;

namespace daan.ui.PrintingApplication
{
    public class ReportTemplateProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string LocalReportTemplatePath = ConfigurationManager.AppSettings.Get("LocalReportTemplatePath");
        private static List<ReportTemplateInfo> _reportTemplateList = new List<ReportTemplateInfo>();

        public static void Init()
        {
            GetLocalReportTemplates();
        }

        //public static String GetLocalReportTemplateCodeById(string reportTemplateId)
        //{
        //    return _reportTemplateList.First(r => r.Id == reportTemplateId).Name;
        //}

        //public static String GetLocalReportTemplateContentById(string reportTemplateId)
        //{
        //    return _reportTemplateList.First(r => r.Id == reportTemplateId).Content;
        //}

        public static String GetLocalReportTemplateContentByCode(string reportTemplateCode)
        {
            return _reportTemplateList.First(r => r.Name == reportTemplateCode).Content;
        }

        public static void GetLocalReportTemplates()
        {
            List<ReportTemplateInfo> temp = new List<ReportTemplateInfo>();

            try
            {
                var files = new DirectoryInfo(LocalReportTemplatePath).GetFiles().ToList();
                temp.AddRange(files.Select(file => new ReportTemplateInfo() { Name = file.Name.Replace(file.Extension, ""), Content = File.ReadAllText(file.FullName) }));
            }
            catch (Exception ex)
            {
                Log.Error("Error while loading report templates", ex);
            }

            _reportTemplateList = temp;
        }

        public static void GetServerReportTemplates()
        {
            
        }

        public static void UpdateReportTemplates(List<ReportTemplateInfo> reportTemplateList)
        {
            _reportTemplateList = reportTemplateList;
        }
    }
}
