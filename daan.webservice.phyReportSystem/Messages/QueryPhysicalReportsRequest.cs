using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true, WrapperNamespace = Declarations.NameSpace)]
    public class QueryPhysicalReportsRequest : RequestBase
    {
        [DataMember(Order = 1)]
        [MessageBodyMember(Order = 1)]
        public string OrderNumber { get; set; }

        [DataMember(Order = 2)]
        [MessageBodyMember(Order = 2)]
        public String PageStart { get; set; }

        [DataMember(Order = 3)]
        [MessageBodyMember(Order = 3)]
        public String PageEnd { get; set; }

        [DataMember(Order = 4)]
        [MessageBodyMember(Order = 4)]
        public String Dictlabid { get; set; }

        [DataMember(Order = 5)]
        [MessageBodyMember(Order = 5)]
        public String Dictcustomerid { get; set; }

        [DataMember(Order = 6)]
        [MessageBodyMember(Order = 6)]
        public String StartDate { get; set; }

        [DataMember(Order = 7)]
        [MessageBodyMember(Order = 7)]
        public String EndDate { get; set; }

        [DataMember(Order = 8)]
        [MessageBodyMember(Order = 8)]
        public String SDateBegin { get; set; }

        [DataMember(Order = 9)]
        [MessageBodyMember(Order = 9)]
        public String SDateEnd { get; set; }

        [DataMember(Order = 10)]
        [MessageBodyMember(Order = 10)]
        public String Status { get; set; }

        [DataMember(Order = 11)]
        [MessageBodyMember(Order = 11)]
        public String Name { get; set; }

        [DataMember(Order = 12)]
        [MessageBodyMember(Order = 12)]
        public String ReportStatus { get; set; }
    }
}