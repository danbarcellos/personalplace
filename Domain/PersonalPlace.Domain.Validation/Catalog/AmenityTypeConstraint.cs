using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class AmenityTypeConstraint : Constraint<AmenityType>
    {
        public AmenityTypeConstraint()
        {
            RuleFor(x => x.Name)
                .NotNull()
                   .WithMessage("Detalhe de endereço da unidade não informado")
                .Length(1, 60)
                   .WithMessage("Detalhe de endereço não contém o mínimo de caracteres ou é maior que o máximo permitido");
        }
    }
}