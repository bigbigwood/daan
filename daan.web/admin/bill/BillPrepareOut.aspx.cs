using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.domain;
using daan.service;
using daan.service.bill;
using System.Data;
using System.Collections;
using daan.service.common;
using daan.service.login;
using daan.util.Common;
using daan.service.dict;
using daan.util.Web;

namespace daan.web.admin.bill
{
    public partial class BillPrepareOut : PageBase
    {
        LoginService loginService = new LoginService();
        BilldetailService billdetailService = new BilldetailService();
        CommonFuncLibService comm = new CommonFuncLibService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnPreInvoice.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnPreInvoice.ConfirmText = "确实要对当前的标本出一张账单？";
                btnPreInvoice.ConfirmTitle = "体检系统";
                btnDeleteSample.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnDeleteSample.ConfirmText = "确定删除选中的记录？";
                btnDeleteSample.ConfirmTitle = "体检系统";

                this.dtpStart.SelectedDate = DateTime.Now.AddDays(-7);
                this.dtpEnd.SelectedDate = DateTime.Now;

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
            DropDictcustomerBinder(dropCustomer, dropLab.SelectedValue.ToString(), false,customertype);
            //List<Dictcustomer> CustomerList = loginService.GetDictcustomer();

            //int labid = int.Parse(dropLab.SelectedValue);
            //string customertype = chkSendOut.Checked == true ? "1" : "0";
            //List<Dictcustomer> List = (from a in CustomerList where a.Dictlabid == labid && a.Customertype == customertype && a.Active == "1"  select a).ToList<Dictcustomer>();

            //dropCustomer.DataSource = List;
            //dropCustomer.DataValueField = "Dictcustomerid";
            //dropCustomer.DataTextField = "Customername";
            //dropCustomer.DataBind();
            //// dropCustomer.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion

        #region 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //验证查询条件合法性
            if (string.IsNullOrEmpty(dtpStart.Text) || string.IsNullOrEmpty(dtpEnd.Text))
            {
                MessageBoxShow("请选择登记日期");
                return;
            }
            DateTime resultDate;

            if (!DateTime.TryParse(dtpStart.Text, out resultDate))
            {
                MessageBoxShow("登记开始日期错误");
                return;
            }
            if (!DateTime.TryParse(dtpEnd.Text, out resultDate))
            {
                MessageBoxShow("登记结束日期错误");
                return;
            }
            if (this.dtpEnd.SelectedDate <= this.dtpStart.SelectedDate)
            {
                MessageBoxShow("结束时间应大于开始时间！");
                return;
            }    
            if (dropCustomer.SelectedValue == null)
            {
                MessageBoxShow("体检单位不能为空");
                return;
            }

            gvList.PageIndex = 0;
            GetSqlWhere();
            //绑定查询结果
            BindData();
            //绑定记录总数等信息
            BindDataCount();
        }
        #endregion

        #region  gridview数据绑定
        //绑定数据
        private void BindData()
        {
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];

                // 事务处理预出账的查询
                bool result = billdetailService.BillPrepareOutSearch(ht);

                IList<Billdetail> detailList = billdetailService.SelectBilldetailListForPage("PrepareOut", ht);
                gvList.DataSource = detailList;
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
                gvList.RecordCount = billdetailService.SelectBilldetailListCount("PrepareOut", ht);
                IList<Billdetail> detailList = billdetailService.GetBilldetailStatisticsByOrdernum("PrepareOut", ht);

                string msg = "";
                if (detailList.Count > 0)
                {
                    gvList.SelectedRowIndexArray = new int[] { 0 };
                    msg = " 订单数目：" + detailList.Count;
                    msg += " 标准金额：" + detailList.Sum(c => c.Standardprice);
                    msg += " 分点金额：" + detailList.Sum(c => c.Groupprice);
                    msg += " 应收金额：" + detailList.Sum(c => c.Contractprice);
                    msg += " 实收金额：" + detailList.Sum(c => c.Finalprice);
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

        #region 设置查询条件
        private void GetSqlWhere()
        {
            //flag:表示查询条件是否为外包账单
            ViewState["flag"] = chkSendOut.Checked == true ? "sendout" : "nosendout";

            //设置当前页和每页显示记录数
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);

            //查询条件
            Hashtable ht = new Hashtable();
            ht["flag"] = ViewState["flag"];
            ht["customerid"] = dropCustomer.SelectedValue.ToString() == "-1" ? null : dropCustomer.SelectedValue.ToString();
            ht["dictlabid"] = dropLab.SelectedValue.ToString() == "-1" ? null : dropLab.SelectedValue.ToString();
            ht["beginDate"] = dtpStart.SelectedDate.HasValue == true ? dtpStart.SelectedDate.Value.ToShortDateString() : null;
            ht["endDate"] = dtpEnd.SelectedDate.HasValue == true ? dtpEnd.SelectedDate.Value.AddDays(1).ToShortDateString() : null;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            ViewState["sqlWhere"] = ht;
        }
        #endregion

