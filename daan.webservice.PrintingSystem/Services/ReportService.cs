using System;
using daan.webservice.PrintingSystem.Contract.Models.Report;
using daan.service.order;
using daan.webservice.PrintingSystem.Repository;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Services
{
    public class ReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public ReportInfo GetReportInfo(string orderNumber)
        {
            var reportRepo = RepositoryManager.GetRepository<IOrderReportRepository>();
            var rawReportData = reportRepo.GetByOrderNumber(orderNumber);
            if (rawReportData == null)
            {
                throw new Exception(string.Format("不能找到order number:({0})的报告数据", orderNumber));
            }

            return new ReportInfo() { OrderNumber = orderNumber, ReportData = rawReportData.ReportData };
        }
    }
}