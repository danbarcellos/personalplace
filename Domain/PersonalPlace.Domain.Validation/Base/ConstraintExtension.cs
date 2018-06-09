using System;
using FluentValidation;

namespace PersonalPlace.Domain.Validation.Base
{
    public static class ConstraintExtension
    {
        public static IRuleBuilderOptions<T, DateTime> IsInTheFuture<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemDateTimeInTheFuturePropertyValidator());
        }
        public static IRuleBuilderOptions<T, DateTime?> IsInTheFuture<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemDateTimeInTheFuturePropertyValidator());
        }

        public static IRuleBuilderOptions<T, DateTime> IsInThePast<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemDateTimeInThePastPropertyValidator());
        }

        public static IRuleBuilderOptions<T, DateTime?> IsInThePast<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemDateTimeInThePastPropertyValidator());
        }

        public static IRuleBuilderOptions<T, DateTime> IsSystemValidDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemValidDateTimePropertyValidator());
        }

        public static IRuleBuilderOptions<T, DateTime?> IsSystemValidDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SystemValidDateTimePropertyValidator());
        }
    }
}