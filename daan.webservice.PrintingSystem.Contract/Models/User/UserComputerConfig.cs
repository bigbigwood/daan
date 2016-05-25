using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models.User
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserComputerConfig
    {
        [DataMember]
        public String HostName { get; set; }
        [DataMember]
        public String HostMac { get; set; }
        [DataMember]
        public String IpAddress { get; set; }
    }
}