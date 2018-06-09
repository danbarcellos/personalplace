using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class AmenityConstraint : Constraint<Amenity>
    {
        public AmenityConstraint()
        {
            RuleFor(x => x.Realty)
                .NotNull()
                   .WithMessage("Imóvel não informado");
            RuleFor(x => x.Description)
                .NotNull()
                   .WithMessage("Descrição de endereço não informado")
                .Length(1, 8000)
                   .WithMessage("Descrição de endereço não contém o mínimo de caracteres ou é maior que o máximo permitido");
        }
    }
}