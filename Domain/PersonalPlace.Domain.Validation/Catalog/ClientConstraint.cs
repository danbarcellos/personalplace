using FluentValidation;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class ClientConstraint : Constraint<Client>
    {
        public ClientConstraint()
        {
            //RuleFor(x => x.User)
            //    .NotNull()
            //       .WithMessage("Usuário do cliente não informado");
            RuleFor(x => x.Telephone)
                .Length(5, 30)
                   .WithMessage("Telefone não contém o mínimo de caracteres ou é maior que o máximo permitido");
        }
    }
}