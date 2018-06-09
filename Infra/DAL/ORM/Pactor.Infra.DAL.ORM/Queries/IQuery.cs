using System;

namespace Pactor.Infra.DAL.ORM.Queries
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
        TResult Execute();
    }

    public interface IQueryFuture<TResult> : IQuery<TResult>
    {
        Lazy<TResult> ExecuteFuture();
    }
}
