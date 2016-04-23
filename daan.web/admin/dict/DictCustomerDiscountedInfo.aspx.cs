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
    public partial class DictCustomerDiscountedInfo : PageBase
    {
        LoginService loginservice = new LoginService();
        static DicttestitemService dicttestservice = new DicttestitemService();
        static DictcustomerdiscountedService cs = new DictcustomerdiscountedService();
        string errreyType = "";
        double CustomerId = 0;
        Dictcustomerdiscounted dictCounted = null;
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
                BindGrid();
                BindProductList();
                BindSubCompanyList();
            }
            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
        }

        /// <summary>
        /// 绑定套餐下拉列表
        /// </summary>
        private void BindProductList()
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetProduct(CustomerId);
            dropProducts.DataSource = TestItemList;
            dropProducts.DataTextField = "Testname";
            dropProducts.DataValueField = "Dicttestitemid";
            dropProducts.DataBind();
            dropProducts.Items.Insert(0,new ExtAspNet.ListItem("请选择套餐","-1"));
        }
        /// <summary>
        /// 绑定签约子公司列表
        /// </summary>
        private void BindSubCompanyList()
        {
            IList<daan.domain.DictSubCompany> list = new DictSubCompanyService().GetDictSubCompanyList("");
            dropSubCompany.DataSource = list;
            dropSubCompany.DataTextField = "SubCompanyName";
            dropSubCompany.DataValueField = "SubCompanyId";
            dropSubCompany.DataBind();
            dropSubCompany.Items.Insert(0,new ExtAspNet.ListItem("请选择","-1"));
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
                ht1.Add("strKey", Dp_Bingin.Text.ToString() == "" ? null : Dp_Bingin.Text);
                ht1.Add("endDate", this.DatePicker3.Text.ToString() == "" ? null : this.DatePicker3.SelectedDate.Value.AddDays(1).ToShortDateString());
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());  
                ht1.Add("Dictcustomerid", CustomerId);
                //设置总项数
                gvList.RecordCount = cs.GetDictcustomerdiscountedPageLstCount(ht1);
                gvList.DataSource = cs.GetDictcustomerdiscountedPageDt(ht1);
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
                    MessageBoxShow("请输入开始时间及结束时间查询",  MessageBoxIcon.Information);
                }
                else
                {
                    BindGrid();
                }
            }
        }

      

        #region 页面事件 删除
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
                int nflag = cs.DelDictcustomerdiscountedByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    Clear();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
        #endregion

        /// <summary>
        /// 时间控件文本改变事件
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
        /// 选择套餐带出标准价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropProducts.SelectedValue != "-1")
            {
                string dicttestitemid = dropProducts.SelectedValue;
                Hashtable ht = new Hashtable();
                ht.Add("Dicttestitemid", dicttestitemid);
                Dicttestitem product = dicttestservice.GetDicttestitemByID(ht).First<Dicttestitem>();
                txtProductPrice.Text = product.Price.ToString();
                txtTestCode.Text = product.Testcode;
                //txtPrice.Text = string.Empty;
            }
        }

        /// <summary>
        /// 保存
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
            }
            else
            {
                MessageBoxShow(errreyType,MessageBoxIcon.Error);
            }
            Clear();
        }
        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            dictCounted = new Dictcustomerdiscounted();
            if (gvList.SelectedRowIndexArray.Length > 0)
            {
                object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                dictCounted.Dictcustomerdiscountid = TypeParse.StrToDouble(objValue[0], 0);
            }
            dictCounted.Sendoutprice = 0;
            try
            {
                dictCounted.Discounted = Convert.ToDouble(tbxDiscounted.Text.Trim() == "" ? null : tbxDiscounted.Text.Trim());
                if (this.DatePicker1.Text.Trim() != "")  //开始时间
                {
                    Dictcustomerdiscounted dict = cs.GetDictcustomerdiscountedById(Convert.ToDouble(dictCounted.Dictcustomerdiscountid));
                    if (dict != null)
                    {
                        if (dict.Begindate != this.DatePicker1.SelectedDate || dict.Enddate != this.DatePicker2.SelectedDate)
                        {
                            #region
                            if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                            {
                                Hashtable ht1 = new Hashtable();
                                ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                                ht1.Add("begdate", this.DatePicker1.Text.Trim());
                                ht1.Add("Dictcustomerid", CustomerId);
                                ht1.Add("DictId", Convert.ToDouble(dictCounted.Dictcustomerdiscountid));
                                ht1.Add("ProductID", dropProducts.SelectedValue);
                                IList<Dictcustomerdiscounted> ds = cs.GetDictcustomerByTime(ht1);
                                if (ds.Count == 0)
                                {
                                    dictCounted.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                                }
                                else
                                {
                                    errreyType = "已存在重复时间段！";
                                    return false;
                                }
                            }
                            else
                            {
                                errreyType = "开始时间应小于结束时间！";
                                return false;
                            }
                            #endregion
                        }
                        else
                        {
                            if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                            {
                                dictCounted.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                            }
                            else
                            {
                                errreyType = "开始时间应小于结束时间！";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        #region
                        if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                        {
                            Hashtable ht1 = new Hashtable();
                            ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                            ht1.Add("begdate", this.DatePicker1.Text.Trim());
                            ht1.Add("Dictcustomerid", CustomerId);
                            ht1.Add("ProductID", dropProducts.SelectedValue);
                            IList<Dictcustomerdiscounted> ds = cs.GetDictcustomerByTimeTo(ht1);
                            if (ds.Count == 0)
                            {
                                dictCounted.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                            }
                            else
                            {
                                errreyType = "已存在重复时间段！";
                                return false;
                            }
                        }
                        else
                        {
                            errreyType = "开始时间应小于结束时间";
                            return false;
                        }
                        #endregion
                    }
                }
                else
                {
                    errreyType = "请输入开始时间！";
                    return false;
                }
                if (this.DatePicker2.Text.Trim() != "")  //结束时间
                {
                    dictCounted.Enddate = Convert.ToDateTime(this.DatePicker2.Text.Trim());
                }
                else
                {
                    errreyType = "请输入结束时间！";
                    return false;
                }
                if (lblAuditStatus.Text == "已审核")
                {
                    errreyType = "选择的单位套餐价格已审核，无法修改，请先取消审核后再修改！";
                    return false;
                }
                dictCounted.Dictcustomerid = CustomerId;
                if (Userinfo.userId != 0 )
                {
                    dictCounted.Updateby = Userinfo.userId;
                }
                else
                {
                    dictCounted.Updateby = 1;
                }
                dictCounted.Updatedate = DateTime.Now;
                if (dropProducts.SelectedValue == "-1")
                {
                    errreyType = "请选择套餐！";
                    return false;
                }
                else
                {
                    dictCounted.Productid = Convert.ToDouble(dropProducts.SelectedValue);//套餐
                }
                if (txtPrice.Text == "")
                {
                    errreyType = "请输入标准价格！";
                    return false;
                }
                else
                {
                    dictCounted.Price = Convert.ToDouble(txtPrice.Text.ToString().Trim());//价格
                }
                if (dropSubCompany.SelectedValue == "-1")
                {
                    errreyType = "请选择签约子公司!";
                    return false;
                }
                else
                {
                    dictCounted.DictSubCompanyID = Convert.ToDouble(dropSubCompany.SelectedValue);//签约子公司
                }
                if (dictCounted.Dictcustomerdiscountid == 0 || dictCounted.Dictcustomerdiscountid == null)
                {
                    dictCounted.Createdate = DateTime.Now;
                }
                else
                {
                    List<Dictcustomerdiscounted> dictcustomerdiscountedList = loginservice.GetDictcustomerdiscounted();//体检单位总体价格
                    Dictcustomerdiscounted dictcustomer = dictcustomerdiscountedList.Where<Dictcustomerdiscounted>(c => c.Dictcustomerdiscountid == dictCounted.Dictcustomerdiscountid).First<Dictcustomerdiscounted>();
                    dictCounted.Createdate = dictcustomer.Createdate;
                }
                return cs.SaveDictcustomerdiscounted(dictCounted);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 绑定详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        string id = gvList.DataKeys[e.RowIndex][0].ToString();
                        DataTable dt = cs.GetDictcustomerdiscountedById(id);
                        if (dt != null || dt.Rows.Count != 0)
                        {
                            DataRow dr = dt.Rows[0];
                            tbxDiscounted.Text = dr["Discounted"].ToString();
                            DatePicker1.SelectedDate = Convert.ToDateTime(dr["Begindate"].ToString());
                            DatePicker2.SelectedDate = Convert.ToDateTime(dr["Enddate"].ToString());
                            dropProducts.SelectedValue = dr["Productid"].ToString();
                            txtProductPrice.Text = dr["nprice"].ToString();
                            txtPrice.Text = dr["Price"].ToString();
                            txtTestCode.Text = dr["testcode"].ToString();
                            dropSubCompany.SelectedValue = dr["DictSubCompanyID"].ToString();
                            lblAuditStatus.Text = dr["Active"].ToString() == "1" ? "已审核" : "未审核";
                            txtUpdateBy.Text = dr["UpdatebyName"].ToString();
                            txtUpdateDate.Text = dr["Updatedate"].ToString();
                            txtAuditBy.Text = dr["Active"].ToString() == "1" ? dr["AuditByName"].ToString() : "";
                            txtAuditDate.Text = dr["Active"].ToString() == "1" ? dr["AuditDate"].ToString() : "";
                        }
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
            Clear();
        }

        private void Clear()
        {
            this.tbxDiscounted.Text = string.Empty;
            this.DatePicker1.Text = string.Empty;
            this.DatePicker2.Text = string.Empty;
            gvList.SelectedRowIndexArray = new int[] { };
            dropProducts.SelectedValue = "-1";
            txtProductPrice.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtAuditBy.Text = string.Empty;
            txtAuditDate.Text = string.Empty;
            txtUpdateBy.Text = string.Empty;
            txtUpdateDate.Text = string.Empty;
            lblAuditStatus.Text = "未审核";
            dropSubCompany.SelectedValue = "-1";
            txtTestCode.Text = string.Empty;
        }
    }
}