using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using daan.service.bill;
using daan.domain;
using System.Collections;
using daan.util.Common;
using daan.service;
using daan.service.order;
using daan.service.common;
using ExtAspNet;
using daan.service.login;
using daan.web.code;
using daan.service.dict;
using FastReport;

namespace daan.web.admin.bill
{
    public partial class BillIndividualCharge : PageBase
    {
        CommonFuncLibService comm = new CommonFuncLibService();
        OrdergrouptestService grouptestService = new OrdergrouptestService();
        LoginService loginService = new LoginService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnDelete.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
                btnDelete.ConfirmText = "确定删除选中的记录？";
                btnDelete.ConfirmTitle = "体检系统";
                btnSave.ConfirmText = "确实要对当前的标本出一张账单？";
                btnSave.ConfirmTitle = "体检系统";

                if (string.IsNullOrEmpty(Request["ordernum"]))
                    return;

                BindData(Request["ordernum"].ToString());
            }
        }


        //页面加载时显示数据
        private void BindData(string ordernum)
        {
            OrdersService service = new OrdersService();

            List<Ordergrouptest> grouptestList = new List<Ordergrouptest>();
            Orders orders = new Orders();

            orders = service.SelectOrdersByOrdernum(ordernum);
            if (orders == null)
                return;
            tbxName.Text = orders.Realname;
            tbxAge.Text = orders.Age;
            tbxCustomer.Text = orders.Customername;
            if (orders.Enterdate.HasValue)
                tbxEnterdate.Text = orders.Enterdate.Value.ToShortDateString();
            tbxOrdernum.Text = ordernum;
            tbxSex.Text = orders.Sex;
            ViewState["dictcustomerid"] = orders.Dictcustomerid;
            ViewState["dictlabid"] = orders.Dictlabid;

            BindGridview(ordernum);
        }

        public List<Ordergrouptest> GetData(string ordernum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            string ids = null;
            if (gvList.Rows.Count != 0)
            {
                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    ids += gvList.Rows[i].Values[0].ToString() + ",";
                }
                ids = ids.TrimEnd(',');
            }
            ht.Add("ids", ids);
            List<Ordergrouptest> list = new List<Ordergrouptest>();
            list= grouptestService.GetOrdergrouptestList(ht).ToList();
            return list;
        }


        //绑定检测项目列表
        private void BindGridview(string ordernum)
        {
            List<Ordergrouptest> list = GetData(ordernum);
            this.gvList.DataSource = list;
            this.gvList.DataBind();
            gvList.RecordCount = list.Count;

            if (list != null && list.Count > 0)
            {
                tbxModifytotalprice.Text = tbxTotalprice.Text = list.Sum(c => c.Finalprice).ToString();
                btnSave.Enabled = true;
                GetTestNamesAndTotalPrice(list);
            }
            else
                btnSave.Enabled = false;
        }

        //获取检测费用和检测项目用，隔开
        private void GetTestNamesAndTotalPrice(List<Ordergrouptest> list)
        {
            List<Ordergrouptest> testitemList = (from item in list
                                                 group item by new
                                                 {
                                                     item.Ordernum
                                                 }
                                                     into g
                                                     let pname = g.Select(x => x.Productname.ToString()).Distinct().ToArray()
                                                     select new Ordergrouptest
                                                     {
                                                         Finalprice = g.Sum(x => x.Finalprice),
                                                         Testnames = string.Join(",", pname)
                                                     }).ToList<Ordergrouptest>();
            ViewState["testname"] = testitemList[0].Testnames;
        }

        #region 删除明细
        //删除明细
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count <= 0) { MessageBoxShow("还没选择要删除的记录！"); return; }
                gvList.Rows.Remove(gvList.Rows[gvList.SelectedRowIndexArray[0]]);
                gvList.DataSource = GetData(Request["ordernum"].ToString());
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }
        #endregion

        #region 保存
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            TotalPrice();
        }

        //重新计算合计的实收金额
        private void TotalPrice()
        {
            try
            {
                List<Ordergrouptest> list = GetData(Request["ordernum"].ToString());

                //定义保存修改实收价格的集合
                List<Ordergrouptest> _newgrouptestList = new List<Ordergrouptest>();
                list.ToList().ForEach(i => _newgrouptestList.Add(i.Copy<Ordergrouptest>()));

                //刷新折扣率
                //double? discount = null;
                //if (!string.IsNullOrEmpty(tbDiscount.Text.Trim()))
                //{
                //    try
                //    {
                //        discount = Convert.ToDouble(tbDiscount.Text.Trim());
                //        if (discount <= 0 || discount > 1) { throw new Exception(); }
                //    }
                //    catch (Exception)
                //    {
                //        MessageBoxShow("折扣率输入只能输入(0-1]之间的数字！"); return;
                //    }

                //    foreach (Ordergrouptest test in _newgrouptestList)
                //    {
                //        //如果有折扣率 实收价格 = 实收价格*折扣率
                //        test.Finalprice = test.Standardprice * discount;
                //    }
                //}

                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    System.Web.UI.WebControls.TextBox tbxfinalprice = (System.Web.UI.WebControls.TextBox)gvList.Rows[i].FindControl("tbxFinalprice");
                    double? finalprice = null;
                    if (string.IsNullOrEmpty(tbxfinalprice.Text.Trim()))
                    {
                        MessageBoxShow("实收价格不能为空！");
                        return;
                    }
                    finalprice = double.Parse(tbxfinalprice.Text.Trim().ToString());

                    //判断集合中实收价格是否有修改
                    if (gvList.Rows[i].Values[0] == list[i].Ordergrouptestid.ToString() && list[i].Finalprice != finalprice)
                    {
                        _newgrouptestList[i].Finalprice = finalprice;
                    }
                }

                tbxModifytotalprice.Text = _newgrouptestList.Sum(c => c.Finalprice).ToString();

                //修改实收价格
                BilldetailService service = new BilldetailService();
                string ordernum = Request["ordernum"].ToString();
                double dictlabid = double.Parse(ViewState["dictlabid"].ToString());
                double? dictcustomerid = null;


                if (ViewState["dictcustomerid"] != null)
                {
                    dictcustomerid = Convert.ToDouble(ViewState["dictcustomerid"].ToString());
                }

                bool result = service.BillIndividualUpdatePrice(list, _newgrouptestList, dictcustomerid, ordernum, dictlabid, txtareaRemark.Text.Trim());
                if (result)
                {

                    MessageBoxShow("操作成功 ！");
                    btnSave.Enabled = false;
                    btnPrint.Enabled = true;
                    gvList.DataSource = _newgrouptestList;
                    gvList.DataBind();

                    //刷新gridview文本框的值
                    //if (string.IsNullOrEmpty(tbDiscount.Text.Trim()))
                    //    return;
                    //for (int i = 0; i < gvList.Rows.Count; i++)
                    //{
                    //    System.Web.UI.WebControls.TextBox tbxFinalprice = (System.Web.UI.WebControls.TextBox)gvList.Rows[i].FindControl("tbxFinalprice");
                    //    if (!string.IsNullOrEmpty(tbxFinalprice.Text))
                    //        tbxFinalprice.Text = Convert.ToString(double.Parse(tbxFinalprice.Text));
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            double? discount = null;
            //刷新gridview文本框的值
            if (string.IsNullOrEmpty(tbDiscount.Text.Trim())) { MessageBoxShow("请先填写折扣率！"); return; }

            try
            {
                discount = Convert.ToDouble(tbDiscount.Text.Trim());
                if (discount <= 0 || discount > 1) { throw new Exception(); }
            }
            catch (Exception)
            {
                MessageBoxShow("折扣率输入只能输入（0-1]之间的数字！"); return;
            }
            double num = 0;
            for (int i = 0; i < gvList.Rows.Count; i++)
            {
                System.Web.UI.WebControls.TextBox tbxFinalprice = (System.Web.UI.WebControls.TextBox)gvList.Rows[i].FindControl("tbxFinalprice");
                System.Web.UI.WebControls.HiddenField HF_Finalprice = (System.Web.UI.WebControls.HiddenField)gvList.Rows[i].FindControl("HF_Finalprice");
                if (!string.IsNullOrEmpty(tbxFinalprice.Text))
                {
                    string lb = HF_Finalprice.Value;
                    tbxFinalprice.Text = Convert.ToString(double.Parse(lb) * discount);
                    tbxFinalprice.ToolTip = tbxFinalprice.Text;
                    num += Convert.ToDouble(tbxFinalprice.Text);
                }
            }
            tbxModifytotalprice.Text = num.ToString();
            gvList.UpdateTemplateFields();
        }


        #region 打印
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            Hashtable htinfo = new Hashtable();
            htinfo["Ordernum"] = tbxOrdernum.Text.Trim();
            htinfo["Realname"] = tbxName.Text.Trim();
            htinfo["Productname"] = ViewState["testname"];
            htinfo["Finalprice"] = tbxModifytotalprice.Text + "(元)";
            htinfo["Billbyname"] = Userinfo.userId;
            htinfo["Billbydate"] = System.DateTime.Now.Year + "年" + System.DateTime.Now.Month + "月" + System.DateTime.Now.Day + "日";
            htinfo["titleName"] = Request["dictlabname"] == null ? "" : Request["dictlabname"];
            DataSet ds = comm.ConvertToDataSet(htinfo);

            //打印
            SetInitlocalsetting(hdMac.Text);//获取打印机配置信息
            CommonReport commonReport = new CommonReport();
            Report report = new Report();          
            report = commonReport.GetReportByDataset("30", ds, 1);     
            commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData, Userinfo);
            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
            btnPrint.Enabled = false;
            gvList.Rows.Clear();            
        }
        #endregion
    }
}