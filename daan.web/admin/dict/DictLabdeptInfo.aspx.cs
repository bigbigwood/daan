using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.util.Web;
using System.Collections;
using System.Text;
using ExtAspNet;
using daan.domain;
using daan.service.login;
using daan.web.code;
using daan.util.Common;

namespace daan.web.admin.dict
{
    public partial class DictLabdeptInfo : PageBase
    {
        LoginService loginService = new LoginService();
        DictlabdeptService dictlabedpService = new DictlabdeptService();
        Dictlabdept dictlabdep = null;
        string erreyType = "";
        //public double dictlabdepId
        //{
        //    get { return Convert.ToDouble(ViewState["dictlabdepId"] == null ? 0 : ViewState["dictlabdepId"]); }
        //    set { ViewState["dictlabdepId"] = value; }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               //BindGrid();
                LoadData();
            }
            //dictlabdep.Dictlabdeptid = dictlabdepId;
            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
        }

        private void LoadData()
        {
            //绑定类型
            try
            {
                this.Drop_LabdeptTyped.DataSource = loginService.GetLoginInitbasicList().FindAll(c => c.Basictype == "LABDEPTTYPE");
                this.Drop_LabdeptTyped.DataTextField = "Basicname";
                this.Drop_LabdeptTyped.DataValueField = "Basicvalue";
                this.Drop_LabdeptTyped.DataBind();
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
                gvList.RecordCount = new DictlabdeptService().GetDictlabdeptPageLstCount(ht1);
                gvList.DataSource = new DictlabdeptService().GetDictlabdeptPageLst(ht1);
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
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = gvList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                        dictlabdep = new Dictlabdept();
                        dictlabdep.Dictlabdeptid = TypeParse.StrToDouble(keys[0], 0);
                        if (dictlabdep.Dictlabdeptid != 0)
                        {
                            this.tbxLabdeptname.Text = TypeParse.ObjToStr(keys[1], "");
                            this.Drop_LabdeptTyped.SelectedValue = TypeParse.ObjToStr(keys[2], "");
                            SimpleFormEdit.Title = "当前状态-编辑";
                        }
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

        #region 页面事件  删除
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
                int nflag = dictlabedpService.DelDictlabdeptByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除",  MessageBoxIcon.Information);
                    BindGrid();
                    gvList.SelectedRowIndexArray = new int[] { };
                    //dictlabdepId = 0;    
                    this.tbxLabdeptname.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion



        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            try
            {
                dictlabdep = new Dictlabdept();
                if (gvList.SelectedRowIndexArray.Length > 0)
                {
                    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                    dictlabdep.Dictlabdeptid = TypeParse.StrToDouble(objValue[0], 0);
                }
                if (this.tbxLabdeptname.Text.Trim() != "")
                {
                    dictlabdep.Labdeptname = this.tbxLabdeptname.Text.Trim();
                }
                else
                {
                    erreyType = "科室名称不能为空！";
                    return false;
                }
                if (this.Drop_LabdeptTyped.SelectedValue != "-1")
                {
                    dictlabdep.Labdepttype = this.Drop_LabdeptTyped.SelectedValue;
                }
                else
                {
                    erreyType = "科室类型不能为空！";
                    return false;
                }
                if (dictlabdep.Dictlabdeptid == 0 || dictlabdep.Dictlabdeptid == null)
                {
                    dictlabdep.Createdate = DateTime.Now;
                }
                else
                {
                    Dictlabdept dictdepback = new DictlabdeptService().GetDictlabdeptInfo(dictlabdep);
                    dictlabdep.Createdate = dictdepback.Createdate;
                }
                return dictlabedpService.SaveDictlabdept(dictlabdep);
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                return false;
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            gvList.SelectedRowIndexArray = new int[] { };
            this.Drop_LabdeptTyped.SelectedValue = "-1";
            this.tbxLabdeptname.Text = string.Empty;
            SimpleFormEdit.Title = "当前状态-新增";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                MessageBoxShow("保存成功！");
                BindGrid();
                gvList.SelectedRowIndexArray = new int[] { };
                this.tbxLabdeptname.Text = string.Empty;
                SimpleFormEdit.Title = "当前状态-新增";
            }
            else
            {
                MessageBoxShow(erreyType);
                BindGrid();
                gvList.SelectedRowIndexArray = new int[] { };
                this.tbxLabdeptname.Text = string.Empty;
                SimpleFormEdit.Title = "当前状态-新增";
                return;
            }
        }
    }
}