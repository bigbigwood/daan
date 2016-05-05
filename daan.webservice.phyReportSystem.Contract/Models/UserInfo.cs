using System.Runtime.Serialization;
namespace daan.webservice.phyReportSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserInfo
    {
        [DataMember]
        public UserCredential UserCredential { get; set; }
        [DataMember]
        public UserPrinterConfig UserPrinterConfig { get; set; }
    }
}