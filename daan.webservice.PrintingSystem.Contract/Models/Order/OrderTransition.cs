using System.Runtime.Serialization;
using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.webservice.PrintingSystem.Contract.Models.Order
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class OrderTransition
    {
        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public OrdersStatus CurrentStatus { get; set; }

        [DataMember]
        public OrdersStatus NewStatus { get; set; }
    }
}