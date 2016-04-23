using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daan.service.login;
using daan.domain;
using System.Collections;
using daan.service.dict;
using daan.service.order;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Web;
using daan.util.Common;

namespace daan.ui.exportExcel
{
    public partial class FrmExport : Form
    {
        static readonly LoginService loginservice = new LoginService();
        static readonly OrdersService os = new OrdersService();
        DataTable dtExport = null;
        public FrmExport()
        {
            InitializeComponent();
            BindLab();
            dateStart.Text = DateTime.Now.AddDays(-7).ToString();
            gridStat.AutoGenerateColumns = false;
        }

        #region 初始化下拉框
        private void BindLab()
        {
            Dictlab lab = new Dictlab();
            lab.Labname = "全部";
            lab.Dictlabid = -1;
            List<Dictlab> lablist = loginservice.GetLoginDictlab();
            lablist.Insert(0, lab);
            comLab.DataSource = lablist;
            comLab.DisplayMember = "Labname";
            comLab.ValueMember = "Dictlabid";
        }

        private void BindCustomer(string labid)
        {
            List<Dictcustomer> dictcustomerback = new List<Dictcustomer>();
            if (labid == "0")
            {
                List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();
                List<Dictlab> dictList = loginservice.GetLoginDictlab();
                foreach (Dictlab dict in dictList)
                {
                    List<Dictcustomer> dictcustomerfirt = CustomerList.FindAll(c => (c.Dictlabid == dict.Dictlabid && c.Customertype == "0" && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));
                    foreach (Dictcustomer dictcust in dictcustomerfirt)
                    {
                        if (!dictcustomerback.Contains(dictcust))
                            dictcustomerback.Add(dictcust);
                    }
                }
            }
            else
            {
                dictcustomerback = loginservice.GetDictcustomer().FindAll(c => (c.Dictlabid == double.Parse(labid) && c.Customertype == "0" && c.Active == "1") || (c.IsPublic == "1" && c.Active == "1"));                
            }
            Dictcustomer cus = new Dictcustomer();
            cus.Customername = "全部";
            cus.Dictcustomerid = -1;
            dictcustomerback.Insert(0, cus);
            comCustomer.DataSource = dictcustomerback;
            comCustomer.DisplayMember = "Customername";
            comCustomer.ValueMember = "Dictcustomerid";
        }

