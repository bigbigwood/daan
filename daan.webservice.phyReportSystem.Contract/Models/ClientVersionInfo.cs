using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace daan.webservice.phyReportSystem.Contract.Models
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
