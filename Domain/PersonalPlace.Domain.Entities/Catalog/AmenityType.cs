using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Entities.Catalog
{
    [EntityParameter(Segregation.Parenthood, isAggregateRoot: true)]
    public class AmenityType : ScopedEntity
    {
        protected AmenityType()
        {
        }

        public AmenityType(string name, string scopeTag) : base(scopeTag)
        {
            Name = name;
        }

        public virtual string Name { get; protected set; }
    }
}