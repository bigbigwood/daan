using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using System.Collections;
using daan.service.order;
using System.Data;
using daan.domain;
using ExtAspNet;
//屏蔽按需加载
// AutoPostBack="true" OnTabIndexChanged="TabStrip1_TabIndexChanged"
namespace daan.web.admin.analyse
{
    public partial class AnaResultSum_ArchivesWin : PageBase
    {
        /// <summary>
        /// 初始化tab判断标志
        /// </summary>
        void InitHt()
        {
            ViewState.Add("gdHEALTHRECORDS", 0);
            ViewState.Add("gdPASTORDERS", 0);
        }
        static DictMEDHistoryService dictMEDHistoryService = new DictMEDHistoryService();
        static OrderlabdeptresultService orderlabdeptresultService = new OrderlabdeptresultService();
        static OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
        static OrdernexttestService _ordernexttestService = new OrdernexttestService();
        static OrderresultcommentService orcs = new OrderresultcommentService();
        static OrderTestService orderTestService = new OrderTestService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["dictmemberid"] != null)
                {
                    //InitHt();
                    string mid = Request.QueryString["dictmemberid"].ToString();
                    hidMemberID.Text = mid;
                    BindGrid(mid);
                }
            }
        }

        /// <summary>
        /// 绑定选项卡数据
        /// </summary>
        void BindGrid(string dictmemberid)
        {
            BindHEALTHRECORDS(dictmemberid);
            BindPastOrders(dictmemberid);
        }
        private void BindHEALTHRECORDS(string dictmemberid)
        {
            gdHEALTHRECORDS.DataSource = dictMEDHistoryService.GetHealthRecordsDataList(dictmemberid);
            gdHEALTHRECORDS.DataBind();
        }
        private void BindPastOrders(string dictmemberid)
        {
            gdPASTORDERS.DataSource = dictMEDHistoryService.GetDictPastOrdersNoPages(dictmemberid);
            gdPASTORDERS.DataBind();
        }
        /// <summary>
        /// 加载用户需要的tab数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStrip1_TabIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["dictmemberid"] == null)
            {
                return;
            }

            switch (TabStrip1.ActiveTabIndex)
            {
                case 0://既往病史
                    {
                        if (ViewState["gdHEALTHRECORDS"].ToString() == "1")//只加载一次
                        {
                            return;
                        }
                        gdHEALTHRECORDS.DataSource = dictMEDHistoryService.GetDictmedHistoryDataList(ViewState["dictmemberid"].ToString());
                        gdHEALTHRECORDS.DataBind();
                        ViewState["gdHEALTHRECORDS"] = 1;
                    }
                    ;
                    break;
                case 1://既往体检档案
                    {
                        if (ViewState["gdPASTORDERS"].ToString() == "1")
                        {
                            return;
                        }
                        gdPASTORDERS.DataSource = dictMEDHistoryService.GetDictPastOrdersNoPages(ViewState["dictmemberid"].ToString());
                        gdPASTORDERS.DataBind();
                        ViewState["gdPASTORDERS"] = 1;
                        break;
                    }
            }
        }

        #region >>>>>健康档案

        //添加
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string url = "../dict/DictHealthRecords_Window.aspx?mid=" + hidMemberID.Text;
            PageContext.RegisterStartupScript(win1.GetShowReference(url));
        }
         //删除
        protected void btnDel_Click(object sender, EventArgs e)
        {
            int[] selectstr = gdHEALTHRECORDS.SelectedRowIndexArray;
            if (selectstr.Length != 1)
            {
                MessageBoxShow("请选中一条既往病史记录删除!"); return;
            }
            string id = gdHEALTHRECORDS.DataKeys[selectstr[0]][0].ToString();
            if (!dictMEDHistoryService.DeleteHealthRecords(id))
            {
                MessageBoxShow("删除失败，请重试!");
                return;
            }
            BindHEALTHRECORDS(hidMemberID.Text);
        }
        //关闭
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
        #endregion

        #region >>>>>> 既往体检档案
        protected void gdPASTORDERS_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            string ordernum = string.Empty;
            if (gdPASTORDERS.DataKeys[e.RowIndex][0] != null)
                ordernum = gdPASTORDERS.DataKeys[e.RowIndex][0].ToString();
            else
                return;
            LoadAllTabData(ordernum);
        }
        /// <summary>
        /// 加载体检详细信息(科室小结、诊断信息、详细结果、总体评价、推荐项目)
        /// </summary>
        /// <param name="ordernum"></param>
        private void LoadAllTabData(string ordernum)
        {
            InitgdOrderlabdeptResult(ordernum);//科室小结
            InitgdOrderdiagnosis(ordernum);//诊断信息
            InitOrdertest(ordernum);//详细结果
            InitgdOrderResultComment(ordernum);//总体评价
            InitgdOrdernexttest(ordernum);//推荐项目
        }
        /// <summary>
        /// 科室小结
        /// </summary>
        /// <param name="ordernum"></param>
        private void InitgdOrderlabdeptResult(string ordernum)
        {
            gdOrderlabdeptResult.DataSource = orderlabdeptresultService.DataForOrderlabdept(ordernum);
            gdOrderlabdeptResult.DataBind();
        }
        /// <summary>
        /// 诊断信息
        /// </summary>
        /// <param name="ordernum"></param>
        private void InitgdOrderdiagnosis(string ordernum)
        {
            gdOrderdiagnosis.DataSource = orderdiagnosisService.SelectOrderdiagnosis(ordernum);
            gdOrderdiagnosis.DataBind();
        }
        /// <summary>
        /// 推荐项目
        /// </summary>
        /// <param name="ordernum"></param>
        private void InitgdOrdernexttest(string ordernum)
        {
            gdOrdernexttest.DataSource = _ordernexttestService.SelectOrdernexttest(ordernum);
            gdOrdernexttest.DataBind();
        }
        /// <summary>
        /// 总体评价
        /// </summary>
        /// <param name="ordernum"></param>
        private void InitgdOrderResultComment(string ordernum)
        {
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
        /// 详细结果
        /// </summary>
        /// <param name="ordernum"></param>
        private void InitOrdertest(string ordernum)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ordernum", ordernum);
            DataTable dt=orderTestService.DataForOrderLabdeptresult(ht);
            gdOrdertest.DataSource = dt;
            gdOrdertest.DataBind();
        }
        #endregion
    }
}