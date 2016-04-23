using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using daan.util.Web;
using daan.util.Common;
using daan.web.code;
using ExtAspNet;
using daan.service.login;
using daan.service.dict;
using daan.domain;
using System.Data;

namespace daan.web.admin.dict
{
    public partial class DictCustomerInfoExport : PageBase
    {
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            BindCustomer();
        }
        #region >>>>1、 下拉列表数据绑定 单位
        private void BindCustomer()
        {
            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
            DropCustomer.DataSource = CustomerList.Where<Dictcustomer>(v => v.Dictsalemanid == Userinfo.userId && v.Active == "1" && v.Customertype == "0");
            DropCustomer.DataValueField = "Dictcustomerid";
            DropCustomer.DataTextField = "Customername";
            DropCustomer.DataBind();
            DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }
        #endregion

        #region >>>>2、 查询  分页 数据导出
        // 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        
        //分页
        protected void GridInfos_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridInfos.PageIndex = e.NewPageIndex;
            BindData();
        }

        //导出当前页数据
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable ht = new Hashtable();
                PageUtil pageUtil = new PageUtil(GridInfos.PageIndex, GridInfos.PageSize);
                ht["Dictcustomerid"] = DropCustomer.SelectedValue;
                ht["Realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
                ht["Idnumber"] = tbxIDNumber.Text = TextUtility.ReplaceText(tbxIDNumber.Text);
                ht["dictsalemanid"] = Userinfo.userId;//销售人员id
                ht["pageStart"] = pageUtil.GetPageStartNum();
                ht["pageEnd"] = pageUtil.GetPageEndNum();

                DataTable  source = memberservice.GetCustomerInfosExportList(ht);
                if (source.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }

                string sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                string filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("Realname", "姓名");
                sortlist.Add("Sex", "性别");
                sortlist.Add("Birthday", "出生日期");
                sortlist.Add("Mobile", "手机");
                sortlist.Add("Idnumber", "身份证");
                sortlist.Add("Addres", "住址");
                ExcelOperation<Dictmember>.ExportDataTableToExcel(source, filename, sheetname, sortlist);
            }
            catch (Exception)
            {
                MessageBoxShow("导出数据出错，请联系管理员！");
            }
        }

        #endregion

        //查询结果绑定
        private void BindData()
        {
            Hashtable ht = new Hashtable();
            PageUtil pageUtil = new PageUtil(GridInfos.PageIndex, GridInfos.PageSize);
            ht["Dictcustomerid"] = DropCustomer.SelectedValue;
            ht["Realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
            ht["Idnumber"] = tbxIDNumber.Text = TextUtility.ReplaceText(tbxIDNumber.Text);
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();
            ht["dictsalemanid"] = Userinfo.userId;//销售人员id

            Hashtable ht2 = new Hashtable();
            ht2["Dictcustomerid"] = DropCustomer.SelectedValue;
            ht2["Realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
            ht2["Idnumber"] = tbxIDNumber.Text = TextUtility.ReplaceText(tbxIDNumber.Text);
            ht2["dictsalemanid"] = Userinfo.userId;

            GridInfos.RecordCount = memberservice.GetCustomerInfosExportCount(ht2);//获取总记录数
            GridInfos.DataSource = memberservice.GetCustomerInfosExportList(ht);//查询当前页数据
            GridInfos.DataBind();
        }
    }
}