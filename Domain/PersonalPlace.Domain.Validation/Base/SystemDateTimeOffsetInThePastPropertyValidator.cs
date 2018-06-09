using System;
using FluentValidation.Validators;

namespace PersonalPlace.Domain.Validation.Base
{
    public class SystemDateTimeOffsetInThePastPropertyValidator : PropertyValidator
    {
        public SystemDateTimeOffsetInThePastPropertyValidator() : base("Property {PropertyName} is in the future.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var dateTime = context.PropertyValue is DateTimeOffset offset ? offset : new DateTimeOffset?();
            return dateTime != null && dateTime < DateTimeOffset.Now;
        }
    }
}