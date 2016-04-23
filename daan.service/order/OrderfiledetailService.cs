using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.domain;
using daan.service.common;
using System.Data;
using daan.util.Common;

namespace daan.service.order
{

    public class OrderfiledetailService : BaseService
    {
        /// <summary>
        /// 订单明细分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetgdBulkImportDetailItemPageLst(Hashtable ht)
        {
            return selectDS("Order.GetgdBulkImportDetailItemPageLst", ht).Tables[0];
        }

        public DataTable aa(Hashtable ht)
        {
            return selectDS("Order.GetgdBulkImportDetailItemPageLstTable", ht).Tables[0];
        }
        /// <summary>
        /// 订单明细总页数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetgdBulkImportDetailItemPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Order.GetgdBulkImportDetailItemPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        /// <summary>
        /// 添加订单明细
        /// </summary>
        /// <param name="orc"></param>
        public void InsertOrderfiledetail(Orderfiledetail orc)
        {
            orc.Orderfiledetailid = getSeqID("SEQ_ORDERFILEDETAIL");
            insert("Order.InsertOrderfiledetail", orc);
        }

        /// <summary>
        /// 早事物中  批量插入导入详情数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool InsertOrderfiledetail(IList<Orderfiledetail> list, Orderfileheader orc)
        {
            SortedList SQLlist = new SortedList(new MySort());
            bool result = false;
            if (list != null && list.Count > 0)
            {
                foreach (Orderfiledetail item in list)
                {
                    item.Orderfiledetailid = getSeqID("SEQ_ORDERFILEDETAIL");
                    SQLlist.Add(new Hashtable() { { "INSERT", "Order.InsertOrderfiledetail" } }, item);
                }
                OrderfileheaderService headerservice = new OrderfileheaderService();
                if (headerservice.UpdateOrderfileheader(orc))
                {
                    //SQLlist.Add(new Hashtable() { { "UPDATE", " Order.UpdateOrderfileheader" } }, orc);

                    string error = string.Empty;
                    result = ExecuteSqlTran(SQLlist, ref error);
                }
            }
            return result;
        }


    }
}
