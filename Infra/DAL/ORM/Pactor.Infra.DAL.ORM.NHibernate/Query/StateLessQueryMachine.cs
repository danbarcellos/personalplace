using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public class StateLessQueryMachine : NHibernateStatelessBase, IQueryMachine
    {
        public StateLessQueryMachine(IStatelessSession session) : base(session) { }

        public IQueryable<TEntity> Query<TEntity>()
        {
            return Session.Query<TEntity>();
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return Session.QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return Session.QueryOver(alias);
        }

        public IQuery GetNamedQuery(string queryName)
        {
            return Session.GetNamedQuery(queryName);
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            return Session.CreateCriteria<T>();
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return Session.CreateCriteria<T>(alias);
        }

        public ICriteria CreateCriteria(string entityName)
        {
            return Session.CreateCriteria(entityName);
        }

        public ICriteria CreateCriteria(Type entityType)
        {
            return Session.CreateCriteria(entityType);
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return Session.CreateCriteria(entityName, alias);
        }

        public ICriteria CreateCriteria(Type entityType, string alias)
        {
            return Session.CreateCriteria(entityType, alias);
        }

        public IQuery CreateQuery(string queryString)
        {
            return Session.CreateQuery(queryString);
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return Session.CreateSQLQuery(queryString);
        }

        public TResult Execute<TResult>(Func<TResult> action)
        {
            return Transact(action);
        }

        public global::NHibernate.Dialect.Dialect Dialect { get { return Session.GetSessionImplementation().Factory.Dialect; } }

        public bool IsStateless { get; } = true;

        public T GetInnerSession<T>() where T : class
        {
            return Session.GetSessionImplementation() as T;
        }

        public bool HasMultipleQueriesInProgress()
        {
            return false;
        }
    }
}