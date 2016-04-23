using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.domain;
using System.Data;
using System.Collections;
using ExtAspNet;
using System.Text;
using daan.service.login;
using hlis.service.common;
using daan.service.common;
using daan.web.code;
using daan.util.Common;
using daan.util.Web;

namespace daan.web.admin.dict
{
    public partial class DictDiagnosis : PageBase
    {
        #region loading
        LoginService loginservice = new LoginService();
        DictDiagnosisService diagnosisservice = new DictDiagnosisService();
        DictdiagnosesmutexService dictdiagnosesmutexService = new DictdiagnosesmutexService();
        protected void Page_Load(object sender, EventArgs e)
        {
  
            if (!IsPostBack)
            {
                btnDel.OnClientClick = gdDiagnosis.GetNoSelectionAlertReference("请选择需要删除的建议字典！", "提示");
                btnDel.ConfirmText = "确实要删除该建议字典？";
                BindDiagnosistype();
                BindLabDept();
            }
        }
        #endregion

        #region >>>诊断建议字典及分页
        private void BindGrid()
        {
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdDiagnosis.PageIndex, gdDiagnosis.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());

            //设置总项数
            gdDiagnosis.RecordCount = diagnosisservice.GetDictdiagnosisPageLstCount(ht1);
            List<Dictdiagnosis> list = diagnosisservice.GetDictdiagnosisPageLst(ht1);
            gdDiagnosis.DataSource = list;
            gdDiagnosis.DataBind();

            if (list.Count!=0)
            {
                gdDiagnosis.SelectedRowIndexArray = new int[] { 0 };
                ShowDetail(list[0].Dictdiagnosisid);
            }
        }

        #endregion

        #region 绑定物理组
        public void BindLabDept()
        {
            DictlabdeptService service = new DictlabdeptService();
            List<Dictlabdept> listlabdept = loginservice.GetLoginDictlabdeptList();
          
            ddlgoupLibrary.DataSource = listlabdept;
            ddlgoupLibrary.DataValueField = "Dictlabdeptid";
            ddlgoupLibrary.DataTextField = "Labdeptname";
            ddlgoupLibrary.DataBind();
        }
        #endregion

        #region 诊断建议字典
        public List<Dictdiagnosis> GetDiagnosisAll()
        {
            List<Dictdiagnosis> diagnosisAll = loginservice.GetLoginDictdiagnosisresultList();
            return diagnosisAll;
        }

        //public void BindDiagnosis()
        //{
        //    gdDiagnosis.DataSource = GetDiagnosisAll();
        //    gdDiagnosis.DataBind();
        //}
        #endregion

        #region 绑定疾病类型
        public void BindDiagnosistype()
        {
            //疾病类型
            ddldiagnosistype.DataSource = loginservice.GetLoginInitbasicList().FindAll(c => c.Basictype == "DIAGNOSISTYPE");
            ddldiagnosistype.DataTextField = "Basicname";
            ddldiagnosistype.DataValueField = "Basicvalue";
            ddldiagnosistype.DataBind();
        }
        #endregion

