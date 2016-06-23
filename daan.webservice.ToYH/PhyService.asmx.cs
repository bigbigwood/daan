using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using daan.domain;
using System.Web.Script.Services;

namespace daan.webservice.ToYH
{
    /// <summary>
    /// PhyService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class PhyService : System.Web.Services.WebService
    {
        readonly Cache cache = new Cache();

        [WebMethod(Description="登录验证")]
        public string login(string UserCode, string PassWord)
        {
            CacheInfo cacheinfo;
            //登录验证
            string res = Utils.ValidateLogin(out cacheinfo, UserCode, PassWord);
            if (res != string.Empty) { return "0|" + res; }
            //添加缓存
            cache.CacheAdd(cacheinfo);
            return "1|" + cacheinfo.AuthorizationCode;
        }

        [WebMethod(Description="查询报告单状态")]
        public string queryReportStatus(String SID, String barcode, String uname, String umobile)
        {
            string str = cache.CheckAuthKey(SID);
            if (str != string.Empty)
            {
                return "0|" + str;
            }
            string res = Utils.QueryReportStatus(barcode, uname, umobile);
            return res;
        }

        [WebMethod(Description = "查询报告单并获取报告单文件")]
        public string getReportBase64String(String SID, String barcode, String uname, String umobile)
        {
            string str = cache.CheckAuthKey(SID);
            string resstr = string.Empty;
            if (str != string.Empty)
            {
                return resstr = "0|" + str;
            }
            return resstr = Utils.GetReport(barcode, uname, umobile);
        }

        [WebMethod(Description="大众健康平台向体检系统添加TM订单")]
        public string uploadOrderInfo(string SID, string xmlStr)
        {
            string str = cache.CheckAuthKey(SID);
            string resstr = string.Empty;
            if (str != string.Empty)
            {
                return resstr = "0|" + str;
            }
            string res = Utils.UploadOrderInfo(xmlStr);
            if (res.Length > 0)
            {
                return "0|" + res;
            }
            else
            {
                return "1|SUCCESS";
            }
        }

        [WebMethod(Description = "获取待同步体检系统单位信息")]
        public string syncCostomerInfo(string SID, string sysType)
        {
            string str = cache.CheckAuthKey(SID);
            string resstr = string.Empty;
            if (str != string.Empty)
            {
                return resstr = "0|" + str;
            }
            return Utils.syncCostomerInfo(sysType);
        }

        [WebMethod(Description="设置体检单位信息同步状态")]
        public string setSyncStatus(string dictcustomerid, string sysType, string status)
        {
            return Utils.setSyncStatus(dictcustomerid, sysType, status);
        }

        //[WebMethod(Description="修改来自大众健康平台的订单信息")]
        //public string updateOrder(string SID, string barcode, string strXML)
        //{
        //    string str = cache.CheckAuthKey(SID);
        //    string resstr = string.Empty;
        //    if (str != string.Empty)
        //    {
        //        return resstr = "0|" + str;
        //    }
        //    return Utils.updateOrder(barcode, strXML);
        // }

        [WebMethod(Description="删除来自大众健康平台订单")]
        public string deleteOrderByBarcode(string SID, string barcode)
        {
            string str = cache.CheckAuthKey(SID);
            string resstr = string.Empty;
            if (str != string.Empty)
            {
                return resstr = "0|" + str;
            }
            return Utils.deleteOrderByBarcode(barcode);
        }

        [WebMethod(true, Description = "防癌检测与C14信息接口")]
        public string getDataString(String uname, String umobile, String type)
        {
            string resstr = string.Empty;
            return resstr = Utils.GetDataForJson(uname, umobile, type);
        }

        [WebMethod(true, Description = "防癌检测与C14信息接口")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false, XmlSerializeString = false)]
        public void getDataJson(String uname, String umobile, String type)
        {
            string resstr = string.Empty;
            resstr = Utils.GetDataForJson(uname, umobile, type);
            Context.Response.Clear();
            Context.Response.ContentType = "text/html";
            Context.Response.Write(resstr);
        }
    }
}
