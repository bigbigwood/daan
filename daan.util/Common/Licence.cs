/**********************************************
 * 类作用：   授权类
 * 建立人：   daan
 * 建立时间： 2012-03-28 
 ***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;

namespace daan.util.Common
{
    /// <summary>
    /// 确定是否授权
    /// </summary>
    public sealed class Licence
    {
        //private static string key = "92f6766f-4b26-40ef-b27c-0b93057d4377";
        public static bool IsLicence(string key)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            if (host.Equals("localhost"))
                return true;

            string Licence = ConfigurationManager.AppSettings["licence"];
            if (Licence != null && Licence == StringUtil.md5(host + key, 16))
                return true;

            return false;
        }

        /// <summary>
        /// 获取许可证号
        /// </summary>
        /// <param name="host"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLicence(string host, string key)
        {
            return StringUtil.md5(host + key, 16);
        }
    }
}
