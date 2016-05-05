using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using daan.webservice.phyReportSystem.Contract.Messages;
using daan.webservice.phyReportSystem.Framework.Operation;

namespace daan.webservice.phyReportSystem.Operations
{
    public class GetLastClientVersionsOp : IOperation<GetLastClientVersionsRequest, GetLastClientVersionsResponse>
    {
        public GetLastClientVersionsResponse Process(GetLastClientVersionsRequest request)
        {
            return new GetLastClientVersionsResponse() { ResultType = ResultTypes.Ok };
        }
    }
}