using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using daan.webservice.phyReportSystem.Contract.Messages;

namespace daan.webservice.phyReportSystem.Contract
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IPhysicalReportService
    {
        [OperationContract]
        QueryPhysicalReportsResponse QueryPhysicalReports(QueryPhysicalReportsRequest request);

        [OperationContract]
        ModifyPhysicalReportsResponse ModifyPhysicalReports(ModifyPhysicalReportsRequest request);
    }
}
