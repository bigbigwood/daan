using System.Runtime.Serialization;
namespace daan.webservice.phyReportSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserCredential
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}