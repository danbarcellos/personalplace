using System;
using System.Reflection;
using FluentValidation;
using Pactor.Infra.Crosscutting.IoC;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IContainer _container;
        private readonly MethodInfo _createInstanceMethod;

        public ValidatorFactory(IContainer container)
        {
            _container = container;
            _createInstanceMethod = typeof(ValidatorFactory).GetMethod("CreateInstance", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public IValidator<T> GetValidator<T>()
        {
            return CreateInstance<T>();
        }

        public IValidator GetValidator(Type type)
        {
            var genericCreateInstanceMethod = _createInstanceMethod.MakeGenericMethod(type);
            return (IValidator) genericCreateInstanceMethod.Invoke(this, null);
        }

        private IValidator<T> CreateInstance<T>()
        {
            IValidator<T> validator;
            return _container.TryResolve(out validator) ? validator : null;
        }
    }
}