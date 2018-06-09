using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using Pactor.Infra.DAL.ORM.NHibernate.Map;
using PersonalPlace.Domain.Base;

namespace Pactor.Infra.DAL.ORM.NHibernate.Config
{
    public abstract class ORMConfigBase : IORMConfig
    {
        private const string NormalizeSqlScriptKey = "NormalizeObjectNames";

        private static readonly ISqlResourceManager SqlResourceManager;

        static ORMConfigBase()
        {
            SqlResourceManager = new SqlResourceManager(Dialect.SQLServer, typeof(ORMConfigBase).Assembly);
        }

        public abstract string Database { get; }

        public Configuration Config(Configuration cfg)
        {
            SetConfig(cfg);
            var mapper = GetModelMapperByConvention();

            using (StateRegisterFactory.GetStateRegistryFactorySession())
            {
                foreach (var hbmMapping in Map(mapper))
                {
                    cfg.AddMapping(hbmMapping);
                }
            }

            foreach (var auxiliaryDatabaseObject in GetAuxiliaryDatabaseObjects())
            {
                cfg.AddAuxiliaryDatabaseObject(auxiliaryDatabaseObject);
            }

            return cfg;
        }

        protected abstract void SetConfig(Configuration cfg);

        protected abstract IEnumerable<HbmMapping> Map(ModelMapper mapper);

        public abstract void IoCRegister(ContainerBuilder containerBuilder);

        public virtual int Priority => 1;

        protected abstract ModelMapper GetModelMapperByConvention();

        protected abstract IEnumerable<IAuxiliaryDatabaseObject> GetAuxiliaryDatabaseObjects();

        public static IAuxiliaryDatabaseObject GetDefaultAuxiliaryDatabaseObject()
        {
            var normalizeSqlScript = SqlResourceManager.GetSqlScript(NormalizeSqlScriptKey);
            return new SimpleAuxiliaryDatabaseObject(normalizeSqlScript, null, new HashSet<string> { typeof(MsSql2008Dialect).FullName, typeof(MsSql2012Dialect).FullName });
        }

        protected static IEnumerable<IDatabaseObjectSource> GetDatabaseObjectSources(IEnumerable<Assembly> assemblies)
        {
            var databaseObjectSources = new List<IDatabaseObjectSource>();

            foreach (var assembly in assemblies)
            {
                var dbObjectSourcesTypes = assembly.GetExportedTypes().Where(type => typeof(IDatabaseObjectSource).IsAssignableFrom(type));
                databaseObjectSources.AddRange(dbObjectSourcesTypes.Select(dbObjectSourcesType => (IDatabaseObjectSource)Activator.CreateInstance(dbObjectSourcesType)));
            }

            return databaseObjectSources;
        }
    }
}