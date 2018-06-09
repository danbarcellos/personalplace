using System.Linq;
using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public abstract class NamedQueryBase<TResult> : NHibernateQueryBase<TResult>, INamedQuery
    {
        protected NamedQueryBase(IQueryMachine queryMachine) : base(queryMachine) { }
        
        public override TResult Execute()
        {
            var nhQuery = GetNamedQuery();
            return QueryMachine.Execute(() => Execute(nhQuery));
        }
        
        protected abstract TResult Execute(global::NHibernate.IQuery query);

        protected virtual global::NHibernate.IQuery GetNamedQuery()
        {
            var nhQuery = QueryMachine.GetNamedQuery(((INamedQuery)this).QueryName);
            nhQuery.SetComment($"Named Query name: {this.GetType().Name}");
            SetFilterParameters(nhQuery);
            SetParameters(nhQuery);
            return nhQuery;
        }

        protected void SetFilterParameters(global::NHibernate.IQuery nhQuery)
        {
            //if (nhQuery.NamedParameters.Any(name => name == ScopeFilters.AscendingSegregation.ParameterName))
            //    nhQuery.SetParameterList(ScopeFilters.AscendingSegregation.ParameterName, DomainContext.ScopeTag.ToAscendingScopeTags());

            //if (nhQuery.NamedParameters.Any(name => name == ScopeFilters.DescendingSegregation.ParameterName))
            //    nhQuery.SetParameter(ScopeFilters.DescendingSegregation.ParameterName, DomainContext.ScopeTag + "%");

            //if (nhQuery.NamedParameters.Any(name => name == ScopeFilters.ParenthoodSegregation.ParameterName))
            //    nhQuery.SetParameterList(ScopeFilters.ParenthoodSegregation.ParameterName, DomainContext.ScopeTag.ToParenthoodAscendingScopeTags());
        }

        protected abstract void SetParameters(global::NHibernate.IQuery nhQuery);

        public virtual string QueryName => this.GetType().Name;

    }
}