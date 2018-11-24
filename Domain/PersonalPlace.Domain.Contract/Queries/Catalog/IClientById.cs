using System;
using Pactor.Infra.DAL.ORM.Queries;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Contract.Queries.Catalog
{
    public interface IClientById : IQuery<Client>
    {
        Guid ClientId { get; set; }
    }
}