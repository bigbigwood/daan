using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetReportTemplatesOp : IOperation<GetReportTemplatesRequest, GetReportTemplatesResponse>
    {
        public GetReportTemplatesResponse Process(GetReportTemplatesRequest request)
        {
            return new GetReportTemplatesResponse() { ResultType = ResultTypes.Ok };
        }
    }
}