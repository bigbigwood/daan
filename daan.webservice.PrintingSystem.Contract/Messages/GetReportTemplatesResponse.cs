using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class GetReportTemplatesResponse : ResponseBase
    {
        [MessageBodyMember]
        public ReportTemplateFile[] ReportTemplateFiles { get; set; }
    }
}