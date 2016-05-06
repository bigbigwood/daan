using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace daan.webservice.PrintingSystem.Contract.Models.Report
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ReportInfo
    {
        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public string ReportTemplateCode { get; set; }

        [DataMember]
        public string ReportData { get; set; }
    }
}
