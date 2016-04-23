using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using System.Collections;
using daan.service.order;
using daan.domain;
namespace daan.web.admin.exceptional
{
    public partial class FrmLisExceptionEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    MessageBoxShow("编辑对像的ID为空，请返回列表选择要编辑的对象。", MessageBoxIcon.Warning);
                    PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
                }
                btnClose.OnClientClick = ActiveWindow.GetConfirmHidePostBackReference();
                Orderexception orderException=new OrderexceptionService().SelectOrderExceptionInfo(Request.QueryString["id"].ToString());
                txtRemark.Text = orderException.Remark;
                txtResult.Text = orderException.Suggestion;
            }
        }
        //保存
        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            if(Request.QueryString["id"]==null)
            {
                MessageBoxShow("编辑对像的ID为空，请返回列表选择要编辑的对象。",MessageBoxIcon.Warning);
                ActiveWindow.GetConfirmHidePostBackReference();
                return;
            }
            Hashtable ht = new Hashtable();
            ht.Add("Disposeby",this.Userinfo.userName);
            ht.Add("Suggestion",txtResult.Text.Trim());
            ht.Add("Disposedate", DateTime.Now);
            ht.Add("Orderexceptionid", Request.QueryString["id"].ToString());
            try
            {
                if(!new OrderexceptionService().UpdateOrderException(ht))
                {
                    MessageBoxShow("保存失败",MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) 
            {
                MessageBoxShow("系统错误:"+ex.Message,MessageBoxIcon.Error);
            }
        }
    }
    
}