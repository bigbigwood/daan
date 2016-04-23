using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using daan.domain;

namespace daan.webservice.phy
{
    /// <summary>
    /// PhyService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class PhyService : WebService
    {
        readonly Cache cache = new Cache();

        #region >>>> 登录
        [WebMethod(Description ="登录验证")]
        public string Login(string UserCode, string PassWord)
        {
            CacheInfo cacheinfo;
            //登录验证
            string res = Utils.ValidateLogin(out cacheinfo, UserCode, PassWord);
            if (res != string.Empty) { return "0|" + res; }
            //添加缓存
            cache.CacheAdd(cacheinfo);
            return "1|" + cacheinfo.AuthorizationCode;
        }
        #endregion

        #region >>>> 接收订单
        [WebMethod(Description="接收来自易感基因系统订单")]
        public string uploadOrderInfo(string SID, string XML)
        {
            string str = cache.CheckAuthKey(SID);
            if (str != string.Empty) { return "0|" + str; }
            string res = Utils.ReceiveXMLData(SID, XML);
            if (res != string.Empty)
            {
                return "0|" + res;
            }
            else
            {
                return "1|SUCCESS";
            }
        }
        #endregion
    }
}
