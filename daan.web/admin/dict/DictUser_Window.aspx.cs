using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;
using ExtAspNet;
using daan.service.login;

namespace daan.web.admin.dict
{
    public partial class DictUser_Window : PageBase
    {
        //单位ID
        public double DictUserId
        {
            get { return Convert.ToDouble(ViewState["DictUserId"]); }
            set { ViewState["DictUserId"] = value; }
        }
         Dictuser dictuser = new Dictuser();
         string erreyType = "";
        protected void Page_Load(object sender, EventArgs e)
        { 
            DictUserId = Request.QueryString["id"] == null ? 0 : Convert.ToDouble(Request.QueryString["id"].ToString());
            if (!IsPostBack)
            {
              
                BindDrop();
            }
            btnClose.OnClientClick = ActiveWindow.GetConfirmHidePostBackReference();
            dictuser.Dictuserid = DictUserId;
        }

        private void BindDrop()
        {
            try
            {
                List<Dictlab> dictlab = new DictlabService().GetDictlabList();
                this.Drop_Dictlab.DataSource = dictlab;
                this.Drop_Dictlab.DataTextField = "Labname";
                this.Drop_Dictlab.DataValueField = "Dictlabid";
                this.Drop_Dictlab.DataBind();
                this.Drop_Dictlab.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));

                this.Drop_DictLabDepTid.DataSource = new LoginService().GetLoginDictlabdeptList();
                this.Drop_DictLabDepTid.DataTextField = "Labdeptname";
                this.Drop_DictLabDepTid.DataValueField = "Dictlabdeptid";
                this.Drop_DictLabDepTid.DataBind();
                Drop_DictLabDepTid.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));

                dictuser.Dictuserid = DictUserId;
                Dictuser  user = new DictuserService().GetDictuserInfo(dictuser);
                this.Drop_Dictlab.SelectedValue = user.Dictlabid.ToString();
                this.Drop_DictLabDepTid.SelectedValue = user.Dictlabdeptid.ToString();
                this.radlIsactive.SelectedValue = user.Active;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                MessageBoxShow("保存成功！需重新登陆数据才可生效！");
                PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
            else
            {
                MessageBoxShow(erreyType, MessageBoxIcon.Error);
                return;
            }
        }

        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            try
            {
                Dictuser user = new DictuserService().GetDictuserInfo(dictuser);
                if (this.Drop_Dictlab.SelectedValue != "-1")
                {
                    user.Dictlabid = Convert.ToDouble(this.Drop_Dictlab.SelectedValue);
                }
                else
                {
                    erreyType = "分点不能为空！";
                    return false;
                }
                if (this.Drop_DictLabDepTid.SelectedValue != "-1")
                {
                    user.Dictlabdeptid = Convert.ToDouble(this.Drop_DictLabDepTid.SelectedValue);
                }
                else
                {
                    erreyType = "实验室不能为空！";
                    return false;
                }
                user.Active = this.radlIsactive.SelectedValue;
                return new DictuserService().SaveDictlab(user);
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}