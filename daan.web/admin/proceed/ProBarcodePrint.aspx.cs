using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.proceed;
using System.Collections;
using ExtAspNet;
using System.Data;
using daan.domain;
using daan.service.login;
using daan.web.code;
using daan.service.dict;
using FastReport;
using daan.service.order;
using daan.service.common;

namespace daan.web.admin.proceed
{
    public partial class ProBarcodePrint : PageBase
    {
        static CommonReport commonReport = new CommonReport();
        static OrderbarcodeService barcodeservice = new OrderbarcodeService();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region >>>> zhouy 事件

        #region >>>> zhouy 查询
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbxOrderNum.Text == string.Empty && tbxBarcode.Text == string.Empty && tbxName.Text == string.Empty)
            {
                MessageBoxShow("[体检号,条码号,姓名]不能同时为空"); return;
            }

            Hashtable ht = new Hashtable();
            ht["ordernum"] = tbxOrderNum.Text;
            ht["ordebarcode"] = tbxBarcode.Text;
            ht["name"] = tbxName.Text;
            BindData(ht);



        }

        #endregion

        #region >>>> zhouy 打印条码
        //打印条码
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//设置打印所需的客户端配置信息

            string ordernum = "";
            string orderbarcodes = GetSelectBarcode(ref ordernum);
            if (orderbarcodes == string.Empty) { return; }
            DataTable dtSource = barcodeservice.GetPrintBarcodeData(new Hashtable() { { "ordernum", null }, { "orderbarcode", orderbarcodes } });

            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                string testnames = dtSource.Rows[i]["TESTNAMES"].ToString();
                dtSource.Rows[i]["AGE"] = WebUI.GetAge(dtSource.Rows[i]["AGE"]);//处理年龄                
                dtSource.Rows[i]["COLLECTDATE"] = dtSource.Rows[i]["COLLECTDATE"].ToString();
                dtSource.Rows[i]["TESTNAMES"] = testnames.TrimEnd(',');
                dtSource.Rows[i]["COUNT"] = "共" + testnames.TrimEnd(',').Split(',').Length + "项";
            }
            //修改订单状态为[条码已打印]（已登记的才改）
            new OrdersService().EditStatusByOldStatus(new Hashtable() { { "ordernum", ordernum }, { "status", (int)ParamStatus.OrdersStatus.BarCodePrint }, { "oldstatus", (int)ParamStatus.OrdersStatus.Register }, });
            //后续调用柯木朗方法打印
            //..........................            

            commonReport.PrintBarCode(dtSource, Userinfo);
            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintBarCode(\'{0}\',\'{1}\');", CommonReport.printer, CommonReport.json));

            //记录日志
            JournalLog(ordernum, orderbarcodes, "条码补打");
        }
        #endregion

        #region >>>> zhouy 获取选中行记录 操作

        /// <summary>获取选中多行的订单号 逗号间隔
        /// 获取选中多行的订单号 逗号间隔
        /// </summary>
        /// <returns></returns>
        private string GetSelectBarcode(ref string ordernum)
        {
            int[] strSelect = GridBarcodes.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return "";
            }
            string str = string.Empty;
            ordernum = GridBarcodes.DataKeys[strSelect[0]][0].ToString();
            for (int i = 0; i < strSelect.Length; i++)
            {
                str +="'"+ GridBarcodes.DataKeys[strSelect[i]][1].ToString() + "',";
              //  if (isCheckCancel) { if (CheckIsCancel(GridBarcodes.DataKeys[strSelect[i]][1])) { MessageBoxShow("选中订单[已作废],请重新操作!"); return ""; } }

            }
            return str.TrimEnd(',');
        }
        #endregion

        #endregion

        #region >>>> zhouy 方法

        /// <summary>绑定Grid数据
        /// 绑定Grid数据
        /// </summary>
        /// <param name="ht">查询参数</param>
        /// <param name="isSearch">是否重新查询</param>
        private void BindData(Hashtable ht)
        {
            GridBarcodes.DataSource = barcodeservice.SelectOrderbarcodePrintList(ht);
            GridBarcodes.DataBind();
        }

        /// <summary>判断是否作废
        /// 判断是否作废
        /// </summary>
        /// <param name="str">作废状态字符串</param>
        /// <returns></returns>
        private bool CheckIsCancel(object str)
        {
            return str.ToString() == "已作废";
        }

        //记录日志
        private static void JournalLog(string ordernum, string barcodes, string str)
        {
            //日志
            string[] arrorder = ordernum.Split(',');
            for (int i = 0; i < arrorder.Length; i++)
            {
                barcodeservice.AddOperationLog(ordernum, barcodes, "条码补打", "批量" + str + "[" + barcodes + "]", "节点信息", "批量" + str);
            }
        }
        #endregion
    }
}