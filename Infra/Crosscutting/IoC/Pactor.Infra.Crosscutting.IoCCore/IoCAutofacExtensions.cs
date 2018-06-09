using Autofac;
using Autofac.Core;

namespace Pactor.Infra.Crosscutting.IoC.Core
{
    public static class IoCAutofacExtensions
    {
        public static void Update(this IContainer originalContainer, Autofac.ContainerBuilder containerBuilder)
        {
            var autofacOriginalContainer = GetInternalComponentRegistry(originalContainer);
            containerBuilder.Update(autofacOriginalContainer);
        }

        private static IComponentRegistry GetInternalComponentRegistry(IContainer container)
        {
            return ((AutofacContainer) container).GetInternalComponentRegistry();
        }

        public static ILifetimeScope GetInternalContainer(this IContainer container)
        {
            return ((AutofacContainer) container).GetInternalContainer();
        }
    }
}