using System;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Transform;
using Pactor.Infra.DAL.ORM.NHibernate.Query;

namespace Pactor.Infra.DAL.ORM.NHibernateQuery
{
    public abstract class NHibernateSQLQueryBase<TResult> : NHibernateQueryBase<TResult>
    {
        protected NHibernateSQLQueryBase(IQueryMachine queryMachine) : base(queryMachine)
        {
            BuildQueryString();
        }

        protected string QueryString { get; private set; }

        public override TResult Execute()
        {
            var nhQuery = GetSQLQuery();
            return QueryMachine.Execute(() => Execute(nhQuery));
        }

        protected virtual ISQLQuery GetSQLQuery()
        {
            var nhQuery = QueryMachine.CreateSQLQuery(QueryString);

            if (!typeof(IEntity).IsAssignableFrom(typeof(TResult)) && typeof(TResult).IsClass && typeof(TResult).GetConstructors(BindingFlags.Public).Any())
            {
                DeclareScalarResults(nhQuery);
                nhQuery.SetResultTransformer(Transformers.AliasToBean<TResult>());
            }

            nhQuery.SetTimeout(120);
            SetParameters(nhQuery);
            return nhQuery;
        }

        protected virtual void DeclareScalarResults(ISQLQuery nhQuery)
        {
            var resultProperties = typeof (TResult).GetProperties(BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var resultProperty in resultProperties)
            {
                nhQuery.AddScalar(resultProperty.Name, NHibernateUtil.GuessType(resultProperty.PropertyType));
            }
        }

        protected abstract TResult Execute(IQuery query);

        protected abstract void SetParameters(IQuery nhQuery);

        protected abstract string GetQueryScript(Type dialectType);

        private void BuildQueryString()
        {
            QueryString = GetQueryScript(QueryMachine.Dialect.GetType());
            if (QueryString == null)
                throw new NotImplementedException(string.Format("Query script \"{0}\" not implemented for the {1} NHibernate dialect",
                    GetType().Name,
                    QueryMachine.Dialect.GetType().Name));
        }
    }
}