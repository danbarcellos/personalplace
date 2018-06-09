using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Base.ORM.Map.Catalog
{
    public class RealtyMap : ClassMapping<Realty>
    {
        public RealtyMap()
        {
            ManyToOne(x => x.Address, m => m.Cascade(Cascade.All.Include(Cascade.DeleteOrphans)));
        }
    }
}