using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class GetLastClientAppVersionsResponse : ResponseBase
    {
        [MessageBodyMember]
        public ClientVersionInfo[] ClientVersions { get; set; }
    }
}