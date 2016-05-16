using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class OperationLogRepositoryMB : MyBatisRepository<Operationlog, int>, IOperationLogRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "PrintingSystem.dict.InsertOperationlog"; }
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
    }
}