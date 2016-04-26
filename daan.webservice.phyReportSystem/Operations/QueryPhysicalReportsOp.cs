using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using daan.webservice.phyReportSystem.Contract.Messages;
using daan.webservice.phyReportSystem.Framework.Operation;
using daan.service.order;

namespace daan.webservice.phyReportSystem.Operations
{
    public class QueryPhysicalReportsOp : IOperation<QueryPhysicalReportsRequest, QueryPhysicalReportsResponse>
    {
        public QueryPhysicalReportsResponse Process(QueryPhysicalReportsRequest request)
        {
            System.Collections.Hashtable htPara = new System.Collections.Hashtable();

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

            OrdersService ordersService = new OrdersService();
            var dataTable = ordersService.DataForFocusPrintPageLst(htPara);

            return new QueryPhysicalReportsResponse() { ResultType = ResultTypes.Ok, Result = dataTable };
        }


    }
}