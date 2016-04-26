using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using daan.webservice.phyReportSystem.Contract.Messages;

namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true, WrapperNamespace = Declarations.NameSpace)]
    public class AuthenticateResponse : ResponseBase
    {
    }
}