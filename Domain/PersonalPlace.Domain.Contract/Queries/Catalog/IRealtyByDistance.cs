using Pactor.Infra.DAL.ORM.Queries;

namespace PersonalPlace.Domain.Contract.Queries.Catalog
{
    public interface IRealtyByDistance : IQuery<RealtyDistanceDto[]>
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        int Distance { get; set; }
    }
}