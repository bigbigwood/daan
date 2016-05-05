using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetLastClientAppVersionOp : IOperation<GetLastClientAppVersionsRequest, GetLastClientAppVersionsResponse>
    {
        public GetLastClientAppVersionsResponse Process(GetLastClientAppVersionsRequest request)
        {
            return new GetLastClientAppVersionsResponse() { ResultType = ResultTypes.Ok };
        }
    }
}