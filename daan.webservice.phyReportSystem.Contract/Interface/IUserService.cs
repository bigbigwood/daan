using System.ServiceModel;
using daan.webservice.phyReportSystem.Contract.Messages;

namespace daan.webservice.phyReportSystem.Contract.Interface
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IUserService
    {
        [OperationContract]
        AuthenticateResponse Authenticate(AuthenticateRequest request);
    }
}
