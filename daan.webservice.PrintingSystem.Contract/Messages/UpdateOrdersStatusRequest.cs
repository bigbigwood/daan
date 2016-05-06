using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Models.Order;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class UpdateOrdersStatusRequest : RequestBase
    {
        [MessageBodyMember]
        public OrderTransition[] OrderTransitions { get; set; }
    }
}