        private void comLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comLab.SelectedItem == null) return;
            string labid = comLab.SelectedIndex == 0 ? "0" : comLab.SelectedValue.ToString();
            BindCustomer(labid);
        }
        #endregion

        #region 按钮事件
        //查询
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetButtonEnable(false);
            BindDate();
            SetButtonEnable(true);
        }
        //导出
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (gridStat.RowCount-1 == 0||dtExport == null || dtExport.Rows.Count == 0)
            {
                messageshow("无待导出数据!");
                return;
            }
            try
            {
                SetButtonEnable(false);
                #region 导出excel列头定义
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("ordercode", "订单编码");
                sortlist.Add("labname", "分点");
                sortlist.Add("customername", "单位");
                sortlist.Add("Section", "区域");
                sortlist.Add("ordertestlst", "套餐");
                sortlist.Add("createdate", "系统录入日期");
                sortlist.Add("samplingdate", "采样日期");
                sortlist.Add("ordercount", "订单数量");
                sortlist.Add("importCount", "单位批量上传");
                sortlist.Add("importTime", "最后上传时间");
                sortlist.Add("resultCount", "检查结果录入");
                sortlist.Add("resultTime", "最后录入时间");
                sortlist.Add("finishedCount", "总检");
                sortlist.Add("finishedTime", "最后总检时间");
                sortlist.Add("printCount", "报告单集中打印");
                sortlist.Add("printTime", "最后打印时间");
                #endregion
                ExportDataTableToExcel(dtExport, sortlist);
            }
            catch (Exception ee)
            {
                messageshow("导出数据出错："+ee.Message);
            }
        }
        //退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("是否确定退出系统?", "退出系统", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                Application.Exit();
            }
        }
        #endregion

        #region private method
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <returns></returns>
        private Hashtable getPara()
        {
            Hashtable ht = new Hashtable();
            ht.Add("DateStart", dateStart.Text == "" ? null : Convert.ToDateTime(dateStart.Text).ToString("yyyy-MM-dd"));
            ht.Add("DateEnd", dateEnd.Text == "" ? null : Convert.ToDateTime(dateEnd.Text).AddDays(1).ToString("yyyy-MM-dd"));
            if (comLab.SelectedIndex == 0)
            {
                List<Dictlab> list = new DictlabService().GetDictlabList();
                StringBuilder joinlabid = new StringBuilder ();
                foreach (Dictlab lab in list)
                {
                    joinlabid.Append(lab.Dictlabid+",");
                }
                ht.Add("labid", joinlabid.ToString().TrimEnd(','));
            }
            else
            {
                ht.Add("labid", comLab.SelectedValue);
            }
            if (comCustomer.SelectedIndex == 0)
            {
                ht.Add("dictcustomerid", "-1");
            }
            else
            {
                ht.Add("dictcustomerid", comCustomer.SelectedValue);
            }
            ht.Add("pageStart", 1);
            ht.Add("pageEnd", 20);
            ht.Add("Section", txtSection.Text.Trim());
            return ht;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        private void BindDate()
        {
            try
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("预加载待导出数据，此功能可能需要加载很长时间，是否确定开始加载?", "体检系统", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    Hashtable ht = getPara();
                    DataTable dtList = os.QueryTMStatExportList(ht);//分组统计数据列表
                    DataTable dtOperaLog = os.QueryOperationLog(ht);//操作日志记录
                    dtExport = dtOperaLog == null ? dtList : FillDataList(dtList, dtOperaLog);
                    gridStat.DataSource = dtExport;
                }
                else
                {
                    SetButtonEnable(true);
                }
            }
            catch (Exception e)
            {
                messageshow("查询数据出错：" + e.Message);
            }
        }
        /// <summary
        /// 计算各状态数量
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="dtOperaLog"></param>
        /// <returns></returns>
        private DataTable FillDataList(DataTable dtList, DataTable dtOperaLog)
        {
            string conditions;
            Hashtable htImport, htResult, htFinished, htPrint;
            #region 分类操作记录数据
            DataTable dtImport = OperaDataFilter(string.Format("modulename='{0}'", "单位批量上传"), dtOperaLog);
            DataTable dtResult = OperaDataFilter(string.Format("modulename='{0}'", "检查结果录入"), dtOperaLog);
            DataTable dtFinished = OperaDataFilter(string.Format("modulename='{0}' and content like '%{1}%'", "总检", "完成总检审核成功"), dtOperaLog);
            DataTable dtPrint = OperaDataFilter(string.Format("modulename='{0}'", "报告单集中打印"), dtOperaLog);
            #endregion
            try
            {
                foreach (DataRow dr in dtList.Rows)
                {
                    conditions = String.Format("ordercode='{0}' and dictlabid={1} and dictcustomerid={2} and section='{3}' and ordertestlst='{4}' and createdate='{5}' and (samplingdate='{6}' or samplingdate is null)",
                        dr["ordercode"].ToString(), dr["dictlabid"].ToString(), dr["dictcustomerid"].ToString(), dr["Section"].ToString(),
                        dr["ordertestlst"].ToString(), dr["createdate"].ToString(), dr["samplingdate"].ToString());
                    #region 分别获取各节点数和最后操作时间
                    htImport = GetCountandTime(conditions, dtImport);
                    dr["importCount"] = htImport["count"];
                    dr["importTime"] = htImport["lasttime"];

                    htResult = GetCountandTime(conditions, dtResult);
                    dr["resultCount"] = htResult["count"];
                    dr["resultTime"] = htResult["lasttime"];

                    htFinished = GetCountandTime(conditions, dtFinished);
                    dr["finishedCount"] = htFinished["count"];
                    dr["finishedTime"] = htFinished["lasttime"];

                    htPrint = GetCountandTime(conditions, dtPrint);
                    dr["printCount"] = htPrint["count"];
                    dr["printTime"] = htPrint["lasttime"];
                    #endregion
                }
            }
            catch (Exception ee)
            {
                messageshow("加载数据出错，异常信息：" + ee.Message);
            }
            return dtList;
        }

        private static DataTable OperaDataFilter(string conditions, DataTable dtsource)
        {
            DataTable dtreturn;
            DataRow[] dr = dtsource.Select(conditions);
            if (dr.Count() > 0)
                dtreturn = dr.CopyToDataTable();
            else
                dtreturn = null;
            return dtreturn;
        }
        private static Hashtable GetCountandTime(string conditions, DataTable dt)
        {
            Hashtable ht = new Hashtable();
            if (dt == null)
            {
                ht["count"] = 0; ht["lasttime"] = "";
                return ht;
            }
            DataRow[] dr = dt.Select(conditions, "logdate desc");
            if (dr.Count() > 0)
            {
                ht["count"] = dr.Count();
                ht["lasttime"] = dr[0]["logdate"].ToString();
            }
            else
            {
                ht["count"] = 0; ht["lasttime"] = "";
            }
            return ht;
        }
        /// <summary>
        /// 弹出消息框
        /// </summary>
        /// <param name="mes"></param>
        private void messageshow(string mes)
        {
            MessageBox.Show(mes);
        }
        /// <summary>
        /// 设置按钮是否可用
        /// </summary>
        /// <param name="f"></param>
        private void SetButtonEnable(bool f)
        {
            btnExport.Enabled = f;
            btnSearch.Enabled = f;
        }
        #region 导出数据到excel
        /// <summary>
        /// 导出数据到EXCEL
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="sortlist"></param>
        private void ExportDataTableToExcel(DataTable dtSource, SortedList sortlist)
        {
            GetDatasourceAsStream(dtSource, DateTime.Now.ToString("yyyy-MM-dd"), sortlist);
        }

        private void GetDatasourceAsStream(DataTable sourceTable, string sheetName, SortedList sortlist)
        {
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                ISheet sheet = workbook.CreateSheet(sheetName);
                int rowIndex = 0;
                IRow headerRow = sheet.CreateRow(rowIndex);
                #region 数据
                bool flag = false;
                //设置异常数据样式
                ICellStyle style = workbook.CreateCellStyle();
                IFont font = workbook.CreateFont();
                font.Color = HSSFColor.RED.index;
                font.Boldweight = short.MaxValue;
                font.IsItalic = true;
                style.SetFont(font);

                foreach (DataRow row in sourceTable.Rows)
                {
                    rowIndex++;
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    int i = 0;
                    int ordercount = Convert.ToInt32(row["ordercount"]);//订单总量

                    foreach (DictionaryEntry dic in sortlist)
                    {
                        foreach (DataColumn column in sourceTable.Columns)
                        {
                            if (dic.Key.ToString().ToUpper() == column.ColumnName.ToUpper())
                            {
                                if (!flag)
                                    headerRow.CreateCell(i).SetCellValue(dic.Value.ToString());
                                ICell cell = dataRow.CreateCell(i);
                                SetCellType(cell, row, column);
                                #region 设置样式
                                int count = 0;
                                bool f = false;
                                switch (column.ColumnName.ToLower())
                                {
                                    case "resultcount"://检查结果录入
                                        count = Convert.ToInt32(row["resultCount"]);
                                        f = true;
                                        break;
                                    case "finishedcount"://完成总检
                                        count = Convert.ToInt32(row["finishedCount"]);
                                        f = true;
                                        break;
                                    case "printcount"://报告打印
                                        count = Convert.ToInt32(row["printCount"]);
                                        f = true;
                                        break;
                                    default:
                                        break;
                                }
                                if (ordercount > count && f)
                                {
                                    cell.CellStyle = style;
                                }
                                #endregion
                            }
                        }
                        i++;
                    }
                    flag = true;
                }
                #endregion
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "导出Excel(*.xls)|*.xls";
                sfd.FileName = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        workbook.Write(fs);
                        fs.Flush();
                        fs.Close();
                        messageshow("导出成功！");
                        SetButtonEnable(true);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                messageshow("生成EXCEL文件时产生错误：" + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }
        /// <summary>
        /// 设置导出EXCEL单元格格式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private static void SetCellType(ICell cell, DataRow row, DataColumn column)
        {
            switch (column.DataType.ToString())
            {
                case "System.String":
                    cell.SetCellValue(row[column].ToString() ?? "");
                    break;
                case "System.DateTime":
                    DateTime dateV;
                    DateTime.TryParse(row[column].ToString(), out dateV);
                    cell.SetCellValue(dateV);
                    break;
                case "System.Boolean":
                    bool boolV = false;
                    bool.TryParse(row[column].ToString(), out boolV);
                    cell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型   
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(row[column].ToString(), out intV);
                    cell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型   
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(row[column].ToString(), out doubV);
                    cell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理   
                    cell.SetCellValue("");
                    break;
                default:
                    cell.SetCellValue("");
                    break;
            }
        }
        #endregion
        #endregion

        private void FrmExport_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
