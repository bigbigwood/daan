using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ClientVersionInfo
    {
        [DataMember]
        public String ApplicationType { get; set; }
        [DataMember]
        public String ApplicationVersion { get; set; }
        [DataMember]
        public String ReportTemplateVersion { get; set; }
    }
}
