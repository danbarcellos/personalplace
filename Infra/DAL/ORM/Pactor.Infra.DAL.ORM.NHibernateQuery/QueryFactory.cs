using Pactor.Infra.Crosscutting.IoC;
using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM.NHibernateQuery
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IContainer _container;

        public QueryFactory(IContainer serviceLocator)
        {
            _container = serviceLocator;
        }

        public TQuery CreateQuery<TQuery>() where TQuery : IQuery
        {
            var query = _container.Resolve<TQuery>();
            return query;
        }
    }
}
