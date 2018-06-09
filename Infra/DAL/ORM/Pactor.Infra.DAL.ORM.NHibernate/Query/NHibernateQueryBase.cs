using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public abstract class NHibernateQueryBase<TResult> : IQuery<TResult>
    {
        protected readonly IQueryMachine QueryMachine;

        protected NHibernateQueryBase(IQueryMachine queryMachine)
        {
            QueryMachine = queryMachine;
            //DomainContext = queryMachine.GetDomainContext();
        }

        //public IDomainContext DomainContext { get; set; }

        public abstract TResult Execute();
    }
}
