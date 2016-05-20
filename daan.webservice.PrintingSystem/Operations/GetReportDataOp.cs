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
            var errorMessages = new List<string>();
            var reportList = new List<ReportInfo>();

            if (string.IsNullOrWhiteSpace((request.OrderNumbers)))
                return new GetReportDataResponse() { ResultType = ResultTypes.DataValidationError, Messages = new [] {"OrderNumbers cannot be null or empty."}};

            var orderNumbers = request.OrderNumbers.Split(new char[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
            if (orderNumbers.Any())
            {
                foreach (var orderNumber in orderNumbers)
                {
                    string errorMessage;
                    var reportInfo = service.GetReportInfo(orderNumber);
                    if (reportInfo != null)
                    {
                        reportList.Add(reportInfo);
                    }
                    else
                    {
                        errorMessages.Add(string.Format("{0}:{1}", orderNumber, "Cannot get and generate report data."));
                    }
                }
            }

            if (errorMessages.Any())
            {
                return new GetReportDataResponse() { ResultType = ResultTypes.PartiallyOk, Reports = reportList.ToArray(), Messages = errorMessages.ToArray()};
            }
            else
            {
                return new GetReportDataResponse() { ResultType = ResultTypes.Ok, Reports = reportList.ToArray() };
            }
        }
    }
}