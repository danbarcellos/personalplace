using System.Linq;
using AutoMapper;
using PersonalPlace.Application.Api.Services.Controllers.Catalog;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Application.Api.Services
{
    public class ServicesObjectMapperProfile : Profile
    {
        public ServicesObjectMapperProfile()
        {
            CreateMap<Realty, RealtyDTO>()
                .ForMember(x => x.Images, mc => mc.ResolveUsing((r, rdto) =>
                {
                    var urls = r.Images.Select(i => i.Url);
                    return urls;
                }));
        }
    }
}