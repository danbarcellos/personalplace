using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Pactor.Infra.DAL.ORM.Queries;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Queries.Security;

namespace PersonalPlace.Domain.IoCM
{
    public class IoCModuleDominio : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var systemDomainContext = DomainContext.GetDefault();
            builder.RegisterInstance(systemDomainContext)
                .As<IDomainContext>()
                .Named<IDomainContext>(DomainContext.SystemInstance)
                .SingleInstance();

            foreach (var queryKeyValue in GetQueryTypesFromAssembly())
            {
                var interfaceQueryType = queryKeyValue.Key;
                var implementedQueryType = queryKeyValue.Value;

                builder.RegisterType(implementedQueryType)
                       .As(interfaceQueryType)
                       .InstancePerDependency();
            }
        }

        private Dictionary<Type, Type> GetQueryTypesFromAssembly()
        {
            var queryBaseTypes = new[]
            {
                typeof (NHibernateQueryBase<>),
                typeof (NamedQueryBase<>),
                typeof (NamedQueryFutureBase<>),
                typeof (CriteriaQueryBase<>),
                typeof (PagedQueryOverBase<>)
            };

            var queryBaseAssembly = typeof(IQuery).Assembly;
            var queriesCatalog = new Dictionary<Type, Type>();
            var assembly = typeof(UserByEmail).Assembly; //todo: Elaborar outro método mais flexível de acesso ao assembly de queries.

            var queryTypes = assembly.GetExportedTypes().Where(t => t.BaseType != null && t.BaseType.IsGenericType
                                                                                       && queryBaseTypes.All(qbt => t != qbt)
                                                                                       && queryBaseTypes.Any(qbt => qbt.IsAssignableFrom(t.BaseType.GetGenericTypeDefinition())));
            foreach (var type in queryTypes)
            {
                //todo: Deve-se colocar interfaces em outro assembly, não no mesmo como está agora
                var interfaceType = type.GetInterfaces().Single(i => i.Assembly != queryBaseAssembly && typeof(IQuery).IsAssignableFrom(i));
                queriesCatalog.Add(interfaceType, type);
            }

            return queriesCatalog;
        }
    }
}
