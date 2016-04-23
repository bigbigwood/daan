using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
namespace hlis.service.common
{
    public static class ScriptHelper
    {

        #region 客户端脚本提示
        /// <summary>
        /// 客户端脚本提示
        /// </summary>
        /// <param name="message">要弹出的内容</param>
        static public void Alert(string message)
        {
            ((Page)HttpContext.Current.Handler).RegisterStartupScript("来自中心数据库平台的消息", "<script>alert(\"" + EncodeScriptText(message) + "\");</script>");
        }
        /// <summary>
        /// 客户端脚本提示并转向
        /// </summary>
        /// <param name="message">要弹出的内容</param>
        /// <param name="lingurl">转向地址</param>
        static public void Alert(string message, string lingurl)
        {
            ((Page)HttpContext.Current.Handler).Response.Write("<script language=javascript>alert('" + message + "');window.location='" + lingurl + "'</script>");
            ((Page)HttpContext.Current.Handler).Response.End();
        }
        /// <summary>
        /// 框架页面弹出对话框并跳出框架转向
        /// </summary>
        /// <param name="message"></param>
        /// <param name="strLinks"></param>
        static public void AlertForHouTai(string message, string strLinks)
        {
            ((Page)HttpContext.Current.Handler).Response.Write("<script language=javascript>alert('" + message + "');top.window.location='" + strLinks + "'</script>");
            ((Page)HttpContext.Current.Handler).Response.End();
        }
        /// <summary>
        /// 编码脚本
        /// </summary>
        /// <param name="script">要编码的脚本</param>
        /// <returns></returns>
        private static string EncodeScriptText(string script)
        {
            return script.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\t", @"\t").Replace("\a", @"\a").Replace("\b", @"\b");
        }
        #endregion

        #region 将datatable转换为json数据格式
        /// <summary>
        /// 将datatable转换为json数据格式
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>json格式的字符串</returns>
        public static string CreateJsonParameters(DataTable dt)
        {

            var jsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                jsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{ ");
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":" + "\"" + dt.Rows[i][j] + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":" + "\"" + dt.Rows[i][j] + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        jsonString.Append("} ");
                    }
                    else
                    {
                        jsonString.Append("}, ");
                    }
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            else
            {
                return "{\"total\":1,\"rows\":[]}";
            }
        }
        #endregion

