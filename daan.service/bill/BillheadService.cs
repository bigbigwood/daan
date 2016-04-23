using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;
using daan.util.Common;
using System.Data;

namespace daan.service.bill
{
    public class BillheadService : BaseService
    {

        /// <summary>
        /// 根据查询条件获取记录总数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">查询条件</param>
        /// <returns>返回记录集</returns>
        public IList<Billhead> SelectBillheadList(string sql, object param)
        {
            try
            {
                if (sql == "SelectBillheadList")
                    return this.QueryList<Billhead>("Bill.SelectBillheadList", param);
                else
                    return this.QueryList<Billhead>("Bill.SelectBillheadForCashReceive", param);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据查询条件获取记录总数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">查询条件</param>
        /// <returns>返回记录数</returns>
        public int GetBillHeadListCount(string sql, object param)
        {
            try
            {
                if (sql == "SelectBillheadList")
                    return int.Parse(this.selectIList("Bill.SelectBillheadListCount", param)[0].ToString());
                else
                    return int.Parse(this.selectIList("Bill.SelectBillheadForCashReceiveCount", param)[0].ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得现金接收账单价格统计信息
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Billhead> GetCashReceiveStatisticsByOrdernum(Hashtable ht)
        {
            try
            {
                return this.QueryList<Billhead>("Bill.SelectBillheadForCashReceivePriceStatistics", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据编号查找记录用于导出
        /// </summary>
        /// <returns></returns>
        public IList<Billhead> SelectBillHeadListByids(string billheadids)
        {
            try
            {
                return this.QueryList<Billhead>("Bill.SelectBillHeadListByids", billheadids);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据编号查找记录用于打印
        /// </summary>
        /// <param name="billheadids"></param>
        /// <returns></returns>
        public IList<Billhead> SelectBillHeadListForPrintByids(string billheadids)
        {
            try
            {
                return this.QueryList<Billhead>("Bill.SelectBillHeadListForPrintByids", billheadids);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 导出:已出账、已接收账单头查询
        /// </summary>
        /// <returns></returns>
        public IList<Billhead> SelectBillheadExcel(Hashtable ht)
        {
            try
            {
                return this.QueryList<Billhead>("Bill.SelectBillheadExcel", ht);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 退款 更改状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public void BillUpdateBillheadRefundment(Hashtable ht)
        {
            update("Bill.UpdateBillheadRefundment", ht);
        }

        /// <summary>
        /// 事物处理billhead表数据操作:"接收"、"反接收"、"作废"
        /// </summary>
        /// <returns></returns>
        public bool BillheadDataOperation(string ids, int status, string operationtype, string flag)
        {
            try
            {
                string[] id = ids.Split(',');
                SortedList SQLlist = new SortedList(new MySort());
                for (int i = 0; i < id.Length; i++)
                {
                    Hashtable ht = new Hashtable();
                    ht["status"] = status;
                    ht["billheadid"] = id[i];
                    ht["duedate"] = System.DateTime.Now;
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBillheadStatus" } }, ht);
                }

                #region 作废

                int value = (int)ParamStatus.BillheadStatus.Invalid;
                if (status == value)
                {
                    //根据billhead表 ids获得billdetail数据集
                    BilldetailService detailservice = new BilldetailService();
                    IList<Billdetail> detailList = detailservice.GetBilldetailListByHeadid(ids);

                    //将作废数据插入billdetailcancel表
                    foreach (Billdetail detail in detailList)
                    {
                        Billdetailcancel billdetailcancel = new Billdetailcancel();
                        billdetailcancel.Billheadid = detail.Billheadid;
                        billdetailcancel.Ordernum = detail.Ordernum;
                        billdetailcancel.Dicttestitemid = detail.Dicttestitemid;
                        billdetailcancel.Standardprice = detail.Standardprice;
                        billdetailcancel.Groupprice = detail.Groupprice;
                        billdetailcancel.Contractprice = detail.Contractprice;
                        billdetailcancel.Finalprice = detail.Finalprice;
                        billdetailcancel.Status = detail.Status;
                        billdetailcancel.Remark = detail.Remark;
                        billdetailcancel.Selfremark = detail.Selfremark;
                        billdetailcancel.Createdate = System.DateTime.Now;
                        billdetailcancel.Enterby = detail.Enterby;
                        billdetailcancel.Enterdate = detail.Enterdate;
                        billdetailcancel.Dictproductid = detail.Dictproductid;
                        billdetailcancel.Testname = detail.Testname;
                        billdetailcancel.Productname = detail.Productname;
                        billdetailcancel.Billdetailcancelid = this.getSeqID("SEQ_BILLDETAILCANCEL");
                        SQLlist.Add(new Hashtable() { { "INSERT", "Bill.InsertBilldetailcancel" } }, billdetailcancel);

                        //修改ordergrouptest表状态
                        Hashtable grouptest = new Hashtable();
                        grouptest["ordernum"] = detail.Ordernum;
                        grouptest["dicttestitemid"] = detail.Dicttestitemid;
                        grouptest["flag"] = flag;
                        SQLlist.Add(new Hashtable() { { "UPDATE", "Order.UpdateOrdergrouptestStatus" } }, grouptest);
                    }
                    //删除billdetail表记录
                    SQLlist.Add(new Hashtable() { { "DELETE", "Bill.DeleteBilldetailByHeadid" } }, ids);
                }

                #endregion


                bool result = this.ExecuteSqlTran(SQLlist);

                if (result)  //日志操作
                {

                    BilldetailService service = new BilldetailService();
                    Hashtable ht = new Hashtable();
                    ht["billheadids"] = ids;
                    IList<Billdetail> detailList = service.SelectBilldetailList(ht);

                    var ordernumList = (from a in detailList
                                        group a by new { a.Ordernum } into g
                                        select new
                                        {
                                            g.Key.Ordernum
                                        }).ToArray();
                    for (int j = 0; j < ordernumList.Length; j++)
                    {
                        AddOperationLog(ordernumList[j].Ordernum, "", "财务管理", operationtype, "节点信息", "");
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
    }
}
