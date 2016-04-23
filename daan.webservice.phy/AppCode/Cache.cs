using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webservice.phy.Cache;
using daan.domain;

namespace daan.webservice.phy
{
    public class Cache
    {
        private const string LoginCacheKey = "Authorization";

        private ICache GetLoginCache()
        {
            if (!EnterpriceLibraryCacheHelper.IsRegistration(LoginCacheKey))
                EnterpriceLibraryCacheHelper.Registration(LoginCacheKey);
            return EnterpriceLibraryCacheHelper.GetCache(LoginCacheKey);
        }


        private string GetAuthKey(string authorizationcode)
        {
            return "AuthId:" + authorizationcode;
        }

        //验证是否登录过
        public string CheckAuthKey(string SID)
        {
            string str = string.Empty;
            if (!GetLoginCache().ExistKey(GetAuthKey(SID)))
            {
                str = ErrorCode.Login_1005;
            }
            return str;
        }

        /// <summary>
        /// 登录成功 添加缓存
        /// </summary>
        /// <param name="result"></param>
        public void CacheAdd(CacheInfo result)
        {
            GetLoginCache().Add(GetAuthKey(result.AuthorizationCode), result, 240);//时间为分钟
        }


        public CacheInfo GetCacheData(string SID)
        {
            return GetLoginCache().GetData(GetAuthKey(SID)) as CacheInfo;
        }
    }
}