using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.web.code;
using System.Collections;
using System.Data;
using FastReport;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using daan.service.common;
using daan.service.dict;

namespace daan.web.admin.report
{
    public partial class RepShowView : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {         
        }
        CommonReport commonReport = new CommonReport();
        protected void reportShowView_StartReport(object sender, EventArgs e)
        {
            if (Request["reportType"].ToString() == "1")//1为个体检，2为团检
            {
                reportShowView.Report = commonReport.GetReport(Request["order_num"].ToString(),2);
            }
            else if (Request["reportType"].ToString() == "2")
            {
                if (Request["resultType"].ToString() == "yes")
                {
                    reportShowView.Report = commonReport.GetReportByDataset("35", (DataSet)Session["GroupDataSet"],2);
                }
                else
                {
                    reportShowView.Report = commonReport.GetReportByDataset("35", new DataSet(),2);
                }
            }
            if (reportShowView.Report.FileName != "")
            {
                reportShowView.ReportDone = true;
            }
        }
    }
}