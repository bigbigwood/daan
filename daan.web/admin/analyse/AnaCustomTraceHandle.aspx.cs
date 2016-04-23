using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.domain;
using System.Collections;
using daan.util.Web;
using daan.service.order;
using ExtAspNet;
using System.Data;
using daan.web.code;

namespace daan.web.admin.analyse
{
    public partial class AnaCustomTraceHandle : PageBase
    {

        #region 字段
        DictlabService dictlabService = new DictlabService();
        OrdersService ordersService = new OrdersService();

        OrderserviceinfoService orderserviceinfoService = new OrderserviceinfoService();
        #endregion

        #region 页面逻辑方法
        /// <summary>
        /// 绑定分点
        /// </summary>
        void BindDrop()
        {

            //var enumerator = dictlabService.GetDictlabList();

            //dropDictLab.Items.Add(new ExtAspNet.ListItem("全部", "-2") { Selected=true});
            //foreach (var item in enumerator)
            //{
            //    dropDictLab.Items.Add(new ExtAspNet.ListItem(item.Labname, item.Dictlabid.ToString()));
            //}
            DDLDictLabBinder(dropDictLab, true);
        }

        /// <summary>
        /// 初始化ordernum，reviewstatus状态
        /// </summary>
        void InitParm()
        {
            ViewState.Add("ordernum", "No");
            ViewState.Add("reviewstate", "No");
            ViewState.Add("orderbarcode", "No");
        }


        /// <summary>
        /// 加载tab页时的判断参数
        /// </summary>
        void InitTab()
        {
            ViewState.Add("tab1", 0);
            ViewState.Add("tab2", 0);
        }

        /// <summary>
        /// 根据订单号初始化客户体检报告
        /// </summary>
        /// <param name="ordernum"></param>
        void InitReport(string ordernum)
        {

            tabReport.IFrameUrl = "../report/RepShowView.aspx?reportType=1&order_num=" + ordernum;
        }

        /// <summary>
        /// 清除tab里面的东西
        /// </summary>
        void ClearTabData()
        {
            gdOrderserviceinfo.DataSource = new List<object>();
            gdOrderserviceinfo.DataBind();
            tabReport.IFrameUrl = "../report/RepShowView.aspx?reportType=1&order_num=-NoData";
            InitParm();
        }
        /// <summary>
        /// 根据订单号初始化客户追踪内容
        /// </summary>
        /// <param name="ordernum"></param>
        void InitOrderserviceinfo(string ordernum)
        {
            gdOrderserviceinfo.DataSource = orderserviceinfoService.GetOrderserviceinfos(ordernum);
            gdOrderserviceinfo.DataBind();
        }

        /// <summary>
        /// 开始加载数据
        /// </summary>
        void InitPageData()
        {
            BindDrop();
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;
            //BindgdOrders();
        }

        /// <summary>
        /// 绑定订单主表的数据
        /// </summary>
        void BindgdOrders()
        {
            Hashtable ht = GetParm();
            //设置页数
            gdOrders.RecordCount = int.Parse(ordersService.CountForAnaCustomTraceHandle(ht).Rows[0][0].ToString());
            gdOrders.DataSource = ordersService.DataForAnaCustomTraceHandle(ht);
            gdOrders.DataBind();
        }

        /// <summary>
        /// 取得查询参数
        /// </summary>
        /// <returns></returns>
        Hashtable GetParm()
        {
            Hashtable _parameterCache = new Hashtable();
            //复查时间
            _parameterCache.Add("StartDate", dpFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            _parameterCache.Add("EndDate", dpTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            if (dropDictLab.SelectedText.Length > 0) //分点选择
            {
                _parameterCache.Add("dropDictLab", dropDictLab.SelectedValue);
            }
            else
            {
                _parameterCache.Add("dropDictLab", null);
            }
            //跟进状态
            if (dropStatus.SelectedText != "全部")
            {
                _parameterCache.Add("reviewstate", dropStatus.SelectedValue);

            }
            else
            {
                _parameterCache.Add("reviewstate", null);

            }
            PageUtil pageUtil = new PageUtil(gdOrders.PageIndex, gdOrders.PageSize);
            _parameterCache.Add("pageStart", pageUtil.GetPageStartNum());
            _parameterCache.Add("pageEnd", pageUtil.GetPageEndNum());
            return _parameterCache;
        }
        #endregion



        #region 页面事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitParm();
                InitPageData();
            }
        }



