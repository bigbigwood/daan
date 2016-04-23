using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using daan.domain;
using System.Configuration;
using daan.service.dict;
using daan.util.Common;
using daan.service.login;
using daan.service.proceed;

namespace daan.webservice.phy
{
    public class Utils
    {
        static readonly LoginService loginservice = new LoginService();
        static readonly ProRegisterService registerservice = new ProRegisterService();
        static readonly OrderbarcodeService barcodeservice = new OrderbarcodeService();
        static readonly DictCustomerService customerservice = new DictCustomerService();
     
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="cacheinfo"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static string ValidateLogin(out CacheInfo cacheinfo, string UserCode, string PassWord)
        {
            //daan123456   daanservice123456
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

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="SID"></param>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static string ReceiveXMLData(string SID, string xmlStr)
        {
            xmlStr = xmlStr.TrimStart('﻿');
//            if (xmlStr == string.Empty)
//            {
//                xmlStr = @"
//                        &lt;data&gt;&lt;datarow&gt;
//                        &lt;uniquecode &gt;TY0008&lt;/uniquecode &gt;
//                        &lt;dictcustomerid&gt;1396&lt;/dictcustomerid&gt;
//                        &lt;barcode&gt;380003772900&lt;/barcode&gt;
//                        &lt;realname&gt;马昌武3&lt;/realname&gt;
//                        &lt;sex&gt;男&lt;/sex&gt;
//                        &lt;birthday&gt;&lt;/birthday&gt;
//                        &lt;age&gt;41&lt;/age&gt;
//                        &lt;ismarried&gt;未知&lt;/ismarried&gt;
//                        &lt;mobile&gt;13096786113&lt;/mobile&gt;
//                        &lt;idnumber&gt;&lt;/idnumber&gt;
//                        &lt;address&gt;地址&lt;/address&gt;
//                        &lt;section&gt;遵义&lt;/section&gt;
//                        &lt;remark&gt;备注&lt;/remark&gt;
//                        &lt;phone&gt;&lt;/phone&gt;
//                        &lt;email&gt;&lt;/email&gt;
//                        &lt;samplingdate&gt;2014-12-18&lt;/samplingdate&gt;
//                        &lt;province&gt;广东&lt;/province&gt;
//                        &lt;city&gt;广州市&lt;/city&gt;
//                        &lt;county&gt;天河区&lt;/county&gt;
//                        &lt;dictlabid&gt;3&lt;/dictlabid&gt;
//                        &lt;/datarow&gt;&lt;/data&gt;
//                        ";
//            }
            string str = "<?xml version='1.0' encoding='utf-8'?>" + StringToXML(xmlStr);
            string strMessage = string.Empty;
            //缓存取登录用户
            Cache cache = new Cache();
            CacheInfo info = cache.GetCacheData(SID);
            DataTable dt = new DataTable();
            try
            {
                dt = GetDataTable(str);
            }
            catch (Exception ex)
            {
                return String.Format("{0} {1}", ErrorCode.Rec_1002, ex.Message);
            }
            if (dt == null || dt.Rows.Count == 0||dt.Columns.Count!=20) { return ErrorCode.Rec_1003; }

            List<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemList();//项目字典表
            List<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetail();//套餐组合字典
            string _productname = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                #region 必填项
                //套餐代码
                string productTestCode = dr["uniquecode"].ToString().Replace('_', ' ').Trim();
                //客户代码
                string dictcustomercode = dr["dictcustomerid"].ToString().Trim();
                //条码号
                string barcode = dr["barcode"].ToString().Trim();
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

                if (string.IsNullOrEmpty(productTestCode) || string.IsNullOrEmpty(dictcustomercode) || string.IsNullOrEmpty(dictlabid)
                    || string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(ismarried)
                    || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(province) || string.IsNullOrEmpty(city))
                {
                    strMessage = ErrorCode.Rec_1005;
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
                    strMessage = ErrorCode.Rec_1006;
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
                if ((birthday!=string.Empty&&!datebirthdayb) || (samplingdate!=string.Empty&&!datesamplingdateb))
                {
                    strMessage = ErrorCode.Rec_1013;
                    break;
                }
                //检查单位是否在体检系统中有维护
                string dictcustomerid = string.Empty;
                try
                {
                    using (DataTable d = customerservice.CheckHasCustomer(dictcustomercode))
                    {
                        if (d == null || d.Rows.Count == 0 || d.Rows.Count > 1)
                        {
                            strMessage = ErrorCode.Up_0011;
                            break;
                        }
                        else
                        {
                            dictcustomerid = d.Rows[0][0].ToString();
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
                    strMessage = ErrorCode.Rec_1008;
                    break;
                }
                else if (productList.Count > 1)
                {
                    strMessage = ErrorCode.Rec_1009;
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
                        strMessage = ErrorCode.Rec_1017;
                        break; 
                    }
                    #endregion
                    if (barcode != string.Empty && barcode.Length != 12)//条码号必须为12位数字
                    {
                        strMessage = ErrorCode.Rec_1010;
                        break;
                    }
                    if (barcode != string.Empty && barcode.Substring(barcode.Length - 2) != "00")//条码号必须以00结尾
                    {
                        strMessage = ErrorCode.Rec_1011;
                        break;
                    }
                    if (barcodeservice.CheckBarCode(barcode))//条码号已在系统中存在
                    {
                        strMessage = ErrorCode.Rec_1012;
                        break;
                    }

                    //套餐下组合项目
                    IEnumerable<Dictproductdetail> IEgroup = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid);
                    bool iscontinue = true;
                    int count=IEgroup.Count<Dictproductdetail>();
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
                            strMessage = ErrorCode.Rec_1015;
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
                        if (k>=count)
                        {
                            strMessage = ErrorCode.Rec_1016;
                            break;
                        }
                    }
                }
                #endregion

                #region 添加会员
                Dictmember member = new Dictmember() { Realname = realname,Idnumber=idnumber,Nickname=realname,Sex=sex,Addres=address,
                    Phone=phone,Mobile=mobile,Email=email};
                if (datebirthdayb) member.Birthday = datebirthday;
                registerservice.checkmember(null, ref member);
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
                        strMessage = ErrorCode.Rec_1006;
                        break;
                    }
                }
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(member.Birthday);//时间差
                year = Math.Truncate((double)(ts.Days / 365));
                month = (ts.Days % 365) / 30;
                day = (ts.Days % 365) % 30;
                hours = ts.TotalHours;

