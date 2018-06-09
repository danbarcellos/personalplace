
namespace PersonalPlace.Domain.Base
{
    public interface IDomainContext
    {
        string ClientToken { get; }

        string ScopeTag { get; }

        string Culture { get; }

        IUserContext UserContext { get; }
    }
}