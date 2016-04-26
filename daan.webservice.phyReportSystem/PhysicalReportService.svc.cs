using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using daan.webservice.phyReportSystem.Contract;
using daan.webservice.phyReportSystem.Contract.Messages;
using log4net;
using daan.webservice.phyReportSystem.Framework.Authenticaition;
using daan.webservice.phyReportSystem.Framework;
using daan.service.order;
using daan.service.dict;
using daan.domain;
using daan.webservice.phyReportSystem.Framework.Operation;
using daan.webservice.phyReportSystem.Operations;

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
