using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using daan.webservice.phyReportSystem.Messages;

namespace daan.webservice.phyReportSystem.Contract
{
    [ServiceContract(Namespace = Declarations.NameSpace, SessionMode = SessionMode.NotAllowed)]
    public interface IPhyReportService
    {
        [OperationContract]
        QueryPhyReportResponse QueryPhyReport(QueryPhyReportRequest request);
    }
}
