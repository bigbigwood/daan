using System;
using System.Collections.Generic;
using System.Linq;
using daan.service.common;
using System.Collections;
using daan.domain;
using daan.util.Common;
using daan.service.dict;
using daan.service.login;

namespace daan.service.proceed
{
    public class ProRegisterService : BaseService
    {
        readonly LoginService loginservice = new LoginService();
        readonly DictmemberService memberservice = new DictmemberService();
        readonly DicttestitemService testitemservice = new DicttestitemService();

        /// <summary>查询体检订单项目
        /// 查询体检订单项目
        /// </summary>
        public List<OrderRegister> SelectOrdersDetail(string Ordernum)
        {
            return QueryList<OrderRegister>("Order.SelectOrdergrouptestByReister", Ordernum).ToList<OrderRegister>();
        }

        /// <summary>获取单个订单信息
        /// 获取单个订单信息
        /// </summary>
        public Orders SelectOrderInfo(string Ordernum)
        {
            return selectObj<Orders>("Order.SelectOrdersByOrdernum", Ordernum);
        }


        /// <summary>修改订单查询条码信息 
        /// 修改订单查询条码信息 
        /// </summary>
        public IList<Orderbarcode> SelectMemberById(string ordernum)
        {
            return QueryList<Orderbarcode>("Order.SelectOrderbarcodeByOrderNum", ordernum);
        }

        /// <summary>获取Barcode序列
        /// 获取Barcode序列
        /// </summary>
        /// <returns></returns>
        public string GetBarcodeSEQ()
        {
            //9位
            return getSeqID("SEQ_DICTBARCODE").ToString("000000000");
        }

        /// <summary>获取体检流水号
        /// 获取体检流水号
        /// </summary>
        /// <returns></returns>
        public string GetOrderNum()
        {
            Hashtable ht = new Hashtable();
            ht["returnValue"] = "";
            insert("Order.MakeOrderNum", ht);
            return ht["returnValue"].ToString();
        }

        /// <summary>获取会员多个项目上一次结果集
        /// 获取会员多个项目上一次结果集
        /// </summary>
        /// <returns></returns>
        public List<Ordertest> GetLastTestResult(double? memberid)
        {
            if (memberid != null)
            {
                return QueryList<Ordertest>("Order.SelectOrdertestLastResultList", memberid).ToList<Ordertest>();
            }
            else
            {
                return null;
            }

        }

        /// <summary>添加|修改订单
        /// 添加|修改订单
        /// </summary>
        /// <param name="type">模块名称 0:体检登记, 1:单位批量上传</param>
        /// <param name="IsInsert">是否添加</param>
        /// <param name="TestGroupDetailList">组合 对 单个项目字典</param>
        /// <param name="labid">分点ID</param>
        /// <param name="iegroupitem">订单中拆分出组合的集合</param>
        /// <param name="member">会员对象</param>
        /// <param name="_orders">orders表对象</param>
        /// <returns></returns>
        public bool insertUpdateOrders(string type, string Content, bool IsInsert, List<Dicttestitem> productList, List<Dicttestitem> grouptestList, Dictmember member, Orders _orders, string ReceivedBarcode, ref string error, Hashtable htScan = null, bool isCacheData = true)
        {
            //sql集合
            SortedList SQLlist = new SortedList(new MySort());
            //拼接barcode写日志
            string strbarcode = string.Empty;
            try
            {
                List<Dicttestitem> TestItemList = isCacheData == true ? loginservice.GetLoginDicttestitemList() : loginservice.GetLoginDicttestitemListNoCache();///项目字典表
                List<Dictproductdetail> ProductDetail = isCacheData == true ? loginservice.GetLoginDictproductdetail() : loginservice.GetLoginDictproductdetailNoCache();///套餐组合字典
                List<Dicttestgroupdetail> TestGroupDetailList = isCacheData == true ? loginservice.GetLoginDicttestgroupdetail() : loginservice.GetLoginDicttestgroupdetailNoCache();///组合项目字典
                //报告模版ID
                string reporttemplateidStr = "";

                //按组合分管原则分管
                IEnumerable<IGrouping<string, Dicttestitem>> iegroupitem = grouptestList.Where<Dicttestitem>(c => !ReceivedBarcode.Contains(c.Barcode)).GroupBy<Dicttestitem, string>(c => c.Barcode);

                //暂存添加到组合表的数据
                List<Ordergrouptest> ordergrouptestlist = new List<Ordergrouptest>();

                if (member != null)
                {
                    SQLlist.Add(new Hashtable() { { member.isAdd ? "INSERT" : "UPDATE", member.isAdd ? "Dict.InsertDictmember" : "Dict.UpdateDictmember" } }, member);
                }

                SQLlist.Add(new Hashtable() { { IsInsert == true ? "INSERT" : "UPDATE", IsInsert == true ? "Order.InsertOrders" : "Order.UpdateOrders" } }, _orders);

                if (!IsInsert)
                {
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrdergrouptestByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderbarcodeByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderproductsByOrderNum" } }, _orders.Ordernum);
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrdertestByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });

                    //更改测试组合明细的 是否测试状态
                    IEnumerable<Dicttestitem> Blist = grouptestList.Where<Dicttestitem>(c => ReceivedBarcode.Contains(c.Barcode));
                    foreach (Dicttestitem item in Blist)
                    {

                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestActive" } }, new Hashtable() { { "isactive", item.IsActive }, { "ordernum", _orders.Ordernum }, { "barcode", item.Barcode } });
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdertestActive" } }, new Hashtable() { { "isactive", item.IsActive }, { "ordernum", _orders.Ordernum }, { "barcode", item.Barcode } });

                    }
                }
                else
                {
                    //添加必检查项目
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdernexttestMustExam" } }, _orders.Ordernum);

                }

                //获取会员多个项目上一次结果集
                List<Ordertest> LastTestResultList = GetLastTestResult(_orders.Dictmemberid);

