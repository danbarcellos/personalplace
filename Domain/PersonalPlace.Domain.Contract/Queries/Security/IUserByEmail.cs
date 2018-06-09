using Pactor.Infra.DAL.ORM.Queries;
using PersonalPlace.Domain.Entities.Security;

namespace PersonalPlace.Domain.Contract.Queries.Security
{
    public interface IUserByEmail : IQuery<User>
    {
        string Email { get; set; }
    }
}