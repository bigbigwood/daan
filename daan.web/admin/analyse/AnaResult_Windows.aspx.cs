using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;
using daan.service.login;
using daan.util.Common;
using daan.service.order;
using ExtAspNet;
namespace daan.web.admin.analyse
{
    public partial class AnaResult_Windows : PageBase
    {
        LoginService loginservice = new LoginService();
        OrderTestService orderTestService = new OrderTestService();
        double selectId =0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 关闭按钮的客户端脚本
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            selectId = TypeParse.StrToDouble(Request.QueryString["testId"], 0); 
            if (!IsPostBack)
            {
                Binder();
            }
        }
        //初始化可选结果列表
        private void Binder()
        {
            List<Dicttestitemresult> testitemresultListAll = loginservice.GetLoginDicttestitemresultList();
            List<Dicttestitemresult> list = (from Dicttestitemresult in testitemresultListAll where Dicttestitemresult.Dicttestitemid == selectId
                                             select Dicttestitemresult).ToList<Dicttestitemresult>();
            gvTestItemResult.DataSource = list;
            gvTestItemResult.DataBind();                               
        }       
        //保存关闭
        protected void btnSaveContinue_Click(object sender, EventArgs e)
        {           
            if (gvTestItemResult.SelectedRowIndexArray.Count<int>() > 0)
            {
                object[] objValue = gvTestItemResult.DataKeys[gvTestItemResult.SelectedRowIndexArray[0]];
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(objValue[1].ToString()) + ActiveWindow.GetHideReference());
            }
        }
    }
}