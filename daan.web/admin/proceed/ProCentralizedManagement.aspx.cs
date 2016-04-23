using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using daan.util.Web;

namespace daan.web.admin.proceed
{
    public partial class ProCentralizedManagement : PageBase
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
                BindDropStatus();
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
        protected void WinCancel_Close(object sender, WindowCloseEventArgs e)
        {
            PageContext.RegisterStartupScript("__doPostBack('','CancelWindowClose');");
        }

        #region >>>>1、 初始化下拉控件数据 分点、单位、状态
        //绑定状态
        private void BindDropStatus()
        {
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }

        // 绑定分点        
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }
        // 绑定单位        
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }  

        #endregion

        #region >>>>2、 查询，分页，及结果展示
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        //分页
        protected void GridOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;            
            BindData();
        }
        //扫描条码查询
        protected void tbxOrderNum_TriggerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxOrderNum.Text.Trim()))
                return;
            BindData();
            tbxOrderNum.Text = string.Empty;
        }
        //查询结果绑定
        private void BindData()
        {
            Hashtable ht = new Hashtable();
            if (string.IsNullOrWhiteSpace(datebegin.Text) ||string.IsNullOrWhiteSpace(dateend.Text))
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                return;
            }            
            PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);            
            ht["DateStart"] = datebegin.Text;
            ht["DateEnd"] = Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd");
            if (DropDictLab.SelectedValue == "0")
                ht["labid"] = Userinfo.joinLabidstr;
            else
                ht["labid"] = DropDictLab.SelectedValue;
            ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
            ht["ordernum"] = tbxOrderNum.Text;
            ht["status"] = dropStatus.SelectedIndex == 0 ? null : dropStatus.SelectedValue;
            ht["customerid"] = DropCustomer.SelectedValue == "-1" ? null : DropCustomer.SelectedValue;
            ht["iscancel"] = DropIScancel.SelectedIndex == 0 ? null : DropIScancel.SelectedValue;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            GridOrders.RecordCount = mamagement.GetManagementOrdersCount(ht);
            GridOrders.DataSource = mamagement.GetManagementOrders(ht);
            GridOrders.DataBind();
        }

        #endregion

        #region >>>>3、按钮事件
        //个人收费
        protected void btnCharge_Click(object sender, EventArgs e)
        {
            object[] obj = EditSelectOrdernum(true);
            if (obj == null) { return; }
            string URL = string.Format("../bill/BillIndividualCharge.aspx?ordernum={0}&dictlabname={1}", obj[0], Server.UrlEncode(obj[2].ToString()));
            OpenWindow("个人收费", URL);
        }       

        // 单位上传
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            OpenWindow("单位批量上传", "ProBulkImport.aspx");
        }
        
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
            object[] obj = EditSelectOrdernum(true);
            if (obj == null) { return; }
            string URL = string.Format("~/admin/proceed/ProOrderModify.aspx?OrderNum={0}", obj[0]);
            OpenWindow("修改订单", URL);
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
            string str = mamagement.SelectOrderbarcodeByCollected(ordernums);
            if (str != null && str != "")
            {
                MessageBoxShow(string.Format("订单[{0}]已经采过血不能作废！请先去掉勾选重试！", str)); return;
            }
            WinCancel.IFrameUrl = "ProOrdersCancel.aspx?ordernums=" + ordernums;
            WinCancel.Hidden = false;
        }

        //打印条码
        protected void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            foreach (int rowIndex in GridOrders.SelectedRowIndexArray)
            {
                GridRow row=GridOrders.Rows[rowIndex];
                if (!(row.Values[5].Contains("已登记") || row.Values[5].Contains("条码已打印")))
                {
                    MessageBoxShow(string.Format("条码号:{0} 姓名:{1} 已打印过条码，条码补打模块可补打条码", row.Values[0], row.Values[1]));
                    return;
                }
            }

            SetInitlocalsetting(hdMac.Text);//设置打印所需的客户端配置信息

            string ordernums = GetSelectOrderNums(false);
            if (ordernums == string.Empty) { return; }
            DataTable dtSource = new OrderbarcodeService().GetPrintBarcodeData(new Hashtable() { { "ordernum", ordernums }, { "orderbarcode", null } });

            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                string testnames = dtSource.Rows[i]["TESTNAMES"].ToString();
                dtSource.Rows[i]["AGE"] = WebUI.GetAge(dtSource.Rows[i]["AGE"]);//处理年龄                
                dtSource.Rows[i]["COLLECTDATE"] = dtSource.Rows[i]["COLLECTDATE"].ToString();
                dtSource.Rows[i]["TESTNAMES"] = testnames.TrimEnd(',');
                dtSource.Rows[i]["COUNT"] = String.Format("共{0}项", testnames.TrimEnd(',').Split(',').Length);
            }


            //修改订单状态为[条码已打印]（已登记的才改）            

            new OrdersService().EditStatusByOldStatus(new Hashtable() { { "ordernum", ordernums }, { "status", (int)ParamStatus.OrdersStatus.BarCodePrint }, { "oldstatus", (int)ParamStatus.OrdersStatus.Register }, });
            //后续调用柯木朗方法打印
            //..........................            

            commonReport.PrintBarCode(dtSource, Userinfo);
            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintBarCode(\'{0}\',\'{1}\');", CommonReport.printer, CommonReport.json));

            //记录日志
            JournalLog(ordernums, "打印条码");
        }

        //在条码上打印体检号与姓名
        protected void btnPrintOrderNum_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//设置打印所需的客户端配置信息

            int[] strSelect = GridOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return;
            }

            //构建待打印临时表
            DataTable dtOrderNum = new DataTable("dtOrderNumAndUser");
            dtOrderNum.Columns.Add("ordernum", typeof(string));
            dtOrderNum.Columns.Add("realname", typeof(string));
            for (int i = 0; i < strSelect.Length; i++)
            {
                object[] obj = GridOrders.DataKeys[strSelect[i]];
                DataRow dr = dtOrderNum.NewRow();
                dr["ordernum"] = obj[0].ToString();
                dr["realname"] = obj[4].ToString();

                dtOrderNum.Rows.Add(dr);
            }
            commonReport.PrintBarCode(dtOrderNum, Userinfo);
            ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintOrderNumAndUserName(\'{0}\',\'{1}\');", CommonReport.printer, CommonReport.json));
        }

        //打印指引单
        protected void btnPrintSingle_Click(object sender, EventArgs e)
        {
            SetInitlocalsetting(hdMac.Text);//设置打印所需的客户端配置信息

            string ordernums = GetSelectOrderNums(false);
            if (ordernums == string.Empty) { return; }
            DataTable dtSource = mamagement.GetPrintDirectData(ordernums);
            string[] arrordernum = ordernums.Split(',');
            Report report = new Report();

            for (int i = 0; i < arrordernum.Length; i++)
            {
                DataSet ds = new DataSet();
                DataTable dt = dtSource.Select("ordernum=" + arrordernum[i]).CopyToDataTable();
                DataTable dthead = dt.Clone();//复制表结构
                dt.TableName = "dtHealthGuideResult";
                dthead.TableName = "dtHealthGuideTitle";
                DataRow dr = dt.Rows[0];
                dr["AGE"] = WebUI.GetAge(dr["AGE"]);//处理年龄
                dr["PHONE"] = dr["PHONE"].ToString().TrimEnd('/').TrimStart('/');//处理电话格式
                dthead.ImportRow(dr);//复制一行

                //title
                DataTable dttitle = new DataTable();
                dttitle.TableName = "dtTitle";
                dttitle.Columns.Add("titleName", typeof(string));//报告标题
                DataRow dttitledr = dttitle.NewRow();
                dttitledr[0] = dt.Rows[0]["titleName"];
                dttitle.Rows.Add(dttitledr);
                //title  end

                ds.Tables.Add(dthead);
                ds.Tables.Add(dt);
                ds.Tables.Add(dttitle);
                //后续调用柯木朗方法打印
                //..........................

                report = commonReport.GetReportByDataset("25", ds, 1);
                commonReport.PrintReport2(report.SaveToString(), commonReport.dsGetReportData.Copy(), Userinfo);
                ExtAspNet.PageContext.RegisterStartupScript(string.Format(" PrintReport(\'{0}\',\'{1}\',\'{2}\');", CommonReport.printer, CommonReport.json, CommonReport.dsjson));
            }
            //记录日志
            JournalLog(ordernums, "打印指引单");
        }


        /// <summary>弹出(个人收费,单位批量上传)窗口
        /// 
        /// </summary>
        /// <param name="title">窗口名</param>
        /// <param name="URL">URL</param>
        private void OpenWindow(string title, string URL)
        {
            PageContext.RegisterStartupScript(WindowFrame.GetShowReference(URL, title));
        }
        #endregion          
       
        #region >>>>4、 选中行的验证及多选时的订单号组合

        /// <summary>验证选中行能否进行相关的操作
        /// 
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
        /// 
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

        /// <summary>判断是否作废
        /// 
        /// </summary>
        /// <param name="str">作废状态字符串</param>
        /// <returns></returns>
        private bool CheckIsCancel(object str)
        {
            return str.ToString() == "已作废";
        }

        /// <summary>是否已完成总检
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool CheckIsFinishCheck(object str)
        {
            return Convert.ToInt32(str) >= (int)daan.service.common.ParamStatus.OrdersStatus.FinishCheck;
        }        
        #endregion

        #region >>>>5、写日志
        /// <summary>写日志
        /// 
        /// </summary>
        /// <param name="ordernums"></param>
        /// <param name="str"></param>
        private static void JournalLog(string ordernums, string str)
        {
            string[] arrorder = ordernums.Split(',');
            for (int i = 0; i < arrorder.Length; i++)
            {
                mamagement.AddOperationLog(arrorder[i], "", "体检集中管理", "批量" + str + "[" + ordernums + "]", "节点信息", "批量" + str);
            }
        }
        #endregion

        // 档案
        protected void btnArchive_Click(object sender, EventArgs e)
        {
            int[] strSelect = GridOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return;
            }
            object[] obj = GridOrders.DataKeys[strSelect[0]];
            string URL = "ProMemberFile.aspx?Mid=" + obj[5].ToString();
            PageContext.RegisterStartupScript(WindowFrame.GetShowReference(URL));
        }
    }
}