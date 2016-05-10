using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Messages;

namespace daan.webservice.PrintingSystem.Contract.Interface
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IClientApplicationServiceContract
    {
        [OperationContract]
        AuthenticateResponse Authenticate(AuthenticateRequest request);

        [OperationContract]
        AuthorizeResponse Authorize(AuthorizeRequest request);

        [OperationContract]
        GetLastClientAppVersionsResponse GetLastClientAppVersions(GetLastClientAppVersionsRequest request);

        [OperationContract]
        GetReportTemplatesResponse GetReportTemplates(GetReportTemplatesRequest request); 
    }
}
