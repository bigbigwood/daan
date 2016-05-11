using System;
using System.ServiceModel;

namespace daan.webservice.PrintingSystem.Contract.Messages
{
    [MessageContract(IsWrapped = true)]
    public class QueryOrdersResponse : ResponseBase
    {
        [MessageBodyMember]
        public System.Data.DataTable Result { get; set; }

        [MessageBodyMember]
        public Int32 OrderCount { get; set; }
    }
}