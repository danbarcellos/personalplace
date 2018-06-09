using System;
using NHibernate;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public abstract class NHibernateStatelessBase
    {
        protected NHibernateStatelessBase(IStatelessSession session)
        {
            _session = session;
        }

        private readonly IStatelessSession _session;
        protected virtual IStatelessSession Session
        {
            get
            {
                return _session;
            }
        }
        
        protected virtual TResult Transact<TResult>(Func<TResult> func)
        {
            if (Session.Transaction.IsActive)
                // Transação ativa. Não encapsula
                return func.Invoke();

            // Encapsula em uma nova transação
            TResult result;
            using (var tx = Session.BeginTransaction())
            {
                result = func.Invoke();
                tx.Commit();
            }
            return result;
        }

        protected virtual void Transact(Action action)
        {
            Transact(() =>
            {
                action.Invoke();
                return false;
            });
        }
    }
}