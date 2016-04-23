using System;

namespace Webservice.phy.Cache
{
    /// <summary>
    /// 缓存数据对象
    /// </summary>
    public class CacheData
    {
        private string _key;
        private DateTime _cachedate;
        private object _data;


        public CacheData(string key, object data)
            : this(key, DateTime.Now, data)
        {

        }

        public CacheData(string key, DateTime cachedate, object data)
        {
            Key = key;
            Cachedate = cachedate;
            Data = data;
        }

        /// <summary>
        /// 缓存关键字
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// 缓存日期，生成缓存的日期
        /// </summary>
        public DateTime Cachedate
        {
            get { return _cachedate; }
            set { _cachedate = value; }
        }

        /// <summary>
        /// 缓存的数据
        /// </summary>
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
