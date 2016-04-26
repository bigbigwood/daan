using System.ServiceModel;
using daan.webservice.phyReportSystem.Contract.Messages;
using log4net;
using daan.webservice.phyReportSystem.Framework;
using daan.webservice.phyReportSystem.Operations;
using daan.webservice.phyReportSystem.Contract.Interface;

namespace daan.webservice.phyReportSystem
{
    [ServiceBehavior]
    public class PhysicalReportService : IPhysicalReportService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public QueryPhysicalReportsResponse QueryPhysicalReports(QueryPhysicalReportsRequest request)
        {
            return MessageProcessor.Process(request, new QueryPhysicalReportsOp());
        }

        public ModifyPhysicalReportsResponse ModifyPhysicalReports(ModifyPhysicalReportsRequest request)
        {
            return MessageProcessor.Process(request, new ModifyPhysicalReportsOp());
        }
    }
}
