using System;
using System.Data;
using Pactor.Infra.Crosscutting.Exceptions;
using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM
{
    public interface IStatelessUnitOfWork : IQueryFactory, IDisposable
    {
        IDbConnection Connection { get; }
        bool IsConnected { get; }
        bool IsOpen { get; }
        void Commit();
        ITransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted);
        T Get<T>(object id);
        void Insert(object entity);
        void Update(object entity);
        void Delete(object entity);
        void Refresh(object entity);
        void Close();
        void SetBatchSize(int batchSize);
        InvalidValue[] GetInvalidValues<TEntity>(TEntity item);
        void Flush();
    }
}