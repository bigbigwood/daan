using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.order;
using daan.service.bill;
using daan.domain;
using ExtAspNet;
using System.Collections;
using System.Data;
using daan.util.Common;
using daan.util.Web;

namespace daan.web.admin.bill
{
    public partial class BillAdjustPrice : PageBase
    {
        BilldetailService detailservice = new BilldetailService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnCanCel.OnClientClick = ActiveWindow.GetHidePostBackReference();

                if (string.IsNullOrEmpty(Request["ordernum"]) || string.IsNullOrEmpty(Request["billdetailids"]) || string.IsNullOrEmpty(Request["flag"]))
                    return;

                BindData(Request["ordernum"].ToString());
            }
        }
        //页面加载时显示数据
        private void BindData(string ordernum)
        {
            try
            {
                OrdersService service = new OrdersService();
                Orders orders = new Orders();

                orders = service.SelectOrdersByOrdernum(ordernum);
                if (orders == null)
                    return;

                //体检流水号客户的基本信息
                tbxName.Text = orders.Realname;
                tbxAge.Text = orders.Age;
                tbxCustomer.Text = orders.Customername;
                if (orders.Enterdate.HasValue)
                    tbxEnterdate.Text = orders.Enterdate.Value.ToShortDateString();
                tbxOrdernum.Text = ordernum;
                tbxSex.Text = orders.Sex;

                //体检流水号查询客户的检测项目列表
                Hashtable ht = new Hashtable();
                ht["ordernum"] = ordernum;
                ht["billdetailids"] = Request["billdetailids"].ToString();
                List<Billdetail> list = detailservice.SelectBilldetailPriceList(ht).ToList<Billdetail>();
                ViewState["Billdetail"] = list;
                this.gvList.RecordCount = list.Count;
                this.gvList.DataSource = list;
                this.gvList.DataBind();

                if (list != null && list.Count > 0)
                {
                    tbxModifytotalprice.Text = tbxTotalprice.Text = list.Sum(c => c.Finalprice).ToString();
                }
            }
            catch
            {
                MessageBoxShow("操作发生异常，请于管理员联系",MessageBoxIcon.Error);
            }
        }

        #region  保存
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0)
                return;

            //重新计算合计的实际金额
            TotalPrice();
        }

        //重新计算合计的实收金额
        private void TotalPrice()
        {
            try
            {
                List<Billdetail> list = (List<Billdetail>)ViewState["Billdetail"];

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
                    if (gvList.Rows[i].Values[0] == list[i].Billdetailid.ToString() && list[i].Finalprice != finalprice)
                    {
                        _newdetailList[i].Finalprice = finalprice;
                    }
                }

                tbxModifytotalprice.Text = _newdetailList.Sum(c => c.Finalprice).ToString();

                //修改实收价格
                bool falg = detailservice.UpdateBilldetailFinalprice(list, _newdetailList, Request["billheadid"].ToString(), Request["ordernum"].ToString(), Request["flag"].ToString());
                if (falg)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    MessageBoxShow("修改成功！");
                }
                else
                {
                    MessageBoxShow("修改失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }
        #endregion


    }

}