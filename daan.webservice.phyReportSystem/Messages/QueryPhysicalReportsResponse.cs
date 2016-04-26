using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true, WrapperNamespace = Declarations.NameSpace)]
    public class QueryPhysicalReportsResponse : ResponseBase
    {
        [DataMember(Order = 1)]
        [MessageBodyMember(Order = 1)]
        public System.Data.DataTable Result { get; set; }
    }
}