using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Messages;

namespace daan.webservice.PrintingSystem.Contract.Interface
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IClientApplicationServiceContract
    {
        #region User Authenticate

        [OperationContract]
        AuthenticateResponse Authenticate(AuthenticateRequest request);

        #endregion

        #region Client Updates

        [OperationContract]
        GetLastClientAppVersionsResponse GetLastClientAppVersions(GetLastClientAppVersionsRequest request);

        [OperationContract]
        GetReportTemplatesResponse GetReportTemplates(GetReportTemplatesRequest request); 

        #endregion
    }
}
