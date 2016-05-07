using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.User
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserCredential
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}