        /// <summary>
        /// 完成跟进事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0) //没有订单号的时候
            {
                return;
            }

            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            string reviewstate = gdOrders.DataKeys[selectIndex][1].ToString();
            //已完成跟进的体检号不用再完成跟进
            if (reviewstate == "1")
            {
                return;
            }

            if (ordersService.EditReviewstate(ordernum))
            {
                ordersService.AddOperationLog(ordernum, null, "客户追踪处理", "完成跟进",
                    "修改", "");
                BindgdOrders();
                ClearTabData();
                MessageBoxShow("完成跟进");
            }
            else
            {
                MessageBoxShow("不可以完成跟进");
            }


        }
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.dpFrom.Text != "" && this.dpTo.Text != "")
            {
                if (this.dpFrom.SelectedDate <= this.dpTo.SelectedDate)
                {
                    BindgdOrders();
                    ClearTabData();
                }
                else
                {

                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
            }
            
        }
        /// <summary>
        /// 订单主表分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gdOrders.PageIndex = e.NewPageIndex;
            BindgdOrders();
            ClearTabData();
        }



        /// <summary>
        /// 订单主表Grid，行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdOrders_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            // InitTab();
            object ordernum = gdOrders.Rows[e.RowIndex].DataKeys[0];
            if (ordernum == null)
            {
                return;
            }
            BindTab(ordernum.ToString());
        }

        /// <summary>
        /// 把数据加载进 Tab
        /// </summary>
        void BindTab(string ordernum)
        {

            InitOrderserviceinfo(ordernum);
            InitReport(ordernum);
        }

        /// <summary>
        /// 保存客户跟进内容事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gdOrders.SelectedRowIndexArray.Length == 0) //没有订单号的时候
            {
                return;
            }

            int selectIndex = gdOrders.SelectedRowIndexArray[0];
            string ordernum = gdOrders.DataKeys[selectIndex][0].ToString();
            Hashtable ht2 = new Hashtable();
            Orderserviceinfo orderserviceinfo = new Orderserviceinfo();
            UserInfo userInfo = (UserInfo)Session["UserInfo"];
            orderserviceinfo.Dictuserid = userInfo.userId.ToString();
            orderserviceinfo.Ordernum = ordernum;
            orderserviceinfo.Servicecontent = tbServicecontent.Text;
            bool flag = false;
            if (dpRerundate.SelectedDate.HasValue)//预约复查时间不为空时
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("Rerundate", dpRerundate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                ht1.Add("Ordernum", ordernum);
                flag = (ordersService.EditRerundate(ht1)) && (orderserviceinfoService.AddOrderserviceinfo(orderserviceinfo));
            }
            else
            {
                flag = orderserviceinfoService.AddOrderserviceinfo(orderserviceinfo);

            }
            if (flag)
            {
                string content = "新加跟进内容:" + tbServicecontent.Text;
                if (dpRerundate.SelectedDate.HasValue)//预约复查时间不为空时
                {
                    content += "  预约复查时间:" + dpRerundate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                InitOrderserviceinfo(ordernum);
                orderserviceinfoService.AddOperationLog(ordernum, null, "客户追踪处理", content,
                   "新增", "");


            }
            else
            {
                MessageBoxShow("保存出错，请联系管理员解决！");
                return;
            }
        }


        /// <summary>
        /// tab页事件,是加载客户跟进内容，还是加载体检报告单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStrip1_TabIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["ordernum"].ToString() == "No")
            {
                return;
            }
            switch (TabStrip1.ActiveTabIndex)
            {
                case 0://加载客户跟进内容
                    {
                        if (ViewState["tab1"].ToString() == "1")
                        {
                            return;
                        }
                        InitOrderserviceinfo(ViewState["ordernum"].ToString());
                        ViewState.Add("tab1", 1);
                    };
                    break;
                case 1://加载体检报告单
                    {
                        if (ViewState["tab2"].ToString() == "1")
                        {
                            return;
                        }
                        InitReport(ViewState["ordernum"].ToString());
                        ViewState.Add("tab2", 1);
                    };
                    break;
            }


        }
        #endregion




    }
}