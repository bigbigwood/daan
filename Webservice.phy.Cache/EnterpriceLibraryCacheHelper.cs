using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Webservice.phy.Cache
{
    /// <summary>
    /// 缓存使用方法封装类，封装访问缓存方法，可以根据key 来分类访问缓存
    /// 缓存形式是 systemid + cachekey + objectid 形式保存缓存key 
    /// </summary>
    public class EnterpriceLibraryCacheHelper
    {
        private static readonly Dictionary<string, ICache> Caches = new Dictionary<string, ICache>();

        private static ICacheManager _cacheManager;
        private EnterpriceLibraryCacheHelper()
        {

        }
        /// <summary>
        /// 注册缓存key 
        /// </summary>
        /// <param name="key"></param>
        public static void Registration(string key)
        {
            if (Caches.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format("已经存在缓存名称为“{0}”。", key));

            }

            if (_cacheManager == null)
            {

                _cacheManager = CacheFactory.GetCacheManager("CacheManager");

            }
            Caches.Add(key, new CacheleImplement(key, _cacheManager));
        }

        /// <summary>
        /// key是否已经注册
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsRegistration(string key)
        {
            return Caches.ContainsKey(key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void UnRegistration(string key)
        {
            if (IsRegistration(key))
            {
                var cache = GetCache(key);
                cache.Clear();
                Caches.Remove(key);
            }
        }

        /// <summary>
        /// 获取所有缓存对象
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ICache> GetAllCache()
        {
            return Caches;
        }

        /// <summary>
        /// 通过key值返回缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ICache GetCache(string key)
        {
            if (!IsRegistration(key))
            {
                throw new Exception(string.Format("不存在关键字“{0}”的缓存。", key));
            }
            return Caches[key];
        }
    }
}
