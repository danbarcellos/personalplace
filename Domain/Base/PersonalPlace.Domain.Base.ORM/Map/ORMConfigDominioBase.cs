using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NHibernate.Mapping.ByCode;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.NHibernate;
using Pactor.Infra.DAL.ORM.NHibernate.Config;

namespace PersonalPlace.Domain.Base.ORM.Map
{
    public abstract class ORMConfigDominioBase : ORMConfigBase
    {
        private readonly IList<Type> _aggregateRootCatalog;

        protected ORMConfigDominioBase()
        {
            _aggregateRootCatalog = new List<Type>();
        }

        public override string Database => DatabaseName.PersonalPlace;


        public override void IoCRegister(ContainerBuilder containerBuilder)
        {
            foreach (var aggregateRoot in _aggregateRootCatalog)
            {
                var root = aggregateRoot;
                var interfaceType = typeof(IRepository<>).MakeGenericType(root);
                containerBuilder.RegisterType(typeof(NHibernateRepository<>).MakeGenericType(root))
                                .As(interfaceType)
                                .InstancePerLifetimeScope();
            }
        }

        protected override ModelMapper GetModelMapperByConvention()
        {
            var mapper = new DomainMapperConventions();
            mapper.AfterMapClass +=
            (mi, type, map) =>
            {
                if (type.GetCustomAttributes(inherit: false).Any(attribute => attribute is EntityParameterAttribute && ((EntityParameterAttribute)attribute).IsAggregateRoot))
                {
                    _aggregateRootCatalog.Add(type);
                }
            };
            return mapper;
        }
    }
}