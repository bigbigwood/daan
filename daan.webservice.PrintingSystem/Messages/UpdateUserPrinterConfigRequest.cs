using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;


namespace daan.webservice.phyReportSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true, WrapperNamespace = Declarations.NameSpace)]
    public class UpdateUserPrinterConfigRequest : RequestBase
    {
    }
}