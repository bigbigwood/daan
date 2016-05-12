using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;
using daan.webservice.PrintingSystem.Services;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetReportTemplatesOp : IOperation<GetReportTemplatesRequest, GetReportTemplatesResponse>
    {
        public GetReportTemplatesResponse Process(GetReportTemplatesRequest request)
        {
            var reportTemplates = ReportTemplateService.GetReportTemplates();

            return new GetReportTemplatesResponse() { ResultType = ResultTypes.Ok, ReportTemplateFiles = reportTemplates.ToArray() };
        }
    }

}