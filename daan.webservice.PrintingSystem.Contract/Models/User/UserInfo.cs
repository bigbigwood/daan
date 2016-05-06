using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.User
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserInfo
    {
        [DataMember]
        public UserPrinterConfig UserPrinterConfig { get; set; }
    }
}