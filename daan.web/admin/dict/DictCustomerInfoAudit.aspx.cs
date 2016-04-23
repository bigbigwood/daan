using System;
using System.Collections.Generic;
using System.Linq;
using daan.domain;
using daan.service.dict;
using ExtAspNet;
using daan.util.Web;
using System.Collections;
using daan.web.code;
using System.Data;
using daan.util.Common;

namespace daan.web.admin.dict
{
    public partial class DictCustomerInfoAudit : PageBase
    {
        DictCustomerService dictCustomerService = new DictCustomerService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLab();
            }
        }

        #region >>> 初始化控件数据
        /// <summary>
        /// 初始
        /// </summary>
        private void BindLab()
        {
            //绑定过滤器分点数据
            DDLDictLabBinder(dropDictlab, true);
            dropDictlab.Items.Insert(0, new ListItem("全部", "-1"));

            //绑定详细信息中分店数据
            DDLDictLabBinder(dropDictlab2, true);
            
            //绑定财务人员
            this.dropDictcheckBillId.DataSource = new DictuserService().GetDictuser();
            this.dropDictcheckBillId.DataTextField = "Username";
            this.dropDictcheckBillId.DataValueField = "Dictuserid";
            this.dropDictcheckBillId.DataBind();

            //绑定销售人员
            this.dropDictsalemanId.DataSource = new DictuserService().GetDictuser();
            this.dropDictsalemanId.DataTextField = "Username";
            this.dropDictsalemanId.DataValueField = "Dictuserid";
            this.dropDictsalemanId.DataBind();
        }
        #endregion

        #region >>>> 按钮事件
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        //绑定单位详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                Dictcustomer dictCustomer = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(gvList.DataKeys[e.RowIndex][0]));
                this.radlActive.SelectedValue = dictCustomer.Active;
                this.radlCustomerType.SelectedValue = dictCustomer.Customertype;
                this.tbxAddress.Text = dictCustomer.Address;
                this.tbxContactman.Text = dictCustomer.Contactman;
                this.tbxContactPhone.Text = dictCustomer.Contactphone;
                this.tbxCoustomerCode.Text = dictCustomer.Customercode;
                this.tbxCoustomerName.Text = dictCustomer.Customername;
                this.tbxDisplayOrder.Text = dictCustomer.Displayorder.ToString();
                this.tbxDocumentCode.Text = dictCustomer.Documentcode;
                this.tbxDocumentType.Text = dictCustomer.Documenttype;
                this.tbxEmail.Text = dictCustomer.Email;
                this.tbxEnAddress.Text = dictCustomer.Engaddress;
                this.tbxEnErpCode.Text = dictCustomer.Erpcode;
                this.tbxEnName.Text = dictCustomer.Customerengname;
                this.tbxErpName.Text = dictCustomer.Erpname;
                this.tbxFastCode.Text = dictCustomer.Fastcode;
                this.tbxFax.Text = dictCustomer.Fax;
                this.tbxPostCode.Text = dictCustomer.Postcode;
                this.tbxReporTitle.Text = dictCustomer.Reporttitle;
                this.tbxTelePhone.Text = dictCustomer.Telephone;
                this.tbxCoustomerName2.Text = dictCustomer.Customername2;
                this.TxaRemark.Text = dictCustomer.Remark;
                this.dropDictcheckBillId.SelectedValue = dictCustomer.Dictcheckbillid.ToString();
                this.dropDictlab2.SelectedValue = dictCustomer.Dictlabid.ToString();
                this.dropDictsalemanId.SelectedValue = dictCustomer.Dictsalemanid.ToString();
                this.dropStatus.SelectedValue = dictCustomer.Status;
                if (dictCustomer.Issms == "1")
                    this.chkIssms.Checked = true;
                else
                    this.chkIssms.Checked = false;

                this.radIsPub.SelectedValue = dictCustomer.IsPublic;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }     
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            int[] strSelect = gvList.SelectedRowIndexArray;
            if (strSelect.Length > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < strSelect.Length; i++)
                {
                    //已审核的单位信息无需重审
                    if (gvList.DataKeys[strSelect[i]][3].ToString() == "0")
                    {
                        str += gvList.DataKeys[strSelect[i]][0].ToString() + ",";
                    }
                }
                str = str.TrimEnd(',');
                if (str.Length == 0)
                {
                    MessageBoxShow("所选的单位都已审核，无需重审!"); return;
                }
                if (dictCustomerService.AuditCustomerInfo(str, "1")>0)
                {
                    MessageBoxShow("审核成功");
                    BindData();
                    gvList.SelectedRowIndexArray = new int[] { };
                }
            }
            else
            {
                MessageBoxShow("请选择要审核的单位!", MessageBoxIcon.Information);
                return;
            }
        }
        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNaudit_Click(object sender, EventArgs e)
        {
            int[] strSelect = gvList.SelectedRowIndexArray;
            if (strSelect.Length > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < strSelect.Length; i++)
                {
                    //已审核的单位信息才能取消审核
                    if (gvList.DataKeys[strSelect[i]][3].ToString() == "1")
                    {
                        str += gvList.DataKeys[strSelect[i]][0].ToString() + ",";
                    }
                }
                str = str.TrimEnd(',');
                if (str.Length == 0)
                {
                    MessageBoxShow("所选的单位都未审核，不能取消审核!"); return;
                }
                if (dictCustomerService.AuditCustomerInfo(str, "0")>0)
                {
                    MessageBoxShow("取消审核成功");
                    BindData();
                    gvList.SelectedRowIndexArray = new int[] { };
                }
            }
            else
            {
                MessageBoxShow("请选择要取消审核的单位!", MessageBoxIcon.Information);
                return;
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindData();
        }
        protected void txtStrKey_TriggerClick(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(txtStrKey.Text.Trim()))
            //    return;
            BindData();
            txtStrKey.Text = string.Empty;
        }
        /// <summary>
        /// 单位套餐价格审核页面跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProductAudit_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRowIndexArray.Count() <= 0)
            {
                MessageBoxShow("请在列表中选择单位", MessageBoxIcon.Information);
                return;
            }
            object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
            string DICTCUSTOMERID = objValue[0].ToString();
            string Customername = objValue[1].ToString();
            string url = "DictCustomerProductPriceAudit.aspx?id=" + DICTCUSTOMERID;
            string title = "体检单位套餐价格审核 - " + Customername;
            ShowWindow(url, title);
        }
        /// <summary>
        /// 导出单位信息到EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0)
            {
                MessageBoxShow("导出没有数据！"); return;
            }
            try
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("Dictlabid", dropDictlab.SelectedValue);
                ht1.Add("IsActive", dropStatus.SelectedValue);
                ht1.Add("strKey", TextUtility.ReplaceText(txtStrKey.Text.Trim()) == "" ? null : TextUtility.ReplaceText(txtStrKey.Text.Trim()));
                DataTable dt = null;
                if (raoType.SelectedValue == "0")
                {
                    dt = dictCustomerService.GetDictcustomerExportList(ht1);
                }
                else
                {
                    dt = dictCustomerService.GetDictcustomerdiscountedExportList(ht1);
                }
                if (dt == null || dt.Rows.Count <= 0)
                {
                    MessageBoxShow("导出没有数据！"); 
                    return;
                }
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("audittext", "审核状态");
                sortlist.Add("DICTCUSTOMERID", "单位ID");
                sortlist.Add("CUSTOMERNAME", "单位名称");
                sortlist.Add("CUSTOMERCODE", "客户代码");
                sortlist.Add("labname", "分点实验室");
                sortlist.Add("ADDRESS", "单位地址");
                sortlist.Add("CONTACTMAN", "联系人");
                sortlist.Add("CONTACTPHONE", "联系人电话");
                sortlist.Add("saleman", "销售人员");
                sortlist.Add("checkbillnam", "财务人员");
                sortlist.Add("TELEPHONE", "联系电话");
                sortlist.Add("FAX", "传真");
                sortlist.Add("EMAIL", "邮箱");
                sortlist.Add("POSTCODE", "邮编");
                sortlist.Add("CUSTOMERNAME2", "单位别名");
                sortlist.Add("REMARK", "备注");
                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                ExcelOperation<DataTable>.ExportDataTableToExcel(dt, filename, sheetname,sortlist);
            }
            catch (Exception ex)
            {
                MessageBoxShow("导出数据出错。错误信息："+ex.Message);
            }
        }
        #endregion

        #region >>>> 私有方法
        /// <summary>
        /// 绑定列表数据
        /// </summary>
        private void BindData()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("Dictlabid", dropDictlab.SelectedValue);
                ht1.Add("IsActive", dropStatus.SelectedValue);
                ht1.Add("strKey", TextUtility.ReplaceText(txtStrKey.Text.Trim()) == "" ? null : TextUtility.ReplaceText(txtStrKey.Text.Trim()));
                
                DataTable dt = null;
                int count = 0;
                if (raoType.SelectedValue == "0")
                {
                    dt = dictCustomerService.GetDictcustomerAuditList(ht1);
                    count = dictCustomerService.GetDictcustomerAuditListCount(ht1);
                }
                else
                {
                    dt = dictCustomerService.GetDictcustomerdiscountedAuditList(ht1);
                    count = dictCustomerService.GetDictcustomerdiscountedAuditListCount(ht1);
                }
                gvList.DataSource = dt;
                gvList.RecordCount = count;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 弹出价格审核窗体
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="title"></param>
        void ShowWindow(string URL, string title)
        {
            PageContext.RegisterStartupScript(WinLibraryEdit.GetShowReference(URL, title));
        }
        #endregion
    }
}