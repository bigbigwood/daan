using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using daan.service.bill;
using daan.domain;
using daan.service;
using ExtAspNet;
using daan.service.login;
using daan.util.Common;
using daan.web.code;
using daan.util.Web;
using daan.service.common;
using FastReport;
using daan.service.dict;

namespace daan.web.admin.bill
{
    public partial class BillReceive : PageBase
    {
        BilldetailService billdetailService = new BilldetailService();
        BillheadService billheadService = new BillheadService();
        CommonFuncLibService comm = new CommonFuncLibService();
        LoginService loginservice = new LoginService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {               
                btnUnReceive.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnUnReceive.ConfirmText = "确定反结账选中的账单？";
                btnUnReceive.ConfirmTitle = "体检系统";

                this.dtpStart.SelectedDate = DateTime.Now.AddDays(-7);
                this.dtpEnd.SelectedDate = DateTime.Now;
                ViewState["currentrowindex"] = 0;
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
        protected void chkSendOut_CheckedChanged(object sender, EventArgs e)
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

        #region  查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetSqlWhere();
            BindData();
            BindDataCount();
            ViewState["dictlabid"] = dropLab.SelectedValue.ToString();
            ViewState["dictlabname"] = dropLab.SelectedText;
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
            ht["status"] = "1";
            ht["dictLabid"] = dropLab.SelectedValue.ToString() == "-1" ? null : dropLab.SelectedValue.ToString();
            ht["customerid"] = dropCustomer.SelectedValue.ToString() == "-1" ? null : dropCustomer.SelectedValue.ToString();
            ht["customertype"] = chkSendOut.Checked == true ? "1" : "0";
            ht["billheadid"] = tbxBillheadid.Text.Trim() == "" ? null : tbxBillheadid.Text.Trim();
            ht["startDate"] = dtpStart.SelectedDate.HasValue == true ? dtpStart.SelectedDate.Value.ToShortDateString() : null;
            ht["endDate"] = dtpEnd.SelectedDate.HasValue == true ? dtpEnd.SelectedDate.Value.AddDays(1).ToShortDateString() : null;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            ViewState["sqlWhere"] = ht;
        }

