using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Data;
using daan.domain;
using System.Collections;
namespace daan.service.report
{
   public class CustomervaliddiagnosisService : BaseService
    {
       /// <summary>
        /// 团检客户有效诊断
       /// </summary>
       /// <param name="customervaliddiagnosis"></param>
       /// <returns></returns>
      public bool InsertCustomervaliddiagnosis(Customervaliddiagnosis customervaliddiagnosis)
       {
           try
           {
               customervaliddiagnosis.Customervaliddiagnosisid = this.getSeqID("SEQ_CUSTOMERVALIDDIAGNOSIS");
               insert("report.InsertCustomervaliddiagnosis", customervaliddiagnosis);
               return true;
           }
           catch (Exception ee)
           {
               return false;
               
           }
       }

      /// <summary>
      /// 删除体检单位指定年度的诊断
      /// </summary>
      /// <param name="customervaliddiagnosis"></param>
      /// <returns></returns>
      public bool DeleteCustomervaliddiagnosis(Hashtable ht)
      {
          return delete("report.DeleteCustomervaliddiagnosis", ht) > 0;
      }
    }
}
