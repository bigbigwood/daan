using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using daan.domain;

namespace daan.service.order
{
    public class OrderserviceinfoService : BaseService
    {
        /// <summary>
        /// 根据体检号取得客户追踪表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderserviceinfos(string ordernum)
        {
            return selectDS("Order.GetOrderserviceinfos", ordernum).Tables[0];

        }

        /// <summary>
        /// 新增一条客户追踪记录
        /// </summary>
        /// <returns></returns>
        public bool AddOrderserviceinfo(Orderserviceinfo orderserviceinfo)
        {

            int nflag = 0;
            try
            {
                insert("Order.InsertOrderserviceinfo", orderserviceinfo);
                nflag = 1;
            }
            catch (Exception ex)
            {
                nflag = 0;
                throw new Exception(ex.Message);
            }
            return nflag == 1;

        }
    }
}
