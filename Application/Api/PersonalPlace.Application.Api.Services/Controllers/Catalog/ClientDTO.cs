using System;
using System.Collections.Generic;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    public class ClientDTO
    {
        public Guid? ClientId { get; set; }

        public UserDTO User { get; set; }

        public string Telephone { get; set; }
    }
}