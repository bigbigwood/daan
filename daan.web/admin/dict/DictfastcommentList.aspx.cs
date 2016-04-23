using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using daan.service.dict;
using System.Text;
using ExtAspNet;
using daan.domain;
using daan.service.login;
using daan.web.code;
using daan.util.Common;

namespace daan.web.admin.dict
{
    public partial class DictfastcommentList : PageBase
    {

        DictfastCommentService dictCommentService = new DictfastCommentService();
        Dictfastcomment dictComment = null; //new Dictfastcomment();
        LoginService loginservice = new LoginService();
        InitBasicService initBasicService = new InitBasicService();
        string erreyType = "";
       /// <summary>
       /// 快速录入模板ID
       /// </summary>
        //public double Dictfastid
        //{
        //    get { return Convert.ToDouble(ViewState["Dictfastid"] == null ? 0 : ViewState["Dictfastid"]); }
        //    set { ViewState["Dictfastid"] = value; }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGrid();
                LoadData();
            }
            //dictComment.Dictfastcommentid =  Convert.ToDouble(this.tbxID.Text);
            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
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
                gvList.RecordCount = new DictfastCommentService().GetDictfastcommentLstCount(ht1);
                gvList.DataSource = new DictfastCommentService().GetDictfastcommentPageLst(ht1);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 初始化信息
        /// </summary>
        private void LoadData()
        {
            //绑定实验室
            try
            {
                this.Drop_DictLabDepTid.DataSource = new DictlabdeptService().GetDictlabdept();// initBasicService.GetInitbasicLst("LABDEPTTYPE");
                this.Drop_DictLabDepTid.DataTextField = "Labdeptname";
                this.Drop_DictLabDepTid.DataValueField = "Dictlabdeptid";
                this.Drop_DictLabDepTid.DataBind();                
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
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = gvList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                         dictComment = new Dictfastcomment();
                         dictComment.Dictfastcommentid = TypeParse.StrToDouble(keys[0], 0);
                         this.tbxFastcode.Text = TypeParse.ObjToStr(keys[1], ""); 
                         this.tbxHotkey.Text =  TypeParse.ObjToStr(keys[2], "");
                         this.tbxKeymask.Text = TypeParse.ObjToStr(keys[3], ""); 
                         this.tbxModulename.Text = TypeParse.ObjToStr(keys[4], "");
                         this.Txa.Text = TypeParse.ObjToStr(keys[5], "");
                         this.Drop_DictLabDepTid.SelectedValue = TypeParse.StrToDouble(keys[6], 0).ToString();
                        SimpleForm3.Title = "当前状态-编辑";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

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

        #region  >>>> 页面事件 删除
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (int row in gvList.SelectedRowIndexArray)
                {
                    sb.Append(gvList.DataKeys[row][0].ToString());
                    sb.Append(",");
                }
                var library = new DictfastCommentService();
                int nflag = dictCommentService.DelDictfastcommentByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    BindGrid();
                    dictComment = new Dictfastcomment();
                    gvList.SelectedRowIndexArray = new int[] { };
                    this.tbxFastcode.Text = string.Empty;
                    this.tbxHotkey.Text = string.Empty;
                    this.tbxKeymask.Text = string.Empty;
                    this.tbxModulename.Text = string.Empty;
                    this.Drop_DictLabDepTid.SelectedValue = "-1";
                    this.Txa.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SimpleForm3.Title = "当前状态-新增";
            ClearControl();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                BindGrid();
                MessageBoxShow("保存成功！");
                ClearControl();
                SimpleForm3.Title = "当前状态-新增";
            }
            else
            {
                BindGrid();
                MessageBoxShow(erreyType);
                ClearControl();
                SimpleForm3.Title = "当前状态-新增";
                return;
            }
        }

        //清空数据
        public void ClearControl()
        {
            gvList.SelectedRowIndexArray = new int[] { };
            this.tbxFastcode.Text = string.Empty;
            this.tbxHotkey.Text = string.Empty;
            this.tbxKeymask.Text = string.Empty;
            this.tbxModulename.Text = string.Empty;
            this.Txa.Text = string.Empty;
        }

        //保存数据的逻辑方法
        public bool SaveDictlibrary()
        {
            try
            {
                dictComment = new Dictfastcomment();
                if (gvList.SelectedRowIndexArray.Length > 0)
                {
                    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                    dictComment.Dictfastcommentid = TypeParse.StrToDouble(objValue[0], 0);
                }
                dictComment.Commentdesc = this.Txa.Text.Trim();
                if (!this.Drop_DictLabDepTid.SelectedValue.Equals(""))
                {
                    dictComment.Dictlabdeptid = Convert.ToDouble(this.Drop_DictLabDepTid.SelectedValue);
                }
                else
                {
                    erreyType = "实验室不能为空！";
                    return false;
                }
                if (!this.tbxFastcode.Text.Trim().Equals(""))
                {
                    dictComment.Fastcode = this.tbxFastcode.Text.Trim();
                }
                else
                {
                    erreyType = "助记码不能为空！";
                    return false;
                }
                dictComment.Hotkey = this.tbxHotkey.Text.Trim();
                dictComment.Keymask = this.tbxKeymask.Text.Trim();
                if (!this.tbxModulename.Text.Trim().Equals(""))
                {
                    dictComment.Modulename = this.tbxModulename.Text.Trim();
                }
                else
                {
                    erreyType = "模块名称不能为空！";
                    return false;
                }
                return dictCommentService.SaveDictfastcomment(dictComment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}