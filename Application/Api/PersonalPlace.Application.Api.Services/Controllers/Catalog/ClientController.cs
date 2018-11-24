using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pactor.Infra.DAL.ORM;
using PersonalPlace.Domain.Common.Exception;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClientById _clientById;
        private readonly IRealtiesPage _realtiesPageQuery;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<Realty> _realtyRepository;

        public ClientController(IMapper mapper, 
                                IRepository<Client> clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        [HttpGet("{clientId}")]
        public ClientDTO Get(Guid clientId)
        {
            if (clientId == default(Guid))
                return null;

            _clientById.ClientId = clientId;
            var realty = _clientById.Execute();

            var clientDto = _mapper.Map<Client, ClientDTO>(realty);

            return clientDto;
        }

        [HttpPost]
        public void Post([FromBody] ClientDTO clientDto)
        {
            if (clientDto == null)
                throw new DomainArgumentException("Invalid client data");

            Client client;
            if (clientDto.ClientId != null)
            {
                client = _clientRepository.FindOne(clientDto.ClientId);

                if (client == null)
                    throw new DomainArgumentException("Invalid client id");
            }
            else
            { 
                client = _mapper.Map<ClientDTO, Client>(clientDto);
            }

            _clientRepository.SaveAll(client);
        }
    }
}