using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace daan.webservice.phyReportSystem.Framework.Operation
{
    public interface IOperation<TRequest, TResponse>
    {
        TResponse Process(TRequest request);
    }
}