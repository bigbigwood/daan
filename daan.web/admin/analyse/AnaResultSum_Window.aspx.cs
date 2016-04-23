using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.login;
using daan.service.order;
using System.Collections;


namespace daan.web.admin.analyse
{
    public partial class AnaResultSum_Window : System.Web.UI.Page
    {

        OrderTestService orderTestService = new OrderTestService();
        /// <summary>
        /// 根据订单号和物理测试科室id
        /// 查找相对应的详细检查结果
        /// </summary>
        string ordernum = null, dictlabdeptid=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ordernum = Request.QueryString["ordernum"];
                dictlabdeptid = Request.QueryString["dictlabdeptid"];
                BindGrid();
            }
            

        }

        /// <summary>
        /// 绑定订单对应明细项目表
        /// </summary>
        void BindGrid()
        {
            if ((ordernum==null))
            {
                return;
            }
            Hashtable ht = new Hashtable();
            ht.Add("dictlabdeptid", dictlabdeptid);
            ht.Add("ordernum", ordernum);
            gdOrdertest.DataSource = orderTestService.DataForOrderLabdeptresult(ht);
            gdOrdertest.DataBind();
        }
    }

}