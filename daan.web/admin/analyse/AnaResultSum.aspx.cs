using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using daan.domain;
using daan.service.common;
using daan.service.dict;
using System.Collections;
using daan.service.order;
using daan.service.login;
using ExtAspNet;
using daan.util.Web;
using daan.util.Common;
using System.Text;
using daan.web.code;
using System.Data;

/* 总体设计思路
 * 初始化数据时和查询按钮共用一个查询参数
 * 体检者结果在每次查询时都不加载，只要在点击gdorders的行之后
 * 才会加载，每点击一次，根据体检号ordernum查询数据库，不使用cache
 * 点击档案按钮，加载体检者以往的病史
 * 给科室小结表加上按钮，根据体检流水号可以查看科室的检查结果明细表
 * 科室信息表和诊断信息表不产生任何联系，即每点击科室小结表的一行，诊断
 * 信息表不对此做任何应答，添加诊断和删除诊断，将立即刷新诊断信息表
 * 下次体检推荐项目采用下拉列表搞，参考周勇例子,下次推荐表不必要太占地方
 */
namespace daan.web.admin.analyse
{
    public partial class AnaResultSum : PageBase
    {
        #region 字段
        LoginService loginservice = new LoginService();
        OrdersService ordersService = new OrdersService();
        DictCustomerService dictCustomerService = new DictCustomerService();
        OrdernexttestService _ordernexttestService = new OrdernexttestService();
        OrderlabdeptresultService orderlabdeptresultService = new OrderlabdeptresultService();
        OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
        Orderdiagnosis orderdiagnosisTo = new Orderdiagnosis();
        #endregion
        public double OrderdiagnosisId
        {
            get { return Convert.ToDouble(Convert.ToInt32(ViewState["OrderdiagnosisId"]) == 0 ? 0 : ViewState["OrderdiagnosisId"]); }
            set { ViewState["OrderdiagnosisId"] = value; }
        }


        #region 页面逻辑方法
        /// <summary>
        /// 清除诊断信息
        /// </summary>
        void ClearOrderdiagnosis()
        {
            taDiseasecause.Text = "";
            taDiseasedescription.Text = "";
            taSuggestion.Text = "";
            tbDiagnosisname.Text = "";
        }

        /// <summary>
        /// 清空右侧控件数据
        /// </summary>
        void ClearOrdersTestAndOrderdiagnosisAndOrderNextTest()
        {
            gdOrderlabdeptResult.DataSource = new List<object>();
            gdOrderlabdeptResult.DataBind();
            gdOrdernexttest.DataSource = new List<object>();
            gdOrdernexttest.DataBind();
            gdOrderdiagnosis.DataSource = new List<object>();
            gdOrderdiagnosis.DataBind();
            labInfo.Text = "";
            btnRecoder.OnClientClick = "";
            btnInsertOrderdiagnosis.OnClientClick = "";

            gdVisitList.DataSource = new DataTable();
            gdVisitList.DataBind();
            txtVisitText.Text = "";
            txtVisitDoctor.Text = "";
            txtVisitdate.Text = "";

            ClearOrderdiagnosis();
        }
        /// <summary>
        /// 根据订单号加载数据进科室小结表
        /// </summary>
        /// <param name="ordernum"></param>
        void InitgdOrderlabdeptResult(string ordernum)
        {
            gdOrderlabdeptResult.SelectedRowIndexArray = null;
            gdOrderlabdeptResult.DataSource = orderlabdeptresultService.DataForOrderlabdept(ordernum);
            gdOrderlabdeptResult.DataBind();
        }

        /// <summary>
        /// 根据订单号加载体检者诊断信息
        /// </summary>
        /// <param name="ordernum"></param>
        void InitgdOrderdiagnosis(string ordernum)
        {
            gdOrderdiagnosis.SelectedRowIndexArray = null;
            gdOrderdiagnosis.DataSource = orderdiagnosisService.SelectOrderdiagnosis(ordernum);
            gdOrderdiagnosis.DataBind();

        }
        /// <summary>
        /// 根据订单号加载体检者推荐项目
        /// </summary>
        /// <param name="ordernum"></param>
        void InitgdOrdernexttest(string ordernum)
        {
            gdOrdernexttest.SelectedRowIndexArray = null;
            gdOrdernexttest.DataSource = _ordernexttestService.SelectOrdernexttest(ordernum);
            gdOrdernexttest.DataBind();
        }

