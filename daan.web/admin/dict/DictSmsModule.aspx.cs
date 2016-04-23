using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using daan.service.dict;
using System.Text;
using ExtAspNet;
using daan.domain;
using System.Collections;
using daan.util.Web;
using daan.service.login;
using daan.util.Common;
using daan.web.code;
namespace daan.web.admin.dict
{
    public partial class DictSmsModule : PageBase
    {
        DictSmsModuleService dictSmsModuleService = new DictSmsModuleService();
        daan.domain.DictSmsModule dictSmsModule = null;
        string erreyType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
                btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());               
            }
        }
        #region >>>初始化及分页        
        //初始化
        private void BindGrid()
        {
            //清除掉已选择的项，以防报下标错误
            gvList.SelectedRowIndexArray = new int[] { };               
            var dictSmsModuleLst = dictSmsModuleService.GetDictSmsModuleLst(null);
            gvList.DataSource = dictSmsModuleLst;
            gvList.DataBind();
            SetEditDate();
        }        
        #endregion

        #region >>>编辑
        //获取选中行详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            SetEditDate();
        }
        //新增
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            gvList.SelectedRowIndexArray = new int[] { };
            SetEditDate();
        }
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //获取文本框值
            if (GetEditDate())
            {
                MessageBoxShow("保存数据成功！");
                BindGrid();
            }
            else
            {
                MessageBoxShow(erreyType);                
                BindGrid();
            }
        }
        //删除
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            int nflag = dictSmsModuleService.DelDictSmsModuleByID(gvList.DataKeys[gvList.SelectedRowIndexArray[0]][0].ToString());
            if (nflag > 0)
            {
                MessageBoxShow("所选项已成功删除");
                BindGrid();
            }
        }
        //加载当前行至编辑框
        protected void SetEditDate()
        {
            //新增
            dictSmsModule = new daan.domain.DictSmsModule();
            SimpleFormEdit.Title = "当前状态-新增";

            //编辑
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                SimpleFormEdit.Title = "当前状态-编辑";

                string[] objValue = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values;
                dictSmsModule.DictSmsModuleid = TypeParse.StrToInt(objValue[0], 0);                
                dictSmsModule.SmsTitle = objValue[1];
                dictSmsModule.SmsContent = objValue[2];                
            }
            txtTitle.Text = dictSmsModule.SmsTitle;
            txtContent.Text = dictSmsModule.SmsContent;            
        }
        //加载文本框数据至对象
        protected bool GetEditDate()
        {
            //bool falg = true;
            dictSmsModule = new daan.domain.DictSmsModule();
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                dictSmsModule.DictSmsModuleid = TypeParse.StrToInt(objValue[0], 0);
            }
            dictSmsModule.SmsTitle = txtTitle.Text.Trim();
            dictSmsModule.SmsContent = txtContent.Text.Trim();
            return dictSmsModuleService.SaveDictSmsModule(dictSmsModule);
        }
        #endregion
    }
}