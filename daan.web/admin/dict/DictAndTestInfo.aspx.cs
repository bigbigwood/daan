using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.util.Web;
using System.Collections;
using daan.domain;
using daan.util.Common;
using System.Data;
using ExtAspNet;
using System.Text;
using daan.service.login;
using daan.web.code;

namespace daan.web.admin.dict
{
    public partial class DictAndTestInfo : PageBase
    {
        #region >>>初始化
        LoginService loginservice = new LoginService();
        Dictlabandtest dictlabandtest = new Dictlabandtest();
        string dictlabId = string.Empty;//分点id

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
            if (!IsPostBack)
            {
                BindDrop();
            }
            if (!string.IsNullOrEmpty(DropDictLab.SelectedValue))
            {
                dictlabId = DropDictLab.SelectedValue;
            }
            else
            {
                MessageBoxShow("请维护分点数据!",MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDrop()
        {
            List<Dictlab> dictlab = new DictlabService().GetDictlabList();
            this.DropDictLab.DataSource = dictlab;
            this.DropDictLab.DataTextField = "Labname";
            this.DropDictLab.DataValueField = "Dictlabid";
            this.DropDictLab.DataBind();
        }
        #endregion

        #region >>>搜索
        // 执行搜索动作   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {
            BindGrid();
            btSearch.ShowTrigger1 = true;
        }
        // 执行清空动作
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }
        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
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
                ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht1.Add("IsActive", chkActive.Checked ? "1" : "0");
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("DictlabId", dictlabId);
                //设置总项数
                gvList.RecordCount = new DictlabandtestService().GetDictlabandtestPageLstCount(ht1);
                gvList.DataSource = new DictlabandtestService().GetDictlabandtestPageLst(ht1);
                gvList.DataBind();

                txtTestItemName.Text = "";
                this.radlIsactive.SelectedValue = "1";
                this.radlIssendouttest.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        public string GetType(object msg)
        {
            string flag = "";
            if (msg.ToString() != "")
            {
                if (msg.ToString() == "0")
                {
                    flag = "单项";
                }
                else if (msg.ToString() == "1")
                {
                    flag = "组合";
                }
                else if (msg.ToString() == "2")
                {
                    flag = "公用套餐";
                }
                else if (msg.ToString() == "3")
                {
                    flag = "客户套餐";
                }
            }
            else
            {
                flag = "";
            }
            return flag;
        }
        #endregion

