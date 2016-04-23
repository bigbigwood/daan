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
using daan.web.code;
using daan.util.Common;
using daan.util.Web;

namespace daan.web.admin.dict
{
    public partial class DictTestItems : PageBase
    { 
        #region loading
        DictLibraryItemService labraryItemService = new DictLibraryItemService();//基础字典
        DicttestitemService testitemservice = new DicttestitemService(); //项目字典
        LoginService loginService = new LoginService();
        DicttestitemresultService testresultservice = new DicttestitemresultService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                btnDel.OnClientClick = gdTestItem.GetNoSelectionAlertReference("请选择需要删除的项目！", "提示");
                btnDel.ConfirmText = "确实要删除该项目？";

                btnDelAll.OnClientClick = gdTestItemResult.GetNoSelectionAlertReference("请选择需要删除的项目结果！", "提示");
                btnDelAll.ConfirmText = "确实要删除选中项目结果？";

                BindLabDept();//物理组

                BandReportTemplate();//报告模板
                DDLLabraryItemBinder(ddlTubeGroup, "TUBEGROUPTYPE");//分管原则
                DDLLabraryItemBinder(ddlContainerType, "CONTAINERTYPE");//试管类型                
                DDLLabraryItemBinder(ddlSpecimenType, "SAMPLETYPE");//标本类型 

                DDLInitbasicBinder(ddlResultType, "RESULTTYPE");//结果类型
                DDLInitbasicBinder(ddlRefmethod, "REFMETHOD");//参考值方式 
                DDLInitbasicBinder(ddlforsex, "REFSEX");//测试项性别

                ddlTubeGroup.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                ddlContainerType.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                ddlSpecimenType.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                ddlResultType.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                ddlRefmethod.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                ddlforsex.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
            else
            {
                if (ViewState["resultstatus"] != null)
                {
                    string str = ViewState["resultstatus"].ToString();
                    if ("Add".Equals(str))
                    {
                        double? id = Convert.ToDouble(ViewState["TestItemID"]);
                        //添加项目结果时刷新局部
                        BindTestResult(id);
                    }
                }


            }
        }
        #endregion

        #region >>>项目及分页
        private void BindGrid(string testtype, string dictlabid)
        {
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdTestItem.PageIndex, gdTestItem.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("testtype", testtype);
            ht1.Add("Dictlabdeptid", dictlabid);
            ht1.Add("IsActive", chkActive.Checked ? "1" : "0");

            //strtestitemid
            //设置总项数
            gdTestItem.RecordCount = testitemservice.GetDictTestItemPageLstCount(ht1);
            List<Dicttestitem> list = testitemservice.GetDictTestItemPageLst(ht1);
            gdTestItem.DataSource = list;
            gdTestItem.DataBind();
        }

        #endregion

        #region 绑定缓存数据
        //public List<Dicttestitem> BindTestItemAll()
        //{
        //    List<Dicttestitem> testitemListAll = loginService.GetLoginDicttestitemList();//所有项目
        //    List<Dicttestitem> testitemList = (from Dicttestitem in testitemListAll where Dicttestitem.Testtype == "0" select Dicttestitem).ToList<Dicttestitem>();//所有单项，0，单项
        //    return testitemList;
        //}
        //public List<Dicttestitemresult> BindTestItemResultAll()
        //{
        //    //所有项目结果
        //    List<Dicttestitemresult> testitemresultListAll = loginService.GetLoginDicttestitemresultList();
        //    return testitemresultListAll;
        //}
        #endregion

        #region 绑定物理组
        public void BindLabDept()
        {
            DictlabdeptService service = new DictlabdeptService();
            ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            List<Dictlabdept> listlabdept = loginService.GetLoginDictlabdeptList();
            foreach (Dictlabdept lab in listlabdept)
            {
                ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
                ddlPhysicalGourp.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
            }
        }
        #endregion

        #region 基础字典

        #region 绑定报告模板
        //绑定报告模板
        private void BandReportTemplate()
        {
            DictreporttemplateService service = new DictreporttemplateService();
            List<Dictreporttemplate> list = service.GetDictreporttemplateAll();
            foreach (Dictreporttemplate reporttemplate in list)
            {
                ddlReportTemplate.Items.Add(new ExtAspNet.ListItem(reporttemplate.Templatename, reporttemplate.Dictreporttemplateid.ToString()));
            }
            ddlReportTemplate.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion
        #endregion

        #region 物理组下的项目

        protected void ddlgoupLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearchData();

        }

