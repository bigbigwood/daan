using daan.webservice.PrintingSystem.Contract.Models.Report;
using daan.service.order;

namespace daan.webservice.PrintingSystem.Services
{
    public class ReportService
    {
        private readonly OrderreportdataService orderReportDataService = new OrderreportdataService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public ReportInfo GetReportInfo(string orderNumber)
        {
            var rawReportData = orderReportDataService.GetOrderreportdata(orderNumber);
            return new ReportInfo() { OrderNumber = orderNumber, ReportData = rawReportData.ReportData };
        }
    }
}