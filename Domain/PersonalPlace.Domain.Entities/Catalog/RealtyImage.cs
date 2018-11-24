using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Entities.Catalog
{
    [EntityParameter(Segregation.Descending)]
    public class RealtyImage : ScopedEntity
    {
        protected RealtyImage() { }

        public RealtyImage(string url, string scopeTag) : base(scopeTag)
        {
            Url = url;
        }

        public virtual string Url { get; protected set; }

        public override string ToString()
        {
            return Url;
        }
    }
}