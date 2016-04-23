using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using daan.service.order;
using System.Collections;

namespace daan.web.admin.analyse
{
    public partial class AnaFocusPrintDetail : System.Web.UI.Page
    {
        OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["order_num"] == null || string.IsNullOrEmpty(Request.QueryString["order_num"].ToString()))
                return;
            if (Request.QueryString["reporttype"] == null || string.IsNullOrEmpty(Request.QueryString["reporttype"].ToString()))
                return;
            string ordernum = Request.QueryString["order_num"].ToString();
            string tp = Request.QueryString["reporttype"].ToString();
            SetInfo(ordernum,tp);
        }
        //设置页面
        private void SetInfo(string ordernum,string t)
        {
            if (t == "1")//迟发
            {
                reportType.InnerText = "标本迟发通知";
                divDelay.InnerText = "预计发单日期：" + DateTime.Now.ToString("yyyy-MM-dd");
                lblType.Text = "迟发";
            }
            else if (t == "2")//退单
            {
                reportType.InnerText = "不合格标本退单通知";
                //divDelay.Disabled = false;
                divDelay.InnerText = "";
                lblType.Text = "退单";
            }
            lblDatetime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //设置订单信息
            using (DataTable dt = os.GetOrderExceptionInfo(ordernum))
            {
                if (dt != null || dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    labName.InnerText = lblLabname.Text = dr["labname"].ToString();
                    lblCustomername.Text = dr["customername"].ToString();
                    lblDate.Text = dr["samplingdate"].ToString();
                    lblRealname.Text = dr["realname"].ToString();
                    lblProductName.Text = dr["ordertestlst"].ToString().TrimEnd(',');
                }
            }
            //设置异常信息
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            ht.Add("reportstatus", t);
            using (DataTable dt_Exception = os.GetOrderException(ht))
            {
                if (dt_Exception != null || dt_Exception.Rows.Count > 0)
                {
                    lblBarcode.Text = dt_Exception.Rows[0]["barcode"].ToString();
                    lblReason.Text = dt_Exception.Rows[0]["exceptionreason"].ToString();
                }
            }
        }
    }
}