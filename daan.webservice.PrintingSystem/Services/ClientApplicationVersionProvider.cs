using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using daan.webservice.PrintingSystem.Contract.Models;
using daan.webservice.PrintingSystem.Helper;
using log4net;

namespace daan.webservice.PrintingSystem.Services
{
    public static class ClientApplicationVersionProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly String ClientAppVersionConfigFile = ConfigurationManager.AppSettings.Get("ClientAppVersionConfigFile");
        private const string CacheKey = "CacheKey_ClientAppVersion";

        /// <summary>
        /// 从内存缓存中读取配置。若缓存中不存在，则重新从文件中读取配置，存入缓存
        /// </summary>
        /// <returns></returns>
        public static List<ClientApplicationVersion> GetClientApplicationVersion()
        {
            List<ClientApplicationVersion> clientApplicationVersions = null;

            ObjectCache cache = MemoryCache.Default;

           if (cache.Contains(CacheKey))
            {
                clientApplicationVersions = cache.GetCacheItem(CacheKey).Value as List<ClientApplicationVersion>;
            }
            else
            {
                clientApplicationVersions = LoadClientApplicationConfigs();
                if (clientApplicationVersions.Any())
                {
                    CacheItemPolicy policy = new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable };
                    cache.Set(CacheKey, clientApplicationVersions, policy);

                    List<string> filePaths = new List<string> { ClientAppVersionConfigFile };
                    HostFileChangeMonitor monitor = new HostFileChangeMonitor(filePaths);
                    monitor.NotifyOnChanged(new OnChangedCallback((o) => cache.Remove(CacheKey)));

                    policy.ChangeMonitors.Add(monitor);
                }
            }

            return clientApplicationVersions;
        }

        private static List<ClientApplicationVersion> LoadClientApplicationConfigs()
        {
            var clientApplicationVersionConfig = XmlHelper.DeserializeFromFile<ClientApplicationVersionConfig>(ClientAppVersionConfigFile, null);
            if (clientApplicationVersionConfig != null && clientApplicationVersionConfig.ClientApplicationVersionList.Any())
            {
                return clientApplicationVersionConfig.ClientApplicationVersionList;
            }
            else
            {
                return new List<ClientApplicationVersion>();
            }
        }
    }
}