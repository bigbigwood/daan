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
    public partial class DictTestProduct : PageBase
    {
        #region Loading
        LoginService loginservice = new LoginService();
        DicttestitemService testitemservice = new DicttestitemService();
        DictCustomerService dictCustomerService = new DictCustomerService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + DropCustomer.ClientID + "')).listWidth=250;");
                btnDel.OnClientClick = gdProductTestItem.GetNoSelectionAlertReference("请选择需要删除的套餐！", "提示");
                btnDel.ConfirmText = "确实要删除该套餐？";
                BingMedical(ddlTestUnit, "0");//绑定体检单位下拉框
                BingMedical(DropCustomer, "0");//绑定体检单位下拉框
                DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
                DDLInitbasicBinder(ddlforsex, "REFSEX");//测试项性别
                ddlforsex.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
                
            }
        }
        #endregion

        #region >>>项目及分页
        private void BindTestProductGrid(string testtype, string dictcustmerid)
        {

            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdProductTestItem.PageIndex, gdProductTestItem.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("Dictcustomerid", dictcustmerid);
            ht1.Add("Testtype", testtype);
            ht1.Add("IsActive", chkActive.Checked ? "1" : "0");
            //设置总项数
            gdProductTestItem.RecordCount = testitemservice.GetDictTestProductPageLstCount(ht1);
            List<Dicttestitem> list = testitemservice.GetDictTestProductPageLst(ht1);
            gdProductTestItem.DataSource = list;
            gdProductTestItem.DataBind();
        }

        #endregion

        #region 绑定缓存数据

        public List<Dicttestitem> BindTestItemAll()
        {
            List<Dicttestitem> testitemListAll = loginservice.GetLoginDicttestitemList();//所有项目包括单项和组合
            return testitemListAll;
        }
        public List<Dicttestitem> BindTestItemProduct()
        {
            List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
            //套餐
            List<Dicttestitem> testproductListAll = (from Dicttestitem in testitemListAll where (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>();
            return testproductListAll;//返回所有套餐
        }

        public List<Dictproductdetail> BindtestproductAll()
        {
            //套餐明细
            List<Dictproductdetail> testproductAll = loginservice.GetLoginDictproductdetail();
            return testproductAll;
        }
        public List<Dictlabandtest> BindLabAndTest()
        {
            //分点项目明细
            List<Dictlabandtest> lanandtestall = loginservice.GetLoginDictlabandtest();
            return lanandtestall;
        }
        public List<Dictlabandtestprice> BindLabAndTestPrice()
        {
            //分点项目对应的价格
            List<Dictlabandtestprice> lanandtestpriceall = loginservice.GetLoginDictlabandtestpriceList();
            return lanandtestpriceall;
        }
        public List<Dicttestgroupdetail> Bindtestgroupdetails()
        {
            //组合明细
            List<Dicttestgroupdetail> dicttestgroupdetails = loginservice.GetLoginDicttestgroupdetail();
            return dicttestgroupdetails;
        }
        #endregion

        #region 查询套餐
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            gdProductTestItem.PageIndex = 0;
            BindSearchData();
        }

        private void BindSearchData()
        {
            if (ddlProductType.SelectedValue != "0" && DropCustomer.SelectedValue != "-1")
            {
                BindTestProductGrid(ddlProductType.SelectedValue, DropCustomer.SelectedValue);
            }
            else if (ddlProductType.SelectedValue != "0")
            {
                BindTestProductGrid(ddlProductType.SelectedValue, null);
            }
            else if (DropCustomer.SelectedValue != "-1")
            {
                BindTestProductGrid(null, DropCustomer.SelectedValue);
            }
            else
            {
                BindTestProductGrid(null, null);
            }

            //List<Dicttestitem> testitemLst = GetGroupTestItemByC();
            //gdProductTestItem.DataSource = testitemLst;
            //gdProductTestItem.DataBind();
        }

        //public List<Dicttestitem> GetGroupTestItemByC()
        //{
        //    List<Dicttestitem> testproductListAll = BindTestItemProduct();//所有套餐
        //    List<Dicttestitem> testitemLst = new List<Dicttestitem>();

        //    double? userid = Convert.ToDouble(DropCustomer.SelectedValue);

        //    string producttype = ddlProductType.SelectedValue;

        //    if (ddlProductType.SelectedValue == "0" && DropCustomer.SelectedValue == "-1")
        //    {
        //        testitemLst = testproductListAll;
        //    }
        //    else if (ddlProductType.SelectedValue == "0")
        //    {
        //        testitemLst = (from Dicttestitem in testproductListAll where Dicttestitem.Dictcustomerid == userid select Dicttestitem).ToList<Dicttestitem>();  //2,公用套餐   3,单位套餐   
        //    }
        //    else if (DropCustomer.SelectedValue == "-1")
        //    {
        //        testitemLst = (from Dicttestitem in testproductListAll where (Dicttestitem.Testtype == producttype) select Dicttestitem).ToList<Dicttestitem>();  //2,公用套餐   3,单位套餐   

        //    }
        //    else
        //    {
        //        testitemLst = (from Dicttestitem in testproductListAll where Dicttestitem.Dictcustomerid == userid && (Dicttestitem.Testtype == producttype) select Dicttestitem).ToList<Dicttestitem>();  //2,公用套餐   3,单位套餐   
        //    }

        //    string testitemStr = ttbSearch.Text;
        //    List<Dicttestitem> newslist = new List<Dicttestitem>();
        //    foreach (Dicttestitem testitem in testitemLst)
        //    {
        //        if ((testitem.Fastcode != null && testitem.Fastcode.ToLower().Contains(testitemStr.ToLower())) ||
        //            (testitem.Engname != null && testitem.Engname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (testitem.Testname != null && testitem.Testname.ToLower().Contains(testitemStr.ToLower())) ||
        //            (testitem.Testcode != null && testitem.Testcode.ToLower().Contains(testitemStr.ToLower())) ||
        //            (testitem.Uniqueid != null && testitem.Uniqueid.ToLower().Contains(testitemStr.ToLower()))
        //            || testitemStr == "")
        //        {
        //            newslist.Add(testitem);
        //        }
        //    }
        //    return newslist;
        //}
        #endregion

        #region 套餐包含的项目
        public List<Dicttestitem_productdetail> GridlistIn()
        {
            List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
            List<Dicttestitem_productdetail> productlist = new List<Dicttestitem_productdetail>();
            foreach (var item1 in gdProductIncludeTestItem.Rows)
            {
                Dicttestitem_productdetail detail = new Dicttestitem_productdetail();
                System.Web.UI.WebControls.CheckBox chb = (System.Web.UI.WebControls.CheckBox)item1.FindControl("chbSendouttest");
                System.Web.UI.WebControls.TextBox finlprice = (System.Web.UI.WebControls.TextBox)item1.FindControl("tbxFinalpricess");
                System.Web.UI.WebControls.DropDownList ddlcustomer = (System.Web.UI.WebControls.DropDownList)item1.FindControl("DropDownList1");
                if (chb != null && chb.Checked)
                {
                    detail.Issendouttest = "1";
                    if (ddlcustomer.SelectedValue != "")
                        detail.Sendoutcustomerid = Convert.ToDouble(ddlcustomer.SelectedValue);
                    else
                        detail.Sendoutcustomerid = 0;
                }
                if (finlprice != null)
                {
                    double d = finlprice.Text == "" ? 0 : Convert.ToDouble(finlprice.Text);
                    detail.Finalprice = d;
                }
                //if (ddlcustomer != null && rdobtnPublicProduLst.SelectedValue == "unitP")
                //{

                //}
                object[] keystr = item1.DataKeys;
                detail.Dicttestitem = (from Dicttestitem in testitemListAll where Dicttestitem.Dicttestitemid == Convert.ToInt32(keystr[0]) select Dicttestitem).ToList<Dicttestitem>()[0];
                productlist.Add(detail);
            }

            return productlist;
        }
        #endregion

        #region 保存套餐信息
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductTestCode.Text.Trim()))
            {
                MessageBoxShow("请输入套餐代码");
                return;
            }
            if (string.IsNullOrEmpty(txtProductTestName.Text.Trim()))
            {
                MessageBoxShow("请输入套餐名称");
                return;
            }
            if (string.IsNullOrEmpty(txtFastCode.Text.Trim()))
            {
                MessageBoxShow("请输入助记符");
                return;
            }
            if (ddlforsex.SelectedValue == "-1")
            {
                MessageBoxShow("请输入测试项性别");
                return;
            }
            if (gdProductIncludeTestItem.Rows.Count == 0)
            {
                MessageBoxShow("没有任何组合项目不能保存");
                return;
            }
            Dicttestitem item = new Dicttestitem();
            List<Dicttestitem> testproductListAll = BindTestItemProduct();//所有套餐            
            //和页面填写代码相同的套餐
            IEnumerable<Dicttestitem> IEdict = testproductListAll.Where<Dicttestitem>(c => c.Testcode == txtProductTestCode.Text.Trim());

            if (ViewState["TestItemID"] != null)
            {
                if (IEdict.Count() > 1)
                {
                    MessageBoxShow("系统中存在相同的套餐代码[" + txtProductTestCode.Text.Trim() + "]");
                    return;
                }
                double? id = Convert.ToDouble(ViewState["TestItemID"]);
                item = (from Dicttestitem in testproductListAll where Dicttestitem.Dicttestitemid == id && (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>()[0];


            }
            else
            {
                if (IEdict.Count() == 1)
                {
                    MessageBoxShow("系统中存在相同的套餐代码[" + txtProductTestCode.Text.Trim() + "]");
                    return;
                }
            }
            item.Fastcode = txtFastCode.Text.Trim();
            item.Testcode = txtProductTestCode.Text.Trim();
            item.Testname = txtProductTestName.Text.Trim();
            item.Forsex = ddlforsex.SelectedValue;

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
            //必做套餐
            if (chbInspection.Checked)
            {
                item.Isneededorder = "1";
            }
            else
            {
                item.Isneededorder = "0";
            }
            //是否只是产品
            if (this.chbIsProject.Checked)
            {
                item.IsProject = "1";
            }
            else
            {
                item.IsProject = "0";
            }
            //如果是单位套餐必须选择体检单位
            if (rdobtnPublicProduLst.SelectedValue == "unitP")
            {
                item.Testtype = "3";//3，单位套餐
                if (ddlTestUnit.SelectedValue == "-1")
                {
                    MessageBoxShow("请选择体检单位！"); return;
                }
                item.Dictcustomerid = Convert.ToDouble(ddlTestUnit.SelectedValue);
            }
            else
            {
                item.Testtype = "2";//2，公共套餐
            }

            List<Dicttestitem_productdetail> productlist = GridlistIn();//包含的项目
            //赋值 成交价和外包单位
            //计算套餐总价
            double totalPrice = 0;
            for (int i = 0, count = gdProductIncludeTestItem.Rows.Count; i < count; i++)
            {
                GridRow row = gdProductIncludeTestItem.Rows[i];
                if (productlist[i].Issendouttest == "1")//外包
                {
                    System.Web.UI.WebControls.DropDownList ddlcustomer = (System.Web.UI.WebControls.DropDownList)row.FindControl("DropDownList1");
                    if (ddlcustomer.SelectedValue != "")
                        productlist[i].Sendoutcustomerid = Convert.ToDouble(ddlcustomer.SelectedValue);
                    else
                        productlist[i].Sendoutcustomerid = 0;
                }
                System.Web.UI.WebControls.TextBox finlprice = (System.Web.UI.WebControls.TextBox)row.FindControl("tbxFinalpricess");
                try
                {
                    double d = finlprice.Text == "" ? 0 : Convert.ToDouble(finlprice.Text);
                    productlist[i].Finalprice = d;
                    totalPrice = totalPrice + d;
                }
                catch (Exception)
                {
                    MessageBoxShow("成交价请输入数字类型"); return;
                }
            }
            item.Price = totalPrice;

            //如果是修改得到旧值
            Dicttestitem testitem = new Dicttestitem();
            if (item.Dicttestitemid != null)
            {
                testitem = (from Dicttestitem in testproductListAll where Dicttestitem.Dicttestitemid == item.Dicttestitemid && (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>()[0];
            }

            //item 实体；productlist 修改值；testitem 原始值
            double? f = testitemservice.SaveDictTestItemProduct(item, productlist, testitem);
            if (f > 0)
            {
                item.Dicttestitemid = f;
                ViewState["TestItemID"] = f;
                CacheHelper.RemoveAllCache("daan.GetDictproductdetail");
                CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                BindtestproductAll();//套餐明细
                BindSearchData();//套餐
                // AddTestProduct();//清空
                MessageBoxShow("新增成功！");
            }
            else if (f == 0)
            {
                CacheHelper.RemoveAllCache("daan.GetDictproductdetail");
                CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                BindtestproductAll();//套餐明细
                BindSearchData();
                //AddTestProduct();//清空
                MessageBoxShow("修改成功！");
            }
            else
            {
                MessageBoxShow("操作报错！");
            }
        }
        #endregion

        #region 删除套餐项目
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Dicttestitem item = new Dicttestitem();
                List<Dicttestitem> testproductListAll = BindTestItemProduct();//所有套餐
                if (ViewState["TestItemID"] != null)
                {
                    double? id = Convert.ToDouble(ViewState["TestItemID"]);
                    item = (from Dicttestitem in testproductListAll where Dicttestitem.Dicttestitemid == id && (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>()[0];
                }

                bool b = testitemservice.DelDictProducttestitemByID(item);//删除操作
                if (b)
                {
                    AddTestProduct();
                    CacheHelper.RemoveAllCache("daan.GetDicttestitem");
                    //BindtestproductAll();//套餐明细
                    BindSearchData();//套餐

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
                if (gdProductTestItem.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }

                Hashtable ht1 = new Hashtable();

                ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));


                if (ddlProductType.SelectedValue != "0" && DropCustomer.SelectedValue != "-1")
                {
                    ht1.Add("Dictcustomerid", DropCustomer.SelectedValue);
                    ht1.Add("Testtype", ddlProductType.SelectedValue);
                }
                else if (ddlProductType.SelectedValue != "0")
                {
                    ht1.Add("Dictcustomerid", null);
                    ht1.Add("Testtype", ddlProductType.SelectedValue);
                }
                else if (DropCustomer.SelectedValue != "-1")
                {
                    ht1.Add("Dictcustomerid", DropCustomer.SelectedValue);
                    ht1.Add("Testtype", null);
                }
                else
                {
                    ht1.Add("Dictcustomerid", null);
                    ht1.Add("Testtype", null);
                }
                ht1.Add("IsActive", chkActive.Checked ? "1" : "0");
                List<Dicttestitem> testitemLst = testitemservice.GetSearchTestProductData(ht1);

                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("Testcode", "套餐代码");
                sortlist.Add("Testname", "套餐名称");
                sortlist.Add("Fastcode", "助记符");
                ExcelOperation<Dicttestitem>.ExportListToExcel(testitemLst, sortlist, filename, sheetname);
            }
            catch (Exception)
            {
                MessageBoxShow("导出出错，请联系管理员！");
            }
        }
        #endregion

        #region 显示套餐细信息
        protected void gdProductTestItem_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (gdProductTestItem.SelectedRowIndexArray.Length > 0)
                {
                    Dicttestitem item = new Dicttestitem();
                    if (e.RowIndex >= 0)
                    {
                        int gridRowID = e.RowIndex;
                        //int index = gdProductTestItem.PageIndex * gdProductTestItem.PageSize + gridRowID;
                        object[] keys = gdProductTestItem.DataKeys[gridRowID];
                        List<Dicttestitem> testproductListAll = BindTestItemProduct();//所有套餐
                        item = (from Dicttestitem in testproductListAll where Dicttestitem.Dicttestitemid == Convert.ToInt32(keys[0]) && (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>()[0];
                        ViewState["TestItemID"] = item.Dicttestitemid;
                        SimpleFormEdit.Title = "当前状态-编辑";
                        txtProductTestCode.Text = item.Testcode;
                        txtProductTestName.Text = item.Testname;
                        txtFastCode.Text = item.Fastcode;
                        txtStandardPrice.Text = item.Price.ToString();
                        ddlforsex.SelectedValue = item.Forsex ?? "-1";

                        if ("1".Equals(item.Active))
                        {
                            chbActive.Checked = true;
                        }
                        else
                        {
                            chbActive.Checked = false;
                        }
                        if ("1".Equals(item.Isneededorder))
                        {
                            chbInspection.Checked = true;
                        }
                        else
                        {
                            chbInspection.Checked = false;
                        }
                        chbIsonlyForBill.Checked = item.Isonlyforbill == "1" ? true : false;
                        //是否只是产品
                        chbIsProject.Checked = item.IsProject.Equals("1") ? true : false;
                        //2,公用套餐   3,单位套餐   
                        if (item.Testtype == "3")
                        {
                            BingMedical(ddlTestUnit, "0"); //单位套餐
                            ddlTestUnit.Enabled = true;
                            rdobtnPublicProduLst.SelectedValue = "unitP";
                            ddlTestUnit.SelectedValue = item.Dictcustomerid.ToString();
                            Dictcustomer dictcustomeru = dictCustomerService.GetDictCustomerById(item.Dictcustomerid.Value);
                            BindGrid(null, dictcustomeru.Dictlabid.ToString());
                        }
                        else if (item.Testtype == "2") //公用套餐
                        {
                            BingMedical(ddlTestUnit, "3");
                            BindGrid(null, null);
                            rdobtnPublicProduLst.SelectedValue = "publicP";
                            ddlTestUnit.Enabled = false;
                        }
                        List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
                        List<Dictproductdetail> testproductAll = BindtestproductAll();//套餐明细
                        //套餐已包含组合 productlist
                        //套餐未包含组合 listNotIn
                        double?[] ss = (from bb in testproductAll where bb.Productid == item.Dicttestitemid select bb.Testgroupid).ToArray();
                        List<Dicttestitem_productdetail> productlistTo = (from aa in testitemListAll
                                                                          join dd in testproductAll
                                                                              on aa.Dicttestitemid equals dd.Testgroupid
                                                                          where dd.Productid == item.Dicttestitemid && ss.Contains(aa.Dicttestitemid) && (aa.Testtype == "0" || aa.Testtype == "1")
                                                                          select new Dicttestitem_productdetail
                                                                          {

                                                                              Dicttestitem = aa,
                                                                              Finalprice = dd.Finalprice,
                                                                              Issendouttest = dd.Issendouttest,
                                                                              Sendoutcustomerid = dd.Sendoutcustomerid
                                                                          }).Distinct().ToList();


                        gdProductIncludeTestItem.DataSource = productlistTo;
                        gdProductIncludeTestItem.DataBind();
                        //if (item.Testtype == "3") //如果是单位套餐，查找相对应的分点下的检测项目价格，如果有，将采用分点检测项目维护价格
                        //{
                        //    BingMedical(ddlTestUnit, "0");                        
                        //}
                        //else if (item.Testtype == "2")  //公用套餐取检查组合中的价格
                        //{
                        //    BingMedical(ddlTestUnit, "3");               
                        //}

                    }
                }
                else
                {
                    AddTestProduct();
                }
            }
            catch (Exception)
            {

                MessageBoxShow("绑定出错，请联系管理员！");
            }

        }
        #endregion

        #region 新增套餐信息
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtProductTestCode.Focus();
            SimpleFormEdit.Title = "当前状态-新增";
            AddTestProduct();
        }

        private void AddTestProduct()
        {
            gdProductTestItem.SelectedRowIndexArray = new int[] { };
            ViewState["TestItemID"] = null;
            txtFastCode.Text = null;
            txtProductTestCode.Text = null;
            txtProductTestName.Text = null;
            chbActive.Checked = true;
            txtStandardPrice.Text = null;
            ddlTestUnit.SelectedIndex = 0;
            ddlforsex.SelectedIndex = ddlforsex.Items.Count() > 1 ? 1 : 0;
            List<Dicttestitem_productdetail> productlist = new List<Dicttestitem_productdetail>();
            gdProductIncludeTestItem.DataSource = productlist;
            gdProductIncludeTestItem.DataBind();
        }
        #endregion

        #region 查找未包含组合
        protected void btnSearchNoIn_Trigger1Click(object sender, EventArgs e)
        {
            btnSearchNoIn.Text = "";
            btnSearchNoIn.ShowTrigger1 = false;
        }
        // 执行搜索动作   
        protected void btnSearchNoIn_Trigger2Click(object sender, EventArgs e)
        {
            string strid = null;
            if (ViewState["strid"] != null)
            {
                strid = ViewState["strid"].ToString();
            }
            gdProductNotIncludeTestItem.PageIndex = 0;
            if (rdobtnPublicProduLst.SelectedValue == "unitP")//单位体检
            {
                Dictcustomer dictcustomeru = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
                BindGrid(strid, dictcustomeru.Dictlabid.ToString());
            }
            else
            {
                BindGrid(strid, null);//模糊查询未包含项及分页
            }
        }
        #endregion

        #region 移除套餐下的组合
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string strid = null;
                ViewState["strid"] = null;

                //选中项索引的集合
                int[] index = gdProductIncludeTestItem.SelectedRowIndexArray;

                if (index == null || index.Length == 0)
                {
                    MessageBoxShow("请选择要移除的项！"); return;
                }

                //集合的索引和选中项的索引对应，删除集合中选中的项
                List<Dicttestitem_productdetail> productlist = GridlistIn();//包含的项目
                for (int i = index.Length - 1; i >= 0; i--)
                {
                    productlist.Remove(productlist[index[i]]);
                }
                //页面操作集合时，包含项的主键字符串
                foreach (Dicttestitem_productdetail testitem in productlist)
                {
                    strid += testitem.Dicttestitem.Dicttestitemid + ",";
                }
                gdProductIncludeTestItem.DataSource = productlist;
                gdProductIncludeTestItem.DataBind();

                //去掉最后一个逗号
                string str = strid == null ? null : strid.TrimEnd(',');

                //把包含项的主键字符串保存在ViewState中，翻页的时候需要
                ViewState["strid"] = str;

                //判断是单位套餐的话就根据单位对应的分点查找项目，否则就查询所有
                if (rdobtnPublicProduLst.SelectedValue == "unitP")
                {
                    Dictcustomer dictcustomeru = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
                    BindGrid(str, dictcustomeru.Dictlabid.ToString());
                }
                else
                {
                    BindGrid(str, null);
                }
                gdProductNotIncludeTestItem.SelectedRowIndexArray = new int[] { };
                gdProductIncludeTestItem.SelectedRowIndexArray = new int[] { };
            }
            catch (Exception)
            {
                MessageBoxShow("移除套餐下的项出错，请联系管理员！", MessageBoxIcon.Information); return;
            }
        }
        #endregion

        #region 得到选中行的信息
        private List<Dicttestitem_productdetail> HowManyRowsAreSelected(Grid g)
        {
            List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
            int selectedCount = g.SelectedRowIndexArray.Length;
            if (selectedCount == 0)
                return null;
            else
            {
                List<Dicttestitem_productdetail> itemList = new List<Dicttestitem_productdetail>();
                for (int i = 0; i < selectedCount; i++)
                {
                    int rowIndex = g.SelectedRowIndexArray[i];
                    if (g.AllowPaging && !g.IsDatabasePaging)
                    {
                        rowIndex = g.PageIndex * g.PageSize + rowIndex;
                    }
                    object[] keystr = g.DataKeys[rowIndex];
                    //选中行的实体添加到集合中
                    //Dicttestitem_productdetail dictitem_productdetail = null;       
                    Dicttestitem_productdetail dictitem_productdetail = new Dicttestitem_productdetail();
                    dictitem_productdetail.Dicttestitem = (from Dicttestitem in testitemListAll where Dicttestitem.Dicttestitemid == Convert.ToInt32(keystr[0]) select Dicttestitem).ToList<Dicttestitem>()[0];

                    //添加到集合中
                    itemList.Add(dictitem_productdetail);

                }
                return itemList;
            }
        }
        #endregion

        #region 得到选中行的信息
        private List<Dicttestitem_productdetail> HowManyRowsAreSelected(Grid g, List<Dicttestitem_productdetail> testLst)
        {
            int selectedCount = g.SelectedRowIndexArray.Length;
            if (selectedCount == 0)
                return null;
            else
            {
                List<Dicttestitem_productdetail> itemList = new List<Dicttestitem_productdetail>();
                for (int i = 0; i < selectedCount; i++)
                {
                    int rowIndex = g.SelectedRowIndexArray[i];
                    if (g.AllowPaging && !g.IsDatabasePaging)
                    {
                        rowIndex = g.PageIndex * g.PageSize + rowIndex;
                    }
                    object[] keystr = g.DataKeys[rowIndex];
                    //选中行的实体添加到集合中
                    itemList.Add((from Dicttestitem_productdetail in testLst where Dicttestitem_productdetail.Dicttestitem.Dicttestitemid == Convert.ToInt32(keystr[0]) select Dicttestitem_productdetail).ToList<Dicttestitem_productdetail>()[0]);
                }
                return itemList;
            }
        }

        #endregion

        #region 添加组合到套餐 (左边列表增加右边选中的项目)
        protected void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                DictlabandtestService lanandtest = new DictlabandtestService();
                DictlabandtestpriceService lanandtestprice = new DictlabandtestpriceService();
                string strid = null;
                ViewState["strid"] = null;
                //选中项集合
                List<Dicttestitem_productdetail> testitemLst = HowManyRowsAreSelected(gdProductNotIncludeTestItem);


                if (testitemLst == null || testitemLst.Count == 0)
                {
                    MessageBoxShow("请选择要添加项！", MessageBoxIcon.Information); return;
                }



                #region 判断选中项(选中多项时)的模板是否一致是否一致 注释
                // 判断选中项(选中多项时)的模板是否一致是否一致
                //if (testitemLst.Count > 1)
                //{
                //    for (int i = 0; i < testitemLst.Count - 1; i++)
                //    {
                //        for (int j = 0; j < testitemLst.Count; j++)
                //        {
                //            if (testitemLst[i].Dicttestitem.Dictreporttemplateid != testitemLst[j].Dicttestitem.Dictreporttemplateid)
                //            {
                //                MessageBoxShow("添加项[" + testitemLst[i].Dicttestitem.Testname + "]与[" + testitemLst[j].Dicttestitem.Testname + "]模板不一致！", "提示", MessageBoxIcon.Information); return;
                //            }
                //        }
                //    }
                //}
                #endregion

                List<Dicttestitem_productdetail> productlist = GridlistIn();//包含的项目

                #region 判断现有项与添加项的模板是否一致 注释
                //if (productlist.Count != 0)
                //{
                //    //判断现有项与添加项的模板是否一致以及组合中的单项在已包含集合中是否存在
                //    //如果模板不一致则返回，如果组合中的单项已存在也返回
                //    if (!checkAddTestItem(productlist, testitemLst))
                //    {
                //        return;
                //    }
                //}
                #endregion

                //publicP,表示公用套餐；unitP,表示单位套餐，如果是单位套餐，则需要选择体检单位
                if (rdobtnPublicProduLst.SelectedValue == "publicP")
                {
                    foreach (Dicttestitem_productdetail testitem in testitemLst)
                    {
                        productlist.Add(testitem);
                    }
                }
                else if (rdobtnPublicProduLst.SelectedValue == "unitP")
                {
                    //如果是单位套餐则体检单位必选
                    if (ddlTestUnit.SelectedValue == "-1")
                    {
                        MessageBoxShow("请选择体检单位！", MessageBoxIcon.Information); return;
                    }
                    foreach (Dicttestitem_productdetail testitem in testitemLst)
                    {
                        //根据体检单位查找客户信息
                        Dictcustomer dictcustomer = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));

                        List<Dictlabandtest> lanandtestall = BindLabAndTest();//分点项目明细集合
                        //根据分点及项目查找对应的项目是否外包
                        List<Dictlabandtest> labandtestLst = (from Dictlabandtest in lanandtestall where Dictlabandtest.Dictlabid == dictcustomer.Dictlabid && (Dictlabandtest.Dicttestitemid == testitem.Dicttestitem.Dicttestitemid) select Dictlabandtest).ToList<Dictlabandtest>();
                        if (labandtestLst.Count != 0)
                        {
                            testitem.Issendouttest = labandtestLst[0].Issendouttest;
                        }
                        List<Dictlabandtestprice> lanandtestpriceall = BindLabAndTestPrice();//分点项目对应的价格集合
                        //根据分点及项目查找对应的项目价钱
                        List<Dictlabandtestprice> labandtestpriceLst = (from Dictlabandtestprice in lanandtestpriceall where Dictlabandtestprice.Dictlabid == dictcustomer.Dictlabid && (Dictlabandtestprice.Dicttestitemid == testitem.Dicttestitem.Dicttestitemid) select Dictlabandtestprice).ToList<Dictlabandtestprice>();
                        //if (labandtestpriceLst.Count != 0)
                        //{
                        //    testitem.Finalprice = labandtestpriceLst[0].Price;
                        //}
                        productlist.Add(testitem);
                    }
                }
                else
                {
                    MessageBoxShow("请选择套餐类型！", MessageBoxIcon.Information); return;
                }
                //把包含项的主键用字符串连接起来并保持在ViewState中，翻页时调用
                foreach (var item in productlist)
                {
                    strid += item.Dicttestitem.Dicttestitemid + ",";
                }
                ViewState["strid"] = strid.TrimEnd(',');

                //判断是公用套餐还是单位套餐，如果是单位套餐，则需要根据单位对于的分点查找项目，否则查询所有
                if (rdobtnPublicProduLst.SelectedValue == "publicP")
                {
                    BindGrid(strid.TrimEnd(','), null);
                }
                else
                {
                    Dictcustomer dictcustomeru = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
                    BindGrid(strid.TrimEnd(','), dictcustomeru.Dictlabid.ToString());
                }
                //重新绑定包含项和未包含项
                gdProductIncludeTestItem.DataSource = productlist;
                gdProductIncludeTestItem.DataBind();

                gdProductNotIncludeTestItem.SelectedRowIndexArray = new int[] { };
                gdProductIncludeTestItem.SelectedRowIndexArray = new int[] { };
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 检测套餐需要添加的组合或单项是否已存在以及模板是否一致 注释
        /// <summary>
        /// 检测套餐需要添加的组合或单项是否已存在以及模板是否一致
        /// </summary>
        /// <param name="testitems1">已存在的项目</param>
        /// <param name="testitems2">需要添加到项目</param>
        /// <returns></returns>
        //public bool checkAddTestItem(List<Dicttestitem_productdetail> testitems1, List<Dicttestitem_productdetail> testitems2)
        //{
        //    List<Dicttestgroupdetail> dicttestgroupdetails = Bindtestgroupdetails();//组合明细
        //    List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
        //    bool b = true;
        //    foreach (Dicttestitem_productdetail item2 in testitems1)
        //    {
        //        //1,是组合; 0,是单项
        //        if (item2.Dicttestitem.Testtype == "1")
        //        {
        //            double?[] ss = (from bb in dicttestgroupdetails where bb.Testgroupid == item2.Dicttestitem.Dicttestitemid select bb.Dicttestitemid).ToArray();
        //            List<Dicttestitem> itemlist = (from aa in testitemListAll where ss.Contains(aa.Dicttestitemid) && aa.Testtype == "0" select aa).ToList();

        //            foreach (Dicttestitem item3 in itemlist)
        //            {
        //                foreach (Dicttestitem_productdetail item1 in testitems2)
        //                {
        //                    //1,是组合; 0,是单项
        //                    if (item1.Dicttestitem.Testtype == "1")
        //                    {
        //                        double?[] ss1 = (from bb in dicttestgroupdetails where bb.Testgroupid == item1.Dicttestitem.Dicttestitemid select bb.Dicttestitemid).ToArray();
        //                        List<Dicttestitem> itemlist1 = (from aa in testitemListAll where ss1.Contains(aa.Dicttestitemid) && aa.Testtype == "0" select aa).ToList();

        //                        foreach (Dicttestitem item4 in itemlist1)
        //                        {
        //                            if (item4.Dicttestitemid == item3.Dicttestitemid)
        //                            {
        //                                MessageBoxShow(string.Format("您添加的组合[{0}]中的项目[{1}]已经在列表组合[{2}]中存在了！", item1.Dicttestitem.Testname, item4.Testname, item2.Dicttestitem.Testname));
        //                                b = false;
        //                            }

        //                            if (item4.Dictreporttemplateid != item3.Dictreporttemplateid)
        //                            {
        //                                MessageBoxShow(string.Format("您添加的组合[{0}]中的项目[{1}]与组合[{2}]中的项目[{3}]的模板不匹配！", item1.Dicttestitem.Testname, item4.Testname, item2.Dicttestitem.Testname, item3.Testname));
        //                                b = false;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //需要添加的项是单项
        //                        if (item1.Dicttestitem.Dicttestitemid == item3.Dicttestitemid)
        //                        {
        //                            MessageBoxShow(string.Format("您添加的单项[{0}]在列表中组合[{1}]中已经存在了！", item1.Dicttestitem.Testname, item2.Dicttestitem.Testname));
        //                            b = false;
        //                        }
        //                        if (item1.Dicttestitem.Dictreporttemplateid != item3.Dictreporttemplateid)
        //                        {
        //                            MessageBoxShow(string.Format("您添加的单项[{0}]与组合[{1}]中的项目[{2}]的模板不匹配！", item1.Dicttestitem.Testname, item2.Dicttestitem.Testname, item3.Testname));
        //                            b = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //已存在的项是单项
        //            foreach (Dicttestitem_productdetail item1 in testitems2)
        //            {
        //                //1,是组合; 0,是单项
        //                if (item1.Dicttestitem.Testtype == "1")
        //                {
        //                    double?[] ss1 = (from bb in dicttestgroupdetails where bb.Testgroupid == item1.Dicttestitem.Dicttestitemid select bb.Dicttestitemid).ToArray();
        //                    List<Dicttestitem> itemlist1 = (from aa in testitemListAll where ss1.Contains(aa.Dicttestitemid) && aa.Testtype == "0" select aa).ToList();

        //                    foreach (Dicttestitem item4 in itemlist1)
        //                    {
        //                        if (item4.Dicttestitemid == item2.Dicttestitem.Dicttestitemid)
        //                        {
        //                            MessageBoxShow(string.Format("您添加的组合[{0}]中的项目[{1}]已经在列表中存在了！", item1.Dicttestitem.Testname, item4.Testname));
        //                            b = false;
        //                        }
        //                        if (item4.Dictreporttemplateid != item2.Dicttestitem.Dictreporttemplateid)
        //                        {
        //                            MessageBoxShow(string.Format("您添加的组合[{0}]中的项目[{1}]与单项[{2}]模板不一致！", item1.Dicttestitem.Testname, item4.Testname, item2.Dicttestitem.Testname));
        //                            b = false;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //添加项与已存在项都是单项
        //                    if (item1.Dicttestitem.Dicttestitemid == item2.Dicttestitem.Dicttestitemid)
        //                    {
        //                        MessageBoxShow(string.Format("您添加的单项[{0}]在列表中已经存在了！", item1.Dicttestitem.Testname));
        //                        b = false;
        //                    }
        //                    if (item1.Dicttestitem.Dictreporttemplateid != item2.Dicttestitem.Dictreporttemplateid)
        //                    {
        //                        MessageBoxShow(string.Format("您添加的单项[{0}]与单项[{1}]模板不一致！", item1.Dicttestitem.Testname, item2.Dicttestitem.Testname));
        //                        b = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return b;
        //}
        #endregion

        #region 编辑区 套餐类型
        protected void rdobtnPublicProduLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            //单位套餐  绑定体检单位  0,一般客户
            //如果是公共套餐  体检单位不可用
            if (rdobtnPublicProduLst.SelectedValue == "publicP")   //公用
            {
                BindGrid(null, null);//绑定未包含项及分页
                ddlTestUnit.Enabled = false;
                BingMedical(ddlTestUnit, "3");
            }
            if (rdobtnPublicProduLst.SelectedValue == "unitP")  //单位
            {
                BingMedical(ddlTestUnit, "0");
                Dictcustomer dictcustomeru = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
                BindGrid(null, dictcustomeru.Dictlabid.ToString());//绑定未包含项及分页
                ddlTestUnit.Enabled = true;
            }
            if (gdProductTestItem.SelectedRowIndexArray.Length > 0)
            {
                Dicttestitem item = new Dicttestitem();
                if (gdProductTestItem.SelectedRowIndexArray.Length >= 0)
                {
                    object[] keys = gdProductTestItem.DataKeys[gdProductTestItem.SelectedRowIndexArray[0]];
                    List<Dicttestitem> testproductListAll = BindTestItemProduct();//所有套餐
                    item = (from Dicttestitem in testproductListAll where Dicttestitem.Dicttestitemid == Convert.ToInt32(keys[0]) && (Dicttestitem.Testtype == "2" || Dicttestitem.Testtype == "3") select Dicttestitem).ToList<Dicttestitem>()[0];
                    List<Dicttestitem> testitemListAll = BindTestItemAll();//所有项目
                    List<Dictproductdetail> testproductAll = BindtestproductAll();//套餐明细
                    double?[] ss = (from bb in testproductAll where bb.Productid == item.Dicttestitemid select bb.Testgroupid).ToArray();
                    //BingMedical(ddlTestUnit, "3");
                    List<Dicttestitem_productdetail> productlistTo = (from aa in testitemListAll
                                                                      join dd in testproductAll
                                                                          on aa.Dicttestitemid equals dd.Testgroupid
                                                                      where dd.Productid == item.Dicttestitemid && ss.Contains(aa.Dicttestitemid) && (aa.Testtype == "0" || aa.Testtype == "1")
                                                                      select new Dicttestitem_productdetail
                                                                      {

                                                                          Dicttestitem = aa,
                                                                          Finalprice = dd.Finalprice,
                                                                          Issendouttest = dd.Issendouttest,
                                                                          Sendoutcustomerid = dd.Sendoutcustomerid
                                                                      }).Distinct().ToList();


                    gdProductIncludeTestItem.DataSource = productlistTo;
                    gdProductIncludeTestItem.DataBind();



                }
            }
            //else
            //{
            //    AddTestProduct();
            //}

        }
        #endregion

        #region 绑定下拉框
        public void BingMedical(System.Web.UI.WebControls.DropDownList ddlMedical, string type, double dictlabId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("value", type);
            ht.Add("dictlabId", dictlabId);
            IList<Dictcustomer> dictcustoms = dictCustomerService.GetDictCustomerListByType(ht).Where(c => c.Active == "1").ToList<Dictcustomer>();
            ddlMedical.DataTextField = "Customername";
            ddlMedical.DataValueField = "Dictcustomerid";
            ddlMedical.DataSource = dictcustoms;
            ddlMedical.DataBind();
        }
        public void BingMedical(ExtAspNet.DropDownList ddlMedical, string type, double dictlabId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("value", type);
            ht.Add("dictlabId", dictlabId);
            IList<Dictcustomer> dictcustoms = dictCustomerService.GetDictCustomerListByType(ht);
            ddlMedical.DataTextField = "Customername";
            ddlMedical.DataValueField = "Dictcustomerid";
            ddlMedical.DataSource = dictcustoms;
            ddlMedical.DataBind();
        }


        public void BingMedical(ExtAspNet.DropDownList ddlMedical, string type)
        {           
            IList<Dictcustomer> dictcustoms = dictCustomerService.GetDictCustomerListByType(type);
            ddlMedical.DataTextField = "Customername";
            ddlMedical.DataValueField = "Dictcustomerid";
            ddlMedical.DataSource = dictcustoms;
            ddlMedical.DataBind();
        }        
        #endregion

        #region 是否外包
        public bool CheckSendouttest(object cheks)
        {
            if (cheks == null || cheks.ToString() == null)
            {
                return false;
            }
            if ("1".Equals(cheks.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 项目类型
        public string CheckTestItemType(object itemtype)
        {

            if ("1".Equals(itemtype.ToString()))
            {
                return "组合";
            }
            else
            {
                return "单项";
            }
        }
        #endregion

        #region 套餐分页
        protected void gdProductTestItem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdProductTestItem.PageIndex = e.NewPageIndex;
            BindSearchData();
        }
        #endregion

        #region >>>未包含项绑定及分页
        private void BindGrid(string strid, string dictlabid)
        {
            double? id = null;
            if (ViewState["TestItemID"] != null)
            {
                id = Convert.ToDouble(ViewState["TestItemID"]);
            }
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdProductNotIncludeTestItem.PageIndex, gdProductNotIncludeTestItem.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(btnSearchNoIn.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btnSearchNoIn.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("testitemid", id);
            ht1.Add("strtestitemid", strid);
            ht1.Add("strdictlabid", dictlabid);

            //strtestitemid
            //设置总项数
            gdProductNotIncludeTestItem.RecordCount = testitemservice.GetDictproductPageLstCount(ht1);
            List<Dicttestitem> listNotIn = testitemservice.GetDictproductPageLst(ht1);
            gdProductNotIncludeTestItem.DataSource = listNotIn;
            gdProductNotIncludeTestItem.DataBind();
        }

        protected void gdProductNotIncludeTestItem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdProductNotIncludeTestItem.PageIndex = e.NewPageIndex;
            string strid = null;
            if (ViewState["strid"] != null)
            {
                strid = ViewState["strid"].ToString();
            }

            //publicP,表示公用套餐；unitP,表示单位套餐
            if (rdobtnPublicProduLst.SelectedValue == "unitP")
            {
                //根据体检单位查找客户信息
                Dictcustomer dictcustomer = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
                BindGrid(strid, dictcustomer.Dictlabid.ToString());
            }
            else
            {
                BindGrid(strid, null);
            }
        }
        #endregion

        #region 包含项行绑定
        protected void gdProductIncludeTestItem_RowDataBound1(object sender, GridRowEventArgs e)
        {
            try
            {
                Dicttestitem_productdetail a = e.DataItem as Dicttestitem_productdetail;
                System.Web.UI.WebControls.DropDownList ddloutcustomer = (System.Web.UI.WebControls.DropDownList)gdProductIncludeTestItem.Rows[e.RowIndex].FindControl("DropDownList1");
                System.Web.UI.WebControls.CheckBox chbSendouttest = (System.Web.UI.WebControls.CheckBox)gdProductIncludeTestItem.Rows[e.RowIndex].FindControl("chbSendouttest");
                if (a.Issendouttest == "1")
                {
                    //确定外包 外包单位下拉框才显示
                    ddloutcustomer.Visible = true;
                    chbSendouttest.Visible = true;
                    double dictlabId = Convert.ToDouble(this.ddlTestUnit.SelectedValue);
                    if (dictlabId != 0)
                    {
                        Dictcustomer dictCustomer = dictCustomerService.GetDictCustomerById(dictlabId);
                        BingMedical(ddloutcustomer, "1", Convert.ToDouble(dictCustomer.Dictlabid));//绑定外包单位下拉框
                    }
                    else
                    {
                        BingMedical(ddloutcustomer, "1", 0);//绑定外包单位下拉框
                    }
                    //如果是外包单位就选中对应的值
                    if (a.Sendoutcustomerid > 0)
                    {
                        ddloutcustomer.SelectedValue = a.Sendoutcustomerid.ToString();
                    }
                }
                else
                {
                    ddloutcustomer.Visible = false;
                    chbSendouttest.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        #endregion

        #region 体检单位对应的分点下的项目
        //体检单位对应的分点下的项目
        protected void ddlTestUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取体检单位信息
            Dictcustomer dictcustomer = dictCustomerService.GetDictCustomerById(Convert.ToDouble(ddlTestUnit.SelectedValue));
            if (dictcustomer != null)
            {
                BindGrid(null, dictcustomer.Dictlabid.ToString());
            }
        }
        #endregion
    }
}