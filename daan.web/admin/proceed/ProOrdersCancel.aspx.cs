using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.service.proceed;

namespace daan.web.admin.proceed
{
    public partial class ProOrdersCancel : PageBase
    {
        static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ordernums"] != null && !string.IsNullOrEmpty(Request.QueryString["ordernums"].ToString()))
                {
                    string ordernums = Request.QueryString["ordernums"].ToString();
                    hidOrdernums.Text = ordernums;
                }
            }
        }
        //保存
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string ordernums = hidOrdernums.Text.ToString();
            if (ordernums.Length == 0) return;
            string reason = txtReason.Text.ToString().Trim();
            if (string.IsNullOrEmpty(reason))
            {
                MessageBoxShow("请填写作废原因!"); return;
            }
            if (!mamagement.UpdateOrdersCancellation(Userinfo.userName, ordernums,reason))
            {
                MessageBoxShow("作废订单失败，请刷新页面重新操作!");
                return;
            }
            string str = mamagement.SelectOrderbarcodeByBill(ordernums);
            if (str != null && str != "")
            {
                MessageBoxShow(string.Format("作废成功,订单[{0}]已经出过账单，请及时处理！", str));
            }
            else
            {
                //MessageBoxShow("订单作废成功！");
            }
            JournalLog(ordernums, "作废订单");
            CloseWinAndRefresh();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            CloseWinAndRefresh();
        }

        private void CloseWinAndRefresh()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }


        /// <summary>写日志
        /// 
        /// </summary>
        /// <param name="ordernums"></param>
        /// <param name="str"></param>
        private static void JournalLog(string ordernums, string str)
        {
            string[] arrorder = ordernums.Split(',');
            for (int i = 0; i < arrorder.Length; i++)
            {
                mamagement.AddOperationLog(arrorder[i], "", "体检集中管理", "批量" + str + "[" + ordernums + "]", "节点信息", "批量" + str);
            }
        }
    }
}