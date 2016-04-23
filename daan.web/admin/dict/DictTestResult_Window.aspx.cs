using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.domain;
using daan.service.dict;

namespace daan.web.admin.dict
{
    public partial class DictTestResult_Window :PageBase
    {
        Dicttestitemresult testitemresult = new Dicttestitemresult();
        DicttestitemresultService testitemresultservice = new DicttestitemresultService();
        protected void Page_Load(object sender, EventArgs e)
        {
            // 关闭按钮的客户端脚本
            btnClose.OnClientClick = ActiveWindow.GetConfirmHidePostBackReference();
            string aa = Request.QueryString["testitemid"].ToString();
            testitemresult.Dicttestitemid = Request.QueryString["testitemid"] == null ? 0 : double.Parse(Request.QueryString["testitemid"].ToString());
        }

        //关闭本窗体，然后刷新父窗体
        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            if (SaveDicttestresult())
            {
                CacheHelper.RemoveAllCache("daan.GetDicttestitemresult");
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            else
            {
                MessageBoxShow("保存出错，请联系管理员解决！", MessageBoxIcon.Information);
                return;
            }
        }

        //保存数据的逻辑
        public bool SaveDicttestresult()
        {
            if ("value1".Equals(rdobtnException.SelectedValue))
            {
                testitemresult.Isexception = "0";//0,正常
            }
            if ("value2".Equals(rdobtnException.SelectedValue))
            {
                testitemresult.Isexception = "1";//1,异常
            }
            testitemresult.Displayorder = Convert.ToDouble(txtDisplayorder.Text);
            testitemresult.Result = txtResult.Text.Trim();

            return testitemresultservice.SaveDictTestItemResult(testitemresult);
        }


    }
}