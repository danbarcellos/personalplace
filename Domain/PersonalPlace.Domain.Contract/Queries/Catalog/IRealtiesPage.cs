using Pactor.Infra.DAL.ORM.Queries;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Contract.Queries.Catalog
{
    public interface IRealtiesPage : IQuery<Realty[]>
    {
        int PageNumber { get; set; }

        int TotalPerPage { get; set; }
    }
}