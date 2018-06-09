using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Queries.Catalog
{
    public class RealtiesPageId : NHibernateQueryBase<Realty[]>, IRealtiesPage
    {
        public RealtiesPageId(IQueryMachine queryMachine) : base(queryMachine)
        {
        }

        public int PageNumber { get; set; }

        public int TotalPerPage { get; set; }

        public override Realty[] Execute()
        {
            return QueryMachine.Execute(() =>
            {
                var realties = (from rlty in QueryMachine.Query<Realty>()
                        select rlty)
                    //.Skip(PageNumber * TotalPerPage)
                    //.Take(TotalPerPage)
                    .Fetch(x => x.Client)
                    .ThenFetch(x => x.User)
                    .Fetch(x => x.Address)
                    .FetchMany(x => x.States);

                if (!QueryMachine.IsStateless && realties != null)
                {
                    foreach (var realty in realties)
                    {
                        NHibernateUtil.Initialize(realty.Amenities);
                        NHibernateUtil.Initialize(realty.Floorplans);
                        NHibernateUtil.Initialize(realty.Comments);
                    }
                }

                return realties?.ToArray();
            });
        }
    }
}