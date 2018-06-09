using System;
using FluentValidation.Validators;

namespace PersonalPlace.Domain.Validation.Base
{
    public class SystemValidDateTimePropertyValidator : PropertyValidator
    {
        private readonly DateTime _minSystemDateTime = new DateTime(1753, 01, 01, 0, 0, 0, 0);
        private readonly DateTime _maxSystemDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

        public SystemValidDateTimePropertyValidator(): base("Property {PropertyName} has invalid date.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var dateTime = context.PropertyValue is DateTime ? (DateTime)context.PropertyValue : new DateTime?();
            return dateTime != null && dateTime >= _minSystemDateTime && dateTime <= _maxSystemDateTime;
        }
    }
}