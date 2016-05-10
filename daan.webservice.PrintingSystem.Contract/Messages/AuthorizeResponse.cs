using System.Collections;
using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class AuthorizeResponse : ResponseBase
    {
        [MessageBodyMember]
        public LabInfo[] LabAssociations { get; set; }

        [MessageBodyMember]
        public OrganizationInfo[] OrganizationAssociations { get; set; }

        [MessageBodyMember]
        public ReportTemplateInfo[] ReportTemplates { get; set; }
    }
}