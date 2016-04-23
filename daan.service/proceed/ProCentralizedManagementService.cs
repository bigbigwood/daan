using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Web;
using daan.util.Common;
using daan.service.bill;
using daan.service.order;

namespace daan.service.proceed
{
    public class ProCentralizedManagementService : BaseService
    {
        /// <summary>
        /// 查询集中管理数据
        /// </summary>
        public DataTable GetManagementOrders(Hashtable ht)
        {
            DataTable dt = this.selectDS("Order.GetManagementOrders", ht).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }

        /// <summary>
        /// 查询集中管理数据总条数
        /// </summary>
        public int GetManagementOrdersCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.GetManagementOrdersCount", ht).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 查询修改单位名称的相关数据
        /// </summary>
        public DataTable GetManagementOrdersByDictmemberid(Hashtable ht)
        {
            DataTable dt = this.selectDS("Order.GetManagementOrdersByDictmemberid", ht).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }

        /// <summary>
        /// 查询修改单位名称的相关数据总条数
        /// </summary>
        public int GetManagementOrdersByDictmemberidCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.GetManagementOrdersByDictmemberidCount", ht).Tables[0].Rows[0][0]);
        }
        /// <summary>
        /// 集中管理-修改单位名称
        /// </summary>
        public bool UpdateDictmemberidByOrderNum(Hashtable ht)
        {
            bool b = true;
            try
            {
                if (this.update("Order.DictmemberidByOrderNum", ht) < 0)
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
        /// <summary>
        /// 集中管理-作废订单
        /// </summary>
        public bool UpdateOrdersCancellation(string username, string strordernum,string reason)
        {
            bool b = true;
            string str = SelectOrderbarcodeByBill(strordernum);
            if (str == null && str == "")
            {
                Hashtable ht = new Hashtable();
                ht["cancelby"] = username;
                ht["ordernum"] = strordernum;
                ht["reason"] = reason;
                try
                {
                    if (this.update("Order.Cancellation", ht) < 0)
                    {
                        b = false;
                    }
                }
                catch (Exception)
                {
                    b = false;
                }
            }
            else
            {
                SortedList SQLlist = new SortedList(new MySort());
                Hashtable ht = new Hashtable();
                ht["cancelby"] = username;
                ht["ordernum"] = strordernum;
                ht["reason"] = reason;
                try
                {
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.Cancellation" } }, ht);
                    var library = new BillheadService();
                    string[] order = strordernum.TrimEnd(',').Split(',');
                    for (int j = 0; j < order.Count(); j++)
                    {
                        Orders orders = new ProRegisterService().SelectOrderInfo(order[j]);
                        if (orders.Ordersource == "0") //这里只修改个人体检账单
                        {
                            List<Billdetail> billdetail = new BilldetailService().GetBilldetailByOrderNum(order[j]);
                            if (billdetail.Count > 0)
                            {
                                for (int m = 0; m < billdetail.Count; m++)
                                {
                                    if (billdetail[m].Billheadid != null)
                                    {
                                        Hashtable htnew = new Hashtable();
                                        htnew.Add("status", (int)daan.service.common.ParamStatus.BillheadStatus.Invalid);
                                        htnew.Add("billheadid", billdetail[m].Billheadid);
                                        SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBillheadRefundment" } }, htnew);
                                    }
                                }

                            }
                           // outorderNum[0] = order[j];
                        }
                    }
                    if (library.ExecuteSqlTran(SQLlist))
                    {
                        b = true;
                    }
                }
                catch (Exception)
                {
                    b = false;
                }
            }
            return b;
        }

        /// <summary>
        /// 集中管理-取消作废订单
        /// </summary>
        public bool UpdateOrdersValid(string username, string strordernum)
        {
            bool b = true;
            string str = SelectOrderbarcodeByBill(strordernum);
            if (str == null && str == "")
            {
                Hashtable ht = new Hashtable();
                ht["cancelby"] = username;
                ht["ordernum"] = strordernum;
                try
                {
                    if (this.update("Order.Valid", ht) < 0)
                    {
                        b = false;
                    }
                }
                catch (Exception)
                {
                    b = false;
                }
            }
            else
            {
                SortedList SQLlist = new SortedList(new MySort());
                Hashtable ht = new Hashtable();
                ht["cancelby"] = username;
                ht["ordernum"] = strordernum;
                try
                {
                    SQLlist.Add(new Hashtable() { { "UPDATE", "Order.Valid" } }, ht);
                    var library = new BillheadService();
                    string[] order = strordernum.TrimEnd(',').Split(',');
                    for (int j = 0; j < order.Count(); j++)
                    {
                        Orders orders = new ProRegisterService().SelectOrderInfo(order[j]);
                        if (orders.Ordersource == "0") //这里只修改个人体检账单
                        {
                            List<Billdetail> billdetail = new BilldetailService().GetBilldetailByOrderNum(order[j]);
                            if (billdetail.Count > 0)
                            {
                                for (int m = 0; m < billdetail.Count; m++)
                                {
                                    if (billdetail[m].Billheadid != null)
                                    {
                                        Hashtable htnew = new Hashtable();
                                        htnew.Add("status", (int)daan.service.common.ParamStatus.BillheadStatus.PrepareOut);
                                        
                                        htnew.Add("billheadid", billdetail[m].Billheadid);
                                        SQLlist.Add(new Hashtable() { { "UPDATE", "Bill.UpdateBillheadRefundment" } }, htnew);
                                    }
                                }

                            }
                            // outorderNum[0] = order[j];
                        }
                    }
                    if (library.ExecuteSqlTran(SQLlist))
                    {
                        b = true;
                    }
                }
                catch (Exception)
                {
                    b = false;
                }
            }
            return b;
        }

        /// <summary>
        /// 查询打印指引单
        /// </summary>
        public DataTable GetPrintDirectData(string ordernum)
        {
            return this.selectDS("Order.SelectOrdergrouptestByPrintDirect", ordernum).Tables[0];
        }


        /// <summary>
        /// 查询订单是否已经采过血
        /// </summary>
        /// <param name="ordernums"></param>
        /// <returns></returns>
        public string SelectOrderbarcodeByCollected(string ordernums)
        {
            return this.selectObj<string>("Order.SelectOrderbarcodeByCollected", ordernums);
        }

        /// <summary>
        /// 查询订单是否已经出了账单
        /// </summary>
        /// <param name="ordernums"></param>
        /// <returns></returns>
        public string SelectOrderbarcodeByBill(string ordernums)
        {
            return this.selectObj<string>("Order.SelectOrdergrouptestBill", ordernums);
        }

        public DataTable GetSusManagementOrdersList(Hashtable ht)
        {
            return selectDS("Order.GetSusManagementOrders",ht).Tables[0];
        }
        public int GetSusmanagemenetOrdersCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.GetSusmanagementOrdersCount", ht).Tables[0].Rows[0][0]);
        }

        public DataTable GetOrderDelete(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectOrderDelete", ht).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="ht">订单编号</param>
        /// <returns></returns>
        public bool DeleteOrders(Hashtable ht)
        {
            bool b = true;
            try
            {
                SortedList SQLlist = new SortedList(new MySort());
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrdertest" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderGroupTest" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderproducts" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderbarcode" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderresultcomment" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrderVisit" } }, ht);
                SQLlist.Add(new Hashtable() { { "DELETE", "Order.DeleteOrders" } }, ht);
                BaseService bs = new BaseService();
                b = bs.ExecuteSqlTran(SQLlist);
            }
            catch (Exception)
            {
                b = false;
            }
            return b;
        }
    }
}
