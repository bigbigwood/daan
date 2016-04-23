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

namespace daan.web.admin.proceed
{
    public partial class ProUploadToLIS : PageBase
    {
        OrderbarcodeService orderbarcodeService = new OrderbarcodeService();
        DictlabService dictlabService = new DictlabService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        }
        /// <summary>
        /// 开始加载数据，默认加载当天没有上传成功的数据
        /// </summary>
        void InitData()
        {
            BindDrop();
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
            DataTable dt = orderbarcodeService.SelectUploadToLIS(ht);
            DataColumn column = new DataColumn("FaileTRANSED", typeof(string));
            dt.Columns.Add(column);
            foreach (DataRow item in dt.Rows)//获取表中的行状态，确认是否已上传
            {
                if (item["TRANSED"].ToString() == "0")//状态为0，表示未上传
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
            gdUploadToLIS.DataSource = dt;
            gdUploadToLIS.DataBind();
        }
        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        Hashtable GetParm()
        {
            Hashtable _parameterCache = new Hashtable();
            #region 分点
            if (dropDictLab.SelectedValue!=null)
            {
                _parameterCache.Add("dropDictLab", dropDictLab.SelectedValue);
            }
            else
            {
                _parameterCache.Add("dropDictLab", null);
            }
            #endregion
            _parameterCache.Add("DropTransed", dropTransed.SelectedValue == "-1" ? null : dropTransed.SelectedValue);

            if (tbStrKey.Text.Trim().Length > 0)
            {
                _parameterCache.Add("tbStrKey", TextUtility.ReplaceText(tbStrKey.Text));
            }
            else
            {
                _parameterCache.Add("tbStrKey", null);
            }
            _parameterCache.Add("RowNum", ddlRowCount.SelectedItem == null ? ddlRowCount.Text.ToString() : ddlRowCount.SelectedValue.ToString());
            return _parameterCache;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"^\d+$");
            if (ddlRowCount.SelectedItem == null && !reg.IsMatch(ddlRowCount.Text.ToString()))
            {
                MessageBoxShow("请输入正确的显示行数！");
                return;
            }

            if (dropDictLab.SelectedText == "分点选择")
            {
                MessageBoxShow("必须选择一个分点！");
                return;
            }
            BindGrid();
               
        }
        /// <summary>
        /// 确认上传事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gdUploadToLIS.SelectedRowIndexArray.Length == 0)
            {
                return;
            }            
            StringBuilder sb = new StringBuilder();
            foreach (int row in gdUploadToLIS.SelectedRowIndexArray)
            {
                sb.Append(gdUploadToLIS.DataKeys[row][0].ToString());
                sb.Append(",");
            }
            string orderNums = sb.ToString();
            Hashtable ht = new Hashtable();
            ht.Add("Transed", "0");
            ht.Add("OrderNum", orderNums.TrimEnd(','));
            bool affectRow = orderbarcodeService.UpdateSelectedTransedToLIS(ht);
            if (affectRow)
            {
                //添加操作日志
                foreach (int row in gdUploadToLIS.SelectedRowIndexArray)
                {
                    orderbarcodeService.AddOperationLog(gdUploadToLIS.DataKeys[row][0].ToString(),
                        "", "重新上传到LIS系统", "修改上传LIS系统失败的记录状态为0,提供重新扫描上传。", "修改留痕", " ");
                }
                gdUploadToLIS.SelectedRowIndexArray = null;
                BindGrid();
                MessageBoxShow("修改成功");
            }
        }
        #endregion
    }
}