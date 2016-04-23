using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.login;
using System.Data;
using ExtAspNet;
using System.Xml;
using System.Collections;
using daan.service.order;
using daan.util.Web;
namespace daan.web
{
    public partial class Main : PageBase
    {
        static OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblCurrentUser.Text = Userinfo.userName;

                XmlDocument XD = new XmlDocument();
                XD.Load(Server.MapPath("menu.xml"));
                //for (int i = 0; i < XD.ChildNodes.Count; i++)
                //{
                //    XmlNode xn=XD.ChildNodes[i];
                //    for (int j = 0; j < xn.ChildNodes.Count; j++)
                //    {
                //        XmlNode xn2 = xn.ChildNodes[j];
                //        string sdfdf = xn2.InnerText;
                //        string sddf = xn2.Name;
                //        string dfdf = xn2.Value;
                //    }
                //}
                // 绑定 XML 数据源到树控件
                treeMenu.DataSource = XD;
                treeMenu.DataBind();
                //加载列表数据
                BindData();
            }
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            CacheHelper.RemoveAllCache();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Session["UserInfo"] = null;
            ExtAspNet.PageContext.RegisterStartupScript("window.parent.location='Login.aspx';");
        }

        //查询结果绑定
        private void BindData()
        {
            //存储过滤条件
            Hashtable ht = new Hashtable();
            PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            GridOrders.RecordCount=os.GetOverdueOrdersCount();
            GridOrders.DataSource = os.GetOverdueOrders(ht);
            GridOrders.DataBind();
        }

        //分页
        protected void GridOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            BindData();
        }    
    }
}