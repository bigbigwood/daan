using System;
using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models.Report;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class GetReportDataResponse : ResponseBase
    {
        [MessageBodyMember]
        public ReportInfo[] Reports { get; set; }
    }
}