                if (iegroupitem.Count() > 0)
                {

                    //条码分管
                    foreach (IGrouping<string, Dicttestitem> item in iegroupitem)
                    {

                        //条码下的组合和项目
                        List<Dicttestitem> groupitem = item.ToList<Dicttestitem>();
                        string tubegroup = groupitem[0].Tubegroup;
                        if (tubegroup == null || tubegroup == string.Empty)
                        {
                            throw new Exception(string.Format("{0}[{1}]没有维护分管原则!", groupitem[0].Testtype == "1" ? "组合" : "单项", groupitem[0].Testname));
                        }
                        string barcode = item.Key;//母条码

                        #region >>>> zhouy insert Orderbarcode

                        Orderbarcode _orderbarcode = new Orderbarcode();

                        //有此分管原则  则取之前条码，以及..账单状态
                        IList<Orderbarcode> tempbarcodeList = SelectMemberById(_orders.Ordernum);
                        IEnumerable<Orderbarcode> IEtempbarcodeList = tempbarcodeList.Where<Orderbarcode>(c => c.Tubegroup == tubegroup);
                        if (IEtempbarcodeList.Count() > 0)
                        {
                            Orderbarcode tempOrderbarcode = IEtempbarcodeList.First<Orderbarcode>();
                            _orderbarcode.Status = tempOrderbarcode.Status;
                            _orderbarcode.Collectdate = tempOrderbarcode.Collectdate;
                            _orderbarcode.Collectby = tempOrderbarcode.Collectby;
                        }

                        _orderbarcode.Orderbarcodeid = getSeqID("SEQ_ORDERBARCODE");
                        _orderbarcode.Ordernum = _orders.Ordernum;
                        _orderbarcode.Barcode = barcode;                              //母条码               
                        _orderbarcode.Tubegroup = tubegroup;                           //分管原则
                        _orderbarcode.Specimentypeid = groupitem[0].Dictspecimentypeid;//随意取子组合项目第一个标本类型，维护都是统一的
                        _orderbarcode.Dictlabdeptid = groupitem[0].Dictlabdeptid;//随意取子组合项目第一个物理组，维护都是统一的

                        #endregion

                        string testnames = "";
                        //组合
                        foreach (Dicttestitem groupdetail in groupitem)
                        {
                            #region >>>> zhouy insert Ordergrouptest

                            Ordergrouptest _ordergrouptest = new Ordergrouptest();
                            _ordergrouptest.Ordergrouptestid = getSeqID("SEQ_ORDERGROUPTEST");
                            _ordergrouptest.Ordernum = _orders.Ordernum;
                            _ordergrouptest.Barcode = barcode;
                            _ordergrouptest.Dictproductid = groupdetail.Productid;  //套餐ID
                            _ordergrouptest.Dicttestitemid = groupdetail.Dicttestitemid;//套餐下的组合ID，当用户选单项存单项ID，对应DICTTESTITEM表id
                            _ordergrouptest.Engname = groupdetail.Engname;//测试项英文名
                            _ordergrouptest.Testname = groupdetail.Testname;//测试项中文名
                            _ordergrouptest.Testcode = groupdetail.Testcode;//测试项编号
                            _ordergrouptest.Tubegroup = groupdetail.Tubegroup;//分管原则（只有TESTTYPE=0需要维护）与dictlibary表dictlibaryid关联
                            _ordergrouptest.Dictlabdeptid = groupdetail.Dictlabdeptid;//测试项物理实验室分组,对应表DICTLABDEPT
                            //_ordergrouptest.Standardprice = groupdetail.Price;//达安标准价        |
                            //_ordergrouptest.Groupprice =//达安不同分点的价钱                      |
                            //_ordergrouptest.Contractprice =//应收价钱(合同价)                     |调用方法统一算
                            //_ordergrouptest.Sendoutprice =//外包成交价                            |
                            //_ordergrouptest.Finalprice =//成交价                                  | 
                            //_ordergrouptest.Issendouttest = groupdetail.Sendoutcustomerid == null ? "0" : "1";//是否外包
                            _ordergrouptest.Sendoutcustomerid = groupdetail.Sendoutcustomerid;//外包客户ID
                            _ordergrouptest.Displayorder = groupdetail.Displayorder; //项目字典表取
                            _ordergrouptest.Isadd = groupdetail.Isadd;//是否追加
                            _ordergrouptest.Adduserid = groupdetail.Adduserid;//追加人ID  
                            _ordergrouptest.Productname = groupdetail.Productname;//套餐名
                            _ordergrouptest.Operationremark = groupdetail.Operationremark;//执行说明
                            _ordergrouptest.Isactive = groupdetail.IsActive ?? "1";//是否继续检测
                            _ordergrouptest.Billed = groupdetail.Billed;//账单
                            _ordergrouptest.Sendbilled = groupdetail.Sendbilled;//外包账单
                            _ordergrouptest.Sendoutcustomerid = groupdetail.Sendoutcustomerid;//外包客户
                            //时间用于计算价格
                            _ordergrouptest.Createdate = _orders.Createdate;

                            ordergrouptestlist.Add(_ordergrouptest);

                            #endregion

                            //if (groupdetail.Productid == null)
                            //{
                            //    #region >>>> zhouy insertOrderproducts

                            //    Orderproducts _orderproducts = new Orderproducts();
                            //    _orderproducts.Orderproductsid = this.getSeqID("SEQ_ORDERPRODUCTS");
                            //    _orderproducts.Ordernum = _orders.Ordernum;
                            //    _orderproducts.Dicttestitemid = groupdetail.Dicttestitemid;//套餐ID，当套餐被拆散存拆散后的组合ID，当用户选单项存单项ID，对应dicttestitem表id
                            //    _orderproducts.Engname = groupdetail.Engname;
                            //    _orderproducts.Testcode = groupdetail.Testcode;
                            //    _orderproducts.Testname = groupdetail.Testname;
                            //    _orderproducts.Standardprice = null;//达安标准价 套餐不计算
                            //    _orderproducts.Groupprice = null;//达安不同分点的价钱 套餐不计算
                            //    _orderproducts.Finalprice = null;//成交价 套餐不计算
                            //    _orderproducts.Contractprice = null;//应收价钱(合同价) 套餐不计算
                            //    _orderproducts.Displayorder = groupdetail.Displayorder;//项目字典表取
                            //    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderproducts" } }, _orderproducts);

                            //    #endregion
                            //}

                            if (groupdetail.Testtype == "1")//组合
                            {
                                #region >>>> zhouy insert OrderTest

                                //获得组合下明细
                                IEnumerable<Dicttestgroupdetail> _testdetaillist = TestGroupDetailList.Where(c => c.Testgroupid == groupdetail.Dicttestitemid);

                                //明细项目
                                foreach (Dicttestgroupdetail testdeteail in _testdetaillist)
                                {
                                    Dicttestitem _testitem = SelectsTestItemListById(testdeteail.Dicttestitemid,isCacheData);

                                    //添加明细项目
                                    if (_testitem == null) { continue; }

                                    //获取上一次结果
                                    IEnumerable<Ordertest> IEOrdertest = LastTestResultList.Where<Ordertest>(c => c.Dicttestitemid == testdeteail.Dicttestitemid);
                                    Ordertest lasttest = new Ordertest();
                                    if (IEOrdertest.Count() > 0)
                                    {
                                        lasttest = IEOrdertest.First<Ordertest>();
                                    }

                                    Ordertest _ordertest = new Ordertest();
                                    _ordertest.Ordertestid = getSeqID("SEQ_ORDERTEST");
                                    _ordertest.Ordernum = _orders.Ordernum;
                                    _ordertest.Barcode = barcode;
                                    _ordertest.Dictproductsid = groupdetail.Productid; //产品ID对应DICTTESTITEM表ID
                                    _ordertest.Dictgroupid = groupdetail.Dicttestitemid;//组合ID对应DICTTESTITEM表ID
                                    _ordertest.Dictgroupname = groupdetail.Testname;//组合名称
                                    _ordertest.Dicttestitemid = _testitem.Dicttestitemid;//明细项目ID对应DICTTESTITEM表ID
                                    _ordertest.Engname = _testitem.Engname;//测试项英文名
                                    _ordertest.Englongname = _testitem.Englongname;//英文长名称（只有TESTTYPE=0需要维护）
                                    _ordertest.Testcode = _testitem.Testcode;//测试项编号
                                    _ordertest.Testname = _testitem.Testname;//明细项目名称
                                    _ordertest.Lastdate = lasttest.Createdate;//上一次时间
                                    _ordertest.Lastresult = lasttest.Testresult;//上一次结果
                                    _ordertest.Report = _testitem.Report;//项目字典表
                                    _ordertest.Displayorder = _testitem.Displayorder;//项目字典表
                                    _ordertest.Testresulttype = _testitem.Resulttype;//项目字典表
                                    _ordertest.Dictlabdeptid = _testitem.Dictlabdeptid;//项目字典表
                                    _ordertest.Unit = _testitem.Unit;
                                    _ordertest.Isimportant = _testitem.Isimportant;
                                    _ordertest.Isactive = groupdetail.IsActive ?? "1";


                                    reporttemplateidStr += reporttemplateidStr.Contains(_testitem.Dictreporttemplateid.ToString()) ? "" : (_testitem.Dictreporttemplateid + ",");
                                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdertest" } }, _ordertest);
                                }
                                #endregion
                            }
                            else if (groupdetail.Testtype == "0")//单项
                            {
                                #region >>>> zhouy insert OrderTest

                                //获取上一次结果
                                IEnumerable<Ordertest> IEOrdertest = LastTestResultList.Where<Ordertest>(c => c.Dicttestitemid == groupdetail.Dicttestitemid);
                                Ordertest lasttest = new Ordertest();
                                if (IEOrdertest.Count() > 0)
                                {
                                    lasttest = IEOrdertest.First<Ordertest>();
                                }
                                Ordertest _ordertest = new Ordertest();
                                _ordertest.Ordertestid = getSeqID("SEQ_ORDERTEST");
                                _ordertest.Ordernum = _orders.Ordernum;
                                _ordertest.Barcode = barcode;
                                _ordertest.Dictproductsid = groupdetail.Productid;//产品ID对应DICTTESTITEM表ID
                                _ordertest.Dictgroupid = null;//组合ID对应DICTTESTITEM表ID
                                _ordertest.Dictgroupname = null;//组合名称
                                _ordertest.Dicttestitemid = groupdetail.Dicttestitemid;//明细项目ID对应DICTTESTITEM表ID
                                _ordertest.Engname = groupdetail.Engname;//测试项英文名
                                _ordertest.Englongname = groupdetail.Englongname;//英文长名称（只有TESTTYPE=0需要维护）
                                _ordertest.Testcode = groupdetail.Testcode;//测试项编号
                                _ordertest.Testname = groupdetail.Testname;//明细项目名称
                                _ordertest.Lastdate = lasttest.Createdate;//上一次时间
                                _ordertest.Lastresult = lasttest.Testresult;//上一次结果
                                _ordertest.Report = groupdetail.Report;//项目字典表
                                _ordertest.Displayorder = groupdetail.Displayorder;//项目字典表
                                _ordertest.Testresulttype = groupdetail.Resulttype;//项目字典表
                                _ordertest.Dictlabdeptid = groupdetail.Dictlabdeptid;//项目字典表
                                _ordertest.Unit = groupdetail.Unit;
                                _ordertest.Isimportant = groupdetail.Isimportant;
                                _ordertest.Isactive = groupdetail.IsActive ?? "1";

                                SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdertest" } }, _ordertest);
                                #endregion

                                reporttemplateidStr += reporttemplateidStr.Contains(groupdetail.Dictreporttemplateid.ToString()) ? "" : (groupdetail.Dictreporttemplateid + ",");
                            }
                            testnames += groupdetail.Testname + ",";
                        }
                        strbarcode += _orderbarcode.Barcode + ",";
                        _orderbarcode.Testnames = testnames;//多个项目用逗号隔开
                        SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderbarcode" } }, _orderbarcode);
                    }

                    //取模版ID
                    string[] reporttemplateid = reporttemplateidStr.TrimEnd(',').Split(',');
                    if (reporttemplateidStr == "" && iegroupitem.Count() > 0)
                    { 
                        throw new Exception("该套餐所有组合项目均未维护模版！");
                    }

                    if (reporttemplateid.Length == 1)
                    {
                        _orders.Dictreporttemplateid = Convert.ToDouble(reporttemplateid[0]);
                    }
                    else if (reporttemplateid.Length > 1)
                    {
                        _orders.Dictreporttemplateid = SelectDictreporttemplateByCommonRep();
                    }

                    //获取价格
                    new CommonFuncLibService().OrdergrouptestNewPrice(ordergrouptestlist, new Hashtable()
                    { 
                    { "dictlabid", _orders.Dictlabid }, 
                    { "flag", "nosendout" }, 
                    { "customerid", _orders.Dictcustomerid } });
                    //循环插入Insert Ordergrouptest
                    foreach (Ordergrouptest item in ordergrouptestlist)
                    {
                        SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdergrouptest" } }, item);
                    }
                }
                #region >>>> zhouy insertOrderproducts
                //循环套餐
                foreach (Dicttestitem item in productList)
                {
                    Orderproducts _orderproducts = new Orderproducts();
                    _orderproducts.Orderproductsid = getSeqID("SEQ_ORDERPRODUCTS");
                    _orderproducts.Ordernum = _orders.Ordernum;
                    _orderproducts.Dicttestitemid = item.Dicttestitemid;//套餐ID，当套餐被拆散存拆散后的组合ID，当用户选单项存单项ID，对应dicttestitem表id
                    _orderproducts.Engname = item.Engname;
                    _orderproducts.Testcode = item.Testcode;
                    _orderproducts.Testname = item.Testname;
                    _orderproducts.Standardprice = null;// item.Price;//达安标准价  套餐不计算
                    _orderproducts.Groupprice = null;// priceitem == null ? item.Price : priceitem.Price;//达安不同分点的价钱  套餐不计算
                    _orderproducts.Finalprice = null;// item.Price;//成交价=达安标准价  套餐不计算
                    _orderproducts.Contractprice = null;//item.Price;//应收价钱(合同价)=达安标准价  套餐不计算
                    _orderproducts.Displayorder = item.Displayorder;//项目字典表取
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderproducts" } }, _orderproducts);
                }
                #endregion

