namespace Pactor.Infra.DAL.ORM
{
    public interface IPreInsertInterceptor
    {
        string Description { get; }

        InterceptorResult OnPreInsert<TEntity>(TEntity item) where TEntity : Entity;
    }
}