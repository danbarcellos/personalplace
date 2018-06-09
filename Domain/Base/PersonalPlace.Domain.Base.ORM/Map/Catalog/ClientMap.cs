using NHibernate.Mapping.ByCode.Conformist;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Base.ORM.Map.Catalog
{
    public class ClientMap : ClassMapping<Client>
    {
        public ClientMap()
        {
            ManyToOne(x => x.User, m => m.Unique(true));
        }
    }
}