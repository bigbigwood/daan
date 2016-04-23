using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using System.Collections;

namespace daan.service.bill
{
    public class BillProductService : BaseService
    {
        /// <summary>
        /// 根据查询条件获取产品类体检统计结果
        /// </summary>
        /// <param name="type">统计类别</param>
        /// <param name="param">查询条件</param>
        /// <returns>返回记录集</returns>
        public DataTable SelectProductPageLst(Hashtable htPara)
        {
            //htPara["state"]  0已返回检验 1已回内勤 2未返回检验 -1所有发出耗材
            string strSql = string.Empty;
            if (htPara["state"].ToString().Equals("0"))
            {
                strSql = "Bill.SelectBillProductPageLst";
            }
            if (htPara["state"].ToString().Equals("1"))
            {
                strSql = "Bill.SelectBillBackProductPageLst";
            }
            if (htPara["state"].ToString().Equals("2"))
            {
                strSql = "Bill.SelectBillNoBackProductPageLst";
            }
            return this.selectDS(strSql, htPara).Tables[0];
        }
        /// <summary>
        /// 根据查询条件获取产品类体检统计总数
        /// </summary>
        /// <param name="type">统计类别</param>
        /// <param name="param">查询条件</param>
        /// <returns>返回总数</returns>
        public int SelectProductPageTotal(Hashtable htPara)
        {
            string strSql = string.Empty;
            //htPara["state"]  0已返回检验 1已回内勤 2未返回检验 -1所有发出耗材
            if (htPara["state"].ToString().Equals("0"))
            {
                strSql = "Bill.SelectBillProductTotal";                
            }
            if (htPara["state"].ToString().Equals("1"))
            {
                strSql = "Bill.SelectBillBackProductTotal";                
            }
            if (htPara["state"].ToString().Equals("2"))
            {
                strSql = "Bill.SelectBillNoBackProductTotal";
            }
            return int.Parse(this.selectDS(strSql, htPara).Tables[0].Rows[0]["total"].ToString());
        }
    }
}
