using System.Linq;
using NHibernate;
using NHibernate.Transform;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Contract.Queries.Catalog;

namespace PersonalPlace.Domain.Queries.Catalog
{
    public class RealtyByDistance : NamedQueryBase<RealtyDistanceDto[]>, IRealtyByDistance
    {
        public RealtyByDistance(IQueryMachine queryMachine) : base(queryMachine) { }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Distance { get; set; }

        protected override RealtyDistanceDto[] Execute(IQuery query)
        {
            return query.List<RealtyDistanceDto>().ToArray();
        }

        protected override void SetParameters(IQuery nhQuery)
        {
            nhQuery.SetDouble("lat", Latitude);
            nhQuery.SetDouble("lon", Longitude);
            nhQuery.SetInt32("distance", Distance);
            nhQuery.SetResultTransformer(Transformers.AliasToBean<RealtyDistanceDto>());
            nhQuery.SetTimeout(120);
        }
    }
}