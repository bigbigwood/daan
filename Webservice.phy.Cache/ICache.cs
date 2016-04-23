using System.Collections.Generic;

namespace Webservice.phy.Cache
{
    /// <summary>
    /// 缓存封装接口，缓存控制需要实现此接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 增加缓存，使用缓存对象cachedata
        /// </summary>
        /// <param name="data"></param>
        void Add(CacheData data);

        /// <summary>
        /// 增加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Add(string key, object data);

        /// <summary>
        /// 加入缓存并且含超时时间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeoutminus"></param>
        void Add(CacheData data, int timeoutminus);


        /// <summary>
        /// 加入缓存并且含超时时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeoutminus"></param>
        void Add(string key, object data, int timeoutminus);

        /// <summary>
        /// 通过关键字移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetData(string key);

        /// <summary>
        /// 取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        CacheData GetCacheData(string key);

        /// <summary>
        /// 是否存在key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ExistKey(string key);

        /// <summary>
        /// 清除缓存
        /// </summary>
        void Clear();

        List<string> FilterKeys(string criteria);
    }
}
