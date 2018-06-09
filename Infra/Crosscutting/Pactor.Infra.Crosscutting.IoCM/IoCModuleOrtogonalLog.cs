using System.Linq;
using Autofac;
using Autofac.Core;
using NLog;
using Pactor.Infra.Crosscutting.LogCore;
using ILog = Pactor.Infra.Crosscutting.Log.ILog;

namespace Pactor.Infra.Crosscutting.IoCM
{
    public class IoCModuleOrtogonalLog : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) => new NLogLogger(LogManager.GetLogger(p.TypedAs<string>())))
                   .As<ILog>()
                   .InstancePerDependency();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var type = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
                           {
                               new ResolvedParameter((p, c) => p.ParameterType == typeof(ILog),
                                   (p, c) => type.IsGenericType 
                                       ? new NLogLogger(LogManager.GetLogger(type.FullName?.Remove(type.FullName.IndexOf('`')))) 
                                       : new NLogLogger(LogManager.GetLogger(type.FullName)))
                           });
        }
    }
}
