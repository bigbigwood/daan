using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class InitlocalsettingRepositoryMB : MyBatisRepository<Initlocalsetting, int>, IInitlocalsettingRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "PrintingSystem.Dict.InsertInitlocalsetting"; }
        }

        protected override string UpdateStatement
        {
            get { return "PrintingSystem.Dict.UpdateInitlocalsetting"; }
        }

        protected override string DeleteStatement
        {
            get { return "PrintingSystem.Dict.DeleteInitlocalsetting"; }
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

        public Initlocalsetting GetInitLocalSettingByHostMac(string hostMac)
        {
            return this._sqlMapper.QueryForObject<Initlocalsetting>("PrintingSystem.Dict.GetInitLocalSettingByHostMac", hostMac);
        }
    }
}