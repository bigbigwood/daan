using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.dict;
using ExtAspNet;
using daan.util.Web;
using System.Collections;

namespace daan.web.admin.dict
{
    public partial class DictCustomerProductPriceAudit : PageBase
    {
        double CustomerId = 0;
        static DicttestitemService dicttestservice = new DicttestitemService();
        static DictcustomerdiscountedService cs = new DictcustomerdiscountedService();
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerId = Request.QueryString["id"] == null ? 0 : Convert.ToDouble(Request.QueryString["id"].ToString());
            if (!IsPostBack)
            {
                BindProductList();
                BindSubCompanyList();
                BindGrid();
            }
            btnAudit.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnNaudit.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
        }

        #region >>>> 页面初始化
        /// <summary>
        /// 绑定套餐下拉列表
        /// </summary>
        private void BindProductList()
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetProduct(CustomerId);
            dropProducts.DataSource = TestItemList;
            dropProducts.DataTextField = "Testname";
            dropProducts.DataValueField = "Dicttestitemid";
            dropProducts.DataBind();
            dropProducts.Items.Insert(0, new ExtAspNet.ListItem("请选择套餐", "-1"));
        }
        /// <summary>
        /// 绑定签约子公司列表
        /// </summary>
        private void BindSubCompanyList()
        {
            IList<daan.domain.DictSubCompany> list = new DictSubCompanyService().GetDictSubCompanyList("");
            dropSubCompany.DataSource = list;
            dropSubCompany.DataTextField = "SubCompanyName";
            dropSubCompany.DataValueField = "SubCompanyId";
            dropSubCompany.DataBind();
            dropSubCompany.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion

        #region >>>> 事件实现
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.Dp_Bingin.Text != "" && this.DatePicker3.Text != "")
            {
                if (this.Dp_Bingin.SelectedDate <= this.DatePicker3.SelectedDate)
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
                if (this.Dp_Bingin.Text != "" || this.DatePicker3.Text != "")
                {
                    MessageBoxShow("请输入开始时间及结束时间查询", MessageBoxIcon.Information);
                }
                else
                {
                    BindGrid();
                }
            }
        }

        protected void btnAudit_Click(object sender,EventArgs e)
        {
            int[] strSelect = gvList.SelectedRowIndexArray;
            if (strSelect.Length > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < strSelect.Length; i++)
                {
                    //已审核的单位信息无需重审
                    if (gvList.DataKeys[strSelect[i]][1].ToString() == "0")
                    {
                        str += gvList.DataKeys[strSelect[i]][0].ToString() + ",";
                    }
                }
                str = str.TrimEnd(',');
                if (str.Length == 0)
                {
                    MessageBoxShow("所选的都已审核，无需重审!"); return;
                }
                double? auditby = null;
                if (Userinfo.userId != 0)
                {
                    auditby = Userinfo.userId;
                }
                else
                {
                    auditby = 1;
                }
                if (cs.AuditDictcustomerdiscounted(str, CustomerId.ToString(), "1", auditby))
                {
                    MessageBoxShow("审核成功");
                    BindGrid();
                    gvList.SelectedRowIndexArray = new int[] { };
                }
            }
        }

        protected void btnNaudit_Click(object sender, EventArgs e)
        {
            int[] strSelect = gvList.SelectedRowIndexArray;
            if (strSelect.Length > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < strSelect.Length; i++)
                {
                    //已审核的单位信息无需重审
                    if (gvList.DataKeys[strSelect[i]][1].ToString() == "1")
                    {
                        str += gvList.DataKeys[strSelect[i]][0].ToString() + ",";
                    }
                }
                str = str.TrimEnd(',');
                if (str.Length == 0)
                {
                    MessageBoxShow("所选的都未审核，不能取消审核!"); return;
                }
                double? auditby = null;
                if (Userinfo.userId != 0)
                {
                    auditby = Userinfo.userId;
                }
                else
                {
                    auditby = 1;
                }
                if (cs.AuditDictcustomerdiscounted(str, CustomerId.ToString(), "0", auditby))
                {
                    MessageBoxShow("取消审核成功");
                    BindGrid();
                    gvList.SelectedRowIndexArray = new int[] { };
                }
            }
        }

        protected void txtStrKey_TriggerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStrKey.Text.Trim()))
                return;
            BindGrid();
            txtStrKey.Text = string.Empty;
        }

        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion

        #region >>>> 私有方法
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", Dp_Bingin.Text.ToString() == "" ? null : Dp_Bingin.Text);
                ht1.Add("endDate", this.DatePicker3.Text.ToString() == "" ? null : this.DatePicker3.SelectedDate.Value.AddDays(1).ToShortDateString());
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("productid",dropProducts.SelectedValue);
                ht1.Add("active", dropStatus.SelectedValue);
                ht1.Add("dictsubcompanyid",dropSubCompany.SelectedValue);
                ht1.Add("Dictcustomerid", CustomerId);
                //设置总项数
                gvList.RecordCount = cs.GetDictcustomerdiscountedPageLstCount(ht1);
                gvList.DataSource = cs.GetDictcustomerdiscountedPageDt(ht1);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}