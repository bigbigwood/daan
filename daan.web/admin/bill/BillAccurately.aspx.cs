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
    public partial class BillAccurately : PageBase
    {
        BilldetailService detailservice = new BilldetailService();
        CommonFuncLibService comm = new CommonFuncLibService();
        OrdergrouptestService grouptestService = new OrdergrouptestService();
        LoginService loginService = new LoginService();
        BilldetailService billdetailService = new BilldetailService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(Request["ordernum"]) || string.IsNullOrEmpty(Request["billheadid"]))
                    return;

                BindData(Request["ordernum"].ToString(), Request["billheadid"].ToString());
            }
        }

        //页面加载时显示数据
        private void BindData(string ordernum, string billheadid)
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
            BindGridview(billheadid);
        }
        //绑定检测项目列表
        private void BindGridview(string billheadid)
        {
            IList<Billdetail> list = billdetailService.GetBilldetailListByHeadid(billheadid);     //grouptestService.GetOrdergrouptestList(ordernum).ToList();
            this.gvList.DataSource = list;
            this.gvList.DataBind();
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
        private void GetTestNamesAndTotalPrice(IList<Billdetail> list)
        {
            List<Billdetail> testitemList = (from item in list
                                                 group item by new
                                                 {
                                                     item.Ordernum
                                                 }
                                                     into g
                                                     let pname = g.Select(x => x.Productname.ToString()).Distinct().ToArray()
                                                     select new Billdetail
                                                     {
                                                         Finalprice = g.Sum(x => x.Finalprice),
                                                         Testname = string.Join(",", pname)
                                                     }).ToList<Billdetail>();
            ViewState["testname"] = testitemList[0].Testname;
        }

        #region 删除明细
        //删除明细
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gvList.Rows.Count <= 0)
        //            return;
        //        string ordergrouptestid = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[0].ToString();
        //        grouptestService.DeleteOrdergrouptest(ordergrouptestid);
        //        BindGridview(Request["ordernum"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxShow(ex.Message,MessageBoxIcon.Error);
        //    }
        //}
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
                List<Billdetail> list = billdetailService.GetBilldetailListByHeadid(Request["billheadid"].ToString()).ToList();
                if (list.Count == 0)
                    return;

                //存放新值集合
                List<Billdetail> _newdetailList = new List<Billdetail>();
                list.ForEach(i => _newdetailList.Add(i.Copy<Billdetail>()));

                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    System.Web.UI.WebControls.TextBox tbxfinalprice = (System.Web.UI.WebControls.TextBox)gvList.Rows[i].FindControl("tbxFinalprice");
                    double? finalprice = null;
                    if (!string.IsNullOrEmpty(tbxfinalprice.Text.Trim()))
                        finalprice = double.Parse(tbxfinalprice.Text.Trim().ToString());

                    //判断值实收价格是否有修改
                    if (gvList.Rows[i].Values[6] == list[i].Billdetailid.ToString() && list[i].Finalprice != finalprice)
                    {
                        _newdetailList[i].Finalprice = finalprice;
                    }
                }

                tbxModifytotalprice.Text = _newdetailList.Sum(c => c.Finalprice).ToString();

                //修改实收价格
                detailservice.UpdateBilldetailFinalprice(list, _newdetailList, Request["billheadid"].ToString(), Request["ordernum"].ToString(), "nosendout");
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                //MessageBoxShow("修改成功");
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
            if (string.IsNullOrEmpty(tbDiscount.Text.Trim()))
                return;
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
                    num += Convert.ToDouble(tbxFinalprice.Text);
                }
            }
            tbxModifytotalprice.Text = num.ToString();
            gvList.UpdateTemplateFields();
        }


      
    }
}