using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
using daan.webservice.phyReportSystem.Contract.Models;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class GetLastClientVersionsResponse : ResponseBase
    {
        [MessageBodyMember]
        public ClientVersionInfo[] ClientVersions { get; set; }
    }
}