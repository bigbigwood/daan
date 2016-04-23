using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using daan.util.Web;
using System.Collections;
using daan.service.dict;
using ExtAspNet;
using daan.domain;
using System.Data;
using daan.service.login;
using daan.util.Common;

namespace daan.web.admin.dict
{
    public partial class DictCustomerDiscountInfo : PageBase
    {
        LoginService loginservice = new LoginService();
        //单位Id
        //public double CustomerId
        //{
        //    get { return Convert.ToDouble(Convert.ToDouble(ViewState["CustomerId"]) == 0 ? 0 : ViewState["CustomerId"]); }
        //    set { ViewState["CustomerId"] = value; }
        //}

        //价格ID
        //public double dictCountedId
        //{
        //    get { return Convert.ToDouble(Convert.ToInt32(ViewState["dictCountedId"]) == 0 ? 0 : ViewState["dictCountedId"]); }
        //    set { ViewState["dictCountedId"] = value; }
        //}

        string erreyType = "";
        //double dictCountedId = 0;
        double CustomerId = 0;
        Dictcustomertestdiscount dictcustomercount = null;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerId = Request.QueryString["id"] == null ? 0 : Convert.ToDouble(Request.QueryString["id"].ToString());
            if (!IsPostBack)
            {

                //BindGrid();
                BindDrop();
            }

            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
            //dictcustomercount.Dictcustomerdiscountid = dictCountedId;
        }

