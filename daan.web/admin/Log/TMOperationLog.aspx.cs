using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.order;
using ExtAspNet;

namespace daan.web.admin.Log
{
    public partial class TMOperationLog : PageBase
    {
        static readonly OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDictLab();
                Dp_BeginDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                Dp_EndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        #region >>>> 1、控件初始化
        // 绑定分点        
        private void BindDictLab()
        {
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            if (dropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(dropDictLab.SelectedValue));
            }
        }

        // 绑定单位        
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }
        #endregion

        #region >>>> 2、控件方法实现
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        { 
        
        }

        //导出
        protected void btnExcel_Click(object sender, EventArgs e)
        { 
        
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToDouble(dropDictLab.SelectedValue));
        }
        #endregion

        #region >>>> 3、私有方法

        #endregion
    }
}