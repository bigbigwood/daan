using System;
using System.Web;
using System.Collections;

public class CacheHelper
{
    /// <summary>
    /// 获取数据缓存
    /// </summary>
    /// <param name="CacheKey">键</param>
    public static object GetCache(string CacheKey)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        return objCache[CacheKey];
    }

    /// <summary>
    /// 设置数据缓存
    /// </summary>
    /// <param name="CacheKey">键</param>
    /// <param name="objObject">缓存值</param>
    public static void SetCache(string CacheKey, object objObject)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject);
    }

    /// <summary>
    /// 设置数据缓存
    /// </summary>
    /// <param name="CacheKey">键</param>
    /// <param name="objObject">缓存值</param>
    /// <param name="Timeout">过期时间,比如1天后过期：TimeSpan.FromDays(1)</param>
    public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
    }

    /// <summary>
    /// 设置数据缓存
    /// </summary>
    /// <param name="CacheKey">键</param>
    /// <param name="objObject">缓存值</param>
    /// <param name="absoluteExpiration">绝对过期时间（不能与弹性过期时间同时存在）</param>
    /// <param name="slidingExpiration">弹性过期时间（不能与绝对过期时间同时存在）</param>
    public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
    }

    /// <summary>
    /// 移除指定数据缓存
    /// </summary>
    /// <param name="CacheKey">键</param>
    public static void RemoveAllCache(string CacheKey)
    {
        System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        _cache.Remove(CacheKey);
    }

    /// <summary>
    /// 移除全部缓存
    /// </summary>
    public static void RemoveAllCache()
    {
        System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
        while (CacheEnum.MoveNext())
        {
            _cache.Remove(CacheEnum.Key.ToString());
        }
    }
}
