using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using ExtAspNet;
using daan.service;
using System.Data;
using daan.web.code;

namespace daan.web.admin.Log
{
    public partial class LogInfo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {              
                BindDrop();
                //增加默认选择日期
                Dp_BinginDate.SelectedDate = DateTime.Now.AddDays(-7);
                Dp_EndDate.SelectedDate = DateTime.Now;
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", TextUtility.ReplaceTable(this.Drop_table.SelectedText) == "请选择" ? null : TextUtility.ReplaceTable(this.Drop_table.SelectedText));
                ht1.Add("code", TextUtility.ReplaceText(this.tbxCode.Text.Trim()) == "" ? null : TextUtility.ReplaceText(this.tbxCode.Text.Trim()));
                ht1.Add("BeginDate", this.Dp_BinginDate.Text == "" ? null : this.Dp_BinginDate.Text);
                ht1.Add("EndDate", this.Dp_EndDate.Text == "" ? null : this.Dp_EndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                //设置总项数
                gvList.RecordCount = new CommonFuncLibService().GetMaintenancelogPagePageLstCount(ht1);
                gvList.DataSource = new CommonFuncLibService().GetMaintenancelogPagePageLst(ht1);
                gvList.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void BindDrop()
        {
            try
            {
               // DataSet ds = new CommonFuncLibService().GetUserTable();

                this.Drop_table.DataSource = new CommonFuncLibService().GetUserTable();
                this.Drop_table.DataTextField = "TABLE_NAME";
                this.Drop_table.DataValueField = "TABLE_NAME";
                this.Drop_table.DataBind();
                this.Drop_table.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.Dp_BinginDate.Text != "" && this.Dp_EndDate.Text != "")
            {
                if (this.Dp_BinginDate.SelectedDate <= this.Dp_EndDate.SelectedDate)
                {
                    BindGrid();
                }
                else
                {
                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                if (this.Dp_BinginDate.Text != "" || this.Dp_EndDate.Text != "")
                {
                    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                }
                else
                {
                    BindGrid();
                }
            }
        }

    }
}