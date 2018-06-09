using System;
using FluentValidation.Validators;

namespace PersonalPlace.Domain.Validation.Base
{
    public class SystemDateTimeInThePastPropertyValidator : PropertyValidator
    {
        public SystemDateTimeInThePastPropertyValidator() : base("Property {PropertyName} is in the future.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var dateTime = context.PropertyValue is DateTime ? (DateTime)context.PropertyValue : new DateTime?();
            return dateTime != null && dateTime < DateTime.Now;
        }
    }
}