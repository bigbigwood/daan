using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;
using daan.webservice.PrintingSystem.Services;

namespace daan.webservice.PrintingSystem.Operations
{
    public class GetLastClientAppVersionOp : IOperation<GetLastClientAppVersionsRequest, GetLastClientAppVersionsResponse>
    {
        public GetLastClientAppVersionsResponse Process(GetLastClientAppVersionsRequest request)
        {
            var clientApplicationVersions = ClientApplicationVersionProvider.GetClientApplicationVersion();

            return new GetLastClientAppVersionsResponse() { ResultType = ResultTypes.Ok, ClientApplicationVersions = clientApplicationVersions.ToArray()};
        }
    }
}