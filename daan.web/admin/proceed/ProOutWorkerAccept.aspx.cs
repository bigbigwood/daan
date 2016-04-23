using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using ExtAspNet;
using daan.util.Web;
using daan.service.proceed;
using System.Collections;
using System.Data;
using System.Text;
using daan.domain;
using daan.service.dict;
using daan.web.code;

namespace daan.web.admin.proceed
{
    public partial class ProOutWorkerAccept :PageBase
    {
        #region 字段
        OrderbarcodeService orderbarcodeService = new OrderbarcodeService();
        DictlabService dictlabService = new DictlabService();
         #endregion
        #region 页面业务方法
        /// <summary>
        /// 初始化分点
        /// </summary>
        void BindDrop()
        {

            
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }
        /// <summary>
        /// 开始加载数据，默认加载当天没有已确认接收的数据
        /// </summary>
        void InitData()
        {
            BindDrop();
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;
            
           // BindGrid();
        }

        /// <summary>
        /// 根据数据和页数绑定Grid,1表示确认状态
        /// </summary>
        /// <param name="count"></param>
        /// <param name="?"></param>
        void BindGrid()
        {
            Hashtable ht = GetParm();
            DataTable dt = orderbarcodeService.SpecimenAccept(ht);
            DataColumn column = new DataColumn("EnSureSTATUS", typeof(string));
            dt.Columns.Add(column);
            foreach (DataRow item in dt.Rows)//获取表中的行状态，确认是否已接收
            {
                if (item["STATUS"].ToString() == "15")//状态为15，表示确认已接收
                {
                    item["EnSureSTATUS"] = "已接收";
                }
                else
                {
                    item["EnSureSTATUS"] = "未接收";
                }
            }
            gdOutWorkerAccept.DataSource = dt;
            gdOutWorkerAccept.DataBind();
        }
        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        Hashtable GetParm()
        {
            Hashtable _parameterCache = new Hashtable();
            #region 分点
            if (dropDictLab.SelectedValue != "-1")
            {
                _parameterCache.Add("dropDictLab", dropDictLab.SelectedValue);
            }
            else
            {
                _parameterCache.Add("dropDictLab", Userinfo.joinLabidstr);
            }
            #endregion     
            _parameterCache.Add("DropSure", dropStatus.SelectedValue == "-1" ? null : dropStatus.SelectedValue);
            _parameterCache.Add("StartDate", ((DateTime)dpFrom.SelectedDate).ToString("yyyy-MM-dd"));
            _parameterCache.Add("EndDate", ((DateTime)dpTo.SelectedDate).AddDays(1).ToString("yyyy-MM-dd"));
            if (tbStrKey.Text.Trim().Length > 0)
            {
                _parameterCache.Add("tbStrKey",TextUtility.ReplaceText(tbStrKey.Text));
            }
            else
            {
                _parameterCache.Add("tbStrKey", null);
            }
            return _parameterCache;
        }
        #endregion


        #region 页面事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
               
            }
           
        }
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (dropDictLab.SelectedText == "分点选择")
            {
                MessageBoxShow("必须选择一个分点");
                return;
            }
            if (this.dpFrom.Text != "" && this.dpTo.Text != "")
            {
                if (this.dpFrom.SelectedDate <= this.dpTo.SelectedDate)
                {
                    BindGrid();
                    tbEnsureBarcode.Text = "";
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
        /// 确认标本已接受事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {          
            if (gdOutWorkerAccept.SelectedRowIndexArray.Length==0)
            {
                return;
            }
            if (dropStatus.SelectedValue == "-1")
            {
                MessageBoxShow("请选择[未接收]状态再扫描确认",MessageBoxIcon.Information);
                return;
            }
            foreach (int row in gdOutWorkerAccept.SelectedRowIndexArray)
            {

                Hashtable ht = new Hashtable();
                ht.Add("OrdersBarcodeIds", gdOutWorkerAccept.DataKeys[row][0].ToString());
                UserInfo userInfo = (UserInfo)Session["UserInfo"];
                ht.Add("UserName", userInfo.userId);
                orderbarcodeService.EnsureAccept(ht);

                orderbarcodeService.AddOperationLog(gdOutWorkerAccept.DataKeys[row][2].ToString(),
                            gdOutWorkerAccept.DataKeys[row][1].ToString(), "外勤标本接收", "确认标本已接收", "修改留痕", " ");
            }
            gdOutWorkerAccept.SelectedRowIndexArray = null;
            BindGrid();
            MessageBoxShow("确认成功");
        }

        /// <summary>
        /// 根据扫描进来的条码号，勾选相对应的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbEnsureBarcode_TriggerClick(object sender, EventArgs e)
        {
            //是否存在改条码
            bool ischeck = false;

            //清空旧条码
            int j = tbEnsureBarcode.Text.IndexOf((char)2);
            if (j >= 0)
            {
                tbEnsureBarcode.Text = tbEnsureBarcode.Text.Substring(j + 1) + (char)2;
            }
            else
            {
                tbEnsureBarcode.Text = tbEnsureBarcode.Text + (char)2;
            }
            List<int> selectedRowIndexArray = gdOutWorkerAccept.SelectedRowIndexArray.ToList();
            for (int i = 0; i < gdOutWorkerAccept.Rows.Count; i++)
            {
                object[] dataKeys = gdOutWorkerAccept.DataKeys[i];
                if (tbEnsureBarcode.Text.Replace(((char)2).ToString(), "") == dataKeys[1].ToString())
                {
                    selectedRowIndexArray.Add(i);
                    ischeck = true;
                }                
            }
            if (selectedRowIndexArray.Count > 0)
            {
                gdOutWorkerAccept.SelectedRowIndexArray = selectedRowIndexArray.ToArray();
            }
            if (!ischeck)
            {
                MessageBoxShow("没有找到该条码号");
            }
            this.tbEnsureBarcode.Text = string.Empty; 
        }       
        #endregion

        

      
       
     
    }
}