﻿using System;
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
        private static readonly String ReportTemplateFilesPath = ConfigurationManager.AppSettings.Get("ReportTemplateFilesPath");
        private const string CacheKey = "CacheKey_ReportTemplateFile";

        /// <summary>
        /// 从内存缓存中读取配置。若缓存中不存在，则重新从文件中读取配置，存入缓存
        /// </summary>
        /// <returns></returns>
        public static List<ReportTemplateFile> GetReportTemplates()
        {
            List<ReportTemplateFile> reportTemplates = null;

            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(CacheKey))
            {
                reportTemplates = cache.GetCacheItem(CacheKey).Value as List<ReportTemplateFile>;
            }
            else
            {
                reportTemplates = LoadReportTemplates();
                if (reportTemplates.Any())
                {
                    CacheItemPolicy policy = new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable };
                    cache.Set(CacheKey, reportTemplates, policy);

                    var fileInfos = new DirectoryInfo(ReportTemplateFilesPath).GetFiles().ToList();
                    List<string> filePaths = fileInfos.Select(f => f.FullName).ToList();
                    HostFileChangeMonitor monitor = new HostFileChangeMonitor(filePaths);
                    monitor.NotifyOnChanged(new OnChangedCallback((o) => cache.Remove(CacheKey)));

                    policy.ChangeMonitors.Add(monitor);
                }
            }

            return reportTemplates;
        }

        public static List<ReportTemplateFile> LoadReportTemplates()
        {
            var templates = new List<ReportTemplateFile>();

            try
            {
                var files = new DirectoryInfo(ReportTemplateFilesPath).GetFiles().ToList();
                templates.AddRange(files.Select(file => new ReportTemplateFile() { FileName = file.Name, FileContent = File.ReadAllText(file.FullName) }));
            }
            catch (Exception ex)
            {
                Log.Error("Error while loading report templates", ex);
            }

            return templates;
        }
    }
}