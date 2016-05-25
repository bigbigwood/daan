using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class InitlocalsettingRepositoryMB : MyBatisRepository<Initlocalsetting, string>, IInitlocalsettingRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "PrintingSystem.Initlocalsetting.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "PrintingSystem.Initlocalsetting.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "PrintingSystem.Initlocalsetting.Delete"; }
        }

        protected override string QueryObjectStatement
        {
            get { return "PrintingSystem.Initlocalsetting.GetByKey"; }
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