                Orders _orders = new Orders() { 
                    Ordernum = new ProRegisterService().GetOrderNum(), 
                    Dictmemberid = member.Dictmemberid, Dictcustomerid = Convert.ToDouble(dictcustomerid), 
                    Realname = realname, Sex = sex, Caculatedage = hours, Remarks = remark, 
                    Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, 0), Enterby = "admin", 
                    Ordertestlst = _productname + ",", Dictlabid = Convert.ToDouble(dictlabid), Ordersource = "1", 
                    Ismarried = ismarried == "未婚" ? "0" : (ismarried == "已婚" ? "1" : "2"), Section = section, 
                    Status = ((int)daan.service.common.ParamStatus.OrdersStatus.BarCodePrint).ToString(), 
                    Province = province, City = city, County = county };
                if (datesamplingdateb) _orders.SamplingDate = datesamplingdate;
                string errstr = string.Empty;
                bool b = registerservice.insertUpdateOrders("易感基因对接订单", "", true, productList, grouptestList, member, _orders, "", ref errstr);
                if (!b) strMessage = String.Format("{0} {1}", ErrorCode.Rec_1018, errstr);
                #endregion
            } 
            return strMessage;
        }
        #region >>>> 转化
        /// <summary>
        /// XML转化为DataTable
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

        private static string StringToXML(object str)
        {
            return str.ToString().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos; ", "'").Replace("&quot;", "\"");
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string createMD5code(string str)
        { 
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");  
        }
        #endregion
    }
}