using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class UpdateOrdersStatusOp : IOperation<UpdateOrdersStatusRequest, UpdateOrdersStatusResponse>
    {
        public UpdateOrdersStatusResponse Process(UpdateOrdersStatusRequest request)
        {
            return new UpdateOrdersStatusResponse() { ResultType = ResultTypes.Ok };
        }


    }
}