        /// <summary>
        /// 防止为空
        /// </summary>
        /// <param name="dicttest"></param>
        /// <returns></returns>
        static Dicttestitem ConVertClass(Dicttestitem dicttestitem)
        {
            dicttestitem.Testname = (dicttestitem.Testname == null ? "" : dicttestitem.Testname);
            dicttestitem.Fastcode = (dicttestitem.Fastcode == null ? "" : dicttestitem.Fastcode);
            dicttestitem.Engname = (dicttestitem.Engname == null ? "" : dicttestitem.Engname);
            return dicttestitem;
        }

        /// <summary>
        /// 加载数据进gdDicttestitem表中
        /// </summary>
        void InitgdDicttestitem(string strKey)
        {
            //
            PageUtil pageUtil = new PageUtil(gdDicttestitem.PageIndex, gdDicttestitem.PageSize);
            int start = pageUtil.GetPageStartNum();
            int end = pageUtil.GetPageEndNum();
            //从缓存中取数据，取检验项目(dicttestitem)
            var TestItemList = loginservice.GetLoginDicttestitemList().Where(c => (int.Parse(c.Testtype) < 3));//项目字典表
            //TestItemList.Join(gdOrdernexttest.DataKeys[2],a=>a.Dicttestitemid,b=>double.Parse(b.ToString()),
            if (strKey.Length == 0)//不进行筛选数据时
            {
                gdDicttestitem.RecordCount = TestItemList.Count();
                gdDicttestitem.DataSource = TestItemList.Skip(start).Take(end - start);
                gdDicttestitem.DataBind();
            }
            else
            {
                //有关键字时，根据项目名称，快捷录入码，英文名进行筛选
                var p = TestItemList.Where(c => (ConVertClass(c).Testname.Contains(tbStrKey.Text)) || (ConVertClass(c).Fastcode.Contains(tbStrKey.Text)) ||
                    (ConVertClass(c).Engname.Contains(tbStrKey.Text)));
                gdDicttestitem.RecordCount = p.Count();
                gdDicttestitem.DataSource = p.Skip(start - 1).Take(end - start + 1);
                gdDicttestitem.DataBind();
            }

        }
        /// <summary>
        /// 初始化页面数据，只加载订单主表数据
        /// 其他表的数据不加载，只有点击行才加载
        /// </summary>
        void InitPage()
        {
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now.AddDays(1);
        }
        /// <summary>
        /// 初始化订单号表格里面的数据
        /// </summary>
        /// <param name="ordernum"></param>
        private void GvOrdersBinder(bool isPage=false)
        {
            string DefaultSortKey = "dictreporttemplateid,ordernum,lastupdatedate";//默认排序方式
            if (isPage)
            {
                DefaultSortKey = hidSortKey.Text;
            }
            BindGridWithSort(DefaultSortKey);
        }

        /// <summary>
        /// 对查询结果排序并绑定到Grid
        /// </summary>
        /// <param name="sortKey">排序</param>
        private void BindGridWithSort(string sortKey)
        {
            Hashtable htPara = new Hashtable();

            PageUtil pageUtil = new PageUtil(gdOrders.PageIndex, gdOrders.PageSize);
            htPara.Add("pageStart", pageUtil.GetPageStartNum());
            htPara.Add("pageEnd", pageUtil.GetPageEndNum());
            htPara.Add("StartDate", dpFrom.Text);
            htPara.Add("EndDate", dpTo.Text);
            htPara.Add("dropDictLab", ddlDictLab.SelectedValue);
            htPara.Add("dictcustomerid", dropDictcustomer.SelectedValue);
            htPara.Add("status", ddlStatus.SelectedValue);
            htPara.Add("iolis", dropStatus.SelectedValue);
            htPara.Add("strKey", TextUtility.ReplaceText(tbKeyWord.Text));
            htPara.Add("sortKey", sortKey);
            htPara.Add("strDoctor",TextUtility.ReplaceText(txtDoctor.Text.Trim()));
            htPara.Add("strLoginUser",Userinfo.userName);
            gdOrders.RecordCount = ordersService.CountForFinalCheck(htPara);
            DataTable dt=ordersService.DataForFinalCheck(htPara);

            //if (ddlStatus.SelectedValue == "20")
            //{
            //    gdOrders.FindColumn("chu").Hidden=false;
            //    gdOrders.FindColumn("wan").Hidden = true;
            //}
            //else if (ddlStatus.SelectedValue == "25")
            //{
            //    gdOrders.FindColumn("chu").Hidden = false;
            //    gdOrders.FindColumn("wan").Hidden = false;
            //}
            //else
            //{
            //    gdOrders.FindColumn("chu").Hidden = true;
            //    gdOrders.FindColumn("wan").Hidden = true;
            //}
            gdOrders.DataSource = dt;
            gdOrders.DataBind();
            gdOrders.SelectedRowIndexArray = null;
        }

        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(ddlDictLab, true);
            ddlDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }
        #endregion

