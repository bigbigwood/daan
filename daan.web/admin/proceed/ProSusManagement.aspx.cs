using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.proceed;
using daan.service.login;
using daan.service.dict;
using daan.domain;
using System.Collections;
using daan.util.Web;
using daan.web.code;
using daan.util.Common;
using ExtAspNet;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace daan.web.admin.proceed
{
    public partial class ProSusManagement : PageBase
    {
        static ProCentralizedManagementService mamagement = new ProCentralizedManagementService();
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDictLab();
                BindDropStatus();
                dateend.Text = DateTime.Today.ToString("yyyy-MM-dd");
                datebegin.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            }
        }

        #region >>>>1、 初始化下拉控件数据 分点、单位、状态
        //绑定状态
        private void BindDropStatus()
        {
            DDLInitbasicBinder(dropStatus, "ORDERSTATUS");
            dropStatus.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
        }

        // 绑定分点        
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            DropDictLab.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }
        // 绑定单位        
        private void BindCustomer(double labid)
        {

            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
            if (labid == -1)
            {
                List<Dictlab> dictList = loginservice.GetPermissionDictlab(Userinfo);
                List<Dictcustomer> dictcustomerback = new List<Dictcustomer>();
                foreach (Dictlab dict in dictList)
                {
                    List<Dictcustomer> dictcustomerfirt = CustomerList.FindAll(c => c.Dictlabid == dict.Dictlabid && c.Customertype == "0" && c.Active == "1");
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        dictcustomerback.Add(dictcust);
                    }
                }
                DropCustomer.DataSource = dictcustomerback;
                DropCustomer.DataValueField = "Dictcustomerid";
                DropCustomer.DataTextField = "Customername";
                DropCustomer.DataBind();
                DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            }
            else
            {
                DropCustomer.DataSource = CustomerList.Where<Dictcustomer>(c => c.Dictlabid == labid && c.Customertype == "0" && c.Active == "1");
                DropCustomer.DataValueField = "Dictcustomerid";
                DropCustomer.DataTextField = "Customername";
                DropCustomer.DataBind();
                DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("全部", "-1"));
            }
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }
        #endregion

        #region >>>>2、按钮事件 分页
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        //分页
        protected void GridOrders_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            GridOrders.PageIndex = e.NewPageIndex;
            BindData();
        }
        //导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable ht = new Hashtable();
                PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);
                ht["DateStart"] = datebegin.Text;
                ht["DateEnd"] = Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd");
                if (DropDictLab.SelectedValue == "0")
                    ht["labid"] = Userinfo.joinLabidstr;
                else
                    ht["labid"] = DropDictLab.SelectedValue;
                ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
                ht["ordernum"] = tbxOrderNum.Text;
                ht["status"] = dropStatus.SelectedIndex == 0 ? null : dropStatus.SelectedValue;
                ht["customerid"] = DropCustomer.SelectedValue == "-1" ? null : DropCustomer.SelectedValue;
                ht["iscancel"] = DropIScancel.SelectedIndex == 0 ? null : DropIScancel.SelectedValue;
                ht["pageStart"] = pageUtil.GetPageStartNum();
                ht["pageEnd"] = pageUtil.GetPageEndNum();

                DataTable source = mamagement.GetSusManagementOrdersList(ht);
                if (source.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }
                string sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                string filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());
                //sortlist.Add("Provincename", "省份");
                //sortlist.Add("Cityname", "城市");
                //sortlist.Add("Countyname", "地区");
                sortlist.Add("Barcode", "条码号");
                sortlist.Add("Realname", "姓名");
                sortlist.Add("Sex", "性别");
                sortlist.Add("Birthday", "出生年月");
                sortlist.Add("Mobile", "手机号码");
                sortlist.Add("Idnumber", "身份证号");
                sortlist.Add("Testcode", "项目1编号");
                sortlist.Add("Customername", "客户（门店名称)");
                sortlist.Add("Addres", "客户(门店)详细地址");
                ExportDataTableToExcel(source, filename, sheetname, sortlist);
            }
            catch (Exception)
            {
                MessageBoxShow("导出数据出错，请联系管理员！");
            }
        }
        #endregion

        #region >>>>3、私有方法
        /// <summary>
        /// 查询结果绑定
        /// </summary>
        private void BindData()
        {
            Hashtable ht = new Hashtable();
            if (string.IsNullOrWhiteSpace(datebegin.Text) ||string.IsNullOrWhiteSpace(dateend.Text))
            {
                MessageBoxShow("请输入开始时间及结束时间查询！", MessageBoxIcon.Information);
                return;
            }            
            PageUtil pageUtil = new PageUtil(GridOrders.PageIndex, GridOrders.PageSize);            
            ht["DateStart"] = datebegin.Text;
            ht["DateEnd"] = Convert.ToDateTime(dateend.Text).AddDays(1).ToString("yyyy-MM-dd");
            if (DropDictLab.SelectedValue == "0")
                ht["labid"] = Userinfo.joinLabidstr;
            else
                ht["labid"] = DropDictLab.SelectedValue;
            ht["realname"] = tbxName.Text = TextUtility.ReplaceText(tbxName.Text);
            ht["ordernum"] = tbxOrderNum.Text;
            ht["status"] = dropStatus.SelectedIndex == 0 ? null : dropStatus.SelectedValue;
            ht["customerid"] = DropCustomer.SelectedValue == "-1" ? null : DropCustomer.SelectedValue;
            ht["iscancel"] = DropIScancel.SelectedIndex == 0 ? null : DropIScancel.SelectedValue;
            ht["pageStart"] = pageUtil.GetPageStartNum();
            ht["pageEnd"] = pageUtil.GetPageEndNum();

            GridOrders.RecordCount = mamagement.GetSusmanagemenetOrdersCount(ht);
            GridOrders.DataSource = mamagement.GetSusManagementOrdersList(ht);
            GridOrders.DataBind();
        }
        /// <summary>
        /// 导出Excel DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">工作簿名称</param>
        private void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName, SortedList sortlist)
        {
            MemoryStream ms = null;
            ms = GetDatasourceAsStream(sourceTable, sheetName, sortlist) as MemoryStream;
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");

            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }
        /// <summary>
        ///  NPOI 导出Excel DataTable
        /// </summary>
        /// <param name="sourceTable">导出数据源</param>
        /// <param name="sheetName">工作簿名称</param>
        /// <param name="sortlist">列头集合</param>
        /// <returns></returns>
        private Stream GetDatasourceAsStream(DataTable sourceTable, string sheetName, SortedList sortlist)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            int rowIndex = 0;
            #region 静态数据行
            IRow staticRow = sheet.CreateRow(rowIndex);
            staticRow.CreateCell(0).SetCellValue("机构名称");
            staticRow.CreateCell(1).SetCellValue("达安特殊客户");
            staticRow.CreateCell(2).SetCellValue("机构代码");
            staticRow.CreateCell(3).SetCellValue("471");
            rowIndex++;
            #endregion

            IRow headerRow = sheet.CreateRow(rowIndex);
            rowIndex++;

            #region 数据
            bool flag = false;
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DictionaryEntry dic in sortlist)
                {
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        if (!flag)
                        {
                            //添加HeaderRow
                            if (dic.Key.ToString().ToUpper() == column.ColumnName.ToUpper())
                            {
                                headerRow.CreateCell(i).SetCellValue(dic.Value.ToString());
                                dataRow.CreateCell(i).SetCellValue(row[column].ToString() == null ? "" : row[column].ToString());
                            }
                        }
                        else
                        {
                            if (dic.Key.ToString().ToUpper() == column.ColumnName.ToUpper())
                            {
                                dataRow.CreateCell(i).SetCellValue(row[column].ToString() == null ? "" : row[column].ToString());
                            }
                        }
                    }
                    i++;
                }
                flag = true;
                rowIndex++;
            }
            #endregion

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            staticRow = null;
            headerRow = null;
            sheet = null;
            workbook = null;
            return ms;
        }
        #endregion
    }
}