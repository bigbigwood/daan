using System;
using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class QueryOrdersRequest : RequestBase
    {
        [MessageBodyMember(Order = 1)]
        public string OrderNumber { get; set; }

        [MessageBodyMember(Order = 2)]
        public String PageStart { get; set; }

        [MessageBodyMember(Order = 3)]
        public String PageEnd { get; set; }

        [MessageBodyMember(Order = 4)]
        public String Dictlabid { get; set; }

        [MessageBodyMember(Order = 5)]
        public String Dictcustomerid { get; set; }

        [MessageBodyMember(Order = 6)]
        public String StartDate { get; set; }

        [MessageBodyMember(Order = 7)]
        public String EndDate { get; set; }

        [MessageBodyMember(Order = 8)]
        public String SDateBegin { get; set; }

        [MessageBodyMember(Order = 9)]
        public String SDateEnd { get; set; }

        [MessageBodyMember(Order = 10)]
        public String Status { get; set; }

        [MessageBodyMember(Order = 11)]
        public String Name { get; set; }

        [MessageBodyMember(Order = 12)]
        public String ReportStatus { get; set; }
    }
}