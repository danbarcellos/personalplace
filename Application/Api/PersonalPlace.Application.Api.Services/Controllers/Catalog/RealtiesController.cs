using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pactor.Infra.Crosscutting.Log;
using Pactor.Infra.DAL.ORM;
using PersonalPlace.Domain.Common.Exception;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class RealtiesController : Controller
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IRealtyById _realtyByIdQuery;
        private readonly IRealtiesPage _realtiesPageQuery;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRealtyByDistance _realtyByDistanceQuery;

        public RealtiesController(ILog logger, 
                                  IMapper mapper, 
                                  IRealtyById realtyByIdQuery, 
                                  IRealtiesPage realtiesPageQuery,
                                  IRealtyByDistance realtyByDistanceQuery,
                                  IRepository<Client> clientRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _realtyByIdQuery = realtyByIdQuery;
            _realtiesPageQuery = realtiesPageQuery;
            _clientRepository = clientRepository;
            _realtyByDistanceQuery = realtyByDistanceQuery;
        }

        [HttpGet("{realtyId}")]
        public RealtyDTO Get(Guid realtyId)
        {
            if (realtyId == default(Guid))
                return null;

            try
            {
                _realtyByIdQuery.RealtyId = realtyId;
                var realty = _realtyByIdQuery.Execute();

                var realtyDto = _mapper.Map<Realty, RealtyDTO>(realty);

                return realtyDto;
            }
            catch (Exception e)
            {
                _logger.Error(() => $"Error getting realty by id", e);
                return null;
            }
        }

        [HttpGet("{lat}/{lon}/{dist}")]
        public RealtyDistanceDto[] Get(string lat, string lon, string dist)
        {
            if (string.IsNullOrWhiteSpace(lat) 
                || string.IsNullOrWhiteSpace(lon) 
                || string.IsNullOrWhiteSpace(dist)
                || !double.TryParse(lat, out var latitude)
                || !double.TryParse(lon, out var longitude)
                || !int.TryParse(dist, out var distance))
                return Array.Empty<RealtyDistanceDto>();


            try
            {
                _realtyByDistanceQuery.Latitude = latitude;
                _realtyByDistanceQuery.Longitude = longitude;
                _realtyByDistanceQuery.Distance = distance;
                var realtyDistanceDtos = _realtyByDistanceQuery.Execute();
                return realtyDistanceDtos;
            }
            catch (Exception e)
            {
                _logger.Error(() => $"Error getting realty by distance", e);
                return Array.Empty<RealtyDistanceDto>();
            }
        }

        [HttpGet]
        public RealtyDTO[] Get()
        {
            _realtiesPageQuery.PageNumber = 1;
            _realtiesPageQuery.TotalPerPage = 10;
            var realties = _realtiesPageQuery.Execute();
            var realtyDtos = _mapper.Map<Realty[], RealtyDTO[]>(realties);

            return realtyDtos;
        }

        [HttpPost]
        public void Post([FromBody] RealtyDTO realtyDto)
        {
            if (realtyDto == null)
                throw new DomainArgumentException("Invalid realty data");

            var client = _clientRepository.FindOne(realtyDto.ClientId);

            if (client == null)
                throw new DomainArgumentException("Invalid client id");

            var realty = _mapper.Map<RealtyDTO, Realty>(realtyDto);

            client.AddRealty(realty);
            _clientRepository.SaveAll(client);
        }
    }
}