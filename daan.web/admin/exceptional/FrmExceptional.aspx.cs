using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using daan.service.dict;
using daan.domain;
using daan.service.login;
using daan.service.proceed;
using daan.web.code;
using ExtAspNet;
using daan.util.Common;
using daan.util.Web;
namespace daan.web.admin.exceptional
{
    public partial class FrmExceptional : PageBase
    {
        static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        LoginService loginservice = new LoginService();
        readonly DictmemberService memberservice = new DictmemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=250;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                BindDictLab();
                BindDrop();
                BindAddress();
                dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
                datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            }
            else
            {
                if (Request.Form["__EVENTARGUMENT"] == "CancelWindowClose")
                {
                    BindData();
                }
            }
        }

        #region >>>>  事件

        #region >>>>  查询，分页
        //关闭窗体
        protected void WinCancel_Close(object sender, WindowCloseEventArgs e)
        {
            PageContext.RegisterStartupScript("__doPostBack('','CancelWindowClose');");
        }
        //分页
        protected void GridOrders_PageIndexChange(object sender, GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            BindData();
        }

        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbxOrderNum.Text))
                {
                    Convert.ToDouble(tbxOrderNum.Text);
                }                
            }
            catch (Exception) { MessageBoxShow("体检流水号必须为数字",MessageBoxIcon.Information); return; }
            if (!string.IsNullOrWhiteSpace(datebegin.Text) && !string.IsNullOrWhiteSpace(dateend.Text))
            {
                if (this.datebegin.SelectedDate <= this.dateend.SelectedDate)
                {
                    GridOrders.PageIndex = 0;//设置显示第一页
                    BindData();
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

        #endregion

        #region >>>>  查看订单 修改订单，追加项目，作废订单
        //查看订单
        protected void btnSeeDetail_Click(object sender, EventArgs e)
        {
            object[] obj = EditSelectOrdernum(false);
            if (obj == null) { return; }
            string URL = string.Format("~/admin/proceed/ProOrderDetails.aspx?OrderNum={0}", obj[0]);
            OpenWindow("查看订单", URL);
        }

        //修改订单
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object[] obj = EditSelectOrdernum(false);
            if (obj == null) { return; }
            string URL = string.Format("~/admin/proceed/ProOrderModify.aspx?OrderNum={0}", obj[0]);
            OpenWindow("修改订单", URL);
        }

        //批量修改单位名称
        protected void btnEditDictcustomerid_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectOrderNumsByDictcustomerid();
            if (ordernums == string.Empty) { return; }
            string URL = string.Format("~/admin/exceptional/ProDictcustomerModify.aspx?OrderNum={0}", ordernums);
            OpenWindow("批量修改订单信息", URL);
        }

        /// <summary>
        /// 修改单位名称
        /// </summary>
        /// <returns></returns>
        private string GetSelectOrderNumsByDictcustomerid()
        {
            int[] strSelect = GridOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return "";
            }
            string str = string.Empty;
            List<string> cus = new List<string>();
            for (int i = 0; i < strSelect.Length; i++)
            {
                str += GridOrders.DataKeys[strSelect[i]][0].ToString() + ",";
                //也可以考虑场次号相同的才允许批量操作
                if (!cus.Contains(GridOrders.DataKeys[strSelect[i]][5].ToString()))
                {
                    cus.Add(GridOrders.DataKeys[strSelect[i]][5].ToString());
                }
            }
            if (cus.Count !=1)
            {
                MessageBoxShow("您勾选的记录不属于同一个体检单位，不能进行统一批量操作!");
                return "";
            }
            return str.TrimEnd(',');
        }

        //修改单位
        protected void btnCustomer_Click(object sender, EventArgs e)
        {
            string obj = GetSelectOrderNums(false);
            if (string.IsNullOrEmpty(obj)) { return; }
            string URL = string.Format("~/admin/proceed/ProCustomerModify.aspx?OrderNum={0}", obj);
            OpenWindow("修改单位", URL);
        }

        //操作记录
        protected void btnLog_Click(object sender, EventArgs e)
        {
            if (GridOrders.Rows.Count <= 0 || GridOrders.SelectedRowIndexArray.Length <= 0)
            {
                MessageBoxShow("请选择一项进行操作记录查询！");
                return;
            }
            object[] objValue = GridOrders.DataKeys[GridOrders.SelectedRowIndexArray[0]];
            string orderNum = TypeParse.ObjToStr(objValue[0], "");
            WinBillRemark.Hidden = false;
            WinBillRemark.IFrameUrl = "../bill/BillOperationLog.aspx?ordernum=" + orderNum;
            WinBillRemark.Title = "订单日志查询";
        }

        //追加项目
        protected void btnSuperAddition_Click(object sender, EventArgs e)
        {
            object[] obj = EditSelectOrdernum(true);
            if (obj == null) { return; }
            string URL = string.Format("~/admin/proceed/ProOrderAddTest.aspx?OrderNum={0}", obj[0]);
            OpenWindow("追加项目", URL);
        }

        //作废
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectOrderNums(true);
            if (ordernums == string.Empty) { return; }
            WinCancel.IFrameUrl = "../proceed/ProOrdersCancel.aspx?ordernums=" + ordernums;
            WinCancel.Hidden = false;
        }

        #endregion

        #region >>>>  绑定分点 以及选分点筛选单位 省市
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ListItem("全部", "0"));
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }
        private void BindDrop()
        {
            //绑定状态
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ListItem("全部", "-1"));
        }
        
        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }

        /// <summary>选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }

        protected void dropPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pagesize = Convert.ToInt32(dropPageSize.SelectedValue.ToString());
            GridOrders.PageSize = pagesize;
            GridOrders.PageIndex = 0;
            BindData();
        }

        private void BindAddress()
        {
            DropProvinceBinder(dpProvince);
            DropCityBinder(dpProvince, dpCity);
        }

        /// <summary>
        /// 选择省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpCity.Items.Clear();
            DropCityBinder(dpProvince, dpCity);
        }
        #endregion

        #region >>>>  获取选中行记录 操作
        /// <summary>操作选中单行的订单号
        /// 操作选中单行的订单号
        /// </summary>
        private object[] EditSelectOrdernum(bool isestimation)
        {
            int[] strselect = GridOrders.SelectedRowIndexArray;
            if (strselect.Length != 1) { MessageBoxShow("此操作只能勾选一个订单！"); return null; }            

            object[] obj = GridOrders.DataKeys[strselect[0]];
            if (isestimation && CheckIsCancel(obj[1])) { MessageBoxShow("订单已作废,不能进行此操作！"); return null; }

            //if (isestimation && CheckIsFinishCheck(obj[3])) { MessageBoxShow("订单已完成总检,不能进行此操作！"); return null; }
            
            return obj;
        }

        /// <summary>获取选中多行的订单号 逗号间隔
        /// 获取选中多行的订单号 逗号间隔
        /// </summary>
        /// <param name="isCheckCancel">是否检查作废</param>
        /// <returns></returns>
        private string GetSelectOrderNums(bool isfinishcheck)
        {
            int[] strSelect = GridOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return "";
            }
            string str = string.Empty;
            for (int i = 0; i < strSelect.Length; i++)
            {
                str += GridOrders.DataKeys[strSelect[i]][0].ToString() + ",";

                ////判断是否总检完成
                //if (isfinishcheck && CheckIsFinishCheck(GridOrders.DataKeys[strSelect[i]][3]))
                //{
                //    MessageBoxShow("选中订单中有部分已[完成总检],不能作废,请先去掉勾选再操作!"); 
                //    return "";
                //}
                //if (CheckIsCancel(GridOrders.DataKeys[strSelect[i]][1])) 
                //{ 
                //    MessageBoxShow("选中订单中有部分[已作废],请先去掉勾选再操作!"); 
                //    return ""; 
                //}
            }
            return str.TrimEnd(',');
        }
        #endregion

        #endregion

        #region >>>> 方法

        /// <summary>绑定Grid数据
        /// 
        /// </summary>
        /// <param name="ht">查询参数</param>
        /// <param name="isSearch">是否重新查询</param>
        private void BindData(Hashtable ht, bool isSearch)
        {
            GridOrders.DataSource = mamagement.GetManagementOrders(ht);
            GridOrders.DataBind();
            if (isSearch)
            {
                int pagecount = mamagement.GetManagementOrdersCount(ht);
                ht["PageCount"] = GridOrders.RecordCount = pagecount;
            }
            else
            {
                GridOrders.RecordCount = Convert.ToInt32(ht["PageCount"]);
            }
        }

        private void BindData()
        {
            Hashtable ht = getPara();
            GridOrders.RecordCount = mamagement.GetManagementOrdersCount(ht);
            GridOrders.DataSource = mamagement.GetManagementOrders(ht);
            GridOrders.DataBind();
        }
        /// <summary>弹出(个人收费,单位批量上传)窗口         
        /// </summary>
        /// <param name="title">窗口名</param>
        /// <param name="URL">URL</param>
        private void OpenWindow(string title, string URL)
        {
            PageContext.RegisterStartupScript(WindowFrame.GetShowReference(URL, title));
        }

        /// <summary>判断是否作废
        /// 
        /// </summary>
        /// <param name="str">作废状态字符串</param>
        /// <returns></returns>
        private bool CheckIsCancel(object str)
        {
            return str.ToString() == "已作废";
        }

        /// <summary>判断是否完成总检
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool CheckIsFinishCheck(object str)
        {
            return Convert.ToInt32(str) >= (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck;
        }

        /// <summary>记录日志
        /// 
        /// </summary>
        /// <param name="ordernums"></param>
        /// <param name="str"></param>
        private static void JournalLog(string ordernums, string str)
        {
            //日志
            string[] arrorder = ordernums.Split(',');
            for (int i = 0; i < arrorder.Length; i++)
            {
                mamagement.AddOperationLog(arrorder[i], "", "异常订单处理中心", "批量" + str + "[" + ordernums + "]", "节点信息", "批量" + str);
            }
        }
        /// <summary>构造查询参数
        /// 
        /// </summary>
        /// <returns></returns>
        private Hashtable getPara()
        {
            Hashtable ht = new Hashtable();
            ht["DateStart"] = datebegin.Text;
            ht["DateEnd"] = Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd");
            if (DropDictLab.SelectedValue == "0")
                ht["labid"] = Userinfo.joinLabidstr;
            else
                ht["labid"] = DropDictLab.SelectedValue;
            ht["memberid"] = null;
            ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
            ht["ordernum"] = tbxOrderNum.Text;
            ht["status"] = dropStatus.SelectedIndex == 0 ? null : dropStatus.SelectedValue;
            ht["customerid"] = DropCustomer.SelectedValue == "-1" ? null : DropCustomer.SelectedValue;
            ht["customertype"] = null;
            ht["iscancel"] = DropIScancel.SelectedIndex == 0 ? null : DropIScancel.SelectedValue;
            PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            if (dpSFrom.Text != "")
                ht.Add("sdatestart", dpSFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (dpSTo.Text != "")
                ht.Add("sdateend", dpSTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            if (dpProvince.SelectedValue != "-1")
                ht.Add("province",dpProvince.SelectedText);
            if (dpCity.SelectedValue != "-1")
                ht.Add("city",dpCity.SelectedText);
            ht.Add("area", string.IsNullOrEmpty(tbxArea.Text.Trim()) ? null : TextUtility.ReplaceText(tbxArea.Text.Trim()));
            ht.Add("section", string.IsNullOrEmpty(tbxSection.Text.Trim()) ? null : TextUtility.ReplaceText(tbxSection.Text.Trim()));
            ht.Add("batchnumber", string.IsNullOrEmpty(tbxBatchNumber.Text.Trim()) ? null : TextUtility.ReplaceText(tbxBatchNumber.Text.Trim()));
            return ht;
        }
        #endregion
    }
}