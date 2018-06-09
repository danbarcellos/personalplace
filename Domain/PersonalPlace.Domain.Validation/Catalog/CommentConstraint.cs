using FluentValidation;
using Pactor.Infra.DAL.ORM;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Catalog
{
    public class CommentConstraint : Constraint<Comment>
    {
        public CommentConstraint()
        {
            RuleFor(x => x.Realty)
                .NotNull()
                   .WithMessage("Imóvel não informado");
            RuleFor(x => x.Client)
                .NotNull()
                   .WithMessage("Client não informado");
            RuleFor(x => x.Client)
                .Must((comment, mention) => mention.Id != Entity.UnsavedId && mention.Id != comment.Id)
                   .WithMessage("Réplica de commentário recursiva");
            RuleFor(x => x.DateTime)
                .SetValidator(new SystemDateTimeOffsetInThePastPropertyValidator())
                   .WithMessage("Data de commentário inválida")
                .Must((comment, dateTime) => comment.Mention is null || comment.Mention.DateTime <= dateTime)
                   .WithMessage("Data de comentário é anterior à data do comentário mensionado");
            RuleFor(x => x.Text)
                .NotNull()
                   .WithMessage("Texto de comentário inexistente")
                .Length(1, 8000)
                   .WithMessage("Texto de comentário não contém o mínimo de caracteres ou é maior que o máximo permitido");
        }
    }
}