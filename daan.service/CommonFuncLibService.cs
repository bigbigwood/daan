using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using daan.service.common;
using daan.domain;
using daan.service.login;
using daan.service.dict;
using daan.service.bill;
using System.Xml;
using System.IO;

namespace daan.service
{
    public class CommonFuncLibService : BaseService
    {


        public Maintenancelog SaveMaintenanceLog(Maintenancelog log)
        {
            if (log.Maintenancelogid == 0)
            {
                log.Maintenancelogid = this.getNextId("maintenanceLog");
                this.insert("dict.InsertMaintenancelog", log);
            }
            else
            {
                this.update("UpdateMaintenanceLog", log);
            }

            return log;
        }

        /// <summary>
        /// 分页获取日志列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Maintenancelog> GetMaintenancelogPagePageLst(Hashtable ht)
        {
            return this.QueryList<Maintenancelog>("Dict.GetMaintenancelogPageLst", ht);
        }

        /// <summary>
        /// 获取日志列表总项数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public int GetMaintenancelogPagePageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetMaintenancelogPageLstCount", ht).Tables[0].Rows[0][0]);
        }

        public DataSet GetUserTable()
        {
            DataSet ds = this.selectDS("Common.GetUserTable", null);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTUSER")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "用户资源管理";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "BILLDETAIL")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "账单明细";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "BILLHEAD")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "账单信息头表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "BILLTRACE")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "账单跟踪";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "CUSTOMERNEXTTEST")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "单位下次体检项目推荐";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "CUSTOMERRESULTCOMMENT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "客户结果评价";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "CUSTOMERVALIDDIAGNOSIS")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "团检客户有效诊断";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTCUSTOMER")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "体检单位维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTCUSTOMERDISCOUNTED")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "客户总体折扣维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTCUSTOMERTESTDISCOUNT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "外包客户项目折扣";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTDIAGNOSIS")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "诊断信息";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTFAMILYMEDHISTORY")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "家族病史";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTFASTCOMMENT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "快速录入模版维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLAB")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "分点维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLABANDTEST")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "分点检测项维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLABANDTESTPRICE")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "分点检测项维护价格维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLABDEPT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "科室维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLIBRARY")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "基础字典维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLIBRARYITEM")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "基础字典明细维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTLOCUSREMARK")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "基因座简介";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTMEDHISTORY")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "既往病史";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTMEMBER")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "会员用户表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTOTHERMEDHISTORY")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "其它病史";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTPRODUCTDETAIL")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "套餐明细";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTREPORTTEMPLATE")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "报告模板";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTRULEFORMULAR")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "产品建议规则公式";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTSCORES")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "易感基因结果得分表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTTESTGROUPDETAIL")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "检查项目组合明细";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTTESTITEM")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "检查项目维护";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTTESTITEMRESULT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "检查项目组合明细可选结果";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTUSERANDLAB")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "用户分点对应表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "DICTUSERANDLABDEPT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "用户物理组对应表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "INITBASIC")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "初始化的基本资料";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "INITLOCALSETTING")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "本地参数设定";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "INITSYSSETTING")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "系统设定";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "INTERFACELOG")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "接口日志";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "INTERFACEMANAGER")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "接口管理表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "MAINTENANCELOG")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "基础资料表维护日志表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "OPERATIONLOG")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "用于订单的修改留痕和节点信息";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERBARCODE")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单条码表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERDIAGNOSIS")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单诊断表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERGROUPTEST")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单对应组合";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERLABDEPTRESULT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "科室小结";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERNEXTTEST")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "医生推荐下次检测项目";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERPRODUCTS")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单套餐表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERRESULTCOMMENT")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单总检信息表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERS")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单主表";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERSERVICEINFO")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "客户订单追踪表情况记录";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "ORDERTEST")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "订单对应明细项目";
                }
                else if (ds.Tables[0].Rows[i]["TABLE_NAME"].ToString() == "TEMPORDERNUM")
                {
                    ds.Tables[0].Rows[i]["TABLE_NAME"] = "临时订单表";
                }

            }
            return ds;
        }


        public List<Maintenancelog> GetMaintenanceLogLst(Hashtable ht)
        {

            List<Maintenancelog> _lst = new List<Maintenancelog>();
            IList lst = this.selectIList("GetMaintenanceLogLst", ht);
            _lst = IListToList<Maintenancelog>(lst);
            return _lst;
        }

        /// <summary>
        /// 重新计算价格
        /// </summary>
        /// <param name="list">Ordergrouptest 集合</param>
        /// <param name="ht"></param>
        /// <returns>重新计算价格后的Ordergrouptest集合</returns>
        public IList<Ordergrouptest> OrdergrouptestNewPrice(IList<Ordergrouptest> grouptestList, Hashtable ht)
        {
            LoginService loginService = new LoginService();
            DictcustomertestdiscountService dctdService = new DictcustomertestdiscountService();
            DictcustomerdiscountedService dcddService = new DictcustomerdiscountedService();

            List<Dicttestitem> dtiList = new List<Dicttestitem>();
            List<Dictlabandtestprice> dltpList = new List<Dictlabandtestprice>();
            List<Dictproductdetail> dpdList = new List<Dictproductdetail>();
            List<Dictcustomerdiscounted> dcddList = new List<Dictcustomerdiscounted>();
            List<Dictcustomertestdiscount> dctdList = new List<Dictcustomertestdiscount>();

            try
            {
                dtiList = loginService.GetLoginDicttestitemList();
                dltpList = loginService.GetLoginDictlabandtestpriceList();
                dpdList = loginService.GetLoginDictproductdetail();
                dcddList = loginService.GetDictcustomerdiscounted();
                dctdList = loginService.GetDictcustomerdiscount();

                string type = ht["flag"].ToString();
                double? customerid = null;
                double? dictlabid = null;

                if (ht["customerid"] != null)
                    customerid = double.Parse(ht["customerid"].ToString());
                if (ht["dictlabid"] != null)
                    dictlabid = double.Parse(ht["dictlabid"].ToString());


                foreach (Ordergrouptest item in grouptestList)
                {
                    //标准价格
                    List<Dicttestitem> _dtiList = (from a in dtiList where a.Dicttestitemid == item.Dicttestitemid select a).ToList();

                    //分点价格   item.Createdate取值来源于orders表enterdate表示登记时间
                    List<Dictlabandtestprice> _dltpList = (from a in dltpList where a.Dicttestitemid == item.Dicttestitemid && a.Begindate <= item.Createdate && a.Enddate.AddDays(1) >= item.Createdate && a.Dictlabid == dictlabid select a).ToList();

                    //单位折扣率
                    List<Dictcustomerdiscounted> _dcddList = (from a in dcddList where a.Dictcustomerid == customerid && a.Begindate <= item.Createdate && a.Enddate.AddDays(1) >= item.Createdate select a).ToList();

                    //成交价
                    List<Dictproductdetail> _dpdList = (from a in dpdList
                                                        where a.Productid == item.Dictproductid && a.Testgroupid == item.Dicttestitemid
                                                        select a).ToList();

                    if (_dtiList.Count > 0)
                        item.Standardprice = _dtiList[0].Price;//达安标准价
                    if (_dltpList.Count > 0)
                        item.Groupprice = _dltpList[0].Price;//分点价   

                    //没值时取Standardprice
                    if (item.Groupprice == null || item.Groupprice == 0) //无分点价则取标准价
                        item.Groupprice = item.Standardprice;

                    if (_dpdList.Count > 0)
                        item.Contractprice = _dpdList[0].Finalprice;   //应收价 取 套餐维护的成交价


                    if (item.Contractprice == null || item.Contractprice == 0)
                    {
                        if (_dcddList.Count > 0)  //折扣率
                        {
                            //维护了客户整体折扣率，取区域价钱*客户整体折扣率；
                            item.Contractprice = item.Groupprice * _dcddList[0].Discounted;
                        }
                        else
                        {
                            //没有整体折扣率取达安标准价钱。
                            item.Contractprice = item.Groupprice;
                        }
                    }

                    //实收价 默认和应收价钱一样
                    item.Finalprice = item.Contractprice;

                    item.Contractsendoutprice = item.Sendoutprice = 0;
                    if (item.Sendoutcustomerid != null)
                    {
                        //外包价格
                        List<Dictcustomertestdiscount> _senddctdList = (from a in dctdList where a.Dicttestitemid == item.Dicttestitemid && a.Dictcustomerid == item.Sendoutcustomerid && a.Begindate <= item.Createdate && a.Enddate.AddDays(1) >= item.Createdate select a).ToList();
                        if (_senddctdList.Count > 0)
                        {
                            item.Contractsendoutprice = item.Sendoutprice = _senddctdList[0].Finalprice;
                        }
                    }

                    //外包账单 【应收=外包价协议价，实收=外报价】
                  if (type == "sendout") { 
                      item.Finalprice = item.Sendoutprice;
                      item.Contractprice = item.Contractsendoutprice;
                  }
                }
                return grouptestList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  财务打印
        /// </summary>
        /// <param name="htinfo">报告单明细信息</param>
        /// <param name="flag">报告单数据来源标示：团检、现金接收</param>
        /// <param name="strwhere">获得打印明细信息的查询条件</param>
        /// <param name="checkbillid">财务清单核对人编号</param>
        /// <param name="dictlabid">分点编号</param>
        /// <returns>dataset：报告单数据集</returns>
        public DataSet ConvertToDataSet(Hashtable htinfo, string strwhere, string flag, double? checkbillid, double? dictlabid, string sendout)
        {
            try
            {
                LoginService loginService = new LoginService();
                BilldetailService detailservice = new BilldetailService();
                BillheadService headservice = new BillheadService();

                IList<Billhead> headlist = null;
                IList<Billdetail> detailList = null;
                DataSet ds = new DataSet();

                //获得打印明细信息
                if (flag == "billdetail")   //团检
                {
                    if (sendout == "nosendout")
                        detailList = detailservice.SelectBilldetailInfoList(strwhere);
                    else if (sendout == "sendout")
                        detailList = detailservice.SelectSendOutBillDetailInfoPrint(strwhere);
                }
                else                       //现金接收
                    headlist = headservice.SelectBillHeadListForPrintByids(strwhere);

                #region 设置报告单中间明细信息
                //生成财务账单详细信息
                DataTable dt = new DataTable("tbdetail");
                DataColumn column;
                DataRow dr = dt.NewRow();

                column = new DataColumn("Orderenterdate");
                dt.Columns.Add(column);
                column = new DataColumn("Ordernum");
                dt.Columns.Add(column);
                column = new DataColumn("Realname");
                dt.Columns.Add(column);
                column = new DataColumn("Productname");
                dt.Columns.Add(column);
                column = new DataColumn("Standardprice");
                dt.Columns.Add(column);
                column = new DataColumn("Finalprice");
                dt.Columns.Add(column);
                column = new DataColumn("Remark");
                dt.Columns.Add(column);

                if (detailList != null)
                {
                    foreach (Billdetail detail in detailList)
                    {
                        dr = dt.NewRow();
                        dr["Orderenterdate"] = detail.Orderenterdate.Value.ToString("yyyy-MM-dd");
                        dr["Ordernum"] = detail.Ordernum;
                        dr["Realname"] = detail.Realname;
                        dr["Productname"] = detail.Productname;
                        dr["Standardprice"] = detail.Standardprice;
                        dr["Finalprice"] = detail.Finalprice;
                        dr["Remark"] = detail.Remark;
                        dt.Rows.Add(dr);
                    }
                }
                else if (headlist != null)
                {
                    foreach (Billhead head in headlist)
                    {
                        dr = dt.NewRow();
                        dr["Orderenterdate"] = head.Orderenterdate.Value.ToString("yyyy-MM-dd");
                        dr["Ordernum"] = head.Ordernum;
                        dr["Realname"] = head.Realname;
                        dr["Productname"] = head.Productname;
                        dr["Standardprice"] = head.Totalstandardprice;
                        dr["Finalprice"] = head.Totalfinalprice;
                        dr["Remark"] = head.Remark;
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
                #endregion


                //获得销售人员、清单核对人、分点信息 
                List<Dictuser> userlist = new List<Dictuser>();
                List<Dictlab> labList = new List<Dictlab>();
                List<Dictuser> usercheck = new List<Dictuser>();
                List<Dictuser> usersale = new List<Dictuser>();
                List<Dictcustomer> customer = new List<Dictcustomer>();

                userlist = loginService.GetDictuser();
                labList = loginService.GetLoginDictlab();

                List<Dictlab> dictLabList = (from a in labList where a.Dictlabid == dictlabid select a).ToList<Dictlab>();

                if (flag == "billdetail")
                {
                    usercheck = (from a in userlist where a.Dictuserid == checkbillid select a).ToList<Dictuser>();
                }
                else
                {
                    //获取"个人客户"基本信息
                    List<Dictcustomer> customerList = loginService.GetDictcustomer();
                    int customercode = (int)ParamStatus.PersonalCustomerID.SingleCustomerCode;
                    customer = (from a in customerList where a.Customercode == customercode.ToString() select a).ToList();
                    if (customer.Count > 0)
                    {
                        usercheck = (from a in userlist where a.Dictuserid == customer[0].Dictcheckbillid select a).ToList();
                        usersale = (from a in userlist where a.Dictuserid == customer[0].Dictsalemanid select a).ToList();
                    }
                }

                #region 设置报告单头尾信息
                //生成财务账单头尾信息
                DataTable dtinfo = new DataTable("headinfo");
                DataColumn headcolumn;
                DataRow headrow = dtinfo.NewRow();

                headcolumn = new DataColumn("customername");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("salename");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("checkbillname");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("phone");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("fax");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("website");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("totalstandardprice");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("totalfinalprice");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("begindate");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("enddate");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("customertype");
                dtinfo.Columns.Add(headcolumn);
                headcolumn = new DataColumn("ordernumcount");
                dtinfo.Columns.Add(headcolumn);

                if (flag == "billdetail")
                {
                    headrow["customername"] = htinfo["customername"];
                    headrow["salename"] = htinfo["salename"];
                    headrow["totalstandardprice"] = detailList.Sum(c => c.Standardprice);
                    headrow["totalfinalprice"] = detailList.Sum(c => c.Finalprice);
                    headrow["ordernumcount"] = detailList.Count;
                }
                else
                {
                    headrow["customername"] = customer.Count > 0 ? customer[0].Customername : "";
                    headrow["salename"] = usersale.Count > 0 ? usersale[0].Username : "";
                    headrow["totalstandardprice"] = headlist.Sum(c => c.Totalstandardprice);
                    headrow["totalfinalprice"] = headlist.Sum(c => c.Totalfinalprice);
                    headrow["ordernumcount"] = headlist.Count;
                }
                headrow["phone"] = "";
                headrow["fax"] = "";

                if (dictLabList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dictLabList[0].Phone))
                        headrow["phone"] = "Tel:" + dictLabList[0].Phone;
                    if (!string.IsNullOrEmpty(dictLabList[0].Fax))
                        headrow["fax"] = "Fax:" + dictLabList[0].Fax;
                }
                headrow["website"] = dictLabList.Count > 0 ? dictLabList[0].Website : "";
                headrow["checkbillname"] = usercheck.Count > 0 ? usercheck[0].Username : "";
                headrow["begindate"] = htinfo["begindate"];
                headrow["enddate"] = htinfo["enddate"];
                headrow["customertype"] = htinfo["customertype"];

                dtinfo.Rows.Add(headrow);
                ds.Tables.Add(dtinfo);
                #endregion

                string titlename = htinfo["titleName"].ToString();

                #region 设置标题
                DataTable dtt = new DataTable("dtTitle");
                DataRow dtr;
                DataColumn dtc;

                dtc = new DataColumn("titleName");
                dtt.Columns.Add(dtc);
                dtr = dtt.NewRow();
                dtr["titleName"] = titlename;
                dtt.Rows.Add(dtr);
                ds.Tables.Add(dtt);
                #endregion

                return ds;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 个人收费打印
        /// </summary>
        /// <param name="htinfo">报告单明细信息</param>
        /// <returns></returns>
        public DataSet ConvertToDataSet(Hashtable htinfo)
        {
            //定义dataset 存放打印信息
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("dtHealthGuideTitle");
            DataRow dr;
            DataColumn column;

            column = new DataColumn("Ordernum");
            dt.Columns.Add(column);
            column = new DataColumn("Realname");
            dt.Columns.Add(column);
            column = new DataColumn("Productname");
            dt.Columns.Add(column);
            column = new DataColumn("Finalprice");
            dt.Columns.Add(column);
            column = new DataColumn("Billbyname");
            dt.Columns.Add(column);
            column = new DataColumn("Billbydate");
            dt.Columns.Add(column);

            dr = dt.NewRow();
            dr["Ordernum"] = htinfo["Ordernum"];
            dr["Realname"] = htinfo["Realname"];
            dr["Productname"] = htinfo["Productname"];
            dr["Finalprice"] = htinfo["Finalprice"];
            dr["Billbyname"] = htinfo["Billbyname"];
            dr["Billbydate"] = htinfo["Billbydate"];
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);

            string titlename = htinfo["titleName"].ToString();
            #region 设置标题
            DataTable dtt = new DataTable("dtTitle");
            DataRow dtr;
            DataColumn dtc;

            dtc = new DataColumn("titleName");
            dtt.Columns.Add(dtc);
            dtr = dtt.NewRow();
            dtr["titleName"] = titlename;
            dtt.Rows.Add(dtr);
            ds.Tables.Add(dtt);
            #endregion
            return ds;
        }

        /// <summary>
        /// xml字符串转DataSet
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static void Copy(DataRow drSource, DataRow drDest)
        {


            foreach (DataColumn col in drSource.Table.Columns)
            {
                if (drDest.Table.Columns[col.ColumnName] != null)  //找到名称一样的，准备复制数据
                {
                    drDest[col.ColumnName] = drSource[col.ColumnName];
                }
            }
        }

    }
}
