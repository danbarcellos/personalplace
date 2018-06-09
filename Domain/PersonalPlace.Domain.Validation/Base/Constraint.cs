using FluentValidation;
using FluentValidation.Results;
using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Validation.Base
{
    public abstract class Constraint<T> : AbstractValidator<T> where T : ScopedEntity
    {
        private DomainConstraint<T>[] _domainConstraints = null;
        private const int ScopeTagMinSize = 2;
        private const int ScopeTagMaxSize = 400;

        protected Constraint()
        {
            RuleFor(x => x.ScopeTag)
                .NotNull()
                   .WithMessage("ScopeTag não informada em " + typeof(T).Name + " Id [{0}].", x => x.Id.ToString("D"))
                .Length(ScopeTagMinSize, ScopeTagMaxSize)
                   .WithMessage("ScopeTag de tamanho inválido em " + typeof(T).Name + " Id [{0}]: Tamanho = {1} (tamanho válido entre " + ScopeTagMinSize + " e " + ScopeTagMaxSize + ")", x => x.Id.ToString("D"), x => x.ScopeTag.Length)
                .Matches(@"^\.\.$|^([0-9]{1,3}\.)*$")
                   .WithMessage("ScopeTag inválida em " + typeof(T).Name + " Id [{0}]: \"{1}\"", x => x.Id.ToString("D"), x => x.ScopeTag);

            Custom(IsValidDomainEntity);
        }

        private ValidationFailure IsValidDomainEntity(T entity)
        {
            if (_domainConstraints == null)
            {
                //todo: melhor usar um scope de container para não contaminar o root container.
                //_domainConstraints = Registry.Container.ResolveAll<DomainConstraint<T>>().ToArray();
                _domainConstraints = new DomainConstraint<T>[] { };
            }

            ValidationFailure validationFailure = null;

            foreach (var domainConstraint in _domainConstraints)
            {
                var checkConstraintReturn = domainConstraint.CheckConstraint(entity);

                if (!checkConstraintReturn.Success)
                {
                    validationFailure = new ValidationFailure(checkConstraintReturn.Property, checkConstraintReturn.ErrorMessage, entity);
                    break;
                }
            }

            return validationFailure;
        }
    }
}