        private Hashtable GetDetailSqlWhere(string billheadid)
        {
            PageUtil pageUtil = new PageUtil(gvDetail.PageIndex, gvDetail.PageSize);
            BilldetailService detailservice = new BilldetailService();
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
                gvList.DataSource = billheadService.SelectBillheadList("SelectBillheadList", ht).ToList();
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
                MessageBoxShow(ex.Message);
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

        //根据账单号查询账单明细 
        private void BindBillDetail(string billheadid)
        {
            try
            {
                Hashtable ht = GetDetailSqlWhere(billheadid);
                gvDetail.DataSource = billdetailService.SelectBilldetailListForPage("Receive",ht);
                gvDetail.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //根据账单号查询账单明细记录总数
        private void BindBillDetailCount(string billheadid)
        {
            try
            {
                Hashtable ht = GetDetailSqlWhere(billheadid);
                gvDetail.RecordCount = billdetailService.SelectBilldetailListCount("Receive", ht);
                IList<Billdetail> detailList = billdetailService.GetBilldetailStatisticsByOrdernum("Receive", ht);
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
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
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

        #region 反结账
        //反结账
        protected void btnUnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                //获取勾选项编号
                string ids = GetGridViewIDs();
                bool result = billheadService.BillheadDataOperation(ids, (int)ParamStatus.BillheadStatus.PrepareOut, "反结账",ViewState["flag"].ToString());

                if (result)
                {
                    MessageBoxShow("反结账成功！", MessageBoxIcon.Question);
                    BindData();
                    BindDataCount();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }
        #endregion

        #region 账单头信息导出
        //账单头信息导出
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                    return;

                //获取勾选项编号
                string ids = GetGridViewIDs();
                Hashtable ht = new Hashtable();
                ht["status"] = (int)ParamStatus.BillheadStatus.Receive;
                ht["billheadids"] = ids;
                List<Billhead> list = billheadService.SelectBillheadExcel(ht).ToList();

                //设置导出的excel文件列头内容
                SortedList sortedList = new SortedList(new MySort());
                sortedList.Add("Billheadid", "账单号");
                sortedList.Add("Customername", "医院名称");
                sortedList.Add("Totalstandardprice", "标准收费");
                sortedList.Add("Totalfinalprice", "实际收费");
                sortedList.Add("Createdate", "生成账单日期");
                sortedList.Add("Duedate", "预计结账日期");
                sortedList.Add("Totalcontractprice", "应收价钱");
                sortedList.Add("Totalgrouprprice", "区域价钱");
                sortedList.Add("Salename", "销售员");

                string filename = "已接收账单信息" + DateTime.Now.Date.ToString("yyyyMMdd");
                ExcelOperation<Billhead>.ExportListToExcel(list, sortedList, filename, "sheet1");
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }
        #endregion

        #region  导出明细信息
        protected void btnExcels_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDetail.Rows.Count <= 0)
                    return;

                int currentrowindex = (int)ViewState["currentrowindex"];
                string billheadid = gvList.Rows[currentrowindex].Values[0].ToString();
                List<Billdetail> detailprintList = billdetailService.SelectBilldetailInfoList(billheadid).ToList();

                //设置导出excel列头内容
                SortedList sortedList = new SortedList(new MySort());
                sortedList.Add("Status", "状态");
                sortedList.Add("Enterdate", "接收日期");
                sortedList.Add("Ordernum", "体检流水号");
                sortedList.Add("Realname", "病人姓名");
                sortedList.Add("Testcount", "项目数量");
                sortedList.Add("Testname", "项目清单");
                sortedList.Add("Standardprice", "标准收费");
                sortedList.Add("Groupprice", "分点收费");
                sortedList.Add("Contractprice", "应收金额");
                sortedList.Add("Finalprice", "实收金额");
                sortedList.Add("Selfremark", "财务说明");
                sortedList.Add("Remark", "财务备注");

                string filename = "已接收明细信息" + DateTime.Now.Date.ToString("yyyyMMdd");
                ExcelOperation<Billdetail>.ExportListToExcel(detailprintList, sortedList, filename, "sheet1");
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 打印明细
        //打印明细
        protected void btnPrintDetail_Click(object sender,EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//获取打印机配置信息

            if (gvDetail.Rows.Count <= 0 || gvDetail.SelectedRowIndexArray.Length <= 0)
                return;

            int currentrowindex = (int)ViewState["currentrowindex"];

            //hashtable保存报表头尾基本信息
            Hashtable htinfo = new Hashtable();
            htinfo["customername"] = gvList.Rows[currentrowindex].Values[1].ToString();
            htinfo["salename"] = gvList.Rows[currentrowindex].Values[8].ToString();
            htinfo["begindate"] = gvList.Rows[currentrowindex].Values[10].ToString() == "" ? "" : DateTime.Parse(gvList.Rows[currentrowindex].Values[10].ToString()).ToString("yyyy-MM-dd");
            htinfo["enddate"] = gvList.Rows[currentrowindex].Values[11].ToString() == "" ? "" : DateTime.Parse(gvList.Rows[currentrowindex].Values[11].ToString()).ToString("yyyy-MM-dd");
            htinfo["customertype"] = gvList.Rows[currentrowindex].Values[12].ToString() == "0" ? "项目" : "外包项目";
            htinfo["titleName"] = ViewState["dictlabname"].ToString();

            double? dictlabid = null;
            double? checkbillid = null;
            if (!string.IsNullOrEmpty(gvList.Rows[currentrowindex].Values[9].ToString()))
                checkbillid = double.Parse(gvList.Rows[currentrowindex].Values[9].ToString());
            dictlabid = double.Parse(ViewState["dictlabid"].ToString());

            //设置打印报表数据
            string billheadid = gvList.Rows[currentrowindex].Values[0].ToString();
            DataSet ds = new DataSet();
            ds = comm.ConvertToDataSet(htinfo, billheadid, "billdetail", checkbillid, dictlabid, ViewState["flag"].ToString());
           
            //打印
            CommonReport commonReport = new CommonReport();
            Report report = new Report();           
            report = commonReport.GetReportByDataset("20", ds,1);   
            commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);

            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson)); 
        }
        #endregion
        
        #region 按体检流水号、姓名查找账单明细
        // 搜索按钮点击事件
        protected void ttbxSearch_Trigger2Click(object sender, EventArgs e)
        {
            if (ttbxSearch.Text == "")
                return;


            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                string ordernum = gvDetail .Rows[i].Values[2].ToString();
                string realname = gvDetail.Rows[i].Values[3].ToString();

                if (ordernum == ttbxSearch.Text.Trim() || realname == ttbxSearch.Text.Trim())
                {
                    gvDetail.SelectedRowIndexArray = new int[] { gvDetail.Rows[i].RowIndex };
                    break;
                }
            }
        }
        #endregion

        #region 关闭子窗口刷新父窗口
        protected void WinBillRemark_Close(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion
    }
}