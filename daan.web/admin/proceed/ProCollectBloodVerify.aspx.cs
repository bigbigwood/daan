using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.proceed;
using System.Collections;
using System.Data;
using ExtAspNet;
using daan.util.Web;
using System.Text;
using daan.service.dict;
using daan.domain;
using daan.web.code;
using daan.service.login;

namespace daan.web.admin.proceed
{
    public partial class CollectBloodVerify : PageBase
    {
        #region 字段
        OrderbarcodeService orderbarcodeService = new OrderbarcodeService();
        DictCustomerService dictCustomerService = new DictCustomerService();
        DictlabService dictlabService = new DictlabService();
        LoginService loginservice = new LoginService();

        /// <summary>
        /// 临时存放的集合
        /// </summary>
        public List<Orderbarcode> orderdatatable
        {
            get
            {
                if (ViewState["orderdatatable"] == null) ViewState["orderdatatable"] = new List<Orderbarcode>();
                return ViewState["orderdatatable"] as List<Orderbarcode>;
            }
            set { ViewState["orderdatatable"] = value; }
        }
        #endregion

        #region 页面业务方法
        /// <summary>
        /// 初始化分点
        /// </summary>
        void BindDrop()
        {
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            //dropDictLab.Items.Add(new ExtAspNet.ListItem() { Text = "全部", Value = "-1", Selected = true });
        }

        //选择分点绑定体检单位
        protected void dropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer();
        }

        //绑定体检单位下拉框
        private void BindCustomer()
        {
            int labid = int.Parse(dropDictLab.SelectedValue);
            DropDictcustomerBinder(dropCustomer, labid.ToString(), true);
        }
        /// <summary>
        /// 开始加载数据，默认加载当天没有采血确认的数据
        /// </summary>
        void InitData()
        {
            BindDrop();
            BindCustomer();
            dpFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dpTo.SelectedDate = DateTime.Now;
        }        

        /// <summary>
        /// 根据数据和页数绑定Grid,1表示确认状态
        /// </summary>
        /// <param name="count"></param>
        /// <param name="?"></param>
        void BindGrid()
        {
            Hashtable ht = GetParm();
            DataTable dt = orderbarcodeService.DataForCollectBlood(ht);
            DataColumn column = new DataColumn("EnSureSTATUS", typeof(string));
            dt.Columns.Add(column);
            foreach (DataRow item in dt.Rows)//获取表中的行状态，确认是否已采血
            {
                if (item["STATUS"].ToString() == "10")//状态为10，表示确认已采血
                {
                    item["EnSureSTATUS"] = "已采血";
                }
                else
                {
                    item["EnSureSTATUS"] = "未采血";
                }
            }
            gdCollectBlood.DataSource = dt;
            gdCollectBlood.DataBind();
        }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        Hashtable GetParm()
        {
            Hashtable _parameterCache = new Hashtable();
            if (dropDictLab.SelectedValue == "-1")
            {
                _parameterCache.Add("dropDictLab", Userinfo.joinLabidstr);
            }
            else
            {
                _parameterCache.Add("dropDictLab", dropDictLab.SelectedValue);
            }
            if (dropCustomer.SelectedValue != "-1")
            {
                _parameterCache.Add("dropCustomer", dropCustomer.SelectedValue);
            }
            _parameterCache.Add("DropSure", dropStatus.SelectedValue == "-1" ? null : dropStatus.SelectedValue);
            _parameterCache.Add("StartDate", ((DateTime)dpFrom.SelectedDate).ToString("yyyy-MM-dd"));
            _parameterCache.Add("EndDate", ((DateTime)dpTo.SelectedDate).AddDays(1).ToString("yyyy-MM-dd"));            

            //关键字查询
            if (tbStrKey.Text.Length > 0)
            {
                _parameterCache.Add("tbStrKey", TextUtility.ReplaceText(tbStrKey.Text));
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
                ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + dropCustomer.ClientID + "')).listWidth=250;");  
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
            //if (dropDictLab.SelectedText == "全部")
            //{
            //    MessageBoxShow("必须选择一个分点");
            //    return;
            //}
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
        /// 确认采血事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gdCollectBlood.SelectedRowIndexArray.Length == 0)
            {
                return;
            }
            if (dropStatus.SelectedValue == "-1")
            {
                MessageBoxShow("请选择[未采血]状态再确认", MessageBoxIcon.Information);
                return;
            }
            if (dropStatus.SelectedValue == "10")
            {
                MessageBoxShow("已采血，不可以再次确认采血", MessageBoxIcon.Information);
                return;
            }
            foreach (int row in gdCollectBlood.SelectedRowIndexArray)
            {

                Hashtable ht = new Hashtable();
                UserInfo userInfo = (UserInfo)Session["UserInfo"];
                ht.Add("OrdersBarcodeIds", gdCollectBlood.DataKeys[row][0].ToString()); 
                ht.Add("UserName", userInfo.userId);
                orderbarcodeService.EnSureCollectBlood(ht);


                orderbarcodeService.AddOperationLog(gdCollectBlood.DataKeys[row][2].ToString(),
               gdCollectBlood.DataKeys[row][1].ToString(), "采血确认", "确认标本已采血", "修改留痕", "");
            }
            gdCollectBlood.SelectedRowIndexArray = null;
            BindGrid();
            MessageBoxShow("确认成功", MessageBoxIcon.Information);
        }