        #region 查找 根据诊断名称
        //查找
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            gdDiagnosis.PageIndex = 0;
            BindGrid();
        }

        //private void BindSearchData()
        //{
        //    List<Dictdiagnosis> newslist = GetDictDiagnosisByC();
        //    gdDiagnosis.DataSource = newslist;
        //    gdDiagnosis.DataBind();
        //}

        //private List<Dictdiagnosis> GetDictDiagnosisByC()
        //{
        //    List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();
        //    string diagnosisStr = ttbSearch.Text;
        //    List<Dictdiagnosis> newslist = new List<Dictdiagnosis>();
        //    foreach (Dictdiagnosis diagnosis in diagnosisAll)
        //    {
        //        if ((diagnosis.Diagnosisname != null && diagnosis.Diagnosisname.ToLower().Contains(diagnosisStr.ToLower()))
        //           )
        //        {
        //            newslist.Add(diagnosis);
        //        }
        //    }
        //    return newslist;
        //}
        #endregion

        #region 新增
        //新增
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtdiagnosisname.Focus();
            SimpleFormEdit.Title = "当前状态-新增";
            AddDictdiagnosis();
        }

        private void AddDictdiagnosis()
        {
            gdDiagnosis.SelectedRowIndexArray = new int[] { };
            ViewState["dictdiagnosisID"] = null;
            txtdiagnosisname.Text = null;
            txtdisplayorder.Text = "1";
            ddldiagnosistype.SelectedIndex = -1;
            chbisdisease.Checked = false;
            tadiseasedescription.Text = null;
            tadiseasecause.Text = null;
            tasuggestion.Text = null;
            taengdiseasedescription.Text = null;
            taengdiseasecause.Text = null;
            taengsuggestion.Text = null;
            txtdiagnosiscode.Text = null;
        }
        #endregion

        #region 保存
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtdiagnosisname.Text.Trim()))
                {
                    MessageBoxShow("请输入诊断名称");
                    return;
                }
                if (ddldiagnosistype.SelectedValue == "-1")
                {
                    MessageBoxShow("请输入疾病分类");
                    return;
                }
                if (string.IsNullOrEmpty(tasuggestion.Text.Trim()))
                {
                    MessageBoxShow("请输入建议");
                    return;
                }               
                try
                {
                    double.Parse(txtdisplayorder.Text);
                }
                catch (Exception)
                {
                    MessageBoxShow("打印顺序请输入数字类型");
                    return;
                }
                List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();
                List<Dictdiagnosis> dictdiagnosisBack = new List<Dictdiagnosis>();

               
                Dictdiagnosis dictdiagnosis = new Dictdiagnosis();
                if (ViewState["dictdiagnosisID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["dictdiagnosisID"]);
                    dictdiagnosis = (from Dictdiagnosis in diagnosisAll where Dictdiagnosis.Dictdiagnosisid == id select Dictdiagnosis).ToList<Dictdiagnosis>()[0];
                    Hashtable ht = new Hashtable();
                    ht.Add("Dictdiagnosisid", id);
                    ht.Add("Diagnosiscode",this.txtdiagnosiscode.Text.Trim());
                    dictdiagnosisBack = new DictDiagnosisService().GetDictdiagnosisByCode(ht);
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("Diagnosiscode", this.txtdiagnosiscode.Text.Trim());
                    dictdiagnosisBack = new DictDiagnosisService().GetDictdiagnosisByCode(ht);
                }
                if (dictdiagnosisBack.Count>0)
                {
                    MessageBoxShow("已存在相同的疾病代码！");
                    return;
                }
                dictdiagnosis.Diagnosisname = txtdiagnosisname.Text;
                dictdiagnosis.Diagnosistype = ddldiagnosistype.SelectedValue;
                dictdiagnosis.Diseasecause = tadiseasecause.Text;
                dictdiagnosis.Diseasedescription = tadiseasedescription.Text;
                dictdiagnosis.Displayorder = Convert.ToDouble(txtdisplayorder.Text);
                dictdiagnosis.Engdiseasecause = taengdiseasecause.Text;
                dictdiagnosis.Engdiseasedescription = taengdiseasedescription.Text;
                dictdiagnosis.Engsuggestion = taengsuggestion.Text;
                dictdiagnosis.Suggestion = tasuggestion.Text;
                dictdiagnosis.Createdate = DateTime.Now;
                dictdiagnosis.Diagnosiscode = txtdiagnosiscode.Text;//疾病代码
                dictdiagnosis.Dictlabdeptid = Convert.ToDouble(ddlgoupLibrary.SelectedValue);
                if (chbisdisease.Checked)
                {
                    dictdiagnosis.Isdisease = "1";
                }
                else
                {
                    dictdiagnosis.Isdisease = "0";
                }
                Dictdiagnosis diagnosisOld = new Dictdiagnosis();
                if (dictdiagnosis.Dictdiagnosisid != null)
                {
                    diagnosisOld = (from Dictdiagnosis in diagnosisAll where Dictdiagnosis.Dictdiagnosisid == dictdiagnosis.Dictdiagnosisid select Dictdiagnosis).ToList<Dictdiagnosis>()[0];
                }
                double? b = diagnosisservice.SaveDiagnosis(dictdiagnosis, diagnosisOld);
                if (b > 0)
                {
                    ViewState["dictdiagnosisID"] = b;
                    MessageBoxShow("新增成功！");
                    BindGrid();
                    AddDictdiagnosis();//清空
                }
                else if (b == 0)
                {
                    MessageBoxShow("修改成功！");
                    BindGrid();
                    AddDictdiagnosis();//清空
                }
                else
                {
                    MessageBoxShow("操作报错！");
                }

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 删除
        //删除
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();
                Dictdiagnosis dictdiagnosis = new Dictdiagnosis();
                if (ViewState["dictdiagnosisID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["dictdiagnosisID"]);
                    dictdiagnosis = (from Dictdiagnosis in diagnosisAll where Dictdiagnosis.Dictdiagnosisid == id select Dictdiagnosis).ToList<Dictdiagnosis>()[0];

                    //List<Dictruleformular> ruleformularAll = loginservice.GetLoginDictruleformularresultList();
                    //List<Dictruleformular> ruleformularLst = (from Dictruleformular in ruleformularAll where Dictruleformular.Dictdiagnosisid == id select Dictruleformular).ToList<Dictruleformular>();
                    //if (ruleformularLst != null)
                    //{
                    //    if (ruleformularLst.Count != 0)
                    //    {
                    //        MessageBoxShow("规则公式中已存在改诊断字典，不能删除！！"); return;
                    //    }
                    //}
                }


                int result = diagnosisservice.DelDiagnosisByID(dictdiagnosis);//删除操作
                if (result > 0)
                {
                    AddDictdiagnosis();//清空
                    BindGrid();
                    MessageBoxShow("删除成功！");
                }
            }
            catch (Exception ex)
            {

                MessageBoxShow(ex.Message);
            }
        }
        #endregion

        #region 显示详细信息
        //显示详细信息
        protected void gdDiagnosis_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                int gridRowID = e.RowIndex;
                //int index = gdDiagnosis.PageIndex * gdDiagnosis.PageSize + gridRowID;
                object[] keys = gdDiagnosis.DataKeys[gridRowID];
                ShowDetail(Convert.ToDouble(keys[0]));

                gvSelectBinder();
            }
            catch (Exception)
            {

                MessageBoxShow("显示数据出错，请联系管理员！");
            }
        }

        private void ShowDetail(double? ID)
        {
            List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();//所有诊断字典
            Dictdiagnosis dictdiagnosis = new Dictdiagnosis();            
            dictdiagnosis = diagnosisAll.Find(c => c.Dictdiagnosisid == ID);
            ViewState["dictdiagnosisID"] = dictdiagnosis.Dictdiagnosisid;
            txtdiagnosisname.Text = dictdiagnosis.Diagnosisname;
            ddldiagnosistype.SelectedValue = dictdiagnosis.Diagnosistype;
            if (dictdiagnosis.Isdisease == "1")
            {
                chbisdisease.Checked = true;
            }
            else
            {
                chbisdisease.Checked = false;
            }
            txtdisplayorder.Text = dictdiagnosis.Displayorder.ToString();
            tadiseasedescription.Text = dictdiagnosis.Diseasedescription;
            tadiseasecause.Text = dictdiagnosis.Diseasecause;
            tasuggestion.Text = dictdiagnosis.Suggestion;
            taengdiseasedescription.Text = dictdiagnosis.Engdiseasedescription;
            taengdiseasecause.Text = dictdiagnosis.Engdiseasecause;
            taengsuggestion.Text = dictdiagnosis.Engsuggestion;
            SimpleFormEdit.Title = "当前状态-编辑";
            txtdiagnosiscode.Text = dictdiagnosis.Diagnosiscode;
            ddlgoupLibrary.SelectedValue = dictdiagnosis.Dictlabdeptid.ToString();
        }

        #endregion

        #region 导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdDiagnosis.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }
                Hashtable ht1 = new Hashtable();

                ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));

                List<Dictdiagnosis> newslist = diagnosisservice.GetDictdiagnosisLst(ht1);

                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("Diagnosisname", "诊断名称");

                ExcelOperation<Dictdiagnosis>.ExportListToExcel(newslist, sortlist, filename, sheetname);
            }
            catch (Exception)
            {

                MessageBoxShow("导出出错，请联系管理员！");
            }
        }
        #endregion

        #region 分页
        protected void gdDiagnosis_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdDiagnosis.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion

        #region >>>管理互斥异常建议
       
        // 执行清空动作
        protected void ttbMutexSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbMutexSearch.Text = "";
            ttbMutexSearch.ShowTrigger1 = false;
        }
        //搜索待添加互斥异常建议
        protected void ttbMutexSearch_Trigger2Click(object sender, EventArgs e)
        {
            if (gdDiagnosis.SelectedRowIndexArray.Length > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("strKey", ttbMutexSearch.Text.Trim());
                ht.Add("Dictdiagnosisid", gdDiagnosis.DataKeys[gdDiagnosis.SelectedRowIndexArray[0]][0].ToString());
                List<Dictdiagnosis> dictDiagnosisLst = diagnosisservice.SelectDictdiagnosisByNameLst(ht);
                gvNoSelect.DataSource = dictDiagnosisLst;
                gvNoSelect.DataBind();
            }            
        }

        //删除已添加的互斥建议
        protected void gvSelect_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ActionDel")
            {
                object[] rowsValue = gvSelect.DataKeys[gvSelect.SelectedRowIndexArray[0]];
                Dictdiagnosesmutex mutex = new Dictdiagnosesmutex();
                mutex.Dictdiagnosesmutexid = Convert.ToDouble(rowsValue[0]);
                mutex.Dictdiagnosisid = Convert.ToDouble(rowsValue[1]);
                mutex.Dictmutexdiagnosisid = Convert.ToDouble(rowsValue[2]);                
                mutex.Createdate = Convert.ToDateTime(rowsValue[3]);
                mutex.Diagnosisname = rowsValue[4].ToString();
                try
                {
                    dictdiagnosesmutexService.DelDictdiagnosesmute(mutex);

                    gvSelectBinder();
                }
                catch (Exception ex)
                {
                    MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                }
            }
        }

        //添加互斥建议
        protected void gvNoSelect_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ActionAdd")
            {

                double? dictdiagnosisid = 0;
                double? dictmutexdiagnosisid = 0;
                object[] rowsValue = null;
                if (gdDiagnosis.SelectedRowIndexArray.Length > 0)
                {
                    dictdiagnosisid = Convert.ToDouble(gdDiagnosis.DataKeys[gdDiagnosis.SelectedRowIndexArray[0]][0]);

                    rowsValue = gvNoSelect.DataKeys[gvNoSelect.SelectedRowIndexArray[0]];
                    dictmutexdiagnosisid = Convert.ToDouble(rowsValue[0]);
                }
                else
                {
                    MessageBoxShow("请选择左边列表中选择要维护的异常建议", MessageBoxIcon.Information);
                    return;
                }

                Hashtable htPara = new Hashtable();
                htPara.Add("dictdiagnosisid", dictdiagnosisid);
                htPara.Add("dictmutexdiagnosisid", dictmutexdiagnosisid);
                if (!dictdiagnosesmutexService.SelectIsHaveMutexd(htPara))
                {
                    MessageBoxShow("已包含该项,不能重复添加", MessageBoxIcon.Information);
                    return;
                }
                Dictdiagnosesmutex mutex = new Dictdiagnosesmutex();
                mutex.Dictdiagnosisid = dictdiagnosisid;
                mutex.Dictmutexdiagnosisid = dictmutexdiagnosisid;
                mutex.Diagnosisname = rowsValue[1].ToString();
                try
                {
                    dictdiagnosesmutexService.AddDictdiagnosesmutex(mutex);

                    gvSelectBinder();
                }
                catch (Exception ex)
                {
                    MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>互斥诊断建议绑定
        /// 
        /// </summary>
        private void gvSelectBinder()
        {
            if (gdDiagnosis.SelectedRowIndexArray.Length > 0)
            {
                object[] objValue = gdDiagnosis.DataKeys[gdDiagnosis.SelectedRowIndexArray[0]];
                gvSelect.DataSource = dictdiagnosesmutexService.SelectDictdiagnosesmutexLst(objValue[0].ToString());
            }
            else
            {
                gvSelect.DataSource = null;
            }
            gvSelect.DataBind();
        }

        #endregion
    }
}