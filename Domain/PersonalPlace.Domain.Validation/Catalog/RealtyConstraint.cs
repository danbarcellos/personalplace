using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class RealtyConstraint : Constraint<Realty>
    {
        public RealtyConstraint()
        {
            RuleFor(x => x.Client)
                .NotNull()
                   .WithMessage("Cliente não informado");
            RuleFor(x => x.Address)
                .NotNull()
                   .WithMessage("Endereço não informado")
                .SetValidator(new AddressConstraint());
            RuleFor(x => x.AddressDetail)
                .NotNull()
                   .WithMessage("Detalhe de endereço da unidade não informado")
                .Length(1, 140)
                   .WithMessage("Detalhe de endereço não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Description)
                .Length(1, 8000)
                   .WithMessage("Descrição não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.TotalRooms)
                .InclusiveBetween(1, 9999)
                   .WithMessage("Total de quartos não contém o valor mínimo ou é maior que o valor máximo permitido");
            RuleFor(x => x.TotalSuites)
                .InclusiveBetween(1, 9999)
                   .WithMessage("Total de suítes não contém o valor mínimo ou é maior que o valor máximo permitido");
            RuleFor(x => x.Age)
                .InclusiveBetween(1, 2000)
                   .WithMessage("Idade do imóvel não contém o valor mínimo ou é maior que o valor máximo permitido");
            RuleFor(x => x.PostDateTime)
                .SetValidator(new SystemDateTimeOffsetInThePastPropertyValidator())
                   .WithMessage("Data de postagem inválida");
            RuleFor(x => x.Amenities)
                .SetCollectionValidator(new AmenityConstraint());
        }
    }
}