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
    public partial class DictionaryItem : PageBase
    {
        DictLibraryItemService libraryitemService = new DictLibraryItemService();
        Dictlibraryitem libraryitem = null;
        string erreyType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
                btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
                DdlDictLibraryBind();
            }
        }
        #region >>>初始化及分页
        //基础字典类别下拉初始化
        private void DdlDictLibraryBind()
        {

            var libraryList = new LoginService().GetLoginDictlibraryList();

            ddlDictLibrary1.DataSource = libraryList;
            ddlDictLibrary1.DataTextField = "Libraryname";
            ddlDictLibrary1.DataValueField = "Dictlibraryid";
            ddlDictLibrary1.DataBind();
            ddlDictLibrary1.Items.Insert(0, new ExtAspNet.ListItem("全部", "0"));

            ddlDictLibrary2.DataSource = libraryList;
            ddlDictLibrary2.DataTextField = "Libraryname";
            ddlDictLibrary2.DataValueField = "Dictlibraryid";
            ddlDictLibrary2.DataBind();
        }
        //初始化
        private void BindGrid()
        {
            //清除掉已选择的项，以防报下标错误
            gvList.SelectedRowIndexArray = new int[] { };

            //分页查询条件
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable ht1 = new Hashtable();
            ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
            ht1.Add("strDictlibraryid", ddlDictLibrary1.SelectedValue == "0" ? null : ddlDictLibrary1.SelectedValue);
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());

            //设置总项数           
            gvList.RecordCount = libraryitemService.GetDictLibraryItemPageLstCount(ht1);

            var libraryitemLst = libraryitemService.GetDictLibraryItemPageLst(ht1);
            gvList.DataSource = libraryitemLst;
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
                gvList.SelectedRowIndexArray = new int[] { };
                BindGrid();
            }
            else
            {
                MessageBoxShow(erreyType);
                gvList.SelectedRowIndexArray = new int[] { };
                BindGrid();
            }
        }
        //删除
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int row in gvList.SelectedRowIndexArray)
            {
                sb.Append(gvList.DataKeys[row][0].ToString());
                sb.Append(",");
            }
            int nflag = libraryitemService.DelDictLibraryItemByID(sb.ToString().TrimEnd(','));
            if (nflag > 0)
            {
                MessageBoxShow("所选项已成功删除");
                gvList.SelectedRowIndexArray = new int[] { };
                BindGrid();
            }
        }
        //加载当前行至编辑框
        protected void SetEditDate()
        {
            //新增
            libraryitem = new Dictlibraryitem();
            SimpleFormEdit.Title = "当前状态-新增";

            //编辑
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                SimpleFormEdit.Title = "当前状态-编辑";

                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];

                libraryitem.Dictlibraryitemid = TypeParse.StrToDouble(objValue[0], 0);
                libraryitem.Dictlibraryid = TypeParse.StrToDouble(objValue[1], 0);
                //libraryitem.Wubicode = TypeParse.ObjToStr(objValue[2], "");
                //libraryitem.Pinyincode = TypeParse.ObjToStr(objValue[3], "");
                //libraryitem.Fastcode = TypeParse.ObjToStr(objValue[4], "");
                libraryitem.Itemname = TypeParse.ObjToStr(objValue[5], "");
                libraryitem.Displayorder = TypeParse.StrToDouble(objValue[6], 0);
                libraryitem.Remark = TypeParse.ObjToStr(objValue[7], "");
                libraryitem.Isactive = TypeParse.ObjToStr(objValue[8], "");
            }
            ddlDictLibrary2.SelectedValue = libraryitem.Dictlibraryid.ToString();
            //txtWubicode.Text = libraryitem.Wubicode;
            //txtPinyincode.Text = libraryitem.Pinyincode;
            //txtFastcode.Text = libraryitem.Fastcode;
            txtItemname.Text = libraryitem.Itemname;
            txtDisplayorder.Text = libraryitem.Displayorder.ToString();
            txtRemark.Text = libraryitem.Remark;
            chkIsactive.Checked = libraryitem.Isactive == "1" ? true : false;
        }
        //加载文本框数据至对象
        protected bool GetEditDate()
        {
            //bool falg = true;
            libraryitem = new Dictlibraryitem();
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                libraryitem.Dictlibraryitemid = TypeParse.StrToDouble(objValue[0], 0);
            }
            if (!txtItemname.Text.Trim().Equals(""))
            {
                libraryitem.Itemname = txtItemname.Text.Trim();
            }
            else
            {
                erreyType = "名称不能为空！";
                return false;
            }
            libraryitem.Dictlibraryid = TypeParse.StrToDouble(ddlDictLibrary2.SelectedValue, 0);
            //if (!txtWubicode.Text.Trim().Equals(""))
            //{
            //    libraryitem.Wubicode = txtWubicode.Text.Trim();
            //}
            //else
            //{

            //    erreyType = "五笔码不能为空！";
            //    return false;
            //}
            //if (!txtPinyincode.Text.Trim().Equals(""))
            //{
            //    libraryitem.Pinyincode = txtPinyincode.Text.Trim();
            //}
            //else
            //{
            //    erreyType = "拼音码不能为空！";
            //    return false;
            //}
            //if (!txtFastcode.Text.Trim().Equals(""))
            //{
            //    libraryitem.Fastcode = txtFastcode.Text.Trim();
            //}
            //else
            //{
            //    erreyType = "自定义码不能为空！";
            //    return false;
            //}
            libraryitem.Displayorder = TypeParse.StrToDouble(txtDisplayorder.Text.Trim(), 0);
            libraryitem.Remark = txtRemark.Text.Trim();
            libraryitem.Isactive = chkIsactive.Checked ? "1" : "0";
            return libraryitemService.SaveDictLibraryItem(libraryitem);

        }
        #endregion

        #region >>>搜索
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
            btSearch.ShowTrigger1 = true;
            gvList.PageIndex = 0;
            BindGrid();
        }
        #endregion

        ////获取五笔码
        //protected void txtItemname_TextChanged(object sender, EventArgs e)
        //{
        //    txtPinyincode.Text = daan.util.Common.StringUtil.GetSpellCode(txtItemname.Text.Trim());
        //    txtWubicode.Text = daan.util.Common.StringUtil.GetWBCode(txtItemname.Text.Trim()); ;
        //}
    }
}