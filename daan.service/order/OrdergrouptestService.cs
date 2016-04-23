using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using daan.domain;
using System.Data;

namespace daan.service
{
    public class OrdergrouptestService : BaseService
    {
        /// <summary>
        /// 按分点、外包单位、体检登记时间为条件查询收费信息
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns></returns>
        public IList<Ordergrouptest> GetOrdergrouptestPrice(Hashtable ht)
        {
            try
            {
                return this.QueryList<Ordergrouptest>("Order.SelectOrdergrouptestPrice", ht);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        ///  根据体检流水号查询项目信息
        /// </summary>
        /// <param name="ordernum">体检流水号</param>
        /// <returns></returns>
        public IList<Ordergrouptest> GetOrdergrouptestList(Hashtable ordernum)
        {
            try
            {
                return this.QueryList<Ordergrouptest>("Order.SelectOrdergrouptestInfo", ordernum);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        ///  查询体检单位的体检项目，团检报告
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetMainTestItem(Hashtable ht)
        {
            return selectDS("Order.GetMainTestItem", ht).Tables[0];
        }
        /// <summary>
        ///  修改ordergrouptest表数据
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="operationType">operationType: price修改价格，status修改状态</param>
        /// <returns></returns>
        public int UpdateOrdergrouptest(Hashtable ht,string operationType)
        {
            try
            {
                if (operationType == "price")
                    return this.update("Order.UpdateOrdergrouptestPrice", ht);
                else
                    return this.update("Order.UpdateOrdergrouptestStatus", ht);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ordergrouptestid"></param>
        /// <returns></returns>
        public int DeleteOrdergrouptest(string ordergrouptestid)
        {
            try
            {
                return this.delete("Order.DeleteOrdergrouptest", ordergrouptestid);
            }
            catch(Exception ex)
            {
                return -1;
                throw new Exception(ex.Message);
            }
        }
    }
}