        #region 预出账

        //预出账
        protected void btnPreInvoice_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0)
                return;
            try
            {
                string flag = ViewState["flag"].ToString();

                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                double? _customerid = Convert.ToDouble(ht["customerid"]);
                double? _dictlabid = Convert.ToDouble(ht["dictlabid"]);
                //获得财务清单核对人编号
                double? checkbillid = null;
                if (_customerid != null)
                {
                    List<Dictcustomer> customerList = loginService.GetDictcustomer();
                    List<Dictcustomer> customer = (from a in customerList where a.Dictcustomerid == _customerid select a).ToList<Dictcustomer>();
                    checkbillid = customer.Count > 0 ? customer[0].Dictcheckbillid : null;
                }

                //生成账单头表billhead
                Billhead billhead = new Billhead();
                billhead = BillheadAdd(checkbillid, _customerid, _dictlabid);
                if (billhead == null)
                    return;

                //事物处理"预出账"操作
                bool result = billdetailService.BillPrepareOutOperation(billhead, ht, flag);
                BindData();
                BindDataCount();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        //生成账单头表billhead
        private Billhead BillheadAdd(double? customerid, double? dropcustomervalue, double? dropLabvalue)
        {
            try
            {
                Hashtable ht = (Hashtable)ViewState["sqlWhere"];
                IList<Billdetail> detailList = billdetailService.GetBilldetailStatisticsByOrdernum("PrepareOut", ht);
                Billhead billhead = new Billhead();

                if (dropLab.SelectedValue == "-1")
                    billhead.Dictlabid = null;
                else
                    billhead.Dictlabid = dropLabvalue;
                if (dropcustomervalue == null)
                    billhead.Dictcustomerid = null;
                else
                    billhead.Dictcustomerid = dropcustomervalue;
                int sta = (int)ParamStatus.BillheadStatus.PrepareOut;
                billhead.Status = sta.ToString();
                billhead.Remark = "";
                billhead.Createdate = System.DateTime.Now;
                billhead.Billby = Userinfo.userId;
                billhead.Duedate = null;
                billhead.Receipno = "";
                billhead.Dictcheckbillid = customerid;
                billhead.Customertype = chkSendOut.Checked == true ? "1" : "0";
                billhead.Billtype = "团检账单";
                billhead.Totalgrouprprice = detailList.Sum(c => c.Groupprice);
                billhead.Totalcontractprice = detailList.Sum(c => c.Contractprice);
                billhead.Totalfinalprice = detailList.Sum(c => c.Finalprice);
                billhead.Totalstandardprice = detailList.Sum(c => c.Standardprice);
                billhead.Billheadid = billdetailService.getSeqID("SEQ_BILLHEAD");
                billhead.Invoiceno = billhead.Billheadid.ToString();
                billhead.Begindate = DateTime.Parse(dtpStart.Text);
                billhead.Enddate = DateTime.Parse(dtpEnd.Text);

                return billhead;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                return null;
            }
        }
        #endregion

        #region 删除标本
        //删除标本
        protected void btnDeleteSample_Click1(object sender, EventArgs e)
        {
            try
            {
                string flag = ViewState["flag"].ToString();

                string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[2].ToString();
                string billheadid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[11].ToString();
                string ids = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[12].ToString();
                string testitemids = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[13].ToString();

                bool result = billdetailService.BillDeleteSample(ids, testitemids, ordernum, billheadid, flag);
                if (result)
                {
                    //刷新
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

        #region 备注
        //备注
        protected void btnRemark_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[2].ToString();
            string billheadid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[11].ToString();
            string url = "BillRemark.aspx?orderNum=" + ordernum + "&billheadid=" + billheadid;
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "备注录入"));
        }
        #endregion

        #region 调整价钱
        //调整价钱
        protected void btnAdjustPrice_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[2].ToString();
            string billheadid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[11].ToString();
            string ids = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[13].ToString();
            string url = "BillAdjustPrice.aspx?ordernum=" + ordernum + "&billheadid=" + billheadid + "&billdetailids=" + ids + "&flag=" + ViewState["flag"];
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "调整价钱"));
        }
        #endregion

        #region 日志操作
        //日志操作
        protected void btnOperationLog_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0 || gvList.SelectedRowIndexArray.Length <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[2].ToString();

            string url = "BillOperationLog.aspx?ordernum=" + ordernum;
            PageContext.RegisterStartupScript(WinBillRemark.GetShowReference(url, "操作记录"));
        }
        #endregion

        #region 关闭子窗口刷新父窗口
        protected void WinBillRemark_Close(object sender, EventArgs e)
        {
            BindData();
            //只有调整价钱时绑定
            //if (this.WinBillRemark.Title == "调整价钱")
            BindDataCount();
        }
        #endregion
    }

}