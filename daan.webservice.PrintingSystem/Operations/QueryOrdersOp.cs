using daan.service.order;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;

namespace daan.webservice.PrintingSystem.Operations
{
    public class QueryOrdersOp : IOperation<QueryOrdersRequest, QueryOrdersResponse>
    {
        public QueryOrdersResponse Process(QueryOrdersRequest request)
        {
            var htPara = new System.Collections.Hashtable();

            if (!string.IsNullOrWhiteSpace(request.OrderNumber))
            {
                htPara.Add("ordernum", request.OrderNumber);
                htPara.Add("pageStart", request.PageStart);
                htPara.Add("pageEnd", request.PageEnd);
            }
            else
            {
                htPara.Add("pageStart", request.PageStart);
                htPara.Add("pageEnd", request.PageEnd);
                htPara.Add("dictlabid", request.Dictlabid); // 分点
                htPara.Add("dictcustomerid", request.Dictcustomerid); //体检单位
                htPara.Add("StartDate", request.StartDate);
                htPara.Add("EndDate", request.EndDate);
                htPara.Add("SDateBegin", request.SDateBegin);
                htPara.Add("SDateEnd", request.SDateEnd);
                htPara.Add("status", request.Status); ;
                htPara.Add("name", request.Name);
                htPara.Add("reportstatus", request.ReportStatus);
            }

            var ordersService = new OrdersService();
            var orderCount = ordersService.DataForFocusPrintPageTotal(htPara);
            var dataTable = ordersService.DataForFocusPrintPageLst(htPara);

            return new QueryOrdersResponse() { ResultType = ResultTypes.Ok, Result = dataTable, OrderCount = orderCount };
        }


    }
}