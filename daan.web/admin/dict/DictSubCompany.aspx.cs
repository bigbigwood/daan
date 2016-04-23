using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using daan.web.code;
using ExtAspNet;
using daan.service.dict;
using System.Text;

namespace daan.web.admin.dict
{
    public partial class DictSubCompany : PageBase
    {

        DictSubCompanyService companyService = new DictSubCompanyService();
        daan.domain.DictSubCompany subCompany = new daan.domain.DictSubCompany();

        string errorType = "";
        public int subCompanyId
        {
            get { return Convert.ToInt32(ViewState["SubCompanyId"] == null ? 0 : ViewState["SubCompanyId"]); }
            set { ViewState["SubCompanyId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGrid();
            }
            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());

        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        // 执行清空动作
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }

        // 执行搜索动作   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {
            BindGrid();
            btSearch.ShowTrigger1 = true;
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleFormEdit.Title = "当前状态-新增";
                subCompanyId = 0;
                LoadEditDate();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                //设置总项数
                gvList.RecordCount = new DictSubCompanyService().GetDictlabdeptPageLstCount(ht1);
                gvList.DataSource = new DictSubCompanyService().GetDictlabdeptPageLst(ht1);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        #region 绑定项目详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                SimpleFormEdit.Title = "当前状态-编辑";
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = gvList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                        subCompanyId = Convert.ToInt32(gvList.DataKeys[e.RowIndex][0].ToString());//获取Grid1中第e.RowIndex+1行的第一个DateKeyName值
                        //编辑绑定数据 
                        LoadEditDate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void LoadEditDate()
        {
            subCompany.SubCompanyId = subCompanyId;
            daan.domain.DictSubCompany dictsubfo = new DictSubCompanyService().GetDictSubcompanyById(subCompany);
            if (dictsubfo == null)
            {
                subCompany = new daan.domain.DictSubCompany();
                SimpleFormEdit.Title = "当前状态-新增";
                this.tbxSUBCOMPANYNAME.Text = subCompany.SubCompanyName;
                this.tbxADDRES.Text = subCompany.Addres;
                this.tbxDISPLAYORDER.Text = subCompany.Displayorder.ToString();
                this.tbxPHONE.Text = subCompany.Phone;
                this.TbsREMARK.Text = subCompany.Remark;
            }
            else
            {
                this.tbxSUBCOMPANYNAME.Text = dictsubfo.SubCompanyName;
                this.tbxADDRES.Text = dictsubfo.Addres;
                this.tbxDISPLAYORDER.Text = dictsubfo.Displayorder.ToString();
                this.tbxPHONE.Text = dictsubfo.Phone;
                this.TbsREMARK.Text = dictsubfo.Remark;
            }
        }
        #endregion


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                BindGrid();
                MessageBoxShow("保存成功！");
                subCompanyId = 0;
                LoadEditDate();
            }
            else
            {
                MessageBoxShow(errorType);
                BindGrid();
                subCompanyId = 0;
                // LoadEditDate();
                return;
            }
        }

        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            try
            {
                if (this.tbxADDRES.Text.Trim() != "")
                {
                    subCompany.Addres = this.tbxADDRES.Text.Trim();
                }
                else
                {
                    errorType = "地址不能为空！";
                    return false;
                }
                if (this.tbxSUBCOMPANYNAME.Text.Trim() != "")
                {
                    subCompany.SubCompanyName = this.tbxSUBCOMPANYNAME.Text.Trim();
                }
                else
                {
                    errorType = "子公司名称不能为空！";
                    return false;
                }
                if (this.tbxPHONE.Text.Trim() != "")
                {
                    subCompany.Phone = this.tbxPHONE.Text.Trim();
                }
                else
                {
                    errorType = "联系电话不能为空！";
                    return false;
                }
                subCompany.Displayorder = Convert.ToInt32(this.tbxDISPLAYORDER.Text.Trim() == "" ? 0 : Convert.ToInt32(this.tbxDISPLAYORDER.Text.Trim()));
                subCompany.Remark = this.TbsREMARK.Text.Trim();
                subCompany.SubCompanyId = subCompanyId;
                return companyService.SaveDictSubCompany(subCompany);
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
                return false;
            }
        }


        #region 页面事件 删除
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (int row in gvList.SelectedRowIndexArray)
                {
                    sb.Append(gvList.DataKeys[row][0].ToString());
                    subCompany.SubCompanyId = Convert.ToDouble(gvList.DataKeys[row][0].ToString());
                    subCompany = companyService.GetDictSubcompanyById(subCompany);
                }
                var library = new DictSubCompanyService();
                int nflag = library.DelSubcompanyById(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    BindGrid();
                    subCompanyId = 0;
                    LoadEditDate();
                }


            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }
        #endregion
    }
}