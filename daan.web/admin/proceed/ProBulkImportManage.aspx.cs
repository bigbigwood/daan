using System;
using System.Collections.Generic;
using System.Linq;
using ExtAspNet;
using daan.util.Web;
using System.Collections;
using daan.web.code;
using daan.service.order;
using System.Data;

namespace daan.web.admin.proceed
{

    public partial class ProBulkImportManage : PageBase
    {
        readonly OrderfileheaderService headerservice = new OrderfileheaderService();
        readonly OrderfiledetailService detailService = new OrderfiledetailService();
        protected void Page_Load(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=300;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                Bindlab();
                dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
                datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            }
        }
        //绑定分点
        private void Bindlab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ListItem("全部", "-1"));
            if (DropDictLab.SelectedValue != null)
                BindCustomer(DropDictLab.SelectedValue);
        }
        // 绑定单位        
        private void BindCustomer(string labid)
        {
            DropDictcustomerBinder(DropCustomer, labid, true);
        }
        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(DropDictLab.SelectedValue);
        }
        #region >>>>>页面查询
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            BindBulkImportManage();
        }
        //左边列表
        private void BindBulkImportManage()
        {
            if (string.IsNullOrWhiteSpace(datebegin.Text) || string.IsNullOrWhiteSpace(dateend.Text))
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                return;
            }
            Hashtable ht1 = new Hashtable();
            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("DateStart", datebegin.Text);
            ht1.Add("DateEnd", Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd"));
            ht1.Add("Dictcustormer", DropCustomer.SelectedValue);
            ht1.Add("Dictlabid", DropDictLab.SelectedValue);
            DataTable list = headerservice.GetBulkImportManagePageLst(ht1);
            if (list != null && list.Rows.Count > 0)
            {
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    string filename = list.Rows[i]["filename"].ToString();
                    string shortfilename = string.Empty;
                    if (filename != "")
                    {
                        shortfilename= filename.Substring(filename.LastIndexOf('\\') + 1);
                        list.Rows[i]["filename"] = shortfilename.Substring(19,shortfilename.Length-19); 
                    }
                }
            }
            gdBulkImportManageItem.DataSource = list;
            gdBulkImportManageItem.DataBind();
        }

        #endregion

        /// <summary>
        /// 显示文件内订单详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdBulkImportManageItem_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindBulkImportDetailItem();
        }
        //分页
        protected void gdBulkImportDetailItem_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gdBulkImportDetailItem.PageIndex = e.NewPageIndex;
            BindBulkImportDetailItem();

        }
        private void BindBulkImportDetailItem()
        {
            PageUtil pageUtil = new PageUtil(gdBulkImportDetailItem.PageIndex, gdBulkImportDetailItem.PageSize);
            Hashtable ht1 = new Hashtable();

            object[] keys = gdBulkImportManageItem.DataKeys[gdBulkImportManageItem.SelectedRowIndexArray[0]];
            ht1["Orderfileheaderid"] = keys[0];
            ht1["pageStart"] = pageUtil.GetPageStartNum();
            ht1["pageEnd"] = pageUtil.GetPageEndNum();

            gdBulkImportDetailItem.RecordCount = detailService.GetgdBulkImportDetailItemPageLstCount(ht1);
            DataTable dt = detailService.GetgdBulkImportDetailItemPageLst(ht1);
            gdBulkImportDetailItem.DataSource = dt;
            gdBulkImportDetailItem.DataBind();
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            OpenWindow("单位批量上传", "ProBulkImportFile.aspx");
        }
        private void OpenWindow(string title, string URL)
        {
            PageContext.RegisterStartupScript(WindowFrame.GetShowReference(URL, title));
        }
    }
}