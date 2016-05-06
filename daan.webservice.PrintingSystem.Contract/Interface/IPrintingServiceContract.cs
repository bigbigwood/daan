using System.ServiceModel;
using daan.webservice.PrintingSystem.Contract.Messages;

namespace daan.webservice.PrintingSystem.Contract.Interface
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IPrintingServiceContract
    {
        #region Order service

        [OperationContract]
        QueryOrdersResponse QueryOrders(QueryOrdersRequest request);

        [OperationContract]
        UpdateOrdersStatusResponse UpdateOrdersStatus(UpdateOrdersStatusRequest request);

        #endregion

        #region Report service

        [OperationContract]
        GetReportDataResponse GetReportData(GetReportDataRequest request); 
        
        #endregion
    }
}
