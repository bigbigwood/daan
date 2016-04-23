using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using daan.service.bill;
using ExtAspNet;

namespace daan.web.admin.bill
{
    public partial class BillRemark :PageBase
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnCanCel.OnClientClick = ActiveWindow.GetHidePostBackReference();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Request["orderNum"]) || string.IsNullOrEmpty(Request["billheadid"]))
                    return;

                BilldetailService detailService = new BilldetailService();
                Hashtable ht = new Hashtable();
                ht["ordernum"] = Request["orderNum"].ToString();
                ht["billheadid"] = Request["billheadid"].ToString();
                ht["remark"] = tbaRemark.Text.Trim();
                ht["selfremark"] = tbaSelfRemark.Text.Trim();
                detailService.UpdateBilldetailRemark(ht);
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        } 
    }
}