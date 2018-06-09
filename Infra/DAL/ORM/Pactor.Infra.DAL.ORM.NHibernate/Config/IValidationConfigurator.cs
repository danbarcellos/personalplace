using System;
using System.Reflection;
using Autofac;
using NHibernate.Cfg;

namespace Pactor.Infra.DAL.ORM.NHibernate.Config
{
    public interface IValidationConfigurator
    {
        IValidationConfigurator RegisterFromAssembly(ContainerBuilder builder, Assembly assembly, Func<Type, bool> filter = null);
        IValidationConfigurator ApplyingDDLConstraints(Configuration configuration);
    }
}