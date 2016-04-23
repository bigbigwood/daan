using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;
using daan.util.Web;
using System.Collections;

namespace daan.web.admin.bill
{
    public partial class BillOperationLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(Request["ordernum"]))
                    return;
                BindData(Request["ordernum"]);    
            }
        }
        //页面加载绑定数据
        private void BindData(string ordernum)
        {
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);

            Hashtable ht = new Hashtable();
            ht["ordernum"] = ordernum;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
           
            OperationlogService logservice = new OperationlogService();
            IList<Operationlog> logList = logservice.SelectOperationlogByOrdernum(ht);
            gvList.RecordCount = logservice.SelectOperationlogCountByOrdernum(ordernum);
            gvList.DataSource = logList;
            gvList.DataBind();
        }
        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindData(Request["ordernum"]);
        }
    }
}