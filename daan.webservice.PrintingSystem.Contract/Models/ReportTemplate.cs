using System;
using System.Runtime.Serialization;

namespace daan.webservice.PrintingSystem.Contract.Models
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ReportTemplate
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Content { get; set; }
    }
}
