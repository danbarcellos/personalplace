using System;
using Autofac;
using Pactor.Infra.Crosscutting.IoC.Core;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace PersonalPlace.Domain.Base
{
    public static class IoCDomainExtension
    {
        public static IContainer BeginDomainLifetimeScope(this IContainer container, IDomainContext domainContext)
        {
            if (domainContext == null)
                throw new ArgumentNullException(nameof(domainContext));

            var internalContainer = container.GetInternalContainer();
            var lifetimeScope = internalContainer.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance(domainContext)
                    .As<IDomainContext>()
                    .Named<IDomainContext>(DomainContext.UserInstance)
                    .SingleInstance();

                builder.RegisterInstance(domainContext.UserContext)
                    .As<IUserContext>()
                    .Named<IUserContext>(DomainContext.UserInstance)
                    .SingleInstance();
            });

            return new AutofacContainer(lifetimeScope);
        }
    }
}