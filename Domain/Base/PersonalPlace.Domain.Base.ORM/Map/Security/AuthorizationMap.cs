using NHibernate.Mapping.ByCode.Conformist;
using PersonalPlace.Domain.Base.CustomType;
using PersonalPlace.Domain.Entities.Security;

namespace PersonalPlace.Domain.Base.ORM.Map.Security
{
    public class AuthorizationMap : ClassMapping<Authorization>
    {
        private const string UserUnitUniqueKey = "userUnitUniqueKey";

        public AuthorizationMap()
        {
            ManyToOne(x => x.User, m => m.UniqueKey(UserUnitUniqueKey));
            ManyToOne(x => x.Unit, m => m.UniqueKey(UserUnitUniqueKey));
            Property("RolesPersistent", m =>
            {
                m.NotNullable(true);
                m.Type<EncryptedLargeString>();
            });
        }
    }
}