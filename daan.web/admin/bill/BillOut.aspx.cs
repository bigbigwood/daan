using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using daan.domain;
using daan.service;
using daan.service.bill;
using ExtAspNet;
using daan.service.login;
using daan.util.Common;
using daan.web.code;
using daan.util.Web;
using daan.service.dict;
using FastReport;
using daan.service.common;


namespace daan.web.admin.bill
{
    public partial class BillOut : PageBase
    {
        CommonFuncLibService comm = new CommonFuncLibService();
        BilldetailService billdetailService = new BilldetailService();
        OrdergrouptestService grouptestService = new OrdergrouptestService();
        BillheadService billheadService = new BillheadService();
        LoginService loginservice = new LoginService();
        CommonReport commonReport = new CommonReport();

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                btnReceive.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnReceive.ConfirmText = "确定接收选中的账单？";
                btnReceive.ConfirmTitle = "体检系统";
                btnInvalid.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnInvalid.ConfirmText = "确定作废选中的账单？";
                btnInvalid.ConfirmTitle = "体检系统";
                btnDeleteSample.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnDeleteSample.ConfirmText = "确定删除选中的记录？";
                btnDeleteSample.ConfirmTitle = "体检系统";

                BindDictLab();
                //首次加载调用绑定单位方法
                BindCustomer();
            }
        }

        #region 绑定分点、体检单位下拉框
        //绑定分点
        private void BindDictLab()
        {
            DDLDictLabBinder(dropLab, true);
        }

        //选择分点绑定体检单位
        protected void dropLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer();
        }

        //"是否外包"绑定体检单位
        protected void chkSendOut_CheckedChanged(object sender,EventArgs e)
        {
            BindCustomer();
        }

        //绑定体检单位下拉框
        private void BindCustomer()
        {
            string customertype = chkSendOut.Checked == true ? "1" : "0";
            DropDictcustomerBinder(dropCustomer, dropLab.SelectedValue.ToString(), false, customertype);
            //List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();

            //int labid = int.Parse(dropLab.SelectedValue);
            //string customertype = chkSendOut.Checked == true ? "1" : "0";
            //List<Dictcustomer> List = (from a in CustomerList where a.Dictlabid == labid && a.Customertype == customertype && a.Active == "1" select a).ToList<Dictcustomer>();

            //dropCustomer.DataSource = List;
            //dropCustomer.DataValueField = "Dictcustomerid";
            //dropCustomer.DataTextField = "Customername";
            //dropCustomer.DataBind();
            //dropCustomer.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion

        #region 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["dictlabid"] = dropLab.SelectedValue.ToString();
            GetSqlWhere();
            BindData();
            BindDataCount();
        }
        #endregion 

        #region 设置查询条件
        private void GetSqlWhere()
        {
            //flag:表示查询条件是否为外包账单
            ViewState["flag"] = chkSendOut.Checked == true ? "sendout" : "nosendout";

            //设置当前页和每页显示记录数
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);

            //查询条件
            Hashtable ht = new Hashtable();
            ht["status"] = "0";
            ht["dictLabid"] = dropLab.SelectedValue.ToString() == "-1" ? null : dropLab.SelectedValue.ToString();
            ht["customerid"] = dropCustomer.SelectedValue.ToString() == "-1" ? null : dropCustomer.SelectedValue.ToString();
            ht["billheadid"] = tbxBillheadid.Text.Trim() == "" ? null : tbxBillheadid.Text.Trim();
            ht["customertype"] = chkSendOut.Checked == true ? "1" : "0";
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            ViewState["sqlWhere"] = ht;
            ViewState["dictlabname"] = dropLab.SelectedText;
        }

        private Hashtable GetDetailSqlWhere(string billheadid)
        {
            PageUtil pageUtil = new PageUtil(gvDetail.PageIndex, gvDetail.PageSize);
            Hashtable ht = new Hashtable();
            ht["billheadid"] = billheadid;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            return ht;
        }
        #endregion

        #region gridview绑定数据
        //绑定数据
        private void BindData()
        {
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                gvList.DataSource = billheadService.SelectBillheadList("SelectBillheadList", ht).ToList<Billhead>();
                gvList.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        //绑定记录总数
        private void BindDataCount()
        { 
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                gvList.RecordCount = billheadService.GetBillHeadListCount("SelectBillheadList", ht);

                if (gvList.RecordCount <= 0)
                {
                    BindBillDetail("0");
                    BindBillDetailCount("0");
                }
                else
                {
                    gvList.SelectedRowIndexArray = new int[] { 0 };
                    ViewState["currentrowindex"] = 0;
                    string billheadid = gvList.Rows[0].Values[0].ToString();
                    BindBillDetail(billheadid);
                    BindBillDetailCount(billheadid);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //左边grid数据行选择改变时绑定右边grid数据
        protected void gvList_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            ViewState["currentrowindex"] = e.RowIndex;
            string billheadid = gvList.Rows[e.RowIndex].Values[0].ToString();
            BindBillDetail(billheadid);
            BindBillDetailCount(billheadid);
        }

        //绑定账单详细信息
        private void BindBillDetail(string billheadid)
        {
            try
            {
               Hashtable ht = GetDetailSqlWhere(billheadid);
               gvDetail.DataSource = billdetailService.SelectBilldetailListForPage("Out",ht); 
               gvDetail.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //绑定账单详细信息总数
        private void BindBillDetailCount(string billheadid)
        {
            Hashtable ht = GetDetailSqlWhere(billheadid);
            gvDetail.RecordCount = billdetailService.SelectBilldetailListCount("Out",ht);
            IList<Billdetail> detailList = billdetailService.GetBilldetailStatisticsByOrdernum("Out",ht);
            string msg = "";
            if (detailList.Count > 0)
            {
                gvDetail.SelectedRowIndexArray = new int[] { 0 };

                msg = " 订单数目：" + detailList.Count;
                msg += "  标准金额：" + detailList.Sum(c => c.Standardprice);
                msg += "  分点金额：" + detailList.Sum(c => c.Groupprice);
                msg += "  应收金额：" + detailList.Sum(c => c.Contractprice);
                msg += "  实收金额：" + detailList.Sum(c => c.Finalprice);
            }
            tbxMsg.Text = msg;
        }

        //账单头信息分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht = (Hashtable)ViewState["sqlWhere"];
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            BindData();
        }

        //账单相信信息分页
        protected void gvDetail_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvDetail.PageIndex = e.NewPageIndex;
            int currentrowindex = (int)ViewState["currentrowindex"];
            string billheadid = gvList.Rows[currentrowindex].Values[0].ToString();
            BindBillDetail(billheadid);
            
        }
        #endregion

        #region 获得账单头gridview选中行的ordernum值
        private string GetGridViewIDs()
        {
            //获取勾选项编号
            int[] index = gvList.SelectedRowIndexArray;
            string ids = "";
            for (int i = 0; i < index.Length; i++)
            {
                ids += gvList.Rows[index[i]].Values[0].ToString() + ",";
            }
            return ids.TrimEnd(',');
        }
        #endregion

        #region 接收、作废
        //接收
        protected void btnReceive_Click(object sender, EventArgs e)
        {
            int status = (int)ParamStatus.BillheadStatus.Receive;
            ViewState["status"] = status;
            gvListDataOperation(status, "已接收"); 
        }

        //作废
        protected void btnInvalid_Click(object sender, EventArgs e)
        {
            int status = (int)ParamStatus.BillheadStatus.Invalid;
            ViewState["status"] = status;
            gvListDataOperation(status, "已作废");
        }

        //gvlist操作数据处理
        private void gvListDataOperation(int status, string operationtype)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                string strMsg = operationtype == "已作废" ? "作废成功！" : "接收成功！";

                //获取选中项
                string ids = GetGridViewIDs();

                bool result = billheadService.BillheadDataOperation(ids, status, operationtype, ViewState["flag"].ToString());
                if (result)
                {
                    MessageBoxShow(strMsg,MessageBoxIcon.Question);
                    BindData();
                    BindDataCount();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region  账单头信息导出
        //账单头信息导出
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                //获取选中项
                string ids = GetGridViewIDs();
                Hashtable ht = new Hashtable();
                ht["status"] = (int)ParamStatus.BillheadStatus.PrepareOut;
                ht["billheadids"] = ids;
                List<Billhead> list = billheadService.SelectBillheadExcel(ht).ToList();

                //设置导出文件列头信息
                SortedList sortedList = new SortedList(new MySort());
                sortedList.Add("Billheadid", "账单号");
                sortedList.Add("Customername", "客户");
                sortedList.Add("Createdate", "出账时间");
                sortedList.Add("Totalstandardprice", "标准收费");
                sortedList.Add("Totalfinalprice", "实际收费");
                sortedList.Add("Salename", "业务员");

                string filename = "已出账账单信息" + DateTime.Now.Date.ToString("yyyyMMdd");
                ExcelOperation<Billhead>.ExportListToExcel(list, sortedList, filename, "sheet1");
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region 调整价钱
        //调整价钱
        protected void btnAdjustPrice_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count <= 0 || gvDetail.SelectedRowIndexArray.Length <= 0)
                return;

            List<Billdetail> billdetailList = (List<Billdetail>)ViewState["billdetailList"];
            string ordernum = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[2].ToString();
            string billheadid = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[12].ToString();
            string ids = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[13].ToString();

            string url = "BillAdjustPrice.aspx?ordernum=" + ordernum + "&billheadid=" + billheadid + "&billdetailids=" + ids + "&flag=" + ViewState["flag"];
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "调整价钱"));
        }
        #endregion

        #region 打印明细
        //打印明细
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//获取打印机配置信息
            try
            {
                if (gvDetail.Rows.Count <= 0 || gvDetail.SelectedRowIndexArray.Length <= 0)
                    return;

                int currentrowindex = (int)ViewState["currentrowindex"];

                //hashtable保存报表头尾信息
                Hashtable htinfo = new Hashtable();
                htinfo["customername"] = gvList.Rows[currentrowindex].Values[1].ToString();
                htinfo["salename"] = gvList.Rows[currentrowindex].Values[5].ToString();
                htinfo["begindate"] = gvList.Rows[currentrowindex].Values[7].Trim() == "" ? "" : DateTime.Parse(gvList.Rows[currentrowindex].Values[7].Trim()).ToString("yyyy-MM-dd");
                htinfo["enddate"] = gvList.Rows[currentrowindex].Values[8].ToString() == "" ? "" : DateTime.Parse(gvList.Rows[currentrowindex].Values[8].ToString()).ToString("yyyy-MM-dd");
                htinfo["customertype"] = gvList.Rows[currentrowindex].Values[9].ToString() == "0" ? "项目" : "外包项目";
                htinfo["titleName"] = ViewState["dictlabname"].ToString();
               

                double? dictlabid = null;
                double? checkbillid = null;
                if (!string.IsNullOrEmpty(gvList.Rows[currentrowindex].Values[6].ToString()))
                    checkbillid = double.Parse(gvList.Rows[currentrowindex].Values[6].ToString());
                dictlabid = double.Parse(ViewState["dictlabid"].ToString());

                //设置打印报表数据
                string billheadid = gvList.Rows[currentrowindex].Values[0].ToString();
                DataSet ds = new DataSet();
                ds = comm.ConvertToDataSet(htinfo, billheadid, "billdetail", checkbillid, dictlabid, ViewState["flag"].ToString());

                //打印
                Report report = new Report();      
                report = commonReport.GetReportByDataset("20", ds,1);
                commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);

                ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson)); 
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 删除标本
        //删除标本
        protected void btnDeleteSample_Click(object sender, EventArgs e)
        {
            try
            {
                string flag = ViewState["flag"].ToString();
                string billheadid = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[12].ToString();
                string ordernum = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[2].ToString();
                string ids = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[13].ToString();
                string testitemids = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[14].ToString();

                bool result = billdetailService.BillDeleteSample(ids, testitemids, ordernum, billheadid, flag);
                if (result)
                {
                    //刷新
                    BindData();
                    int currentrowindex = (int)ViewState["currentrowindex"];
                    string headid = gvList.Rows[currentrowindex].Values[0].ToString();
                    BindBillDetail(headid);
                    BindBillDetailCount(headid);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region 导出账单详细信息
        //导出账单详细信息
        protected void btnExcels_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDetail.Rows.Count <= 0)
                    return;

                int currentrowindex = (int)ViewState["currentrowindex"];
                string billheadid = gvList.Rows[currentrowindex].Values[0].ToString();
                List<Billdetail> detailprintList = billdetailService.SelectBilldetailInfoList(billheadid).ToList();

                //设置导出文件列头内容
                SortedList sortedList = new SortedList(new MySort());
                sortedList.Add("Status", "状态");
                sortedList.Add("Enterdate", "登记日期");
                sortedList.Add("Ordernum", "体检流水号");
                sortedList.Add("Realname", "病人姓名");
                sortedList.Add("Testname", "体检项目");
                sortedList.Add("Testcount", "项目数量");
                sortedList.Add("Standardprice", "标准收费");
                sortedList.Add("Groupprice", "分点收费");
                sortedList.Add("Contractprice", "应收金额");
                sortedList.Add("Finalprice", "实收金额");
                sortedList.Add("Selfremark", "财务说明");
                sortedList.Add("Remark", "财务备注");
                
                string filename = "已出账明细信息" + DateTime.Now.Date.ToString("yyyyMMdd");
                ExcelOperation<Billdetail>.ExportListToExcel(detailprintList, sortedList, filename, "sheet1");
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
            if (gvDetail.Rows.Count <= 0 || gvDetail.SelectedRowIndexArray.Length <= 0)
                return;
            string ordernum = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[2].ToString();
            string billheadid = gvDetail.Rows[gvDetail.SelectedRowIndexArray[0]].Values[12].ToString();

            string url = "BillRemark.aspx?orderNum=" + ordernum + "&billheadid=" + billheadid;
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "备注录入"));
        }
        #endregion 

        #region 关闭子窗口刷新父窗口
        protected void WinBillRemark_Close(object sender, EventArgs e)
        {
            BindData();
            int currentrowindex = (int)ViewState["currentrowindex"];
            string headid = gvList.Rows[currentrowindex].Values[0].ToString();
            BindBillDetail(headid);
            BindBillDetailCount(headid);
        }
        #endregion 
    }
}