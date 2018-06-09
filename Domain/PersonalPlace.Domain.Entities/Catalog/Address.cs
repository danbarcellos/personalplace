using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Entities.Component;

namespace PersonalPlace.Domain.Entities.Catalog
{
    public class Address : ScopedEntity
    {

        protected Address() { }

        public Address(string addressLine, 
                       string neighborhood, 
                       string postcode, 
                       string state, 
                       string country, 
                       GeoCoordinate coordinate, 
                       string scopeTag = null) : base (scopeTag ?? DomainContextConstants.DefaultScopeTag)
        {
            AddressLine = addressLine;
            Neighborhood = neighborhood;
            Postcode = postcode;
            State = state;
            Country = country;
            Coordinate = coordinate;
        }

        public virtual string AddressLine { get; set; }

        public virtual string Neighborhood { get; set; }

        public virtual string PopulatedPlace { get; set; }

        public virtual string Postcode { get; set; }

        public virtual string AdminDivision1 { get; set; }

        public virtual string AdminDivision2 { get; set; }

        public virtual string State { get; set; }

        public virtual string CountryRegion { get; set; }

        public virtual string Country { get; set; }

        public virtual GeoCoordinate Coordinate { get; set; }
    }
}