                //更改订单上传社区状态
                SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateTransed" } }, _orders.Ordernum);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            bool b = ExecuteSqlTran(SQLlist, ref error);

            //记录日志
            AddOperationLog(_orders.Ordernum, strbarcode.TrimEnd(','), type, Content, IsInsert ? "节点信息" : "修改留痕", IsInsert ? "添加" : "修改", htScan);

            return b;
        }
        
        //获取拼接格式条码号
        public string GetBarCode()
        {
            string barcode = string.Format("8{0}00", GetBarcodeSEQ());
            return barcode;
        }
        //获取HPV样本条码号
        public static string GetBarCode(string barcode)
        {
            return barcode;
        }
        //更新会员信息
        public bool UpdateMemberInfo(Dictmember oldmember, Dictmember newmember)
        {
            bool b = true;
            try
            {
                if (update("Dict.UpdateDictmember", newmember) >= 0)
                {
                    //写日志
                    List<LogInfo> logLst = getLogInfo<Dictmember>(oldmember, newmember);
                    AddMaintenanceLog("dictmember", newmember.Dictmemberid, logLst, "修改", newmember.Realname, newmember.Loginname, "会员档案");
                }
                else
                {
                    b = false;
                }
            }
            catch (Exception)
            {
                b = false;
            }

            return b;
        }

