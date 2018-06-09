using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Domain.Entities.Catalog
{
    public class Floorplan : ScopedEntity
    {
        protected Floorplan() { }

        public Floorplan(Realty realty, double dimension, string description, MeasureUnit mesureUnit = MeasureUnit.SquareMeters, 
            string scopeTag = null) 
            : base(scopeTag ?? realty.ScopeTag)
        {
            Realty = realty;
            Dimension = dimension;
            Description = description;
            MesureUnit = mesureUnit;
        }

        public virtual Realty Realty { get; protected set; }

        public virtual string Description { get; set; }

        public virtual double Dimension { get; set; }

        public virtual MeasureUnit MesureUnit { get; set; }
    }
}