using System;
using System.Collections.Generic;
using System.Linq;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models.Report;
using daan.webservice.PrintingSystem.Framework.Operation;
using daan.webservice.PrintingSystem.Services;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetReportDataOp : IOperation<GetReportDataRequest, GetReportDataResponse>
    {
        private readonly ReportService service = new ReportService();

        public GetReportDataResponse Process(GetReportDataRequest request)
        {
            if (string.IsNullOrWhiteSpace((request.OrderNumbers)))
                return new GetReportDataResponse() { ResultType = ResultTypes.DataValidationError, Messages = new [] {"OrderNumbers cannot be null or empty."}};

            var reportList = new List<ReportInfo>();
            var orderNumbers = request.OrderNumbers.Split(new char[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
            if (orderNumbers.Any())
            {
                reportList.AddRange(orderNumbers.Select(orderNumber => service.GetReportInfo(orderNumber)));
            }

            return new GetReportDataResponse() { ResultType = ResultTypes.Ok, Reports = reportList.ToArray()};
        }
    }
}