using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FastReport;
using daan.service.dict;
using daan.service.report;
using daan.service.order;
using daan.domain;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;
using daan.service.common;

namespace daan.web.code
{
    public class CommonReport
    {
        public static string json = string.Empty;//报表模板CODE
        public static string printer = string.Empty;//打印机名称
        public static string dsjson = string.Empty;//报表的数据源DS
        public string repID = "";//报告模板ID      
        ReportService repServeic = new ReportService();
        OrdersService orderService = new OrdersService();
        InitlocalsettingService initlocalsettingService = new InitlocalsettingService();
        Initlocalsetting initlocalsetting = new Initlocalsetting();
        DictreporttemplateService repService = new DictreporttemplateService();
        Report fReport = new Report();
        static string reporttemplateCode = "";
        public DataSet dsGetReportData = new DataSet();//取得报表数据源

        #region 传入体检流水号取对应的报告,web调用
        /// <summary>
        /// 传入体检流水号取对应的报告,web调用
        /// </summary>
        /// <param name="order_num">体检号</param>
        /// <param name="isprint">1为打印2为预览</param>
        /// <returns></returns>
        public Report GetReport(String order_num,int isprint)
        {
            fReport.Clear();
            repID = "";//报告模板ID
            Orders orders = orderService.SelectOrdersByOrdernum(order_num);
            if (orders != null)
            {
                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    reporttemplateCode = dictreporttemplate.Templatecode.ToString();
                    repID = dictreporttemplate.Reporttype.ToString();
                }
            }
            fReport = repServeic.GetReport(order_num, reporttemplateCode, repID, new DataSet(), HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["path"].ToString()),out dsGetReportData,isprint);            
            return fReport;
        }
        #endregion

        #region 传入体检流水号取对应的报告,winform程序调用
        /// <summary>
        /// 传入体检流水号取对应的报告,winform程序调用
        /// </summary>
        /// <param name="order_num">体检流水号</param>
        /// <param name="parth">报告模板路径</param>
        /// <returns></returns>
        public Report GetReport(String order_num,string parth)
        {
            fReport.Clear();
            repID = "";//报告模板ID
            Orders orders = orderService.SelectOrdersByOrdernum(order_num);
            if (orders != null)
            {

                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    reporttemplateCode = dictreporttemplate.Templatecode;
                    repID = dictreporttemplate.Reporttype.ToString();
                }

            }
            fReport = repServeic.GetReport(order_num, reporttemplateCode, repID, new DataSet(), parth,out dsGetReportData,2);//winform需要注册数据，所以这里为2
            return fReport;
        }
        #endregion

        #region 通过dataset取得报表，reporttype为报表类型，20为财务结算，25为健康指引单，30为收费收据,35为团检报告
        public Report GetReportByDataset(string reportType, DataSet ds,int isprint)
        {
            fReport.Clear();
            repID = "";
            repID = reportType;
            reporttemplateCode = "";
            if (reportType == ((int)ParamStatus.ReportTypeStatus.Financial).ToString())//取财务结算报告
            {
                reporttemplateCode = "FinancialDatialRep";
            }
            else if (reportType == ((int)ParamStatus.ReportTypeStatus.ChcekOrder).ToString())//取得健康指引单
            {
                reporttemplateCode = "HealthGuidelRep";
            }
            else if (reportType == ((int)ParamStatus.ReportTypeStatus.MoneyOrder).ToString())//取得收费收据
            {
                reporttemplateCode = "MoneyReceiptRep";
            }
            else if (reportType == ((int)ParamStatus.ReportTypeStatus.GroupOrder).ToString())//取得收费收据
            {
                reporttemplateCode = "GroupRep";
            }
            fReport = repServeic.GetReport("", reporttemplateCode, reportType, ds, HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["path"].ToString()), out dsGetReportData, isprint);
            return fReport;

        }
        #endregion

        #region 打印报告单
        public void PrintReport(List<string> list, List<DataSet> dslist,UserInfo userInfo)
        {
            json = Serialize(list);
            dsjson = Serialize(dslist);
            //  UserInfo的MAC地址在表initlocalsetting中查找对应当的打印机。
             // printer = "HP LaserJet 1020";

            initlocalsetting.Hostmac = userInfo.initlocalsetting.Hostmac;
            initlocalsetting = initlocalsettingService.GetInitlocalsettingInfo(initlocalsetting);
            try
            {
                if (repID == ((int)ParamStatus.ReportTypeStatus.ChcekOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.Financial).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.GroupOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.Normal).ToString())
                {
                    printer = initlocalsetting.A4printer.ToString();
                }
                else if (repID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.MoneyOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.TM15).ToString())
                {
                    printer = initlocalsetting.A5printer.ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("请先设置打印机");
            }

        }
        //add by lee
        public void PrintReport2(string strreport, DataSet ds, UserInfo userInfo)
        {
            json = Serialize(strreport);
            dsjson = Serialize(ds);
            //  UserInfo的MAC地址在表initlocalsetting中查找对应当的打印机。
            // printer = "HP LaserJet 1020";

            initlocalsetting.Hostmac = userInfo.initlocalsetting.Hostmac;
            initlocalsetting = initlocalsettingService.GetInitlocalsettingInfo(initlocalsetting);
            try
            {
                if (repID == ((int)ParamStatus.ReportTypeStatus.ChcekOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.Financial).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.GroupOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.Normal).ToString())
                {
                    printer = initlocalsetting.A4printer.ToString();
                }
                else if (repID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.MoneyOrder).ToString() || repID == ((int)ParamStatus.ReportTypeStatus.TM15).ToString())
                {
                    printer = initlocalsetting.A5printer.ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("请先设置打印机");
            }
        }

        public Hashtable PrintReport3(string orderNum, UserInfo userInfo, int isPrint)
        {
            fReport.Clear();
            string reportID = string.Empty;
            string reportCode = string.Empty;
            Orders orders = orderService.SelectOrdersByOrdernum(orderNum);
            if (orders != null)
            {
                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    reportCode = dictreporttemplate.Templatecode.ToString();
                    reportID = dictreporttemplate.Reporttype.ToString();
                }
            }
            string path=HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["path"].ToString());
            fReport = repServeic.GetReport(orderNum, reportCode, reportID, new DataSet(), path, out dsGetReportData, isPrint);
            string reportString = Serialize(fReport.SaveToString());
            string dsString = Serialize(dsGetReportData);
            string printerString = string.Empty;
            initlocalsetting.Hostmac = userInfo.initlocalsetting.Hostmac;
            initlocalsetting = initlocalsettingService.GetInitlocalsettingInfo(initlocalsetting);
            try
            {
                if (reportID == ((int)ParamStatus.ReportTypeStatus.ChcekOrder).ToString() || reportID == ((int)ParamStatus.ReportTypeStatus.Financial).ToString() || reportID == ((int)ParamStatus.ReportTypeStatus.GroupOrder).ToString() || reportID == ((int)ParamStatus.ReportTypeStatus.Normal).ToString())
                {
                    printerString = initlocalsetting.A4printer.ToString();
                }
                else if (reportID == ((int)ParamStatus.ReportTypeStatus.HPV).ToString() || reportID == ((int)ParamStatus.ReportTypeStatus.MoneyOrder).ToString() || reportID == ((int)ParamStatus.ReportTypeStatus.TM15).ToString())
                {
                    printerString = initlocalsetting.A5printer.ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("请先设置打印机");
            }
            Hashtable ht = new Hashtable();
            ht.Add("printer", printerString);
            ht.Add("json",reportString);
            ht.Add("dsjson", dsString);
            return ht;
        }

        public Hashtable getPrintReportDate(string ordernum)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["SerializeReportDateFile"].ToString();
            string path2 = path + ordernum.Substring(0, 6);
            string rep_filename = path2 + "\\" + ordernum + "_rep.bin";

            Hashtable htPrint = new Hashtable();
            if (File.Exists(rep_filename))
            {
                htPrint = DesrializeReportDataFromFile(rep_filename);
            }
            else//如果没有预先生成的文件，则通过读取数据库填充报告模板
            {
                //根据体检号获取报告模板和数据
                Orders orders = orderService.SelectOrdersByOrdernum(ordernum);
                string repCode = string.Empty;
                string repID = string.Empty;
                if (orders != null)
                {
                    Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                    if (dictreporttemplate != null)
                    {
                        repCode = dictreporttemplate.Templatecode.ToString();
                        repID = dictreporttemplate.Reporttype.ToString();
                    }
                }
                string reportTemplatePath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["path"].ToString());
                DataSet dsReportData = new DataSet();
                Report rep = repServeic.GetReport(ordernum, repCode, repID, new DataSet(), reportTemplatePath, out dsReportData, 1);
                //序列化报告模板和数据
                htPrint["json"] = Serialize(rep.SaveToString());
                htPrint["dsjson"] = Serialize(dsReportData);
                //throw new Exception(string.Format("未读取到体检号为{0}的报告序列化文件！",ordernum));
            }
            return htPrint;
        }
        /// <summary>
        /// 获取报告数据，不涉及模板
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public Hashtable getPrintData(string ordernum, string dictreporttemplateid)
        {
            string repCode = string.Empty;
            string repID = string.Empty;
            Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(dictreporttemplateid);
            if (dictreporttemplate != null)
            {
                repCode = dictreporttemplate.Templatecode.ToString();
                repID = dictreporttemplate.Reporttype.ToString();
            }
            DataSet ds = repServeic.GetReportData(ordernum, repID);
            Hashtable htPrint = new Hashtable();
            htPrint.Add("repCode",repCode);
            htPrint.Add("dsjson",Serialize(ds));
            return htPrint;
        }

        public void SerializeReportDate(string ordernum)
        {
            //存放文件位置
            string path = System.Configuration.ConfigurationManager.AppSettings["SerializeReportDateFile"].ToString();
            string path2 = path + ordernum.Substring(0, 6);
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            string rep_filename = path2 + "\\" + ordernum + "_rep.bin";
            //根据体检号获取报告模板和数据
            Orders orders = orderService.SelectOrdersByOrdernum(ordernum);
            string repCode = string.Empty;
            string repID = string.Empty;
            if (orders != null)
            {
                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    repCode = dictreporttemplate.Templatecode.ToString();
                    repID = dictreporttemplate.Reporttype.ToString();
                }
            }
            string reportTemplatePath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["path"].ToString());
            DataSet dsReportData = new DataSet();
            Report rep = repServeic.GetReport(ordernum, repCode, repID, new DataSet(), reportTemplatePath, out dsReportData, 1);
            //序列化报告模板和数据
            Hashtable ht = new Hashtable();
            ht["json"] = Serialize(rep.SaveToString());
            ht["dsjson"] = Serialize(dsReportData);
            SerializeReportDataToFile(ht,rep_filename);
        }

        public string GetSerializeReportDate(string ordernum)
        {
            Orders orders = orderService.SelectOrdersByOrdernum(ordernum);
            string repID = string.Empty;
            if (orders != null)
            {
                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    repID = dictreporttemplate.Reporttype.ToString();
                }
            }
            DataSet ds = repServeic.GetReportData(ordernum, repID);
            return Serialize(ds);
        }
        #endregion

        #region 打印条码
        public void PrintBarCode(DataTable dt,UserInfo userInfo)
        {
            json = Serialize(dt);
            //   查找对应的条码打印机
            //printer = "TSC TTP-244 Plus";
            initlocalsetting.Hostmac = userInfo.initlocalsetting.Hostmac;
            initlocalsetting = initlocalsettingService.GetInitlocalsettingInfo(initlocalsetting);
            try
            {
                printer = initlocalsetting.Barcodeprinter.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("请先设置打印机");
            }
        }
        #endregion

        #region 辅助方法 序列化  保存和读取
        private static string Serialize<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }

        public void SerializeReportDataToFile(Hashtable ht,string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);
                FileStream fs = new FileStream(filepath, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, ht);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("保存序列化文件失败,原因:" + ex.Message);
            }
        }

        public Hashtable DesrializeReportDataFromFile(string filepath)
        {
            try
            {
                if (!File.Exists(filepath)) return null;
                FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                Hashtable ht = (Hashtable)bf.Deserialize(fs);
                fs.Flush();
                fs.Close();
                return ht;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}