using System;
using FluentValidation;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();

        IValidator GetValidator(Type type);
    }
}