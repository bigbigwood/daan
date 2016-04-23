using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace daan.webservice
{
    /// <summary>
    /// CenterService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CenterService : WebService
    {
        [WebMethod(Description="登录")]
        public string Login(string UserCode, string PassWord)
        {
            const string methodname = "Login";
            object[] para = new object[] { UserCode, PassWord };
            string SID = WebServiceUtil.Execute(methodname, para);
            return SID;
        }

        [WebMethod(Description = "易感基因系统向体检系统发送订单信息")]
        public string uploadOrderInfo(string SID, string XML)
        {
            const string methodname = "uploadOrderInfo";
            object[] obj = new object[] { SID,XML};
            string returnstr = string.Empty;
            returnstr = WebServiceUtil.Execute(methodname, obj);
            return returnstr;
        }
    }
}
