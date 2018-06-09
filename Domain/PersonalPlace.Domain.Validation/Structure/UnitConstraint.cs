using FluentValidation;
using PersonalPlace.Domain.Entities.Structure;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Structure
{
    public class UnitConstraint : Constraint<Unit>
    {
        public UnitConstraint()
        {
            RuleFor(x => x.UpperUnit)
                .Must(unit => unit.UpperUnit == null || unit.UpperUnit.Id != unit.Id)
                   .WithMessage("A unidade não pode ter por unidade superior ela mesma");
            RuleFor(x => x.ExternalId)
                .NotNull()
                   .WithMessage("Id externo da unidade não informado")
                .Length(1, 50)
                   .WithMessage("Id externo não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Description)
                .NotNull()
                   .WithMessage("Descricao da unidade não informado")
                .Length(2, 60)
                   .WithMessage("Descricao da unidade contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Culture)
                .NotNull()
                   .WithMessage("Cultura da unidade não informado")
                .Length(2, 5)
                   .WithMessage("Cultura da unidade contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.PhotoFilePrefix)
                .Length(1, 80)
                   .WithMessage("Prefixo de imagem da unidade não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.State)
                .NotNull()
                   .WithMessage("Estado da unidade não informado");
        }
    }
}