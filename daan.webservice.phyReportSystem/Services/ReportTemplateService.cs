using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using daan.webservice.PrintingSystem.Contract.Models;
using System.Configuration;
using System.IO;
using log4net;

namespace daan.webservice.PrintingSystem.Services
{
    public class ReportTemplateService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly String ReportTemplatePath = ConfigurationManager.AppSettings.Get("ReportTemplatePath");
        private const string CacheKey = "CacheKey_ReportTemplate";

        //public List<ReportTemplate> GetReportTemplates()
        //{
        //    if (_reportTemplates == null)
        //        LoadReportTemplates();

        //    return _reportTemplates;
        //}

        /// <summary>
        /// 从内存缓存中读取配置。若缓存中不存在，则重新从文件中读取配置，存入缓存
        /// </summary>
        /// <returns></returns>
        public static List<ReportTemplate> GetReportTemplates()
        {
            List<ReportTemplate> reportTemplates = null;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(CacheKey))
            {
                reportTemplates = cache.GetCacheItem(CacheKey).Value as List<ReportTemplate>;
            }
            else
            {
                reportTemplates = LoadReportTemplates();
                if (reportTemplates.Any())
                {
                    CacheItemPolicy policy = new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable };
                    cache.Set(CacheKey, reportTemplates, policy);

                    var fileInfos = new DirectoryInfo(ReportTemplatePath).GetFiles().ToList();
                    List<string> filePaths = fileInfos.Select(f => f.FullName).ToList();
                    HostFileChangeMonitor monitor = new HostFileChangeMonitor(filePaths);
                    monitor.NotifyOnChanged(new OnChangedCallback((o) => cache.Remove(CacheKey)));

                    policy.ChangeMonitors.Add(monitor);
                }
            }

            return reportTemplates;
        }

        public static List<ReportTemplate> LoadReportTemplates()
        {
            var templates = new List<ReportTemplate>();

            try
            {
                var files = new DirectoryInfo(ReportTemplatePath).GetFiles().ToList();
                templates.AddRange(files.Select(file => new ReportTemplate() { Name = file.Name, Content = File.ReadAllText(file.FullName) }));
            }
            catch (Exception ex)
            {
                Log.Error("Error while loading report templates", ex);
            }

            return templates;
        }
    }
}