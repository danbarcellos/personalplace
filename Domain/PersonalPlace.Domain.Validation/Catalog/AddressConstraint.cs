using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class AddressConstraint : Constraint<Address>
    {
        public AddressConstraint()
        {
            RuleFor(x => x.AddressLine)
                .NotNull()
                   .WithMessage("Linha de endereço da unidade não informado")
                .Length(1, 140)
                   .WithMessage("Linha de endereço não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Neighborhood)
                .NotNull()
                   .WithMessage("Bairro não informado")
                .Length(2, 60)
                   .WithMessage("Bairro não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.PopulatedPlace)
                .Length(2, 60)
                   .WithMessage("Logradouro não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Postcode)
                .NotNull()
                   .WithMessage("CEP não informado")
                .Length(2, 10)
                   .WithMessage("CEP não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.AdminDivision1)
                .Length(2, 60)
                   .WithMessage("Divisão administrativa 1 não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.AdminDivision2)
                .Length(2, 60)
                   .WithMessage("Divisão administrativa 2 não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.State)
                .Length(2, 60)
                   .WithMessage("Divisão administrativa 2 não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.CountryRegion)
                .Length(2, 60)
                   .WithMessage("Região não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Country)
                .NotNull()
                   .WithMessage("País não informado")
                .Length(2, 30)
                   .WithMessage("País não contém o mínimo de caracteres ou é maior que o máximo permitido");
            //RuleFor(x => x.Coordinate)
            //    .Must((_, coord) => coord.Longitude > -180 && coord.Longitude < 180 && coord.Latitude > -90 && coord.Latitude < 90)
            //       .WithMessage("Coordenadas estão fora da faixa de valores válidos");
        }
    }
}