using System;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Contract.Queries.Catalog
{
    public class RealtyDistanceDto
    {
        public Guid RealtyId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AddressLine { get; set; }
        public string AddressDetail { get; set; }
        public decimal? RentValue { get; set; }
        public decimal? SaleValue { get; set; }
        public float Distance { get; set; }
    }
}