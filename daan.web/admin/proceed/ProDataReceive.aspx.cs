using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.service.login;
using System.Collections;
using System.Data;
using daan.util.Web;
using daan.web.code;
using daan.service.proceed;
using ExtAspNet;
using daan.service;
using daan.service.order;
using System.Configuration;
using System.Text;

namespace daan.web.admin.proceed
{
    public partial class ProDataReceive : PageBase
    {
        ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        LoginService loginservice = new LoginService();
        OrderTestService testservice = new OrderTestService();
        OrdersService orderservice = new OrdersService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + dropCustomer.ClientID + "')).listWidth=250;");

                dtpStart.SelectedDate = DateTime.Now.AddDays(-7);
                dtpEnd.SelectedDate = DateTime.Now;
                BindStatus();
                BindDictLab();
                //首次加载调用绑定单位方法
                BindCustomer();
            }
            btnSearchResult.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！", "体检系统");
        }

        #region 绑定分点、体检单位、状态下拉框
        //绑定状态
        private void BindStatus()
        {
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }

        //绑定分点
        private void BindDictLab()
        {
            DDLDictLabBinder(dropLab, true);
        }

        //选择分点绑定体检单位
        protected void dropLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer();
        }

        //绑定体检单位下拉框
        private void BindCustomer()
        {
            DropDictcustomerBinder(dropCustomer, dropLab.SelectedValue.ToString(), true);
        }
        #endregion

        #region 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region gridview绑定数据
        //数据绑定
        private void BindData()
        {
            try
            {
                Hashtable ht = new Hashtable();
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                ht["DateStart"] = dtpStart.Text;
                ht["DateEnd"] = Convert.ToDateTime(dtpEnd.Text).AddDays(1).ToString("yyyy-MM-dd");
                ht["labid"] = dropLab.SelectedValue;
                ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
                ht["customerid"] = dropCustomer.SelectedValue == "-1" ? null : dropCustomer.SelectedValue;
                ht["ordernum"] = TextUtility.ReplaceText(tbxordernum.Text);
                ht["sex"] = null;
                ht["iscancel"] = "0";
                ht["iolis"] = dropIOLIS.SelectedValue == "-1" ? null : dropIOLIS.SelectedValue;
                ht["status"] = dropStatus.SelectedValue == "-1" ? null : dropStatus.SelectedValue;
                ht["pageStart"]= pageUtil.GetPageStartNum();
                ht["pageEnd"] = pageUtil.GetPageEndNum();

                gvList.RecordCount = mamagement.GetManagementOrdersCount(ht);
                gvList.DataSource = mamagement.GetManagementOrders(ht);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }


        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;            
            BindData();
        }

        //行绑定事件
        protected void gvList_RowDataBound(object sender, GridRowEventArgs e)
        {
            if (e.DataItem != null)
            {
                System.Web.UI.WebControls.Label tbxpacs = (System.Web.UI.WebControls.Label)gvList.Rows[e.RowIndex].FindControl("lblpacs");
                System.Web.UI.WebControls.Label tbxlis = (System.Web.UI.WebControls.Label)gvList.Rows[e.RowIndex].FindControl("lbllis");

                if (!string.IsNullOrEmpty(tbxpacs.Text))
                    tbxpacs.Text = GetValue(tbxpacs.Text);
                if (!string.IsNullOrEmpty(tbxlis.Text))
                    tbxlis.Text = GetValue(tbxlis.Text);
            }
        }
        //gridview 绑定是设置b超 lis项绑定内容
        protected string GetValue(string flag)
        {
            string value = "";
            switch (flag)
            {
                case "0":
                    value = "-";
                    break;
                case "1":
                    value = "○";
                    break;
                case "2":
                    value = "√";
                    break;
                case "3":
                    value = "×";
                    break;
            }
            return value;
        }
        #endregion

        #region 查看结果
        protected void btnSearchResult_Click(object sender, EventArgs e)
        {
            if (gvList.Rows.Count <= 0)
                return;

            string ordernum = gvList.Rows[gvList.SelectedRowIndexArray[0]].Values[2].ToString();

            this.WinDataReceive.Hidden = false;
            this.WinDataReceive.Title = "查看结果";
            this.WinDataReceive.IFrameUrl = "ProDataResult.aspx?ordernum=" + ordernum;

        }
        #endregion        

        #region 数据接收
        protected void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selectValue = gvList.SelectedRowIndexArray;
                if (selectValue.Count() <= 0)
                {
                    MessageBoxShow("请选择要接收的项",MessageBoxIcon.Information);
                    return;
                }
                StringBuilder sb = new StringBuilder();
                ProDataReceiveService service = new ProDataReceiveService();

                foreach (int row in selectValue)
                {
                    string statusvalue = gvList.DataKeys[row][1].ToString() == "" ? "0" : gvList.DataKeys[row][1].ToString();
                    if (statusvalue != "10")
                    {
                        sb.AppendFormat("[{0}]", gvList.DataKeys[row][0].ToString());
                        continue;
                    }
                    service.DownResult(true, Userinfo, double.Parse(dropLab.SelectedValue), gvList.DataKeys[row][0].ToString());
                }
                if (sb.ToString().Length > 0)
                {
                    MessageBoxShow("以下条码号非条码已打印状态，不能接收:" + sb);
                   
                }
                else
                {
                    MessageBoxShow("数据已正常接收，请留意接收状态", MessageBoxIcon.Information);
                }
                BindData();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 接受结果后处理
        /// </summary>
        /// <param name="ordernumList"></param>
        /// <param name="i"></param>
        /// <param name="result"></param>
        private void UpdateTreatment(string ordernum, bool result)
        {
            if (result)
            {
                new OrderbarcodeService().AddOperationLog(ordernum, "", "数据接收", "Lis数据接收", "修改留痕", "");
            }

            //当同一订单号ORDERTEST表的所有检验记录取到结果，更新ORDERS表IOLIS状态为1
            int count = testservice.SelectTestResultByOrdernum(ordernum);
            Hashtable htorder = new Hashtable();
            htorder["ordernum"] = ordernum;
            if (!result)
            { htorder["iolis"] = "3"; } //接收失败
            else
            {
                if (count == 0)
                    htorder["iolis"] = "2"; //接收成功
                else if (count >= 1)
                    htorder["iolis"] = "1"; //部分接受
            }
            orderservice.UpdateOrderIOLIS(htorder);
        }
        #endregion
    }
}