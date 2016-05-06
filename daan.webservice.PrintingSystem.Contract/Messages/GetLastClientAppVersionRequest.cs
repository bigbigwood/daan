using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class GetLastClientAppVersionsRequest : RequestBase
    {
    }
}