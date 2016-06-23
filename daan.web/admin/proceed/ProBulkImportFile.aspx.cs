using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.login;
using daan.service.dict;
using daan.service.proceed;
using daan.domain;
using System.IO;
using System.Data;
using System.Collections;
using daan.util.Common;
using daan.service.order;
using ExtAspNet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace daan.web.admin.proceed
{
    public partial class ProBulkImportFile : PageBase
    {
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();
        static ProRegisterService registerservice = new ProRegisterService();
        static OrderbarcodeService barcodeservice = new OrderbarcodeService();
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + DropCustomer.ClientID + "')).listWidth=250;");
            if (Request.Form["__EVENTTARGET"] == tbxmember.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey") { SelectCustomer(); }
            if (!IsPostBack)
            {
                BindDictLab();
                BindAddress();
            }
        }

        #region >>>>体检单位模糊查询
        private void SelectCustomer()
        {
            string cusName = tbxmember.Text.Trim();
            if (string.IsNullOrEmpty(cusName))
            {
                //体检单位初始化
                BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
            }
            else
            {
                string labid = string.Empty;
                if (DropDictLab.SelectedValue == "-1")
                {
                    labid = Userinfo.joinLabidstr;
                }
                else
                {
                    labid = DropDictLab.SelectedValue;
                }
                Hashtable htPara = new Hashtable();
                htPara.Add("labid", labid);
                htPara.Add("customername", cusName);

                DataTable dtList = new DictCustomerService().GetCustomerListBySearchBox(htPara);
                if (dtList == null || dtList.Rows.Count == 0)
                {
                    MessageBoxShow("没有搜索到匹配的体检单位！");
                    tbxmember.Text = string.Empty;
                    tbxmember.Focus();
                    return;
                }
                else if (dtList.Rows.Count == 1)
                {
                    DropCustomer.SelectedValue = dtList.Rows[0]["dictcustomerid"].ToString();
                    tbxmember.Text = string.Empty;
                }
                else
                {
                    DropCustomer.DataSource = dtList;
                    DropCustomer.DataValueField = "Dictcustomerid";
                    DropCustomer.DataTextField = "Customername";
                    DropCustomer.DataBind();
                    tbxmember.Text = string.Empty;
                }
            }
        }
        #endregion

        #region >>>省市区
        private void BindAddress()
        {
            DropProvinceBinder(dpProvince);
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        /// <summary>
        /// 选择省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpCity.Items.Clear();
            DropCityBinder(dpProvince, dpCity);

            dpCounty.Items.Clear();
            DropCountyBinder(dpCity, dpCounty);

            if (dpProvince.SelectedValue != "-1")
            {
                hidProvince.Text = dpProvince.SelectedText;
                hidCity.Text = string.Empty;
                hidCounty.Text = string.Empty;
            }
            if (ck1.Checked)
                setAddress();
        }
        /// <summary>
        /// 选择市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpCounty.Items.Clear();
            DropCountyBinder(dpCity, dpCounty);
            if (dpCity.SelectedValue != "-1")
            {
                hidCity.Text = dpCity.SelectedText;
                hidCounty.Text = string.Empty;
            }
            if (ck1.Checked)
                setAddress();
        }

        protected void dpCounty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpCounty.SelectedValue != "-1")
                hidCounty.Text = dpCounty.SelectedText;
            if (ck1.Checked)
                setAddress();
        }
        #endregion

        #region >>>>绑定分点 以及选分点筛选单位
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {
            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();

            DropCustomer.DataSource = CustomerList.Where<Dictcustomer>(c => (c.Dictlabid == labid && c.Customertype == "0" && c.Active == "1")||c.IsPublic=="1");
            DropCustomer.DataValueField = "Dictcustomerid";
            DropCustomer.DataTextField = "Customername";
            DropCustomer.DataBind();
            DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("请选择体检单位", "-1"));
        }

        /// <summary>选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }
        #endregion

        protected void ck1_CheckedChanged(object sender, EventArgs e)
        {
            if (ck1.Checked)
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = true;
                setAddress();
            }
            else
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = false;
                txtAddress.Text = txtRecName.Text = txtTelphone.Text = string.Empty;
            }
        }

        //关闭
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        //导入
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (DropDictLab.SelectedValue == null)
            {
                MessageBoxShow("请先选择分点！"); return;
            }

            if (DropCustomer.SelectedIndex == 0)
            {
                MessageBoxShow("请先选择单位！"); return;
            }

            if (dpProvince.SelectedValue == "-1" || dpCity.SelectedValue == "-1")
            {
                MessageBoxShow("请选择省、市"); return;
            }

            string fileType = fileExcel.PostedFile.ContentType;
            if (fileType != "application/octet-stream" && fileType != "application/vnd.ms-excel" && fileType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && fileType != "application/kset")
            {
                MessageBoxShow("请上传指定格式的Excel文件"); return;
            }
            if (ck1.Checked)
            {
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()) || string.IsNullOrEmpty(txtRecName.Text.Trim()) || string.IsNullOrEmpty(txtTelphone.Text.Trim()))
                {
                    MessageBoxShow("邮寄地址、收件人、联系电话不能为空"); return;
                }
            }
            string fileName = fileExcel.ShortFileName;
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = String.Format("{0}_{1}", DateTime.Now.Ticks, fileName);

            string savePath = Server.MapPath("~/upload/ExcelFiles/" + fileName);

            //上传文件
            if (fileExcel.HasFile)
            {
                if (!Directory.Exists(Server.MapPath("~/upload/ExcelFiles")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/upload/ExcelFiles"));
                }
                try { fileExcel.SaveAs(savePath); }
                catch (Exception ex) { MessageBoxShow("上传错误：" + ex.Message); return; }
            }
            DataTable dt;
            try
            {
                dt = RenderDataTableFromExcel(savePath);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBoxShow("上传的文件数据不能为空！"); return;
                }
                string str=CheckDate(dt,fileName);
                if (!string.IsNullOrEmpty(str))
                {
                    MessageBoxShow(str);
                    File.Delete(savePath);//删除文件
                    return;
                }
            }
            catch (Exception ex) { MessageBoxShow("请上传指定格式的Excel文件!\r\n提示信息:" + ex.Message); return; }
            Orderfileheader oh = new Orderfileheader();
            oh.Dictlabid = Convert.ToDouble(DropDictLab.SelectedValue);
            oh.Dictcustormer = Convert.ToDouble(DropCustomer.SelectedValue);
            oh.Enterby = Userinfo.userId;
            oh.Createdate = DateTime.Now;
            oh.Filename = savePath;
            oh.Status = 0;
            oh.Province = dpProvince.SelectedValue == "-1" ? "" : dpProvince.SelectedText;
            oh.City = dpCity.SelectedValue == "-1" ? "" : dpCity.SelectedText;
            oh.County = dpCounty.SelectedValue == "-1" ? "" : dpCounty.SelectedText;
            oh.IsUnifiedpost = ck1.Checked == true ? "1" : "0";
            oh.PostAddress = Regex.Replace(txtAddress.Text, @"\s", "");
            oh.Recipient = Regex.Replace(txtRecName.Text, @"\s", "");
            oh.ContactNumber = Regex.Replace(txtTelphone.Text, @"\s", ""); 
            OrderfileheaderService ohs = new OrderfileheaderService();
            try
            {
                ohs.InsertOrderfileheader(oh);
                MessageBoxShow("导入成功！请等待10s后，再查询数据");
            }
            catch (Exception ex)
            {
                MessageBoxShow("导入失败！请重新导入 ：" + ex.Message);
            }
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="path">服务器excel文档路径</param>
        /// <returns></returns>
        public DataTable RenderDataTableFromExcel(string path)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                if (headerRow.GetCell(j) == null)
                {
                    continue;
                }
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            dt.Columns.Add("上传状态", typeof(string));
            dt.Columns.Add("失败原因", typeof(string));
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                //套餐代码为空
                if (row.GetCell(5) == null)
                {
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(row.GetCell(5).ToString()))
                    {
                        continue;
                    }
                }
                //姓名为空
                if (row.GetCell(8) == null)
                {
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(row.GetCell(8).ToString()))
                    {
                        continue;
                    }
                }
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < (cellCount + 2); j++)
                {
                    if (j == cellCount)
                    {
                        dataRow[j] = "未上传";
                    }
                    else
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        #region 检测导入EXCEL文件列头是否标准
        string[] arr = CommonConst.ImportExcelCols;
        private string CheckDate(DataTable dt,string fileName)
        {
            string res = string.Empty;
            foreach (string str in arr)
            {
                if (!dt.Columns.Contains(str))
                {
                    res = SetErrorMessage(str, fileName);
                    break;
                }
            }
            return res;
        }
        private string SetErrorMessage(string colName,string fileName)
        {
            string errstring = string.Format("文件名:{0} 缺少列[{1}]。【Excel格式请严格参照导入模版说明】\n\n导入过程中如有疑问，请将异常信息截图后发与系统管理员。", fileName.Substring(fileName.LastIndexOf('_') + 1), colName);
            return errstring;
        }

        private void setAddress()
        {
            txtAddress.Text = string.Format("{0} {1} {2}", hidProvince.Text, hidCity.Text, hidCounty.Text);
        }
        #endregion
    }
}