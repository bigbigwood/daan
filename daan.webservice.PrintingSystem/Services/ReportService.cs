using System;
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
            if (rawReportData == null)
            {
                throw new Exception(string.Format("不能找到order number:({0})的报告数据", orderNumber));
            }

            return new ReportInfo() { OrderNumber = orderNumber, ReportData = rawReportData.ReportData };
        }
    }
}