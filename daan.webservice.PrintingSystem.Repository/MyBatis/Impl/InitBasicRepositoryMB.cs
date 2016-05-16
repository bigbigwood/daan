using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class InitBasicRepositoryMB : MyBatisRepository<Initbasic, int>, IInitBasicRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "PrintingSystem.Dict.InsertInitbasic"; }
        }

        protected override string UpdateStatement
        {
            get { return "PrintingSystem.Dict.UpdateInitbasic"; }
        }

        protected override string DeleteStatement
        {
            get { return "PrintingSystem.Dict.DeleteInitbasic"; }
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