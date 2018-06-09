using System.Linq;
using NHibernate.Linq;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Contract.Queries.Security;
using PersonalPlace.Domain.Entities.Security;

namespace PersonalPlace.Domain.Queries.Security
{
    public class UserByEmail : NHibernateQueryBase<User>, IUserByEmail
    {
        public UserByEmail(IQueryMachine queryMachine) : base(queryMachine) { }

        public string Email { get; set; }

        public override User Execute()
        {
            return QueryMachine.Execute(() =>
            {
                var user = (from usu in QueryMachine.Query<User>()
                            where usu.Email == Email
                            select usu)
                           .FetchMany(x => x.Authorizations)
                           .ThenFetch(x => x.Unit)
                           .SingleOrDefault();

                return user;
            });
        }
    }
}
