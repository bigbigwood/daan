using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.order;
using daan.domain;
using System.Collections;
using ExtAspNet;

namespace daan.web.admin.proceed
{
    public partial class ProDataResult : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(Request["ordernum"]))
                    return;

                BindData(Request["ordernum"].ToString());
            }
        }
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
                ht["OrderNum"] = ordernum;
                ht["UserId"] = Userinfo.userId;
                OrderTestService testservice = new OrderTestService();
                IList<Ordertest> testList = testservice.GetOrderLabdeptresultList(ht);
                this.gvList.DataSource = testList;
                this.gvList.DataBind();
            }
            catch
            {
                MessageBoxShow("操作发生异常，请于管理员联系",MessageBoxIcon.Error);
            }
        }
        protected void gvList_RowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.DataItem != null)
            {
                string result = gvList.Rows[e.RowIndex].Values[2].ToString();
                if (result == "0")
                    gvList.Rows[e.RowIndex].Values[2] = "正常";
                else
                    gvList.Rows[e.RowIndex].Values[2] = "异常";

                string hlflag = gvList.Rows[e.RowIndex].Values[4].ToString();
                if (hlflag == "H")
                {
                    gvList.Rows[e.RowIndex].Values[4] = "↑";
                }
                else if (hlflag == "L")
                {
                    gvList.Rows[e.RowIndex].Values[4] = "↓";
                }
            }
        }
    }
}