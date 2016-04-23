using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Collections;
using System.Data;

namespace daan.service.order
{

    public class OrderfileheaderService : BaseService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="orc"></param>
        public void InsertOrderfileheader(Orderfileheader orc)
        {
            orc.Orderfileheaderid = getSeqID("SEQ_ORDERFILEHEADER");
            //orc.Orderfileheaderid = getSeqID("Seq_DictLibraryItem");
            insert("Order.InsertOrderfileheader", orc);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orc"></param>
        public bool UpdateOrderfileheader(Orderfileheader orc)
        {
            return update("Order.UpdateOrderfileheader", orc) > 0;
        }
        /// <summary>
        /// 订单分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetBulkImportManagePageLst(Hashtable ht)
        {
            return selectDS("Order.GetBulkImportManagePageLst", ht).Tables[0];
        }

        /// <summary>
        /// 订单总页数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetBulkImportManagePageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.GetBulkImportManagePageLstCount", ht).Tables[0].Rows[0][0]);
        }
        public DataTable GetFrmUploadFileName()
        {
            return selectDS("Order.FrmUploadFileName.select", null).Tables[0];
        }
    }
}
