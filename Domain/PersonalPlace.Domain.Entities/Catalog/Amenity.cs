using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Entities.Catalog
{
    public class Amenity : ScopedEntity
    {
        protected Amenity() { }

        public Amenity(Realty realty, AmenityType amenityType, string description, string scopeTag = null) : base(scopeTag ?? realty.ScopeTag)
        {
            Realty = realty;
            AmenityType = amenityType;
            Description = description;
        }

        public virtual Realty Realty { get; protected set; }

        public virtual AmenityType AmenityType { get; protected set; }

        public virtual string Description { get; set; }
    }
}