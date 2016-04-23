using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace daan.web.admin.analyse
{
    public partial class AnaResultSun_AddRemark : PageBase
    {
        daan.service.order.OrdersService os = new service.order.OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ordernum"] != null)
                {
                    hidOrdernum.Text = Request.QueryString["ordernum"].ToString();
                }
                GetRemarks();
            }
        }

        private void GetRemarks()
        {
            string r=os.GetRemarksByordernum(hidOrdernum.Text);
            txaRemark.Text = r;
            hidOldRemarks.Text = r;
        }

        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveRemark_Click(object sender, EventArgs e)
        {
            string remarks=txaRemark.Text;
            string oldremarks=hidOldRemarks.Text;
            if (remarks == oldremarks){return;}//没有修改备注点保存就不做任何操作
            string ordernum = hidOrdernum.Text;
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            ht.Add("remarks",remarks);

            if (!os.UpdateOrdersRemarks(ht))
            {
                return;
            }
            else
            {
                hidOldRemarks.Text = remarks;
                MessageBoxShow("保存成功!",ExtAspNet.MessageBoxIcon.Information);
            }
        }
    }
}