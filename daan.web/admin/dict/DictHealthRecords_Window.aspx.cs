using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.service.login;
using daan.domain;
using daan.service.dict;

namespace daan.web.admin.dict
{
    public partial class DictHealthRecords_Window : PageBase
    {
        static LoginService loginService = new LoginService();
        static DictMEDHistoryService ms = new DictMEDHistoryService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string mid = string.Empty;
                if (Request.QueryString["mid"] != null)
                {
                    mid = Request.QueryString["mid"].ToString();
                    hidMemberID.Text = mid;
                }
                BindData();
            }
        }
        //绑定类型
        private void BindData()
        {
            dpType.DataSource = loginService.GetLoginInitbasicList().FindAll(v => v.Basictype == "HEALTHRECORDTYPE");
            dpType.DataTextField = "Basicname";
            dpType.DataValueField = "Basicvalue";
            dpType.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dicthealthrecords model = new Dicthealthrecords();
            model.Dictmemberid = Convert.ToDouble(hidMemberID.Text);
            model.Dictrecordtype = Convert.ToInt32(dpType.SelectedValue);
            model.Dictrecordtext = txtHealthRecord.Text;
            bool b = ms.InsertOrUpdateHealthRecord(model, true);
            if (!b)
            {
                MessageBoxShow("添加失败，请重试!"); 
                return;
            }
            else
            {
                MessageBoxShow("保存成功!");
                dpType.SelectedIndex = 0;
                txtHealthRecord.Text = string.Empty;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}