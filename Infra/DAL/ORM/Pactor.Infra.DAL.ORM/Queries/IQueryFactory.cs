namespace Pactor.Infra.DAL.ORM.Queries
{
    public interface IQueryFactory
    {
        TQuery CreateQuery<TQuery>() where TQuery : IQuery;
    }
}
