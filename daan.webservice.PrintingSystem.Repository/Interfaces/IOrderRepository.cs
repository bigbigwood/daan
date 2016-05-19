using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using daan.domain;
using IBatisNet.DataMapper.Configuration.Statements;

namespace daan.webservice.PrintingSystem.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Orders, Int32>
    {
        bool UpdateOrderStatus(string orderNumber, string newStatus);
        DataTable QueryOrderReportSummaryByOrderNum(string orderNumber);
        Orders GetByOrderNum(string orderNumber);
    }
}