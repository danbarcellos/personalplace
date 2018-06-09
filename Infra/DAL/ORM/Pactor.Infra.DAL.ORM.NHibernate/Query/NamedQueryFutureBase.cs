using System;
using Pactor.Infra.DAL.ORM.Queries;
using IQuery = NHibernate.IQuery;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public abstract class NamedQueryFutureBase<TResult> : NamedQueryBase<TResult>, IQueryFuture<TResult>
    {
        protected NamedQueryFutureBase(IQueryMachine queryMachine) : base(queryMachine) { }

        public Lazy<TResult> ExecuteFuture()
        {
            var nhQuery = GetNamedQuery();
            return ExecuteFuture(nhQuery);
        }

        protected abstract Lazy<TResult> ExecuteFuture(IQuery query);

        protected Lazy<TResult> GetFuture(Func<TResult> getResult)
        {
            return QueryMachine.IsStateless || QueryMachine.HasMultipleQueriesInProgress()
                ? new Lazy<TResult>(() => QueryMachine.Execute(getResult)) 
                : new Lazy<TResult>(getResult);
        }
    }
}