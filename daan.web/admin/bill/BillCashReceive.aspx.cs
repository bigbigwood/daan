using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using daan.service.dict;
using daan.domain;
using daan.service.bill;
using System.Collections;
using daan.util.Common;
using daan.service;
using daan.web.code;
using System.IO;
using ExtAspNet;
using daan.util.Web;
using daan.service.login;
using daan.service.common;
using FastReport;

namespace daan.web.admin.bill
{
    public partial class BillCashReceive : PageBase
    {
        BillheadService headService = new BillheadService();
        CommonFuncLibService comm = new CommonFuncLibService();
        DictuserService userService = new DictuserService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                btnReceive.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnReceive.ConfirmText = "确定接收选中的账单？";
                btnReceive.ConfirmTitle = "体检系统";

                this.dtpStart.SelectedDate = DateTime.Now.AddDays(-7);
                this.dtpEnd.SelectedDate = DateTime.Now;

                BindDictLab();
            }
        }

        //绑定分点
        private void BindDictLab()
        {
            DDLDictLabBinder(dropLab, true);
        }
        #region 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvList.PageIndex = 0;
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht = new Hashtable();
            ht["status"] = dropStatus.SelectedValue.ToString() == "-1" ? null : dropStatus.SelectedValue.ToString();
            ht["startDate"] = dtpStart.SelectedDate.HasValue == true ? dtpStart.SelectedDate.Value.ToShortDateString() : null;
            ht["endDate"] = dtpEnd.SelectedDate.HasValue == true ? dtpEnd.SelectedDate.Value.AddDays(1).ToShortDateString() : null;
            ht["dictlabid"] = dropLab.SelectedValue.ToString() == "-1" ? null : dropLab.SelectedValue.ToString();
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            ViewState["sqlWhere"] = ht;
            BindData();
            BindDataCount();
            ViewState["dictlabname"] = dropLab.SelectedText;
        }
        #endregion

        #region gridview绑定数据
        //数据绑定
        private void BindData()
        {
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                gvList.DataSource = headService.SelectBillheadList("SelectBillheadForCashReceive", ht);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //绑定记录总数
        private void BindDataCount()
        {
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                gvList.RecordCount = headService.GetBillHeadListCount("SelectBillheadForCashReceive", ht);
                string msg = "";
                IList<Billhead> headList = headService.GetCashReceiveStatisticsByOrdernum(ht);
                if (headList.Count > 0)
                {
                    gvList.SelectedRowIndexArray = new int[] { 0 };
                    msg = " 订单数目：" + headList.Count;
                    msg += " 标准金额：" + headList.Sum(c => c.Totalstandardprice);
                    msg += " 分点金额：" + headList.Sum(c => c.Totalgrouprprice);
                    msg += " 应收金额：" + headList.Sum(c => c.Totalcontractprice);
                    msg += " 实收金额：" + headList.Sum(c => c.Totalfinalprice);
                }
                tbxMsg.Text = msg;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;

            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht = (Hashtable)ViewState["sqlWhere"];
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            BindData();
        }
        #endregion
        //protected void gvList_RowDataBound(object sender, GridRowEventArgs e)
        //{
        //    System.Web.UI.WebControls.BoundField ddloutcustomer = (System.Web.UI.WebControls.BoundField)gvList.Rows[e.RowIndex].FindControl("");
        //}


        #region 获得gridview选中行的ordernum值
        private string GetGridViewIds()
        {
            int[] index = gvList.SelectedRowIndexArray;
            string ordernums = "";
            for (int i = 0; i < index.Length; i++)
            {
                ordernums += gvList.Rows[index[i]].Values[0].ToString() + ",";
            }
            return ordernums.TrimEnd(',');
        }
        #endregion

        #region 打印清单
        //打印清单
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//取mac地址   

            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                //获取勾选的记录
                string ordernums = GetGridViewIds();

                DataSet ds = new DataSet();

                //hashtable保存财务报表头尾信息
                Hashtable htinfo = new Hashtable();
                htinfo["begindate"] = dtpStart.Text;
                htinfo["enddate"] = dtpEnd.Text;
                htinfo["customertype"] = "项目";
                htinfo["titleName"] = ViewState["dictlabname"].ToString();


                //设置打印报表数据
                ds = comm.ConvertToDataSet(htinfo, ordernums, "billhead", null, null, "");

                CommonReport commonReport = new CommonReport();
                Report report = new Report();     
                report = commonReport.GetReportByDataset("20", ds, 1);      
                commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);

                ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 接收
        //接收
        protected void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                System.Web.UI.WebControls.Label lab = (System.Web.UI.WebControls.Label)gvList.Rows[gvList.SelectedRowIndexArray[0]].FindControl("Label1");
                if (lab.Text == "预出账")
                {
                    int[] index = gvList.SelectedRowIndexArray;
                    string ids = "";
                    for (int i = 0; i < index.Length; i++)
                    {
                        ids += gvList.Rows[index[i]].Values[0].ToString() + ",";
                    }

                    //事物处理：修改选中的账单头表状态
                    bool result = headService.BillheadDataOperation(ids.TrimEnd(','), (int)ParamStatus.BillheadStatus.Receive, "已接收", null);
                    MessageBoxShow("接收成功！", MessageBoxIcon.Question);
                    BindData();
                    BindDataCount();
                }
                else
                {
                    if (lab.Text == "已接收")
                    {
                        MessageBoxShow("已接收过账单不能重复接收，请筛选出需要接收账单！");
                    }
                    else if (lab.Text == "已作废")
                    {
                        MessageBoxShow("已作废过账单不能接收，请筛选出需要接收账单！");
                    }
                    else if (lab.Text == "已退款")
                    {
                        MessageBoxShow("已退款账单不能接收，请筛选出需要接收账单！");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 导出
        //导出
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                string ordernums = GetGridViewIds();

                //获得选中的账单头表记录
                List<Billhead> list = headService.SelectBillHeadListByids(ordernums).ToList<Billhead>();

                //设置导出文件列头信息
                SortedList sortedList = new SortedList(new MySort());
                sortedList.Add("Billheadid", "账单号");
                sortedList.Add("Realname", "姓名");
                sortedList.Add("Duedate", "收费日期");
                sortedList.Add("Ordernum", "体检流水号");
                sortedList.Add("Testname", "体检项目");
                sortedList.Add("Testcount", "项目数量");
                sortedList.Add("Totalstandardprice", "标准收费");
                sortedList.Add("Totalgrouprprice", "分点收费");
                sortedList.Add("Totalcontractprice", "应收金额");
                sortedList.Add("Totalfinalprice", "实收金额");
                sortedList.Add("Status", "账单状态");
                sortedList.Add("Selfremark", "财务说明");
                sortedList.Add("Remark", "财务备注");
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Status == "1")
                        {
                            list[i].Status = "已接收";
                        }
                        else if (list[i].Status == "0")
                        {
                            list[i].Status = "预出账";
                        }
                        else if (list[i].Status == "5")
                        {
                            list[i].Status = "已退款";
                        }
                        else if (list[i].Status == "9")
                        {
                            list[i].Status = "已作废";
                        }
                    }
                }
                string filename = "现金接收账单信息" + DateTime.Now.Date.ToString("yyyyMMdd");
                ExcelOperation<Billhead>.ExportListToExcel(list, sortedList, filename, "sheet1");
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region >>>> zhouy 退款
        protected void btnRefundment_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                 System.Web.UI.WebControls.Label lab = (System.Web.UI.WebControls.Label)gvList.Rows[gvList.SelectedRowIndexArray[0]].FindControl("Label1");
                 if (lab.Text == "已退款")
                 {
                     MessageBoxShow("已退款账单不能重复退款，请筛选出需要退款账单！");
                     return;
                 }
                int[] index = gvList.SelectedRowIndexArray;
                string ids = "";
                for (int i = 0; i < index.Length; i++)
                {
                    ids += gvList.Rows[index[i]].Values[0].ToString() + ",";
                }
                Hashtable ht = new Hashtable();
                ht.Add("status", (int)daan.service.common.ParamStatus.BillheadStatus.Refundment);
                ht.Add("billheadid", ids.TrimEnd(','));
                BillheadService billheadservice = new BillheadService();
                billheadservice.BillUpdateBillheadRefundment(ht);
                string[] strs = ids.TrimEnd(',').Split(',');
                for (int i = 0; i < strs.Length; i++)
                {
                    billheadservice.AddOperationLog(strs[i], "", "现金接收", "作废订单", "节点信息", strs.Length > 1 ? "批量操作" : "");
                }
                MessageBoxShow("退款成功！");
                BindData();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 备注
        //备注
        protected void btnRemark_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[3].ToString();
            string billheadid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[0].ToString();

            string url = "BillRemark.aspx?orderNum=" + ordernum + "&billheadid=" + billheadid;
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "备注录入"));
        }
        #endregion

        #region 日志
        //日志
        protected void btnOperationLog_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[3].ToString();

            string url = "BillOperationLog.aspx?ordernum=" + ordernum;
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "操作记录"));
        }
        #endregion

        #region 调整价钱
        //调整价钱
        protected void btnAdjustPrice_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;
            System.Web.UI.WebControls.Label lab = (System.Web.UI.WebControls.Label)gvList.Rows[gvList.SelectedRowIndexArray[0]].FindControl("Label1");
            if (lab.Text == "预出账")
            {
                List<Billdetail> billdetailList = (List<Billdetail>)ViewState["billdetailList"];
                string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[3].ToString();
                string billheadid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[0].ToString();
                //string ids = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[12].ToString();

                string url = "BillAccurately.aspx?ordernum=" + ordernum + "&billheadid=" + billheadid;
                PageContext.RegisterStartupScript(WindowFrame.GetShowReference(url, "调整价钱"));
            }
            else
            {
                if (lab.Text == "已退款")
                {
                    MessageBoxShow("已退款账单，不能再修改价格！");
                }
                else if (lab.Text == "已接收")
                {
                    MessageBoxShow("已接收账单，不能再修改价格！");
                }
                else if (lab.Text == "已作废")
                {
                    MessageBoxShow("已作废账单，不能再修改价格！");
                }
            }
        }
        #endregion

        #region 个人收费打印
        protected void btnPrintIndividual_Click(object sender, EventArgs e)
        {

            SetInitlocalsetting(hdMac.Text);//取mac地址   

            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                //获取勾选的记录
                string ordernums = GetGridViewIds();

                IList<Billhead> headlist = headService.SelectBillHeadListForPrintByids(ordernums);

                //定义dataset 存放打印信息
                foreach (Billhead head in headlist)
                {
                    Hashtable htinfo = new Hashtable();
                    htinfo["Ordernum"] = head.Ordernum;
                    htinfo["Realname"] = head.Realname;
                    htinfo["Productname"] = head.Productname;
                    htinfo["Finalprice"] = head.Totalfinalprice + "(元)";
                    htinfo["Billbyname"] = head.Username;
                    htinfo["Billbydate"] = System.DateTime.Now.Year + "年" + System.DateTime.Now.Month + "月" + System.DateTime.Now.Day + "日";
                    htinfo["titleName"] = ViewState["dictlabname"].ToString();

                    DataSet ds = comm.ConvertToDataSet(htinfo);

                    //打印
                    CommonReport commonReport = new CommonReport();
                    Report report = new Report();
                    List<string> lists = new List<string>();
                    List<DataSet> dslist = new List<DataSet>();
                    report = commonReport.GetReportByDataset("30", ds, 1);          
                    commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);
                    ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 关闭子窗口刷新父窗口
        protected void WinBillRemark_Close(object sender, EventArgs e)
        {
            BindData();
        }

        protected void WindowFrame_Close(object sender, EventArgs e)
        {
            BindData();
        }

        #endregion
    }
}