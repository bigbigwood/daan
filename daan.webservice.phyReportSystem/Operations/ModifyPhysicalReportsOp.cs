using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using daan.webservice.phyReportSystem.Contract.Messages;
using daan.webservice.phyReportSystem.Framework.Operation;

namespace daan.webservice.phyReportSystem.Operations
{
    public class ModifyPhysicalReportsOp : IOperation<ModifyPhysicalReportsRequest, ModifyPhysicalReportsResponse>
    {
        public ModifyPhysicalReportsResponse Process(ModifyPhysicalReportsRequest request)
        {
            return new ModifyPhysicalReportsResponse();
        }
    }
}