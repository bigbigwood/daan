using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class AuthenticateRequest : RequestBase
    {
        [MessageBodyMember]
        public string HostMac { get; set; }

        [MessageBodyMember]
        public string HostName { get; set; }
    }
}