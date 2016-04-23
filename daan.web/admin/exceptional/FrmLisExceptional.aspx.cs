using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.order;
using System.Data;
using System.Collections;
using daan.domain;
using daan.service.login;
using daan.util.Web;
using daan.web.code;
namespace daan.web.admin.exceptional
{
    public partial class FrmLisExceptional : PageBase
    {
        OrderexceptionService service = new OrderexceptionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {             
               datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
               dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Binder();
        }

        protected void GridOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            Binder();
        }

        protected void Binder()
        {
            PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);
            Hashtable ht = new Hashtable();
            ht.Add("Disposestate", DropState.SelectedValue);
            ht.Add("OrderNum", TextUtility.ReplaceText(txtOrderNum.Text));
            ht.Add("BarCode", TextUtility.ReplaceText(txtBarCode.Text));
            ht.Add("startDate", datebegin.Text.ToString() == "" ? null : datebegin.Text.ToString());
            ht.Add("endDate", dateend.Text.ToString() == "" ? null : dateend.Text.ToString());
            ht.Add("pageStart", pageUtil.GetPageStartNum());
            ht.Add("pageEnd", pageUtil.GetPageEndNum());

            //设置总项数
            GridOrders.RecordCount = service.SelectOrderExceptionCount(ht);
            GridOrders.DataSource = service.SelectOrderExceptionLst(ht);
            GridOrders.DataBind();
        }
    }
}