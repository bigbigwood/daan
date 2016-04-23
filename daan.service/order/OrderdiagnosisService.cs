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
    /// 体检者诊断表
    /// </summary>
    public class OrderdiagnosisService : BaseService
    {
        /// <summary>
        ///疾病数据对比,团检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DiseaseDataCompare(Hashtable ht)
        {
            return selectDS("Order.DiseaseDataCompare", ht).Tables[0];
        }
        /// <summary>
        ///统计诊断建议是否存在
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable CountDiagnosis(Hashtable ht)
        {
            return selectDS("Order.CountDiagnosis", ht).Tables[0];
        }
        /// <summary>
        ///团检报告，异常统计，人数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetDiseasePeople(Hashtable ht)
        {

            return selectDS("order.GetDiseasePeople", ht).Tables[0];
        }
        /// <summary>
        /// 团检报告,体检异常统计表，只统计医生选择的异常
        /// </summary>
        /// <param name="orderdiagnosisid"></param>
        /// <returns></returns>
        public DataTable GetdtGroupHealthCompare(Hashtable ht)
        {
            return selectDS("Order.GetdtGroupHealthCompare", ht).Tables[0];
        }

        /// <summary>
        /// 根据表主键查询相对应的诊断信息
        /// </summary>
        /// <param name="orderdiagnosisid"></param>
        /// <returns></returns>
        public DataTable SelectSingleOrderdiagnosis(string orderdiagnosisid)
        {
            return selectDS("Order.SelectSingleOrderdiagnosis", orderdiagnosisid).Tables[0];
        }

        /// <summary>
        /// 查询体检结果的前10种异常，团检报告
        /// </summary>
        /// <param name="orderdiagnosisid"></param>
        /// <returns></returns>
        public DataTable GetTopOrderdiagnosis(Hashtable ht)
        {
            return selectDS("Order.GetTopOrderdiagnosis", ht).Tables[0];            
        }


        /// <summary>
        /// 全体员工体检结果汇总,团检
        /// </summary>
        /// <param name="orderdiagnosisid"></param>
        /// <returns></returns>
        public DataTable GetdtGroupAllResult(Hashtable ht)
        {
            return selectDS("Order.GetdtGroupAllResult", ht).Tables[0];
        }
        /// <summary>
        /// 根据表主键查询相对应的诊断信息
        /// </summary>
        /// <param name="orderdiagnosisid"></param>
        /// <returns></returns>
        public IList<Orderdiagnosis> SelectSingleOrderdiagnosisLst(Hashtable ht)
        {
            return this.QueryList<Orderdiagnosis>("Order.SelectOrderdiagnosisLst", ht);
        }
        /// <summary>
        /// 根据订单号查询相对应的诊断信息
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable SelectOrderdiagnosis(string ordernum)
        {
            return selectDS("Order.SelectOrderdiagnosisDs", ordernum).Tables[0];

        }
        /// <summary>
        /// 根据ORDERDIAGNOSISID主键进行多条数据删除
        /// </summary>
        /// <param name="orderdiagnosisIds"></param>
        /// <returns></returns>
        public bool DeleteOrderdiagnosis(string orderdiagnosisIds)
        {
            return int.Parse(delete("Order.DeleteOrderdiagnosis", orderdiagnosisIds).ToString()) > 0;
        }

        /// <summary>
        /// 总检时如果诊断信息不对，最后审核者可以修改诊断信息
        /// </summary>
        /// <param name="orderdiagnosisIds"></param>
        /// <returns></returns>
        public bool UpdateOrderdiagnosis(Orderdiagnosis orderdiagnosis)
        {
            return int.Parse(delete("Order.UpdateOrderdiagnosis", orderdiagnosis).ToString()) > 0;
        }

        /// <summary>
        /// 添加诊断信息，总检
        /// </summary>
        /// <returns></returns>
        public bool AddOrderdiagnosis(Hashtable ht)
        {
            try
            {
                insert("Order.AddOrderdiagnosis", ht);
                return true;
            }
            catch (Exception ee)
            {

                return false;
            }
        }

        /// <summary>
        /// 社区网站数据上传根据据订单号查询建议内容
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetOrderdiagnosisByordernum(Hashtable ht)
        {
            return selectDS("Order.GetOrderdiagnosisByordernum", ht).Tables[0];
        }
    }
}
