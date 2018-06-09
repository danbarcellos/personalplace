using System;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    public class AmenityDTO
    {
        public virtual Guid AmenityTypeId { get; protected set; }

        public virtual string AmenityTypeName { get; protected set; }

        public virtual string Description { get; set; }
    }
}