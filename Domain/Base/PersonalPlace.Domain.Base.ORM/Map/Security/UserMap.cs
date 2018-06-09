using NHibernate.Mapping.ByCode.Conformist;
using PersonalPlace.Domain.Entities.Security;

namespace PersonalPlace.Domain.Base.ORM.Map.Security
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Property(x => x.Email, m => m.Unique(true));
            Property(x => x.ExternalId, m => m.Unique(true));
        }
    }
}