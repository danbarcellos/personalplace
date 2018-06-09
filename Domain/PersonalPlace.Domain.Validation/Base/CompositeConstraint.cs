using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Validation.Base
{
    public abstract class CompositeConstraint<T> : Constraint<T> where T : ScopedEntity
    {
        private readonly List<IValidator> _otherValidators = new List<IValidator>();

        protected void RegisterBaseValidator<TBase>(IValidator<TBase> validator)
        {
            // Ensure that we've registered a compatible validator. 
            if (validator.CanValidateInstancesOfType(typeof(T)))
            {
                _otherValidators.Add(validator);
            }
            else
            {
                throw new NotSupportedException($"ValueType {typeof(TBase).Name} is not a base-class or interface implemented by {typeof(T).Name}.");
            }

        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var mainErrors = base.Validate(context).Errors;
            var errorsFromOtherValidators = _otherValidators.SelectMany(x => x.Validate(context).Errors);
            var combinedErrors = mainErrors.Concat(errorsFromOtherValidators);

            return new ValidationResult(combinedErrors);
        }
    }
}