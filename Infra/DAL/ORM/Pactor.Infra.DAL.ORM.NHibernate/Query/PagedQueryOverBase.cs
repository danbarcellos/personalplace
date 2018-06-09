using NHibernate;
using Pactor.Infra.DAL.ORM.Queries;

namespace Pactor.Infra.DAL.ORM.NHibernate.Query
{
    public abstract class PagedQueryOverBase<T> : NHibernateQueryBase<PagedResult<T>>, IPagedQuery<T>
    {
        protected PagedQueryOverBase(IQueryMachine queryMachine) : base(queryMachine)
        {
        }

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public override PagedResult<T> Execute()
        {
            var query = GetQuery();
            SetPaging(query);
            return QueryMachine.Execute(() => Execute(query));
        }

        protected abstract IQueryOver<T, T> GetQuery();

        protected virtual void SetPaging(IQueryOver<T, T> query)
        {
            var maxResults = ItemsPerPage;
            var firstResult = (PageNumber - 1) * ItemsPerPage;
            query.Skip(firstResult).Take(maxResults);
        }

        protected virtual PagedResult<T> Execute(IQueryOver<T, T> query)
        {
            var results = query.Future<T>();
            var count = query.ToRowCountQuery().FutureValue<int>();
            return new PagedResult<T>
            {
                PageOfResults = results,
                TotalItems = count.Value
            };
        }
    }
}