        #region 页面事件
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=250;", dropDictcustomer.ClientID));
            if (!IsPostBack)
            {
                btnLog.OnClientClick = gdOrders.GetNoSelectionAlertReference("您还未选择！", "体检系统");
                btnSaveDicttestitem.OnClientClick = gdDicttestitem.GetNoSelectionAlertReference("您还未选择！", "体检系统");
                btn_Save.OnClientClick = gdOrders.GetNoSelectionAlertReference("您还未选择体检者信息！","体检系统-回访记录");
                hidSortKey.Text = "dictreporttemplateid,ordernum,lastupdatedate";//默认排序规则
                BindDictLab();
                InitPage();
                //体检单位初始化
                BinddropDictcustomer(ddlDictLab.SelectedValue);
                dpNextRERUNDATE.MinDate = DateTime.Now;
            }
            orderdiagnosisTo.Orderdiagnosisid = OrderdiagnosisId;
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
        /// 刷新新加的诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void winDictdiagnosis_Close(object sender, WindowCloseEventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();

            InitgdOrderdiagnosis(ordernum);
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
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbKeyWord_Trigger2Click(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
            tbKeyWord.ShowTrigger1 = true;
        }

        /// <summary>
        /// 执行清空动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbKeyWord_Trigger1Click(object sender, EventArgs e)
        {
            tbKeyWord.Text = "";
            tbKeyWord.ShowTrigger1 = false;
        }
        /// <summary>
        /// 查询备选项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbStrKey_TextChanged(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            InitgdDicttestitem(tbStrKey.Text);
        }
        /// <summary>
        /// 保存诊断信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveOrderdiagnosis_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            //总检时，体检者的诊断如果不合理，审核者将会修改诊断信息
            if (gdOrderdiagnosis.SelectedRowIndexArray.Length == 0)//如果没有选中诊断信息，就返回
            {
                return;
            }
            Orderdiagnosis orderdiagnosis = new Orderdiagnosis();
            foreach (int row in gdOrderdiagnosis.SelectedRowIndexArray)
            {

                //int id = Convert.ToInt32(gdOrderdiagnosis.DataKeys[row][0]);
                orderdiagnosis.Orderdiagnosisid = orderdiagnosisTo.Orderdiagnosisid;
                orderdiagnosis.Diagnosisname = tbDiagnosisname.Text;
                orderdiagnosis.Diseasedescription = taDiseasedescription.Text;
                orderdiagnosis.Diseasecause = taDiseasecause.Text;
                orderdiagnosis.Suggestion = taSuggestion.Text;
                //orderdiagnosis.Diagnosistype = ddldiagnosistype.SelectedValue;
                orderdiagnosis.Diagnosistype = "3";
                if (orderdiagnosisService.UpdateOrderdiagnosis(orderdiagnosis))
                {
                    InitgdOrderdiagnosis(ordernum);
                    orderdiagnosisService.AddOperationLog(ordernum, null, "总检", "修改诊断信息", "修改留痕", "无");
                    MessageBoxShow("修改成功");
                }
                else
                {
                    MessageBoxShow("修改失败");
                }
            }
        }

        /// <summary>
        /// 删除诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteOrderdiagnosis_Click(object sender, EventArgs e)
        {
            if (gdOrderdiagnosis.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("请先选择诊断信息"); return;
            }

            foreach (int id in gdOrderdiagnosis.SelectedRowIndexArray)
            {
                orderdiagnosisService.DeleteOrderdiagnosis(gdOrderdiagnosis.DataKeys[id][0].ToString());
            }

            //刷新数据
            int selectorderIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectorderIndex][0].ToString();
            InitgdOrderdiagnosis(ordernum);
            tbDiagnosisname.Text = string.Empty;
            taDiseasedescription.Text = string.Empty;
            taDiseasecause.Text = string.Empty;
            taSuggestion.Text = string.Empty;


        }

        private void DeleteOrderdiagnosis()
        {
            if (gdOrderdiagnosis.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("请先选择诊断信息"); return;
            }

            int selectorderIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectorderIndex][0].ToString();

            int selectOrderdiagnosisIndex = gdOrderdiagnosis.SelectedRowIndexArray[0];
            string Orderdiagnosisid = gdOrderdiagnosis.DataKeys[selectOrderdiagnosisIndex][0].ToString();


            if (!orderdiagnosisService.DeleteOrderdiagnosis(Orderdiagnosisid))
            {
                MessageBoxShow("删除失败！ 请刷新重试"); return;
            }
            else
            {
                InitgdOrderdiagnosis(ordernum);
                tbDiagnosisname.Text = string.Empty;
                taDiseasedescription.Text = string.Empty;
                taDiseasecause.Text = string.Empty;
                taSuggestion.Text = string.Empty;

            }

        }

        /// <summary>
        /// 删除体检者推荐项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelgdOrdernexttest_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            foreach (var item in gdOrdernexttest.SelectedRowIndexArray)
            {
                _ordernexttestService.DeleteOrdernexttest(gdOrdernexttest.DataKeys[item][0].ToString());
                string content = gdOrdernexttest.DataKeys[item][1].ToString();
                _ordernexttestService.AddOperationLog(ordernum, null, "总检", "删除推荐项目" + content,
                    "删除", "");
                InitgdOrdernexttest(ordernum);
                gdOrdernexttest.SelectedRowIndexArray = null;
            }
        }
        
        //体检人档案
        protected void btnRecoder_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            winArchives.Hidden = false;
            winArchives.IFrameUrl = "AnaResultSum_ArchivesWin.aspx?dictmemberid=" + gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][3];
            winArchives.Title = "健康档案";
        }
        /// <summary>
        /// 添加推荐项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveDicttestitem_Click(object sender, EventArgs e)
        {
            if (!dpNextRERUNDATE.SelectedDate.HasValue)
            {
                MessageBoxShow("必须选择复查时间", MessageBoxIcon.Error);
                return;
            }


            if (!dpReRunEndDate.SelectedDate.HasValue)
            {
                MessageBoxShow("必须选择复查结束时间", MessageBoxIcon.Error);
                return;
            }


            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }



            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            var selectArray = gdDicttestitem.SelectedRowIndexArray;
            foreach (var item in selectArray)
            {
                Ordernexttest ordernexttest = new Ordernexttest();
                ordernexttest.Ordernexttestid = _ordernexttestService.getSeqID("SEQ_ORDERNEXTTEST");
                ordernexttest.Ordernum = ordernum;
                ordernexttest.Dicttestitemid = double.Parse(gdDicttestitem.DataKeys[item][0].ToString());

                if (gdDicttestitem.DataKeys[item][1] != null)
                    ordernexttest.Engname = gdDicttestitem.DataKeys[item][1].ToString();
                else
                    ordernexttest.Engname = "";
                if (gdDicttestitem.DataKeys[item][3] != null)
                    ordernexttest.Testcode = gdDicttestitem.DataKeys[item][3].ToString();
                else
                    ordernexttest.Testcode = "";
                if (gdDicttestitem.DataKeys[item][4] != null)
                    ordernexttest.Testname = gdDicttestitem.DataKeys[item][4].ToString();
                else
                    ordernexttest.Testname = "";
                ordernexttest.Rerundate = dpNextRERUNDATE.SelectedDate;
                ordernexttest.Rerunenddate = dpReRunEndDate.SelectedDate;

                ordernexttest.Isneededorder = gdDicttestitem.DataKeys[item][5].ToString();//是否必检项目
               
                string count = _ordernexttestService.CountForOrdernexttest(ordernexttest);
                if (int.Parse(count) == 0)
                {
                    if (_ordernexttestService.InsertOrdernexttest(ordernexttest))
                    {
                        _ordernexttestService.AddOperationLog(ordernum, null, "总检", "增加" + ordernexttest.Testname, "增加", "");
                    }
                }
            }
            InitgdOrdernexttest(ordernum);
        }

        /// <summary>
        /// 体检单位改变响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinddropDictcustomer(ddlDictLab.SelectedValue);
        }

        // 初始化体检单位
        void BinddropDictcustomer(string dictlabid)
        {
            DropDictcustomerBinder(dropDictcustomer, dictlabid, true);
        }

        #region >>>> zhouy 报告预览
        /// <summary>
        /// 报告预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportView_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length != 1)
            {
                MessageBoxShow("请选择一项进行预览！"); return;
            }
            //判断是否总检完成
            if (CheckIsFinishCheck(gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][1]))
            {
                MessageBoxShow("选中订单没有[完成总检],不能预览"); return;
            }
            PageContext.RegisterStartupScript(WinReportView.GetShowReference("../report/RepShowView.aspx?reportType=1&order_num=" + gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][0]));
        }

        private bool CheckIsFinishCheck(object str)
        {
            return Convert.ToInt32(str) < (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck;
        }
        #endregion

        #region >>>> zhouy 查看此订单的所有结果
        /// <summary>
        /// 查询结果
        /// </summary>
        /// <param name="ordernum"></param>
        private void BindResult(string ordernum)
        {
            OrderTestService orderTestService = new OrderTestService();
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            gdOrdertest.DataSource = orderTestService.DataForOrderLabdeptresult(ht);
            gdOrdertest.DataBind();
        }

        #endregion

        /// <summary>
        /// 订单主表行点击事件，点击行后加载相对应信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrders_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            int states = Convert.ToInt32(gdOrders.DataKeys[e.RowIndex][1]);
            //如果订单状态为完成总检状态时，不可以添加诊断信息
            if (states >= (int)ParamStatus.OrdersStatus.FinishCheck)
            {
                btnInsertOrderdiagnosis.Hidden = true;
                btnSaveOrderdiagnosis.Hidden = true;
                btnDeleteOrderdiagnosis.Hidden = true;
                btnInsertOrderdiagnosis.OnClientClick = "";
            }
            else
            {
                btnInsertOrderdiagnosis.Hidden = false;
                btnSaveOrderdiagnosis.Hidden = false;
                btnDeleteOrderdiagnosis.Hidden = false;
                btnInsertOrderdiagnosis.OnClientClick = winDictdiagnosis.GetShowReference(String.Format("./AnaResultSum_AddDictdiagnosisWin.aspx?ordernum={0}", gdOrders.DataKeys[e.RowIndex][0]));
            }
            btnRecoder.OnClientClick = winArchives.GetShowReference("./AnaResultSum_ArchivesWin.aspx?dictmemberid=" + gdOrders.DataKeys[e.RowIndex][3]);
            LoadAllTabData(e.RowIndex);
            ClearOrderdiagnosis();

            BindResult(gdOrders.DataKeys[e.RowIndex][0].ToString());
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            if (gdOrders.Rows.Count <= 0 || gdOrders.SelectedRowIndexArray.Length <= 0)
                return;
            object[] objValue = gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]];
            string orderNum = TypeParse.ObjToStr(objValue[0], "");
            WinBillRemark.Hidden = false;
            WinBillRemark.IFrameUrl = "../bill/BillOperationLog.aspx?ordernum=" + orderNum;
            WinBillRemark.Title = "订单日志查询";
        }

        /// <summary>
        /// 初步总检审核成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFirstInstance_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("您未选中记录！", MessageBoxIcon.Information);
                return;
            }
            StringBuilder errOrdernum = new StringBuilder();
            try
            {
                for (int i = 0; i < gdOrders.SelectedRowIndexArray.Length; i++)
                {
                    int selectIndex = gdOrders.SelectedRowIndexArray[i];
                    string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
                    //int status = int.Parse(gdOrders.DataKeys[i][1].ToString());
                    int status = int.Parse(gdOrders.DataKeys[selectIndex][1].ToString());
                    if (status != (int)ParamStatus.OrdersStatus.WaitCheck)
                    {
                        MessageBoxShow("状态为待总检时才可以初审");
                        return;
                    }
                    Hashtable ht = new Hashtable();
                    ht.Add("ordernum", ordernum);
                    ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.WaitCheck);
                    ht.Add("status", (int)ParamStatus.OrdersStatus.FirstCheck);
                    ht.Add("authorizedbyid", Userinfo.userId);

                    if (ordersService.EditStatusByOldStatus(ht))
                    {
                        ordersService.AddOperationLog(ordernum, null, "初步总检", "初步总检审核成功", "修改留痕", "");
                    }
                    else
                    {
                        errOrdernum.Append(ordernum + ",");   //string.Format("{0},{1}", errOrdernum, ordernum);
                    }
                }
                MessageBoxShow("所选记录初步总检审核成功！", MessageBoxIcon.Information);
                GvOrdersBinder();
                ClearOrdersTestAndOrderdiagnosisAndOrderNextTest();
            }
            catch (Exception ex)
            {
                MessageBoxShow(string.Format("订单号为：{0}初步总检审核不成功,错误信息：{1}", errOrdernum, ex.Message));
            }
        }
        /// <summary>
        /// 完成总检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("您未选中记录！", MessageBoxIcon.Information);
                return;
            }
            StringBuilder errOrdernum = new StringBuilder();
            try
            {
                for (int i = 0; i < gdOrders.SelectedRowIndexArray.Length; i++)
                {
                    int selectIndex = gdOrders.SelectedRowIndexArray[i];
                    string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
                    int status = int.Parse(gdOrders.DataKeys[selectIndex][1].ToString());
                    //int status = int.Parse(gdOrders.DataKeys[i][1].ToString());
                    if (status != (int)ParamStatus.OrdersStatus.FirstCheck)
                    {
                        MessageBoxShow("状态为初步总检时才可以审核");
                        return;
                    }
                    Hashtable ht = new Hashtable();
                    ht.Add("ordernum", ordernum);
                    ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FirstCheck);
                    ht.Add("status", (int)ParamStatus.OrdersStatus.FinishCheck);
                    ht.Add("finishbyid", Userinfo.userId);
                    ht.Add("TRANSED",0);
                    if (ordersService.EditStatusByOldStatus(ht))
                    {
                        ordersService.AddOperationLog(ordernum, null, "总检", "完成总检审核成功", "修改留痕", "");
                    }
                    else
                    {
                        errOrdernum.Append(ordernum + ",");
                    }
                }
                MessageBoxShow("所选记录完成总检审核成功！", MessageBoxIcon.Information);
                GvOrdersBinder();
                ClearOrdersTestAndOrderdiagnosisAndOrderNextTest();
            }
            catch (Exception ex)
            {
                MessageBoxShow(string.Format("订单号为：{0}完成总检审核失败,错误信息：{1}", errOrdernum, ex.Message));
            }


        }
        /// <summary>
        /// 取消完成总检事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelFinish_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("您未选中记录！", MessageBoxIcon.Information);
                return;
            }
            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            int status = int.Parse(gdOrders.DataKeys[selectIndex][1].ToString());
            if (status != (int)ParamStatus.OrdersStatus.FinishCheck)
            {
                MessageBoxShow("状态为完成总检时才可以取消完成总检审核");
                return;
            }
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.FinishCheck);
            ht.Add("status", (int)ParamStatus.OrdersStatus.FirstCheck);
            ht.Add("TRANSED", 0);
            if (ordersService.EditStatusByOldStatus(ht))
            {
                ordersService.AddOperationLog(ordernum, null, "总检", " 取消完成总检成功", "修改留痕", "");
                MessageBoxShow(String.Format("订单号：{0} 取消完成总检审核成功", ordernum));
                GvOrdersBinder();
                ClearOrdersTestAndOrderdiagnosisAndOrderNextTest();
            }
            else
            {
                MessageBoxShow("取消完成总检审核失败");
            }
        }
        /// <summary>
        /// 退回重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                MessageBoxShow("您未选中记录！", MessageBoxIcon.Information);
                return;
            }
            Hashtable ht = new Hashtable();
            string ordernum = gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]][0].ToString();
            ht.Add("ordernum", ordernum);
            ht.Add("oldstatus", (int)ParamStatus.OrdersStatus.WaitCheck);
            ht.Add("status", (int)ParamStatus.OrdersStatus.BarCodePrint);
            ht.Add("TRANSED", "0");
            try
            {
                if (ordersService.EditStatusByOldStatus(ht))
                {
                    ordersService.AddOperationLog(ordernum, null, "总检", "退回重做", "修改留痕", "");
                }
                tbKeyWord_Trigger2Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 诊断表行点击事件，点击行后加载相对应信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrderdiagnosis_RowClick(object sender, GridRowClickEventArgs e)
        {
            OrderdiagnosisId = Convert.ToDouble(gdOrderdiagnosis.DataKeys[e.RowIndex][0]);
            var str_tbDiagnosisname = gdOrderdiagnosis.DataKeys[e.RowIndex][1];
            var str_taDiseasedescription = gdOrderdiagnosis.DataKeys[e.RowIndex][2];
            var str_taDiseasecause = gdOrderdiagnosis.DataKeys[e.RowIndex][3];
            var str_taSuggestion = gdOrderdiagnosis.DataKeys[e.RowIndex][4];
            var str_Diagnosistype = gdOrderdiagnosis.DataKeys[e.RowIndex][5];
            tbDiagnosisname.Text = (str_tbDiagnosisname == null ? "" : str_tbDiagnosisname.ToString());
            taDiseasedescription.Text = (str_taDiseasedescription == null ? "" : str_taDiseasedescription.ToString());
            taDiseasecause.Text = (str_taDiseasecause == null ? "" : str_taDiseasecause.ToString());
            taSuggestion.Text = (str_taSuggestion == null ? "" : str_taSuggestion.ToString());
            //ddldiagnosistype.SelectedValue = (str_Diagnosistype == null ? "" : str_Diagnosistype.ToString());
        }
        /// <summary>
        /// gdorders分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrders_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdOrders.PageIndex = e.NewPageIndex;
            //分页查询条件
            GvOrdersBinder(true);
            ClearOrdersTestAndOrderdiagnosisAndOrderNextTest();
        }
        /// <summary>
        /// 根据条件查询orders里面的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlDictLab.SelectedText.Length == 0)
            {
                MessageBoxShow("没有选择分点时不可以查询数据");
                return;
            }
            ClearOrdersTestAndOrderdiagnosisAndOrderNextTest();
            GvOrdersBinder();
        }

        #region >>>> zhouy 保存总体评价
        //保存总体评价 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (tbxordernum.Text.Trim() == string.Empty || gdOrders.SelectedRowIndexArray.Length == 0)
            {
                MessageBoxShow("请先选择一条记录!"); return;
            }
            string suggestion = tbxSuggestion.Text.Trim();
            string comment = tbxComment.Text.Trim();

            Orderresultcomment orc = new Orderresultcomment();
            orc.Engresultcomment = orc.Engresultsuggestion = string.Empty;
            orc.Resultcomment = comment;
            orc.Resultsuggestion = suggestion;
            orc.Ordernum = tbxordernum.Text;
            int[] intstr = gdOrders.SelectedRowIndexArray;

            OrderresultcommentService orcs = new OrderresultcommentService();
            try
            {
                if (orcs.SelectOrderresultcomment(orc.Ordernum) == null)
                {
                    orcs.InsertOrderresultcomment(orc);
                }
                else
                {
                    orcs.UpdateOrderresultcomment(orc);
                }
                MessageBoxShow("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBoxShow("保存失败：" + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 根据选择，加载体检者基本信息
        /// </summary>
        void InitOrderText(int rowindex)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0)//判断是否选中一个体检者
            {
                return;
            }
            //初始化客户信息
            labInfo.Text = string.Format("[姓名:{0}]  [性别:{1}]  [年龄:{2}]  [婚姻:{3}]  [联系方式:{4}]  [单位:{5}]",
                gdOrders.DataKeys[rowindex][4],
                gdOrders.DataKeys[rowindex][5],
                WebUI.GetAge(gdOrders.DataKeys[rowindex][6].ToString()),
                gdOrders.DataKeys[rowindex][7].ToString() == "1" ? "已婚" : (gdOrders.DataKeys[rowindex][7].ToString() == "0" ? "未婚" : "未知"),
                gdOrders.DataKeys[rowindex][8] == null ? "无" : gdOrders.DataKeys[rowindex][8].ToString(),
                gdOrders.DataKeys[rowindex][9] == null ? "无" : gdOrders.DataKeys[rowindex][9].ToString()
            );
        }

        /// <summary>
        /// 列表排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrders_Sort(object sender, GridSortEventArgs e)
        {
            string sortkey = string.Empty;
            switch (e.SortField)
            {
                case "ORDERNUM":
                    sortkey = string.Format("{0} {1},{2}", e.SortField,e.SortDirection,"dictreporttemplateid,lastupdatedate");
                    break;
                case "dictreporttemplateid":
                    sortkey = string.Format("{0} {1},{2}", e.SortField, e.SortDirection,"ORDERNUM,lastupdatedate");
                    break;
                case "lastupdatedate":
                    sortkey = string.Format("{0} {1},{2}", e.SortField, e.SortDirection, "dictreporttemplateid,ORDERNUM");
                    break;
                default:
                    sortkey = string.Format("{0} {1}", e.SortField, e.SortDirection);
                    break;
            }
            hidSortKey.Text = sortkey;
            BindGridWithSort(sortkey);
        }

        /// <summary>
        /// 加载总体评价
        /// </summary>
        /// <param name="rowindex"></param>
        void InitgdOrderResultComment(string ordernum)
        {
            OrderresultcommentService orcs = new OrderresultcommentService();
            Orderresultcomment orc = orcs.SelectOrderresultcomment(ordernum);
            if (orc != null)
            {
                tbxComment.Text = orc.Resultcomment;
                tbxSuggestion.Text = orc.Resultsuggestion;
            }
            else
            {
                tbxComment.Text = tbxSuggestion.Text = string.Empty;
            }
        }

        /// <summary>
        /// 加载4个tab数据
        /// </summary>
        void LoadAllTabData(int rowindex)
        {
            string ordernum = tbxordernum.Text = gdOrders.DataKeys[rowindex][0].ToString();
            InitOrderText(rowindex);
            InitgdOrderlabdeptResult(ordernum);
            InitgdOrderdiagnosis(ordernum);
            InitgdOrdernexttest(ordernum);
            InitgdOrderResultComment(ordernum);
            InitgdOrderVisitList(ordernum);
        }

        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRemark_Click(object sender,EventArgs e)
        {
            if (gdOrders.Rows.Count <= 0 || gdOrders.SelectedRowIndexArray.Length <= 0)
                return;
            object[] objValue = gdOrders.DataKeys[gdOrders.SelectedRowIndexArray[0]];
            string orderNum = TypeParse.ObjToStr(objValue[0], "");
            WinAddRemark.Hidden = false;
            WinAddRemark.IFrameUrl = "./AnaResultSun_AddRemark.aspx?ordernum=" + orderNum;
        }

        #region >>>> 回访记录
        //点击体检者，加载回访记录列表
        private void InitgdOrderVisitList(string ordernum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            DataTable dt=ordersService.GetVisitList(ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <=dt.Rows.Count-1; i++)
                {
                    dt.Rows[i]["rn"] =dt.Rows.Count - i;
                }
            }
            gdVisitList.DataSource = dt;
            gdVisitList.DataBind();
            clearVisit();
        }
        //设置回访记录表单到原始状态
        private void clearVisit()
        {
            txtVisitText.Text = string.Empty;
            txtVisitDoctor.Text = Userinfo.userName;
            txtVisitdate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            gdVisitList.SelectedRowIndexArray = null;
        }
        //行点击事件
        protected void gdVisitList_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            if (gdVisitList.SelectedRowIndexArray.Length > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("visitid", gdVisitList.DataKeys[gdVisitList.SelectedRowIndexArray[0]][0]);
                DataTable dt = ordersService.GetVisitList(ht);
                if (dt != null || dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtVisitdate.Text = Convert.ToDateTime(dr["visittime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    txtVisitDoctor.Text = dr["visitor"].ToString();
                    txtVisitText.Text = dr["visitcontext"].ToString();
                }
            }
            else
            {
                clearVisit();
            }
        }
        // 保存
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVisitText.Text.Trim()))
            {
                MessageBoxShow("回访内容不能为空!"); return;
            }
            Hashtable htPara = new Hashtable();
            if (gdVisitList.SelectedRowIndexArray.Length == 0)
            {
                htPara.Add("visitid", 0);
            }
            else
            {
                //最新记录才可以修改
                if (Convert.ToInt32(gdVisitList.SelectedRowIndexArray[0].ToString()) != 0)
                {
                    MessageBoxShow("只有最新回访记录，才可进行修改操作!");
                    return;
                }
                //判断只有【回访记录人】为当前用户，才可进行修改操作
                string visitor = gdVisitList.DataKeys[gdVisitList.SelectedRowIndexArray[0]][1].ToString();
                if (Userinfo.userName != visitor)
                {
                    MessageBoxShow("只有【回访记录人】为当前用户，才可进行修改操作!");
                    return;
                }
                htPara.Add("visitid", gdVisitList.DataKeys[gdVisitList.SelectedRowIndexArray[0]][0].ToString());
            }
            htPara.Add("visitcontext",TextUtility.ReplaceText(txtVisitText.Text.Trim()));
            htPara.Add("ordernum", tbxordernum.Text);
            htPara.Add("visitor", TextUtility.ReplaceText(txtVisitDoctor.Text.Trim()));
            htPara.Add("visittime", TextUtility.ReplaceText(txtVisitdate.Text.Trim()));
            try
            {
                if (ordersService.SaveVisit(htPara))
                {
                    InitgdOrderVisitList(tbxordernum.Text);
                }
                else
                {
                    MessageBoxShow("保存失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow("保存出错！错误信息："+ex.Message);
            }
        }
        // 添加
        protected void btn_Add_Click(object sender, EventArgs e)
        {
            clearVisit();
        }
        //删除
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            if (gdVisitList.SelectedRowIndexArray.Length == 0)
            {
                MessageBoxShow("请先选择一条回访记录!");
                return;
            }
            else
            {
                //最新记录才可以删除
                if (Convert.ToInt32(gdVisitList.SelectedRowIndexArray[0].ToString()) != 0)
                {
                    MessageBoxShow("只有最新回访记录，才可进行删除操作!");
                    return;
                }
                //判断只有【回访记录人】为当前用户，才可进行删除
                string visitor = gdVisitList.DataKeys[gdVisitList.SelectedRowIndexArray[0]][1].ToString();
                if (Userinfo.userName != visitor)
                {
                    MessageBoxShow("只有【回访记录人】为当前用户，才可进行删除操作!");
                    return;
                }
            }
            if (ordersService.DeleteVisit(Convert.ToDouble(gdVisitList.DataKeys[gdVisitList.SelectedRowIndexArray[0]][0].ToString())))
            {
                InitgdOrderVisitList(tbxordernum.Text);
            }
            else
            {
                MessageBoxShow("删除失败！");
            }
        }
        #endregion

        #endregion

    }
}