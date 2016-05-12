using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FastReport;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Printing;
using System.Net;
using System.Net.NetworkInformation;
using ReportDTO = daan.webservice.PrintingSystem.Contract.Models.Report.ReportInfo;

namespace daan.ui.PrintingApplication.PrintingImpl
{
    public class PrintingProxy
    {
        bool IsSetReportFileName = ConfigurationManager.AppSettings.Get("AutoSetReportFileNameWhilePrinting").ToUpper() == "TRUE";
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

            report.PrintSettings.Printer = printerName;
            report.PrintSettings.ShowDialog = false;
            if (IsSetReportFileName)
                report.FileName = string.Format("{0}.pdf", reportDTO.OrderNumber);


            report.Print();
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
