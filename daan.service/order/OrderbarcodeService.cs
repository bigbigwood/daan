using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using System.Collections;
using System.Data;
using daan.domain;
using System.Web;
/**
 * 代码开发者： caix
 * 2012-4-11
 * */

namespace daan.service.proceed
{
    /// <summary>
    /// 订单条码表业务类
    /// </summary>
    public class OrderbarcodeService : BaseService
    {
        #region 字段

        #endregion



        #region 方法
        /// <summary>
        /// 根据相关条件查询标本接收状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SpecimenAccept(Hashtable ht)
        {

            return selectDS("Order.SpecimenAccept", ht).Tables[0];
        }

        /// <summary>
        /// 查询采血确认状态的数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable DataForCollectBlood(Hashtable ht)
        {
            return selectDS("Order.DataForCollectBlood", ht).Tables[0];

        }


        /// <summary>
        /// 将相对应的条码确认为已采血状态
        /// </summary>
        /// <param name="ordersBarcodeId"></param>
        /// <returns></returns>
        public int EnSureCollectBlood(Hashtable ht)
        {
            return update("Order.EnSureCollectBlood", ht);

        }
        /// <summary>
        /// 将相对应的条码确认为已接收状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int EnsureAccept(Hashtable ht)
        {
            return update("Order.EnsureAccept", ht);

        }

        /// <summary>
        /// 根据ordernum 查询条码拼接成的字符串用于体检系统数据接收 yhl
        /// </summary>
        /// <param name="strOrdernums">ordernum拼接字符串</param>
        /// <returns></returns>
        public string SelectOrderbarcodeString(string strOrdernums)
        {
            return this.selectObj<string>("Order.SelectOrderbarcodeString", strOrdernums).ToString();
        }
        #endregion

        /// <summary>
        /// 条码补打列表查询 zhouy
        /// </summary>
        /// <param name="ht">条码和订单号</param>
        /// <returns></returns>
        public DataTable SelectOrderbarcodePrintList(Hashtable ht)
        {
            DataTable dt = selectDS("Order.SelectOrderbarcodePrintList", ht).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["AGE"] = WebUI.GetAge(dt.Rows[i]["AGE"]);
            }
            return dt;
        }

        /// <summary>
        /// 查询是否存在Barcode条码的记录 Zhangwei
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Orderbarcode> SelectOrderbarcode(Hashtable ht)
        {
            return this.QueryList<Orderbarcode>("Order.SelectOrderbarcode", ht);
        }

        /// <summary>
        /// 查询打印条码数据
        /// </summary>
        public DataTable GetPrintBarcodeData(Hashtable ht)
        {
            return this.selectDS("Order.SelectOrderbarcodeByPrint", ht).Tables[0];
        }

        #region 上传LIS成功后，更新状态 ylp
        /// <summary>
        /// 上传LIS成功后，更新状态
        /// </summary>
        /// <param name="strbarcode"></param>
        /// <returns></returns>
        public int UpdateTransedToLis(string strbarcode)
        {
            return update("Order.UpdateTransedToLis", strbarcode);
        }

        /// <summary>
        /// 上传LIS失败后，更新状态
        /// </summary>
        /// <param name="strbarcode"></param>
        /// <returns></returns>
        public int UpdateTransedToLisFail(string strbarcode)
        {
            return update("Order.UpdateTransedToLisFail", strbarcode);
        }
        #endregion

        /// <summary>
        /// 根据相关条件查询订单上传LIS标志失败状态的记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable SelectUploadToLIS(Hashtable ht)
        {

            return selectDS("Order.UploadToLIS", ht).Tables[0];
        }
        /// <summary>
        /// 根据选择的订单号修改上传社区标志状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool UpdateSelectedTransedToLIS(Hashtable ht)
        {
            return update("Order.UpdateSelectedTransedToLIS", ht) > 0;
        }

        /// <summary>
        /// 检查条码号是否已在系统中存在
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool CheckBarCode(string barcode)
        {
            bool b = true;
            Decimal count = selectObj<Decimal>("Order.CheckBarcode", barcode);
            if (count == 0) { b = false; }
            return b;
        }

        public DataTable CheckBarCode2(string barcode)
        {
            return selectDS("Barcode.CheckBarcode", barcode).Tables[0];
        }
    }
}
