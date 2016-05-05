using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class QueryPhysicalReportsResponse : ResponseBase
    {
        [MessageBodyMember]
        public System.Data.DataTable Result { get; set; }
    }
}