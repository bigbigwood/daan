using System;
using System.Data;
using System.ServiceModel.Channels;
using daan.domain;
using daan.service.dict;
using daan.webservice.PrintingSystem.Contract.Models.Report;
using daan.service.order;
using daan.webservice.PrintingSystem.Helper;
using daan.webservice.PrintingSystem.Repository;
using daan.webservice.PrintingSystem.Repository.Interfaces;
using log4net;

namespace daan.webservice.PrintingSystem.Services
{
    public class ReportService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        OrdersService orderService = new OrdersService();
        DictreporttemplateService repService = new DictreporttemplateService();
        daan.service.report.ReportService repServeic = new daan.service.report.ReportService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public ReportInfo GetReportInfo(string orderNumber)
        {
            var reportRepo = RepositoryManager.GetRepository<IOrderReportRepository>();
            var rawReportData = reportRepo.GetByOrderNumber(orderNumber) ?? GenerateReportData(orderNumber);
            if (rawReportData == null)
                return null;

            return new ReportInfo() { OrderNumber = orderNumber, ReportData = rawReportData.ReportData };
        }

        private Orderreportdata GenerateReportData(string orderNumber)
        {
            var reportRepo = RepositoryManager.GetRepository<IOrderReportRepository>();
            try
            {
                Log.InfoFormat("Cannot find report data, will generate report data for order numder: {0}", orderNumber);
                var rawReportData = new Orderreportdata();
                rawReportData.Orderreportdataid = orderService.getSeqID("SEQ_ORDERREPORTDATA");
                rawReportData.Ordernum = orderNumber;
                rawReportData.ReportData = GetSerializeReportDate(orderNumber);
                rawReportData.Createdate = DateTime.Now;
                reportRepo.Insert(rawReportData);
                Log.Info("Generate report data successfully!");
                return rawReportData;
            }
            catch (Exception ex)
            {
                Log.Error("Generate report data error.", ex);
                return null;
            }
        }

        public string GetSerializeReportDate(string orderNumber)
        {
            Orders orders = orderService.SelectOrdersByOrdernum(orderNumber);
            string repId = string.Empty;
            if (orders != null)
            {
                Dictreporttemplate dictreporttemplate = repService.GetDictreporttemplateByID(orders.Dictreporttemplateid.ToString());
                if (dictreporttemplate != null)
                {
                    repId = dictreporttemplate.Reporttype.ToString();
                }
            }
            DataSet ds = repServeic.GetReportData(orderNumber, repId);
            return SerializationHelper.Serialize(ds);
        }
    }
}