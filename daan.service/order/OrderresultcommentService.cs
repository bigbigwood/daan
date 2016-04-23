using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;

namespace daan.service.order
{
    public class OrderresultcommentService : BaseService
    {
        /// <summary>
        /// 新加一个推荐项目
        /// </summary>
        /// <param name="ordernexttestService"></param>
        /// <returns></returns>
        public void InsertOrderresultcomment(Orderresultcomment orc)
        {
            orc.Orderresultcommentid = getSeqID("SEQ_ORDERRESULTCOMMENT");
            insert("Order.InsertOrderresultcomment", orc);
        }

        /// <summary>
        /// 根据体检流水号查询一条记录
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public Orderresultcomment SelectOrderresultcomment(string ordernum)
        {
            return selectObj<Orderresultcomment>("Order.SelectOrderresultcomment", ordernum);
        }
        /// <summary>
        /// 根据体检流水号查询总体评价，返回DataTable
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable SelectOrderresultcommentDs(string ordernum)
        {
            return selectDS("Order.SelectOrderresultcommentDs", ordernum).Tables[0];
        }
         /// <summary>
        /// 根据体检流水号查询一条记录
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public void UpdateOrderresultcomment(Orderresultcomment orc)
        {
            update("Order.UpdateOrderresultcomment", orc);
        }
        
    }
}
