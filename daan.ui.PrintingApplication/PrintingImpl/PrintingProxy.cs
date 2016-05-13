using System;
using System.Configuration;
using System.Data;
using daan.ui.PrintingApplication.Helper;
using FastReport;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FastReport.Utils;
using log4net;
using ReportDTO = daan.webservice.PrintingSystem.Contract.Models.Report.ReportInfo;

namespace daan.ui.PrintingApplication.PrintingImpl
{
    public class PrintingProxy
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly bool EnableAutoSetReportFileName = ConfigurationManager.AppSettings.Get("AutoSetReportFileNameWhilePrinting").ToUpper() == "TRUE";
        private readonly bool EnableFastReportProgressBar = ConfigurationManager.AppSettings.Get("EnableFastReportProgressBar").ToUpper() == "TRUE";
        
        
        public void PrintReport(string printerName, ReportDTO reportDTO)
        {
            string printType = string.Empty;
            string repCode = reportDTO.ReportTemplateCode;

            DataSet ds = new DataSet();
            ds = Desrialize(ds, reportDTO.ReportData);

            string reportTemplateContent = ReportTemplateFileProvider.GetLocalReportTemplateFileByCode(repCode);
            Report report = new Report();
            report.Clear();
            report.LoadFromString(reportTemplateContent);

            if (repCode.Contains("CommonRep") || repCode.Contains("TM15Rep") || repCode.Contains("HpvLctRep"))
            {
                printType = "横向";
                report.RegisterData(ds.Tables["dtRepTitle"], "dtRepTitle");//注册表头首页信息
                report.RegisterData(ds.Tables["dtRepImportantSigns"], "dtRepImportantSigns");//本次体检结果
                report.RegisterData(ds.Tables["dtRepExamCompared"], "dtRepExamCompared");//注册历次体检比对
                report.RegisterData(ds.Tables["dtRepDiseaseGuide"], "dtRepDiseaseGuide");//解读与建议
                report.RegisterData(ds.Tables["dtRepMustExam"], "dtRepMustExam");//注册每次体检必须检查的项目
                report.RegisterData(ds.Tables["dtRepRecommendExam"], "dtRepRecommendExam");//注册下次体检特别推荐的项目
                report.RegisterData(ds.Tables["dtOrderresultcomment"], "dtOrderresultcomment");//注册总体评价
            }
            else if (repCode.Contains("C14Rep"))
            {
                printType = "横向";
                report.RegisterData(ds.Tables["dtRepTitle"], "dtRepTitle");//注册表头首页信息
                report.RegisterData(ds.Tables["dtRepImportantSigns"], "dtRepImportantSigns");//本次体检结果
            }
            else
            {
                printType = "横向";
                report.RegisterData(ds.Tables["dtRepTitle"], "dtRepTitle");//注册表头首页信息
                report.RegisterData(ds.Tables["dtRepImportantSigns"], "dtRepImportantSigns");//本次体检结果
                report.RegisterData(ds.Tables["dtRepExamCompared"], "dtRepExamCompared");//注册历次体检比对
                report.RegisterData(ds.Tables["dtRepDiseaseGuide"], "dtRepDiseaseGuide");//解读与建议
                report.RegisterData(ds.Tables["dtRepMustExam"], "dtRepMustExam");//注册每次体检必须检查的项目
                report.RegisterData(ds.Tables["dtRepRecommendExam"], "dtRepRecommendExam");//注册下次体检特别推荐的项目
                report.RegisterData(ds.Tables["dtOrderresultcomment"], "dtOrderresultcomment");//注册总体评价
            }

            foreach (FastReport.ReportPage page in report.Pages)
            {
                page.Landscape = (printType != "横向");
            }

            FastReport.Utils.Config.ReportSettings.ShowProgress = EnableFastReportProgressBar;
            report.PrintSettings.Printer = printerName;
            report.PrintSettings.ShowDialog = false;
            if (EnableAutoSetReportFileName)
                report.FileName = string.Format("{0}.pdf", reportDTO.OrderNumber);

            Log.InfoFormat("FastReport print {0} report start...", reportDTO.OrderNumber);
            report.Print();
            Log.InfoFormat("FastReport print {0} report finished...", reportDTO.OrderNumber);
        }


        public static T Desrialize<T>(T obj, string str)
        {
            try
            {
                obj = default(T);
                IFormatter formatter = new BinaryFormatter();
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("反序列化失败,原因:" + ex.Message);
            }
            return obj;
        }
    }
}
