using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.util.Web;
using System.Collections;
using daan.service.dict;
using daan.domain;
using System.Text;
using ExtAspNet;
using daan.util.Common;
using daan.web.code;

namespace daan.web.admin.dict
{
    public partial class DictCustomerInfo : PageBase
    {
        DictCustomerService dictCustomerService = new DictCustomerService();
        Dictcustomer dictCustomer = new Dictcustomer();
        string erreyType = "";       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                LoadData();
            }
            btnDelAll.OnClientClick = gvList.GetNoSelectionAlertReference("至少选择一项！");
            btnDelAll.ConfirmText = String.Format("确定要删除<script>{0}</script> 项纪录吗？", gvList.GetSelectCountReference());
        }

        /// <summary>初始化信息
        /// 
        /// </summary>
        private void LoadData()
        {
            try
            {
                //绑定财务人员
                this.dropDictcheckBillId.DataSource = new DictuserService().GetDictuser();
                this.dropDictcheckBillId.DataTextField = "Username";
                this.dropDictcheckBillId.DataValueField = "Dictuserid";
                this.dropDictcheckBillId.DataBind();         

                //绑定销售人员
                this.dropDictsalemanId.DataSource = new DictuserService().GetDictuser();
                this.dropDictsalemanId.DataTextField = "Username";
                this.dropDictsalemanId.DataValueField = "Dictuserid";
                this.dropDictsalemanId.DataBind();


                //绑定分点
                List<Dictlab> lisSource = new DictlabService().GetDictlabList();
                this.dropDictLab.DataSource = lisSource;
                this.dropDictLab.DataTextField = "Labname";
                this.dropDictLab.DataValueField = "Dictlabid";
                this.dropDictLab.DataBind();

                //绑定查询分点
                this.dropLabSearch.DataSource = lisSource;
                this.dropLabSearch.DataTextField = "Labname";
                this.dropLabSearch.DataValueField = "Dictlabid";
                this.dropLabSearch.DataBind();
                this.dropLabSearch.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        

        // 执行搜索   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {            
            BindGrid();            
            EditClear();
            btSearch.ShowTrigger1 = true;
        }

        // 清空搜索框关键词
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        // 新增 
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EditClear();
        }

        // 编辑 绑定项目详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                Form1.Title = "当前状态-编辑";
                dictCustomer = new DictCustomerService().GetDictCustomerById(Convert.ToDouble(gvList.DataKeys[e.RowIndex][0]));
                this.radlActive.SelectedValue = dictCustomer.Active;
                this.radlCustomerType.SelectedValue = dictCustomer.Customertype;
                this.tbxAddress.Text = dictCustomer.Address;
                this.tbxContactman.Text = dictCustomer.Contactman;
                this.tbxContactPhone.Text = dictCustomer.Contactphone;
                this.tbxCoustomerCode.Text = dictCustomer.Customercode;
                this.tbxCoustomerName.Text = dictCustomer.Customername;
                this.tbxDisplayOrder.Text = dictCustomer.Displayorder.ToString();
                this.tbxDocumentCode.Text = dictCustomer.Documentcode;
                this.tbxDocumentType.Text = dictCustomer.Documenttype;
                this.tbxEmail.Text = dictCustomer.Email;
                this.tbxEnAddress.Text = dictCustomer.Engaddress;
                this.tbxEnErpCode.Text = dictCustomer.Erpcode;
                this.tbxEnName.Text = dictCustomer.Customerengname;
                this.tbxErpName.Text = dictCustomer.Erpname;
                this.tbxFastCode.Text = dictCustomer.Fastcode;
                this.tbxFax.Text = dictCustomer.Fax;
                this.tbxPostCode.Text = dictCustomer.Postcode;
                this.tbxReporTitle.Text = dictCustomer.Reporttitle;
                this.tbxTelePhone.Text = dictCustomer.Telephone;
                this.tbxCoustomerName2.Text = dictCustomer.Customername2;
                this.TxaRemark.Text = dictCustomer.Remark;
                this.dropDictcheckBillId.SelectedValue = dictCustomer.Dictcheckbillid.ToString();
                this.dropDictLab.SelectedValue = dictCustomer.Dictlabid.ToString();
                this.dropDictsalemanId.SelectedValue = dictCustomer.Dictsalemanid.ToString();
                this.dropStatus.SelectedValue = dictCustomer.Status;
                if (dictCustomer.Issms == "1")
                    this.chkIssms.Checked = true;
                else
                    this.chkIssms.Checked = false;

                this.radIsPub.SelectedValue = dictCustomer.IsPublic;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }     
        
        // 删除
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
                int nflag = dictCustomerService.DelDictcustomerByID(sb.ToString().TrimEnd(','));
                if (nflag > 0)
                {
                    MessageBoxShow("所选项已成功删除", MessageBoxIcon.Information);
                    btSearch_Trigger2Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        // 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveDictlibrary())
            {
                MessageBoxShow("保存成功！");
                btSearch_Trigger2Click(null,null);
            }
            else
            {
                MessageBoxShow(erreyType);
            }
        }

        //设置单位折扣价格       
        protected void btnPrice_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRowIndexArray.Count() <= 0)
            {
                MessageBoxShow("请在列表中选择单位", MessageBoxIcon.Information);
                return;
            }

            object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];

            int Dictlabandtestid = Convert.ToInt32(objValue[0]);//获取Grid1中第e.RowIndex+1行的第一个DateKeyName值
            string Customername = objValue[1].ToString();
            string Customertype = objValue[2].ToString();
            //if (objValue[3].ToString() == "0")
            //{
            //    MessageBoxShow("选中的单位信息未审核，请先通过审核再设置价格", MessageBoxIcon.Information);
            //    return;
            //}
            //弹出价格设置窗口 
            if (Customertype == "0") //一般客户
            {
                string url = "DictCustomerDiscountedInfo.aspx?id=" + Dictlabandtestid;
                string title = "体检单位套餐价格维护 - " + Customername;
                ShowWindow(url, title);
            }
            else if (Customertype == "1") //外包客户
            {
                string url = "DictCustomerDiscountInfo.aspx?id=" + Dictlabandtestid;
                string title = "外包单位总体价格维护 - " + Customername;
                ShowWindow(url, title);
            }
        }

        /// <summary>绑定列表
        /// 
        /// </summary>
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht1.Add("IsActive", chkActive.Checked ? "1" : "0");
                ht1.Add("Dictlabid", dropLabSearch.SelectedValue);
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                //设置总项数
                gvList.RecordCount = new DictCustomerService().GetDictCustomerPageLstCount(ht1);
                gvList.DataSource = new DictCustomerService().GetDictCustomerPageLst(ht1);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>清空
        /// 
        /// </summary>
        protected void EditClear()
        {
            Form1.Title = "当前状态-新增";

            gvList.SelectedRowIndexArray = new int[] { };

            dictCustomer = new Dictcustomer();
            this.radlActive.SelectedValue = "0";// dictCustomer.Active;
            this.radlCustomerType.SelectedValue = "0";// dictCustomer.Customertype;
            this.tbxAddress.Text = dictCustomer.Address;
            this.tbxContactman.Text = dictCustomer.Contactman;
            this.tbxContactPhone.Text = dictCustomer.Contactphone;
            this.tbxCoustomerCode.Text = dictCustomer.Customercode;
            this.tbxCoustomerName.Text = dictCustomer.Customername;
            this.tbxDisplayOrder.Text = dictCustomer.Displayorder.ToString();
            this.tbxDocumentCode.Text = dictCustomer.Documentcode;
            this.tbxDocumentType.Text = dictCustomer.Documenttype;
            this.tbxEmail.Text = dictCustomer.Email;
            this.tbxEnAddress.Text = dictCustomer.Engaddress;
            this.tbxEnErpCode.Text = dictCustomer.Erpcode;
            this.tbxEnName.Text = dictCustomer.Customerengname;
            this.tbxErpName.Text = dictCustomer.Erpname;
            this.tbxFastCode.Text = dictCustomer.Fastcode;
            this.tbxFax.Text = dictCustomer.Fax;
            this.tbxPostCode.Text = dictCustomer.Postcode;
            this.tbxReporTitle.Text = dictCustomer.Reporttitle;
            this.tbxTelePhone.Text = dictCustomer.Telephone;
            this.tbxCoustomerName2.Text = dictCustomer.Customername2;
            this.TxaRemark.Text = dictCustomer.Remark;
            this.dropDictcheckBillId.SelectedIndex = 0;
            this.dropDictLab.SelectedIndex = 0;
            this.dropDictsalemanId.SelectedIndex = 0;
            this.dropStatus.SelectedIndex = 0;
            this.chkIssms.Checked = true;

            this.radIsPub.SelectedValue = "0";
        }

        /// <summary>//保存数据的逻辑
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveDictlibrary()
        {
            if (radlActive.SelectedValue == "1")
            {
                erreyType = "该单位信息已审核，无法编辑单位信息，请取消审核再修改单位信息";
                return false;
            }
            if (tbxCoustomerCode.Text.Trim() == "" || tbxCoustomerName.Text.Trim() == "") //单位代码            
            {
                erreyType = "单位代码或单位名称不能为空！";
                return false;
            }
            dictCustomer.Active = this.radlActive.SelectedValue;  
            if (tbxAddress.Text.Trim() != "")
            {
                dictCustomer.Address = this.tbxAddress.Text.Trim();   //公司地址
            }
            else
            {
                erreyType = "公司地址不能为空！";
                return false;
            }
            if (tbxContactman.Text.Trim() != "")
            {
                dictCustomer.Contactman = this.tbxContactman.Text.Trim();  //联系人
            }
            else
            {
                erreyType = "联系人不能为空！";
                return false;
            }
            dictCustomer.Contactphone = this.tbxContactPhone.Text.Trim(); //联系人电话
            dictCustomer.Customerengname = this.tbxEnName.Text.Trim();    //公司英文名称
            dictCustomer.Customertype = this.radlCustomerType.SelectedValue;  //公司类型
            if (this.dropDictcheckBillId.SelectedValue != "-1")            //财务人员
            {
                dictCustomer.Dictcheckbillid = Convert.ToDouble(this.dropDictcheckBillId.SelectedValue);
            }
            else
            {
                erreyType = "财务人员不能为空！";
                return false;
            }
            if (this.dropDictLab.SelectedValue != "-1")   //所属分点
            {
                dictCustomer.Dictlabid = Convert.ToDouble(this.dropDictLab.SelectedValue);
            }
            else
            {
                erreyType = "所属分点不能为空！";
                return false;
            }
            if (this.dropDictsalemanId.SelectedValue != "-1") //销售人员
            {
                dictCustomer.Dictsalemanid = Convert.ToDouble(this.dropDictsalemanId.SelectedValue);
            }
            else
            {
                erreyType = "销售人员不能为空！";
                return false;
            }
            if (this.dropStatus.SelectedValue != "" && this.dropStatus.SelectedValue != null)
            {
                dictCustomer.Status = this.dropStatus.SelectedValue;   //客户状态
            }
            else
            {
                erreyType = "客户状态不能为空！";
                return false;
            }
            dictCustomer.Dictcustomerid = 0;
            if (gvList.SelectedRowIndexArray.Count() > 0)
            {
                dictCustomer.Dictcustomerid = Convert.ToDouble(gvList.DataKeys[gvList.SelectedRowIndexArray[0]][0]);
                dictCustomer.YGSyncStatus = "2";
                dictCustomer.DZSyncStatus = "2";
            }
            else
            {
                dictCustomer.Dictcustomerid = 0;
                dictCustomer.YGSyncStatus = "0";
                dictCustomer.DZSyncStatus = "0";
            }

            #region 单位名称或单位代码唯一性检验
            Hashtable ht = new Hashtable();
            ht.Add("value", tbxCoustomerCode.Text.Trim());
            ht.Add("Customername", tbxCoustomerName.Text.Trim());
            ht.Add("Dictcustomerid", dictCustomer.Dictcustomerid);
            List<Dictcustomer> listDictcustomer = new DictCustomerService().GetDictCustomerByCode(ht);
            if (listDictcustomer.Count > 0)
            {
                erreyType = "已存在相同的单位代码或单位名称,请重新填写！";
                return false;
            }   
            #endregion

            dictCustomer.Customername = this.tbxCoustomerName.Text.Trim();
            dictCustomer.Customercode = this.tbxCoustomerCode.Text.Trim();
            dictCustomer.Displayorder = Convert.ToInt32(this.tbxDisplayOrder.Text.Trim() == "" ? 0 : Convert.ToInt32(this.tbxDisplayOrder.Text.Trim()));   //排序 Convert.ToInt32(this.tbxDISPLAYORDER.Text.Trim() == "" ? 0 : Convert.ToInt32(this.tbxDISPLAYORDER.Text.Trim()));
            dictCustomer.Documentcode = this.tbxDocumentCode.Text.Trim();  //证件代号
            dictCustomer.Documenttype = this.tbxDocumentType.Text.Trim();  //证件类型
            dictCustomer.Engaddress = this.tbxEnAddress.Text.Trim();   //英文地址
            dictCustomer.Erpcode = this.tbxEnErpCode.Text.Trim();     //ERP代号
            dictCustomer.Erpname = this.tbxErpName.Text.Trim();        //ERP客户名称
            dictCustomer.Fastcode = this.tbxFastCode.Text.Trim();    //助记符
            dictCustomer.Fax = this.tbxFax.Text.Trim();               //传真
            dictCustomer.Postcode = this.tbxPostCode.Text.Trim();     //邮编
            dictCustomer.Customername2 = this.tbxCoustomerName2.Text.Trim();
            dictCustomer.Remark = this.TxaRemark.Text.Trim();        //备注
            dictCustomer.Reporttitle = this.tbxReporTitle.Text.Trim(); //报告单抬头
            if (tbxTelePhone.Text.Trim() != "")
            {
                dictCustomer.Telephone = this.tbxTelePhone.Text.Trim();     //电话
            }
            else
            {
                erreyType = "电话不能为空！";
                return false;
            }
            dictCustomer.Lastupdatedate = DateTime.Now;
            dictCustomer.Issms = chkIssms.Checked ? "1" : "0";

            dictCustomer.IsPublic = radIsPub.SelectedValue;//是否公用单位
            return dictCustomerService.SaveDictcustomer(dictCustomer);
        }

        /// <summary>
        /// 弹出价格编辑框
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="title"></param>
        void ShowWindow(string URL, string title)
        {
            PageContext.RegisterStartupScript(WinLibraryEdit.GetShowReference(URL, title));
        }
    }
}