using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Queries.Catalog
{
    public class RealtyById : NHibernateQueryBase<Realty>, IRealtyById
    {
        public RealtyById(IQueryMachine queryMachine) : base(queryMachine) { }

        public Guid RealtyId { get; set; }

        public override Realty Execute()
        {
            return QueryMachine.Execute(() =>
            {
                var realty = (from rlty in QueryMachine.Query<Realty>()
                              where rlty.Id == RealtyId
                              select rlty)
                             .Fetch(x => x.Client)
                                .ThenFetch(x => x.User)
                             .Fetch(x => x.Address)
                             .FetchMany(x => x.States)
                             .SingleOrDefault();

                if (!QueryMachine.IsStateless && realty != null)
                {
                    NHibernateUtil.Initialize(realty.Amenities);
                    NHibernateUtil.Initialize(realty.Floorplans);
                    NHibernateUtil.Initialize(realty.Comments);
                }

                return realty;
            });
        }
    }
}