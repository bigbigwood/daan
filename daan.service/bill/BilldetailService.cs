using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;
using daan.util.Common;
using System.Web;
using daan.service.dict;
using System.Data;
using daan.service.login;

namespace daan.service.bill
{
    public class BilldetailService : BaseService
    {
        /// <summary>
        /// 根据订单号查找账单信息
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public List<Billdetail> GetBilldetailByOrderNum(string ordernum)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectBilldetailByOrderNum", ordernum).ToList();
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 体检流水号查询客户的检测项目列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Billdetail> SelectBilldetailPriceList(Hashtable ht)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectBilldetailPriceList", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件分页查询账单明细列表
        /// </summary>
        /// <returns></returns>
        public IList<Billdetail> SelectBilldetailListForPage(string sql, Hashtable ht)
        {
            try
            {
                if (sql == "PrepareOut")
                    return this.QueryList<Billdetail>("Bill.SelectBilldetailInfoPrepareOutForPage", ht);
                else
                    return this.QueryList<Billdetail>("Bill.SelectBilldetailInfoForPage", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件查询账单明细列表总记录数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int SelectBilldetailListCount(string sql, Hashtable ht)
        {
            try
            {
                if (sql == "PrepareOut")
                    return int.Parse(this.selectIList("Bill.SelectBilldetailInfoPrepareOutCount", ht)[0].ToString());
                else
                    return int.Parse(this.selectIList("Bill.SelectBilldetailInfoCount", ht)[0].ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得账单明细价格统计信息
        /// </summary>
        /// <returns></returns>
        public IList<Billdetail> GetBilldetailStatisticsByOrdernum(string sql, Hashtable ht)
        {
            try
            {
                if (sql == "PrepareOut")
                    return this.QueryList<Billdetail>("Bill.SelectBilldetailPrepareOutPriceStatistics", ht);
                else
                    return this.QueryList<Billdetail>("Bill.SelectBilldetailPriceStatistics", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据查询条件账单明细用于打印和导出操作
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Billdetail> SelectBilldetailInfoList(string billheadid)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectBilldetailInfo", billheadid);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 外包帐单明细打印
        /// </summary>
        /// <param name="billheadid"></param>
        /// <returns></returns>
        public IList<Billdetail> SelectSendOutBillDetailInfoPrint(string billheadid)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectSendOutBilldetailInfo", billheadid);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <returns></returns>
        public int UpdateBilldetailRemark(Hashtable ht)
        {
            int result = 0;
            try
            {
                result = this.update("Bill.UpdateBilldetailRemark", ht);

                if (!string.IsNullOrEmpty(ht["remark"].ToString()))
                    AddOperationLog(ht["ordernum"].ToString(), "", "财务管理", "", "财务备注", ht["remark"].ToString());
                if (!string.IsNullOrEmpty(ht["selfremark"].ToString()))
                    AddOperationLog(ht["ordernum"].ToString(), "", "财务管理", "", "财务备注", ht["selfremark"].ToString());

                return result;
            }
            catch (Exception ex)
            {
                return -1;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改实收价格的日志记录
        /// </summary>
        /// <returns></returns>
        public int UpdateBilldetailFinalpriceLog(object olddetail, object newdetail, string ordernum)
        {
            try
            {
                string content = GetModifyString(olddetail, newdetail);
                AddOperationLog(ordernum, "", "财务管理", content, "调整价格", "");
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 预出账时查询所有账单明细编号、体检流水号、检测项目编号
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Billdetail> SelectBilldetailList(Hashtable ht)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectBilldetailList", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 事务处理预出账查询
        /// </summary>
        /// <returns></returns>
        public bool BillPrepareOutSearch(Hashtable ht)
        {
            UserInfo userInfo = GetUserInfo();
            OrdergrouptestService ordergrouptestService = new OrdergrouptestService();
            CommonFuncLibService comm = new CommonFuncLibService();

            try
            {
                //获取grouptest收费信息，并重新算价
                IList<Ordergrouptest> grouptestList = ordergrouptestService.GetOrdergrouptestPrice(ht);
                IList<Ordergrouptest> grouptestNewPriceList = comm.OrdergrouptestNewPrice(grouptestList, ht);

                //删除billdetail表老记录
                SortedList SQLlist = new SortedList(new MySort());
                SQLlist.Add(new Hashtable() { { "DELETE", "Bill.DeleteBilldetailBySearch" } }, ht);

                //插入重组后的集合grouptest到数据到表 BILLDETAIL中
                foreach (Ordergrouptest test in grouptestNewPriceList)
                {
                    Billdetail detail = new Billdetail();
                    detail = SetBillDetail(null, test.Ordernum, test,"");

                    detail.Billdetailid = this.getSeqID("SEQ_BILLDETAIL");
                    SQLlist.Add(new Hashtable() { { "INSERT", "Bill.InsertBilldetail" } }, detail);
                }
                return this.ExecuteSqlTran(SQLlist);
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事物处理"预出账"操作
        /// </summary>
        /// <returns></returns>
        public bool BillPrepareOutOperation(Billhead billhead, Hashtable ht, string flag)
        {
            try
            {
                BilldetailService service = new BilldetailService();
                IList<Billdetail> detailList = service.SelectBilldetailList(ht);
                SortedList SQLlist = new SortedList(new MySort());
                billhead.Billheadid = this.getSeqID("SEQ_BILLHEAD");
                billhead.Invoiceno = billhead.Billheadid.ToString();
                SQLlist.Add(new Hashtable() { { "INSERT", "Bill.InsertBillhead" } }, billhead);

                foreach (Billdetail detail in detailList)
                {
                    //更新BILLDETAIL 表
                    Hashtable htdetail = new Hashtable();
                    htdetail["Billdetailids"] = detail.Billdetailid;
                    htdetail["Billheadid"] = billhead.Billheadid;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBilldetailHeadId" } }, htdetail);

                    //更新ordergrouptest表
                    Hashtable htgrouptest = new Hashtable();
                    htgrouptest["flag"] = flag;
                    htgrouptest["billdetailid"] = detail.Billdetailid;
                    htgrouptest["ordernum"] = detail.Ordernum;
                    htgrouptest["dicttestitemid"] = detail.Dicttestitemid;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestPrice" } }, htgrouptest);
                }
                bool result = this.ExecuteSqlTran(SQLlist);
                if (result)
                {
                    IEnumerator<Billdetail> ordernumList = (from a in detailList
                                                            group a by new { a.Ordernum } into g
                                                            select new Billdetail
                                                            {
                                                                Ordernum = g.Key.Ordernum
                                                            }).ToList().GetEnumerator();

                    //预出账日志
                    while (ordernumList.MoveNext())
                    {
                        this.AddOperationLog(ordernumList.Current.Ordernum, "", "财务管理", "预出账", "节点信息", "");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事物处理"删除标本"
        /// </summary>
        /// <returns></returns>
        public bool BillDeleteSample(string ids, string testitemids, string ordernum, string billheadid, string flag)
        {
            try
            {
                SortedList SQLlist = new SortedList(new MySort());
                SQLlist.Add(new Hashtable() { { "DELETE", "Bill.DeleteBilldetail" } }, ids);

                if (!string.IsNullOrEmpty(billheadid) && int.Parse(billheadid) > 0)
                {
                    string[] testitemidsArray = testitemids.Split(',');
                    //修改ordergouptest表状态
                    for (int i = 0; i < testitemidsArray.Length; i++)
                    {
                        Hashtable ht1 = new Hashtable();
                        ht1["flag"] = flag;
                        ht1["ordernum"] = ordernum;
                        ht1["dicttestitemid"] = testitemidsArray[i];
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestStatus" } }, ht1);
                    }

                    //修改billheader表汇总价格
                    BilldetailService detailservice = new BilldetailService();
                    Hashtable ht = new Hashtable();
                    ht["billdetailids"] = ids;
                    ht["billheadid"] = billheadid;
                    IList<Billdetail> detailList = detailservice.GetBilldetailStatisticsByOrdernum("Out", ht);

                    Hashtable hthead = new Hashtable();
                    hthead["Billheadid"] = billheadid;
                    hthead["Totalstandardprice"] = detailList.Sum(c => c.Standardprice);
                    hthead["Totalgrouprprice"] = detailList.Sum(c => c.Groupprice);
                    hthead["Totalcontractprice"] = detailList.Sum(c => c.Contractprice);
                    hthead["Totalfinalprice"] = detailList.Sum(c => c.Finalprice);
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBillheadTotalPrice" } }, hthead);
                }
                bool result = this.ExecuteSqlTran(SQLlist);
                if (result)
                    //写入日志记录
                    this.AddOperationLog(ordernum, "", "财务管理", "删除标本", "节点信息", "");
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事物处理"调整价钱"
        /// </summary>
        /// <returns></returns>
        public bool UpdateBilldetailFinalprice(IList<Billdetail> oldList, IList<Billdetail> newList, string billheadid, string ordernum, string state)
        {
            try
            {
                bool flag = false;
                bool result = true;

                SortedList SQLlist = new SortedList(new MySort());
                for (int i = 0; i < oldList.Count; i++)
                {
                    Hashtable ht = new Hashtable();
                    ht["billdetailid"] = newList[i].Billdetailid;
                    ht["finalprice"] = newList[i].Finalprice;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBilldetailFinalprice" } }, ht);

                    //修改ordergrouptest表实收价格
                    Hashtable htgrouptest = new Hashtable();
                    htgrouptest["ordernum"] = ordernum;
                    htgrouptest["dicttestitemid"] = newList[i].Dicttestitemid;
                    htgrouptest["price"] = newList[i].Finalprice;
                    htgrouptest["state"] = state;
                   
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestFinalPrice" } }, htgrouptest);

                    if (!oldList[i].Finalprice.Equals(newList[i].Finalprice))
                    {
                        flag = true;
                    }
                }

                //集合中有记录修改时重算BIIHEAD表汇总价钱
                if (!string.IsNullOrEmpty(billheadid) && flag)
                {
                    Hashtable hthead = new Hashtable();
                    hthead["billheadid"] = billheadid;
                    hthead["ordernum"] = ordernum;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBillheadTotalFinalprice" } }, hthead);
                }
                result = this.ExecuteSqlTran(SQLlist);

                if (!flag) //没有修改任一条记录直接返回
                    return result;

                //日志记录
                for (int i = 0; i < oldList.Count; i++)
                {
                    if (!oldList[i].Finalprice.Equals(newList[i].Finalprice))
                    {
                        UpdateBilldetailFinalpriceLog(oldList[i], newList[i], ordernum);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事物处理个人收费"调价"
        /// </summary>
        /// <returns></returns>
        public bool BillIndividualUpdatePrice(IList<Ordergrouptest> oldList, IList<Ordergrouptest> newList,double? customerid, string ordernum, double dictlabid,string remark)
        {
            UserInfo userInfo = GetUserInfo();

            try
            {
                //获得财务清单核对人
                LoginService loginService = new LoginService();

                List<Dictcustomer> customerList = loginService.GetDictcustomer();
                int customercode = (int)ParamStatus.PersonalCustomerID.SingleCustomerCode;
                List<Dictcustomer> customer = (from a in customerList where a.Customercode == customercode.ToString() select a).ToList();

                SortedList SQLlist = new SortedList(new MySort());
                double? d = 0;
                //生成账单头表数据
                Billhead head = new Billhead();
                head.Dictlabid = dictlabid;
                int sta = (int)ParamStatus.BillheadStatus.PrepareOut;
                head.Status = sta.ToString();
                head.Remark = remark;
                head.Createdate = System.DateTime.Now;
                head.Billby = userInfo.userId;
                head.Duedate = System.DateTime.Now;
                head.Receipno = "";
                head.Dictcustomerid = customerid;
                head.Dictcheckbillid = customer.Count > 0 ? customer[0].Dictcheckbillid : null;
                head.Customertype = "0";
                head.Billtype = "个检账单";
                head.Totalcontractprice = newList.Sum(c => c.Contractprice);
                head.Totalfinalprice = newList.Sum(c => c.Finalprice);
                head.Totalstandardprice = newList.Sum(c => c.Standardprice);
                head.Totalgrouprprice = newList.Sum(c => c.Groupprice);
                head.Billheadid = this.getSeqID("SEQ_BILLHEAD");
                head.Invoiceno = head.Billheadid.Value.ToString();
                head.Begindate = DateTime.Now;
                head.Enddate = DateTime.Now;
                SQLlist.Add(new Hashtable() { { "INSERT", "Bill.InsertBillhead" } }, head);

                //循环生成账单明细信息
                foreach (Ordergrouptest test in newList)
                {
                    Billdetail detail = new Billdetail();
                    detail = SetBillDetail(head.Billheadid, ordernum, test, remark);

                    detail.Billdetailid = this.getSeqID("SEQ_BILLDETAIL");
                    SQLlist.Add(new Hashtable() { { "INSERT", "Bill.InsertBilldetail" } }, detail);


                    //更新ordergrouptest表实收价格
                    Hashtable htgrouptest = new Hashtable();
                    htgrouptest["flag"] = "nosendout";
                    htgrouptest["ordernum"] = ordernum;
                    htgrouptest["dicttestitemid"] = test.Dicttestitemid;
                    htgrouptest["finalprice"] = test.Finalprice;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestFinalPrice" } }, htgrouptest);
                }

                bool result = this.ExecuteSqlTran(SQLlist);
                if (result)
                {
                    //修改价钱日志记录
                    for (int i = 0; i < oldList.Count; i++)
                    {
                        if (!oldList[i].Finalprice.Equals(newList[i].Finalprice))
                        {
                            UpdateBilldetailFinalpriceLog(oldList[i], newList[i], ordernum);
                        }
                    }

                    //已付款日志记录
                    AddOperationLog(ordernum, "", "财务管理", "已收费", "节点信息", "");
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

      

        /// <summary>
        /// 设置billdetail赋值
        /// </summary>
        /// <param name="billheadid">billheadid</param>
        /// <param name="ordernum">ordernum</param>
        /// <param name="test">ordergrouptest实体</param>
        /// <returns></returns>
        public Billdetail SetBillDetail(double? billheadid, string ordernum, Ordergrouptest test, string remark)
        {
            UserInfo userInfo = GetUserInfo();
            Billdetail detail = new Billdetail();
            detail.Billheadid = billheadid;
            detail.Ordernum = ordernum;
            detail.Dicttestitemid = test.Dicttestitemid;
            detail.Standardprice = test.Standardprice;
            detail.Groupprice = test.Groupprice;
            detail.Contractprice = test.Contractprice;
            detail.Finalprice = test.Finalprice;
            int stat = (int)ParamStatus.BilldetailStatus.Normal;
            detail.Status = stat.ToString();
            detail.Remark = remark;
            detail.Selfremark = "";
            detail.Createdate = System.DateTime.Now;
            detail.Enterby = userInfo.userId;
            detail.Enterdate = System.DateTime.Now;
            detail.Dictproductid = test.Dictproductid;
            detail.Productname = test.Productname;
            detail.Testname = test.Testname;

            return detail;
        }

        /// <summary>
        /// 根据账单表id获得账单明细信息列表
        /// </summary>
        /// <param name="headids">账单表id</param>
        /// <returns></returns>
        public IList<Billdetail> GetBilldetailListByHeadid(string headids)
        {
            try
            {
                return this.QueryList<Billdetail>("Bill.SelectBilldetailByheadid", headids);
            }
            catch
            {
                return null;
            }
        }
    }
}
