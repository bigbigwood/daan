using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.service.order;
using System.Collections;
using daan.domain;

namespace daan.web.admin.analyse
{
    public partial class AnaCustomTraceHandle_Window :PageBase
    {
  


        #region 字段
      
        OrderserviceinfoService _orderserviceinfoService = new OrderserviceinfoService();
        OrdersService _ordersService = new OrdersService();
        #endregion


        #region 页面逻辑方法
        #endregion


        #region 页面事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState.Add("ordernum",Request.QueryString["ordernum"]);
                ViewState.Add("orderbarcode", Request.QueryString["orderbarcode"]);
               
            }
        }

        /// <summary>
        /// 保存客户跟进内容事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Hashtable ht2 = new Hashtable();
            Orderserviceinfo orderserviceinfo = new Orderserviceinfo();
            orderserviceinfo.Dictuserid = "1";
            orderserviceinfo.Ordernum = ViewState["ordernum"].ToString();
            orderserviceinfo.Servicecontent = tbServicecontent.Text;
            bool flag=false;
            if (dpRerundate.SelectedDate.HasValue)//预约复查时间不为空时
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("Rerundate", dpRerundate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("Ordernum", ViewState["ordernum"].ToString());
                flag = (_ordersService.EditRerundate(ht1)) && (_orderserviceinfoService.AddOrderserviceinfo(orderserviceinfo));
            }
            else
            {
                flag = _orderserviceinfoService.AddOrderserviceinfo(orderserviceinfo);

            }
            if (flag)
            {
                string content = "新加跟进内容:" + tbServicecontent.Text;
                if (dpRerundate.SelectedDate.HasValue)//预约复查时间不为空时
                {
                    content += "预约复查时间:" + dpRerundate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                _orderserviceinfoService.AddOperationLog(ViewState["ordernum"].ToString(), ViewState["orderbarcode"].ToString(), "客户追踪处理", content,
                   "新增", "");
               PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

            }
            else
            {
                MessageBoxShow("保存出错，请联系管理员解决！");
                return;
            }
        }
        #endregion

        
    }
}