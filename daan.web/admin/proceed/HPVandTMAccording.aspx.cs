using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using ExtAspNet;
using System.Collections;
using daan.service.order;

namespace daan.web.admin.proceed
{
    public partial class HPVandTMAccording : PageBase
    {
        static OrdersService os = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Dp_BeginDate.SelectedDate = DateTime.Now.AddDays(-7);
                Dp_EndDate.SelectedDate = DateTime.Now;
            }
        }
        /// <summary>
        /// 绑定查询数据列表
        /// </summary>
        private void BindData()
        {
            PageUtil pageUtil = new PageUtil(GridAcconding.PageIndex, GridAcconding.PageSize);
            Hashtable ht = new Hashtable();
            ht.Add("accordingtype",DropAccordingType.SelectedValue);
            ht.Add("begindate", Dp_BeginDate.Text == "" ? null : Dp_BeginDate.Text);
            ht.Add("enddate", this.Dp_EndDate.Text == "" ? null : this.Dp_EndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            ht.Add("pageStart", pageUtil.GetPageStartNum());
            ht.Add("pageEnd", pageUtil.GetPageEndNum());
            
            GridAcconding.RecordCount = os.GetHPVTMAccondingInfosCount(ht);
            GridAcconding.DataSource = os.GetHPVTMAccondingInfos(ht);
            GridAcconding.DataBind();
        }

        protected void GridAcconding_PageIndexChange(object sender, GridPageEventArgs e)
        {
            GridAcconding.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Dp_BeginDate.Text != "" && Dp_EndDate.Text != "")
            {
                if (Dp_BeginDate.SelectedDate <= Dp_EndDate.SelectedDate)
                {
                    BindData();
                }
                else
                {
                    MessageBoxShow("结束时间应大于开始时间！",MessageBoxIcon.Information);
                }
            }
            else
            {
                if (Dp_BeginDate.Text == "" || Dp_EndDate.Text == "")
                {
                    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                }
                else
                {
                    BindData();
                }
            }
        }
    }
}