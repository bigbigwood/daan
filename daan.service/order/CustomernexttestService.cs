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
    public class CustomernexttestService : BaseService
    {
        /// <summary>
        /// 根据单位号查询推荐项目
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable SelectCustomernexttest(Hashtable ht)
        {

            return selectDS("Order.SelectCustomernexttest",  ht).Tables[0];
        }
        /// <summary>
        /// 根据订单号和dicttestitemid查询推荐项目是否存在
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public string CountForCustomernexttest(Customernexttest customernexttest)
        {
            Hashtable ht = new Hashtable();
            ht.Add("dicttestitemid", customernexttest.Dicttestitemid);
            ht.Add("Dictcustomerid", customernexttest.Dictcustomerid);
            return selectDS("Order.CountForCustomernexttest", ht).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 新加一个推荐项目
        /// </summary>
        /// <param name="ordernexttestService"></param>
        /// <returns></returns>
        public bool InsertCustomernexttest(Customernexttest customernexttest)
        {
            try
            {
                insert("Order.InsertCustomernexttest", customernexttest);
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
        public bool DeleteCustomernexttest(string customernexttestid)
        {
            return int.Parse(delete("Order.DeleteCustomernexttest", customernexttestid).ToString()) > 0;

        }
    }
}
