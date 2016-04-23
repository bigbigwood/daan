using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.proceed;
using System.Collections;
using ExtAspNet;
using System.Data;
using daan.domain;
using daan.service.login;
using daan.web.code;
using daan.service.dict;
using FastReport;
using daan.service.order;
using daan.service.common;

namespace daan.web.admin.proceed
{
    public partial class ProExceptionReport : PageBase
    {
        static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();
        static CommonReport commonReport = new CommonReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + DropCustomer.ClientID + "')).listWidth=250;");
            if (!IsPostBack)
            {
                BindDictLab();
                dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
                datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");

            }
            else { SelectMember(); }

        }

        #region >>>> fenghp 事件

        #region >>>> fenghp 查询，分页

        //分页
        protected void GridOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            Hashtable ht = ViewState["SearchWhere"] as Hashtable;
            ht["pageStart"] = GridOrders.PageSize * (GridOrders.PageIndex);
            ht["pageEnd"] = GridOrders.PageSize * (GridOrders.PageIndex + 1);
            BindData(ht, false);
        }

        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            double? memberid = null;
            try
            {
                if (tbxOrderNum.Text != string.Empty)
                {
                    Convert.ToDouble(tbxOrderNum.Text);
                }
                if (tbxmember.Text != string.Empty)
                {
                    memberid = Convert.ToDouble(tbxmember.Text);
                }
            }
            catch (Exception) { MessageBoxShow("体检流水号和会员编号只能为数字"); return; }
            if (this.datebegin.Text != "" && this.dateend.Text != "")
            {
                if (this.datebegin.SelectedDate <= this.dateend.SelectedDate)
                {
                    Hashtable ht = new Hashtable();
                    ht["pageStart"] = 0;
                    ht["pageEnd"] = GridOrders.PageSize;
                    ht["DateStart"] = datebegin.Text;
                    ht["DateEnd"] = Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd");
                    if (DropDictLab.SelectedValue == "0")
                        ht["labid"] = Userinfo.joinLabidstr;
                    else
                        ht["labid"] = DropDictLab.SelectedValue;
                    ht["memberid"] = memberid;
                    ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
                    ht["ordernum"] = tbxOrderNum.Text;
                    ht["status"] = DropStatus.SelectIndex == 0 ? null : DropStatus.SelectedValue;
                    ht["customerid"] = DropCustomer.SelectedValue == "-1" ? null : DropCustomer.SelectedValue;                    
                    ht["iscancel"] = DropIScancel.SelectedIndex == 0 ? null : DropIScancel.SelectedValue;
                    ViewState["SearchWhere"] = ht;
                    GridOrders.PageIndex = 0;//设置显示第一页
                    BindData(ht, true);
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

        #region >>>> fenghp 查看订单 作废订单 取消作废订单
        //查看订单
        protected void btnSeeDetail_Click(object sender, EventArgs e)
        {
            object[] obj = EditSelectOrdernum(false);
            if (obj == null) { return; }
            string URL = string.Format("~/admin/proceed/ProOrderDetails.aspx?OrderNum={0}", obj[0]);
            OpenWindow("查看订单", URL);
        }


        //作废
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectOrderNums(true);
            bool falg = true;
            if (ordernums == string.Empty) { return; }
            string str = mamagement.SelectOrderbarcodeByCollected(ordernums);
            if (str != null && str != "")
            {
                MessageBoxShow(string.Format("订单[{0}]已经采过血不能作废！请先去掉勾选重试！", str)); return;
            }
            if (mamagement.UpdateOrdersCancellation(Userinfo.userName, ordernums,""))
            {
                //刷记录
                falg = true;
                BindData((ViewState["SearchWhere"] as Hashtable), false);
            }
            else
            {
                falg = false;
                MessageBoxShow("作废订单失败，请刷新页面重试！"); return;
            }

            str = mamagement.SelectOrderbarcodeByBill(ordernums);
            if (str != null && str != "")
            {
                if (falg)
                    MessageBoxShow(string.Format("作废成功,订单[{0}]已经出过账单，请及时处理！", str));
            }
            else
            {
                if (falg)
                    MessageBoxShow("订单作废成功！");
            }


            //记录日志
            JournalLog(ordernums, "作废订单");
        }
        //取消作废
        protected void btnValid_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectIsCalcelOrderNums();
            if (ordernums == string.Empty) { return; }
            string str = string.Empty;
            if (mamagement.UpdateOrdersValid(Userinfo.userName, ordernums))
            {
                BindData((ViewState["SearchWhere"] as Hashtable), false);
                MessageBoxShow("订单取消作废成功！");
            }
            else
            {
                MessageBoxShow("取消作废订单失败，请刷新页面重试！"); return;
            }

            //记录日志
            JournalLog(ordernums, "取消作废订单");

        }

        #endregion

        #region >>>> fenghp 选择会员窗口(选择&关闭 事件)
        ///选择会员
        protected void btnSelectMember_Click(object sender, EventArgs e)
        {
            int[] selectrow = GridMember.SelectedRowIndexArray;

            if (selectrow.Length <= 0) { MessageBoxShow("您还没有选择会员"); return; }
            selectMemberByRowIndex(selectrow[0]);
        }

        ///关闭会员选择窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            winMemberSelect.Hidden = true;
        }

        //双击行事件
        protected void GridMember_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            selectMemberByRowIndex(e.RowIndex);
        }

        private void selectMemberByRowIndex(int index)
        {
            int memberid = Convert.ToInt32(GridMember.DataKeys[index][0]);
            Dictmember member = memberservice.GetMemberById(memberid);

            if (member == null) { MessageBoxShow(string.Format("不存在ID为[{0}]的会员", memberid)); return; }
            tbxmember.Text = member.Dictmemberid.ToString();
            tbxName.Text = member.Realname;
            winMemberSelect.Hidden = true;
        }
        #endregion

        #region >>>> fenghp 绑定分点 以及选分点筛选单位
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "0"));
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
            DropCustomer.Readonly = true;//初始设置不可选择

        }

        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {

            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
            if (labid == 0)
            {
                List<Dictlab> dictList = loginservice.GetPermissionDictlab(Userinfo);
                List<Dictcustomer> dictcustomerback = new List<Dictcustomer>();
                foreach (Dictlab dict in dictList)
                {
                    List<Dictcustomer> dictcustomerfirt = CustomerList.FindAll(c => c.Dictlabid == dict.Dictlabid);
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        dictcustomerback.Add(dictcust);
                    }
                }

                DropCustomer.DataSource = dictcustomerback;// CustomerList.Where<Dictcustomer>(c => c.Dictlabid == labid); 
                DropCustomer.DataValueField = "Dictcustomerid";
                DropCustomer.DataTextField = "Customername";
                DropCustomer.DataBind();
                DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
            else
            {
                DropCustomer.DataSource = CustomerList.Where<Dictcustomer>(c => c.Dictlabid == labid);
                DropCustomer.DataValueField = "Dictcustomerid";
                DropCustomer.DataTextField = "Customername";
                DropCustomer.DataBind();
                DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
        }

        /// <summary>选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }

        /// <summary>选择客户类型
        protected void DropCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropCustomerType.SelectedValue == "1")
            {
                DropCustomer.Readonly = false;
            }
            else
            {
                if (DropCustomer.Items.Count > 0) { DropCustomer.SelectedValue = "-1"; }
                DropCustomer.Readonly = true;
            }
        }


        #endregion

        #region >>>> fenghp 获取选中行记录 操作
        /// <summary>操作选中单行的订单号
        /// 操作选中单行的订单号
        /// </summary>
        private object[] EditSelectOrdernum(bool isestimation)
        {
            int[] strselect = GridOrders.SelectedRowIndexArray;
            if (strselect.Length > 1) { MessageBoxShow("此操作只能勾选一个订单！"); return null; }
            if (strselect.Length <= 0) { MessageBoxShow("此操作必须勾选一个订单！"); return null; }

            object[] obj = GridOrders.DataKeys[strselect[0]];
            if (isestimation && CheckIsCancel(obj[1])) { MessageBoxShow("订单已作废,不能进行此操作！"); return null; }

            if (isestimation && CheckIsFinishCheck(obj[3])) { MessageBoxShow("订单已完成总检,不能进行此操作！"); return null; }

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

                //判断是否总检完成
                if (isfinishcheck && CheckIsFinishCheck(GridOrders.DataKeys[strSelect[i]][3]))
                {
                    MessageBoxShow("选中订单中有部分已[完成总检],不能作废,请先去掉勾选再操作!"); return "";
                }
                if (CheckIsCancel(GridOrders.DataKeys[strSelect[i]][1])) { MessageBoxShow("选中订单中有部分[已作废],请先去掉勾选再操作!"); return ""; }
            }
            return str.TrimEnd(',');
        }

        /// <summary>获取选中多行已经作废的订单号 逗号间隔
        /// 获取选中多行已经作废的订单号 逗号间隔
        /// </summary>
        /// <returns></returns>
        private string GetSelectIsCalcelOrderNums()
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

                //判断是否已经作废
                if (!CheckIsCancel(GridOrders.DataKeys[strSelect[i]][1])) { MessageBoxShow("选中订单中有部分[未作废],请先去掉勾选再操作!"); return ""; }
            }
            return str.TrimEnd(',');
        }
        #endregion

        #endregion

        #region >>>> fenghp 方法

        /// <summary>绑定Grid数据
        /// 绑定Grid数据
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

        /// <summary>选择会员
        /// 选择会员
        /// </summary>
        private void SelectMember()
        {
            if (Request.Form["__EVENTTARGET"] == tbxmember.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey")
            {
                Dictmember m = new Dictmember();
                m.Realname = tbxmember.Text.Trim();
                List<Dictmember> memberlist = memberservice.GetDictmemberList(m);
                if (memberlist.Count == 0)
                {
                    MessageBoxShow("没有搜索到匹配的会员！");
                    tbxmember.Text = string.Empty;
                    return;
                }
                else if (memberlist.Count == 1)
                {
                    Dictmember member = memberlist[0];
                    tbxmember.Text = member.Dictmemberid.ToString();
                    tbxName.Text = member.Realname;
                }
                else
                {
                    GridMember.DataSource = memberlist;
                    GridMember.DataBind();
                    winMemberSelect.Hidden = false;
                }
            }
        }

        /// <summary>弹出(个人收费,单位批量上传)窗口
        /// 弹出(个人收费,单位批量上传)窗口
        /// </summary>
        /// <param name="title">窗口名</param>
        /// <param name="URL">URL</param>
        private void OpenWindow(string title, string URL)
        {
            PageContext.RegisterStartupScript(WindowFrame.GetShowReference(URL, title));
        }



        /// <summary>判断是否作废
        /// 判断是否作废
        /// </summary>
        /// <param name="str">作废状态字符串</param>
        /// <returns></returns>
        private bool CheckIsCancel(object str)
        {
            return str.ToString() == "已作废";
        }

        private bool CheckIsFinishCheck(object str)
        {
            return Convert.ToInt32(str) >= (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck;
        }

        //记录日志
        private static void JournalLog(string ordernums, string str)
        {
            //日志
            string[] arrorder = ordernums.Split(',');
            for (int i = 0; i < arrorder.Length; i++)
            {
                mamagement.AddOperationLog(arrorder[i], "", "异常报告", "批量" + str + "[" + ordernums + "]", "节点信息", "批量" + str);
            }
        }
        #endregion
    }
}