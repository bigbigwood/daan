using System;
using System.Collections.Generic;
using System.Linq;
using daan.service.proceed;
using System.Collections;
using ExtAspNet;

namespace daan.web.admin.exceptional
{
    public partial class ProDictcustomerModify : PageBase
    {
        readonly static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        protected void Page_Load(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=250;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                if (Request.QueryString["OrderNum"] != null && Request.QueryString["OrderNum"] != string.Empty)
                {
                    BindDictLab();
                    string OrderNum = Request.QueryString["OrderNum"];
                    //string OrderNums = String.Format("'{0}'", OrderNum.Replace(",", "','"));
                    hidOrderNums.Text = OrderNum;
                    Hashtable ht = new Hashtable();
                    ht["pageStart"] = 0;
                    ht["pageEnd"] = GridOrders.PageSize;
                    ht["ordernum"] = OrderNum;
                    ViewState["SearchWhere"] = ht;
                    GridOrders.PageIndex = 0;//设置显示第一页
                    BindData(ht, true);
                }
            }
        }

        /// <summary>
        /// 绑定Grid数据
        /// </summary>
        /// <param name="ht">查询参数</param>
        /// <param name="isSearch">是否重新查询</param>
        private void BindData(Hashtable ht, bool isSearch)
        {
            GridOrders.DataSource = mamagement.GetManagementOrdersByDictmemberid(ht);
            GridOrders.DataBind();
            if (isSearch)
            {
                int pagecount = mamagement.GetManagementOrdersByDictmemberidCount(ht);
                ht["PageCount"] = GridOrders.RecordCount = pagecount;
            }
            else
            {
                GridOrders.RecordCount = Convert.ToInt32(ht["PageCount"]);
            }
        }
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            //DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "0"));
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        /// <summary>
        /// 选择分点事件 绑定单位
        /// </summary>
        protected void DropDictLab_SelectedIndexChangeds(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }
        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), false);
        }

        protected void ck1_CheckedChanged(object sender, EventArgs e)
        {
            if (ck1.Checked)
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = true;
                txtAddress.Focus();
            }
            else
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = false;
                txtAddress.Text = txtRecName.Text = txtTelphone.Text = string.Empty;
            }
        }

        //分页
        protected void GridOrders_PageIndexChange(object sender, GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            Hashtable ht = ViewState["SearchWhere"] as Hashtable;
            ht["pageStart"] = GridOrders.PageSize * (GridOrders.PageIndex);
            ht["pageEnd"] = GridOrders.PageSize * (GridOrders.PageIndex + 1);
            BindData(ht, false);
        }

        ///保存
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string ordernums = hidOrderNums.Text;
            if (string.IsNullOrEmpty(ordernums))
            {
                MessageBoxShow("未有体检号，请关闭窗口后重新选择！"); return;
            }
            int dropDictLab = Convert.ToInt32(DropDictLab.SelectedValue);
            int dropCustomer = Convert.ToInt32(DropCustomer.SelectedValue);
            if (dropDictLab == 0 || dropCustomer == 0 || dropCustomer == -1)
            {
                MessageBoxShow("请先选择分点后再选择单位名称！"); return;
            }
            if (ordernums == string.Empty) { return; }
            Hashtable ht = new Hashtable();
            if (ck1.Checked)
            {
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()) || string.IsNullOrEmpty(txtRecName.Text.Trim()) || string.IsNullOrEmpty(txtTelphone.Text.Trim()))
                {
                    MessageBoxShow("勾选统一寄送报告后，报告寄送信息不能为空！"); return;
                }
                else
                {
                    ht.Add("ck","1");
                    ht.Add("address",txtAddress.Text.Trim());
                    ht.Add("recname", txtRecName.Text.Trim());
                    ht.Add("telphone", txtTelphone.Text.Trim());
                }
            }
            ht.Add("ordernum", ordernums);
            ht.Add("dictlabid", dropDictLab);
            ht.Add("dictcustomerid", dropCustomer);
            if (!string.IsNullOrEmpty(txtSection.Text.Trim()))
                ht.Add("section",txtSection.Text.Trim());
            
            if (mamagement.UpdateDictmemberidByOrderNum(ht))
            {
                //刷记录
                BindData((ViewState["SearchWhere"] as Hashtable), false);

                //记录操作日志
                string[] arr = ordernums.Split(',');
                foreach (string str in arr)
                {
                    mamagement.AddOperationLog(str, "", "异常管理中心", "批量修改订单" + ordernums + "[" + str + "]", "修改留痕", "批量" + ordernums);
                }
            }
            else
            {
                MessageBoxShow("修改单位名称失败，请刷新页面重试！"); 
            }
        }
        //关闭窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}