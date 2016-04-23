using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.proceed;
using System.Collections;
using System.Data;
using ExtAspNet;

namespace daan.web.admin.exceptional
{
    public partial class DeleteOrder : PageBase
    {
        static ProCentralizedManagementService ps = new ProCentralizedManagementService();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        //绑定数据
        private void BindDate()
        {
            string ordernum = txtOrderNum.Text.ToString().Trim();
            Hashtable ht = new Hashtable();
            ht["ordernum"] = ordernum;
            DataTable dt=ps.GetOrderDelete(ht);
            grigList.DataSource = dt;
            grigList.DataBind();
        }
        //查询
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderNum.Text.ToString().Trim()))
            {
                MessageBoxShow("订单号不能为空"); return;
            }
            BindDate();
        }
        

        //删除订单
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (grigList.Rows.Count == 0)
            {
                MessageBoxShow("没有要删除的订单");
                return;
            }
            string ordernum = grigList.DataKeys[0][0].ToString();
            Hashtable ht = new Hashtable();
            ht["ordernum"] = ordernum;
            try
            {
                if (ps.DeleteOrders(ht))
                {
                    MessageBoxShow("删除成功！");
                    BindDate();
                    txtOrderNum.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow("删除失败!" + ex.Message);
            }
        }

    }
}