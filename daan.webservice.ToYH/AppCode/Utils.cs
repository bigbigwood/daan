using System;
using System.Collections.Generic;
using System.Linq;
using daan.domain;
using System.Configuration;
using daan.service.order;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using daan.service.login;
using daan.service.proceed;
using daan.service.dict;
using daan.service.report;
using daan.util.Common;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace daan.webservice.ToYH
{
    public class Utils
    {
        static readonly OrdersService os = new OrdersService();
        static readonly LoginService loginservice = new LoginService();
        static readonly ProRegisterService registerservice = new ProRegisterService();
        static readonly OrderbarcodeService barcodeservice = new OrderbarcodeService();
        static readonly DictCustomerService customerservice = new DictCustomerService();
        static readonly ReportService rservice = new ReportService();
        static readonly string enterby = "大众平台";
        static readonly double? enterbyID = 0;
        /// <summary>登录验证
        /// 
        /// </summary>
        /// <param name="cacheinfo"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static string ValidateLogin(out CacheInfo cacheinfo, string UserCode, string PassWord)
        {
            cacheinfo = new CacheInfo() { UserCode = UserCode, PassWord = PassWord };
            if (string.IsNullOrEmpty(UserCode) || string.IsNullOrEmpty(PassWord))
            {
                return ErrorCode.Login_1003;
            }
            string userid = ConfigurationManager.AppSettings["userid"];
            string pwd = ConfigurationManager.AppSettings["pwd"];
            if (cacheinfo.UserCode == userid && createMD5code(cacheinfo.PassWord) == pwd)
                cacheinfo.AuthorizationCode = Guid.NewGuid().ToString();
            else
                return ErrorCode.Login_1004;
            return string.Empty;
        }

        /// <summary>查询报告状态
        /// 条码号必填
        /// 姓名和手机号选填，填了就会匹配
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="uname">姓名</param>
        /// <param name="umobile">手机号码</param>
        /// <returns>报告状态</returns>
        public static string QueryReportStatus(string barcode,string uname,string umobile)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return "0|" + ErrorCode.Query_0001;
            }
            DataTable dt = new DataTable();
            try
            {
                dt = os.QueryReportStatus(barcode);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return "0|" + ErrorCode.Query_0002;
                }
                DataRow dr = dt.Rows[0];
                //传了姓名过来才匹配
                if (!string.IsNullOrEmpty(uname) && dr["realname"].ToString() != uname)
                {
                    return "0|" + ErrorCode.Query_0003;
                }
                //传了手机号码才匹配
                if (!string.IsNullOrEmpty(umobile) && dr["mobile"].ToString() != umobile)
                {
                    return "0|" + ErrorCode.Query_0004;
                }
                if (dr["iscancel"].ToString() == "1")
                {
                    return "1|" + "已作废。作废原因[" + dr["cancelreason"] + "]";
                }
                return "1|" + dr["basicname"];
            }
            catch (Exception ex)
            {
                return String.Format("0|{0} {1}", ErrorCode.Query_0005, ex.Message);
            }
        }

        /// <summary>查询并将PDF报告以Base64字符串返回
        /// 
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="uname">姓名</param>
        /// <param name="umobile">手机号码</param>
        /// <returns></returns>
        public static string GetReport(string barcode, string uname, string umobile)
        {
            string retstr = "0|";
            if (string.IsNullOrEmpty(barcode) || string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(umobile))
            {
                return retstr += ErrorCode.Down_0001;
            }
            //获取需要下载的报告路径，此路径为报告上传社区扫描程序的路径  
            string pdfFileServicePath = ConfigurationManager.AppSettings["PdfFile"];
            try
            {
                DataTable dt = os.QueryReportStatus(barcode);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return retstr += ErrorCode.Down_0002;
                }
                DataRow dr = dt.Rows[0];
                if (dr["realname"].ToString() != uname)
                    return retstr += ErrorCode.Down_0003;
                if (dr["mobile"].ToString() != umobile)
                    return retstr += ErrorCode.Down_0004;
                string realname = dr["realname"].ToString();//姓名
                string idnumber = dr["idnumber"].ToString();//身份证号码
                string status = dr["status"].ToString();//状态值
                string ordernum=dr["ordernum"].ToString();//体检号
                //检查是否已完成总检
                if (Convert.ToInt32(status) < (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck)
                {
                    return retstr += ErrorCode.Down_0005;
                }
                //报告文件名
                string filename = string.Empty;
                if (string.IsNullOrEmpty(idnumber))
                {
                    filename = string.Format("{0}_{1}.pdf", ordernum, realname);
                }
                else
                {
                    filename = string.Format("{0}_{1}_{2}.pdf", idnumber, ordernum, realname);
                }
                string fullpath = pdfFileServicePath + filename;
                if (!File.Exists(fullpath))
                    return retstr += ErrorCode.Down_0006;
                //下载报告
                WebClient my = new WebClient();
                byte[] bt = my.DownloadData(fullpath);
                string base64String = byteTOBase64String(bt);
                retstr = "1|" + base64String;
                return retstr;
            }
            catch (Exception ex)
            {
                return retstr += ErrorCode.Down_0007 + ex.Message;
            }
        }

        /// <summary>上传体检订单
        /// 
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string UploadOrderInfo(string xmlStr)
        {
            xmlStr = xmlStr.TrimStart('﻿');
            string str = "<?xml version='1.0' encoding='utf-8'?>" + StringToXML(xmlStr);
            string strMessage = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                dt = GetDataTable(str);
            }
            catch (Exception ex)
            {
                return String.Format("{0} {1}", ErrorCode.Up_0001, ex.Message);
            }
            if (dt == null || dt.Rows.Count == 0 || dt.Columns.Count != 20) { return ErrorCode.Up_0002; }
            List<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemList();//项目字典表
            List<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetail();//套餐组合字典
            string _productname = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                #region 必填项
                //套餐代码
                string productTestCode = dr["uniquecode"].ToString().Replace('_', ' ').Trim();
                //客户代码
                string dictcustomerid = dr["dictcustomerid"].ToString().Trim();
                //姓名
                string realname = dr["realname"].ToString().Trim();
                //性别
                string sex = dr["sex"].ToString().Replace('_', ' ').Trim() == "女" ? "F" : (dr["sex"].ToString().Replace('_', ' ').Trim() == "男" ? "M" : "U");
                //婚否
                string ismarried = dr["ismarried"].ToString().Trim();
                //手机
                string mobile = dr["mobile"].ToString().Trim();
                //住址
                string address = dr["address"].ToString().Trim();
                //省
                string province = dr["province"].ToString().Trim();
                //市
                string city = dr["city"].ToString().Trim();
                //分点实验室
                string dictlabid = dr["dictlabid"].ToString().Trim();
                //条码号
                string barcode = dr["barcode"].ToString().Trim();
                if (string.IsNullOrEmpty(productTestCode) || string.IsNullOrEmpty(dictcustomerid) || string.IsNullOrEmpty(dictlabid)||string.IsNullOrEmpty(barcode)
                    || string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(ismarried)
                    || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(province) || string.IsNullOrEmpty(city))
                {
                    strMessage = ErrorCode.Up_0003;
                    break;
                }
                #endregion
                #region 二者不可都为空
                //出生日期
                string birthday = dr["birthday"].ToString().Trim();
                //年龄
                string age = dr["age"].ToString().Trim();
                if (string.IsNullOrEmpty(birthday) && string.IsNullOrEmpty(age))
                {
                    strMessage = ErrorCode.Up_0004;
                    break;
                }
                #endregion
                #region 可空字段
                //身份证
                string idnumber = dr["idnumber"].ToString().Trim();
                //部门
                string section = dr["section"].ToString().Trim();
                //备注
                string remark = dr["remark"].ToString().Trim();
                //电话
                string phone = dr["phone"].ToString().Trim();
                //邮箱
                string email = dr["email"].ToString().Trim();
                //采样日期
                string samplingdate = dr["samplingdate"].ToString().Trim();
                //区
                string county = dr["county"].ToString().Trim();
                #endregion
                DateTime datebirthday;
                bool datebirthdayb = DateTime.TryParse(birthday, out datebirthday);
                DateTime datesamplingdate;
                bool datesamplingdateb = DateTime.TryParse(samplingdate, out datesamplingdate);
                if ((birthday != string.Empty && !datebirthdayb) || (samplingdate != string.Empty && !datesamplingdateb))
                {
                    strMessage = ErrorCode.Up_0010;
                    break;
                }
                //检查单位是否在体检系统中有维护
                try
                {
                    using (DataTable d = customerservice.CheckHasCustomer(dictcustomerid))
                    {
                        if (d == null || d.Rows.Count == 0||d.Rows.Count > 1)
                        {
                            strMessage = ErrorCode.Up_0011;
                            break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    strMessage = ErrorCode.Up_0015 + " " + ee.Message;
                    break;
                }
                #region 添加套餐
                //查询分点+公用套餐
                List<Dicttestitem> productlistTemp = new DicttestitemService().GetProduct(TypeParse.StrToDouble(dictcustomerid, 0));
                List<Dicttestitem> productList = productlistTemp.Where<Dicttestitem>(c => c.Testcode == productTestCode && (c.Forsex == sex || c.Forsex == "B")).ToList<Dicttestitem>();
                List<Dicttestitem> grouptestList = new List<Dicttestitem>();
                Dicttestitem productinfo = null;
                if (productList.Count == 0)
                {
                    strMessage = ErrorCode.Up_0005;
                    break;
                }
                else if (productList.Count > 1)
                {
                    strMessage = ErrorCode.Up_0006;
                    break;
                }
                else
                {
                    productinfo = productList.First<Dicttestitem>();
                    List<OrderRegister> _gridtestList = null;
                    #region 添加套餐
                    string msg = registerservice.AddProduct(ref _gridtestList, sex, productinfo.Dicttestitemid, false, null, ref _productname, null);
                    if (msg != string.Empty)
                    {
                        strMessage = ErrorCode.Up_0014;
                        break;
                    }
                    #endregion
                    if (barcode != string.Empty && barcode.Length != 12)//条码号必须为12位数字
                    {
                        strMessage = ErrorCode.Up_0007;
                        break;
                    }
                    if (barcode != string.Empty && barcode.Substring(barcode.Length - 2) != "00")//条码号必须以00结尾
                    {
                        strMessage = ErrorCode.Up_0008;
                        break;
                    }
                    //检验条码号是否合格且存在于体检系统
                    DataTable dtBarcode = barcodeservice.CheckBarCode2(barcode);
                    if (dtBarcode != null && dtBarcode.Rows.Count > 0)
                    {
                        //判断已存在的条码是否来自大众平台
                        if (!dtBarcode.Rows[0]["enterby"].ToString().Contains("大众平台"))
                        {
                            //条码已存在且不是通过大众平台上传
                            strMessage = ErrorCode.Up_0009;
                            break;
                        }
                        else
                        {
                            //判断该条码是否已经上传到康源系统，否则不允许修改；
                            if (dtBarcode.Rows[0]["transed"].ToString()=="1")
                            {
                                strMessage = ErrorCode.Update_0005;
                                break;
                            }
                            //删除该体检系统中订单
                            string ordernum = dtBarcode.Rows[0]["ordernum"].ToString();
                            Hashtable ht = new Hashtable();
                            ht["ordernum"] = ordernum;
                            new ProCentralizedManagementService().DeleteOrders(ht);
                        }
                    }

                    //套餐下组合项目
                    IEnumerable<Dictproductdetail> IEgroup = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid);
                    bool iscontinue = true;
                    int count = IEgroup.Count<Dictproductdetail>();
                    int k = 0;
                    foreach (Dictproductdetail item in IEgroup)
                    {
                        IEnumerable<Dicttestitem> IEgruptest = TestItemList.Where<Dicttestitem>(c => c.Dicttestitemid == item.Testgroupid);
                        if (IEgruptest.Count() <= 0)
                        {
                            //没有找到套餐下组合
                            k++;
                            continue;
                        }
                        Dicttestitem groupinfo = IEgruptest.First<Dicttestitem>();
                        //校验性别是否符合
                        string res = registerservice.checkSex(groupinfo.Dicttestitemid, sex);
                        if (res != string.Empty)
                        {
                            //性别项目不合
                            strMessage = ErrorCode.Up_0012;
                            iscontinue = false;
                            break;
                        }

                        groupinfo.Productid = productinfo.Dicttestitemid;
                        groupinfo.Productname = productinfo.Testname;///套餐名
                        groupinfo.IsActive = "1";//是否停止测试
                        groupinfo.Isadd = "0";///是否追加 
                        groupinfo.Billed = "0";
                        groupinfo.Sendbilled = "0";
                        groupinfo.Adduserid = null;//追加人ID

                        if (barcode == string.Empty)
                        {
                            IEnumerable<Dicttestitem> IEtempbarcodeList = grouptestList.Where<Dicttestitem>(c => c.Tubegroup == groupinfo.Tubegroup);
                            if (IEtempbarcodeList.Count() > 0)
                            {
                                groupinfo.Barcode = IEtempbarcodeList.First<Dicttestitem>().Barcode;
                            }
                            else
                            {
                                groupinfo.Barcode = registerservice.GetBarCode();
                            }
                        }
                        else
                        {
                            groupinfo.Barcode = barcode;
                        }

                        //获取外包客户
                        Dictproductdetail detail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid && c.Testgroupid == groupinfo.Dicttestitemid).First<Dictproductdetail>();
                        groupinfo.Sendoutcustomerid = detail.Sendoutcustomerid;

                        grouptestList.Add(groupinfo);
                    }
                    if (!iscontinue) { continue; }
                    else
                    {
                        if (k >= count)
                        {
                            strMessage = ErrorCode.Up_0013;
                            break;
                        }
                    }
                }
                #endregion

                #region 添加会员信息
                Dictmember member = new Dictmember() { Realname = realname, Idnumber = idnumber };
                
                registerservice.checkmember(null, ref member);
                if (datebirthdayb) member.Birthday = datebirthday;
                member.Nickname = member.Realname;
                member.Sex = sex;
                member.Addres = address;
                member.Phone =phone;
                member.Mobile = mobile;
                member.Email = email;
                #endregion

                #region 添加订单
                double year = 0, month = 0, day = 0;
                double hours = 0;
                double aged = 0;
                string agestr = age;
                bool ageb = double.TryParse(agestr, out aged);

                if (member.Birthday == null)
                {
                    if (agestr != string.Empty && ageb)
                    {
                        year = aged;
                        day = aged * 365;
                        member.Birthday = datebirthday = DateTime.Now.AddDays((0 - day));
                    }
                    else
                    {
                        strMessage = ErrorCode.Up_0004;
                        break;
                    }
                }
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(member.Birthday);//时间差
                year = Math.Truncate((double)(ts.Days / 365));
                month = (ts.Days % 365) / 30;
                day = (ts.Days % 365) % 30;
                hours = ts.TotalHours;

                Orders _orders = new Orders()
                {
                    Ordernum = new ProRegisterService().GetOrderNum(),
                    Dictmemberid = member.Dictmemberid,
                    Dictcustomerid = Convert.ToDouble(dictcustomerid),
                    Realname = realname,
                    Sex = sex,
                    Caculatedage = hours,
                    Remarks = remark,
                    Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, 0),
                    Enterby = enterby,
                    Ordertestlst = _productname + ",",
                    Dictlabid = Convert.ToDouble(dictlabid),
                    Ordersource = "1",
                    Ismarried = ismarried == "未婚" ? "0" : (ismarried == "已婚" ? "1" : "2"),
                    Section = section,
                    Status = ((int)daan.service.common.ParamStatus.OrdersStatus.BarCodePrint).ToString(),
                    Province = province,
                    City = city,
                    County = county
                };
                if (datesamplingdateb) _orders.SamplingDate = datesamplingdate;
                string errstr = string.Empty;
                System.Collections.Hashtable htScan = new System.Collections.Hashtable();
                htScan.Add("isScan", true);
                htScan.Add("EnterByID", enterbyID);
                htScan.Add("EnterBy", enterby);
                bool b = registerservice.insertUpdateOrders("大众健康平台对接订单", "", true, productList, grouptestList, member, _orders, "", ref errstr, htScan);
                if (!b)
                { 
                    strMessage = String.Format("{0} {1}", ErrorCode.Up_0015, errstr);
                    break;
                }
                #endregion
            }

            return strMessage;
        }

        /// <summary>同步体检系统单位信息
        /// 
        /// </summary>
        /// <param name="sysType"></param>
        /// <returns></returns>
        public static string syncCostomerInfo(string sysType)
        {
            string result_Fail = "0|";
            string result_Suss = "1|";
            if (sysType != "1" && sysType != "2")
            {
                return result_Fail + ErrorCode.Sync_0001;
            }
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("sysType", sysType);
                DataTable dt = customerservice.GetNotSynchronizedInfoList(ht);
                if (dt == null || dt.Rows.Count == 0)
                    return result_Fail + ErrorCode.Sync_0002;
                string xmlString = ConvertDataTableToXML(dt);
                if (string.IsNullOrEmpty(xmlString))
                    return result_Fail + ErrorCode.Sync_0003;
                return result_Suss + xmlString.Replace("<NewDataSet>", "<data>").Replace("<Table>", "<datarow>").Replace("</Table>", "</datarow>").Replace("</NewDataSet>", "</data>");
            }
            catch (Exception e)
            {
                return string.Format("{0}{1} :{2}", result_Fail, ErrorCode.Sync_0006, e.Message);
            }
        }

        /// <summary>设置同步状态
        /// 
        /// </summary>
        /// <param name="dictcustomerid">单位ID</param>
        /// <param name="sysType">系统类型  1为易感基因  2为大众健康</param>
        /// <param name="status">同步状态  1同步成功  0同步失败</param>
        /// <returns></returns>
        public static string setSyncStatus(string dictcustomerid, string sysType, string status)
        {
            string result_Fail = "0|";
            string result_Suss = "1|";
            if (sysType != "1" && sysType != "2")
            {
                return result_Fail + ErrorCode.Sync_0001;
            }
            using (DataTable d = customerservice.CheckHasCustomer(dictcustomerid))
            {
                if (d == null || d.Rows.Count == 0 || d.Rows.Count > 1)
                {
                    return result_Fail + ErrorCode.Sync_0004;
                }
            }
            string s = status != "1" ? "3" : status;
            Hashtable htPara = new Hashtable();
            htPara.Add("dictcustomerid", dictcustomerid);
            htPara.Add("status", s);
            htPara.Add("sysType", sysType);
            string res = customerservice.setSyncStatus(htPara);
            if (!string.IsNullOrEmpty(res))
                return string.Format("{0}{1}:{2}", result_Fail, ErrorCode.Sync_0006, res);
            return result_Suss + "SUCCESS"; 
        }

        /// <summary>修改大众平台对接受检者信息
        /// 
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="strXML">正确的信息(格式同订单上传)</param>
        /// <returns></returns>
        public static string updateOrder(string barcode,string strXML)
        {
            string result_Fail = "0|";
            string result_Suss = "1|";
            //检验条码号是否合格且存在于体检系统
            DataTable dtBarcode = null;
            if (string.IsNullOrEmpty(barcode))
            {
                return result_Fail + ErrorCode.Update_0001;
            }
            else
            {
                if (barcode.Length != 12 || barcode.Substring(barcode.Length - 2) != "00")
                {
                    return result_Fail + ErrorCode.Update_0002;
                }
                dtBarcode = barcodeservice.CheckBarCode2(barcode);
                if (dtBarcode == null || dtBarcode.Rows.Count == 0)
                {
                    return result_Fail + ErrorCode.Update_0003;
                }
            }
            //判断待修改的订单是否来自大众平台
            if (!dtBarcode.Rows[0]["enterby"].ToString().Contains("大众平台"))
            {
                return result_Fail + ErrorCode.Update_0004;
            }
            //判断该条码是否已经有检测项有结果，否则不允许修改；
            if (Convert.ToInt32(dtBarcode.Rows[0]["iolis"].ToString()) > 0)
            {
                return result_Fail + ErrorCode.Update_0005;
            }
            //删除该体检系统中订单
            string ordernum = dtBarcode.Rows[0]["ordernum"].ToString();
            Hashtable ht = new Hashtable();
            ht["ordernum"] = ordernum;
            new ProCentralizedManagementService().DeleteOrders(ht);
            //重新上传订单到体检系统
            string res = UploadOrderInfo(strXML);
            if (string.IsNullOrEmpty(res))
            {
                return result_Suss + "SUCCESS";
            }
            else
            {
                return result_Fail + res;
            }
        }

        /// <summary>删除大众平台订单
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string deleteOrderByBarcode(string barcode)
        {
            string result_Fail = "0|";
            string result_Suss = "1|";

            if (string.IsNullOrEmpty(barcode))
                return result_Fail + ErrorCode.Update_0001;
            if (barcode.Length != 12 || barcode.Substring(barcode.Length - 2) != "00")
                return result_Fail + ErrorCode.Update_0002;

            //检验条码号是否合格且存在于体检系统
            DataTable dtBarcode = barcodeservice.CheckBarCode2(barcode);
            if (dtBarcode != null && dtBarcode.Rows.Count > 0)
            {
                //判断已存在的条码是否来自大众平台
                if (!dtBarcode.Rows[0]["enterby"].ToString().Contains("大众平台"))
                {
                    //条码已存在且不是通过大众平台上传
                    return result_Fail + ErrorCode.Update_0004;
                }
                else
                {
                    //判断该条码是否已经同步到康源系统，否则不允许修改；
                    if (dtBarcode.Rows[0]["transed"].ToString() == "1")
                    {
                        return result_Fail + ErrorCode.Update_0005;
                    }
                    //删除该体检系统中订单
                    string ordernum = dtBarcode.Rows[0]["ordernum"].ToString();
                    Hashtable ht = new Hashtable();
                    ht["ordernum"] = ordernum;
                    try
                    {
                        bool b = new ProCentralizedManagementService().DeleteOrders(ht);
                        if (b)
                            return result_Suss + "SUCCESS";
                        else
                            return result_Fail +ErrorCode.Update_0007;
                    }
                    catch (Exception e)
                    {
                        return result_Fail + ErrorCode.Update_0007 + e.Message;
                    }
                }
            }
            else
            {
                return result_Fail + ErrorCode.Update_0003;
            }
        }

        /// <summary>
        /// 防癌orC14信息以json形式返回
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="umobile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDataForJson(string uname, string umobile, string type)
        {
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(umobile) || string.IsNullOrEmpty(type))
            {
                return "{\"0|\":\"姓名|手机号|类型为空\"}";
            }
            else if (type != "BG_1001" && type != "BG_1006")
            {
                return "{\"0|\":\"此类型不存在\"}";
            }

            try
            {
                DataTable dt = new DataTable();
                Hashtable ht = new Hashtable();
                ht.Add("uname", uname);
                ht.Add("umobile", umobile);
                //根据用户名、手机号码获取最新订单号
                dt = os.GetOrderNum(ht);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return "{\"0|\":\"不存在条码号\"}";
                }
                DataRow dr = dt.Rows[0];
                if (dr["iscancel"].ToString() == "1")
                {
                    return "{\"0|\":" + "已作废。作废原因[" + dr["cancelreason"] + "]}";
                }
                if (Convert.ToInt32(dr["status"]) < 25)
                {
                    return "{\"0|\":\"完成总检后才能查看报告\"}";
 
                }

                DataSet ds = new DataSet();
                string strjson = string.Empty;
                if (type == "BG_1001")
                {
                    //获取报告相关信息
                    ds = rservice.GetReportDataByType(dr["ordernum"].ToString(), type);
                    string dtRepTitleresult = "";//基本信息
                    string dtOrderresultcommentresult = "";//总体评价
                    string dtRepImportantSignsresult = "";//本次体检结果
                    string dtRepDiseaseGuideresult = "";//解读与建议
                    string dtRepExamComparedresult = "";//历史检测比对

                    #region>>>>>基本信息
                    ////基本信息
                    //DataTable dtRepTitle = new DataTable();
                    //dtRepTitle.Columns.Add("姓名");
                    //dtRepTitle.Columns.Add("性别");
                    //dtRepTitle.Columns.Add("年龄");
                    //dtRepTitle.Columns.Add("检验编号");
                    //dtRepTitle.Columns.Add("报告日期");
                    //dtRepTitle.Columns.Add("单位");
                    //if (ds.Tables["dtRepTitle"].Rows.Count > 0) 
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepTitle"].Rows)
                    //    {
                    //        DataRow newRow = dtRepTitle.NewRow();
                    //        newRow["姓名"] = row["REALNAME"];
                    //        newRow["性别"] = row["SEX"];
                    //        newRow["年龄"] = row["AGE"];
                    //        newRow["检验编号"] = row["ORDERNUM"];
                    //        newRow["报告日期"] = row["FINISHDATE"];
                    //        newRow["单位"] = row["CUSTOMERNAME"];
                    //        dtRepTitle.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepTitleresult = JsonConvert.SerializeObject(ds.Tables["dtRepTitle"], new DataTableConverter());



                    #endregion

                    #region>>>>总体评价
                    ////总体评价
                    //DataTable dtOrderresultcomment = new DataTable();
                    //dtOrderresultcomment.Columns.Add("评价内容与建议");
                    //if (ds.Tables["dtOrderresultcomment"].Rows.Count > 0) 
                    //{
                    //    foreach (DataRow row in ds.Tables["dtOrderresultcomment"].Rows)
                    //    {
                    //        DataRow newRow = dtOrderresultcomment.NewRow();
                    //        newRow["评价内容与建议"] = row["RESULTCOMMENT"];
                    //        dtOrderresultcomment.Rows.Add(newRow);
                    //    }
                    //}
                    dtOrderresultcommentresult = JsonConvert.SerializeObject(ds.Tables["dtOrderresultcomment"], new DataTableConverter());
                    #endregion

                    #region>>>>>本次体检结果
                    ////本次体检结果
                    //DataTable dtRepImportantSigns = new DataTable();
                    //dtRepImportantSigns.Columns.Add("项目名称");
                    //dtRepImportantSigns.Columns.Add("本次检测结果");
                    //dtRepImportantSigns.Columns.Add("提示");
                    //dtRepImportantSigns.Columns.Add("单位");
                    //dtRepImportantSigns.Columns.Add("参考范围");
                    //if (ds.Tables["dtRepImportantSigns"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepImportantSigns"].Rows)
                    //    {
                    //        DataRow newRow = dtRepImportantSigns.NewRow();
                    //        newRow["项目名称"] = row["TESTNAME"];
                    //        newRow["本次检测结果"] = row["TESTRESULT"];
                    //        newRow["提示"] = row["HLFLAG"];
                    //        newRow["单位"] = row["UNIT"];
                    //        newRow["参考范围"] = row["TEXTSHOW"];
                    //        dtRepImportantSigns.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepImportantSignsresult = JsonConvert.SerializeObject(ds.Tables["dtRepImportantSigns"], new DataTableConverter());
                    #endregion

                    #region>>>>>解读与建议
                    ////解读与建议
                    //DataTable dtRepDiseaseGuide = new DataTable();
                    //dtRepDiseaseGuide.Columns.Add("解读项目");
                    //dtRepDiseaseGuide.Columns.Add("医学解释");
                    //dtRepDiseaseGuide.Columns.Add("病因提示");
                    //dtRepDiseaseGuide.Columns.Add("本次检验建议");
                    //if (ds.Tables["dtRepDiseaseGuide"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepDiseaseGuide"].Rows)
                    //    {
                    //        DataRow newRow = dtRepDiseaseGuide.NewRow();
                    //        newRow["解读项目"] = row["DIAGNOSISNAME"];
                    //        newRow["医学解释"] = row["DISEASEDESCRIPTION"];
                    //        newRow["病因提示"] = row["DISEASECAUSE"];
                    //        newRow["本次检验建议"] = row["SUGGESTION"];
                    //        dtRepDiseaseGuide.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepDiseaseGuideresult = JsonConvert.SerializeObject(ds.Tables["dtRepDiseaseGuide"], new DataTableConverter());
                    #endregion

                    #region>>>>>历史检测比对
                    ////历史检测比对
                    //DataTable dtRepExamCompared = new DataTable();
                    //dtRepExamCompared.Columns.Add("日期");
                    //dtRepExamCompared.Columns.Add("项目名称");
                    //dtRepExamCompared.Columns.Add("历史检测结果");
                    //if (ds.Tables["dtRepExamCompared"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepExamCompared"].Rows)
                    //    {
                    //        DataRow newRow = dtRepExamCompared.NewRow();
                    //        newRow["日期"] = row["FIVETESTDATE"];
                    //        newRow["项目名称"] = row["TESTNAME"];
                    //        newRow["历史检测结果"] = row["FIVETESTRESULT"];
                    //        dtRepExamCompared.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepExamComparedresult = JsonConvert.SerializeObject(ds.Tables["dtRepExamCompared"], new DataTableConverter());
                    #endregion
                    string strRepTitleresult = dtRepTitleresult.Substring(0, dtRepTitleresult.Length - 2);
                    strjson = "{\"1|\":" + strRepTitleresult + ",\"CommentResult\":" + dtOrderresultcommentresult + ",\"TestResult\":" + dtRepImportantSignsresult + ",\"DiseaseGuideresult\":"
                    + dtRepDiseaseGuideresult + ",\"Comparedresult\":" + dtRepExamComparedresult + "}]}";

                }
                else if (type == "BG_1006")
                {
                    //获取C14报告相关信息
                    ds = rservice.GetReportDataByType(dr["ordernum"].ToString(), type);

                    string dtRepTitleresult = "";//基本信息
                    string dtRepImportantSignsresult = "";//本次体检结果

                    #region>>>>>基本信息
                    ////基本信息
                    //DataTable dtRepTitle = new DataTable();
                    //dtRepTitle.Columns.Add("姓名");
                    //dtRepTitle.Columns.Add("性别");
                    //dtRepTitle.Columns.Add("年龄");
                    //dtRepTitle.Columns.Add("检验编号");
                    //dtRepTitle.Columns.Add("报告日期");
                    //dtRepTitle.Columns.Add("单位");
                    //if (ds.Tables["dtRepTitle"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepTitle"].Rows)
                    //    {
                    //        DataRow newRow = dtRepTitle.NewRow();
                    //        newRow["姓名"] = row["REALNAME"];
                    //        newRow["性别"] = row["SEX"];
                    //        newRow["年龄"] = row["AGE"];
                    //        newRow["检验编号"] = row["ORDERNUM"];
                    //        newRow["报告日期"] = row["FINISHDATE"];
                    //        newRow["单位"] = row["CUSTOMERNAME"];
                    //        dtRepTitle.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepTitleresult = JsonConvert.SerializeObject(ds.Tables["dtRepTitle"], new DataTableConverter());
                    #endregion

                    #region>>>>>本次体检结果
                    ////本次体检结果
                    //DataTable dtRepImportantSigns = new DataTable();
                    //dtRepImportantSigns.Columns.Add("项目名称");
                    //dtRepImportantSigns.Columns.Add("本次检测结果");
                    //dtRepImportantSigns.Columns.Add("检验方法");
                    //dtRepImportantSigns.Columns.Add("提示");
                    //dtRepImportantSigns.Columns.Add("单位");
                    //dtRepImportantSigns.Columns.Add("参考范围");
                    //if (ds.Tables["dtRepImportantSigns"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in ds.Tables["dtRepImportantSigns"].Rows)
                    //    {
                    //        DataRow newRow = dtRepImportantSigns.NewRow();
                    //        newRow["项目名称"] = row["TESTNAME"];
                    //        newRow["本次检测结果"] = row["TESTRESULT"];
                    //        newRow["提示"] = row["HLFLAG"];
                    //        newRow["单位"] = row["UNIT"];
                    //        newRow["参考范围"] = row["TEXTSHOW"];
                    //        newRow["检验方法"] = row["testmethod"];
                    //        dtRepImportantSigns.Rows.Add(newRow);
                    //    }
                    //}
                    dtRepImportantSignsresult = JsonConvert.SerializeObject(ds.Tables["dtRepImportantSigns"], new DataTableConverter());
                    #endregion

                    string strRepTitleresult = dtRepTitleresult.Substring(0, dtRepTitleresult.Length - 2);
                    strjson = "{\"1|\":" + strRepTitleresult + ",\"TestResult\":" + dtRepImportantSignsresult + "}]}";
                }
                return strjson;
            }
            catch (Exception ex)
            {
                return "{" + String.Format("\"0|\":{0} {1}", "\"其它原因", ex.Message) + "\"}";
            }
        }

        #region private method
        /// <summary>MD5加密
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string createMD5code(string str)
        {
            string pwd=System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            return pwd;
        }

        /// <summary>将文件流转换成Base64编码字符串
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static string byteTOBase64String(byte[] stream)
        {
            return Convert.ToBase64String(stream);
        }

        private static string StringToXML(object str)
        {
            return str.ToString().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos; ", "'").Replace("&quot;", "\"");
        }

        /// <summary>XML转化为DataTable
        /// 
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);

            XmlNodeList xlist = doc.SelectNodes("//data/datarow");
            if (xlist.Count == 0) return null;
            DataTable Dt = new DataTable();
            DataRow Dr;

            for (int i = 0; i < xlist.Count; i++)
            {
                Dr = Dt.NewRow();
                XmlElement xe = (XmlElement)xlist.Item(i);
                for (int j = 0; j < xe.ChildNodes.Count; j++)
                {
                    if (!Dt.Columns.Contains(xe.ChildNodes.Item(j).Name))
                        Dt.Columns.Add(xe.ChildNodes.Item(j).Name);
                    Dr[xe.ChildNodes.Item(j).Name] = xe.ChildNodes.Item(j).InnerText;
                }
                Dt.Rows.Add(Dr);
            }
            return Dt;
        }

        private static string ConvertDataTableToXML(DataTable xmlDS) 
        {         
            TextWriter tw = new StringWriter();
            try
            {
                xmlDS.WriteXml(tw);
                string xml = tw.ToString();
                return xml;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (tw != null)
                    tw.Close();
            }
        } 
        #endregion

        /// <summary>String字符串转换为Byte[]
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] stringToByteArray(string str)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            return byteArray;
        }
    }
}