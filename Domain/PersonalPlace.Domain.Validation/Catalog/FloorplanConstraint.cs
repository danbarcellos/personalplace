using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class FloorplanConstraint : Constraint<Floorplan>
    {
        public FloorplanConstraint()
        {
            RuleFor(x => x.Realty)
                .NotNull()
                   .WithMessage("Imóvel da planta não informado");
            RuleFor(x => x.Description)
                .NotNull()
                   .WithMessage("Descrição da planta não informado")
                .Length(1, 8000)
                   .WithMessage("Descrição da planta não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Dimension)
                .GreaterThan(0)
                   .WithMessage("Medidas da planta inválido");
        }
    }
}