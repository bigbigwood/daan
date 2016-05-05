using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class AuthenticateRequest : RequestBase
    {
    }
}