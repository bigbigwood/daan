using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Collections;

namespace daan.service.order
{
    /// <summary>
    /// 推荐项目
    /// </summary>
    public class OrdernexttestService : BaseService
    {
        /// <summary>
        /// 根据订单号查询推荐项目
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable SelectOrdernexttest(string ordernum)
        {

            return selectDS("Order.SelectOrdernexttest", ordernum).Tables[0];
        }
        /// <summary>
        /// 根据订单号和dicttestitemid查询推荐项目是否存在
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public string CountForOrdernexttest(Ordernexttest ordernexttest)
        {
            Hashtable ht = new Hashtable();
            ht.Add("dicttestitemid", ordernexttest.Dicttestitemid);
            ht.Add("ordernum", ordernexttest.Ordernum);
            return selectDS("Order.CountForOrdernexttest", ht).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 新加一个推荐项目
        /// </summary>
        /// <param name="ordernexttestService"></param>
        /// <returns></returns>
        public bool InsertOrdernexttest(Ordernexttest ordernexttest)
        {
            try
            {
                insert("Order.InsertOrdernexttest", ordernexttest);
                return true;
            }
            catch (Exception gg)
            {

                return false;
            }

        }
        /// <summary>
        /// 删除一个推荐项目
        /// </summary>
        /// <param name="ordernexttestid"></param>
        /// <returns></returns>
        public bool DeleteOrdernexttest(string ordernexttestid)
        {
            bool b = true;
            try
            {
                delete("Order.DeleteOrdernexttest", ordernexttestid);
            }
            catch (Exception)
            {
                b = false;
            }
            return b;

        }
    }
}
