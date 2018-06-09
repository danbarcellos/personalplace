using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NHibernate;
using Pactor.Infra.Crosscutting.IoC;
using IQuery = Pactor.Infra.DAL.ORM.Queries.IQuery;
using IsolationLevel = System.Data.IsolationLevel;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHUnitOfWork : NHibernateBase, IUnitOfWork
    {
        private readonly IContainer _container;
        private readonly IDisposable _disposable;

        public NHUnitOfWork(ISession session, IContainer container, IDisposable disposable = null) : base(session)
        {
            _container = container;
            _disposable = disposable;
        }

        public virtual ITransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return new NHTransaction(Session.Transaction.IsActive ? Session.Transaction : Session.BeginTransaction(isolation));
        }

        public virtual void Commit()
        {
            if (!Session.IsOpen)
                return;

            if (Session.Transaction.IsActive)
                Session.Transaction.Commit();
        }

        public FlushMode FlushMode
        {
            get { return (FlushMode)(int)Session.FlushMode; }
            set { Session.FlushMode = (global::NHibernate.FlushMode)(int)value; }
        }

        public virtual void Flush()
        {
            Transact(() => Session.Flush());
        }

        public virtual TEntidade Merge<TEntidade>(TEntidade entidade) where TEntidade : class, IEntity
        {
            return Transact(() => Session.Merge(entidade));
        }

        public virtual void Refresh(IEntity entity)
        {
            Transact(() => Session.Refresh(entity));
        }

        public virtual T Load<T>(object id)
        {
            return Session.Load<T>(id);
        }

        public virtual void Load(IEntity entity, object id)
        {
            Session.Load(entity, id);
        }

        public virtual void Evict(object obj)
        {
            Session.Evict(obj);
        }

        public virtual void Clear()
        {
            Session.Clear();
        }

        public virtual void SetBatchSize(int batchSize)
        {
            Session.SetBatchSize(batchSize);
        }

        public T UnProxy<T>(T entidade) where T : IEntity
        {
            return (T)Session.GetSessionImplementation().PersistenceContext.Unproxy(entidade);
        }

        public virtual TResult ExecuteSealed<TSealedOperation, TResult>(params object[] args) where TSealedOperation : ISealedOperation<TResult>
        {
            var result = default(TResult);
            var expectedException = (Type)null;
            var attempts = 1;
            var mergeResult = true;
            var sealedOperationOption = (SealedOperationOptionAttribute)typeof(TSealedOperation)
                                         .GetCustomAttributes(typeof(SealedOperationOptionAttribute), false)
                                         .FirstOrDefault();

            if (sealedOperationOption != null)
            {
                expectedException = sealedOperationOption.ExpectedException;
                attempts = sealedOperationOption.Attempts;
                mergeResult = sealedOperationOption.MergeResult;
            }

            var totalAttempts = 0;
            var rootContainer = _container.ResolveNamed<IContainer>(ContainerTag.Root);
            while (totalAttempts < attempts)
            {
                using (var scope = rootContainer.BeginLifetimeScope())
                {
                    try
                    {
                        using (var txs = new TransactionScope(TransactionScopeOption.Suppress))
                        using (var session = scope.Resolve<ISession>())
                        using (var tx = session.BeginTransaction())
                        {
                            var sealedOperation = scope.Resolve<TSealedOperation>(new TypedParameter(typeof(IContainer), scope));
                            totalAttempts++;
                            result = sealedOperation.Execute(args);
                            tx.Commit();
                            txs.Complete();
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == expectedException)
                            continue;

                        throw;
                    }
                }
            }

            if (mergeResult)
            {
                if (result is IEntity)
                    return (TResult)Session.Merge((object)result);

                var resultType = typeof (TResult);
                if (resultType != typeof(string) 
                    && (resultType.IsGenericType 
                        && resultType.GetGenericTypeDefinition() == typeof(IEnumerable<>) 
                        &&  typeof(IEntity).IsAssignableFrom(resultType.GetGenericArguments()[0])))
                {
                    return (TResult)((IEnumerable<object>)result).Select(entity => Session.Merge(entity));
                }
            }

            return result;
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>()
        {
            return Session.Query<TEntity>();
        }

        public virtual TQuery GetQuery<TQuery>() where TQuery : IQuery
        {
            return _container.Resolve<TQuery>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            return _container.Resolve<IRepository<TEntity>>();
        }

        internal virtual object GetEncapsulatedUnityOfWork()
        {
            return Session;
        }

        public override int GetHashCode()
        {
            return Session.GetHashCode();
        }

        public void Dispose()
        {
            Session.Dispose();
            _disposable?.Dispose();
        }
    }
}
