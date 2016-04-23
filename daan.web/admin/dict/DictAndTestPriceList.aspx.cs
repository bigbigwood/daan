using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using daan.domain;
using daan.util.Web;
using System.Collections;
using daan.service.dict;
using System.Text;
using System.Data;
using daan.service.login;
using daan.web.code;

namespace daan.web.admin.dict
{
    public partial class DictAndTestPriceList : PageBase
    {
        //分点试验室Id
        public double DictlabId
        {
            get { return Convert.ToDouble(Convert.ToInt32(ViewState["DictlabId"]) == 0 ? 0 : ViewState["DictlabId"]); }
            set { ViewState["DictlabId"] = value; }
        }

        //分点测试项Id
        public double DicttestitemId
        {
            get { return Convert.ToDouble(Convert.ToInt32(ViewState["DicttestitemId"]) == 0 ? 0 : ViewState["DicttestitemId"]); }
            set { ViewState["DicttestitemId"] = value; }
        }

        //价格ID
        public double dictCountedId
        {
            get { return Convert.ToDouble(Convert.ToInt32(ViewState["dictCountedId"]) == 0 ? 0 : ViewState["dictCountedId"]); }
            set { ViewState["dictCountedId"] = value; }
        }
        string erreyType = "";
        LoginService loginservice = new LoginService();
        Dictlabandtest dictlabandtest = new Dictlabandtest();
        Dictlabandtestprice dictlabandtestprice = new Dictlabandtestprice();
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDrop();
            }
            dictlabandtestprice.Dictlabandtestpriceid = dictCountedId;
            if (this.DropDictLab.SelectedValue != null && this.DropDictLab.SelectedValue != "")
            {
                DictlabId = Convert.ToDouble(this.DropDictLab.SelectedValue.ToString());

            }
            else
            {
                DictlabId = 0;
            }
            btnDelAll.OnClientClick = GridList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", GridList.GetSelectCountReference());
        }

        /// <summary>
        /// 绑定分点下拉框
        /// </summary>
        private void BindDrop()
        {
            try
            {
                List<Dictlab> dictlab = new DictlabService().GetDictlabList();
                this.DropDictLab.DataSource = dictlab;
                this.DropDictLab.DataTextField = "Labname";
                this.DropDictLab.DataValueField = "Dictlabid";
                this.DropDictLab.DataBind();
                //this.DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
            }
            catch (Exception ex)
            {
                
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }


        }

        public string GetType(object msg)
        {
            string flag = "";
            if (msg.ToString() != "")
            {
                if (msg.ToString() == "0")
                {
                    flag = "单项";
                }
                else if (msg.ToString() == "1")
                {
                    flag = "组合";
                }
                else if (msg.ToString() == "2")
                {
                    flag = "公用套餐";
                }
                else if (msg.ToString() == "3")
                {
                    flag = "客户套餐";
                }
            }
            else
            {
                flag = "";
            }
            return flag;
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindGrid(double dictid)
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht1.Add("IsActive", chkActive.Checked ? "1" : "0");
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("DictlabId", dictid);
                //设置总项数
                gvList.RecordCount = new DictlabandtestService().GetDictlabandtestPageLstCount(ht1);
                gvList.DataSource = new DictlabandtestService().GetDictlabandtestPageLst(ht1);
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
            BindGrid(DictlabId);
            this.tbxFinalprice.Text = string.Empty;
            this.DatePicker1.Text = string.Empty;
            this.DatePicker2.Text = string.Empty;
            dictCountedId = 0;
            dictlabandtestprice.Dictlabandtestpriceid = 0;
        }

        // 执行清空动作
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }

        // 执行搜索动作   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {
            BindGrid(DictlabId);
            btSearch.ShowTrigger1 = true;
        }

        /// <summary>
        /// 分点下拉框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDictLab.SelectedValue != "-1")
            {
                //BindGrid(Convert.ToDouble(this.DropDictLab.SelectedValue.ToString()));
                dictlabandtest.Dictlabid = Convert.ToDouble(this.DropDictLab.SelectedValue.ToString());
                DictlabId = Convert.ToDouble(dictlabandtest.Dictlabid);
                DicttestitemId = Convert.ToDouble(dictlabandtest.Dicttestitemid);
                BindGrid();

            }
            else
            {
                DictlabId = 0;
                dictCountedId = 0;
                DicttestitemId = 0;
                BindGrid(DictlabId);
                BindGrid();
                this.tbxFinalprice.Text = string.Empty;
                this.DatePicker1.Text = string.Empty;
                this.DatePicker2.Text = string.Empty;
            }
        }



        #region 绑定详细信息
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
                        dictlabandtest.Dictlabandtestid = Convert.ToInt32(keys[0]);
                        if (dictlabandtest.Dictlabandtestid != 0)
                        {
                            List<Dictlabandtest> dictlabandtestList = loginservice.GetLoginDictlabandtest();
                            dictlabandtest = dictlabandtestList.Where<Dictlabandtest>(c => c.Dictlabandtestid == dictlabandtest.Dictlabandtestid).First<Dictlabandtest>();
                            DictlabId = Convert.ToDouble(dictlabandtest.Dictlabid);
                            DicttestitemId = Convert.ToDouble(dictlabandtest.Dicttestitemid);
                            BindGrid();
                            //DictlabId = 0;
                            //dictCountedId = 0;
                            //DicttestitemId = 0;
                            //BindGrid(DictlabId);
                            this.tbxFinalprice.Text = string.Empty;
                            this.DatePicker1.Text = string.Empty;
                            this.DatePicker2.Text = string.Empty;
                            this.Form1.Title = "当前状态-新增";
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


        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(GridList.PageIndex, GridList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", Dp_Bingin.Text.ToString() == "" ? null : Dp_Bingin.Text);
                ht1.Add("endDate", this.DatePicker3.Text.ToString() == "" ? null : this.DatePicker3.Text);
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                ht1.Add("DictlabId", DictlabId);
                ht1.Add("DicttestitemId", DicttestitemId);
                //设置总项数
                GridList.RecordCount = new DictlabandtestpriceService().GetGetDictlabandtestpricePageLstCount(ht1);
                GridList.DataSource = new DictlabandtestpriceService().GetGetDictlabandtestpricePageLstPageLst(ht1);
                GridList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        //分页
        protected void GridList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridList.PageIndex = e.NewPageIndex;
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
                foreach (int row in GridList.SelectedRowIndexArray)
                {
                    sb.Append(GridList.DataKeys[row][0].ToString());
                    sb.Append(",");
                }
                int nflag = new DictlabandtestpriceService().DelDictlabandtestpriceByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    
                    MessageBoxShow("所选项已成功删除！", MessageBoxIcon.Information);
                    BindGrid();
                    this.tbxFinalprice.Text = string.Empty;
                    this.DatePicker1.Text = string.Empty;
                    this.DatePicker2.Text = string.Empty;
                    dictCountedId = 0;
                    dictlabandtestprice.Dictlabandtestpriceid = 0;
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
                this.Form1.Title = "当前状态-新增";
                this.tbxFinalprice.Text = string.Empty;
                this.DatePicker1.Text = string.Empty;
                this.DatePicker2.Text = string.Empty;
                dictCountedId = 0;
                dictlabandtestprice.Dictlabandtestpriceid = 0;
             
            }
            else
            {
                MessageBoxShow(erreyType, MessageBoxIcon.Error);
                this.tbxFinalprice.Text = string.Empty;
                this.DatePicker1.Text = string.Empty;
                this.DatePicker2.Text = string.Empty;
                dictCountedId = 0;
                dictlabandtestprice.Dictlabandtestpriceid = 0;
                return;
            }
        }

        //保存数据的逻辑
        public bool SaveDictlibrary()
        {
            try
            {
                if (DictlabId != 0)
                {
                    if (DicttestitemId != 0)
                    {
                        #region
                        if (this.tbxFinalprice.Text.Trim() != "") //报价
                        {
                            dictlabandtestprice.Price = Convert.ToDouble(this.tbxFinalprice.Text.Trim());
                        }
                        if (this.DatePicker1.Text.Trim() != "")  //开始时间
                        {
                            Dictlabandtestprice dictCustomer = new DictlabandtestpriceService().GetDictlabandtestpriceById(dictCountedId);
                            if (dictCustomer != null)
                            {
                                if (dictCustomer.Begindate != this.DatePicker1.SelectedDate || dictCustomer.Enddate != this.DatePicker2.SelectedDate)
                                {
                                    if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                                    {
                                        Hashtable ht1 = new Hashtable();
                                        ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                                        ht1.Add("begdate", this.DatePicker1.Text.Trim());
                                        ht1.Add("itemsId", DicttestitemId);
                                        ht1.Add("Dictlabid", DictlabId);
                                        ht1.Add("Dictlabandtestpriceid", dictCountedId);
                                        IList<Dictlabandtestprice> ds = new DictlabandtestpriceService().GetDictlabandtestpriceByTime(ht1);
                                        if (ds.Count == 0)
                                        {
                                            dictlabandtestprice.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                                        }
                                        else
                                        { 
                                            erreyType = "不能小于原始记录时间！";
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
                                        dictlabandtestprice.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
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
                                if (this.DatePicker1.SelectedDate <= this.DatePicker2.SelectedDate)
                                {
                                    Hashtable ht1 = new Hashtable();
                                    ht1.Add("Enddate", this.DatePicker2.Text.Trim());
                                    ht1.Add("begdate", this.DatePicker1.Text.Trim());
                                    ht1.Add("itemsId", DicttestitemId);
                                    ht1.Add("Dictlabid", DictlabId);
                                    // ht1.Add("itemsId", this.Drop_DictTestItemId.SelectedValue);
                                    IList<Dictlabandtestprice> ds = new DictlabandtestpriceService().GetDictlabandtestpriceByTimeTo(ht1);
                                    if (ds.Count == 0)
                                    {
                                        dictlabandtestprice.Begindate = Convert.ToDateTime(this.DatePicker1.Text.Trim());
                                    }
                                    else
                                    {
                                        //this.DatePicker1.CompareMessage = "不能小于原始记录时间";
                                        erreyType = "不能小于原始记录时间！";
                                        return false;
                                    }
                                }
                                else
                                {
                                    //this.DatePicker1.CompareMessage = "开始时间应小于结束时间";
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
                            dictlabandtestprice.Enddate = Convert.ToDateTime(this.DatePicker2.Text.Trim());
                        }
                        else
                        {
                            erreyType = "结束时间不能为空！";
                            return false;
                        }
                        dictlabandtestprice.Dicttestitemid = DicttestitemId;
                        dictlabandtestprice.Dictlabid = DictlabId;
                        if (dictlabandtestprice.Dictlabandtestpriceid == 0)
                        {
                            dictlabandtestprice.Createdate = DateTime.Now;
                        }
                        else
                        {

                            Dictlabandtestprice dictCountedprice = new DictlabandtestpriceService().GetDictlabandtestpriceById(Convert.ToDouble(dictlabandtestprice.Dictlabandtestpriceid));
                            dictlabandtestprice.Createdate = dictCountedprice.Createdate;
                        }
                        return new DictlabandtestpriceService().SaveDictlabandtestprice(dictlabandtestprice);
                        #endregion
                    }
                    else
                    {
                        erreyType = "您还未选择检测项目暂不能添加数据！";
                        return false;
                    }

                }
                else
                {
                    erreyType = "您还未选择分点暂不能添加数据！";
                    return false;
                }
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
        protected void GridList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = GridList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                        int id = Convert.ToInt32(GridList.DataKeys[e.RowIndex][0].ToString());//获取Grid1中第e.RowIndex+1行的第一个DateKeyName值
                        //编辑绑定数据 
                        dictlabandtestprice.Dictlabandtestpriceid = id;
                        Dictlabandtestprice dictCountedprice = new DictlabandtestpriceService().GetDictlabandtestpriceById(id);
                        dictCountedId = Convert.ToDouble(dictlabandtestprice.Dictlabandtestpriceid);
                        this.tbxFinalprice.Text = dictCountedprice.Price.ToString();
                        this.DatePicker1.SelectedDate = dictCountedprice.Begindate;
                        this.DatePicker2.SelectedDate = dictCountedprice.Enddate;
                        this.Form1.Title = "当前状态-编辑";
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
            this.Form1.Title = "当前状态-新增";
            this.tbxFinalprice.Text = string.Empty;
            this.DatePicker1.Text = string.Empty;
            this.DatePicker2.Text = string.Empty;
            dictCountedId = 0;
            dictlabandtestprice.Dictlabandtestpriceid = 0;

        }
    }
}