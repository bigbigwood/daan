using daan.webservice.PrintingSystem.Contract.Interface;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework;
using daan.webservice.PrintingSystem.Operations;

namespace daan.webservice.PrintingSystem
{
    public class PrintingService : IPrintingServiceContract
    {
        public QueryOrdersResponse QueryOrders(QueryOrdersRequest request)
        {
            return MessageProcessor.Process(request, new QueryOrdersOp());
        }

        public UpdateOrdersStatusResponse UpdateOrdersStatus(UpdateOrdersStatusRequest request)
        {
            return MessageProcessor.Process(request, new UpdateOrdersStatusOp());
        }

        public GetReportDataResponse GetReportData(GetReportDataRequest request)
        {
            return MessageProcessor.Process(request, new GetReportDataOp());
        }
    }
}
