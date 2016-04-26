using daan.webservice.phyReportSystem.Contract.Messages;
using System.ServiceModel;

namespace daan.webservice.phyReportSystem.Contract.Interface
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
