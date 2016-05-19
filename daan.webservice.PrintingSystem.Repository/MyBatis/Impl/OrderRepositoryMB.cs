using System.Data;
using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class OrderRepositoryMB : MyBatisRepository<Orders, int>, IOrderRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "TO BE DEFINE"; }
        }

        protected override string UpdateStatement
        {
            get { return "RBAC.updateMenu"; }
        }

        protected override string DeleteStatement
        {
            get { return "RBAC.deleteMenu"; }
        }

        protected override string QueryObjectStatement
        {
            get { return null; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryAllStatement
        {
            get { return null; }
        }
        #endregion

        public bool UpdateOrderStatus(string orderNumber, string newStatus)
        {
            return this._sqlMapper.Update("PrintingSystem.Order.UpdateStatusByOrderNum", new Orders() { Ordernum = orderNumber, Status = newStatus }) > 0;  
        }

        public DataTable QueryOrderReportSummaryByOrderNum(string orderNumber)
        {
            var result = base.SelectDS("Order.SelectOrderReportSummaryByOrderNum", orderNumber).Tables[0];
            return result;
        }

        public Orders GetByOrderNum(string orderNumber)
        {
            return this._sqlMapper.QueryForObject<Orders>("Order.SelectOrders", orderNumber);
        }
    }
}