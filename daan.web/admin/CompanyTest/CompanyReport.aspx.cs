using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using System.Data;
using daan.domain;
using daan.service.order;
using System.Collections;
using daan.service.report;
using daan.service;
using ExtAspNet;
using daan.service.login;
using daan.service.common;
using daan.util.Web;
using FastReport;
using daan.web.code;
namespace daan.web.admin.CompanyTest
{
    public partial class CompanyReport : PageBase
    {

        CommonReport commonReport = new CommonReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPage();
            }
        }
        #region tab3相关逻辑代码
        /// <summary>
        /// 删除体检者推荐项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelgdOrdernexttest_Click(object sender, EventArgs e)
        {
            CustomernexttestService _customernexttestservice = new CustomernexttestService();

            if (string.IsNullOrEmpty(dropDictcustomer.SelectedValue))
            {
                return;
            }

            bool flag = true;
            string errname = string.Empty;
            foreach (var item in gdOrdernexttest.SelectedRowIndexArray)
            {
                string id = gdOrdernexttest.DataKeys[item][0].ToString();
                string testname = gdOrdernexttest.DataKeys[item][1].ToString();
                if (!_customernexttestservice.DeleteCustomernexttest(id))
                {
                    flag = false;
                    errname += testname + ",";
                }
                else
                {
                    //记录日志
                    _customernexttestservice.AddMaintenanceLog("Customernexttest", Convert.ToInt32(id), null, "删除", testname, testname, "团检报告");
                }
            }
            if (!flag)
            {
                MessageBoxShow(string.Format("项目[{0}]删除失败", errname.TrimEnd(',')));
            }
            InitgdOrdernexttest(dropDictcustomer.SelectedValue);
        }

        /// <summary>
        /// 查询推荐项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbStrKey_Trigger2Click(object sender, EventArgs e)
        {
            tbStrKey_TextChanged(null, null);
            tbStrKey.ShowTrigger1 = true;
        }

        /// <summary>
        /// 执行清空动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbStrKey_Trigger1Click(object sender, EventArgs e)
        {
            tbStrKey.Text = "";
            tbStrKey.ShowTrigger1 = false;
        }

        /// <summary>
        /// 查询备选项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbStrKey_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dropDictcustomer.SelectedText))//没有选择体检单位的时候不加载任何数据
            {
                return;
            }
            InitgdDicttestitem(tbStrKey.Text);
        }
        /// <summary>
        /// 加载数据进gdDicttestitem表中
        /// </summary>
        void InitgdDicttestitem(string strKey)
        {
            DicttestitemService dictserver = new DicttestitemService();
            // LoginService loginservice = new LoginService();
            PageUtil pageUtil = new PageUtil(gdDicttestitem.PageIndex, gdDicttestitem.PageSize);
            int start = pageUtil.GetPageStartNum() - 1;
            int end = pageUtil.GetPageEndNum();
            //从缓存中取数据，取检验项目(dicttestitem)
            var TestItemList = dictserver.GetInitgdGroupTestByLabId(DropDictLab.SelectedValue, start, end, strKey);//取分点项目
            gdDicttestitem.RecordCount = dictserver.GetInitgdGroupTestByLabIdCount(DropDictLab.SelectedValue, strKey);//总数
            gdDicttestitem.DataSource = TestItemList;
            gdDicttestitem.DataBind();
            // loginservice.GetLoginDicttestitemList().Where(c => (int.Parse(c.Testtype) < 3));//项目字典表
            //TestItemList.Join(gdOrdernexttest.DataKeys[2],a=>a.Dicttestitemid,b=>double.Parse(b.ToString()),
            //if (strKey.Length == 0)//不进行筛选数据时
            //{
            //    gdDicttestitem.RecordCount = TestItemList.Count();
            //    gdDicttestitem.DataSource = TestItemList.Skip(start).Take(end - start);
            //    gdDicttestitem.DataBind();
            //}
            //else
            //{
            //    //有关键字时，根据项目名称，快捷录入码，英文名进行筛选
            //    var p = TestItemList.Where(c => (ConVertClass(c).Testname.Contains(tbStrKey.Text)) || (ConVertClass(c).Fastcode.Contains(tbStrKey.Text)) ||
            //        (ConVertClass(c).Engname.Contains(tbStrKey.Text)));
            //    gdDicttestitem.RecordCount = p.Count();
            //    gdDicttestitem.DataSource = p.Skip(start).Take(end - start);
            //    gdDicttestitem.DataBind();
            //}

        }

        /// <summary>
        /// 备选项目分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdDicttestitem_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdDicttestitem.PageIndex = e.NewPageIndex;
            InitgdDicttestitem(tbStrKey.Text);
        }

        /// <summary>
        /// 防止为空
        /// </summary>
        /// <param name="dicttest"></param>
        /// <returns></returns>
        Dicttestitem ConVertClass(Dicttestitem dicttestitem)
        {
            dicttestitem.Testname = (dicttestitem.Testname == null ? "" : dicttestitem.Testname);
            dicttestitem.Fastcode = (dicttestitem.Fastcode == null ? "" : dicttestitem.Fastcode);
            dicttestitem.Engname = (dicttestitem.Engname == null ? "" : dicttestitem.Engname);
            return dicttestitem;
        }
        /// <summary>
        /// 添加推荐项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveDicttestitem_Click(object sender, EventArgs e)
        {
            if (dpNextRERUNDATE.Text == string.Empty || dpNextRERUNDATE.SelectedDate == null)
            {
                MessageBoxShow("请选择复查时间");
                return;
            }
            if (string.IsNullOrEmpty(dropDictcustomer.SelectedValue))
            {
                MessageBoxShow("请选择[体检单位选择]的选项");
                return;
            }

            CustomernexttestService customernexttestService = new CustomernexttestService();

            var selectArray = gdDicttestitem.SelectedRowIndexArray;
            if (selectArray.Length == 0)
            {
                MessageBoxShow("请选择备选项目");
                return;
            }

            string testnames = string.Empty;
            foreach (var item in selectArray)
            {
                Customernexttest customernexttest = new Customernexttest();
                customernexttest.Customernexttestid = customernexttestService.getSeqID("seq_customernexttest");
                customernexttest.Dictcustomerid = double.Parse(dropDictcustomer.SelectedValue);
                customernexttest.Dicttestitemid = double.Parse(gdDicttestitem.DataKeys[item][0].ToString());
                if (gdDicttestitem.DataKeys[item][1] != null)
                    customernexttest.Engname = gdDicttestitem.DataKeys[item][1].ToString();
                else
                    customernexttest.Engname = "";
                if (gdDicttestitem.DataKeys[item][3] != null)
                    customernexttest.Testcode = gdDicttestitem.DataKeys[item][3].ToString();
                else
                    customernexttest.Testcode = "";
                if (gdDicttestitem.DataKeys[item][4] != null)
                    customernexttest.Testname = gdDicttestitem.DataKeys[item][4].ToString();
                else
                    customernexttest.Testname = "";
                customernexttest.Orderyear = dpThisyearFrom.SelectedDate.Value.ToString("yyyy");
                customernexttest.Rerundate = dpNextRERUNDATE.SelectedDate;
                string count = customernexttestService.CountForCustomernexttest(customernexttest);
                if (int.Parse(count) == 0)
                {
                    if (customernexttestService.InsertCustomernexttest(customernexttest))
                    {
                        //记录日志
                        List<LogInfo> logLst = CustomernexttestService.getLogInfo<Customernexttest>(new Customernexttest(), customernexttest);
                        Dictmember _member = ViewState["oldmember"] as Dictmember;
                        customernexttestService.AddMaintenanceLog("Customernexttest", customernexttest.Customernexttestid, logLst, "添加", customernexttest.Testname, customernexttest.Testcode, "团检报告");

                    }
                }
                else
                {
                    testnames += customernexttest.Testname + ",";
                }
            }
            if (testnames != string.Empty)
            {
                MessageBoxShow(string.Format("项目[{0}]已经存在[已选项目]列表！", testnames.TrimEnd(',')));
            }
            InitgdOrdernexttest(dropDictcustomer.SelectedValue);
        }

        /// <summary>
        /// 根据订单号加载体检者推荐项目
        /// </summary>
        /// <param name="ordernum"></param>
        void InitgdOrdernexttest(string dictcustomerid)
        {
            gdOrdernexttest.DataSource = GetdtCustomernexttest();
            gdOrdernexttest.DataBind();
        }


        #endregion


        #region 左边代码逻辑
        /// <summary>
        /// 查询grid表格数据
        /// </summary>
        void BindGrid()
        {

            Hashtable ht = new Hashtable();
            ht.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
            gdOrderdiagnosis.DataSource = orderdiagnosisService.GetDiseasePeople(ht);
            gdOrderdiagnosis.DataBind();
            //如果已给体检单位做过结果评价，则在前面打勾
            List<int> temp = new List<int>();
            for (int i = 0; i < gdOrderdiagnosis.DataKeys.Count; i++)
            {
                var checkstatus = gdOrderdiagnosis.DataKeys[0][1];
                if (checkstatus.ToString() == "1")
                {
                    temp.Add(i);
                }
            }
            gdOrderdiagnosis.SelectedRowIndexArray = temp.ToArray();
        }


        /// <summary>
        /// 初始化缓存中的数据
        /// </summary>
        IList<Dictcustomer> InitCache()
        {
            LoginService loginService = new LoginService();
            return loginService.GetDictcustomer();
        }

        /// <summary>
        /// 初始化下拉列表的数据,体检单位
        /// </summary>
        void BindDrop()
        {
            InitCache();
            var dictcustoms = InitCache();
            if (DropDictLab.SelectedValue != null)
            {
                //var custlist = dictcustoms.Where<Dictcustomer>(c => c.Dictlabid == Convert.ToDouble(DropDictLab.SelectedValue) && c.Customertype == "0" && c.Active == "1");
                //dropDictcustomer.DataSource = custlist;
                //dropDictcustomer.DataTextField = "Customername";
                //dropDictcustomer.DataValueField = "Dictcustomerid";
                //dropDictcustomer.DataBind();
                //dropCommetDictcustomer.DataSource = custlist;
                //dropCommetDictcustomer.DataTextField = "Customername";
                //dropCommetDictcustomer.DataValueField = "Dictcustomerid";
                //dropCommetDictcustomer.DataBind();
                string labid=DropDictLab.SelectedValue.ToString();
                DropDictcustomerBinder(dropDictcustomer, labid, false);
                DropDictcustomerBinder(dropCommetDictcustomer, labid, false);

                if (dropCommetDictcustomer.SelectedValue != null)
                {
                    gdCustomervaliddiagnosisDataBind(dropCommetDictcustomer.SelectedValue);
                }

                if (dropDictcustomer.SelectedValue != null)
                {
                    //加载已选项目
                    InitgdOrdernexttest(dropDictcustomer.SelectedValue);
                    //推荐项目
                    InitgdDicttestitem(tbStrKey.Text); ;//加载备选项目
                }

            }

        }

        ///选择单位查找
        protected void dropCommetDictcustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdCustomervaliddiagnosisDataBind(dropCommetDictcustomer.SelectedValue);
        }

        ///选择分点
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDrop();
        }


        //绑定坐标已有记录列表
        private void gdCustomervaliddiagnosisDataBind(string Dictcustomerid)
        {
            Customerresultcomment cus = new Customerresultcomment();
            cus.Dictcustomerid = Convert.ToDouble(Dictcustomerid);
            gdCustomervaliddiagnosis.DataSource = new CustomerresultcommentService().SelectCustomerresultcomment(cus);
            gdCustomervaliddiagnosis.DataBind();
        }

        /// <summary>
        /// 根据主键得到体检单位实体
        /// </summary>
        /// <param name="Dictcustomerid"></param>
        /// <returns></returns>
        Dictcustomer GetDictcustomer(string Dictcustomerid)
        {

            var dictcustoms = InitCache();
            return dictcustoms.First(c => (double)c.Dictcustomerid == double.Parse(Dictcustomerid));

        }

        #endregion

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        void InitPage()
        {
            //默认查询去年数据
            dplastyearFrom.SelectedDate = DateTime.Now.AddYears(-1).AddDays(-1);
            dplastyearTo.SelectedDate = DateTime.Now.AddYears(-1);
            dpThisyearFrom.SelectedDate = DateTime.Now.AddDays(-1);
            dpThisyearTo.SelectedDate = DateTime.Now;

            //绑定分点
            base.DDLDictLabBinder(DropDictLab, true);
            //推荐项目
            InitgdDicttestitem(tbStrKey.Text); ;//加载备选项目
            BindDrop();
            ViewState.Add("printstatue", 0);
        }

        /// <summary>
        /// 查询单位异常情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchMethod();
        }

        private void SearchMethod()
        {
            string strmsg = CheckWhere();
            if (strmsg != string.Empty) { MessageBoxShow(strmsg); return; }

            BindGrid();
            BindTabData();
            gdDicttestitem.SelectedRowIndexArray = null;
            gdOrdernexttest.SelectedRowIndexArray = null;
        }

        private string CheckWhere()
        {
            DateTime outtime;
            string msg = string.Empty;

            if (this.dplastyearFrom.Text == "" || this.dplastyearTo.Text == "" || !DateTime.TryParse(dplastyearFrom.Text, out outtime) || !DateTime.TryParse(dplastyearTo.Text, out outtime))
            {

                msg = "请输入正确的上年度开始时间及结束时间查询";
            }
            else
            {
                if (this.dplastyearFrom.SelectedDate <= this.dplastyearTo.SelectedDate)
                {
                    var timespan_last = dplastyearTo.SelectedDate.Value - dplastyearFrom.SelectedDate.Value;
                    if (timespan_last.Days > 365)
                    {
                        msg = "上年度时间跨度不可以超过1年";
                    }
                }
                else
                {

                    msg = "上年度结束时间应大于开始时间！";
                }
            }
            if (this.dpThisyearTo.Text == "" || this.dpThisyearFrom.Text == "" || !DateTime.TryParse(dpThisyearFrom.Text, out outtime) || !DateTime.TryParse(dpThisyearTo.Text, out outtime))
            {

                msg = "请输入正确的本年度开始时间及结束时间查询！";
            }
            else
            {
                if (this.dpThisyearFrom.SelectedDate <= this.dpThisyearTo.SelectedDate)
                {
                    var timespan_last = dpThisyearTo.SelectedDate.Value - dpThisyearFrom.SelectedDate.Value;
                    if (timespan_last.Days > 365)
                    {
                        msg = "本年度时间跨度不可以超过1年";
                    }
                }
                else
                {

                    msg = "本年度结束时间应大于开始时间！";
                }
            }

            return msg;
        }

        /// <summary>
        /// 保存建议与解析事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (taContent.Text.Length == 0)
            {
                MessageBoxShow("没有建议,不能保存！");
                return;
            }
            if (dropDictcustomer.SelectedValue == null)
            {
                MessageBoxShow("没有选择单位,不能保存！");
                return;
            }
            string strmsg = CheckWhere();
            if (strmsg != string.Empty) { MessageBoxShow(strmsg); return; }
            Customerresultcomment customerresultcomment = new Customerresultcomment();
            customerresultcomment.Dictcustomerid = double.Parse(dropDictcustomer.SelectedValue);
            customerresultcomment.Resultcomment = taContent.Text;
            customerresultcomment.Ordersyear = dpThisyearFrom.SelectedDate.Value.ToString("yyyy");
            customerresultcomment.Laststartdate = dplastyearFrom.Text;
            customerresultcomment.Lastenddate = dplastyearTo.Text;
            customerresultcomment.Thisstartdate = dpThisyearFrom.Text;
            customerresultcomment.Thisenddate = dpThisyearTo.Text;
            CustomervaliddiagnosisService customervaliddiagnosisService = new CustomervaliddiagnosisService();
            CustomerresultcommentService customerresultcommentService = new CustomerresultcommentService();
            bool b = true;
            if (customerresultcommentService.CheckCustomerresultcomment(customerresultcomment))
            {
                b = customerresultcommentService.UpdateCustomerresultcomment(customerresultcomment);
            }
            else
            {
                customerresultcomment.Ordersyear = customerresultcomment.Ordersyear + "_" + customerresultcommentService.SelectCustomerresultcommentYearCount(new Hashtable() { { "custid", customerresultcomment.Dictcustomerid }, { "yearstr", customerresultcomment.Ordersyear } }).ToString("00");
                b = customerresultcommentService.InsertCustomerresultcomment(customerresultcomment);
            }

            if (!b) { MessageBoxShow("保存失败，请刷新重试！"); }
            gdCustomervaliddiagnosisDataBind(customerresultcomment.Dictcustomerid.ToString());
            //先把本年度旧的诊断建议删除
            Hashtable ht = new Hashtable();
            ht.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            customervaliddiagnosisService.DeleteCustomervaliddiagnosis(ht);
            foreach (var item in gdOrderdiagnosis.SelectedRowIndexArray)
            {
                Customervaliddiagnosis customervaliddiagnosis = new Customervaliddiagnosis();
                customervaliddiagnosis.Dictcustomerid = double.Parse(dropDictcustomer.SelectedValue);
                customervaliddiagnosis.Ordersyear = dpThisyearFrom.SelectedDate.Value.ToString("yyyy");
                customervaliddiagnosis.Dictdiagnosisid = double.Parse(gdOrderdiagnosis.DataKeys[item][0].ToString());
                customervaliddiagnosisService.InsertCustomervaliddiagnosis(customervaliddiagnosis);
            }


        }
        /// <summary>
        /// 取得所有诊断建议字典
        /// </summary>
        /// <returns></returns>
        IEnumerable<Dictdiagnosis> GetDiagnosisAll(double? dictdiagnosisid)
        {
            LoginService loginservice = new LoginService();
            return loginservice.GetLoginDictdiagnosisresultList().Where(c => c.Dictdiagnosisid == dictdiagnosisid);
        }

        /// <summary>
        /// 生成异常建议，事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gdOrderdiagnosis.SelectedRowIndexArray.Length; i++)
            {
                double? dictdiagnosisid = double.Parse(gdOrderdiagnosis.DataKeys[i][0].ToString());
                var dictdiagnosisList = GetDiagnosisAll(dictdiagnosisid).First();

                taContent.Text += (dictdiagnosisList.Diagnosisname + (char)10);
                taContent.Text += ("【解释】" + (char)10);
                taContent.Text += (dictdiagnosisList.Diseasedescription + (char)10);
                taContent.Text += ("【建议】" + (char)10);
                taContent.Text += (dictdiagnosisList.Suggestion + (char)10);
            }

        }

        /// <summary>
        /// 加载tab数据
        /// </summary>
        void BindTabData()
        {
            Customerresultcomment cus = GetreportDate();//诊断建议
            if (cus == null)
            {
                taContent.Text = "";
            }
            else
            {
                taContent.Text = cus.Resultcomment;
            }

            ////报表界面
            //DataSet data = InitTabReport();//取报告数据，数据存放在sesssion当中
            //if (data == null)//无数据时
            //{
            //    tabReport.IFrameUrl = "../report/RepShowView.aspx?reportType=2&resultType=no";
            //    // Tab2.IFrameUrl = "../report/RepShowView.aspx?reportType=2&resultType=no";
            //}
            //else//有数据时
            //{
            //    Session["GroupDataSet"] = data;
            //    // Tab2.IFrameUrl = "../report/RepShowView.aspx?reportType=2&resultType=no";
            //    tabReport.IFrameUrl = "../report/RepShowView.aspx?reportType=2&resultType=yes";
            //    tabReport.RefreshIFrame();
            //    //Tab2.RefreshIFrame();

            //}
            InitgdOrdernexttest(dropDictcustomer.SelectedValue);//加载相关单位的已选项目
        }
        #region >>>> zhouy 报告预览
        /// <summary>
        /// 报告预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportView_Click(object sender, EventArgs e)
        {            
            string Url = "../report/RepShowView.aspx?reportType=2&resultType=no";
            //报表界面
            DataSet data = InitTabReport();//取报告数据，数据存放在sesssion当中
            if (data != null)//无数据时
            {            
                Session["GroupDataSet"] = data;
                Url = "../report/RepShowView.aspx?reportType=2&resultType=yes";
            }
            PageContext.RegisterStartupScript(WinReportView.GetShowReference(Url));
        }

        #endregion

        #region 报表数据

        /// <summary>
        /// 推荐项目
        /// </summary>
        /// <returns></returns>
        DataTable GetdtCustomernexttest()
        {
            CustomernexttestService customernexttestService = new CustomernexttestService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            ht1.Add("Dictcustomerid", dropDictcustomer.SelectedValue);
            return customernexttestService.SelectCustomernexttest(ht1);
        }

        /// <summary>
        /// 报告标题
        /// </summary>
        /// <returns></returns>
        DataTable GetdtTitle()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("titleName");
            dt.TableName = "dtTitle";
            LoginService login = new LoginService();
            DataRow row = dt.NewRow();
            var dictcustomer = login.GetDictcustomer().Where(c => c.Dictcustomerid == double.Parse(dropDictcustomer.SelectedValue));
            if (dictcustomer.Count() > 0)
            {
                var one = dictcustomer.First();
                if (one.Reporttitle != null)
                {
                    row["titleName"] = one.Reporttitle;
                }
                else
                {
                    OrdersService ordersService = new OrdersService();
                    Hashtable ht1 = new Hashtable();
                    ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                    ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    var dt1 = ordersService.GetdtTitle(ht1);
                    foreach (DataRow item in dt1.Rows)
                    {
                        row["titleName"] = item[0];
                    }

                }
            }
            dt.Rows.Add(row);
            return dt;
        }

        /// <summary>
        /// 体检基本信息
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="Dictcustomerid"></param>
        /// <returns></returns>
        DataTable GetdtGroupBasicMessage()
        {
            OrdersService ordersService = new OrdersService();
            DataTable dt = new DataTable();
            dt.Columns.Add("StartDate");//开始时间
            dt.Columns.Add("EndDate");//截止日期
            dt.Columns.Add("PlanPeople");//计划参加人数
            dt.Columns.Add("ActualPeople");//实际参加人数
            dt.Columns.Add("APRatio");//实际参加率
            dt.Columns.Add("FJoin");//男性参加人数
            dt.Columns.Add("MJoin");//女性参加人数
            dt.Columns.Add("FJoinRatio");//男性参加比例
            dt.Columns.Add("MJoinRatio");//女性参加比例
            dt.Columns.Add("MainTestItem");//主要项目
            DataRow row = dt.NewRow();
            //时间
            row["StartDate"] = dpThisyearFrom.SelectedDate;
            row["EndDate"] = dpThisyearTo.SelectedDate;
            //获取男女实际人数
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            DataTable dt1 = ordersService.GetFMRitol(ht1);
            if (dt1 != null)
            {
                //男女数量，比例
                // if (dt1.Rows[0]["sex"].ToString() == "M")
                //  {
                row["MJoin"] = dt1.Rows[0]["numpeople"];
                row["FJoin"] = dt1.Rows[1]["numpeople"];
                row["MJoinRatio"] = dt1.Rows[0]["opercentM"];
                row["FJoinRatio"] = dt1.Rows[0]["opercentF"];
                //  }
                //  else
                //  {
                //      row["MJoin"] = dt1.Rows[1]["numpeople"];
                //        row["FJoin"] = dt1.Rows[0]["numpeople"];
                //        row["MJoinRatio"] = dt1.Rows[1]["opercent"];
                //        row["FJoinRatio"] = dt1.Rows[0]["opercent"];
                //    }
                row["PlanPeople"] = dt1.Rows[0]["PlanPeople"];
                row["ActualPeople"] = dt1.Rows[0]["ActualPeople"];
                row["APRatio"] = dt1.Rows[0]["APRatio"];
                OrdergrouptestService ordergrouptestService = new OrdergrouptestService();
                //检查项目
                var dt2 = ordergrouptestService.GetMainTestItem(ht1);
                if (dt2.Rows.Count != 0)
                {
                    string testItem = null;
                    foreach (DataRow item in dt2.Rows)
                    {
                        testItem += item[0].ToString();
                    }
                    row["MainTestItem"] = testItem;
                }
                dt.Rows.Add(row);
                return dt;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 得到报告单首页
        /// </summary>
        /// <param name="Dictcustomerid"></param>
        /// <returns></returns>
        DataTable GetdtGroupTitle()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Customercode");
            dt.Columns.Add("Customername");
            dt.Columns.Add("Enterdate");
            dt.Columns.Add("Commentdate");
            DataRow row = dt.NewRow();
            var dictcustomer = GetDictcustomer(dropDictcustomer.SelectedValue);
            row["Customercode"] = dictcustomer.Customercode;
            row["Customername"] = dictcustomer.Customername;
            //体检日期
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            OrdersService ordersService = new OrdersService();
            DataTable dt1 = ordersService.GetEnterdate(ht1);
            row["Enterdate"] = dt1.Rows[0][0];
            //报告日期
            Customerresultcomment cus = GetreportDate();
            if (cus == null)
            {
                row["Commentdate"] = null;
            }
            else
            {
                row["Commentdate"] = cus.Commentdate;
            }

            dt.Rows.Add(row);
            return dt;
        }

        /// <summary>
        /// 报告日期
        /// </summary>
        /// <returns></returns>
        private Customerresultcomment GetreportDate()
        {
            CustomerresultcommentService customerresultcommentService = new CustomerresultcommentService();
            Customerresultcomment cus = new Customerresultcomment();
            cus.Dictcustomerid = Convert.ToDouble(dropDictcustomer.SelectedValue);
            cus.Laststartdate = dplastyearFrom.Text;
            cus.Lastenddate = dplastyearTo.Text;
            cus.Thisstartdate = dpThisyearFrom.Text;
            cus.Thisenddate = dpThisyearTo.Text;
            //Hashtable ht2 = new Hashtable();
            //ht2.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            //ht2.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            cus = customerresultcommentService.GetreportDate(cus);
            return cus;
        }

        /// <summary>
        /// 参检人员年龄及性别,柱形图
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="Dictcustomerid"></param>
        /// <returns></returns>
        DataTable GetdtGroupAgeSexColumnResult()
        {

            OrdersService ordersService = new OrdersService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            DataTable dt = ordersService.GetAgeRegion(ht1);
            foreach (DataRow item in dt.Rows)
            {
                switch (item["ageregion"].ToString())
                {
                    case "1":
                        {
                            item["ageregion"] = "<20";
                        };
                        break;
                    case "2":
                        {
                            item["ageregion"] = "20-29";
                        }
                        break;
                    case "3":
                        {
                            item["ageregion"] = "30-39";
                        }
                        break;
                    case "4":
                        {
                            item["ageregion"] = "40-49";
                        }
                        break;
                    case "5":
                        {
                            item["ageregion"] = "50-59";
                        }
                        break;
                    case "6":
                        {
                            item["ageregion"] = "60-69";
                        }
                        break;
                    case "7":
                        {
                            item["ageregion"] = "70-79";
                        }
                        break;
                    case "8":
                        {
                            item["ageregion"] = ">80";
                        };
                        break;
                }
            }
            return dt;
        }

        /// <summary>
        /// 平均年龄,平均年龄之上的人数，之下的人数,左边
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="Dictcustomerid"></param>
        /// <returns></returns>
        DataTable GetdtGroupAgeSexCakeResultLeft()
        {
            try
            {
                OrdersService ordersService = new OrdersService();
                DataTable dt = new DataTable();
                dt.Columns.Add("AverageAgeType");
                dt.Columns.Add("AverageAgeUpNumber");
                dt.Columns.Add("AverageAgeDownNumber");
                DataRow row = dt.NewRow();

                Hashtable ht1 = new Hashtable();
                ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                DataTable dt1 = ordersService.GetAvgAge(ht1);
                if (dt1.Rows[0][0] is DBNull)
                {
                    return null;
                }
                string avgage = dt1.Rows[0][0].ToString();
                ht1.Add("AvgAge", avgage);
                DataTable dt2 = ordersService.GetAvgAgeLow(ht1);
                DataTable dt3 = ordersService.GetAvgAgeHight(ht1);
                //平均年龄
                row["AverageAgeType"] = avgage;
                row["AverageAgeUpNumber"] = dt3.Rows[0][0];
                row["AverageAgeDownNumber"] = dt2.Rows[0][0];
                dt.Rows.Add(row);

                return dt;
            }
            catch (Exception ee)
            {

                return null;
            }

        }

        /// <summary>
        /// 平均年龄之上的人数，之下的人数,右边
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupAgeSexCakeResultRight(DataTable dt)
        {
            try
            {

                DataTable dt3 = new DataTable();
                dt3.Columns.Add("AverageAgeType");
                dt3.Columns.Add("number");
                DataRow row = dt3.NewRow();
                row["AverageAgeType"] = "平均年龄(含)以上";
                row["number"] = dt.Rows[0]["AverageAgeUpNumber"];
                dt3.Rows.Add(row);
                DataRow row1 = dt3.NewRow();
                row1["AverageAgeType"] = "平均年龄以下";
                row1["number"] = dt.Rows[0]["AverageAgeDownNumber"];
                dt3.Rows.Add(row1);
                return dt3;
            }
            catch (Exception ee)
            {

                return null;
            }

        }

        /// <summary>
        /// 健康问题分析与建议
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupHealthSuggest()
        {
            CustomerresultcommentService customerresultcommentService = new CustomerresultcommentService();
            //Hashtable ht1 = new Hashtable();
            //ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            //ht1.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            Customerresultcomment cus = new Customerresultcomment();
            cus.Dictcustomerid = Convert.ToDouble(dropDictcustomer.SelectedValue);
            cus.Laststartdate = dplastyearFrom.Text;
            cus.Lastenddate = dplastyearTo.Text;
            cus.Thisstartdate = dpThisyearFrom.Text;
            cus.Thisenddate = dpThisyearTo.Text;
            cus = customerresultcommentService.GetreportDate(cus);
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("text");
            if (cus != null)
            {
                DataRow row = dt2.NewRow();
                row["text"] = cus.Resultcomment;
                dt2.Rows.Add(row);
            }
            return dt2;
        }


        /// <summary>
        /// 已预约未体检人员，团检报告
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupAnExamination()
        {

            OrdersService ordersService = new OrdersService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            return ordersService.GetDonotJoinTest(ht1);
        }

        /// <summary>
        /// 疾病数据对比,团检报告
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupChronicDiseasesCompare(ParamStatus.OrderDiagnosisType orderDiagnosisType)
        {
            try
            {
                OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
                Hashtable ht1 = new Hashtable();
                ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("diagnosistype", (int)orderDiagnosisType);
                ht1.Add("lastStartDate", dplastyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("lastEndDate", dplastyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                return orderdiagnosisService.DiseaseDataCompare(ht1);
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        /// <summary>
        /// 团检报告,体检异常统计表，只统计医生选择的异常
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupHealthCompare()
        {
            OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            return orderdiagnosisService.GetdtGroupHealthCompare(ht1);
        }
        /// <summary>
        /// 全体员工体检结果汇总,团检
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupAllResult()
        {
            OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("ordersyear", dpThisyearFrom.SelectedDate.Value.ToString("yyyy"));
            return orderdiagnosisService.GetdtGroupAllResult(ht1);
        }
        /// <summary>
        /// 已体检而未完成项目，团检报告
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupAnFinish()
        {
            OrderTestService orderTestService = new service.order.OrderTestService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            return orderTestService.GetdtGroupAnFinish(ht1);
        }

        /// <summary>
        /// 查询体检结果的前10种异常，团检报告
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="Dictcustomerid"></param>
        /// <param name="SexStatus"></param>
        /// <returns></returns>
        DataTable GetdtGroupAllDiseasePercent(string SexStatus)
        {
            try
            {
                OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
                Hashtable ht1 = new Hashtable();
                ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                switch (SexStatus)
                {
                    case "MStatus"://男
                        {
                            ht1.Add("MStatus", "MStatus");
                            ht1.Add("FStatus", null);
                            ht1.Add("All", null);
                        };
                        break;
                    case "FStatus"://女
                        {
                            ht1.Add("MStatus", null);
                            ht1.Add("FStatus", "FStatus");
                            ht1.Add("All", null);
                        };
                        break;
                    case "All"://全部
                        {
                            ht1.Add("MStatus", null);
                            ht1.Add("FStatus", null);
                            ht1.Add("All", "All");
                        };
                        break;

                }
                return orderdiagnosisService.GetTopOrderdiagnosis(ht1);

            }
            catch (Exception ee)
            {

                return null;
            }
        }


        /// <summary>
        /// 团检，重要指标检查结果查询,单项
        /// </summary>
        /// <returns></returns>
        DataTable DataForImporttantTestItem()
        {
            OrderTestService orderTestService = new service.order.OrderTestService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            return orderTestService.DataForImporttantTestItem(ht1);
        }

        /// <summary>
        /// 团检，重要指标检查结果查询,双项
        /// </summary>
        /// <returns></returns>
        DataTable DataForImporttantTestItemTwo()
        {
            OrderTestService orderTestService = new service.order.OrderTestService();
            Hashtable ht1 = new Hashtable();
            ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
            return orderTestService.DataForImporttantTestItemTwo(ht1);
        }

        /// <summary>
        /// 团检，重要指标检查结果详细单一查询,单项
        /// </summary>
        /// <returns></returns>
        DataTable DataForOneImporttantTestItem(string dicttestitemid)
        {
            try
            {
                OrderTestService orderTestService = new service.order.OrderTestService();
                Hashtable ht1 = new Hashtable();
                ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("dicttestitemid", dicttestitemid);
                return orderTestService.DataForOneImporttantTestItem(ht1);
            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// 团检，重要指标检查结果详细单一查询,双项
        /// </summary>
        /// <returns></returns>
        DataTable DataForTwoImporttantTestItem(string dictgroupid)
        {
            try
            {
                OrderTestService orderTestService = new service.order.OrderTestService();
                Hashtable ht1 = new Hashtable();
                ht1.Add("dictcustomerid", dropDictcustomer.SelectedValue);
                ht1.Add("StartDate", dpThisyearFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("EndDate", dpThisyearTo.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("dictgroupid", dictgroupid);
                return orderTestService.DataForTwoImporttantTestItem(ht1);
            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// 团检，重要指标检查结果,统计图数据源,双项
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetdtGroupImportResultChart(DataTable dt, string testName)
        {
            //dt.Columns.RemoveAt(4);
            // dt.Columns.RemoveAt(2);
            //    dt.Columns["numpercent"].ColumnName = "NUMCOUNT";//由于修改报告模板麻烦  将此字段名更改 
            DataColumn col = new DataColumn();
            col.ColumnName = "testName";
            col.DefaultValue = testName;
            dt.Columns.Add(col);
            return dt;
        }

        /// <summary>
        /// 团检，重要指标检查结果,统计图数据源,单项
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        DataTable GetdtGroupImportResultChart(DataTable dt, string testName, string testType)
        {
            DataTable result = new DataTable();
            result.Columns.Add("resultType");
            result.Columns.Add("numCount");
            result.Columns.Add("testType");
            result.Columns.Add("testName");
            result.Columns["testType"].DefaultValue = testName;
            result.Columns["testName"].DefaultValue = testType;
            foreach (DataRow row in dt.Rows)
            {
                DataRow newrow = result.NewRow();
                newrow["resultType"] = row[0];
                newrow["numCount"] = row[1];
                result.Rows.Add(newrow);
            }
            return result;
        }

        /// <summary>
        /// 重要指标，表格数据,双项
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupImportTableTwo(DataTable dt, int type)
        {
            DataTable dtGroupImportTable1 = new DataTable();
            dtGroupImportTable1.Columns.Add("remark", typeof(string));//说明
            dtGroupImportTable1.Columns.Add("numCount1", typeof(string));//类型1的人数
            dtGroupImportTable1.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
            dtGroupImportTable1.Columns.Add("numCount2", typeof(string));//类型2的人数
            dtGroupImportTable1.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
            dtGroupImportTable1.Columns.Add("numCount3", typeof(string));//类型3的人数
            dtGroupImportTable1.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
            dtGroupImportTable1.Columns.Add("numCount4", typeof(string));//类型4的人数
            dtGroupImportTable1.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            //去除相同项
            List<string> resultTypes = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                if (resultTypes.Count(c => c == item["resultType"].ToString()) == 0)
                {
                    resultTypes.Add(item["resultType"].ToString());
                }
            }
            List<string> testTypes = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                if (testTypes.Count(c => c == item["testType"].ToString()) == 0)
                {
                    testTypes.Add(item["testType"].ToString());
                }
            }
            if (type == 1)
            {
                //表头
                DataRow row = dtGroupImportTable1.NewRow();
                row["remark"] = "总人数";
                for (int i = 0; ((i < 4) && (i < resultTypes.Count)); i++)
                {
                    row[(i * 2 + 1)] = resultTypes[i];
                }
                dtGroupImportTable1.Rows.Add(row);
            }
            else
            {
                //第二表格，头
                var row1 = dtGroupImportTable1.NewRow();
                row1["remark"] = dt.Rows[0]["sumcount"];
                row1["numCount1"] = "人次";
                row1["numPercent1"] = "百分比";
                row1["numCount2"] = "人次";
                row1["numPercent2"] = "百分比";
                row1["numCount3"] = "人次";
                row1["numPercent3"] = "百分比";
                row1["numCount4"] = "人次";
                row1["numPercent4"] = "百分比";
                dtGroupImportTable1.Rows.Add(row1);
                for (int w = 0; w < testTypes.Count; w++)
                {
                    var row2 = dtGroupImportTable1.NewRow();
                    row2["remark"] = testTypes[w];
                    int k = 1;
                    var tempArray = dt.Select("testType='" + testTypes[w] + "'");
                    for (int i = 0; ((i < 4) && (i < tempArray.Length)); i++)
                    {
                        for (int j = 2; j < 4; j++)
                        {
                            row2[k] = tempArray[i][j].ToString();
                            k++;
                        }
                    }
                    dtGroupImportTable1.Rows.Add(row2);
                }


            }


            return dtGroupImportTable1;
        }

        /// <summary>
        /// 重要指标，表格数据,单项
        /// </summary>
        /// <returns></returns>
        DataTable GetdtGroupImportTableOne(DataTable dt, int type)
        {
            DataTable dtGroupImportTable1 = new DataTable();
            dtGroupImportTable1.Columns.Add("remark", typeof(string));//说明
            dtGroupImportTable1.Columns.Add("numCount1", typeof(string));//类型1的人数
            dtGroupImportTable1.Columns.Add("numPercent1", typeof(string));//类型1人数的百分比
            dtGroupImportTable1.Columns.Add("numCount2", typeof(string));//类型2的人数
            dtGroupImportTable1.Columns.Add("numPercent2", typeof(string));//类型2人数的百分比
            dtGroupImportTable1.Columns.Add("numCount3", typeof(string));//类型3的人数
            dtGroupImportTable1.Columns.Add("numPercent3", typeof(string));//类型3人数的百分比
            dtGroupImportTable1.Columns.Add("numCount4", typeof(string));//类型4的人数
            dtGroupImportTable1.Columns.Add("numPercent4", typeof(string));//类型4人数的百分比
            if (type == 1)
            {
                //表头
                DataRow row = dtGroupImportTable1.NewRow();
                row["remark"] = "总人数";
                for (int i = 0; ((i < 4) && (i < dt.Rows.Count)); i++)
                {
                    row[i + 1] = dt.Rows[i];
                }
                dtGroupImportTable1.Rows.Add(row);
            }
            else
            {
                //第二表格，头
                var row1 = dtGroupImportTable1.NewRow();
                row1["remark"] = dt.Rows[0]["sumcount"];
                row1["numCount1"] = "人次";
                row1["numPercent1"] = "百分比";
                row1["numCount2"] = "人次";
                row1["numPercent2"] = "百分比";
                row1["numCount3"] = "人次";
                row1["numPercent3"] = "百分比";
                row1["numCount4"] = "人次";
                row1["numPercent4"] = "百分比";
                dtGroupImportTable1.Rows.Add(row1);

                var row2 = dtGroupImportTable1.NewRow();
                row2["remark"] = "";
                int k = 1;
                for (int i = 0; ((i < 4) && (i < dt.Rows.Count)); i++)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        row2[k] = dt.Rows[i][j];
                        k++;
                    }
                }
                dtGroupImportTable1.Rows.Add(row2);
            }


            return dtGroupImportTable1;
        }
        /// <summary>
        /// 初始化报表tab
        /// </summary>
        /// <returns></returns>
        DataSet InitTabReport()
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Clear();
            #region 头部
            //报告抬头
            DataTable dtTitle = GetdtTitle();
            dtTitle.TableName = "dtTitle";
            dataSet.Tables.Add(dtTitle.Copy());
            //得到报告单首页
            DataTable dtGroupTitle = GetdtGroupTitle();
            if (dtGroupTitle == null)
            {
                dtGroupTitle = new DataTable();
                //return null;
            }
            dtGroupTitle.TableName = "dtGroupTitle";
            dataSet.Tables.Add(dtGroupTitle.Copy());
            //体检基本信息
            DataTable dtGroupBasicMessage = GetdtGroupBasicMessage();
            if (dtGroupBasicMessage == null)
            {
                dtGroupBasicMessage = new DataTable();
                //return null;
            }
            dtGroupBasicMessage.TableName = "dtGroupBasicMessage";
            dataSet.Tables.Add(dtGroupBasicMessage.Copy());
            //参检人员年龄及性别,柱形图
            DataTable dtGroupAgeSexColumnResult = GetdtGroupAgeSexColumnResult();
            if (dtGroupAgeSexColumnResult == null)
            {
                dtGroupAgeSexColumnResult = new DataTable();
                //return null;
            }
            dtGroupAgeSexColumnResult.TableName = "dtGroupAgeSexColumnResult";
            dataSet.Tables.Add(dtGroupAgeSexColumnResult.Copy());
            //平均年龄,平均年龄之上的人数，之下的人数，左边
            DataTable dtGroupAgeSexCakeLeftResult = GetdtGroupAgeSexCakeResultLeft();
            if (dtGroupAgeSexCakeLeftResult == null)
            {
                dtGroupAgeSexCakeLeftResult = new DataTable();
                //return null;
            }
            dtGroupAgeSexCakeLeftResult.TableName = "dtGroupAgeSexCakeLeftResult";
            dataSet.Tables.Add(dtGroupAgeSexCakeLeftResult.Copy());
            //平均年龄之上的人数，之下的人数,右边
            DataTable dtGroupAgeSexCakeResult = GetdtGroupAgeSexCakeResultRight(dtGroupAgeSexCakeLeftResult);
            if (dtGroupAgeSexCakeResult == null)
            {
                dtGroupAgeSexCakeResult = new DataTable();
                //return null;
            }
            dtGroupAgeSexCakeResult.TableName = "dtGroupAgeSexCakeResult";
            dataSet.Tables.Add(dtGroupAgeSexCakeResult.Copy());
            // 查询体检结果的前10种异常，团检报告,全部
            DataTable dtGroupAllDiseasePercent = GetdtGroupAllDiseasePercent("All");
            if (dtGroupAllDiseasePercent == null)
            {
                dtGroupAllDiseasePercent = new DataTable();
                //return null;
            }
            dtGroupAllDiseasePercent.TableName = "dtGroupAllDiseasePercent";
            dataSet.Tables.Add(dtGroupAllDiseasePercent.Copy());
            //查询体检结果的前10种异常，团检报告,男
            DataTable dtGroupMaleDiseasePercent = GetdtGroupAllDiseasePercent("MStatus");
            if (dtGroupMaleDiseasePercent == null)
            {
                dtGroupMaleDiseasePercent = new DataTable();
                //return null;
            }
            dtGroupMaleDiseasePercent.TableName = "dtGroupMaleDiseasePercent";
            dataSet.Tables.Add(dtGroupMaleDiseasePercent.Copy());
            //查询体检结果的前10种异常，团检报告,女
            DataTable dtGroupFamaleDiseasePercent = GetdtGroupAllDiseasePercent("FStatus");
            if (dtGroupFamaleDiseasePercent == null)
            {
                dtGroupFamaleDiseasePercent = new DataTable();
                //return null;
            }
            dtGroupFamaleDiseasePercent.TableName = "dtGroupFamaleDiseasePercent";
            dataSet.Tables.Add(dtGroupFamaleDiseasePercent.Copy());
            #endregion

            #region 中部
            //单项项目,重要指标,
            var dt17 = DataForImporttantTestItem();

            for (int i = 0; i < dt17.Rows.Count; i++)
            {
                //统计图
                var dt18 = DataForOneImporttantTestItem(dt17.Rows[i]["dicttestitemid"].ToString());
                if (dt18 == null || dt18.Rows.Count == 0)
                {
                    DataTable dt1 = new DataTable("dtGroupImportResult" + (i + 1).ToString());
                    DataTable dt2 = new DataTable("dtGroupImportTableTitle" + (i + 1).ToString());
                    DataTable dt3 = new DataTable("dtGroupImportTable" + (i + 1).ToString());
                    dataSet.Tables.Add(dt1);
                    dataSet.Tables.Add(dt2);
                    dataSet.Tables.Add(dt3);
                    continue;
                    // return null;
                }
                var dt19 = GetdtGroupImportResultChart(dt18, dt17.Rows[i]["testname"].ToString(), dt17.Rows[i]["testname"].ToString());
                dt19.TableName = "dtGroupImportResult" + (i + 1).ToString();
                dataSet.Tables.Add(dt19.Copy());
                //表格
                //头
                var dt23 = GetdtGroupImportTableOne(dt18, 1);
                dt23.TableName = "dtGroupImportTableTitle" + (i + 1).ToString();
                dataSet.Tables.Add(dt23.Copy());
                //数据
                var dt24 = GetdtGroupImportTableOne(dt18, 2);
                dt24.TableName = "dtGroupImportTable" + (i + 1).ToString();
                dataSet.Tables.Add(dt23.Copy());
            }

            //重要指标，双项
            var dt20 = DataForImporttantTestItemTwo();

            for (int i = 0; i < dt20.Rows.Count; i++)
            {

                if (i + 1 + dt17.Rows.Count < 21)//只添加20个table 大于20个 不添加
                {
                    //统计图
                    DataTable dt21 = DataForTwoImporttantTestItem(dt20.Rows[i]["DICTGROUPID"].ToString());
                    if (dt21.Rows.Count == 0)
                    {
                        // return null;
                        DataTable dt1 = new DataTable("dtGroupImportResult" + (i + 1 + dt17.Rows.Count).ToString());
                        DataTable dt2 = new DataTable("dtGroupImportTableTitle" + (i + 1 + dt17.Rows.Count).ToString());
                        DataTable dt3 = new DataTable("dtGroupImportTable" + (i + 1).ToString());
                        dataSet.Tables.Add(dt1);
                        dataSet.Tables.Add(dt2);
                        dataSet.Tables.Add(dt3);
                        continue;
                    }
                    var dt22 = GetdtGroupImportResultChart(dt21, dt20.Rows[i]["DICTGROUPNAME"].ToString());
                    dt22.TableName = "dtGroupImportResult" + (i + 1 + dt17.Rows.Count).ToString();
                    dataSet.Tables.Add(dt22.Copy());
                    //表格
                    //头
                    var dt25 = GetdtGroupImportTableTwo(dt21, 1);
                    dt25.TableName = "dtGroupImportTableTitle" + (i + 1 + dt17.Rows.Count).ToString();
                    dataSet.Tables.Add(dt25.Copy());
                    //数据
                    var dt26 = GetdtGroupImportTableTwo(dt21, 2);
                    dt26.TableName = "dtGroupImportTable" + (i + 1).ToString();
                    dataSet.Tables.Add(dt26.Copy());

                }

            }

            int iRowcount = (dt20.Rows.Count + dt17.Rows.Count);
            //if (iRowcount == 0)//如果重要指标结果为0 不显示报表
            //{
            //    return null;
            //}
            //必须添加20个table，所以在这里判断table 有没有够20个，如果不够就添加
            for (int i = 0; i < 20 - iRowcount; i++)
            {
                var dt22 = new DataTable();
                dt22.TableName = "dtGroupImportResult" + (i + 1 + iRowcount).ToString();
                dataSet.Tables.Add(dt22.Copy());
                var dt23 = new DataTable();
                dt23.TableName = "dtGroupImportTableTitle" + (i + 1 + iRowcount).ToString();
                dataSet.Tables.Add(dt23.Copy());
                var dt24 = new DataTable();
                dt24.TableName = "dtGroupImportTable" + (i + 1 + iRowcount).ToString();
                dataSet.Tables.Add(dt24.Copy());
            }

            #endregion

            #region 尾部
            //慢性病历年比较
            DataTable dtGroupChronicDiseasesCompare = GetdtGroupChronicDiseasesCompare(ParamStatus.OrderDiagnosisType.ImportantIssue);//应该定义枚举变量 1 
            if (dtGroupChronicDiseasesCompare == null)
            {
                return null;
            }
            dtGroupChronicDiseasesCompare.TableName = "dtGroupChronicDiseasesCompare";
            dataSet.Tables.Add(dtGroupChronicDiseasesCompare.Copy());
            //健康问题历年比较
            DataTable dtGroupHealthCompare = GetdtGroupChronicDiseasesCompare(ParamStatus.OrderDiagnosisType.MajorProblem);//应该定义枚举变量 2 
            if (dtGroupHealthCompare == null)
            {
                dtGroupHealthCompare = new DataTable();
                //return null;
            }
            dtGroupHealthCompare.TableName = "dtGroupHealthCompare";
            dataSet.Tables.Add(dtGroupHealthCompare.Copy());
            //健康问题分析与建议
            DataTable dtGroupHealthSuggest = GetdtGroupHealthSuggest();
            dtGroupHealthSuggest.TableName = "dtGroupHealthSuggest";
            dataSet.Tables.Add(dtGroupHealthSuggest.Copy());
            //体检异常统计表
            DataTable dtGroupHealthAnormal = GetdtGroupHealthCompare();
            if (dtGroupHealthAnormal == null)
            {
                dtGroupHealthAnormal = new DataTable();
                //  return null;
            }
            dtGroupHealthAnormal.TableName = "dtGroupHealthAnormal";
            dataSet.Tables.Add(dtGroupHealthAnormal.Copy());
            if (chkShowSum.Checked)//显示汇总结果
            {
                DataTable dtGroupAllResult = GetdtGroupAllResult();
                dtGroupAllResult.TableName = "dtGroupAllResult";
                dataSet.Tables.Add(dtGroupAllResult.Copy());
            }
            else//不显示汇总结果
            {
                DataTable dtGroupAllResult = new DataTable();
                dtGroupAllResult.TableName = "dtGroupAllResult";
                dataSet.Tables.Add(dtGroupAllResult.Copy());
            }
            // 已体检而未完成项目，团检报告
            DataTable dtGroupAnFinish = GetdtGroupAnFinish();
            dtGroupAnFinish.TableName = "dtGroupAnFinish";
            dataSet.Tables.Add(dtGroupAnFinish.Copy());
            //已预约未体检人员，团检报告
            DataTable dtGroupAnExamination = GetdtGroupAnExamination();
            dtGroupAnExamination.TableName = "dtGroupAnExamination";
            dataSet.Tables.Add(dtGroupAnExamination.Copy());
            //推荐项目
            DataTable dtCustomernexttest = GetdtCustomernexttest();
            dtCustomernexttest.TableName = "dtCustomernexttest";
            dataSet.Tables.Add(dtCustomernexttest.Copy());
            #endregion
            return dataSet;
        }
        #endregion

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet data = InitTabReport();//取报告数据，数据存放在sesssion当中
            ////查询数据才可以打印
            //if (Session["GroupDataSet"] == null)
            //{
            //    MessageBoxShow("没有报告数据！请先查询或者双击左边列表行数据！");
            //    return;
            //}

            if (Userinfo.initlocalsetting == null)
            {
                //取mac地址                   
                string hostmac = hdMac.Text;
                Userinfo.initlocalsetting = new InitlocalsettingService().GetInitlocalsetting(hostmac);
                Session["UserInfo"] = Userinfo;
            }

            //var ds = Session["GroupDataSet"] as DataSet;
            if (data == null)
            {
                MessageBoxShow("没有报告数据！");
                return;
            }
            Report report = new Report();
            report = commonReport.GetReportByDataset("35", data, 1);
            commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);
            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
        }

        //双击年诊断建议
        protected void gdCustomervaliddiagnosis_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            try
            {
                object[] arrstr = gdCustomervaliddiagnosis.DataKeys[e.RowIndex];
                dplastyearFrom.Text = (arrstr[1] ?? "").ToString();
                dplastyearTo.Text = (arrstr[2] ?? "").ToString();
                dpThisyearFrom.Text = (arrstr[3] ?? "").ToString();
                dpThisyearTo.Text = (arrstr[4] ?? "").ToString();
                dropDictcustomer.SelectedValue = dropCommetDictcustomer.SelectedValue;

                SearchMethod();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
