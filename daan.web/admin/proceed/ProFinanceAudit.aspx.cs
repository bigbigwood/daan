using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using daan.service.order;
using daan.web.code;
using daan.util.Web;
using ExtAspNet;
using daan.service.common;
using System.Data;

namespace daan.web.admin.proceed
{
    public partial class ProFinanceAudit : PageBase
    {
        OrdersService ordersService = new OrdersService();
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + dropDictcustomer.ClientID + "')).listWidth=250;");
            if (Request.Form["__EVENTTARGET"] == tbxmember.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey") { SelectCustomer(); }
            if (!IsPostBack)
            {
                InitPageData();
            }
        }

        #region 体检单位模糊查询
        private void SelectCustomer()
        {
            string cusName = tbxmember.Text.Trim();
            if (string.IsNullOrEmpty(cusName))
            {
                //体检单位初始化
                BinddropDictcustomer(dropDictLab.SelectedValue);
            }
            else
            {
                string labid = string.Empty;
                if (dropDictLab.SelectedValue == "-1")
                {
                    labid = Userinfo.joinLabidstr;
                }
                else
                {
                    labid = dropDictLab.SelectedValue;
                }
                Hashtable htPara = new Hashtable();
                htPara.Add("labid", labid);
                htPara.Add("customername", cusName);

                DataTable dtList = new daan.service.dict.DictCustomerService().GetCustomerListBySearchBox(htPara);
                if (dtList == null || dtList.Rows.Count == 0)
                {
                    MessageBoxShow("没有搜索到匹配的体检单位！");
                    tbxmember.Text = string.Empty;
                    tbxmember.Focus();
                    return;
                }
                else if (dtList.Rows.Count == 1)
                {
                    dropDictcustomer.SelectedValue = dtList.Rows[0]["dictcustomerid"].ToString();
                    tbxmember.Text = string.Empty;
                }
                else
                {
                    dropDictcustomer.DataSource = dtList;
                    dropDictcustomer.DataValueField = "Dictcustomerid";
                    dropDictcustomer.DataTextField = "Customername";
                    dropDictcustomer.DataBind();
                    tbxmember.Text = string.Empty;
                }
            }
        }
        #endregion

        #region 初始化数据
        //初始化页面的数据
        void InitPageData()
        {
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;
            dpSFrom.Text = "";
            dpSTo.Text = "";
            dpFFrom.Text = "";
            dpFTo.Text = "";
            BindDrop();
        }

        // 初始化下拉列表的数据
        void BindDrop()
        {
            //分点
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //体检单位初始化
            BinddropDictcustomer(dropDictLab.SelectedValue);
            //订单状态
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //报告状态
            DDLInitbasicBinder(dropReportStatus, "REPORTSTATUS");
            dropReportStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            dropReportStatus.SelectedValue = "0";
            //省份
            DropProvinceBinder(dpProvince);
        }

        // 初始化体检单位
        void BinddropDictcustomer(string dictlabid)
        {
            DropDictcustomerBinder(dropDictcustomer, dictlabid, true);
        }

