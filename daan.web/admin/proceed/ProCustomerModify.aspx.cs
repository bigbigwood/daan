using System;
using System.Collections.Generic;
using System.Linq;
using daan.domain;
using daan.service.login;
using ExtAspNet;
using System.Collections;
using daan.service.proceed;

namespace daan.web.admin.proceed
{
    public partial class ProCustomerModify : PageBase
    {
        readonly static ProRegisterService rs = new ProRegisterService();
        readonly static LoginService loginservice = new LoginService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["OrderNum"] != null && !string.IsNullOrEmpty(Request.QueryString["OrderNum"]))
                    hidOrderNum.Text = Request.QueryString["OrderNum"];
                BindDictLab();
            }
        }

        /// 绑定分点
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        //绑定单位
        private void BindCustomer(double? labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), false);
        }

        ///选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            double customerid = Convert.ToDouble(DropCustomer.SelectedValue);
            Hashtable ht = new Hashtable();
            string ordernum = hidOrderNum.Text;
            ht.Add("ordernum", ordernum);
            ht.Add("customerid", customerid);
            if (rs.UpdateCustomerByOrdernum(ht) > 0)
            {
                MessageBoxShow("保存成功");
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}