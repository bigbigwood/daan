using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ExtAspNet;
using daan.util.Web;
using daan.service.proceed;
using daan.service.order;
using System.Collections;
using System.Data;
using System.Text;
using daan.domain;
using daan.service.dict;
using daan.web.code;
using System.Text.RegularExpressions;
using daan.service.login;

namespace daan.web.admin.proceed
{
    public partial class ProUploadPdfToSheQu : PageBase
    {
        DictlabService dictlabService = new DictlabService();
        OrdersService orderService = new OrdersService();
        LoginService loginservice = new LoginService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + dropDictcustomer.ClientID + "')).listWidth=250;");  
                InitData();
            }
        }
        #region 方法
        /// <summary>
        /// 初始化分点
        /// </summary>
        void BindDrop()
        {
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //体检单位初始化
            BinddropDictcustomer(dropDictLab.SelectedValue);
        }

        // 初始化体检单位
        void BinddropDictcustomer(string dictlabid)
        {
            DropDictcustomerBinder(dropDictcustomer, dictlabid, true);
        }
        /// <summary>
        /// 开始加载数据，默认加载当天没有上传成功的数据
        /// </summary>
        void InitData()
        {
            BindDrop();
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;

            BindGrid();
        }
        /// <summary>
        /// 根据数据和页数绑定Grid
        /// </summary>
        /// <param name="count"></param>
        /// <param name="?"></param>
        void BindGrid()
        {
            Hashtable ht = GetParm();

            DataTable dt = orderService.SelectUploadToSheQu(ht);
            int count = orderService.SelectUploadToSheQuCount(ht);

            DataColumn column = new DataColumn("FaileTRANSED", typeof(string));
            dt.Columns.Add(column);
            foreach (DataRow item in dt.Rows)//获取表中的行状态，确认是否已上传
            {
                if (item["TRANSED"].ToString() == "0"||item["TRANSED"].ToString() == "")//状态为0，表示未上传
                {
                    item["FaileTRANSED"] = "未上传";
                }
                else if (item["TRANSED"].ToString() == "1")//状态为1，表示已上传
                {
                    item["FaileTRANSED"] = "已上传";
                }
                else if (item["TRANSED"].ToString() == "2")
                {
                    item["FaileTRANSED"] = "上传失败";
                }
                else
                {
 
                }
            }
            gdUploadPdfToSheQu.RecordCount = count;
            gdUploadPdfToSheQu.DataSource = dt;
            gdUploadPdfToSheQu.DataBind();
        }
        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        private Hashtable GetParm()
        {
            Hashtable _parameterCache = new Hashtable();
            #region 分点
            if (dropDictLab.SelectedValue!="-1")
            {
                _parameterCache.Add("dropDictLab", dropDictLab.SelectedValue);
            }
            else
            {
                _parameterCache.Add("dropDictLab", null);
            }
            #endregion
            _parameterCache.Add("DropTransed", dropTransed.SelectedValue == "-1" ? null : dropTransed.SelectedValue == "0" ? "0 or TranSed is null" : dropTransed.SelectedValue);
            _parameterCache.Add("StartDate", ((DateTime)dpFrom.SelectedDate).ToString("yyyy-MM-dd"));
            _parameterCache.Add("EndDate", ((DateTime)dpTo.SelectedDate).AddDays(1).ToString("yyyy-MM-dd"));
            _parameterCache.Add("Dictcustomerid", dropDictcustomer.SelectedValue == "-1" ? null : dropDictcustomer.SelectedValue);

            if (tbStrKey.Text.Trim().Length > 0)
            {
                _parameterCache.Add("tbStrKey", TextUtility.ReplaceText(tbStrKey.Text));
            }
            else
            {
                _parameterCache.Add("tbStrKey", null);
            }
            PageUtil pageutil = new PageUtil(gdUploadPdfToSheQu.PageIndex, gdUploadPdfToSheQu.PageSize);
            _parameterCache.Add("PageStart", pageutil.GetPageStartNum());
            _parameterCache.Add("PageEnd", pageutil.GetPageEndNum());
            return _parameterCache;
        }
        #endregion

        #region 事件

        /// <summary>
        /// 体检单位改变响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinddropDictcustomer(dropDictLab.SelectedValue);
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Regex reg = new Regex(@"^\d+$");
            //if(ddlRowCount.SelectedItem==null && !reg.IsMatch(ddlRowCount.Text.ToString()))
            //{
            //    MessageBoxShow("请输入正确的显示行数！");
            //    return;
            //}
            if (dropDictLab.SelectedText == "分点选择")
            {
                MessageBoxShow("必须选择一个分点！");
                return;
            }
            if (this.dpFrom.Text != "" && this.dpTo.Text != "")
            {
                if (this.dpFrom.SelectedDate <= this.dpTo.SelectedDate)
                {
                    
                    BindGrid();
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
        /// 确认上传事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gdUploadPdfToSheQu.SelectedRowIndexArray.Length == 0)
            {
                return;
            }           
            StringBuilder sb = new StringBuilder();
            foreach (int row in gdUploadPdfToSheQu.SelectedRowIndexArray)
            {
                //判断选择的数据是否为上传失败的记录
                if (gdUploadPdfToSheQu.Rows[row].Values[3].ToString() != "上传失败")
                {
                    MessageBoxShow("选择的数据中存在非上传失败的记录,确定要上传吗？", MessageBoxIcon.Question);
                }
                sb.Append(gdUploadPdfToSheQu.DataKeys[row][0].ToString());
                sb.Append(",");
            }
            string orderNums=sb.ToString();
            Hashtable ht = new Hashtable();
            ht.Add("Transed", "0");
            ht.Add("OrderNum", orderNums.TrimEnd(','));
            bool affectRow = orderService.EditSelectTransed(ht);
            if (affectRow)
            {
                //添加操作日志
                foreach (int row in gdUploadPdfToSheQu.SelectedRowIndexArray)
                {
                    orderService.AddOperationLog(gdUploadPdfToSheQu.DataKeys[row][0].ToString(),
                        "", "重新上传订单", "修改上传社区失败的记录状态为0,提供重新扫描上传。", "修改留痕", " ");
                }
                gdUploadPdfToSheQu.SelectedRowIndexArray = null;
                BindGrid();
                MessageBoxShow("修改成功");

            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdUploadPdfToSheQu_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdUploadPdfToSheQu.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion
    }
}