        public static string SwitchToJson<T>(IList<T> li) where T : class
        {
            var jsonString = new StringBuilder();
            if (li != null && li.Count > 0)
            {
                jsonString.Append("[ ");
                for (int i = 0; i < li.Count; i++)
                {
                    jsonString.Append("{ ");
                    T obj = Activator.CreateInstance<T>();
                    Type type = obj.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    for (var j = 0; j < pis.Length; j++)
                    {
                        if (j < pis.Length - 1)
                        {
                            jsonString.Append("\"" + pis[j].Name.ToUpper() + "\":" + "\"" + pis[j].GetValue(li[i], null) + "\",");
                        }
                        else if (j == pis.Length - 1)
                        {
                            jsonString.Append("\"" + pis[j].Name + "\":" + "\"" + pis[j].GetValue(li[i], null) + "\"");
                        }
                    }
                    if (i == li.Count - 1)
                    {
                        jsonString.Append("} ");
                    }
                    else
                    {
                        jsonString.Append("}, ");
                    }
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            else
            {
                return "{\"total\":1,\"rows\":[]}";
            }
        }


        #region 将Datatable转换为数组类型
        /// <summary>
        /// 将Datatable转换为数组类型
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>object数组</returns>
        static public object[,] CreateArrayParameters(DataTable dt)
        {
            var obj = new object[dt.Rows.Count, dt.Columns.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    obj[i, j] = dt.Rows[i][j].ToString().Replace("<br />", "");
                }
            }
            return obj;
        }
        #endregion
        #region Lis查询将查询ID转换成对应的中文描述
        static public string GetLisName(int type)
        {
            string lisname;
            switch (type)
            {
                case 12:
                    lisname = "医院工作量统计";
                    break;
                case 18:
                    lisname = "医院样本明细统计";
                    break;
                case 19:
                    lisname = "医院样本开单项目统计";
                    break;
                case 23:
                    lisname = "医院项目结果明细统计";
                    break;
                case 20:
                    lisname = "医院开单医生工作量统计";
                    break;
                case 14:
                    lisname = "区域工作量统计";
                    break;
                case 16:
                    lisname = "业务员样本工作量统计";
                    break;
                case 17:
                    lisname = "业务员项目工作量统计";
                    break;
                case 28:
                    lisname = "地区工作量统计";
                    break;
                default:
                    lisname = "医院工作量统计";
                    break;
            }
            return lisname;
        }
        #endregion

        #region 返回各查询类别对应的权限识别字符
        /// <summary>
        /// 返回各查询类别对应的权限识别字符
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public string GetRuleResourceCode(int type)
        {
            string resourcecode;
            switch (type)
            {
                case 12:
                    resourcecode = "itmufrmStatCustomerAll";//医院工作量统计
                    break;
                case 18:
                    resourcecode = "itmufrmStatCustomerSid";//医院样本明细统计
                    break;
                case 19:
                    resourcecode = "itmufrmStatCustomerSidTest";//医院样本开单项目统计
                    break;
                case 23:
                    resourcecode = "itmufrmStatCustomerTestResult";//医院项目结果明细统计
                    break;
                case 20:
                    resourcecode = "itmufrmStatCustomerDoctorTest";//医院开单医生工作量统计
                    break;
                case 14:
                    resourcecode = "itmufrmStatRegionidAll";//区域工作量统计
                    break;
                case 16:
                    resourcecode = "itmufrmStatSalesmanSid";//业务员样本工作量统计
                    break;
                case 17:
                    resourcecode = "itmufrmStatSalesmanTest";//业务员项目工作量统计
                    break;
                case 28:
                    resourcecode = "itmufrmStatCustdistrictAll";//地区工作量统计
                    break;
                default:
                    resourcecode = "itmufrmStatCustomerAll";//医院工作量统计
                    break;
            }
            return resourcecode;
        }
        #endregion

        #region 获取MD5值
        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        #region  将datatable转化为类型为T的LIST集合
        /// <summary>
        /// 将datatable转化为类型为T的LIST集合
        /// </summary>
        /// <typeparam name="T"> 必须继承自basedomain</typeparam>
        /// <returns></returns>
        public static List<T> datatableToLstT<T>(DataTable lstSource)
        {
            List<T> lst = new List<T>();
            Type type = typeof(T);
            if (lstSource == null) return lst;
            foreach (DataRow ht in lstSource.Rows)
            {

                T t = (T)Activator.CreateInstance(type);
                Copy(ht, t);
                //type.InvokeMember("Copy", BindingFlags.InvokeMethod, null, t, new Object[] { ht });
                lst.Add(t);
            }
            return lst;
        }
        #endregion

        /// <summary>
        /// 从一个datarow中复制和属性名相同的key对应的值，不区分大小写
        /// </summary>
        /// <param name="ht"></param>
        public static void Copy(DataRow ht, Object obj)
        {
            Type t = obj.GetType();
            foreach (PropertyInfo p in t.GetProperties())
            {

                foreach (DataColumn col in ht.Table.Columns)
                {
                    if (col.ColumnName.ToString().ToLower() == p.Name.ToLower() && (p.GetSetMethod() != null))  //找到名称一样的，准备复制数据
                    {
                        if (ht[col.ColumnName.ToString()] == null)
                        {
                            try
                            {
                                p.SetValue(obj, null, null);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {

                            if (ht[col.ColumnName.ToString()].GetType() == p.PropertyType)
                            {
                                p.SetValue(obj, ht[col.ColumnName.ToString()], null);
                            }
                            else if (p.PropertyType.FullName.ToLower().Contains("system.nullable"))
                            {
                                int j = p.PropertyType.FullName.IndexOf("[[System") + 2; ;
                                int k = p.PropertyType.FullName.IndexOf(","); ;
                                string typename = p.PropertyType.FullName.Substring(j, k - j);
                                try
                                {
                                    object obj2 = Convert.ChangeType(ht[col.ColumnName.ToString()], Type.GetType(typename));
                                    p.SetValue(obj, obj2, null);
                                }
                                catch (Exception) { }
                            }
                            else
                            {
                                object obj2 = Convert.ChangeType(ht[col.ColumnName.ToString()], p.PropertyType);
                                p.SetValue(obj, obj2, null);
                            }
                        }
                        break;
                    }

                }
            }
        }

        #region 根据关键字返回对应的业务表名称
        /// <summary>
        /// 根据下标返回对应的业务表名称
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static string GetBussinessTableName(string key)
        {
            Dictionary<string, string> dictBussinessTableName = new Dictionary<string, string>();
            dictBussinessTableName.Add("search", "RecCustomerSearch"); //查询
            dictBussinessTableName.Add("out", "RecCustomerConsult");//外部咨询
            dictBussinessTableName.Add("inner", "RecInnerConsult");//内部咨询
            dictBussinessTableName.Add("need", "RecNeedConsumable");//耗材
            dictBussinessTableName.Add("research", "RecResearchConsult");//科研咨询
            dictBussinessTableName.Add("spe", "RecGetSpecimen");//有标本
            dictBussinessTableName.Add("borrow", "RecBorrow");//借片
            dictBussinessTableName.Add("addspe", "RecAddSpecimen");//补单
            dictBussinessTableName.Add("ice", "RecBookIce");//约冰冻
            dictBussinessTableName.Add("emer", "RecBookEmergency");//约急诊
            dictBussinessTableName.Add("special", "RecSpecialrequest");//特殊要求
            dictBussinessTableName.Add("feedback", "RecNormalFeedback");//日常反馈
            dictBussinessTableName.Add("other", "RecOther");//其它
            dictBussinessTableName.Add("udplog", "RecUpdateLog");//信息修改    
            return dictBussinessTableName[key];
        }
        #endregion




    }
}
