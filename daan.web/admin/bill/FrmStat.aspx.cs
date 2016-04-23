using System;
using System.Collections.Generic;
using System.Linq;
using daan.service.order;
using System.Collections;
using ExtAspNet;
using daan.web.code;
using daan.util.Web;
using System.Data;

namespace daan.web.admin.bill
{
    public partial class FrmStat : PageBase
    {
        #region 全局变量及属性
        readonly static OrdersService os = new OrdersService();
        readonly static HpvtestingService hs = new HpvtestingService();
        public int RecordCount { get; set; }
        public DataTable Dt_Source { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=250;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                BindDictLab();
                BindProvice();
                Dp_BeginDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                Dp_EndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 绑定查询列表数据
        /// </summary>
        private void BindData()
        {
            Hashtable ht = new Hashtable();
            PageUtil pageUtil = new PageUtil(GridAcconding.PageIndex, GridAcconding.PageSize);

            ht.Add("DateStart", Dp_BeginDate.Text == "" ? null : Dp_BeginDate.Text);
            ht.Add("DateEnd", Dp_EndDate.Text == "" ? null : Dp_EndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            if (dropDictLab.SelectedValue == "-1")
            {
                ht.Add("labid", Userinfo.joinLabidstr);
            }
            else
            {
                ht.Add("labid", dropDictLab.SelectedValue);
            }
            ht.Add("province",dropProvice.SelectedValue);
            ht.Add("pageStart", pageUtil.GetPageStartNum());
            ht.Add("pageEnd", pageUtil.GetPageEndNum());
            ht.Add("Section",txtSection.Text.Trim());
            if (DropCustomer.SelectedValue != "-1")
            {
                ht.Add("customername",DropCustomer.SelectedValue);
            }
            IDictionary<string, object> dic = GetDate(ddlStatus.SelectedValue,ht);
            GridAcconding.RecordCount =Convert.ToInt32(dic["RecordCount"].ToString());
            GridAcconding.DataSource = (DataTable)dic["DataSource"];
            GridAcconding.DataBind();
        }

        private IDictionary<string, object> GetDate(string t,Hashtable ht)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            switch (t)
            { 
                case "0":
                    RecordCount = hs.GetTM15ListCount(ht);
                    Dt_Source = hs.GetTM15List(ht);
                    break;
                case "1":
                    RecordCount = hs.GetHuMeiListCount(ht);
                    Dt_Source = hs.GetHuMeiList(ht);
                    break;
                case "2":
                    //太平TM15
                    ht.Add("isTP", "1");
                    RecordCount = hs.GetTM15ListCount(ht);
                    Dt_Source = hs.GetTM15List(ht);
                    break;
                case "3":

                    break;
                case "4":
                    RecordCount = os.GetHPVTMAccondingInfosCount(ht);
                    Dt_Source = os.GetHPVTMAccondingInfos(ht);
                    break;
                default:
                    RecordCount = 0;
                    Dt_Source = null;
                    break;
            }
            dic.Add("RecordCount", RecordCount);
            dic.Add("DataSource", Dt_Source);
            return dic;
        }

        // 绑定分点        
        private void BindDictLab()
        {
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ListItem("全部", "-1"));
            if (dropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(dropDictLab.SelectedValue));
            }
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(dropDictLab.SelectedValue));
        }
        // 绑定单位        
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }

        private void BindProvice()
        {
            DropProvinceBinder(dropProvice);

        }
        #region 工具栏按钮
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Dp_BeginDate.Text != "" && Dp_EndDate.Text != "")
            {
                if (Dp_BeginDate.SelectedDate <= Dp_EndDate.SelectedDate)
                {
                    BindData();
                }
                else
                {
                    MessageBoxShow("结束时间应大于开始时间！", MessageBoxIcon.Information);
                }
            }
            else
            {
                if (Dp_BeginDate.Text == "" || Dp_EndDate.Text == "")
                {
                    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                }
                else
                {
                    BindData();
                }
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridAcconding_PageIndexChange(object sender, GridPageEventArgs e)
        {
            GridAcconding.PageIndex = e.NewPageIndex;
            BindData();
        }
        #endregion

        #region >>>导出到Excel
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dp_BeginDate.Text == "" || Dp_EndDate.Text == "")
                {
                    MessageBoxShow("起止时间不能为空！", MessageBoxIcon.Information);
                    return;
                }
                Hashtable ht = new Hashtable();
                if (dropDictLab.SelectedValue == "-1")
                {
                    ht.Add("labid", Userinfo.joinLabidstr);
                }
                else
                {
                    ht.Add("labid", dropDictLab.SelectedValue);
                }
                if (DropCustomer.SelectedValue != "-1")
                {
                    ht.Add("customername", DropCustomer.SelectedValue);
                }
                ht.Add("province", dropProvice.SelectedValue);
                ht.Add("DateStart", Dp_BeginDate.Text);
                ht.Add("Section", txtSection.Text.Trim());
                ht.Add("DateEnd", Convert.ToDateTime(Dp_EndDate.Text).AddDays(1).ToString("yyyy-MM-dd"));
                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                if (ddlStatus.SelectedValue == "1")//护美类产品返回公司体检量统计
                {
                    using (DataTable hpvinstrumentsList = new HpvtestingService().GetListHpvinstrumentsByWhereTime(ht))
                    {
                        if (hpvinstrumentsList.Rows.Count > 0)
                        {
                            ExcelOperation<DataTable>.ExportDataTableToExcel(hpvinstrumentsList, filename, sheetname);
                        }
                        else
                        {
                            MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
                        }
                    }
                }
                else if (ddlStatus.SelectedValue == "0")//全部检测数据导出
                {
                    using (DataTable TM15List = new HpvtestingService().GetListTM15ByWhereTime(ht))
                    {
                        if (TM15List.Rows.Count > 0)
                        {
                            ExcelOperation<DataTable>.ExportDataTableToExcel(TM15List, filename, sheetname);
                        }
                        else
                        {
                            MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
                        }
                    }
                }
                else if (ddlStatus.SelectedValue == "2")//查询TM15检测数据
                {
                    ht.Add("isTP", "1");
                    using (DataTable TM15List = new HpvtestingService().GetListTM15ByWhereTime(ht))
                    {
                        if (TM15List.Rows.Count > 0)
                        {
                            ExcelOperation<DataTable>.ExportDataTableToExcel(TM15List, filename, sheetname);
                        }
                        else
                        {
                            MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
                        }
                    }
                }
                else if (ddlStatus.SelectedValue == "3")//分点查询所有检测项目，并组合项目及价格求和
                {
                    using (DataTable TestNameList = new HpvtestingService().GetListTestNameWhereTime(ht))
                    {
                        if (TestNameList.Rows.Count > 0)
                        {
                            ExcelOperation<DataTable>.ExportDataTableToExcel(TestNameList, filename, sheetname);
                        }
                        else
                        {
                            MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
                        }
                    }
                }
                else if (ddlStatus.SelectedValue == "4")//HPV+TM检查统计导出
                {
                    using (DataTable TM15HPVList = new HpvtestingService().SelectHPVTMAccondingInfos(ht))
                    {
                        if (TM15HPVList.Rows.Count > 0)
                        {
                            ExcelOperation<DataTable>.ExportDataTableToExcel(TM15HPVList, filename, sheetname);
                        }
                        else
                        {
                            MessageBoxShow("没有需要导出的数据！", MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion
    }

}