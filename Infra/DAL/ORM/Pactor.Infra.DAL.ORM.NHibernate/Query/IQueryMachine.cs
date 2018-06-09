using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public interface IQueryMachine
    {
        IQueryable<TEntity> Query<TEntity>();
        IQueryOver<T, T> QueryOver<T>() where T : class;
        IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class;
        IQuery GetNamedQuery(string queryName);
        ICriteria CreateCriteria<T>() where T : class;
        ICriteria CreateCriteria<T>(string alias) where T : class;
        ICriteria CreateCriteria(string entityName);
        ICriteria CreateCriteria(System.Type entityType);
        ICriteria CreateCriteria(string entityName, string alias);
        ICriteria CreateCriteria(System.Type entityType, string alias);
        IQuery CreateQuery(string queryString);
        ISQLQuery CreateSQLQuery(string queryString);
        TResult Execute<TResult>(Func<TResult> action);
        global::NHibernate.Dialect.Dialect Dialect { get; }
        bool IsStateless { get; }
        T GetInnerSession<T>() where T : class;
        bool HasMultipleQueriesInProgress();
    }
}