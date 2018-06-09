using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Impl;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public class QueryMachine : NHibernateBase, IQueryMachine
    {
        private readonly ISession _session;

        public QueryMachine(ISession session) : base(session)
        {
            _session = session;
        }

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
            return _session.GetNamedQuery(queryName);
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            return _session.CreateCriteria<T>();
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return _session.CreateCriteria<T>(alias);
        }

        public ICriteria CreateCriteria(string entityName)
        {
            return _session.CreateCriteria(entityName);
        }

        public ICriteria CreateCriteria(Type entityType)
        {
            return _session.CreateCriteria(entityType);
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return _session.CreateCriteria(entityName, alias);
        }

        public ICriteria CreateCriteria(Type entityType, string alias)
        {
            return _session.CreateCriteria(entityType, alias);
        }

        public IQuery CreateQuery(string queryString)
        {
            return _session.CreateQuery(queryString);
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _session.CreateSQLQuery(queryString);
        }

        public TResult Execute<TResult>(Func<TResult> action)
        {
            return Transact(action);
        }

        public global::NHibernate.Dialect.Dialect Dialect { get { return Session.GetSessionImplementation().Factory.Dialect; } }

        public bool IsStateless { get; } = false;

        public T GetInnerSession<T>() where T : class
        {
            return Session.GetSessionImplementation() as T;
        }

        public bool HasMultipleQueriesInProgress()
        {
            var sessionImpl = _session as AbstractSessionImpl;
            return sessionImpl.HasMultipleQueriesInProgress();
        }
    }
}