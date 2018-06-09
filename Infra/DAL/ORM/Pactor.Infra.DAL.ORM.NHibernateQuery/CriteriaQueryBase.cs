using NHibernate;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM.NHibernateQuery
{
    public abstract class CriteriaQueryBase<TResult> : NHibernateQueryBase<TResult>, ICriteriaQuery
    {
        protected CriteriaQueryBase(IQueryMachine queryMachine) : base(queryMachine) { }

        public override TResult Execute()
        {
            var criteria = GetCriteria();
            return QueryMachine.Execute(() => Execute(criteria));
        }

        protected abstract ICriteria GetCriteria();
        
        protected abstract TResult Execute(ICriteria criteria);
    }
}