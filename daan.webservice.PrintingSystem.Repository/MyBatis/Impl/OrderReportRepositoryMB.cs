using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class OrderReportRepositoryMB : MyBatisRepository<Orderreportdata, int>, IOrderReportRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "PrintingSystem.Order.InsertOrderreportdata"; }
        }

        protected override string UpdateStatement
        {
            get { return "PrintingSystem.Order.UpdateOrderreportdata"; }
        }

        protected override string DeleteStatement
        {
            get { return "PrintingSystem.Order.DeleteOrderreportdata"; }
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

        public Orderreportdata GetByOrderNumber(string orderNumber)
        {
            return this._sqlMapper.QueryForObject<Orderreportdata>("PrintingSystem.Order.SelectOrderreportdataByOrdernum", orderNumber);
        }
    }
}