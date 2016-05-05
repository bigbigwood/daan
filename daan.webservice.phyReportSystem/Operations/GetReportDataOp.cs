using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetReportDataOp : IOperation<GetReportDataRequest, GetReportDataResponse>
    {
        public GetReportDataResponse Process(GetReportDataRequest request)
        {
            return new GetReportDataResponse() { ResultType = ResultTypes.Ok };
        }
    }
}