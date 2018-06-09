using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Pactor.Infra.DAL.ORM.Queries;
using PersonalPlace.Domain.Base;

namespace Pactor.Infra.DAL.ORM.NHibernateQuery
{
    public abstract class NHibernateQueryBase<TResult> : IQuery<TResult>
    {
        protected readonly IQueryMachine QueryMachine;

        protected NHibernateQueryBase(IQueryMachine queryMachine)
        {
            QueryMachine = queryMachine;
            DomainContext = queryMachine.GetDomainContext();
        }

        public IDomainContext DomainContext { get; set; }

        public abstract TResult Execute();
    }
}
