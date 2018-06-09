using System;
using System.Data;
using System.Linq;

namespace Pactor.Infra.DAL.ORM
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        FlushMode FlushMode { get; set; }
        void Flush();
        void Refresh(IEntity entity);
        T Load<T>(object id);
        void Load(IEntity entity, object id);
        TEntidade Merge<TEntidade>(TEntidade entidade) where TEntidade : class, IEntity;
        void Evict(object obj);
        void Clear();
        void SetBatchSize(int batchSize);
        T UnProxy<T>(T entidade) where T : IEntity;
        ITransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted);
        TResult ExecuteSealed<TSealedOperation, TResult>(params object[] args) where TSealedOperation : ISealedOperation<TResult>;
        IQueryable<TEntity> GetQueryable<TEntity>();
        TQuery GetQuery<TQuery>() where TQuery : Queries.IQuery;
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
    }
}
