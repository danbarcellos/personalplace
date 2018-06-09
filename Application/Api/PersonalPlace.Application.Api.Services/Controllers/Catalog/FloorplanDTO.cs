using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    public class FloorplanDTO
    {
        public virtual string Description { get; set; }

        public virtual double Dimension { get; set; }

        public virtual MeasureUnit MesureUnit { get; set; }
    }
}