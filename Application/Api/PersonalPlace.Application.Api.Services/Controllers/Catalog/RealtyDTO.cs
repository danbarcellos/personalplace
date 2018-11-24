using System;
using System.Collections.Generic;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    public class RealtyDTO
    {
        public Guid ClientId { get; set; }

        //public string ClientName { get; set; }

        public Address Address { get; set; }

        public string AddressDetail { get; set; }

        public string Description { get; set; }

        public bool Furnished { get; set; }

        public bool DisabilityAccess { get; set; }

        public int? TotalRooms { get; set; }

        public int? TotalSuites { get; set; }

        public int? Age { get; set; }

        public decimal? RentValue { get; set; }

        public decimal? SaleValue { get; set; }

        public DateTimeOffset PostDateTime { get; set; }

        public IEnumerable<FloorplanDTO> Floorplans { get; set; }

        public IEnumerable<AmenityDTO> Amenities { get; set; }

        public IEnumerable<CommentDTO> Comments { get; set; }

        public IEnumerable<string> Images { get; set; }
    }
}