using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace daan.webservice.phyReportSystem.Messages
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class QueryPhyReportRequest : RequestBase
    {
        [DataMember(Order = 1)]
        [MessageBodyMember(Order = 1)]
        public string OrderNumber { get; set; }

        [DataMember(Order = 2)]
        [MessageBodyMember(Order = 2)]
        public DateTime? RangeStartDate { get; set; }

        [DataMember(Order = 3)]
        [MessageBodyMember(Order = 3)]
        public DateTime? RangeEndDate { get; set; }

        [DataMember(Order = 4)]
        [MessageBodyMember(Order = 4)]
        public String OrderStatus { get; set; }
    }
}