using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class UpdateUserInfoRequest : RequestBase
    {
        [MessageBodyMember]
        public UserInfo UserInfo { get; set; }
    }
}