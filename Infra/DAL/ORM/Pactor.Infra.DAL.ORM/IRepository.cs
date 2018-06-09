using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pactor.Infra.Crosscutting.Exceptions;

namespace Pactor.Infra.DAL.ORM
{
    public interface IRepository<T> : IEnumerable<T> where T : class, IEntity
    {
        int Count { get; }
        bool Contains(T entity);
        void SaveAll<TEntity>(TEntity entity, [CallerMemberName] string memberName = null, [CallerLineNumber] int sourceLineNumber = 0) where TEntity : class, T;
        void Save<TEntity>(TEntity entity, [CallerMemberName] string memberName = null, [CallerLineNumber] int sourceLineNumber = 0) where TEntity : class, T;
        bool Remove(T entity);
        InvalidValue[] GetInvalidValues<TEntity>(TEntity entity) where TEntity : T;
        T FindOne(object id);
        T Load(object id);
        TEntity FindOne<TEntity>(object id) where TEntity : class, T;
    }
}
