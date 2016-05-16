using System;
using System.Data;
using IBatisNet.DataMapper;

namespace daan.webservice.PrintingSystem.Repository.MyBatis
{
    internal class SessionToPersistanceAdapter : IPersistanceConnection
    {
        ISqlMapper _session;
        ClearSessionForThread _notifier;

        public SessionToPersistanceAdapter(ISqlMapper iSession)
        {
            this._session = iSession;
        }

        public void Close()
        {
            _session.CloseConnection();
        }

        public void Dispose()
        {
            _notifier.Invoke();
            _session.CloseConnection();
        }

        public ISqlMapper GetUndelayingSession()
        {
            return _session;
        }

        public IPersistanceTransaction BeginTransaction()
        {
            return (new TransactionToPersistanceTransaction(_session));
        }

        public IPersistanceTransaction BeginTransaction(IsolationLevel level)
        {
            return (new TransactionToPersistanceTransaction(level, _session));
        }

        public void SetNotifier(ClearSessionForThread notifier)
        {
            if (notifier == null)
                throw new ArgumentNullException("notifier");

            _notifier = notifier;
        }


    }
}