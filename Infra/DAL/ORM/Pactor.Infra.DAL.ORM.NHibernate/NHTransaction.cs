using System.Data;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHTransaction : ITransaction
    {
        private readonly global::NHibernate.ITransaction _transaction;

        internal NHTransaction(global::NHibernate.ITransaction transaction)
        {
            _transaction = transaction;
        }

        public bool IsActive { get { return _transaction.IsActive; } }

        public bool WasCommitted { get { return _transaction.WasCommitted; } }

        public bool WasRolledBack { get { return _transaction.WasRolledBack; } }

        public void Begin()
        {
            _transaction.Begin();
        }

        public void Begin(IsolationLevel isolationLevel)
        {
            _transaction.Begin(isolationLevel);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}