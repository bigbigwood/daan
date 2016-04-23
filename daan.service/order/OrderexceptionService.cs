using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using System.Collections;
using daan.util.Common;
using daan.domain;
namespace daan.service.order
{
    public class OrderexceptionService : BaseService
    {
        /// <summary>查询异常详细内容
        /// 
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public Orderexception SelectOrderExceptionInfo(string strid)
        {
            return this.selectObj<Orderexception>("Order.SelectOrderExceptionInfo", strid);
        }

        /// <summary>分页记录总数
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int SelectOrderExceptionCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.SelectOrderExceptionCount", ht).Tables[0].Rows[0][0]);             
        }
        /// <summary>分页获取LIS异常信息列表
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SelectOrderExceptionLst(Hashtable ht)
        {
            return selectDS("Order.SelectOrderExceptionLst", ht).Tables[0];
        }

        /// <summary>新增LIS异常信息
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool AddOrderExceptional(DataTable dt,string labCode)
        {
            Hashtable htPara = new Hashtable();
            
            SortedList sqlLst = new SortedList(new MySort());
            foreach (DataRow dr in dt.Rows)
            {
                Orderexception exception = new Orderexception();
                exception.Orderexceptionid = this.getSeqID("SEQ_ORDEREXCEPTION");
                exception.Exceptiontype = dr["EXCEPTIONTYPE"].ToString();
                exception.Subbarcode = dr["SUBBARCODE"].ToString();
                exception.Applyby = dr["APPLYBY"].ToString();
                exception.Applydate = Convert.ToDateTime(dr["APPLYDATE"]);//申请时间不会为空
                exception.Remark = dr["remark"].ToString();
                exception.Approveby = dr["APPROVEBY"].ToString();
                if (dr["APPROVEDATE"] != null)
                { 
                    exception.Approvedate=Convert.ToDateTime(dr["APPROVEDATE"]);
                }    
                exception.Status = dr["status"].ToString();
                exception.Barcode = dr["BARCODE"].ToString();
                if (dr["lastupdatedate"] != null)
                {
                    exception.LastUpdateDate = Convert.ToDateTime(dr["lastupdatedate"]);
                }
                exception.Labcode = labCode;
                htPara.Clear();
                htPara.Add("INSERT", "Order.InsertOrderException");
                sqlLst.Add(htPara, exception);
            }
            return this.ExecuteSqlTran(sqlLst);
        }

        /// <summary>查询最后一次更新时间
        /// 
        /// </summary>
        /// <returns></returns>
        public string SelectOrderExceptionLastDate(string labCode)
        {
            return this.selectObj<string>("Order.SelectOrderExceptionLastDate", labCode);
        }
        /// <summary>编辑处理状态及意见
        /// 
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateOrderException(Hashtable ht)
        {
            return this.update("Order.UpdateOrderException", ht)>0;
        }
    }
}
