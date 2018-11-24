using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Queries.Catalog
{
    public class ClientById : NHibernateQueryBase<Client>, IClientById
    {
        public ClientById(IQueryMachine queryMachine) : base(queryMachine) { }

        public Guid ClientId { get; set; }

        public override Client Execute()
        {
            return QueryMachine.Execute(() =>
            {
                var realty = (from cli in QueryMachine.Query<Client>()
                              where cli.Id == ClientId
                              select cli)
                             .Fetch(x => x.User)
                             .SingleOrDefault();
                
                return realty;
            });
        }
    }
}