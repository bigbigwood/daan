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

namespace daan.web.admin
{
    public partial class DictTestGroup : PageBase
    {
        #region Loading
        LoginService loginservice = new LoginService();
        DicttestitemService testitemservice = new DicttestitemService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDel.OnClientClick = gdGroupTestItem.GetNoSelectionAlertReference("请选择需要删除的组合！", "提示");
                btnDel.ConfirmText = "确实要删除该组合？";
                BindLabDept();
                BandSpecimenType();
            }
        }
        #endregion

        #region >>>组合及分页
        private void BindGrid(string testtype, string dictlabid)
        {

            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdGroupTestItem.PageIndex, gdGroupTestItem.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("testtype", testtype);
            ht1.Add("Dictlabdeptid", dictlabid);
            ht1.Add("IsActive", chkActive.Checked ? "1" : "0");

            //strtestitemid
            //设置总项数
            gdGroupTestItem.RecordCount = testitemservice.GetDictTestItemPageLstCount(ht1);
            List<Dicttestitem> list = testitemservice.GetDictTestItemPageLst(ht1);
            gdGroupTestItem.DataSource = list;
            gdGroupTestItem.DataBind();
        }

        #endregion

        #region 绑定缓存数据

        //public List<Dicttestitem> BindTestGourp()
        //{
        //    List<Dicttestitem> testitemListAll = loginservice.GetLoginDicttestitemList();
        //    //绑定组合
        //    List<Dicttestitem> testitemgoutpLst = (from Dicttestitem in testitemListAll where Dicttestitem.Testtype == "1" select Dicttestitem).ToList<Dicttestitem>();
        //    return testitemgoutpLst;
        //}

        //public List<Dicttestitem> BindTestItem()
        //{
        //    List<Dicttestitem> testitemListAll = loginservice.GetLoginDicttestitemList();
        //    //绑定所有单项
        //    List<Dicttestitem> testitemLst = (from Dicttestitem in testitemListAll where Dicttestitem.Testtype == "0" select Dicttestitem).ToList<Dicttestitem>();
        //    return testitemLst;
        //}

        ////组合明细
        //public List<Dicttestgroupdetail> BindtestgroupdetailAll()
        //{
        //    List<Dicttestgroupdetail> testgroupdetailLisAll = loginservice.GetLoginDicttestgroupdetail();
        //    return testgroupdetailLisAll;
        //}
        #endregion

        #region 绑定物理组
        public void BindLabDept()
        {
            DictlabdeptService service = new DictlabdeptService();
            //ddlPhysicalGourp.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            List<Dictlabdept> listlabdept = loginservice.GetLoginDictlabdeptList();
            foreach (Dictlabdept lab in listlabdept)
            {
                ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
                ddlPhysicalGourp.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
            }

            ddlPhysicalGourp.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            DDLLabraryItemBinder(ddlTubeGroup, "TUBEGROUPTYPE");//分管原则
            ddlTubeGroup.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion

        #region 绑定标本类型
        //绑定标本类型
        private void BandSpecimenType()
        {
            DDLLabraryItemBinder(ddlSpecimenType, "SAMPLETYPE");//标本类型 
            ddlSpecimenType.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        #endregion

        #region 物理组下的组合项目

        protected void ddlgoupLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearchData();
        }
        #endregion

        #region 组合查询
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            gdGroupTestItem.PageIndex = 0;
            BindSearchData();
        }

        private void BindSearchData()
        {
            if (ddlgoupLibrary.SelectedValue != "-1")
            {
                BindGrid("1", ddlgoupLibrary.SelectedValue);
            }
            else
            {
                BindGrid("1", null);
            }
        }

        //private void BindSearchData()
        //{
        //    List<Dicttestitem> testitemLst = GetGroupTestItemByC();
        //    gdGroupTestItem.DataSource = testitemLst;
        //    gdGroupTestItem.DataBind();
        //}

        //public List<Dicttestitem> GetGroupTestItemByC()
        //{
        //    List<Dicttestitem> testitemgoutpLst = BindTestGourp();//所有组合项目
        //    List<Dicttestitem> testitemLst = new List<Dicttestitem>();
        //    double? Dictlabdeptid = Convert.ToDouble(ddlgoupLibrary.SelectedValue);
        //    if (Dictlabdeptid == -1)
        //    {
        //        testitemLst = testitemgoutpLst;
        //    }
        //    else
        //    {
        //        testitemLst = (from Dicttestitem in testitemgoutpLst where Dicttestitem.Dictlabdeptid == Dictlabdeptid select Dicttestitem).ToList<Dicttestitem>();
        //    }
        //    string testitemStr = ttbSearch.Text;
        //    List<Dicttestitem> newslist = new List<Dicttestitem>();
        //    foreach (Dicttestitem item in testitemLst)
        //    {
        //        if ((item.Fastcode != null && item.Fastcode.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Engname != null && item.Engname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Testname != null && item.Testname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Testcode != null && item.Testcode.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Uniqueid != null && item.Uniqueid.ToLower().Contains(testitemStr.ToLower())) ||
        //            (item.Englongname != null && item.Englongname.ToLower().Contains(testitemStr.ToLower()))
        //            || testitemStr == "")
        //        {
        //            newslist.Add(item);
        //        }
        //    }
        //    return newslist;
        //}
        #endregion

        #region 保存组合信息
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPhysicalGourp.SelectedValue == "-1")
                {
                    MessageBoxShow("请选择物理组!"); return;
                }
                if (ddlSpecimenType.SelectedValue == "-1")
                {
                    MessageBoxShow("请选择标本类型!"); return;
                }
                if (ddlTubeGroup.SelectedValue == "-1")
                {
                    MessageBoxShow("请选择分管原则!"); return;
                }
                if (txtGroupTestCode.Text.Trim() == "")
                {
                    MessageBoxShow("请填写组合代码名称"); return;
                }
                if (txtGroupTestName.Text.Trim() == "")
                {
                    MessageBoxShow("请填写组合名称"); return;
                }
                if (txtFastCode.Text.Trim() == "")
                {
                    MessageBoxShow("请填写助记符名称"); return;
                }
                if (nbbStandardPrice.Text.Trim() == "")
                {
                    MessageBoxShow("请填写标准价格"); return;
                }
                if (txtUniQueid.Text.Trim() == "")
                {
                    MessageBoxShow("请填写全国统一码"); return;
                }
                Dicttestitem item = new Dicttestitem();
                Dicttestitem testitem = new Dicttestitem();//日志操作，旧值
                List<Dicttestitem> dicttestitemList = new List<Dicttestitem>();
                //List<Dicttestitem> testitemgoutpLst = BindTestGourp();//所有组合项目
                if (ViewState["TestItemID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["TestItemID"]);
                    //item = (from Dicttestitem in testitemgoutpLst where Dicttestitem.Dicttestitemid == id select Dicttestitem).ToList<Dicttestitem>()[0];
                    item = testitemservice.SelectDicttestitemByDicttestitemid(id);
                    testitem = item;
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
                item.Fastcode = txtFastCode.Text.Trim();
                item.Testcode = txtGroupTestCode.Text.Trim();
                item.Testname = txtGroupTestName.Text.Trim();
                item.Operationremark = txtOperationRemark.Text.Trim();
                item.Price = Convert.ToDouble(nbbStandardPrice.Text.Trim());
                item.Uniqueid = txtUniQueid.Text.Trim();
                item.Dictlabdeptid = Convert.ToDouble(ddlPhysicalGourp.SelectedValue);
                item.Dictspecimentypeid = Convert.ToDouble(ddlSpecimenType.SelectedValue);
                item.Testtype = "1";//1，组合
                item.Tubegroup = ddlTubeGroup.SelectedValue;//分管原则
                item.Displayorder = int.Parse(txtDisplayOrder.Text);
                if (chbActive.Checked)
                {
                    item.Active = "1";
                }
                else
                {
                    item.Active = "0";
                }
                if (chbIsonlyForBill.Checked)
                {
                    item.Isonlyforbill = "1";
                }
                else
                {
                    item.Isonlyforbill = "0";
                }
                //if (item.Dicttestitemid != null)
                //{
                //    testitem = (from Dicttestitem in testitemgoutpLst where Dicttestitem.Dicttestitemid == item.Dicttestitemid select Dicttestitem).ToList<Dicttestitem>()[0];
                //}
                List<Dicttestitem> listIn = GridlistIn();//包含的项目
                int num = 0;
                int flag = 0;
                StringBuilder type = new StringBuilder();
                for (int i = 0; i < listIn.Count(); i++)
                {
                    if (listIn[i].Tubegroup != "" && listIn[i].Dictlabdeptid.ToString() != "" && listIn[i].Dictspecimentypeid.ToString() != "")
                    {
                        //if (this.ddlTubeGroup.SelectedValue != listIn[i].Tubegroup || this.ddlPhysicalGourp.SelectedValue != listIn[i].Dictlabdeptid.ToString() || this.ddlSpecimenType.SelectedValue != listIn[i].Dictspecimentypeid.ToString())
                        if (ddlTubeGroup.SelectedValue != listIn[i].Tubegroup || ddlSpecimenType.SelectedValue != listIn[i].Dictspecimentypeid.ToString())
                        {
                            num++;
                            type.Append(listIn[i].Testname + ",");
                        }
                    }
                    else
                    {
                        flag++;
                        type.Append(listIn[i].Testname + ",");
                    }
                }
                if (flag > 0)
                {
                    MessageBoxShow(String.Format("{0}分管原则、所属科室、标本类型都不能为空，请重新维护！", type));
                    return;
                }
                if (num > 0)
                {
                    //MessageBoxShow(String.Format("{0}必须与所选分管原则、所属科室、标本类型相同！", type));
                    MessageBoxShow(String.Format("{0}必须与所选分管原则、标本类型相同！", type));
                    return ;
                }
                string strerr = string.Empty;
                double? f = testitemservice.SaveDictTestItem(item, listIn, testitem, ref strerr);
                if (f > 0)
                {

                    item.Dicttestitemid = f;
                    ViewState["TestItemID"] = f;
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");//删除项目缓存
                    CacheHelper.RemoveAllCache("daan.GetDicttestgroupdetail");//删除项目结果缓存
                    BindSearchData();//筛选下的项目
                  //  AddTestGroup();//清空
                    MessageBoxShow("新增成功！");

                }
                else if (f == 0)
                {
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");//删除项目缓存
                    CacheHelper.RemoveAllCache("daan.GetDicttestgroupdetail");//删除项目结果缓存
                    BindSearchData();//筛选下的项目
                   // AddTestGroup();//清空
                    MessageBoxShow("修改成功！");
                }
                else
                {
                    MessageBoxShow("操作出错:"+strerr);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.ToString());
            }

        }
        #endregion

        #region 删除组合项目
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Dicttestitem item = new Dicttestitem();
                //List<Dicttestitem> testitemgoutpLst = BindTestGourp();//所有组合项目
                if (ViewState["TestItemID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["TestItemID"]);
                    //item = (from Dicttestitem in testitemgoutpLst where Dicttestitem.Dicttestitemid == id select Dicttestitem).ToList<Dicttestitem>()[0];
                    item = testitemservice.SelectDicttestitemByDicttestitemid(id);
                }
                DicttestitemService service = new DicttestitemService();
                bool b = service.DelDictGrouptestitemByID(item);//删除操作
                if (b)
                {
                    AddTestGroup();//清空
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                    BindSearchData();
                    MessageBoxShow("删除成功！");
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
                if (gdGroupTestItem.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }
                Hashtable ht = new Hashtable();
                ht.Add("testtype", 1);
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
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("Testcode", "组合代码");
                //sortlist.Add("Engname", "英文名");
                sortlist.Add("Testname", "组合名称");
                sortlist.Add("Fastcode", "助记符");
                ExcelOperation<Dicttestitem>.ExportListToExcel(testitemLst, sortlist, filename, sheetname);
            }
            catch (Exception)
            {
                MessageBoxShow("导出出错，请联系管理员！");
            }
        }
        #endregion

        #region 显示组合详细信息
        protected void gdGroupTestItem_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (gdGroupTestItem.SelectedRowIndexArray.Length > 0)
                {
                    if (e.RowIndex >= 0)
                    {
                        int gridRowID = e.RowIndex;
                        Dicttestitem item = new Dicttestitem();
                        //int index = gdGroupTestItem.PageIndex * gdGroupTestItem.PageSize + gridRowID;
                        object[] keys = gdGroupTestItem.DataKeys[gridRowID];
                        //List<Dicttestitem> testitemgoutpLst = BindTestGourp();//所有组合项目
                        item = new Dicttestitem();
                        //item = (from Dicttestitem in testitemgoutpLst where Dicttestitem.Dicttestitemid == Convert.ToInt32(keys[0]) select Dicttestitem).ToList<Dicttestitem>()[0];
                        item = testitemservice.SelectDicttestitemByDicttestitemid(Convert.ToDouble(keys[0]));
                        ViewState["TestItemID"] = item.Dicttestitemid;
                        FormEdit.Title = "当前状态-编辑";
                        txtGroupTestCode.Text = item.Testcode;
                        txtGroupTestName.Text = item.Testname;
                        txtFastCode.Text = item.Fastcode;
                        ddlPhysicalGourp.SelectedValue = (item.Dictlabdeptid ?? -1).ToString();//物理组
                        ddlSpecimenType.SelectedValue = (item.Dictspecimentypeid ?? -1).ToString();//标本类型

                        nbbStandardPrice.Text = item.Price.ToString();
                        txtUniQueid.Text = item.Uniqueid;
                        ddlTubeGroup.SelectedValue = item.Tubegroup ?? "-1";//分管原则
                        //if ("1".Equals(item.Active))
                        //{
                        chbActive.Checked = item.Active == "1" ? true : false;
                        //}
                        chbIsonlyForBill.Checked = item.Isonlyforbill == "1" ? true : false;
                        txtOperationRemark.Text = item.Operationremark;
                        txtDisplayOrder.Text = item.Displayorder.ToString();
                        BindTestGourp(item);
                    }
                }
                else
                {
                    AddTestGroup();
                }
            }
            catch (Exception)
            {

                MessageBoxShow("绑定出错，请联系管理员！");
            }

        }

        private void BindTestGourp(Dicttestitem item)
        {
            List<Dicttestitem> listIn = testitemservice.GetGroupInTestItem(item.Dicttestitemid);
            //List<Dicttestgroupdetail> testgroupdetailLisAll = BindtestgroupdetailAll();//所有组合明细
            //double?[] ss = (from bb in testgroupdetailLisAll where bb.Testgroupid == item.Dicttestitemid select bb.Dicttestitemid).ToArray();
            //List<Dicttestitem> testitemLst = BindTestItem();//所有单项
            //List<Dicttestitem> listIn = (from aa in testitemLst where ss.Contains(aa.Dicttestitemid) select aa).ToList();

            gdGroupIncludeTestItem.DataSource = listIn;
            gdGroupIncludeTestItem.DataBind();
            //if (ViewState["strid"] != null)
            //{
            //    strid = ViewState["strid"].ToString();
            //}
            BindGrid((string)null);//绑定组合未包含项目

        }
        #endregion

        #region 新增组合信息
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtGroupTestCode.Focus();
            AddTestGroup();
            FormEdit.Title = "当前状态-新增";
        }

        private void AddTestGroup()
        {
            gdGroupTestItem.SelectedRowIndexArray = new int[] { };
            ViewState["TestItemID"] = null;
            txtFastCode.Text = null;
            txtGroupTestCode.Text = null;
            txtGroupTestName.Text = null;
            txtOperationRemark.Text = null;
            nbbStandardPrice.Text = null;
            //txtStandardPrice.Text = null;
            chbActive.Checked = true;
            txtUniQueid.Text = null;
            //ddlPhysicalGourp.SelectedValue = "-1";
            //ddlSpecimenType.SelectedValue = "-1";
            //ddlTubeGroup.SelectedValue = "-1";
            //组合未包含项目
            //double? Dictlabdeptid = Convert.ToDouble(ddlgoupLibrary.SelectedValue);
            //listNotIn = (from Dicttestitem in testitemListAll where Dicttestitem.Dictlabdeptid == Dictlabdeptid && Dicttestitem.Testtype == "0" select Dicttestitem).ToList<Dicttestitem>();

            //listNotIn = (from Dicttestitem in testitemListAll where Dicttestitem.Testtype == "0" select Dicttestitem).ToList<Dicttestitem>();

            //gdGroupNotIncludeTestItem.DataSource = listNotIn;
            //gdGroupNotIncludeTestItem.DataBind();
            BindGrid(null);//绑定未包含项

            List<Dicttestitem> listIn = new List<Dicttestitem>();
            gdGroupIncludeTestItem.DataSource = listIn;
            gdGroupIncludeTestItem.DataBind();
        }
        #endregion

        #region 查找未包含项目
        protected void btnSearchNoIn_Trigger1Click(object sender, EventArgs e)
        {
            btnSearchNoIn.Text = "";
            btnSearchNoIn.ShowTrigger1 = false;
        }
        // 执行搜索动作   
        protected void btnSearchNoIn_Trigger2Click(object sender, EventArgs e)
        {
            string strid;
            if (ViewState["strid"] != null)
                strid = ViewState["strid"].ToString();
            else
                strid = null;
            gdGroupNotIncludeTestItem.PageIndex = 0;
            BindGrid(strid);
        }
        #endregion

        #region 移除组合下的项目
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string strid = null;
                List<Dicttestitem> listIn = GridlistIn();//包含的项目
                int[] index = gdGroupIncludeTestItem.SelectedRowIndexArray;
                if (index == null || index.Length == 0)
                {
                    MessageBoxShow("请选择要移除的项！"); return;
                }
                for (int i = index.Length - 1; i >= 0; i--)
                {
                    listIn.Remove(listIn[index[i]]);
                }
                foreach (Dicttestitem testitem in listIn)
                {
                    strid += testitem.Dicttestitemid + ",";
                }

                string str = strid == null ? null : strid.TrimEnd(',');
                ViewState["strid"] = str;
                gdGroupIncludeTestItem.DataSource = listIn;
                gdGroupIncludeTestItem.DataBind();

                //未包含项重新绑定分页
                BindGrid(str);

                gdGroupIncludeTestItem.SelectedRowIndexArray = new int[] { };
                gdGroupNotIncludeTestItem.SelectedRowIndexArray = new int[] { };
            }
            catch (Exception)
            {
                MessageBoxShow("移除组合中的项目报错，请联系管理员！");
            }
        }
        #endregion

        #region 得到选中行的信息
        private List<Dicttestitem> HowManyRowsAreSelected(Grid g)
        {

            Hashtable ht = new Hashtable();
            ht.Add("testtype", 0);
            ht.Add("IsActive", 1);
            List<Dicttestitem> testitemLst = testitemservice.GetSearchData(ht);//所有单项
            int selectedCount = g.SelectedRowIndexArray.Length;
            if (selectedCount == 0)
                return null;
            else
            {
                List<Dicttestitem> itemList = new List<Dicttestitem>();
                for (int i = 0; i < selectedCount; i++)
                {
                    int rowIndex;
                    if (g.AllowPaging && !g.IsDatabasePaging)
                        rowIndex = g.PageIndex * g.PageSize + g.SelectedRowIndexArray[i];
                    else
                        rowIndex = g.SelectedRowIndexArray[i];
                    object[] keystr = g.DataKeys[rowIndex];
                    itemList.Add((from Dicttestitem in testitemLst where Dicttestitem.Dicttestitemid == Convert.ToInt32(keystr[0]) select Dicttestitem).ToList<Dicttestitem>()[0]);
                }
                return itemList;
            }
        }
        #endregion

        #region 组合包含项目
        public List<Dicttestitem> GridlistIn()
        {
            Hashtable ht = new Hashtable();
            ht.Add("testtype", 0);
            ht.Add("IsActive", 1);
            List<Dicttestitem> testitemLst = testitemservice.GetSearchData(ht);//所有单项
            List<Dicttestitem> listIn = new List<Dicttestitem>();

            foreach (var item in gdGroupIncludeTestItem.Rows)
            {
                object[] keystr = item.DataKeys;
                IEnumerable<Dicttestitem> ietestitem = testitemLst.Where(c => c.Dicttestitemid == Convert.ToInt32(keystr[0]));
                if (ietestitem.Count() > 0)
                {
                    listIn.Add(ietestitem.First<Dicttestitem>());
                }

                //   listIn.Add((from Dicttestitem in testitemLst where Dicttestitem.Dicttestitemid == Convert.ToInt32(keystr[0]) select Dicttestitem).ToList<Dicttestitem>()[0]);
            }
            return listIn;
        }
        #endregion

        #region 添加项目到组合
        protected void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                string strid = null;
                ViewState["strid"] = null;

                //得到选择项的集合
                List<Dicttestitem> list = HowManyRowsAreSelected(gdGroupNotIncludeTestItem);

                if (list == null || list.Count == 0)
                {
                    MessageBoxShow("请选择要添加的项！"); return;
                }

                #region 判断选中项(选中多项时)的模板是否一致
                // 判断选中项(选中多项时)的模板是否一致
                //if (list.Count > 1)
                //{
                //    for (int i = 0; i < list.Count - 1; i++)
                //    {
                //        for (int j = 0; j < list.Count; j++)
                //        {
                //            if (list[i].Dictreporttemplateid != list[j].Dictreporttemplateid)
                //            {
                //                MessageBoxShow("添加项[" + list[i].Testname + "]与[" + list[j].Testname + "]模板不一致！"); return;
                //            }
                //        }

                //    }
                //}
                #endregion

                List<Dicttestitem> listIn = GridlistIn();//包含的项目
                //如果集合中存在项目则需判断模板与原有项是否一致,否则直接添加
                if (listIn.Count != 0)
                {
                    //判断添加项的模板与原有项的模板是否一致，一致则添加，否则不让添加
                    //if (list[0].Dictreporttemplateid == listIn[0].Dictreporttemplateid)
                    //{
                    foreach (var testitemmode in list)
                    {
                        foreach (var item in listIn)
                        {
                            if (testitemmode.Dicttestitemid == item.Dicttestitemid)
                            {
                                //不允许添加相同的项
                                MessageBoxShow(String.Format("添加项[{0}]已存在，不允许添加重复项！", testitemmode.Testname)); return;
                            }
                        }
                        listIn.Add(testitemmode);
                    }
                    //}
                    //else
                    //{
                    //    MessageBoxShow("添加项与已有项模板不一致！"); return;
                    //}

                }
                else
                {
                    foreach (var testitemmode in list)
                    {
                        listIn.Add(testitemmode);
                    }
                }
                foreach (var item in listIn)
                {
                    strid += item.Dicttestitemid + ",";
                }

                ViewState["strid"] = strid.TrimEnd(',');
                //重新绑定数据
                gdGroupIncludeTestItem.DataSource = listIn;
                gdGroupIncludeTestItem.DataBind();

                //未包含项重新绑定分页
                BindGrid(strid.TrimEnd(','));

                gdGroupNotIncludeTestItem.SelectedRowIndexArray = new int[] { };
                gdGroupIncludeTestItem.SelectedRowIndexArray = new int[] { };
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.ToString());
            }
        }
        #endregion

        #region 组合分页
        protected void gdGroupTestItem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdGroupTestItem.PageIndex = e.NewPageIndex;
            BindSearchData();
        }
        #endregion

        #region >>>未包含项绑定数据及分页
        private void BindGrid(string strid)
        {
            Dicttestitem item = new Dicttestitem();
            if (ViewState["TestItemID"] != null)
            {
                double? id = Convert.ToDouble(ViewState["TestItemID"]);                
                item = testitemservice.SelectDicttestitemByDicttestitemid(id);
            }
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdGroupNotIncludeTestItem.PageIndex, gdGroupNotIncludeTestItem.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", btnSearchNoIn.Text.Trim() == "" ? null : btnSearchNoIn.Text.Trim());
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("testitemid", item.Dicttestitemid);
            ht1.Add("strtestitemid", strid);
            //strtestitemid
            //设置总项数
            gdGroupNotIncludeTestItem.RecordCount = testitemservice.GetDicttestgroupPageLstCount(ht1);
            List<Dicttestitem> listNotIn = testitemservice.GetDicttestgroupPageLst(ht1);
            gdGroupNotIncludeTestItem.DataSource = listNotIn;
            gdGroupNotIncludeTestItem.DataBind();
        }



        protected void gdGroupNotIncludeTestItem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdGroupNotIncludeTestItem.PageIndex = e.NewPageIndex;
            string strid;
            if (ViewState["strid"] != null)
                strid = ViewState["strid"].ToString();
            else
                strid = null;

            BindGrid(strid);
        }
        #endregion
    }
}