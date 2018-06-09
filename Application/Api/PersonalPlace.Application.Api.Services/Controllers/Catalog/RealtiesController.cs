using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalPlace.Domain.Contract.Queries.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class RealtiesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRealtyById _realtyByIdQuery;
        private readonly IRealtiesPage _realtiesPageQuery;

        public RealtiesController(IMapper mapper, IRealtyById realtyByIdQuery, IRealtiesPage realtiesPageQuery)
        {
            _mapper = mapper;
            _realtyByIdQuery = realtyByIdQuery;
            _realtiesPageQuery = realtiesPageQuery;
        }

        [HttpGet("{realtyId}")]
        public RealtyDTO Get(Guid realtyId)
        {
            if (realtyId == default(Guid))
                return null;

            _realtyByIdQuery.RealtyId = realtyId;
            var realty = _realtyByIdQuery.Execute();

            var realtyDTO = _mapper.Map<Realty, RealtyDTO>(realty);

            return realtyDTO;
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
    }
}