        /// <summary>
        /// 绑定测试项
        /// </summary>
        private void BindDrop()
        {
            try
            {
                Dictcustomer dictcustomer = new DictCustomerService().GetDictCustomerById(CustomerId);
                Hashtable ht = new Hashtable();
                ht.Add("dictlabId", dictcustomer.Dictlabid);
                this.Drop_DictTestItemId.DataSource = new DictlabandtestService().GetTestItem(ht);
                this.Drop_DictTestItemId.DataTextField = "Testname";
                this.Drop_DictTestItemId.DataValueField = "Dicttestitemid";
                this.Drop_DictTestItemId.DataBind();
                this.Drop_DictTestItemId.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", Dp_Bingin.Text.Trim() == "" ? null : Dp_Bingin.Text.Trim());
                ht1.Add("endDate", this.DatePicker3.Text.ToString() == "" ? null : this.DatePicker3.Text);
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("Dictcustomerid", CustomerId);
                //设置总项数
                gvList.RecordCount = new DictcustomertestdiscountService().GetDictcustomerdiscountPageLstCount(ht1);
                gvList.DataSource = new DictcustomertestdiscountService().GetDictcustomerdiscountPageLst(ht1);
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
            BindGrid();
        }

        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.Dp_Bingin.Text != "" && this.DatePicker3.Text != "")
            {
                if (this.Dp_Bingin.SelectedDate <= this.DatePicker3.SelectedDate)
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
                if (this.Dp_Bingin.Text != "" || this.DatePicker3.Text != "")
                {
                    MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                }
                else
                {
                    BindGrid();
                }
            }
        }

        #region >>>> 页面事件 删除
        protected void btnDelAll_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (int row in gvList.SelectedRowIndexArray)
                {
                    sb.Append(gvList.DataKeys[row][0].ToString());
                    sb.Append(",");
                }
                var library = new DictCustomerService();
                int nflag = new DictcustomertestdiscountService().DelDictcustomerdiscountByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    BindGrid();
                    gvList.SelectedRowIndexArray = new int[] { };
                    //this.Drop_DictTestItemId.SelectedValue = "-1";
                    this.tbxFinalprice.Text = string.Empty;
                    this.DatePicker1.Text = string.Empty;
                    this.DatePicker2.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }


        #endregion

        /// <summary>
        /// 时间控件的改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (DatePicker1.SelectedDate.HasValue)
            {
                DatePicker2.SelectedDate = DatePicker1.SelectedDate.Value.AddDays(3);
            }
        }




        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (SaveDictlibrary())
            {
                BindGrid();
                MessageBoxShow("保存成功！");
                SimpleFormEdit.Title = "当前状态-新增";
                this.Drop_DictTestItemId.SelectedItem.Value = "-1";
               // this.Drop_DictTestItemId.SelectedValue = "-1";
                this.tbxFinalprice.Text = string.Empty;
                this.DatePicker1.Text = string.Empty;
                this.DatePicker2.Text = string.Empty;
                gvList.SelectedRowIndexArray = new int[] { };


            }
            else
            {
                MessageBoxShow(erreyType, MessageBoxIcon.Error);
                this.Drop_DictTestItemId.SelectedItem.Value = "-1";
              //  this.Drop_DictTestItemId.SelectedValue = "-1";
                this.tbxFinalprice.Text = string.Empty;
                this.DatePicker1.Text = string.Empty;
                this.DatePicker2.Text = string.Empty;
                gvList.SelectedRowIndexArray = new int[] { };
                return;
            }
        }

        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            try
            {
                dictcustomercount = new Dictcustomertestdiscount();
                if (gvList.SelectedRowIndexArray.Length > 0)
                {
                    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                    dictcustomercount.Dictcustomerdiscountid = TypeParse.StrToDouble(objValue[0], 0);
                }
                if (this.tbxFinalprice.Text.Trim() != "") //报价
                {
                    dictcustomercount.Finalprice = Convert.ToDouble(this.tbxFinalprice.Text.Trim());
                }
                if (this.Drop_DictTestItemId.SelectedValue != "-1")  //测试项
                {
                    dictcustomercount.Dicttestitemid = Convert.ToDouble(this.Drop_DictTestItemId.SelectedValue);
                }
                else
                {
                    erreyType = "测试项不能为空！";
                    return false;
                }
                if (this.DatePicker1.Text.Trim() != "")  //开始时间
                {
                    Dictcustomertestdiscount dictCustomer = new DictcustomertestdiscountService().GetDictcustomerdiscountById(Convert.ToDouble(dictcustomercount.Dictcustomerdiscountid));
                    if (dictCustomer != null)
                    {
                        #region
                        if (dictCustomer.Begindate != this.DatePicker1.SelectedDate || dictCustomer.Enddate != this.DatePicker2.SelectedDate)
                        {
                            if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                            {
                                Hashtable ht1 = new Hashtable();
                                ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                                ht1.Add("begdate", this.DatePicker1.Text.Trim());
                                ht1.Add("itemsId", this.Drop_DictTestItemId.SelectedValue);
                                ht1.Add("Dictcustomerdiscountid", Convert.ToDouble(dictcustomercount.Dictcustomerdiscountid));
                                ht1.Add("Dictcustomerid", CustomerId);

                                IList<Dictcustomertestdiscount> ds = new DictcustomertestdiscountService().GetDictcustomerByTime(ht1);
                                if (ds.Count == 0)
                                {
                                    dictcustomercount.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                                }
                                else
                                {

                                    erreyType = "已存在重复时间段！";
                                    return false;
                                }
                            }
                            else
                            {
                                erreyType = "开始时间应小于结束时间！";
                                return false;
                            }
                        }
                        else
                        {
                            if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                            {
                                dictcustomercount.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                            }
                            else
                            {
                                erreyType = "开始时间应小于结束时间！";
                                return false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                        {
                            Hashtable ht1 = new Hashtable();
                            ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                            ht1.Add("begdate", this.DatePicker1.Text.Trim());
                            ht1.Add("itemsId", this.Drop_DictTestItemId.SelectedValue);
                            ht1.Add("Dictcustomerid", CustomerId);
                            IList<Dictcustomertestdiscount> ds = new DictcustomertestdiscountService().GetDictcustomerByTimeTo(ht1);
                            if (ds.Count == 0)
                            {
                                dictcustomercount.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                            }
                            else
                            {
                                erreyType = "已存在重复时间段！";
                                return false;
                            }
                        }
                        else
                        {
                            erreyType = "开始时间应小于结束时间！";
                            return false;
                        }
                    }
                }
                else
                {
                    erreyType = "开始时间不能为空！";
                    return false;
                }
                if (this.DatePicker2.Text.Trim() != "")  //结束时间
                {
                    dictcustomercount.Enddate = Convert.ToDateTime(this.DatePicker2.Text.Trim());
                }
                else
                {
                    erreyType = "结束时间不能为空！";
                    return false;
                }
                dictcustomercount.Dictcustomerid = CustomerId;
                if (Userinfo.userId != 0)
                {
                    dictcustomercount.Updateby = Userinfo.userId;
                }
                else
                {
                    dictcustomercount.Updateby = 1;
                }
                dictcustomercount.Updatedate = DateTime.Now;
                if (dictcustomercount.Dictcustomerdiscountid == 0 || dictcustomercount.Dictcustomerdiscountid == null)
                {
                    dictcustomercount.Createdate = DateTime.Now;
                }
                else
                {
                    List<Dictcustomertestdiscount> dictcustomerdiscountedList = loginservice.GetDictcustomerdiscount();//体检单位总体价格
                    Dictcustomertestdiscount dictcustomer = dictcustomerdiscountedList.Where<Dictcustomertestdiscount>(c => c.Dictcustomerdiscountid == dictcustomercount.Dictcustomerdiscountid).First<Dictcustomertestdiscount>();
                    dictcustomercount.Createdate = dictcustomer.Createdate;
                }
                return new DictcustomertestdiscountService().SaveDictcustomerdiscount(dictcustomercount);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = gvList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                        int id = Convert.ToInt32(gvList.DataKeys[e.RowIndex][0].ToString());//获取Grid1中第e.RowIndex+1行的第一个DateKeyName值
                        //编辑绑定数据 
                        List<Dictcustomertestdiscount> dictcustomerdiscountedList = loginservice.GetDictcustomerdiscount();//体检单位总体价格
                        Dictcustomertestdiscount dictcustomer = dictcustomerdiscountedList.Where<Dictcustomertestdiscount>(c => c.Dictcustomerdiscountid == id).First<Dictcustomertestdiscount>();
                        //Dictcustomertestdiscount dictCounted = new DictcustomertestdiscountService().GetDictcustomerdiscountById(id);
                        //dictCountedId = Convert.ToDouble(dictcustomer.Dictcustomerdiscountid);
                        this.tbxFinalprice.Text = dictcustomer.Finalprice.ToString();
                        this.Drop_DictTestItemId.SelectedValue = dictcustomer.Dicttestitemid.ToString();
                        this.DatePicker1.SelectedDate = dictcustomer.Begindate;
                        this.DatePicker2.SelectedDate = dictcustomer.Enddate;
                        SimpleFormEdit.Title = "当前状态-编辑";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SimpleFormEdit.Title = "当前状态-新增";
            this.Drop_DictTestItemId.SelectedItem.Value = "-1";
            this.tbxFinalprice.Text = string.Empty;
            this.DatePicker1.Text = string.Empty;
            this.DatePicker2.Text = string.Empty;
            gvList.SelectedRowIndexArray = new int[] { };


        }

    }
}