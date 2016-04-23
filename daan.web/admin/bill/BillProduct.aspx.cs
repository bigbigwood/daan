using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using daan.service.bill;
using daan.domain;
using daan.service.order;
using daan.util.Common;
using daan.web.code;
namespace daan.web.admin.bill
{
    public partial class BillProduct :PageBase
    {

        BillProductService service = new BillProductService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initial();
                BindDicttestitemid();
                dtpStart.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                dtpEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }
        #region 页面事件
        //分点联动单位
        protected void dropLab_SelectedIndexChanged(object sender, EventArgs e)
        {           
            DropDictcustomerBinder(dropCustomer, dropLab.SelectedValue,true);

        }
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            Hashtable htPara = new Hashtable();
            htPara.Add("state",drpState.SelectedValue);
            htPara.Add("customerid", dropCustomer.SelectedValue);      
            htPara.Add("dictTestItemID", dropDicttestitem.SelectedValue);      
            htPara.Add("startDate", dtpStart.Text.ToString() == "" ? null : dtpStart.Text.ToString());
            htPara.Add("endDate", dtpEnd.Text.ToString() == "" ? null : dtpEnd.Text.ToString());
            htPara.Add("pageStart", pageUtil.GetPageStartNum());
            htPara.Add("pageEnd", pageUtil.GetPageEndNum());

            try
            {
                gvList.RecordCount = service.SelectProductPageTotal(htPara);
                gvList.DataSource = service.SelectProductPageLst(htPara);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }

        }
        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            btnSearch_Click(null,null);
        }
        //导出Excel
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gvList.Rows.Count == 0)
            //    {
            //        MessageBoxShow("导出没有数据！");
            //        return;
            //    }

            //    PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
            //    Hashtable htPara = new Hashtable();
            //    htPara.Add("state", drpState.SelectedValue);
            //    htPara.Add("customerid", dropCustomer.SelectedValue);
            //    htPara.Add("dictTestItemID", dropDicttestitem.SelectedValue);
            //    htPara.Add("startDate", dtpStart.Text.ToString() == "" ? null : dtpStart.Text.ToString());
            //    htPara.Add("endDate", dtpEnd.Text.ToString() == "" ? null : dtpEnd.Text.ToString());
            //    htPara.Add("pageStart", pageUtil.GetPageStartNum());
            //    htPara.Add("pageEnd", pageUtil.GetPageEndNum());

            //    SortedList sortlist = new SortedList(new MySort());
            //    sortlist.Add("INSTRUMENTSBARCODE", "耗材条码");
            //    sortlist.Add("customername", "标本条码");
            //    sortlist.Add("testname", "体检项目");
            //    sortlist.Add("STANDARDPRICE", "标准价");
            //    sortlist.Add("GROUPPRICE", "分点价");
            //    sortlist.Add("CONTRACTPRICE", "应收价钱");
            //    sortlist.Add("FINALPRICE", "成交价");
            //    sortlist.Add("createdate", "录入时间");
            //    ExcelOperation<Dicttestitem>.ExportListToExcel(service.SelectProductPageTotal(htPara), sortlist, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
            //}
            //catch (Exception)
            //{
            //    MessageBoxShow("导出出错，请联系管理员！");
            //}
        }
        #endregion

        #region 页面方法
        /// <summary>初始化
        /// 
        /// </summary>
        protected void Initial()
        {
            //分点
            DDLDictLabBinder(dropLab, true);
            dropLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));

            //绑定用户默认分点单位
            DropDictcustomerBinder(dropCustomer,Userinfo.dictlabid.ToString(),true);


        }
        /// <summary>绑定套餐选择
        /// 
        /// </summary>
        private void BindDicttestitemid()
        {
            HpvtestingService hpvService = new HpvtestingService();
            IList<Dicttestitem> list = hpvService.GetDicttestitemWithIsProject();
            if (list == null)
                return;
            foreach (Dicttestitem item in list)
            {
                dropDicttestitem.Items.Add(item.Testname, item.Dicttestitemid == null ? "-1" : item.Dicttestitemid.Value.ToString());
            }
            dropDicttestitem.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }
       
        #endregion
    }
}