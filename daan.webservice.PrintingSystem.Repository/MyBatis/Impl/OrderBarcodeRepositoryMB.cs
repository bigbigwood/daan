using System.Data;
using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class OrderBarcodeRepositoryMB : MyBatisRepository<Orderbarcode, int>, IOrderBarcodeRepository
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

        public Orderbarcode GetByBarcode(string barcode)
        {
            return this._sqlMapper.QueryForObject<Orderbarcode>("PrintingSystem.OrderBarcode.GetByBarcode", barcode);
        }
    }
}