        /// <summary>检查是否添加 是否存在组合项目，并且分管原则不为空
        /// 检查是否添加
        /// </summary>
        /// <param name="row">列表数据行</param>
        /// <param name="itemtest">添加的组合|项目</param>
        /// <param name="strproductname">套餐名，添加套餐时使用，添加组合传string.Empty</param>
        public string checkInsert(OrderRegister row, Dicttestitem itemtest, string strproductname, bool isCacheData = true)
        {
            string msg = string.Empty;
            List<Dicttestgroupdetail> TestGroupDetailList = isCacheData == false ? loginservice.GetLoginDicttestgroupdetailNoCache() : loginservice.GetLoginDicttestgroupdetail();///组合项目字典

            if (!string.IsNullOrEmpty(strproductname))
            {
                strproductname = String.Format("套餐[{0}]中的", strproductname);
            }
            Dicttestitem rowdict = SelectsTestItemListById(Convert.ToDouble(row.Id), isCacheData);
            if (itemtest.Testtype == "1")///添加的组合
            {
                double? id = itemtest.Dicttestitemid;
                ///添加的组合
                IEnumerable<Dicttestgroupdetail> ienewtest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == id);

                if (row.IsGroup)///列表行为组合
                {
                    #region >>>> zhouy 列表行 为组合，添加行 为组合

                    if (rowdict.Dicttestitemid == itemtest.Dicttestitemid)
                    {
                        msg = string.Format("您添加的{0}组合[{1}]已经在列表中了！", strproductname, itemtest.Testname);
                        return msg;
                    }
                    /////列表行的组合
                    //IEnumerable<Dicttestgroupdetail> ieoldtest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == rowdict.Dicttestitemid);
                    //foreach (Dicttestgroupdetail oldtest in ieoldtest)
                    //{
                    //    foreach (Dicttestgroupdetail newtest in ienewtest)
                    //    {

                    //        Dicttestitem newtestdetail = SelectsTestItemListById(newtest.Dicttestitemid);
                    //        if (oldtest.Dicttestitemid == newtest.Dicttestitemid)
                    //        {
                    //            msg = string.Format("您添加的" + strproductname + "组合[{0}]中的项目[{1}]已经在列表组合[{2}]中了！", itemtest.Testname, newtestdetail.Testname, rowdict.Testname);
                    //            return msg;
                    //        }
                    //    }
                    //}

                    #endregion

                }
                else///列表行 为单项
                {
                    #region >>>> zhouy 列表行 为单项，添加行 为组合

                    foreach (Dicttestgroupdetail newtest in ienewtest)
                    {
                        Dicttestitem newtestdetail = SelectsTestItemListById(newtest.Dicttestitemid, isCacheData);
                        if (newtest.Dicttestitemid == rowdict.Dicttestitemid)
                        {
                            msg = string.Format("您添加的{0}组合[{1}]中的项目[{2}]已经在列表中了！", strproductname, itemtest.Testname, rowdict.Testname);
                            return msg;
                        }
                    }

                    #endregion
                }

                if (itemtest.Tubegroup == null)
                {
                    msg = string.Format("您添加的{0}组合[{1}]未维护好分管原则！", strproductname, itemtest.Testname);
                    return msg;
                }
            }
            else///单项
            {
                if (row.IsGroup)///列表行为组合
                {
                    #region >>>> zhouy 列表行 为组合，添加行 为单项

                    IEnumerable<Dicttestgroupdetail> ietest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == rowdict.Dicttestitemid);
                    foreach (Dicttestgroupdetail test in ietest)
                    {
                        if (test.Dicttestitemid == itemtest.Dicttestitemid)
                        {
                            msg = string.Format("列表组合[{0}]中的已经包含了您添加的{1}项目[{2}]！", rowdict.Testname, strproductname, itemtest.Testname);
                            return msg;
                        }
                    }

                    #endregion
                }
                else///单项
                {
                    #region >>>> zhouy 列表行 为单项，添加行 为单项

                    if (rowdict.Dicttestitemid == itemtest.Dicttestitemid)
                    {
                        msg = string.Format("您添加的{0}项目[{1}]已经在列表中了！", strproductname, itemtest.Testname);
                        return msg;
                    }

                    #endregion
                }
                if (itemtest.Tubegroup == null)
                {
                    msg = string.Format("您添加的{0}项目[{1}]未维护好分管原则！", strproductname, itemtest.Testname);
                    return msg;
                }
            }
            return msg;
        }


        /// <summary>判断性别是否符合
        /// 判断性别是否符合
        /// </summary>
        /// <param name="dicttestitemid">组合或者项目ID</param>
        /// <param name="sex">性别 M F U</param>
        /// <returns></returns>
        public string checkSex(double? dicttestitemid, string sex, bool isCacheData = true)
        {
            Dicttestitem testitem = SelectsTestItemListById(dicttestitemid, isCacheData);
            string str = string.Empty;
            if (testitem.Testtype == "0")
            {
                if (testitem.Forsex != "B" && sex != "U" && testitem.Forsex != sex)
                {
                    str = string.Format("您添加的项目[{0}]适合性别[{1}]与所选性别[{2}]不匹配！", testitem.Testname, testitem.Forsex == "M" ? "男" : "女", sex == "M" ? "男" : "女");
                }
            }
            else
            {
                List<Dicttestgroupdetail> TestGroupDetailList = isCacheData == true ? loginservice.GetLoginDicttestgroupdetail() : loginservice.GetLoginDicttestgroupdetailNoCache();///组合项目字典
                IEnumerable<Dicttestgroupdetail> ienewtest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == dicttestitemid);
                foreach (Dicttestgroupdetail newtest in ienewtest)
                {
                    Dicttestitem newtestdetail = SelectsTestItemListById(newtest.Dicttestitemid, isCacheData);
                    if (newtestdetail.Forsex != "B" && sex != "U" && newtestdetail.Forsex != sex)
                    {
                        str = string.Format("您添加的组合[{0}]中的项目[{1}]适合性别[{2}]与所选性别[{3}]不匹配！", testitem.Testname, newtestdetail.Testname, newtestdetail.Forsex == "M" ? "男" : "女", sex == "M" ? "男" : "女"); return str;
                    }
                }
            }
            return str;
        }

        /// <summary>检查会员是否存在
        /// 检查会员是否存在
        /// </summary>
        /// <param name="memberid">会员id</param>
        /// <param name="member">会员对象</param>
        /// <param name="memberidtext">会员ID</param>
        /// <param name="idnumbertext">身份证</param>
        /// <returns></returns>
        public string checkmember(double? memberid, ref Dictmember member)
        {
            string str = "";
            //根据身份证+姓名查找记录
            List<Dictmember> memberlist = memberservice.GetDictmemberList(member);
            //查到一条记录 并且 界面未选择会员,身份证未填写，[姓名相同] ->  重新新建帐号
            if (memberlist.Count == 1 && memberid == null && member.Idnumber == string.Empty)
            {
                memberid = null;
            }
            //查询到一条记录 并且(记录ID等于界面选择会员ID或者没有选择界面会员) 使用以前帐号
            else if (memberlist.Count == 1 && (memberid == memberlist[0].Dictmemberid || memberid == null))
            {
                member = memberlist[0];
                memberid = member.Dictmemberid;
            }
            //记录大于一条的情况
            else if (memberlist.Count > 1)
            {
                //有选择界面会员
                if (memberid != null)
                {
                    Dictmember _member = memberservice.GetMemberById(memberid);
                    //用会员ID查询　如姓名相同　则使用以前帐号
                    if (_member.Realname == member.Realname)
                    {
                        member = _member;
                    }
                    //姓名不相同  ->  重新新建帐号
                    else
                    {
                        memberid = null;
                    }
                }
                else
                {
                    str = string.Format("系统中存在在姓名为[{0}],身份证号为[{1}]的记录!", member.Realname, member.Idnumber);
                }
            }
            //没有记录 ->  重新新建帐号
            else if (memberlist.Count == 0)
            {
                memberid = null;
            }


            //member.Realname = member.Realname;
            //member.Idnumber = member.Idnumber;
            member.Dictmemberid = memberid ?? getSeqID("SEQ_DICTMEMBER");
            member.isAdd = memberid == null;
            return str;
        }


        /// <summary>根据Dicttestitemid获取单条字典信息
        /// 根据Dicttestitemid获取单条字典信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dicttestitem SelectsTestItemListById(double? ID, bool isCacheData = true)
        {
            Dicttestitem test;
            List<Dicttestitem> TestItemList = isCacheData == true ? loginservice.GetLoginDicttestitemList() : loginservice.GetLoginDicttestitemListNoCache();//项目字典表
            IEnumerable<Dicttestitem> IEtest = TestItemList.Where<Dicttestitem>(c => c.Dicttestitemid == ID);
            if (IEtest.Count() > 0)
                test = IEtest.First<Dicttestitem>();
            else
                test = new Dicttestitem();
            return test;
        }

        /// <summary>添加套餐
        /// 添加套餐
        /// </summary>
        /// <param name="_gridtestList">ref 列表集合</param>
        /// <param name="sex">性别value  F M U</param>
        /// <param name="productid">套餐ID</param>
        /// <param name="isinsert">是否追加</param>
        /// <param name="userinfo">登录用户信息</param>
        /// <param name="productname">ref 套餐名</param>
        /// <returns></returns>
        public string AddProduct(ref  List<OrderRegister> _gridtestList, string sex, double? productid, bool isinsert, UserInfo userinfo, ref string productname, string barcode, bool flag = false)
        {
            string msg = string.Empty;

            List<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetail();///套餐组合字典

            IEnumerable<Dictproductdetail> testlist = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productid);
            Dicttestitem product = SelectsTestItemListById(productid);
            productname = product.Testname;
            //还未添加项目则new一个
            if (_gridtestList == null) { _gridtestList = new List<OrderRegister>(); }

            foreach (Dictproductdetail group in testlist)
            {
                msg = AddGroupTest(ref _gridtestList, sex, productid, group.Testgroupid, isinsert, userinfo, ProductDetail, barcode, flag);
                if (msg != string.Empty) { return msg; }
                //Dicttestitem grouptest = SelectsTestItemListById(group.Testgroupid);
                ////校验性别是否符合
                //check Sex(grouptest.Dicttestitemid, sex);
                //if (msg != string.Empty) { return msg; }

                //bool isNewBarcode = true;
                //string barccode = "";
                //foreach (OrderRegister item in _gridtestList)
                //{
                //    //  此处判断组合存在，或者性别不允许就不需要允许添加
                //    msg = checkInsert(item, grouptest, product.Testname);
                //    if (msg != string.Empty) { return msg; }

                //    Dicttestitem itemtest = SelectsTestItemListById(item.Id);
                //    //  此处判断分管原则相同，所属科室也要相同
                //    if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictlabdeptid != grouptest.Dictlabdeptid) { return string.Format("[{0}]与[{1}]的分管原则相同，但[所属科室]不同",grouptest.Testname,itemtest.Testname); }
                //    //  此处判断分管原则相同，标本类型也要相同
                //    if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictspecimentypeid != grouptest.Dictspecimentypeid) { return string.Format("[{0}]与[{1}]的分管原则相同，但[标本类型]不同", grouptest.Testname, itemtest.Testname); }

                //    //分管原则相同时 没有标本接受时使用以前条码
                //    if ((Convert.ToInt32(item.Status)) < ((int)daan.service.common.ParamStatus.OrderbarcodeStatus.Received) && item.Tubegroup == grouptest.Tubegroup)
                //    {
                //        isNewBarcode = false;
                //        barccode = item.Barcode;
                //    }
                //}
                ////判断使用原条码还是新条码
                //if (isNewBarcode) { barccode = GetBarCode(); }


                //OrderRegister newOrderRegister = new OrderRegister();
                //newOrderRegister.Productid = product.Dicttestitemid;
                //newOrderRegister.Productname = product.Testname;
                //newOrderRegister.Id = grouptest.Dicttestitemid;
                //newOrderRegister.Code = grouptest.Testcode;
                //newOrderRegister.Name = grouptest.Testname;
                //newOrderRegister.Type = grouptest.Testtype;
                //newOrderRegister.Isadd = "0";
                //if (isinsert)
                //{
                //    newOrderRegister.Isadd = "1";
                //    newOrderRegister.Adduserid = userinfo.userId;///追加人,ID
                //}
                //newOrderRegister.Isactive = "1";
                //newOrderRegister.Billed = "0";
                //newOrderRegister.Sendbilled = "0";

                //////获取外包客户
                //Dictproductdetail detail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == product.Dicttestitemid && c.Testgroupid == grouptest.Dicttestitemid).First<Dictproductdetail>();
                //newOrderRegister.Sendoutcustomerid = detail.Sendoutcustomerid;

                //newOrderRegister.Tubegroup = grouptest.Tubegroup;
                //newOrderRegister.Barcode = barccode;
                //_gridtestList.Add(newOrderRegister);
            }

            return msg;

        }
        /// <summary>添加组合|单项
        /// 添加组合|单项
        /// </summary>
        /// <param name="_gridtestList">ref 列表集合</param>
        /// <param name="sex">性别value  F M U</param>
        /// <param name="productid">套餐ID</param>
        /// <param name="grouptestid">组合项目ID</param>
        /// <param name="isinsert">是否追加</param>
        /// <param name="userinfo">登录用户信息</param>
        /// <param name="ProductDetail">套餐对组合</param>
        /// <returns></returns>
        public string AddGroupTest(ref  List<OrderRegister> _gridtestList, string sex, double? productid, double? grouptestid, bool isinsert, UserInfo userinfo, List<Dictproductdetail> ProductDetail, string barcode, bool flag = false)
        {
            string msg = string.Empty;

            Dicttestitem product = SelectsTestItemListById(productid);
            //grouptestname = product.Testname;
            //还未添加项目则new一个
            if (_gridtestList == null) { _gridtestList = new List<OrderRegister>(); }

            Dicttestitem grouptest = SelectsTestItemListById(grouptestid);
            //校验性别是否符合
            msg = checkSex(grouptest.Dicttestitemid, sex);
            if (msg != string.Empty) { return msg; }

            bool isNewBarcode = true;
            string barccode = "";
            foreach (OrderRegister item in _gridtestList)
            {
                msg = checkInsert(item, grouptest, product.Testname);
                //  此处判断组合存在，或者性别不允许就不需要允许添加
                if (msg != string.Empty) { return msg; }

                Dicttestitem itemtest = SelectsTestItemListById(item.Id);
                //  所属科室 未维护
                if (grouptest.Dictlabdeptid == null || itemtest.Dictlabdeptid == null)
                {
                    return string.Format("可能[{0}]与[{1}]之一的[所属科室]未维护", grouptest.Testname, itemtest.Testname);
                }
                //  标本类型 未维护
                if (grouptest.Dictspecimentypeid == null || itemtest.Dictspecimentypeid == null)
                {
                    return string.Format("可能[{0}]与[{1}]之一的[标本类型]未维护", grouptest.Testname, itemtest.Testname);
                }
                ////  此处判断分管原则相同，所属科室也要相同
                //if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictlabdeptid != grouptest.Dictlabdeptid) 
                //{ 
                //    return string.Format("[{0}]与[{1}]的分管原则相同，但[所属科室]不同", grouptest.Testname, itemtest.Testname); 
                //}
                //  此处判断分管原则相同，标本类型也要相同
                if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictspecimentypeid != grouptest.Dictspecimentypeid)
                {
                    return string.Format("[{0}]与[{1}]的分管原则相同，但[标本类型]不同", grouptest.Testname, itemtest.Testname);
                }

                //分管原则相同时 没有标本接受时使用以前条码
                if ((Convert.ToInt32(item.Status)) < ((int)ParamStatus.OrderbarcodeStatus.Received) && item.Tubegroup == grouptest.Tubegroup)
                {
                    isNewBarcode = false;
                    if (flag)//易感基因条码扫描，条码递增区分
                        barccode = (Convert.ToInt64(item.Barcode) + 1).ToString();
                    else
                        barccode = item.Barcode;
                }
            }
            //判断使用原条码还是新条码
            if (isNewBarcode)
            {
                if (barcode != null)
                {
                    if (flag)//易感基因条码扫描，条码尾数从1开始
                    {
                        barcode = (Convert.ToInt64(barcode) + 1).ToString();
                        barccode = GetBarCode(barcode);
                    }
                    else
                    {
                        barccode = GetBarCode(barcode);
                    }
                }
                else
                {
                    barccode = GetBarCode();
                }
            }
            OrderRegister newOrderRegister = new OrderRegister();
            newOrderRegister.Productid = product.Dicttestitemid;
            newOrderRegister.Productname = product.Testname;
            newOrderRegister.Id = grouptest.Dicttestitemid;
            newOrderRegister.Code = grouptest.Testcode;
            newOrderRegister.Name = grouptest.Testname;
            newOrderRegister.Type = grouptest.Testtype;
            newOrderRegister.Isadd = "0";
            if (isinsert)
            {
                newOrderRegister.Isadd = "1";
                newOrderRegister.Adduserid = userinfo == null ? 4 : userinfo.userId;///追加人,ID
            }
            newOrderRegister.Isactive = "1";
            newOrderRegister.Billed = "0";
            newOrderRegister.Sendbilled = "0";
            newOrderRegister.Tubegroup = grouptest.Tubegroup;
            newOrderRegister.Barcode = barccode;
            //获取外包客户
            newOrderRegister.Sendoutcustomerid = -1;
            if (ProductDetail != null)
            {
                IEnumerable<Dictproductdetail> IEdetail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == product.Dicttestitemid && c.Testgroupid == grouptest.Dicttestitemid);
                if (IEdetail.Count() > 0)
                {
                    newOrderRegister.Sendoutcustomerid = IEdetail.First<Dictproductdetail>().Sendoutcustomerid ?? -1;
                }
            }

            _gridtestList.Add(newOrderRegister);

            return msg;
        }

        #region 自动扫描上传订单 lsp
        /// <summary>添加套餐(自动扫描程序用)
        /// 添加套餐
        /// </summary>
        /// <param name="sex">性别value  F M U</param>
        /// <param name="productid">套餐ID</param>
        /// <param name="isinsert">是否追加</param>
        /// <param name="userinfo">登录用户信息</param>
        /// <param name="productname">ref 套餐名</param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public string AddProductAuto(string sex, Dicttestitem product, string barcode)
        {
            string msg = string.Empty;
            List<Dictproductdetail> productdetaillist = loginservice.GetDictproductdetailByProductID(product.Dicttestitemid);
            if (productdetaillist == null || productdetaillist.Count == 0)
            {
                msg = "无法获取套餐[" + product.Testname + "]的套餐明细";
                return msg;
            }
            List<OrderRegister> _gridtestList = null; 
            foreach (Dictproductdetail group in productdetaillist)
            {
                msg = AddGroupTestAuto(ref _gridtestList,sex, product, group.Testgroupid,productdetaillist, barcode);
                if (msg != string.Empty) { return msg; }
            }
            return msg;
        }
        
        /// <summary> 添加组合|单项
        /// 添加组合|单项
        /// </summary>
        /// <param name="sex">性别value  F M U</param>
        /// <param name="productid">套餐</param>
        /// <param name="grouptestid">组合项目ID</param>
        /// <param name="ProductDetail">套餐对组合</param>
        /// <returns></returns>
        public string AddGroupTestAuto(ref List<OrderRegister> _gridtestList, string sex, Dicttestitem product, double? grouptestid,List<Dictproductdetail> ProductDetail, string barcode)
        {
            string msg = string.Empty;
            //还未添加项目则实例化一个空容器
            if (_gridtestList == null) { _gridtestList = new List<OrderRegister>(); }
            Dicttestitem grouptest = SelectDicttestitemByDicttestitemid(grouptestid);
            //校验性别是否符合
            msg = checkSexAuto(grouptest, sex, product.Testname);
            if (msg != string.Empty) { return msg; }

            bool isNewBarcode = true;
            string barccode = "";
            foreach (OrderRegister item in _gridtestList)
            {
                msg = checkInsertAuto(item, grouptest, product.Testname);
                //  此处判断组合存在，或者性别不允许就不需要允许添加
                if (msg != string.Empty) { return msg; }

                Dicttestitem itemtest = SelectDicttestitemByDicttestitemid(item.Id);
                //  所属科室 未维护
                if (grouptest.Dictlabdeptid == null || itemtest.Dictlabdeptid == null)
                {
                    return string.Format("可能[{0}]与[{1}]之一的[所属科室]未维护", grouptest.Testname, itemtest.Testname);
                }
                //  标本类型 未维护
                if (grouptest.Dictspecimentypeid == null || itemtest.Dictspecimentypeid == null)
                {
                    return string.Format("可能[{0}]与[{1}]之一的[标本类型]未维护", grouptest.Testname, itemtest.Testname);
                }
                ////  此处判断分管原则相同，所属科室也要相同
                //if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictlabdeptid != grouptest.Dictlabdeptid)
                //{
                //    return string.Format("[{0}]与[{1}]的分管原则相同，但[所属科室]不同", grouptest.Testname, itemtest.Testname);
                //}
                ////  此处判断分管原则相同，标本类型也要相同
                //if (item.Tubegroup == grouptest.Tubegroup && itemtest.Dictspecimentypeid != grouptest.Dictspecimentypeid)
                //{
                //    return string.Format("[{0}]与[{1}]的分管原则相同，但[标本类型]不同", grouptest.Testname, itemtest.Testname);
                //}

                //分管原则相同时 没有标本接受时使用以前条码
                if ((Convert.ToInt32(item.Status)) < ((int)ParamStatus.OrderbarcodeStatus.Received) && item.Tubegroup == grouptest.Tubegroup)
                {
                    isNewBarcode = false;
                    barccode = item.Barcode;
                }
            }
            //判断使用原条码还是新条码
            if (isNewBarcode)
            {
                if (barcode != null)
                {
                    barccode = GetBarCode(barcode);
                }
                else
                {
                    barccode = GetBarCode();
                }
            }
            OrderRegister newOrderRegister = new OrderRegister();
            newOrderRegister.Productid = product.Dicttestitemid;
            newOrderRegister.Productname = product.Testname;
            newOrderRegister.Id = grouptest.Dicttestitemid;
            newOrderRegister.Code = grouptest.Testcode;
            newOrderRegister.Name = grouptest.Testname;
            newOrderRegister.Type = grouptest.Testtype;
            newOrderRegister.Isadd = "0";
            //if (isinsert)
            //{
            //    newOrderRegister.Isadd = "1";
            //    newOrderRegister.Adduserid = userinfo;///追加人,ID
            //}
            newOrderRegister.Isactive = "1";
            newOrderRegister.Billed = "0";
            newOrderRegister.Sendbilled = "0";
            newOrderRegister.Tubegroup = grouptest.Tubegroup;
            newOrderRegister.Barcode = barccode;
            //获取外包客户
            newOrderRegister.Sendoutcustomerid = -1;
            if (ProductDetail != null)
            {
                IEnumerable<Dictproductdetail> IEdetail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == product.Dicttestitemid && c.Testgroupid == grouptest.Dicttestitemid);
                if (IEdetail.Count() > 0)
                {
                    newOrderRegister.Sendoutcustomerid = IEdetail.First<Dictproductdetail>().Sendoutcustomerid ?? -1;
                }
            }
            _gridtestList.Add(newOrderRegister);

            return msg;
        }

        /// <summary>
        /// 检查项目性别是否符合设置
        /// </summary>
        /// <param name="testitem"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public string checkSexAuto(Dicttestitem testitem, string sex,string productname)
        {
            string str = string.Empty;
            if (testitem.Testtype == "0")
            {
                if (testitem.Forsex != "B" && sex != "U" && testitem.Forsex != sex)
                {
                    str = string.Format("套餐[{0}]中的项目[{1}]适合性别[{2}]与所选性别[{3}]不匹配！",productname, testitem.Testname, testitem.Forsex == "M" ? "男" : "女", sex == "M" ? "男" : "女");
                }
            }
            else
            {
                List<Dicttestgroupdetail> ienewtest = loginservice.GetDicttestgroupdetailByGroupID(testitem.Dicttestitemid);
                foreach (Dicttestgroupdetail newtest in ienewtest)
                {
                    Dicttestitem newtestdetail = SelectDicttestitemByDicttestitemid(newtest.Dicttestitemid);
                    if (newtestdetail.Forsex != "B" && sex != "U" && newtestdetail.Forsex != sex)
                    {
                        str += string.Format("套餐[{0}]中的组合[{1}]中的项目[{2}]适合性别[{3}]与所选性别[{4}]不匹配！",
                            productname, testitem.Testname, newtestdetail.Testname, newtestdetail.Forsex == "M" ? "男" : "女", sex == "M" ? "男" : "女")+"；";
                    }
                    if (!string.IsNullOrEmpty(str))
                        str = str.TrimEnd('；');
                }
            }
            return str;
        }

        /// <summary>检查是否添加 是否存在组合项目，并且分管原则不为空
        /// 检查是否添加
        /// </summary>
        /// <param name="row">列表数据行</param>
        /// <param name="itemtest">添加的组合|项目</param>
        /// <param name="strproductname">套餐名，添加套餐时使用，添加组合传string.Empty</param>
        public string checkInsertAuto(OrderRegister row, Dicttestitem itemtest, string strproductname)
        {
            string msg = string.Empty;
            List<Dicttestgroupdetail> TestGroupDetailList = loginservice.GetLoginDicttestgroupdetailNoCache(); //组合项目字典

            if (!string.IsNullOrEmpty(strproductname))
            {
                strproductname = String.Format("套餐[{0}]中的", strproductname);
            }
            Dicttestitem rowdict = SelectDicttestitemByDicttestitemid(Convert.ToDouble(row.Id));// SelectsTestItemListById(Convert.ToDouble(row.Id), isCacheData);
            if (itemtest.Testtype == "1")///添加的组合
            {
                double? id = itemtest.Dicttestitemid;
                ///添加的组合
                IEnumerable<Dicttestgroupdetail> ienewtest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == id);

                if (row.IsGroup)///列表行为组合
                {
                    #region >>>> zhouy 列表行 为组合，添加行 为组合

                    if (rowdict.Dicttestitemid == itemtest.Dicttestitemid)
                    {
                        msg = string.Format("您添加的{0}组合[{1}]已经在列表中了！", strproductname, itemtest.Testname);
                        return msg;
                    }
                    #endregion

                }
                else///列表行 为单项
                {
                    #region >>>> zhouy 列表行 为单项，添加行 为组合

                    foreach (Dicttestgroupdetail newtest in ienewtest)
                    {
                        Dicttestitem newtestdetail = SelectDicttestitemByDicttestitemid(newtest.Dicttestitemid);// SelectsTestItemListById(newtest.Dicttestitemid, isCacheData);
                        if (newtest.Dicttestitemid == rowdict.Dicttestitemid)
                        {
                            msg = string.Format("您添加的{0}组合[{1}]中的项目[{2}]已经在列表中了！", strproductname, itemtest.Testname, rowdict.Testname);
                            return msg;
                        }
                    }

                    #endregion
                }

                if (itemtest.Tubegroup == null)
                {
                    msg = string.Format("您添加的{0}组合[{1}]未维护好分管原则！", strproductname, itemtest.Testname);
                    return msg;
                }
            }
            else///单项
            {
                if (row.IsGroup)///列表行为组合
                {
                    #region >>>> zhouy 列表行 为组合，添加行 为单项

                    IEnumerable<Dicttestgroupdetail> ietest = TestGroupDetailList.Where<Dicttestgroupdetail>(c => c.Testgroupid == rowdict.Dicttestitemid);
                    foreach (Dicttestgroupdetail test in ietest)
                    {
                        if (test.Dicttestitemid == itemtest.Dicttestitemid)
                        {
                            msg = string.Format("列表组合[{0}]中的已经包含了您添加的{1}项目[{2}]！", rowdict.Testname, strproductname, itemtest.Testname);
                            return msg;
                        }
                    }

                    #endregion
                }
                else///单项
                {
                    #region >>>> zhouy 列表行 为单项，添加行 为单项

                    if (rowdict.Dicttestitemid == itemtest.Dicttestitemid)
                    {
                        msg = string.Format("您添加的{0}项目[{1}]已经在列表中了！", strproductname, itemtest.Testname);
                        return msg;
                    }

                    #endregion
                }
                if (itemtest.Tubegroup == null)
                {
                    msg = string.Format("您添加的{0}项目[{1}]未维护好分管原则！", strproductname, itemtest.Testname);
                    return msg;
                }
            }
            return msg;
        }
        /// <summary>
        /// 根据Dicttestitemid获取单条字典信息
        /// </summary>
        /// <param name="dicttestitemid"></param>
        /// <returns></returns>
        public Dicttestitem SelectDicttestitemByDicttestitemid(double? dicttestitemid)
        {
            return testitemservice.SelectDicttestitemByDicttestitemid(dicttestitemid);
        }
        /// <summary>添加|修改订单
        /// 添加|修改订单
        /// </summary>
        /// <param name="type">模块名称 0:体检登记, 1:单位批量上传</param>
        /// <param name="IsInsert">是否添加</param>
        /// <param name="TestGroupDetailList">组合 对 单个项目字典</param>
        /// <param name="labid">分点ID</param>
        /// <param name="iegroupitem">订单中拆分出组合的集合</param>
        /// <param name="member">会员对象</param>
        /// <param name="_orders">orders表对象</param>
        /// <returns></returns>
        public bool insertUpdateOrdersAuto(string type, string Content, bool IsInsert, List<Dicttestitem> productList, List<Dicttestitem> grouptestList, Dictmember member, Orders _orders, string ReceivedBarcode, ref string error, Hashtable htScan = null)
        {
            //sql集合
            SortedList SQLlist = new SortedList(new MySort());
            //拼接barcode写日志
            string strbarcode = string.Empty;
            try
            {
                List<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemListNoCache();///项目字典表
                //报告模版ID
                string reporttemplateidStr = "";
                //按组合分管原则分管
                IEnumerable<IGrouping<string, Dicttestitem>> iegroupitem = grouptestList.Where<Dicttestitem>(c => !ReceivedBarcode.Contains(c.Barcode)).GroupBy<Dicttestitem, string>(c => c.Barcode);

                //暂存添加到组合表的数据
                List<Ordergrouptest> ordergrouptestlist = new List<Ordergrouptest>();

                if (member != null)
                {
                    SQLlist.Add(new Hashtable() { { member.isAdd ? "INSERT" : "UPDATE", member.isAdd ? "Dict.InsertDictmember" : "Dict.UpdateDictmember" } }, member);
                }

                SQLlist.Add(new Hashtable() { { IsInsert == true ? "INSERT" : "UPDATE", IsInsert == true ? "Order.InsertOrders" : "Order.UpdateOrders" } }, _orders);

                if (!IsInsert)
                {
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrdergrouptestByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderbarcodeByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderproductsByOrderNum" } }, _orders.Ordernum);
                    SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrdertestByOrderNum" } }, new Hashtable() { { "ordernum", _orders.Ordernum }, { "barcode", ReceivedBarcode } });

                    //更改测试组合明细的 是否测试状态
                    IEnumerable<Dicttestitem> Blist = grouptestList.Where<Dicttestitem>(c => ReceivedBarcode.Contains(c.Barcode));
                    foreach (Dicttestitem item in Blist)
                    {
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestActive" } }, new Hashtable() { { "isactive", item.IsActive }, { "ordernum", _orders.Ordernum }, { "barcode", item.Barcode } });
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdertestActive" } }, new Hashtable() { { "isactive", item.IsActive }, { "ordernum", _orders.Ordernum }, { "barcode", item.Barcode } });
                    }
                }
                else
                {
                    //添加必检查项目
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdernexttestMustExam" } }, _orders.Ordernum);

                }

                //获取会员多个项目上一次结果集
                List<Ordertest> LastTestResultList = GetLastTestResult(_orders.Dictmemberid);

                if (iegroupitem.Count() > 0)
                {
                    //条码分管
                    foreach (IGrouping<string, Dicttestitem> item in iegroupitem)
                    {
                        //条码下的组合和项目
                        List<Dicttestitem> groupitem = item.ToList<Dicttestitem>();
                        string tubegroup = groupitem[0].Tubegroup;
                        if (tubegroup == null || tubegroup == string.Empty)
                        {
                            throw new Exception(string.Format("{0}[{1}]没有维护分管原则!", groupitem[0].Testtype == "1" ? "组合" : "单项", groupitem[0].Testname));
                        }
                        string barcode = item.Key;//母条码

                        #region >>>> zhouy insert Orderbarcode

                        Orderbarcode _orderbarcode = new Orderbarcode();

                        //有此分管原则  则取之前条码，以及..账单状态
                        IList<Orderbarcode> tempbarcodeList = SelectMemberById(_orders.Ordernum);
                        IEnumerable<Orderbarcode> IEtempbarcodeList = tempbarcodeList.Where<Orderbarcode>(c => c.Tubegroup == tubegroup);
                        if (IEtempbarcodeList.Count() > 0)
                        {
                            Orderbarcode tempOrderbarcode = IEtempbarcodeList.First<Orderbarcode>();
                            _orderbarcode.Status = tempOrderbarcode.Status;
                            _orderbarcode.Collectdate = tempOrderbarcode.Collectdate;
                            _orderbarcode.Collectby = tempOrderbarcode.Collectby;
                        }

                        _orderbarcode.Orderbarcodeid = getSeqID("SEQ_ORDERBARCODE");
                        _orderbarcode.Ordernum = _orders.Ordernum;
                        _orderbarcode.Barcode = barcode;                              //母条码               
                        _orderbarcode.Tubegroup = tubegroup;                           //分管原则
                        #endregion

                        string testnames = "";
                        //组合
                        List<string> lstDictlabdeptid = new List<string>();
                        foreach (Dicttestitem groupdetail in groupitem)
                        {
                            #region >>>> zhouy insert Ordergrouptest

                            Ordergrouptest _ordergrouptest = new Ordergrouptest();
                            _ordergrouptest.Ordergrouptestid = getSeqID("SEQ_ORDERGROUPTEST");
                            _ordergrouptest.Ordernum = _orders.Ordernum;
                            _ordergrouptest.Barcode = barcode;
                            _ordergrouptest.Dictproductid = groupdetail.Productid;  //套餐ID
                            _ordergrouptest.Dicttestitemid = groupdetail.Dicttestitemid;//套餐下的组合ID，当用户选单项存单项ID，对应DICTTESTITEM表id
                            _ordergrouptest.Engname = groupdetail.Engname;//测试项英文名
                            _ordergrouptest.Testname = groupdetail.Testname;//测试项中文名
                            _ordergrouptest.Testcode = groupdetail.Testcode;//测试项编号
                            _ordergrouptest.Tubegroup = groupdetail.Tubegroup;//分管原则（只有TESTTYPE=0需要维护）与dictlibary表dictlibaryid关联
                            _ordergrouptest.Dictlabdeptid = groupdetail.Dictlabdeptid;//测试项物理实验室分组,对应表DICTLABDEPT
                            //_ordergrouptest.Standardprice = groupdetail.Price;//达安标准价        |
                            //_ordergrouptest.Groupprice =//达安不同分点的价钱                      |
                            //_ordergrouptest.Contractprice =//应收价钱(合同价)                     |调用方法统一算
                            //_ordergrouptest.Sendoutprice =//外包成交价                            |
                            //_ordergrouptest.Finalprice =//成交价                                  | 
                            //_ordergrouptest.Issendouttest = groupdetail.Sendoutcustomerid == null ? "0" : "1";//是否外包
                            _ordergrouptest.Sendoutcustomerid = groupdetail.Sendoutcustomerid;//外包客户ID
                            _ordergrouptest.Displayorder = groupdetail.Displayorder; //项目字典表取
                            _ordergrouptest.Isadd = groupdetail.Isadd;//是否追加
                            _ordergrouptest.Adduserid = groupdetail.Adduserid;//追加人ID  
                            _ordergrouptest.Productname = groupdetail.Productname;//套餐名
                            _ordergrouptest.Operationremark = groupdetail.Operationremark;//执行说明
                            _ordergrouptest.Isactive = groupdetail.IsActive ?? "1";//是否继续检测
                            _ordergrouptest.Billed = groupdetail.Billed;//账单
                            _ordergrouptest.Sendbilled = groupdetail.Sendbilled;//外包账单
                            _ordergrouptest.Sendoutcustomerid = groupdetail.Sendoutcustomerid;//外包客户
                            //时间用于计算价格
                            _ordergrouptest.Createdate = _orders.Createdate;

                            ordergrouptestlist.Add(_ordergrouptest);

                            #endregion

                            if (groupdetail.Testtype == "1")//组合
                            {
                                #region >>>> zhouy insert OrderTest

                                //获得组合下明细
                                //IEnumerable<Dicttestgroupdetail> _testdetaillist = TestGroupDetailList.Where(c => c.Testgroupid == groupdetail.Dicttestitemid);
                                List<Dicttestgroupdetail> _testdetaillist = loginservice.GetDicttestgroupdetailByGroupID(groupdetail.Dicttestitemid);
                                //明细项目
                                foreach (Dicttestgroupdetail testdeteail in _testdetaillist)
                                {
                                    Dicttestitem _testitem = TestItemList.Where<Dicttestitem>(x => x.Dicttestitemid == testdeteail.Dicttestitemid).First<Dicttestitem>();

                                    //添加明细项目
                                    if (_testitem == null) { continue; }

                                    //获取上一次结果
                                    IEnumerable<Ordertest> IEOrdertest = LastTestResultList.Where<Ordertest>(c => c.Dicttestitemid == testdeteail.Dicttestitemid);
                                    Ordertest lasttest = new Ordertest();
                                    if (IEOrdertest.Count() > 0)
                                    {
                                        lasttest = IEOrdertest.First<Ordertest>();
                                    }

                                    Ordertest _ordertest = new Ordertest();
                                    _ordertest.Ordertestid = getSeqID("SEQ_ORDERTEST");
                                    _ordertest.Ordernum = _orders.Ordernum;
                                    _ordertest.Barcode = barcode;
                                    _ordertest.Dictproductsid = groupdetail.Productid; //产品ID对应DICTTESTITEM表ID
                                    _ordertest.Dictgroupid = groupdetail.Dicttestitemid;//组合ID对应DICTTESTITEM表ID
                                    _ordertest.Dictgroupname = groupdetail.Testname;//组合名称
                                    _ordertest.Dicttestitemid = _testitem.Dicttestitemid;//明细项目ID对应DICTTESTITEM表ID
                                    _ordertest.Engname = _testitem.Engname;//测试项英文名
                                    _ordertest.Englongname = _testitem.Englongname;//英文长名称（只有TESTTYPE=0需要维护）
                                    _ordertest.Testcode = _testitem.Testcode;//测试项编号
                                    _ordertest.Testname = _testitem.Testname;//明细项目名称
                                    _ordertest.Lastdate = lasttest.Createdate;//上一次时间
                                    _ordertest.Lastresult = lasttest.Testresult;//上一次结果
                                    _ordertest.Report = _testitem.Report;//项目字典表
                                    _ordertest.Displayorder = _testitem.Displayorder;//项目字典表
                                    _ordertest.Testresulttype = _testitem.Resulttype;//项目字典表
                                    _ordertest.Dictlabdeptid = _testitem.Dictlabdeptid;//项目字典表
                                    _ordertest.Unit = _testitem.Unit;
                                    _ordertest.Isimportant = _testitem.Isimportant;
                                    _ordertest.Isactive = groupdetail.IsActive ?? "1";

                                    reporttemplateidStr += reporttemplateidStr.Contains(_testitem.Dictreporttemplateid.ToString()) ? "" : (_testitem.Dictreporttemplateid + ",");
                                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdertest" } }, _ordertest);
                                }
                                #endregion
                            }
                            else if (groupdetail.Testtype == "0")//单项
                            {
                                #region >>>> zhouy insert OrderTest

                                //获取上一次结果
                                IEnumerable<Ordertest> IEOrdertest = LastTestResultList.Where<Ordertest>(c => c.Dicttestitemid == groupdetail.Dicttestitemid);
                                Ordertest lasttest = new Ordertest();
                                if (IEOrdertest.Count() > 0)
                                {
                                    lasttest = IEOrdertest.First<Ordertest>();
                                }
                                Ordertest _ordertest = new Ordertest();
                                _ordertest.Ordertestid = getSeqID("SEQ_ORDERTEST");
                                _ordertest.Ordernum = _orders.Ordernum;
                                _ordertest.Barcode = barcode;
                                _ordertest.Dictproductsid = groupdetail.Productid;//产品ID对应DICTTESTITEM表ID
                                _ordertest.Dictgroupid = null;//组合ID对应DICTTESTITEM表ID
                                _ordertest.Dictgroupname = null;//组合名称
                                _ordertest.Dicttestitemid = groupdetail.Dicttestitemid;//明细项目ID对应DICTTESTITEM表ID
                                _ordertest.Engname = groupdetail.Engname;//测试项英文名
                                _ordertest.Englongname = groupdetail.Englongname;//英文长名称（只有TESTTYPE=0需要维护）
                                _ordertest.Testcode = groupdetail.Testcode;//测试项编号
                                _ordertest.Testname = groupdetail.Testname;//明细项目名称
                                _ordertest.Lastdate = lasttest.Createdate;//上一次时间
                                _ordertest.Lastresult = lasttest.Testresult;//上一次结果
                                _ordertest.Report = groupdetail.Report;//项目字典表
                                _ordertest.Displayorder = groupdetail.Displayorder;//项目字典表
                                _ordertest.Testresulttype = groupdetail.Resulttype;//项目字典表
                                _ordertest.Dictlabdeptid = groupdetail.Dictlabdeptid;//项目字典表
                                _ordertest.Unit = groupdetail.Unit;
                                _ordertest.Isimportant = groupdetail.Isimportant;
                                _ordertest.Isactive = groupdetail.IsActive ?? "1";

                                SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdertest" } }, _ordertest);
                                #endregion
                                reporttemplateidStr += reporttemplateidStr.Contains(groupdetail.Dictreporttemplateid.ToString()) ? "" : (groupdetail.Dictreporttemplateid + ",");
                            }
                            testnames += groupdetail.Testname + ",";
                            if (!lstDictlabdeptid.Contains(groupdetail.Dictlabdeptid.ToString()))
                            {
                                lstDictlabdeptid.Add(groupdetail.Dictlabdeptid.ToString());
                            }
                        }
                        _orderbarcode.Specimentypeid = groupitem[0].Dictspecimentypeid;
                        //_orderbarcode.Dictlabdeptid = groupitem[0].Dictlabdeptid;
                        //存在多个科室，且第一个科室为物理检查科，则取第二个
                        if (lstDictlabdeptid.Count > 1 && lstDictlabdeptid[0] == "1")
                        {
                            _orderbarcode.Dictlabdeptid = Convert.ToDouble(lstDictlabdeptid[1]);
                        }
                        else
                        {
                            _orderbarcode.Dictlabdeptid = groupitem[0].Dictlabdeptid;
                        }
                        strbarcode += _orderbarcode.Barcode + ",";
                        _orderbarcode.Testnames = testnames.TrimEnd(',');//多个项目用逗号隔开
                        SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderbarcode" } }, _orderbarcode);
                    }

                    //取模版ID
                    string[] reporttemplateid = reporttemplateidStr.TrimEnd(',').Split(',');
                    if (reporttemplateidStr == "" && iegroupitem.Count() > 0)
                    {
                        throw new Exception("该套餐所有组合项目均未维护模版！");
                    }

                    if (reporttemplateid.Length == 1)
                    {
                        _orders.Dictreporttemplateid = Convert.ToDouble(reporttemplateid[0]);
                    }
                    else if (reporttemplateid.Length > 1)
                    {
                        _orders.Dictreporttemplateid = SelectDictreporttemplateByCommonRep();
                    }

                    //获取价格
                    new CommonFuncLibService().OrdergrouptestNewPrice(ordergrouptestlist, new Hashtable()
                    { 
                    { "dictlabid", _orders.Dictlabid }, 
                    { "flag", "nosendout" }, 
                    { "customerid", _orders.Dictcustomerid } });
                    //循环插入Insert Ordergrouptest
                    foreach (Ordergrouptest item in ordergrouptestlist)
                    {
                        SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrdergrouptest" } }, item);
                    }
                }
                #region >>>> zhouy insertOrderproducts
                //循环套餐
                foreach (Dicttestitem item in productList)
                {
                    Orderproducts _orderproducts = new Orderproducts();
                    _orderproducts.Orderproductsid = getSeqID("SEQ_ORDERPRODUCTS");
                    _orderproducts.Ordernum = _orders.Ordernum;
                    _orderproducts.Dicttestitemid = item.Dicttestitemid;//套餐ID，当套餐被拆散存拆散后的组合ID，当用户选单项存单项ID，对应dicttestitem表id
                    _orderproducts.Engname = item.Engname;
                    _orderproducts.Testcode = item.Testcode;
                    _orderproducts.Testname = item.Testname;
                    _orderproducts.Standardprice = null;// item.Price;//达安标准价  套餐不计算
                    _orderproducts.Groupprice = null;// priceitem == null ? item.Price : priceitem.Price;//达安不同分点的价钱  套餐不计算
                    _orderproducts.Finalprice = null;// item.Price;//成交价=达安标准价  套餐不计算
                    _orderproducts.Contractprice = null;//item.Price;//应收价钱(合同价)=达安标准价  套餐不计算
                    _orderproducts.Displayorder = item.Displayorder;//项目字典表取
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderproducts" } }, _orderproducts);
                }
                #endregion

                //更改订单上传社区状态
                SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateTransed" } }, _orders.Ordernum);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            bool b = ExecuteSqlTran(SQLlist, ref error);

            //记录日志
            AddOperationLog(_orders.Ordernum, strbarcode.TrimEnd(','), type, Content, IsInsert ? "节点信息" : "修改留痕", IsInsert ? "添加" : "修改", htScan);

            return b;
        }

        #endregion

        public double SelectDictreporttemplateByCommonRep()
        {
            return Convert.ToDouble(selectObj<object>("Dict.SelectDictreporttemplateByCommonRep", null));
        }
        //批量修改订单单位
        public int UpdateCustomerByOrdernum(Hashtable ht)
        {
            return update("Order.UpdateCustomer", ht);
        }
    }
}
