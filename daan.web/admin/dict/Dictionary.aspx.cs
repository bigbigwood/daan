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
using daan.util.Common;
using daan.web.code;
namespace daan.web.admin.dict
{
    public partial class Dictionary : System.Web.UI.Page
    {
        DictLibraryService libraryService = new DictLibraryService();
        Dictlibrary library=null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
                //btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
            }
        }

        #region >>>初始化及分页
        //初始化
        private void BindGrid()
        {            
            //清除掉已选择的项，以防报下标错误
            gvList.SelectedRowIndexArray = new int[] { };
 
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);  
            Hashtable ht1 = new Hashtable();
            ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());

            //设置总项数
            gvList.RecordCount = libraryService.GetDictLibraryPageLstCount(ht1);
            var libraryLst = libraryService.GetDictLibraryPageLst(ht1);
            gvList.DataSource = libraryLst;
            gvList.DataBind();
            
            SetEditDate();

        }              
        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion

        #region >>>编辑
        //获取选中行详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {                   

            SetEditDate();

        }
        ////新增
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    gvList.SelectedRowIndexArray = new int[] { };
        //    SetEditDate();
        //}        
        ////保存
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    //获取文本框值
        //    GetEditDate();

        //    if (libraryService.SaveDictLibrary(library))
        //    {
        //        MessageBoxShow("保存数据成功！",MessageBoxIcon.Information);
        //        gvList.SelectedRowIndexArray = new int[] {};
        //        BindGrid();
        //        CacheHelper.RemoveAllCache();
        //    }
        //}
        ////删除
        //protected void btnDelAll_Click(object sender, EventArgs e)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (int row in gvList.SelectedRowIndexArray)
        //    {
        //        sb.Append(gvList.DataKeys[row][0].ToString());
        //        sb.Append(",");
        //    }
        //    int nflag = libraryService.DelDictLibraryByID(sb.ToString().TrimEnd(','));
        //    if (nflag > 0)
        //    {
        //        MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
        //        gvList.SelectedRowIndexArray = new int[] { };
        //        BindGrid();
        //        CacheHelper.RemoveAllCache();
        //    }
        //}
        //加载当前行至编辑框
        protected void SetEditDate()
        {
            //新增
            library = new Dictlibrary();

            //编辑
            if (gvList.SelectedRowIndexArray.Length > 0)
            {    

                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];

                library.Dictlibraryid = TypeParse.StrToDouble(objValue[0], 0);
                library.Librarycode = TypeParse.ObjToStr(objValue[1], "");
                library.Libraryname = TypeParse.ObjToStr(objValue[2], "");
            }            
            txtLibraryCode.Text = library.Librarycode;
            txtLibraryName.Text = library.Libraryname;
        }
        //加载文本框数据至对象
        protected void GetEditDate()
        {
            library = new Dictlibrary();
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                library.Dictlibraryid = TypeParse.StrToDouble(objValue[0], 0);                
            }            
            library.Librarycode = txtLibraryCode.Text.Trim();
            library.Libraryname = txtLibraryName.Text.Trim();

        }
        #endregion    
  
        #region >>>搜索        
        // 执行清空动作
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }
        // 执行搜索动作   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {
            btSearch.ShowTrigger1 = true;
            gvList.PageIndex = 0;
            BindGrid();
        }
        #endregion
    }
}