using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using daan.util.Web;
using daan.service.order;
using ExtAspNet;

namespace daan.web.admin.analyse
{
    public partial class FinishOrdersList : PageBase
    {
        static OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //绑定数据
        private void BindDate()
        {
            Hashtable ht = new Hashtable();
            PageUtil pageutil = new PageUtil(gridList.PageIndex, gridList.PageSize);
            double userid = Userinfo.userId;
            ht["userid"] = userid;
            string uname=txtName.Text.ToString();
            ht["uname"] = uname;
            string status = dpStatus.SelectedValue;
            ht["status"] = status;
            string dateStart = string.Empty;
            if (!string.IsNullOrEmpty(TimeStart.SelectedDate.ToString()))
                dateStart = Convert.ToDateTime(TimeStart.SelectedDate.ToString()).ToShortDateString();
            string dateEnd = string.Empty;
            if (!string.IsNullOrEmpty(TimeEnd.SelectedDate.ToString()))
                dateEnd = Convert.ToDateTime(TimeEnd.SelectedDate.Value.AddDays(1).ToString()).ToShortDateString();
            ht["dateStart"] = dateStart;
            ht["dateEnd"] = dateEnd;
            ht["pagestart"] = pageutil.GetPageStartNum();
            ht["pageend"] = pageutil.GetPageEndNum();
            gridList.RecordCount = os.GetFinishOrdersListCount(ht,status);
            gridList.DataSource = os.GetFinishOrdersList(ht,status);
            gridList.DataBind();
        }
        //查询
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindDate();
        }
        //分页
        protected void gridList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gridList.PageIndex = e.NewPageIndex;
            BindDate();
        }
    }
}