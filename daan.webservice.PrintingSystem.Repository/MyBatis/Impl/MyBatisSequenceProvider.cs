using System;
using System.Data;
using daan.domain;
using daan.webservice.PrintingSystem.Repository.Interfaces;
using IBatisNet.DataMapper;
using log4net;

namespace daan.webservice.PrintingSystem.Repository.MyBatis.Impl
{
    public class MyBatisSequenceProvider : ISequenceProvider
    {
                protected ISqlMapper _sqlMapper;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MyBatisSequenceProvider()
        {
            SessionToPersistanceAdapter session = RepositoryManager.GetConnection() as SessionToPersistanceAdapter;
            _sqlMapper = session.GetUndelayingSession();
        }

        public Int32 GetNextSequence(string sequence)
        {
            object obj = this._sqlMapper.QueryForObject("Common.GetSeqID", sequence);
            int result = (int)obj;
            return result;
        }
    }
}