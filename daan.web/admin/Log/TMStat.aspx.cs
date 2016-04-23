using System;
using System.Collections.Generic;
using System.Linq;
using ExtAspNet;
using System.Collections;
using daan.util.Web;
using daan.service.order;
using System.Data;
using daan.util.Common;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using daan.service.dict;
using daan.domain;

namespace daan.web.admin.Log
{
    public partial class TMStat : PageBase
    {
        static readonly OrdersService os = new OrdersService();
        static readonly DictlabService dictlabService = new DictlabService();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 3600;
            //Session.Timeout = 3600;
            if (!IsPostBack)
            {
                BindDictLab();
                Dp_BeginDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                Dp_EndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

                BindSearchList();
            }
        }

        // 绑定分点        
        private void BindDictLab()
        {
            DDLDictLabBinder(dropDictLab, true);
            dropDictLab.Items.Insert(0, new ListItem("全部", "-1"));
            if (dropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(dropDictLab.SelectedValue));
            }
        }

        //选择分点，分点与单位联动
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToDouble(dropDictLab.SelectedValue));
        }
        // 绑定单位        
        private void BindCustomer(double labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), true);
        }
        protected void dropStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearchList();
            BindDate();
        }
        protected void dropSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选择生成状态为未生成则不用查询结果
            if (dropStatus.SelectedValue != "0")
            {
                BindDate();
            }
        }
        protected void ckSelf_CheckedChanged(object sender, EventArgs e)
        {
            BindSearchList();
            BindDate();
        }
        /// <summary>
        /// 绑定查询记录列表
        /// </summary>
        private void BindSearchList()
        {
            Hashtable ht = new Hashtable();
            ht.Add("status", dropStatus.SelectedValue);
            if (ckSelf.Checked)
            {
                ht.Add("userid", Userinfo.userId);
            }
            DataTable dt = os.SelectPreSearchList(ht);
            if (dt == null || dt.Rows.Count == 0)
            {
                dropSearch.Items.Clear();
            }
            else
            {
                List<Dictlab> lablist = dictlabService.GetDictlabList();
                List<SearchListClass> myList = new List<SearchListClass>();
                string dictlabid = string.Empty;
                string labname = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    dictlabid = dr["dictlabid"].ToString();
                    if (dictlabid.IndexOf(',') > 0)
                    {
                        labname = "全部分点";
                    }
                    else
                    {
                        labname = lablist.First<Dictlab>(c => c.Dictlabid == Convert.ToInt32(dictlabid)).Labname;
                    }
                    myList.Add(new SearchListClass(Convert.ToInt32(dr["searchid"]), string.Format("{0}[{1}][{2}][{3}][{4}][{5}][{6}]", dr["username"], dr["createtime"], labname, dr["customername"], dr["begindate"], dr["enddate"], dr["section2"])));
                }
                dropSearch.DataSource = myList;
                dropSearch.DataTextField = "Name";
                dropSearch.DataValueField = "ID";
                dropSearch.DataBind();
            }
            dropSearch.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        //绑定列表数据
        private void BindDate()
        {
            try
            {
                PageUtil pageUtil = new PageUtil(gridStat.PageIndex, gridStat.PageSize);
                Hashtable ht = new Hashtable();
                ht.Add("searchid", dropSearch.SelectedValue);
                ht.Add("pageStart", pageUtil.GetPageStartNum());
                ht.Add("pageEnd", pageUtil.GetPageEndNum());
                gridStat.RecordCount = os.SelectTMStatSearchResultListCount(ht);
                gridStat.DataSource = os.SelectTMStatSearchResultList(ht);
                gridStat.DataBind();
            }
            catch (Exception e)
            {
                MessageBoxShow("查询数据出错："+e.Message);
            }
        }
        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //BindDate();
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("begindate", Dp_BeginDate.Text == "" ? null : Dp_BeginDate.Text);
                ht.Add("enddate", Dp_EndDate.Text == "" ? null : Dp_EndDate.Text);
                if (dropDictLab.SelectedValue == "-1")
                {
                    ht.Add("dictlabid", Userinfo.joinLabidstr);
                }
                else
                {
                    ht.Add("dictlabid", dropDictLab.SelectedValue);
                }
                ht.Add("dictcustomerid", DropCustomer.SelectedValue);
                ht.Add("section", txtSection.Text.Trim());
                ht.Add("dictuserid", Userinfo.userId);
                ht.Add("createtime", DateTime.Now.ToString());

                if (os.InsertPreSearchInfo(ht))
                {
                    MessageBoxShow("统计查询已进入结果生成列队,查询结果生成成功后可在结果生成状态为已生成的列表中选择您的查询批次查看结果并导出数据！");
                    BindSearchList();//重新加载查询条件下拉框选项
                }
                else
                {
                    MessageBoxShow("生成统计预查询条件失败");
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow("生成查询条件出错。错误信息："+ex.Message);
            }
        }
        //导出
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gridStat.RecordCount == 0)
            {
                MessageBoxShow("导出没有数据！"); return;
            }
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("searchid", dropSearch.SelectedValue);
                DataTable dtExport = os.SelectTMStatSearchResultExportList(ht);
                if (dtExport == null || dtExport.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！"); return;
                }
                #region 导出字段定义
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
            catch (Exception ex)
            {
                MessageBoxShow("导出数据出错，异常信息：" + ex.Message);
            }
        }
        //分页
        protected void gridStat_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gridStat.PageIndex = e.NewPageIndex;
            BindDate();
        }
        /// <summary>
        /// 计算各状态数量
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="dtOperaLog"></param>
        /// <returns></returns>
        //private DataTable FillDataList(DataTable dtList, DataTable dtOperaLog)
        //{
        //    string conditions;
        //    Hashtable htImport, htResult,htFinished,htPrint;
        //    #region 分类操作记录数据
        //    DataTable dtImport = OperaDataFilter(string.Format("modulename='{0}'", "单位批量上传"), dtOperaLog);
        //    DataTable dtResult = OperaDataFilter(string.Format("modulename='{0}'", "检查结果录入"), dtOperaLog);
        //    DataTable dtFinished = OperaDataFilter(string.Format("modulename='{0}' and content like '%{1}%'", "总检", "完成总检审核成功"), dtOperaLog);
        //    DataTable dtPrint = OperaDataFilter(string.Format("modulename='{0}'", "报告单集中打印"), dtOperaLog);
        //    #endregion
        //    try
        //    {
        //        foreach (DataRow dr in dtList.Rows)
        //        {
        //            conditions = String.Format("ordercode='{0}' and dictlabid={1} and dictcustomerid={2} and section='{3}' and ordertestlst='{4}' and createdate='{5}' and (samplingdate='{6}' or samplingdate is null)",
        //                dr["ordercode"].ToString(), dr["dictlabid"].ToString(), dr["dictcustomerid"].ToString(), dr["Section"].ToString(),
        //                dr["ordertestlst"].ToString(), dr["createdate"].ToString(), dr["samplingdate"].ToString());
        //            #region 分别获取各节点数和最后操作时间
        //            htImport = GetCountandTime(conditions, dtImport);
        //            dr["importCount"] = htImport["count"]; 
        //            dr["importTime"] = htImport["lasttime"];

        //            htResult = GetCountandTime(conditions, dtResult);
        //            dr["resultCount"] =htResult["count"]; 
        //            dr["resultTime"] = htResult["lasttime"];

        //            htFinished = GetCountandTime(conditions, dtFinished);
        //            dr["finishedCount"] = htFinished["count"];
        //            dr["finishedTime"] = htFinished["lasttime"];

        //            htPrint = GetCountandTime(conditions,dtPrint);
        //            dr["printCount"] = htPrint["count"];
        //            dr["printTime"] = htPrint["lasttime"];
        //            #endregion
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBoxShow("加载数据出错，异常信息：" + ee.Message);
        //    }
        //    return dtList;
        //}

        //private static DataTable OperaDataFilter(string conditions,DataTable dtsource)
        //{
        //    DataTable dtreturn;
        //    DataRow[] dr = dtsource.Select(conditions);
        //    if (dr.Count() > 0)
        //        dtreturn = dr.CopyToDataTable();
        //    else
        //        dtreturn = null;
        //    return dtreturn;
        //}
        //private static Hashtable GetCountandTime(string conditions ,DataTable dt)
        //{
        //    Hashtable ht = new Hashtable();
        //    if(dt==null)
        //    {
        //        ht["count"]=0;ht["lasttime"]="";
        //        return ht;
        //    }
        //    DataRow[] dr = dt.Select(conditions, "logdate desc");
        //    if (dr.Count() > 0)
        //    {
        //        ht["count"] = dr.Count();
        //        ht["lasttime"] = dr[0]["logdate"].ToString();
        //    }
        //    else
        //    {
        //        ht["count"] = 0;ht["lasttime"] = "";
        //    }
        //    return ht;
        //}
        #region 导出数据实现
        /// <summary>
        /// 导出数据到EXCEL
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="sortlist"></param>
        private static void ExportDataTableToExcel(DataTable dtSource, SortedList sortlist)
        {
            MemoryStream ms = GetDatasourceAsStream(dtSource, DateTime.Now.ToString("yyyy-MM-dd"), sortlist) as MemoryStream;
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", DateTime.Now.ToString("yyyyMMdd_hhmmss")));
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Flush();
            ms.Close();
            ms = null;
        }
        private static Stream GetDatasourceAsStream(DataTable sourceTable, string sheetName, SortedList sortlist)
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
                                //case "sampcount"://采血确认
                                //    count = Convert.ToInt32(row["sampCount"]);
                                //    f = true;
                                //    break;
                                //case "reccount"://外勤标本接收
                                //    count = Convert.ToInt32(row["recCount"]);
                                //    f = true;
                                //    break;
                                case "resultcount"://检查结果录入
                                    count = Convert.ToInt32(row["resultCount"]);
                                    f = true;
                                    break;
                                //case "authorizedcount"://初步总检
                                //    count = Convert.ToInt32(row["authorizedCount"]);
                                //    break;
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
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            headerRow = null;
            sheet = null;
            workbook = null;
            return ms;
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

        #region SearchListClass
        public class SearchListClass
        {
            private double? _id;
            public double? ID
            {
                get { return _id; }
                set { _id = value; }
            }
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public SearchListClass(double? id, string name)
            {
                _id = id;
                _name = name;
            }
        }
        #endregion
    }
}