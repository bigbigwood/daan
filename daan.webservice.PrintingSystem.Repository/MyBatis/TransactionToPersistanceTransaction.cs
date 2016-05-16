using System.Data;
using IBatisNet.DataMapper;

namespace daan.webservice.PrintingSystem.Repository.MyBatis
{
    /// <summary>
    /// Abstraction class from NHibernate ITransaction to CRM IPersistanceTransaction
    /// </summary>
    internal class TransactionToPersistanceTransaction : IPersistanceTransaction
    {
        private ISqlMapSession _tran;
        private ISqlMapper _session;

        public TransactionToPersistanceTransaction(ISqlMapper session)
        {
            _session = session;
            _tran = _session.BeginTransaction();
        }

        public TransactionToPersistanceTransaction(IsolationLevel level, ISqlMapper session)
        {
            _session = session;
            _tran = _session.BeginTransaction(level);
        }

        public void Commit()
        {
            _tran.CommitTransaction();
        }

        public void Rollback()
        {
            _tran.RollBackTransaction();
        }

        public void Dispose()
        {
            if (_tran.IsTransactionStart)
            {
                Rollback();
            }
        }
    }
}