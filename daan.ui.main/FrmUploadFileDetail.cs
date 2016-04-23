using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using daan.domain;
using daan.service.login;
using daan.service.dict;
using daan.service.proceed;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using daan.service.common;
using daan.service.order;
using daan.util.Common;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace daan.ui.main
{
    public partial class FrmUploadFileDetail : Form
    {
        readonly LoginService loginservice = new LoginService();
        readonly ProRegisterService registerservice = new ProRegisterService();
        readonly OrderbarcodeService barcodeservice = new OrderbarcodeService();
        readonly OrderfileheaderService headerservice = new OrderfileheaderService();
        readonly OrderfiledetailService detailservice = new OrderfiledetailService();

        public FrmUploadFileDetail()
        {
            InitializeComponent();
            this.tbTime.Text = "10";

            this.btnStop.Enabled = false;
        }

        public bool _IsRunning = false;

        private int outTime
        {
            get
            {
                if (!string.IsNullOrEmpty(this.tbTime.Text) && this.tbTime.Text != "0")
                {
                    return Convert.ToInt32(this.tbTime.Text);
                }
                else
                    return 5;
            }
        }

        /// <summary>
        /// 开启线程  在线程中定时获取接口数据
        /// </summary>
        public void ThreadStart()
        {
            if (_IsRunning)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MainDeposit));
            }
        }

        /// <summary>
        /// 线程主要执行程序
        /// </summary>
        /// <param name="obj"></param>
        private void MainDeposit(object obj)
        { 
             string msg = string.Empty;
             while (this._IsRunning)
             {
                 #region 导入表格数据
                 try
                 {
                     using (DataTable dt = headerservice.GetFrmUploadFileName())
                     {
                         string strmessage = string.Empty;
                         if (dt == null || dt.Rows.Count == 0)
                         {
                             //strmessage = string.Format("{0}:未扫描到新上传的文件", DateTime.Now);
                             //SetTB(strmessage);
                             Thread.Sleep(outTime * 1000);
                             continue;
                         }
                         foreach (DataRow dr in dt.Rows)
                         {
                             Orderfileheader fileheader = new Orderfileheader() { Orderfileheaderid = Convert.ToDouble(dr["Orderfileheaderid"]), Status = 2 };
                             string fileName = dr["filename"] == DBNull.Value ? string.Empty : dr["filename"].ToString();
                             if (string.IsNullOrEmpty(fileName))
                             {
                                 strmessage = string.Format("{0}:没有相关的文件路径", DateTime.Now);
                                 SetTB(strmessage);
                                 headerservice.UpdateOrderfileheader(fileheader);
                                 continue;
                             }
                             DataTable dtt = RenderDataTableFromExcel(fileName);
                             if (dtt == null || dtt.Rows.Count == 0)
                             {
                                 strmessage = string.Format("{0}:未获取到Excl文档上的文件数据或服务器上找不到上传的Excel文件！", DateTime.Now);
                                 SetTB(strmessage);
                                 headerservice.UpdateOrderfileheader(fileheader);
                                 continue;
                             }
                             try
                             {
                                 if (AutoUploadFile(dtt, dr))
                                 {
                                     strmessage = String.Format("{0}: 文件名: {1}扫描完毕。", DateTime.Now, dr["filename"]);
                                     SetTB(strmessage);
                                     dtt.Dispose();  //释放资源
                                 }
                             }
                             catch (Exception ex)
                             {
                                 strmessage = String.Format("{0}: 文件名: {1}扫描失败。原因：{2}", DateTime.Now, dr["filename"], ex.Message);
                                 SetTB(strmessage);
                                 headerservice.UpdateOrderfileheader(fileheader);
                                 continue;
                             }
                         }
                         dt.Dispose();   //释放资源
                     }
                 }
                 catch (Exception ex)
                 {
                     string strmessage = String.Format("{0}:  {1}", DateTime.Now, ex.Message);
                     SetTB(strmessage);
                     CreateErrorLog(strmessage);
                 }
                 Thread.Sleep(outTime * 1000);
                 #endregion
             }
        }

        private bool AutoUploadFile(DataTable dt, DataRow headerDr)
        {
            double? Orderfileheaderid = Convert.ToDouble(headerDr["Orderfileheaderid"]);
            double? enterby = Convert.ToDouble(headerDr["enterby"]);
            double? dictcustormer = Convert.ToDouble(headerDr["dictcustormer"]);
            double? dictlabid = Convert.ToDouble(headerDr["dictlabid"]);
            string province = headerDr["province"].ToString();
            string city = headerDr["city"].ToString();
            string county = headerDr["county"].ToString();
            bool isunifiedpost = false;
            if (headerDr["isunifiedpost"].ToString() == "1")
                isunifiedpost = true;
            string postaddress = headerDr["postaddress"].ToString();
            string recipient = headerDr["recipient"].ToString();
            string contactnumber = headerDr["contactnumber"].ToString();

            DictuserService userService = new DictuserService();
            string username = userService.GetDictuserInfoAuto(enterby).Username;
            Orderfiledetail filedetail = new Orderfiledetail();
            Orderfileheader fileheader = new Orderfileheader() { Orderfileheaderid = Orderfileheaderid, Status = 1 };

            //bool isCacheData = true;
            //string conTestCode = ConfigurationManager.AppSettings["NoCacheTestCode"];
            //List<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemListNoCache();//项目字典表
            List<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetailNoCache();//套餐组合字典
            List<Dicttestitem> productlistTemp = new DicttestitemService().GetProduct(TypeParse.StrToDouble(dictcustormer, 0));//查询分点+公用套餐
            string _productname = string.Empty;
            for (int i = (dt.Rows.Count - 1); i >= 0; i--)
            {
                bool b = false;//添加是否成功
                string errstr = "";
                DataRow dr = dt.Rows[i];

                string productTestCode = dr["套餐代码"].ToString().Replace('_', ' ').Trim();
                string detailbarcode = string.Empty;
                try
                {
                    detailbarcode = dr["条码号"].ToString().Replace('_', ' ').Trim();
                    if (detailbarcode != string.Empty)
                        Convert.ToDouble(detailbarcode);
                }
                catch (Exception) { }
                string mobile = dr["手机"].ToString().Trim();
                string idnumber = dr["身份证"].ToString().Trim();
                string realname = dr["姓名"].ToString().Trim();
                if (string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(productTestCode))
                {
                    filedetail.Reason = "姓名、套餐代码不可以为空！";
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }

                #region 条码号检查
                if (detailbarcode != string.Empty && detailbarcode.Length != 12)//条码号非12位
                {
                    filedetail.Reason = string.Format("条码号[{0}]必须为12位数字，可以为空！", detailbarcode);
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                if (detailbarcode != string.Empty && detailbarcode.Substring(detailbarcode.Length - 2) != "00")//条码号不以00结尾
                {
                    filedetail.Reason = string.Format("此条码号[{0}]不是以00结尾，请更改条码号！", detailbarcode);
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                if (barcodeservice.CheckBarCode(detailbarcode))//条码号存在
                {
                    filedetail.Reason = string.Format("此条码号[{0}]已在本系统内生成，请更改条码号！", detailbarcode);
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                #endregion

                string sex = "U";
                if (dr["性别"] != DBNull.Value && !string.IsNullOrEmpty(dr["性别"].ToString()))
                {
                    if (dr["性别"].ToString() == "女")
                        sex = "F";
                    else if (dr["性别"].ToString() == "男")
                        sex = "M";
                }   

                List<Dicttestitem> productList = productlistTemp.Where<Dicttestitem>(c => c.Testcode == productTestCode && (c.Forsex.ToUpper() == sex.ToUpper() || c.Forsex.ToUpper() == "B")).ToList<Dicttestitem>();
                List<Dicttestitem> grouptestList = new List<Dicttestitem>();
                Dicttestitem productinfo = null;
                if (productList.Count == 0)
                {
                    filedetail.Reason = String.Format("套餐代码[{0}]无匹配项，请查看性别是否匹配或者是否有该套餐。", productTestCode);
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                else if (productList.Count > 1)
                {
                    filedetail.Reason = String.Format("存在多个套餐代码为[{0}]的套餐", productTestCode);
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                else
                {
                    productinfo = productList[0];
                    if (productinfo.Testtype=="2")//公用套餐
                        _productname = productinfo.Testname.ToString().Replace("(公用套餐)","");
                    else
                        _productname = productinfo.Testname;

                    //检验套餐中项目组合信息（性别是否相符；是否重复添加项目组合；项目是否维护分管原则、科室和标本类型）
                    string msg = registerservice.AddProductAuto(sex, productinfo,detailbarcode);
                    if (msg != string.Empty)
                    {
                        filedetail.Reason = msg;
                        filedetail.Status = 0;
                        filedetail.Barcode = detailbarcode;
                        filedetail.Orderfileheaderid = Orderfileheaderid;
                        filedetail.Createdate = DateTime.Now;
                        filedetail.Realname = realname;
                        filedetail.Mobile = mobile;
                        filedetail.Idnumber = idnumber;
                        detailservice.InsertOrderfiledetail(filedetail);
                        continue;
                    }

                    //套餐下组合项目
                    IEnumerable<Dictproductdetail> IEgroup = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid);

                    bool iscontinue = true;
                    string msgdetail = string.Empty;
                    foreach (Dictproductdetail item in IEgroup)
                    {
                        Dicttestitem groupinfo = registerservice.SelectDicttestitemByDicttestitemid(item.Testgroupid);
                        if (groupinfo == null)
                        {
                            msgdetail += string.Format("没有找到套餐[{0}]下ID为[{1}]的{2}[{3}]！", _productname, item.Testgroupid, groupinfo.Testtype == "0" ? "单项" : "组合", groupinfo.Testname)+"；";
                            iscontinue = false;
                            continue;
                        }
                        groupinfo.Productid = productinfo.Dicttestitemid;
                        groupinfo.Productname = productinfo.Testname;///套餐名
                        groupinfo.IsActive = "1";//是否停止测试
                        groupinfo.Isadd = "0";///是否追加 
                        groupinfo.Billed = "0";
                        groupinfo.Sendbilled = "0";
                        groupinfo.Adduserid = null;//追加人ID

                        if (detailbarcode == string.Empty)
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
                            groupinfo.Barcode = detailbarcode;
                        }
                        //获取外包客户
                        Dictproductdetail detail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid && c.Testgroupid == groupinfo.Dicttestitemid).First<Dictproductdetail>();
                        groupinfo.Sendoutcustomerid = detail.Sendoutcustomerid;
                        grouptestList.Add(groupinfo);
                    }
                    if (!iscontinue)
                    {
                        if (!string.IsNullOrEmpty(msgdetail))
                        {
                            filedetail.Reason = msgdetail;
                            filedetail.Status = 0;
                            filedetail.Barcode = detailbarcode;
                            filedetail.Orderfileheaderid = Orderfileheaderid;
                            filedetail.Createdate = DateTime.Now;
                            filedetail.Realname = realname;
                            filedetail.Mobile = mobile;
                            filedetail.Idnumber = idnumber;
                            detailservice.InsertOrderfiledetail(filedetail);
                        }
                        continue; 
                    }
                }

                #region >>>>  不存在此会员添加会员

                Dictmember member = new Dictmember() { Realname = realname, Idnumber = idnumber };
                //检查会员
                errstr = registerservice.checkmember(null, ref member);
                if (errstr != string.Empty)
                {
                    filedetail.Reason = errstr;
                    filedetail.Status = 0;
                    filedetail.Barcode = detailbarcode;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                    continue;
                }
                member.Nickname = member.Realname;
                member.Sex = sex;
                DateTime datebirthday;
                bool dateb = DateTime.TryParse(dr["出生日期"].ToString(), out datebirthday);
                if (dateb) { member.Birthday = datebirthday; }
                member.Addres = dr["住址"].ToString().Trim();
                member.Phone = dr["电话"].ToString().Trim();

                if (!string.IsNullOrWhiteSpace(dr["手机"].ToString().Trim()))
                {
                    double a;
                    bool mobileb = double.TryParse(dr["手机"].ToString().Trim(), out a);
                    if (!mobileb)
                    {
                        filedetail.Reason = "手机号码填写不正确,不要有特殊字符，[-]也不能包含";
                        filedetail.Barcode = detailbarcode;
                        filedetail.Status = 0;
                        filedetail.Orderfileheaderid = Orderfileheaderid;
                        filedetail.Createdate = DateTime.Now;
                        filedetail.Realname = realname;
                        filedetail.Mobile = mobile;
                        filedetail.Idnumber = idnumber;
                        detailservice.InsertOrderfiledetail(filedetail);
                        continue;
                    }
                }
                member.Mobile = dr["手机"].ToString().Trim();
                member.Email = dr["邮箱"].ToString().Trim();
                #endregion

                #region >>>>  insert Orders

                double year = 0, month = 0, day = 0;
                double hours = 0;//小时
                double age = 0;
                string agestr = dr["年龄"].ToString().Replace('_', ' ').Trim();
                string ageFiled = string.Empty;
                bool ageb = double.TryParse(agestr, out age);
                if (member.Birthday == null)
                {
                    if (agestr != string.Empty && ageb)
                    {
                        year = age;
                        day = age * 365;
                        member.Birthday = datebirthday = DateTime.Now.AddDays((0 - day));
                    }
                    else
                    {
                        filedetail.Reason = "生日和年龄必须填写一项或者两项均填写错误";
                        filedetail.Barcode = detailbarcode;
                        filedetail.Status = 0;
                        filedetail.Orderfileheaderid = Orderfileheaderid;
                        filedetail.Createdate = DateTime.Now;
                        filedetail.Realname = realname;
                        filedetail.Mobile = mobile;
                        filedetail.Idnumber = idnumber;
                        detailservice.InsertOrderfiledetail(filedetail);
                        continue;
                    }
                }
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(member.Birthday);//时间差
                year = Math.Truncate((double)(ts.Days / 365));
                month = (ts.Days % 365) / 30;
                day = (ts.Days % 365) % 30;
                hours = ts.TotalHours;
                ageFiled = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, 0); ;//年龄字符串拼接 岁月日时
               
                Orders _orders = new Orders();
                _orders.Ordernum = new ProRegisterService().GetOrderNum(); ;//体检流水号
                _orders.Remarks = dr["备注"].ToString().Trim();//备注
                _orders.Dictmemberid = member.Dictmemberid;  //会员ID
                _orders.Dictcustomerid = dictcustormer;//所属客户ID 界面选择
                _orders.Realname = member.Realname;
                _orders.Sex = member.Sex; //性别 对应INITBASIC表
                _orders.Caculatedage = hours;//计算后的年龄（小时为单位）
                _orders.Age = ageFiled;
                _orders.Enterby = username;//录入人
                _orders.Ordertestlst = _productname + ",";//项目清单（冗余字段）
                _orders.Dictlabid = dictlabid;//实验室分点
                _orders.Ordersource = "1";//单位上传 全是单位来源
                _orders.Ismarried = dr["婚否"].ToString() == "未婚" ? "0" : (dr["婚否"].ToString() == "已婚" ? "1" : "2");
                _orders.Section = dr["部门"].ToString().Trim();
                _orders.Status = ((int)ParamStatus.OrdersStatus.BarCodePrint).ToString();
                DateTime samplingdate;
                bool s = DateTime.TryParse(dr["采样日期"].ToString(), out samplingdate);
                if (s) { _orders.SamplingDate = samplingdate; }

                _orders.Province = province;
                _orders.City = city;
                _orders.County = county;
                if (isunifiedpost)
                {
                    _orders.PostAddress = postaddress;
                    _orders.Recipient = recipient;
                    _orders.ContactNumber = contactnumber;
                }
                else
                {
                    _orders.PostAddress = dr["住址"].ToString().Trim();
                    _orders.Recipient = realname;
                    _orders.ContactNumber = dr["手机"].ToString().Trim();
                }
                #endregion

                System.Collections.Hashtable htScan = new System.Collections.Hashtable();
                htScan.Add("isScan", true);
                htScan.Add("EnterByID", enterby);
                htScan.Add("EnterBy", username);
                b = registerservice.insertUpdateOrdersAuto("单位批量上传", "", true, productList, grouptestList, member, _orders, "", ref errstr, htScan);
                if (b)
                {
                    filedetail.Barcode = detailbarcode;
                    filedetail.Reason = "";
                    filedetail.Status = 1;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                }
                else
                {
                    filedetail.Barcode = detailbarcode;
                    filedetail.Reason = errstr + " 【Excel格式参照导入模版说明】";
                    filedetail.Status = 0;
                    filedetail.Orderfileheaderid = Orderfileheaderid;
                    filedetail.Createdate = DateTime.Now;
                    filedetail.Realname = realname;
                    filedetail.Mobile = mobile;
                    filedetail.Idnumber = idnumber;
                    detailservice.InsertOrderfiledetail(filedetail);
                }
            }
            return headerservice.UpdateOrderfileheader(fileheader);
        }

        /****
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="headerDr"></param>
        private bool AutoUploadFile1(DataTable dt, DataRow headerDr)
        {
            double? Orderfileheaderid = Convert.ToDouble(headerDr["Orderfileheaderid"]);
            double? enterby = Convert.ToDouble(headerDr["enterby"]);
            double? dictcustormer = Convert.ToDouble(headerDr["dictcustormer"]);
            double? dictlabid = Convert.ToDouble(headerDr["dictlabid"]);
            string province = headerDr["province"].ToString();
            string city = headerDr["city"].ToString();
            string county = headerDr["county"].ToString();
            bool isunifiedpost = false;
            if (headerDr["isunifiedpost"].ToString() == "1")
                isunifiedpost = true;
            string postaddress = headerDr["postaddress"].ToString();
            string recipient = headerDr["recipient"].ToString();
            string contactnumber = headerDr["contactnumber"].ToString();

            DictuserService userService = new DictuserService();
            string username = userService.GetDictuserInfoAuto(enterby).Username;

            Orderfileheader fileheader = new Orderfileheader(){Orderfileheaderid = Orderfileheaderid,Status = 1};

            IList<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemListNoCache();//项目字典表
            IList<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetailNoCache();//套餐组合字典

            string error = string.Empty;
            IList<Dicttestitem> productlistTemp = new DicttestitemService().GetProduct(TypeParse.StrToDouble(dictcustormer, 0));
            Dicttestitem productinfo = null;

            
            IList<Orderfiledetail> list = new List<Orderfiledetail>();
            if (!dt.Columns.Contains("Error"))
            {
                dt.Columns.Add("Error");
            }
            foreach (DataRow dr in dt.Rows)
            {
                List<Dicttestitem> grouptestList = new List<Dicttestitem>();
                string productTestCode = dr["套餐代码"] == DBNull.Value ? string.Empty : dr["套餐代码"].ToString().Replace("_", "").Trim();
                string detailbarcode = dr["条码号"] == DBNull.Value ? string.Empty : dr["条码号"].ToString().Replace('_', ' ').Trim();

                string realname = dr["姓名"] == DBNull.Value ? string.Empty : dr["姓名"].ToString();
                string idnumber = dr["身份证"] == DBNull.Value ? string.Empty : dr["身份证"].ToString();

                Regex reMobile = new Regex("(^18\\d{9}$)|(^13\\d{9}$)|(^15\\d{9})|(^14\\d{9})|(^17\\d{9}$)");    //验证手机号码的正则表达式
                Regex reNumber = new Regex("^[0-9]*$");  //验证是否为全数字的  正则表达式
                string mobile = dr["手机"] == DBNull.Value ? string.Empty : dr["手机"].ToString();

                string _productname = string.Empty;
                if (string.IsNullOrEmpty(productTestCode))
                {
                    error += "套餐代码不可以为空；";
                }
                if (string.IsNullOrEmpty(realname))
                {
                    error += "姓名不可以为空；";
                }
                if (!string.IsNullOrEmpty(detailbarcode))
                {
                    if (detailbarcode.Length != 12 && !reNumber.IsMatch(detailbarcode))
                    {
                        error += "条码号：[" + detailbarcode + "]必须为12位的数字；";
                    }
                    if (detailbarcode.Substring(detailbarcode.Length - 2) != "00")  //条码号不以00结尾
                    {
                        error += "条码号：[" + detailbarcode + "] 最后两位不是00，请更改条码号；";
                    }
                }
                string sex = "U";
                if (dr["性别"] != DBNull.Value && !string.IsNullOrEmpty(dr["性别"].ToString()))
                {
                    if (dr["性别"].ToString() == "女")
                        sex = "F";
                    else if (dr["性别"].ToString() == "男")
                        sex = "M";
                }

                List<Dicttestitem> productList = productlistTemp.Where<Dicttestitem>(c => c.Testcode == productTestCode && (c.Forsex.ToUpper() == sex.ToUpper() || c.Forsex.ToUpper() == "B")).ToList<Dicttestitem>();
                if (productList.Count == 0)
                {
                    error += String.Format("套餐代码[{0}]无匹配项，请查看性别是否匹配或者是否有该套餐；", productTestCode);
                }
                else if (productList.Count > 1)
                {
                    error += String.Format("存在多个套餐代码为[{0}]的套餐；", productTestCode);
                }
                else
                {
                    List<OrderRegister> _gridtestList = null;
                    productinfo = productList[0];
                    string msg = registerservice.AddProductAuto(ref _gridtestList, sex, productinfo.Dicttestitemid, false, enterby, ref _productname, null,true);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        error += msg + "；";
                    }

                    #region 套餐下组合项目
                    //套餐下组合项目
                    IEnumerable<Dictproductdetail> IEgroup = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid);

                    foreach (Dictproductdetail item in IEgroup)
                    {
                        IEnumerable<Dicttestitem> IEgruptest = TestItemList.Where<Dicttestitem>(c => c.Dicttestitemid == item.Testgroupid);
                        if (IEgruptest.Count() <= 0)
                        {
                            error += string.Format("没有找到套餐[{0}]下ID为[{1}]的组合；", _productname, item.Testgroupid);
                            break;
                        }
                        Dicttestitem groupinfo = IEgruptest.First<Dicttestitem>();

                        //校验性别是否符合
                        string str = registerservice.checkSex(groupinfo.Dicttestitemid, sex);
                        if (!string.IsNullOrEmpty(str))
                        {
                            error += str + "；";
                            break;
                        }

                        groupinfo.Productid = productinfo.Dicttestitemid;
                        groupinfo.Productname = productinfo.Testname;///套餐名
                        groupinfo.IsActive = "1";//是否停止测试
                        groupinfo.Isadd = "0";///是否追加 
                        groupinfo.Billed = "0";
                        groupinfo.Sendbilled = "0";
                        groupinfo.Adduserid = null;//追加人ID

                        if (detailbarcode == string.Empty)
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
                            groupinfo.Barcode = detailbarcode;
                        }
                        //获取外包客户
                        Dictproductdetail detail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid && c.Testgroupid == groupinfo.Dicttestitemid).First<Dictproductdetail>();
                        groupinfo.Sendoutcustomerid = detail.Sendoutcustomerid;
                        grouptestList.Add(groupinfo);
                    }
                    #endregion
                }
                #region >>>>  不存在此会员添加会员
                Dictmember member = new Dictmember() { Realname = realname, Idnumber = idnumber };
                //检查会员
                string errstr = registerservice.checkmember(null, ref member);
                if (!string.IsNullOrEmpty(errstr))
                {
                    error += errstr + "；";
                }

                member.Nickname = member.Realname;
                member.Sex = sex;
                DateTime datebirthday;
                bool dateb = DateTime.TryParse(dr["出生日期"].ToString(), out datebirthday);
                if (dateb) { member.Birthday = datebirthday; }
                member.Addres = dr["住址"].ToString().Trim();
                member.Phone = dr["电话"].ToString().Trim();

                if (!string.IsNullOrEmpty(mobile))
                {
                    if (!reMobile.IsMatch(mobile))
                    {
                        error += "手机号码填写不正确,不要有特殊字符，[-]也不能包含；";
                    }
                }
                member.Mobile = mobile;
                member.Email = dr["邮箱"].ToString().Trim();
                double year = 0, month = 0, day = 0;
                double hours = 0;//小时
                double age = 0;
                string agestr = dr["年龄"].ToString().Replace('_', ' ').Trim();
                bool ageb = double.TryParse(agestr, out age);

                if (member.Birthday == null)
                {
                    if (agestr != string.Empty && ageb)
                    {
                        year = age;
                        day = age * 365;
                        member.Birthday = datebirthday = DateTime.Now.AddDays((0 - day));
                    }
                    else
                    {
                        error += "生日和年龄必须填写一项或者两项均填写错误；";
                    }
                }
                #endregion
                if (!string.IsNullOrEmpty(error))
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        dr["Error"] = error.TrimEnd('；');
                        error = string.Empty;
                    }
                }
                else
                {
                    #region 订单信息
                    Orders _orders = new Orders();
                    _orders.Ordernum = new ProRegisterService().GetOrderNum(); ;//体检流水号
                    _orders.Remarks = dr["备注"].ToString().Trim();//备注
                    _orders.Dictmemberid = member.Dictmemberid;  //会员ID
                    _orders.Dictcustomerid = dictcustormer;//所属客户ID 界面选择
                    _orders.Realname = member.Realname;
                    _orders.Sex = member.Sex; //性别 对应INITBASIC表
                    _orders.Caculatedage = hours;//计算后的年龄（小时为单位）
                    _orders.Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, 0); ;//年龄字符串拼接 岁月日时
                    _orders.Enterby = username;//录入人
                    _orders.Ordertestlst = _productname + ",";//项目清单（冗余字段）
                    _orders.Dictlabid = dictlabid;//实验室分点
                    _orders.Ordersource = "1";//单位上传 全是单位来源
                    _orders.Ismarried = dr["婚否"].ToString() == "未婚" ? "0" : (dr["婚否"].ToString() == "已婚" ? "1" : "2");
                    _orders.Section = dr["部门"].ToString().Trim();
                    _orders.Status = ((int)ParamStatus.OrdersStatus.BarCodePrint).ToString();
                    DateTime samplingdate;
                    bool s = DateTime.TryParse(dr["采样日期"].ToString(), out samplingdate);
                    if (s) { _orders.SamplingDate = samplingdate; }

                    _orders.Province = province;
                    _orders.City = city;
                    _orders.County = county;
                    if (isunifiedpost)
                    {
                        _orders.PostAddress = postaddress;
                        _orders.Recipient = recipient;
                        _orders.ContactNumber = contactnumber;
                    }
                    else
                    {
                        _orders.PostAddress = dr["住址"].ToString().Trim();
                        _orders.Recipient = realname;
                        _orders.ContactNumber = mobile;
                    }
                    #endregion
                    Hashtable htScan = new Hashtable();
                    htScan.Add("isScan", true);
                    htScan.Add("EnterByID", enterby);
                    htScan.Add("EnterBy", username);
                    bool result = registerservice.insertUpdateOrders("单位批量上传", "", true, productList, grouptestList, member, _orders, "", ref errstr, htScan);
                    if (!result)
                    {
                        error = " 【Excel格式参照导入模版说明】";

                        dr["Error"] = error.TrimEnd('；');
                    }
                }
               
                Orderfiledetail filedetail = this.GetOrderfiledetail(dr, Orderfileheaderid);
                list.Add(filedetail);
            }
            if (list.Count > 0)
            {
               return detailservice.InsertOrderfiledetail(list, fileheader);
            }
            return false;
        }
        ****/

        /// <summary>
        /// 根据数据行 获取数据对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Orderfiledetail GetOrderfiledetail(DataRow dr, double? Orderfileheaderid)
        {
            Orderfiledetail filedetail = new Orderfiledetail();
            filedetail.Barcode = dr["条码号"] != DBNull.Value ? string.Empty : dr["条码号"].ToString().Replace('_', ' ').Trim(); ;
            filedetail.Reason = dr["Error"] == DBNull.Value ? string.Empty : dr["Error"].ToString();
            filedetail.Status = 0;
            filedetail.Orderfileheaderid = Orderfileheaderid;
            filedetail.Createdate = DateTime.Now;
            filedetail.Realname = dr["姓名"] == DBNull.Value ? string.Empty : dr["姓名"].ToString();
            filedetail.Mobile = dr["手机"] == DBNull.Value ? string.Empty : dr["手机"].ToString().Trim();
            filedetail.Idnumber = dr["身份证"] == DBNull.Value ? string.Empty : dr["身份证"].ToString();
            return filedetail;
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="path">服务器excel文档路径</param>
        /// <returns></returns>
        public DataTable RenderDataTableFromExcel(string path)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return null;
            }
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);


                ISheet sheet = hssfworkbook.GetSheetAt(0);
                //System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    if (headerRow.GetCell(j) == null)
                    {
                        continue;
                    }
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());
                }
                dt.Columns.Add("上传状态", typeof(string));
                dt.Columns.Add("失败原因", typeof(string));

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row.GetCell(0) == null && row.GetCell(2) == null)
                    {
                        break;
                    }
                    else
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int j = row.FirstCellNum; j < (cellCount + 2); j++)
                        {
                            if (j == cellCount)
                            {
                                dataRow[j] = "未上传";
                            }
                            else
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }

        }
        private delegate void SetTBMethodInvok(string value);

        /// <summary>
        /// 写出返回的消息
        /// </summary>
        /// <param name="value"></param>
        private void SetTB(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new SetTBMethodInvok(SetTB), value);
            }
            else
            {
                if (tbxMessage == null || tbxMessage.IsDisposed)
                {
                    tbxMessage = new RichTextBox();
                }
                tbxMessage.Text += value + "\n";
                CreateErrorLog(value);
                if (tbxMessage.Text.Length > 3000)
                {
                    tbxMessage.Text = value + "\n";
                }
                //有滚动条时 ，定位到textbox最下方
                tbxMessage.SelectionStart = tbxMessage.Text.Length;
                tbxMessage.ScrollToCaret();
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBegan_Click(object sender, EventArgs e)
        {
            this._IsRunning = true;
            this.ThreadStart();

            btnBegan.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnStop.Enabled = true;
            string strmsg = string.Format(">>>系统已启动： {0}", DateTime.Now);
            SetTB(strmsg);

        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnBegan.Enabled = true;
            this.btnStop.Enabled = false;
            this.btnExit.Enabled = true;
            this._IsRunning = false;

            string strmsg = string.Format(">>>系统已停止： {0}", DateTime.Now);
            SetTB(strmsg);

        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            FrmTestitems frm = new FrmTestitems();
            frm.Dispose();
            string strmsg = string.Format(">>>退 出 时 间：{0}", DateTime.Now);
            SetTB(strmsg);
            Close();
        }



        /// <summary>
        /// 验证文本框只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 阻止从键盘输入键
            e.Handled = true;
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {
                if ((e.KeyChar == (char)8))
                {
                    e.Handled = false;
                    return;
                }
                else
                {
                    int len = tbTime.Text.Length;
                    if (len < 3)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false;
                            return;
                        }
                        else if (len == 0)
                        {
                            MessageBox.Show("不能以0为开头！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("只能输入3位数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            else
            {
                MessageBox.Show("只能输入数字！", "体检系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private readonly static string FileOrPath = Application.StartupPath + "\\Log\\UploadFile\\";
        private static string m_fileName = String.Format("{0}{1:yyyyMMdd}.txt", FileOrPath, DateTime.Now);
        public static String FileName
        {
            get { return (m_fileName); }
            set
            {
                if (value != null || value != "")
                { m_fileName = value; }
            }
        }
        /// <summary>      
        /// 记录日志至文本文件，每天保存一个日志文件
        /// </summary>    
        /// <param name="message">记录的内容</param>    
        public static void CreateErrorLog(string message)
        {
            if (!Directory.Exists(FileOrPath))//若文件夹不存在则新建文件夹
            {
                Directory.CreateDirectory(FileOrPath); //新建文件夹
            }
            m_fileName = String.Format("{0}{1:yyyyMMdd}.txt", FileOrPath, DateTime.Now);
            if (File.Exists(m_fileName))
            {
                ///如果日志文件已经存在，则直接写入已有的日志文件    
                using (StreamWriter sr = File.AppendText(FileName))
                {
                    sr.WriteLine("\n");
                    sr.WriteLine(message);
                    sr.Close();
                }
            }
            else
            {
                ///创建日志文件           
                using (StreamWriter sr = File.CreateText(FileName))
                {
                    sr.WriteLine("\n");
                    sr.WriteLine(message);
                    sr.Close();
                }
            }
        }


    }
}