        #region >>>点击行获取详细信息并编辑
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                string[] row = gvList.Rows[e.RowIndex].Values;  
                txtTestItemName.Text = row[1].ToString();
                radlIssendouttest.SelectedValue = row[4].ToString();//是否外包项目
                radlIsactive.SelectedValue = row[5].ToString();//是否可用
                SimpleForm3.Title = "当前状态-编辑";
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region >>>保存事件        
        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestItemName.Text.Trim()))
            {
                MessageBoxShow("请选择要操作的项",MessageBoxIcon.Information);
                return;
            }
            try
            {
                object[] keys = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                Dictlabandtest dictlabandtest =new Dictlabandtest();
                dictlabandtest.Dictlabandtestid = TypeParse.StrToDouble(keys[0].ToString(), 0);
                dictlabandtest.Issendouttest=this.radlIssendouttest.SelectedValue.ToString();
                dictlabandtest.Isactive=this.radlIsactive.SelectedValue.ToString();
                dictlabandtest.Labname = DropDictLab.SelectedText;

                new DictlabandtestService().SaveDictlabandtest(dictlabandtest);
                MessageBoxShow("保存成功！");   
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);    
            }
            BindGrid();            
        }
        protected void radlIsactive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radlIsactive.SelectedValue == "0")
            {
                MessageBoxShow("选择不可用将对数据有影响，请慎重！确定要禁用吗？", MessageBoxIcon.Warning); 
            }
        }
        #endregion

        #region >>>删除        
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestItemName.Text.Trim()))
            {
                MessageBoxShow("请选择要操作的项", MessageBoxIcon.Information);
                return;
            }
            try
            {
                SortedList SQLlist = new SortedList(new MySort());
                string dictLabandtestid = gvList.DataKeys[gvList.SelectedRowIndexArray[0]][0].ToString();

                Dictlabandtest oldDictLabandtest = new DictlabandtestService().GetDictDictlabandtestById(Convert.ToDouble(dictLabandtestid));
                Dictlabandtestprice dictLabandtestprice = new Dictlabandtestprice();
                dictLabandtestprice.Dictlabid = oldDictLabandtest.Dictlabid;
                dictLabandtestprice.Dicttestitemid = oldDictLabandtest.Dicttestitemid;

                SQLlist.Add(new Hashtable() { { "DELETE", "Dict.DeleteDictlabandtestpriceByWhere" } }, dictLabandtestprice);

                var library = new DictlabandtestService();
                SQLlist.Add(new Hashtable() { { "DELETE", "Dict.DeleteDictlabandtest" } }, dictLabandtestid);
                if (library.ExecuteSqlTran(SQLlist))
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    BindGrid();
                    this.radlIsactive.SelectedValue = "1";
                    this.radlIssendouttest.SelectedValue = "0";
                }
                CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
            BindGrid();
        }
        #endregion

        #region >>>导入所有检查项
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                
                SortedList SQLlist = new SortedList(new MySort());
                if (!string.IsNullOrEmpty(dictlabId))
                {
                    Hashtable ht = new Hashtable();
                    DictlabandtestService dictlabandtestSerivce = new DictlabandtestService();
                    ht.Add("Dictlabid", dictlabId);
                    bool flag = false;
                    flag = dictlabandtestSerivce.BatchInsert(ht);
                    if (flag)
                    {

                        MessageBoxShow("操作成功！", MessageBoxIcon.Information);
                        CacheHelper.RemoveAllCache("daan.GetDictlabandtest");
                        BindGrid();
                    }
                    else
                    {
                        MessageBoxShow("没有需要导入的数据", MessageBoxIcon.Information);                        
                    }
                }
                else
                {
                    MessageBoxShow("您还没选择分点！", MessageBoxIcon.Information);                 
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region >>>导出到Excel         
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvList.Rows.Count == 0)
                {
                    MessageBoxShow("没有导出的数据", MessageBoxIcon.Information);
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("DictlabId", dictlabId);
                ht.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht.Add("IsActive", chkActive.Checked ? "1" : "0");
                List<Dictlabandtest> dictlabandtestIList = new DictlabandtestService().GetDictlabandtestByDictlabId(ht);
                if (dictlabandtestIList.Count > 0)
                {
                    String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                    String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                    SortedList sortlist = new SortedList(new MySort());
                    sortlist.Add("Labname", "分点名称");
                    sortlist.Add("Testname", "套餐名称");
                    sortlist.Add("Createdate", "创建时间");
                    sortlist.Add("Issendouttest", "是否外包");
                    sortlist.Add("Isactive", "是否可用");
                    List<Dictlabandtest> dictlabandtestIListTo = new List<Dictlabandtest>();
                    for (int i = 0; i < dictlabandtestIList.Count; i++)
                    {
                        Dictlabandtest dictlaband = new DictlabandtestService().GetDictDictlabandtestById(Convert.ToDouble(dictlabandtestIList[i].Dictlabandtestid));
                        if (dictlaband.BoolIsactive == true)
                        {
                            dictlaband.Isactive = "是";
                        }
                        else
                        {
                            dictlaband.Isactive = "否";
                        }
                        if (dictlaband.BoolIssendouttest == true)
                        {
                            dictlaband.Issendouttest = "是";
                        }
                        else
                        {
                            dictlaband.Issendouttest = "否";
                        }
                        dictlaband.Labname = dictlabandtestIList[i].Labname;
                        dictlaband.Testname = dictlabandtestIList[i].Testname;
                        dictlabandtestIListTo.Add(dictlaband);
                    }
                    ExcelOperation<Dictlabandtest>.ExportListToExcel(dictlabandtestIListTo, sortlist, filename, sheetname);
                }
                else
                {
                    MessageBoxShow("没有导出的数据", MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion        
    }
}