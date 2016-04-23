using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace Webservice.phy.Cache
{
    /// <summary>
    /// 缓存控制实现类，将 Icachemanager 重新封装，更适用于按系统分类的缓存使用
    /// </summary>
    internal class CacheleImplement:ICache
    {
        private readonly ICacheManager _cacheManager;
        private readonly string _cachekey;
        private readonly List<string> keys;
        public CacheleImplement(string cachekey, ICacheManager cachemanager)
        {
            _cachekey = cachekey;
            _cacheManager = cachemanager;
            keys = new List<string>();
        }

        #region Implementation of ICache

        /// <summary>
        /// 增加缓存，使用缓存对象cachedata
        /// </summary>
        /// <param name="data"></param>
        public void Add(CacheData data)
        {
            var keyvalue = data.Key;
            keys.Add(keyvalue);
            _cacheManager.Add(keyvalue, data);


        }

        /// <summary>
        /// 增加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Add(string key, object data)
        {
            var cachedata = new CacheData(_cachekey + key, DateTime.Now, data);
            Add(cachedata);
        }

        /// <summary>
        /// 加入缓存并且含超时时间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeoutminus"></param>
        public void Add(CacheData data, int timeoutminus)
        {
            var keyvalue = data.Key;
            keys.Add(keyvalue);
            _cacheManager.Add(keyvalue, data, CacheItemPriority.Normal, null,
                              new SlidingTime(TimeSpan.FromMinutes(timeoutminus)));
        }

        /// <summary>
        /// 加入缓存并且含超时时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeoutminus"></param>
        public void Add(string key, object data, int timeoutminus)
        {
            var keyvalue = _cachekey + key;
            var cachedata = new CacheData(keyvalue, DateTime.Now, data);
            keys.Add(keyvalue);
            Add(cachedata, timeoutminus);
        }

        /// <summary>
        /// 通过关键字移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            var keyvalue = _cachekey + key;
            if (_cacheManager.Contains(keyvalue)) _cacheManager.Remove(keyvalue);
            if (keys.Contains(keyvalue)) keys.Remove(keyvalue);
        }

        /// <summary>
        /// 取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            if (!_cacheManager.Contains(_cachekey + key))
            {
                throw new Exception(string.Format("不存在关键字“{0}”的缓存.", key));
            }
            var data = (CacheData)_cacheManager.GetData(_cachekey + key);
            return data.Data;
        }

        /// <summary>
        /// 取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CacheData GetCacheData(string key)
        {
            if (!_cacheManager.Contains(_cachekey + key))
            {
                throw new Exception(string.Format("不存在关键字“{0}”的缓存.", key));
            }
            return (CacheData)_cacheManager.GetData(_cachekey + key);
        }

        /// <summary>
        /// 是否存在key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistKey(string key)
        {
            return _cacheManager.Contains(_cachekey + key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void Clear()
        {
            foreach (var key in keys)
            {
                if (_cacheManager.Contains(key))
                    _cacheManager.Remove(key);
            }
            keys.Clear();
        }

        /// <summary>
        /// 过滤出需要的key 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<string> FilterKeys(string criteria)
        {
            return criteria.Length == 0 ? keys : keys.FindAll(x => x.Contains(criteria));
        }

        #endregion
    }
}