        private void BindSearchData()
        {
            if (ddlgoupLibrary.SelectedValue == "-1")
            {
                BindGrid("0", null);
            }
            else
            {
                BindGrid("0", ddlgoupLibrary.SelectedValue);
            }
        }
        #endregion

        #region 绑定项目详细信息
        protected void gdTestItem_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (gdTestItem.SelectedRowIndexArray.Length > 0)
                {
                    Dicttestitem item = new Dicttestitem();
                    ViewState["TestItemID"] = null;
                    if (e.RowIndex >= 0)
                    {
                        int gridRowID = e.RowIndex;
                        //int index = gdTestItem.PageIndex * gdTestItem.PageSize + gridRowID;
                        string keys = gdTestItem.Rows[gridRowID].Values[0].ToString();
                        //object[] keys = gdTestItem.DataKeys[e.RowIndex];
                        //List<Dicttestitem> testitemList = BindTestItemAll();//所有单项的集合  
                        //根据选中的行得到当前选中的实例
                        //item = (from Dicttestitem in testitemList where Dicttestitem.Dicttestitemid == Convert.ToInt32(keys) select Dicttestitem).ToList<Dicttestitem>()[0];
                        item = testitemservice.SelectDicttestitemByDicttestitemid(Convert.ToDouble(keys));
                        ViewState["TestItemID"] = item.Dicttestitemid;
                        //SimpleFormEdit.Title = "当前状态-编辑";
                        txtUniQueid.Text = item.Uniqueid;
                        txtEngName.Text = item.Engname;
                        txtEngLongName.Text = item.Englongname;
                        txtItemCode.Text = item.Testcode;
                        txtTestName.Text = item.Testname;
                        txtUnit.Text = item.Unit;
                        txtFastCode.Text = item.Fastcode;
                        ddlContainerType.SelectedValue = (item.Dictcontainerid ?? -1).ToString();//试管类型
                        ddlPhysicalGourp.SelectedValue = (item.Dictlabdeptid ?? -1).ToString();//物理组
                        ddlReportTemplate.SelectedValue = (item.Dictreporttemplateid ?? -1).ToString();//报告模板
                        ddlTubeGroup.SelectedValue = item.Tubegroup ?? "-1";//分管原则
                        ddlforsex.SelectedValue = item.Forsex ?? "-1";
                        ddlSpecimenType.SelectedValue = (item.Dictspecimentypeid ?? -1).ToString();//标本类型
                        txtPrecision.Text = item.Precision.ToString();//小数位

                        ddlResultType.SelectedValue = item.Resulttype ?? "-1";//结果类型
                        txtDefaultResult.Text = item.Defaultresult;//默认结果
                        //txtRefmethod.Text = item.Refmethod;//参考值方式
                        ddlRefmethod.SelectedValue = item.Refmethod ?? "-1";
                        txtLimitHight.Text = item.Limithigh.ToString();//限制高
                        txtLimiLow.Text = item.Limitlow.ToString();//限制低
                        txtLabelNumber.Text = item.Labelnumber.ToString();
                        txtPrintOrder.Text = item.Displayorder.ToString();//打印排序
                        txtStandardPrice.Text = item.Price.ToString();//价格
                        txtOperationRemark.Text = item.Operationremark;//操作指引说明

                        if (item.Report == "1") { chbReport.Checked = true; } //1,打印报告
                        else chbReport.Checked = false;

                        if (item.Active == "1") { chbActive.Checked = true; } //1,可用
                        else chbActive.Checked = false;

                        if (item.Billable == "1") { chbBillable.Checked = true; } //1,需要记账
                        else chbBillable.Checked = false;

                        if (item.Isimportant == "1") { chbImportanttest.Checked = true; } //1,是重要项目
                        else chbImportanttest.Checked = false;

                        if (item.Isonlyforbill == "1") { chbImportanttest.Checked = true; } //是否只是收费项目  0 不是 1 是
                        else chbImportanttest.Checked = false;

                        //项目可选结果
                        BindTestResult(item.Dicttestitemid);
                    }

                }
                else
                {
                    AddTestItem();
                }
            }
            catch (Exception)
            {
                MessageBoxShow("显示数据出错，请联系管理员！");
            }

        }

        private void BindTestResult(double? testitemid)
        {
            if (testitemid != null)
            {
                List<Dicttestitemresult> list = testresultservice.GetDicttestitemResult(testitemid);
                //List<Dicttestitemresult> testitemresultListAll = BindTestItemResultAll();
                ////根据当前的项目单项查找该项目下的所有结果并绑定到Grid控件
                //IList list = (from Dicttestitemresult in testitemresultListAll where Dicttestitemresult.Dicttestitemid == testitemid select Dicttestitemresult).ToList<Dicttestitemresult>();
                gdTestItemResult.DataSource = list;
                gdTestItemResult.DataBind();
            }

        }
        #endregion

        #region 模糊查询
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ViewState["resultstatus"] = null;
            gdTestItem.PageIndex = 0;
            BindSearchData();


        }

        //private void BindSearchData()
        //{
        //    List<Dicttestitem> testitemLst = GetTestItemByC();
        //    gdTestItem.DataSource = testitemLst;
        //    gdTestItem.DataBind();
        //}

        //public List<Dicttestitem> GetTestItemByC()
        //{
        //    List<Dicttestitem> testitemList = BindTestItemAll();//所有单项的集合
        //    List<Dicttestitem> testitemLst = new List<Dicttestitem>();
        //    //按物理组类型显示 没有选择时则显示全部
        //    double? dictlabdeptid = Convert.ToDouble(ddlgoupLibrary.SelectedValue);
        //    if (dictlabdeptid == -1)
        //    {
        //        testitemLst= testitemList;
        //    }
        //    else
        //    {
        //        //按条件查找出来的单项的集合
        //        testitemLst = (from Dicttestitem in testitemList where Dicttestitem.Dictlabdeptid == dictlabdeptid  select Dicttestitem).ToList<Dicttestitem>();

        //    }

        //    string testitemStr = ttbSearch.Text;
        //    List<Dicttestitem> newslist = new List<Dicttestitem>();

        //    foreach (Dicttestitem item in testitemLst)
        //    {
        //        if ((item.Fastcode != null && item.Fastcode.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Engname != null && item.Engname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Testname != null && item.Testname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Testcode != null && item.Testcode.ToLower().Contains(testitemStr.ToLower()))
        //            || testitemStr == "")
        //        {
        //            newslist.Add(item);
        //        }
        //    }
        //    return newslist;
        //}
        #endregion

        #region 新增
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtUniQueid.Focus();
            ViewState["resultstatus"] = null;
            //新增 初始化控件
            AddTestItem();
        }
        //新增
        private void AddTestItem()
        {
            gdTestItem.SelectedRowIndexArray = new int[] { };
            //item = new Dicttestitem();
            ViewState["TestItemID"] = null;
            //SimpleFormEdit.Title = "当前状态-新增";
            txtUniQueid.Text = null;
            txtEngName.Text = null;
            txtEngLongName.Text = null;
            txtItemCode.Text = null;
            txtTestName.Text = null;
            txtUnit.Text = null;
            txtPrintOrder.Text = null;
            txtFastCode.Text = null;
            ddlContainerType.SelectedIndex = ddlContainerType.Items.Count() > 1 ? 1 : 0;
            ddlforsex.SelectedIndex = ddlforsex.Items.Count() > 1 ? 1 : 0;
            //ddlSpecimenType.SelectedIndex = ddlSpecimenType.Items.Count() > 1 ? 1 : 0;

            //ddlSpecimenType.SelectedValue = "-1";

            //ddlPhysicalGourp.SelectedIndex = ddlPhysicalGourp.Items.Count() > 1 ? 1 : 0;
             //ddlTubeGroup.SelectedIndex = ddlTubeGroup.Items.Count() > 1 ? 1 : 0;
            //ddlTubeGroup.SelectedValue = "-1";
            //ddlTubeGroup.SelectIndex = 0;

            txtPrecision.Text = null;
            txtStandardPrice.Text = null;
            ddlResultType.SelectedIndex = ddlResultType.Items.Count() > 1 ? 1 : 0;
            txtDefaultResult.Text = null;
            //txtRefmethod.Text = null;
            ddlRefmethod.SelectedIndex = ddlRefmethod.Items.Count() > 1 ? 1 : 0;
            txtLimitHight.Text = null;
            txtLimiLow.Text = null;
            txtLabelNumber.Text = null;
            txtPrintOrder.Text = null;
            txtOperationRemark.Text = null;

            chbActive.Checked = true;
            chbBillable.Checked = true;

            chbImportanttest.Checked = false;
            chbReport.Checked = true;

            List<Dicttestitemresult> list = new List<Dicttestitemresult>();
            gdTestItemResult.DataSource = list;
            gdTestItemResult.DataBind();
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dicttestitem item = new Dicttestitem();
                Dicttestitem itemtest = new Dicttestitem();//修改前的信息 用作日志
                //List<Dicttestitem> testitemList = BindTestItemAll();//所有单项的集合  
                List<Dicttestitem> dicttestitemList = new List<Dicttestitem>();
               
                if (txtUniQueid.Text.Trim() == "")
                {
                    MessageBoxShow("全国统一码不能为空!"); return;
                }
                if (txtEngName.Text.Trim() == "")
                {
                    MessageBoxShow("英文缩写不能为空!"); return;
                }
                if (txtItemCode.Text.Trim() == "")
                {
                    MessageBoxShow("项目代码不能为空!"); return;
                }
                if (txtEngLongName.Text.Trim() == "")
                {
                    MessageBoxShow("英文全称不能为空!"); return;
                }
                if (txtTestName.Text.Trim() == "")
                {
                    MessageBoxShow("项目名称不能为空!"); return;
                }
                if (txtPrintOrder.Text.Trim() == "")
                {
                    MessageBoxShow("打印次序不能为空!"); return;
                }
                if (txtFastCode.Text.Trim() == "")
                {
                    MessageBoxShow("助记符不能为空!"); return;
                }
                if (txtLabelNumber.Text.Trim() == "")
                {
                    MessageBoxShow("标签份数不能为空!"); return;
                }
                if (txtStandardPrice.Text.Trim() == "")
                {
                    MessageBoxShow("标准价格不能为空!"); return;
                }
                if (ddlforsex.SelectedValue == "-1")
                {
                    MessageBoxShow("测试项性别不能为空!"); return;
                }
                if (ddlforsex.SelectedValue == "-1")
                {
                    MessageBoxShow("测试项性别不能为空!"); return;
                }
                if (ddlResultType.SelectedValue == "-1")
                {
                    MessageBoxShow("结果类型不能为空!"); return;
                }
                if (ddlReportTemplate.SelectedValue == "-1")
                {
                    MessageBoxShow("报告模板不能为空!"); return;
                }

                if (ViewState["TestItemID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["TestItemID"]);
                    //item = (from Dicttestitem in testitemList where Dicttestitem.Dicttestitemid == id select Dicttestitem).ToList<Dicttestitem>()[0];
                    item = testitemservice.SelectDicttestitemByDicttestitemid(id);
                    itemtest = item;
                    Hashtable ht = new Hashtable();
                    ht.Add("Uniqueid", txtUniQueid.Text.Trim());
                    ht.Add("Dicttestitemid", id);
                    dicttestitemList = new DicttestitemService().GetDicttestitemByCode(ht);
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("Uniqueid", txtUniQueid.Text.Trim());
                    dicttestitemList = new DicttestitemService().GetDicttestitemByCode(ht);
                }

                if (dicttestitemList.Count > 0)
                {
                    MessageBoxShow("已存在相同的全国统一码!"); return;
                }

                item.Uniqueid = txtUniQueid.Text.Trim();
                item.Engname = txtEngName.Text.Trim();
                item.Testcode = txtItemCode.Text.Trim();
                item.Englongname = txtEngLongName.Text.Trim();
                item.Testname = txtTestName.Text.Trim();
                item.Unit = txtUnit.Text.Trim();
                item.Fastcode = txtFastCode.Text.Trim();
                item.Dictcontainerid = Convert.ToDouble(ddlContainerType.SelectedValue);//试管类型

                item.Dictreporttemplateid = Convert.ToDouble(ddlReportTemplate.SelectedValue);//报告模板
                item.Dictspecimentypeid = Convert.ToDouble(ddlSpecimenType.SelectedValue);//标本类型
                item.Dictlabdeptid = Convert.ToDouble(ddlPhysicalGourp.SelectedValue);//物理组
                item.Tubegroup = ddlTubeGroup.SelectedValue.ToString();//分管原则
                item.Precision = txtPrecision.Text.Trim() == "" ? 0 : Convert.ToDouble(txtPrecision.Text.Trim());

                item.Forsex = ddlforsex.SelectedValue;
                item.Resulttype = ddlResultType.SelectedValue;
                item.Defaultresult = txtDefaultResult.Text.Trim();
                item.Refmethod = ddlRefmethod.SelectedValue;
                item.Price = Convert.ToDouble(txtStandardPrice.Text.Trim());//价格
                item.Limithigh = txtLimitHight.Text == "" ? 0 : Convert.ToDouble(txtLimitHight.Text.Trim());
                item.Limitlow = txtLimiLow.Text == "" ? 0 : Convert.ToDouble(txtLimiLow.Text.Trim());
                item.Labelnumber = txtLabelNumber.Text == "" ? 0 : Convert.ToDouble(txtLabelNumber.Text.Trim());
                item.Displayorder = txtPrintOrder.Text == "" ? 0 : Convert.ToDouble(txtPrintOrder.Text.Trim());//打印排序
                item.Testtype = "0";//单项

                item.Operationremark = txtOperationRemark.Text.Trim();//操作指引说明
                if (chbActive.Checked) { item.Active = "1"; }
                else item.Active = "0";

                if (chbBillable.Checked) { item.Billable = "1"; }
                else item.Billable = "0";

                if (chbReport.Checked) { item.Report = "1"; }
                else item.Report = "0";

                if (chbImportanttest.Checked) { item.Isimportant = "1"; }
                else item.Isimportant = "0";

                if (chbIsonlyForBill.Checked) { item.Isonlyforbill = "1"; }
                else item.Isonlyforbill = "0";


                //if (item.Dicttestitemid != null)
                //{
                //    itemtest = (from Dicttestitem in testitemList where Dicttestitem.Dicttestitemid == item.Dicttestitemid select Dicttestitem).ToList<Dicttestitem>()[0];
                //}
                
                double? f = testitemservice.SaveDictTestItem(item, itemtest);//新增、修改操作
                if (f > 0)
                {
                    item.Dicttestitemid = f;
                    ViewState["TestItemID"] = f;
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                    MessageBoxShow("新增项目成功！此项目在康源系统未做对照，需做完项目对照才可开单，请及时对照！");
                    BindSearchData();
                    //项目可选结果
                    //BindTestResult(item.Dicttestitemid);
                   // AddTestItem();//清空
                }
                else if (f == 0)
                {
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                    Hashtable htPara = new Hashtable();
                    htPara.Add("olduniquecode", txtUniQueid.Text.Trim());
                    //htPara.Add("testname", txtTestName.Text.Trim());
                    if (new daan.service.ProjectControlService().GetProjectControlCountByUniquecode(htPara))
                    {
                        MessageBoxShow("修改成功！");
                    }
                    else
                    {
                        MessageBoxShow("修改成功！修改后的项目在康源系统未做对照，需做完项目对照才可开单，请及时对照！");
                    }
                    BindSearchData();
                    //项目可选结果
                    //BindTestResult(item.Dicttestitemid);
                   // AddTestItem();//清空
                }
                else
                {
                    MessageBoxShow("操作出错！");
                }

            }
            catch (Exception ex)
            {
                MessageBoxShow(string.Format("保存数据出错,错误原因：{0}",ex.Message));
            }
        }
        #endregion

        #region 删除
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Dicttestitem item = new Dicttestitem();
                //List<Dicttestitem> testitemList = BindTestItemAll();//所有单项的集合
                if (ViewState["TestItemID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["TestItemID"]);
                    //item = (from Dicttestitem in testitemList where Dicttestitem.Dicttestitemid == id select Dicttestitem).ToList<Dicttestitem>()[0];
                    item = testitemservice.SelectDicttestitemByDicttestitemid(id);
                }
                bool b = testitemservice.DelDicttestitemByID(item);//删除操作
                if (b)
                {
                    MessageBoxShow("删除成功！");
                    //CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                    //CacheHelper.RemoveAllCache("daan.GetDicttestitemresult");
                    AddTestItem();//清空
                    BindSearchData();
                    //项目可选结果
                    //BindTestResult(item.Dicttestitemid);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }


        }
        #endregion

        #region 导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdTestItem.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("testtype", 0);

                if (ddlgoupLibrary.SelectedValue == "-1")
                {
                    ht.Add("Dictlabdeptid", null);
                }
                else
                {
                    ht.Add("Dictlabdeptid", ddlgoupLibrary.SelectedValue);
                }
                ht.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
                ht.Add("IsActive", chkActive.Checked ? "1" : "0");
                List<Dicttestitem> testitemLst = testitemservice.GetSearchData(ht);

                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());//集合中排序
                sortlist.Add("Uniqueid", "全国统一码");
                sortlist.Add("Testcode", "项目代码");
                sortlist.Add("Engname", "英文名");
                sortlist.Add("Testname", "项目名称");
                sortlist.Add("Fastcode", "助记符");
                ExcelOperation<Dicttestitem>.ExportListToExcel(testitemLst, sortlist, filename, sheetname);
            }
            catch (Exception)
            {
                MessageBoxShow("导出数据出错，请联系管理员！");
            }
        }
        #endregion

        #region 删除项目结果
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            try
            {
                double? id = null;
                if (ViewState["TestItemID"] != null)
                {
                    id = Convert.ToDouble(ViewState["TestItemID"]);
                }
                List<Dicttestitemresult> itemresultLst = HowManyRowsAreSelected(gdTestItemResult);

                bool b = testresultservice.DelDicttestitemResultByIdStr(itemresultLst);
                if (b)
                {
                    //CacheHelper.RemoveAllCache("daan.GetDicttestitemresult");
                    BindTestResult(id);
                    MessageBoxShow("所选项已成功删除！");
                }
            }
            catch (Exception)
            {

                MessageBoxShow("删除失败，请与管理员联系！");
            }
        }
        #endregion

        #region 得到选中行的信息
        private List<Dicttestitemresult> HowManyRowsAreSelected(Grid g)
        {
            double? d = null;
            //List<Dicttestitemresult> list = testresultservice.GetDicttestitemResult(testitemid);
            if (ViewState["TestItemID"] != null)
            {
                d = Convert.ToDouble(ViewState["TestItemID"]);
            }
            else
            {

            }
            List<Dicttestitemresult> testitemresultListAll = testresultservice.GetDicttestitemResult(d);
            int selectedCount = g.SelectedRowIndexArray.Length;
            if (selectedCount == 0)
                return null;
            else
            {
                List<Dicttestitemresult> itemresultList = new List<Dicttestitemresult>();
                for (int i = 0; i < selectedCount; i++)
                {
                    int rowIndex = g.SelectedRowIndexArray[i];
                    if (g.AllowPaging && !g.IsDatabasePaging)
                    {
                        rowIndex = g.PageIndex * g.PageSize + rowIndex;
                    }
                    object[] keystr = g.DataKeys[rowIndex];

                    itemresultList.Add((from Dicttestitemresult in testitemresultListAll where Dicttestitemresult.Dicttestitemresultid == Convert.ToInt32(keystr[0]) select Dicttestitemresult).ToList<Dicttestitemresult>()[0]);
                }
                return itemresultList;
            }
        }
        #endregion

        #region 添加项目结果
        protected void btnAddTestResult_Click(object sender, EventArgs e)
        {
            double? testitemid = null;
            if (ViewState["TestItemID"] != null)
            {
                testitemid = Convert.ToDouble(ViewState["TestItemID"]);
            }
            ViewState["resultstatus"] = "Add";
            if (testitemid == null)
            {
                MessageBoxShow("请选择项目！");
                return;
            }
            //this.WinTestResultAdd.Hidden = false;
            //this.WinTestResultAdd.Title = "添加";
            //this.WinTestResultAdd.IFrameUrl = "DictTestResult_Window.aspx?testitemid=" + testitemid;
            PageContext.RegisterStartupScript(WinTestResultAdd.GetShowReference("DictTestResult_Window.aspx?testitemid=" + testitemid, "添加"));
        }
        #endregion

        #region 项目结果诊断
        public string GetDiagnosis(Object str)
        {
            if ("0".Equals(str.ToString()))
            {
                return "正常";
            }
            else
            {
                return "异常";
            }
        }
        #endregion

        #region 分页
        protected void gdTestItem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdTestItem.PageIndex = e.NewPageIndex;
            BindSearchData();
        }

        protected void gdTestItemResult_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdTestItemResult.PageIndex = e.NewPageIndex;
        }
        #endregion

    }
}