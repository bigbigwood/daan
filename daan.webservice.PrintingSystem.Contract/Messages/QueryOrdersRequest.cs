using System;
using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class QueryOrdersRequest : RequestBase
    {
        [MessageBodyMember(Order = 1)]
        public String OrderNumber { get; set; }

        [MessageBodyMember(Order = 2)]
        public String Barcode { get; set; }

        [MessageBodyMember(Order = 3)]
        public String PageStart { get; set; }

        [MessageBodyMember(Order = 4)]
        public String PageEnd { get; set; }

        [MessageBodyMember(Order = 5)]
        public String Dictlabid { get; set; }

        [MessageBodyMember(Order = 6)]
        public String Dictcustomerid { get; set; }

        [MessageBodyMember(Order = 7)]
        public String StartDate { get; set; }

        [MessageBodyMember(Order = 8)]
        public String EndDate { get; set; }

        [MessageBodyMember(Order = 9)]
        public String ScanStartDate { get; set; }

        [MessageBodyMember(Order = 10)]
        public String ScanEndDate { get; set; }

        [MessageBodyMember(Order = 11)]
        public String OrderStatus { get; set; }

        [MessageBodyMember(Order = 12)]
        public String Keyword { get; set; }

        [MessageBodyMember(Order = 13)]
        public String ReportStatus { get; set; }

        [MessageBodyMember(Order = 14)]
        public String Section { get; set; }

        [MessageBodyMember(Order = 15)]
        public String Province { get; set; }

        [MessageBodyMember(Order = 16)]
        public String FinanceAuditStatus { get; set; }

        [MessageBodyMember(Order = 17)]
        public String BatchNumber { get; set; }

        [MessageBodyMember(Order = 18)]
        public String AuditStartDate { get; set; }

        [MessageBodyMember(Order = 19)]
        public String AuditEndDate { get; set; }
    }
}