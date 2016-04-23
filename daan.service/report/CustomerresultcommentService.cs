using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using System.Collections;
using daan.domain;

namespace daan.service.report
{
    public class CustomerresultcommentService : BaseService
    {
        /// <summary>
        /// 报告日期，团检
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public Customerresultcomment GetreportDate(Customerresultcomment cus)
        {
         //   return selectDS("report.GetreportDate", ht).Tables[0];
            return selectObj<Customerresultcomment>("report.GetreportDate", cus);
        }
        /// <summary>
        /// 添加客户结果评价，团检
        /// </summary>
        /// <returns></returns>
        public bool InsertCustomerresultcomment(Customerresultcomment customerresultcomment)
        {
            try
            {
              //  customerresultcomment.Customerresultcommentid = getSeqID("SEQ_CUSTOMERRESULTCOMMENT");
                insert("report.InsertCustomerresultcomment", customerresultcomment);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        ///查询客户ID的年度诊断评价列表
        /// </summary>
        /// <param name="customervaliddiagnosis"></param>
        /// <returns></returns>
        public IList<Customerresultcomment> SelectCustomerresultcomment(Customerresultcomment customervaliddiagnosis)
        {
            try
            {
                return QueryList<Customerresultcomment>("report.SelectCustomerresultcomment", customervaliddiagnosis);
            }
            catch (Exception)
            {
                return new List<Customerresultcomment>();
            }
        }

        /// <summary>
        ///查询是否存在评价记录
        /// </summary>
        /// <param name="customervaliddiagnosis"></param>
        /// <returns></returns>
        public bool CheckCustomerresultcomment(Customerresultcomment customervaliddiagnosis)
        {
            bool b = false;
            try
            {
                int i = Convert.ToInt32(selectObj<object>("report.SelectCustomerresultcommentExist", customervaliddiagnosis));
                if (i > 0) { return true; }
            }
            catch (Exception) { }
            return b;
        }


        /// <summary>
        ///修改评价记录
        /// </summary>
        /// <param name="customervaliddiagnosis"></param>
        /// <returns></returns>
        public bool UpdateCustomerresultcomment(Customerresultcomment customervaliddiagnosis)
        {
            bool b = true;
            try
            {
                update("report.updateCustomerresultcomment", customervaliddiagnosis);                
            }
            catch (Exception) { b = false; }
            return b;
        }

        /// <summary>
        ///查询此单位诊断评价次数
        /// </summary>
        /// <param name="customervaliddiagnosis"></param>
        /// <returns></returns>
        public int SelectCustomerresultcommentYearCount(Hashtable ht)
        {
            int i = 1;
            try
            {
                i = Convert.ToInt32(selectObj<object>("report.SelectCustomerresultcommentYearCount", ht));
            }
            catch (Exception) { }
            return i;
        }
    }
}