        /// <summary>
        /// 分点改变响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinddropDictcustomer(dropDictLab.SelectedValue);
        }
        #endregion

        #region 页面事件
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (this.dpFrom.Text != "" && this.dpTo.Text != "")
            //{
            //    if (this.dpFrom.SelectedDate <= this.dpTo.SelectedDate)
            //    {
            //        BindgdOrders();
            //    }
            //    else
            //    {
            //        MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
            //    }
            //}
            //else
            //{
            //    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
            //}

            if ((dpFrom.Text == "" || dpTo.Text == "") && (dpSFrom.Text == "" || dpSTo.Text == "") && (dpFFrom.Text == "" || dpFTo.Text == ""))
            {
                MessageBoxShow("登记时间、采样日期、财务审核日期必须要有一个作为查询条件！", MessageBoxIcon.Information);
            }
            else
            {
                BindgdOrders();
            }
        }

        //分页
        protected void gdOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gdOrders.PageIndex = e.NewPageIndex;
            BindgdOrders();
        }

        /// <summary>
        /// 选择每页数据量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pagesize = Convert.ToInt32(dropPageSize.SelectedValue.ToString());
            gdOrders.PageSize = pagesize;
            gdOrders.PageIndex = 0;
            BindgdOrders();
        }

        //审核
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectOrderNums(false);
            if (ordernums == string.Empty) { return; }
            Hashtable ht = new Hashtable();
            ht.Add("ordernums", ordernums);
            ht.Add("AuditStatus", (int)ParamStatus.FinanceAuditStatus.Audit);
            ht.Add("AuditUserName", Userinfo.userName);
            if (ordersService.FinanceAudit(ht))
            {
                string ordernums2 = ordernums.Replace("'", "");
                string[] arr = ordernums2.Split(new char[] { ',' });
                foreach (string ordernum in arr)
                {
                    ordersService.AddOperationLog(ordernum, "", "报告打印审核", "对报告【" + ordernum + "】财务审核成功", "修改留痕",
                        ordernums2.Length > 2000 ? ordernums2.Substring(0, 1998) + "等" : ordernums2);
                }
                MessageBoxShow("审核成功" + arr.Length.ToString() + "条记录！", MessageBoxIcon.Information);
                BindgdOrders();
            }
        }

        //取消审核
        //只能取消审核自己审核的报告
        protected void btnUnAudit_Click(object sender, EventArgs e)
        {
            string ordernums = GetSelectOrderNums(true);
            if (ordernums == string.Empty) { return; }
            Hashtable ht = new Hashtable();
            ht.Add("ordernums", ordernums);
            ht.Add("AuditStatus", (int)ParamStatus.FinanceAuditStatus.UnAudit);
            ht.Add("AuditUserName", Userinfo.userName);
            if (ordersService.FinanceAudit(ht))
            {
                string ordernums2 = ordernums.Replace("'", "");
                string[] arr = ordernums2.Split(new char[] { ',' });
                foreach (string ordernum in arr)
                {
                    ordersService.AddOperationLog(ordernum, "", "报告打印审核", "对报告【" + ordernum + "】财务取消审核成功", "修改留痕",
                        ordernums2.Length > 2000 ? ordernums2.Substring(0, 1998) + "等" : ordernums2);
                }   
                MessageBoxShow("取消审核成功" + arr.Length.ToString() + "条记录！", MessageBoxIcon.Information);
                BindgdOrders();
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Hashtable ht = GetParm();
            DataTable dt = ordersService.DataForFinanceAuditExport(ht);
            if (dt.Rows.Count > 0)
            {
                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                ExcelOperation<DataTable>.ExportDataTableToExcel(dt, filename, sheetname);
            }
            else
            {
                MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 页面方法
        // 绑定订单主表的数据
        void BindgdOrders()
        {
            Hashtable _parameterCache = GetParm();
            gdOrders.RecordCount = ordersService.DataForFocusPrintPageTotal(_parameterCache);
            gdOrders.DataSource = ordersService.DataForFocusPrintPageLst(_parameterCache);
            gdOrders.DataBind();
        }

        // 取得查询参数
        private Hashtable GetParm()
        {
            string ordernum = tbxOrderNum.Text = TextUtility.ReplaceText(tbxOrderNum.Text.Trim());
            PageUtil pageUtil = new PageUtil(gdOrders.PageIndex, gdOrders.PageSize);
            Hashtable _parameterCache = new Hashtable();
            _parameterCache.Add("pageStart", pageUtil.GetPageStartNum());
            _parameterCache.Add("pageEnd", pageUtil.GetPageEndNum());
            _parameterCache.Add("ordernum", ordernum);
            //有体检号时 忽略其他条件
            if (ordernum != string.Empty) { return _parameterCache; }
            //分点
            switch (dropDictLab.SelectedText)
            {
                case "全部":
                    {
                        if (dropDictLab.SelectedValue == "-1")
                        {
                            _parameterCache.Add("dictlabid", Userinfo.joinLabidstr);
                        }
                    };
                    break;
                default:
                    {
                        _parameterCache.Add("dictlabid", dropDictLab.SelectedValue);
                    };
                    break;
            }
            //体检单位
            _parameterCache.Add("dictcustomerid", dropDictcustomer.SelectedValue == "-1" ? null : dropDictcustomer.SelectedValue);
            //登记日期开始日期
            if (dpFrom.Text != "")
            {
                _parameterCache.Add("StartDate", dpFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            //登记日期结束日期
            if (dpTo.Text != "")
            {
                _parameterCache.Add("EndDate", dpTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }
            //采样日期开始日期
            if (dpSFrom.Text != "")
            {
                _parameterCache.Add("SDateBegin", dpSFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            //采样日期结束日期
            if (dpSTo.Text != "")
            {
                _parameterCache.Add("SDateEnd", dpSTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }
            //标本状态
            _parameterCache.Add("status", dropStatus.SelectedValue == "-1" ? null : dropStatus.SelectedValue);
            //关键字（姓名,套餐,营业区,收件人,场次号）
            _parameterCache.Add("name", TextUtility.ReplaceText(tbxName.Text.Trim()));
            //报告状态
            _parameterCache.Add("reportstatus", this.dropReportStatus.SelectedValue == "-1" ? null : this.dropReportStatus.SelectedValue);
            //机构部门
            _parameterCache.Add("section", TextUtility.ReplaceText(tbxSection.Text.Trim()));
            //省份
            _parameterCache.Add("province", dpProvince.SelectedValue == "-1" ? null : dpProvince.SelectedText);
            //财务审核状态
            _parameterCache.Add("auditstatus", dropAuditStatus.SelectedValue == "-1" ? null : dropAuditStatus.SelectedValue);
            //财务审核人
            _parameterCache.Add("auditusername",TextUtility.ReplaceText(tbxAuditUserName.Text.Trim()));
            //财务审核日期开始日期
            if (dpFFrom.Text != "")
            {
                _parameterCache.Add("FDateBegin", dpFFrom.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            //财务审核日期结束日期
            if (dpFTo.Text != "")
            {
                _parameterCache.Add("FDateEnd", dpFTo.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }
            return _parameterCache;
        }

        //是否检查已打印
        private string GetSelectOrderNums(bool isUnAudit)
        {
            int[] strSelect = gdOrders.SelectedRowIndexArray;
            if (strSelect.Length <= 0)
            {
                MessageBoxShow("您还没有勾选记录!");
                return "";
            }
            string str = string.Empty;
            for (int i = 0; i < strSelect.Length; i++)
            {
                str += "'"+gdOrders.DataKeys[strSelect[i]][0].ToString() + "',";
                
                //判断是否已打印报告
                if (CheckIsFinishPring(gdOrders.DataKeys[strSelect[i]][1]))
                {
                    MessageBoxShow("选中订单中有部分已经打印报告，无法进行审核和取消审核操作！"); return "";
                }
                //取消审核只能取消本人审核的报告
                if (isUnAudit)
                {
                    if (gdOrders.DataKeys[strSelect[i]][2] != null && Userinfo.userName != gdOrders.DataKeys[strSelect[i]][2].ToString())
                    {
                        MessageBoxShow("选中订单中有报告不是您审核的，无法进行取消审核操作！"); return "";
                    }
                    if (Convert.ToInt32(gdOrders.DataKeys[strSelect[i]][3].ToString()) == (int)ParamStatus.FinanceAuditStatus.UnAudit)
                    {
                        MessageBoxShow("选中订单中有报告有未审核记录，无法进行取消审核操作！"); return "";
                    }
                }
                else
                {
                    if (Convert.ToInt32(gdOrders.DataKeys[strSelect[i]][3].ToString()) == (int)daan.service.common.ParamStatus.FinanceAuditStatus.Audit)
                    {
                        MessageBoxShow("选中订单中有报告有已审核记录，无法进行重复审核操作！"); return "";
                    }
                }
            }
            return str.TrimEnd(',');
        }

        private bool CheckIsFinishPring(object str)
        {
            return Convert.ToInt32(str) == (int)daan.service.common.ParamStatus.OrdersStatus.FinishPrint;
        }
        #endregion
    }
}