        #region >>> 根据扫描进来的条码号修改为已采血状态 zhangwei
        protected void tbEnsureBarcode_TriggerClick(object sender, EventArgs e)
        {
            #region
            ////是否存在该条码
            //bool ischeck = false;
            ////清空旧条码
            //int j = tbEnsureBarcode.Text.IndexOf((char)2);
            //if (j >= 0)
            //{

            //    tbEnsureBarcode.Text = tbEnsureBarcode.Text.Substring(j + 1) + (char)2;
            //}
            //else
            //{
            //    tbEnsureBarcode.Text = tbEnsureBarcode.Text + (char)2;
            //}
            //List<int> selectedRowIndexArray = gdCollectBlood.SelectedRowIndexArray.ToList();
            //for (int i = 0; i < gdCollectBlood.Rows.Count; i++)
            //{
            //    object[] dataKeys = gdCollectBlood.DataKeys[i];
            //    if (tbEnsureBarcode.Text.Replace(((char)2).ToString(), "") == dataKeys[1].ToString())
            //    {
            //        if (selectedRowIndexArray.IndexOf(i) >= 0)
            //        {
            //            MessageBoxShow("该条码号已扫描");
            //            this.tbEnsureBarcode.Text = string.Empty;
            //            return;
            //        }
            //        selectedRowIndexArray.Add(i);
            //        ischeck = true;
            //    }
            //}
            //if (selectedRowIndexArray.Count > 0)
            //{
            //    gdCollectBlood.SelectedRowIndexArray = selectedRowIndexArray.ToArray();
            //}
            //if (!ischeck)
            //{
            //    MessageBoxShow("没有找到该条码号");
            //}
            //this.tbEnsureBarcode.Text = string.Empty;
            #endregion
            if (tbEnsureBarcode.Text.Trim() == "")
            {
                MessageBoxShow("条码号不能为空！", MessageBoxIcon.Information);
                return;
            }
            Hashtable ht1 = new Hashtable();
            ht1.Add("ordebarcode", this.tbEnsureBarcode.Text.Trim());
            ht1.Add("status", 5);
            //是否存在有该未采血的条码
            List<Orderbarcode> ordrbarcodeList = orderbarcodeService.SelectOrderbarcode(ht1).ToList();
            if (ordrbarcodeList.Count == 0)
            {
                MessageBoxShow("您要确认的条码号不存在或已采过血！", MessageBoxIcon.Information);
                this.tbEnsureBarcode.Text = string.Empty;
                return;
            }
            Hashtable ht = new Hashtable();
            UserInfo userInfo = (UserInfo)Session["UserInfo"];
            ht.Add("OrdersBarcodeIds", ordrbarcodeList[0].Orderbarcodeid);
            ht.Add("UserName", userInfo.userId);
            //修改状态为已采血
            int affectRow = orderbarcodeService.EnSureCollectBlood(ht);
            if (affectRow > 0)
            {
                Hashtable htback = new Hashtable();
                htback.Add("tbStrKey", ordrbarcodeList[0].Barcode);
                DataTable dt = orderbarcodeService.DataForCollectBlood(htback);
                //修改为已采血的条码信息添加到临时集合中显示
                Orderbarcode orderbarcodeback = new Orderbarcode();
                CreateOrder(dt, orderbarcodeback);
                orderdatatable.Add(orderbarcodeback);
                //绑定前20条数据
                var order = (from da in orderdatatable orderby da.Collectdate descending select da).Take(20);
                gdCollectBlood.DataSource = order;
                gdCollectBlood.DataBind();
                orderbarcodeService.AddOperationLog(ordrbarcodeList[0].Ordernum,
                        ordrbarcodeList[0].Barcode, "采血确认", "确认标本已采血", "修改留痕", "");          
                this.tbEnsureBarcode.Text = string.Empty;
            }
        }
        #endregion
       
         
        /// <summary>存放集合
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="orderbarcodeback"></param>
        private static void CreateOrder(DataTable dt, Orderbarcode orderbarcodeback)
        {
            orderbarcodeback.Orderbarcodeid = Convert.ToDouble(dt.Rows[0]["Orderbarcodeid"]);
            orderbarcodeback.Ordernum = dt.Rows[0]["Ordernum"].ToString();
            orderbarcodeback.Barcode = dt.Rows[0]["Barcode"].ToString();
            orderbarcodeback.Ensurestatus = dt.Rows[0]["STATUS"].ToString() == "5" ? "未采血" : "已采血";
            orderbarcodeback.Collectdate = Convert.ToDateTime(dt.Rows[0]["Collectdate"]);
            orderbarcodeback.Realname = dt.Rows[0]["Realname"].ToString();
            orderbarcodeback.Itemname = dt.Rows[0]["Itemname"].ToString();
            orderbarcodeback.Username = dt.Rows[0]["Username"].ToString();
            orderbarcodeback.Labdeptname = dt.Rows[0]["Labdeptname"].ToString();
            orderbarcodeback.Testnames = dt.Rows[0]["Testnames"].ToString();           
        }
        #endregion
    }
}