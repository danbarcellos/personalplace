using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using Pactor.Infra.Crosscutting.IoC;
using Pactor.Infra.Crosscutting.Log;
using Pactor.Infra.DAL.ORM.NHibernate.Config;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Environment = NHibernate.Cfg.Environment;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace Pactor.Infra.DAL.ORM.NHibernate.Facility
{
    public class NHibernateORMBootstrap
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ILog _logger;

        public NHibernateORMBootstrap(ILog logger, IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        public void Config(ContainerBuilder builder)
        {
            var configurators = GetConfigsFromAssemblyLocation();
            var ormConfigs = new Dictionary<string, Configuration>();
            var validatorsConfigs = new Dictionary<string, IValidationConfigurator>();

            foreach (var configurator in configurators)
            {
                _logger.Debug(() => $"Found ORM configurator: {configurator.GetType().FullName} -> Target database: {configurator.Database}");

                Configuration cfg;

                if (ormConfigs.ContainsKey(configurator.Database))
                    cfg = ormConfigs[configurator.Database];
                else
                {
                    cfg = GetORMConfig(configurator.Database);
                    ormConfigs.Add(configurator.Database, cfg);
                }

                configurator.Config(cfg);
                configurator.IoCRegister(builder);

                if (configurator is IORMValidatable validatable)
                {
                    IValidationConfigurator valCfg;
                    if (validatorsConfigs.ContainsKey(configurator.Database))
                        valCfg = validatorsConfigs[configurator.Database];
                    else
                    {
                        valCfg = new ValidationConfigurator();
                        validatorsConfigs.Add(configurator.Database, valCfg);
                    }

                    validatable.ConfigValidator(builder, valCfg);
                }
            }

            foreach (var cfgKeyValuePair in ormConfigs)
            {
                var databaseName = cfgKeyValuePair.Key;
                var databaseConfig = cfgKeyValuePair.Value;

                if (validatorsConfigs.ContainsKey(databaseName))
                {
                    validatorsConfigs[databaseName].ApplyingDDLConstraints(databaseConfig);
                }

                databaseConfig.ApplyDatabaseSchemaConventions();

                builder.Register(c => new ValidatorFactory(c.Resolve<IContainer>()))
                       .As<IValidatorFactory>()
                       .SingleInstance();
                builder.Register(c => databaseConfig)
                       .As<Configuration>()
                       .WithMetadata("DatabaseName", databaseName)
                       .Named<Configuration>(databaseName)
                       .SingleInstance();
                builder.Register(c => databaseConfig.BuildSessionFactory())
                       .Named<ISessionFactory>(databaseName)
                       .SingleInstance()
                       .OnRelease(s => s.Dispose());
                builder.Register(c => new NHUnitOfWorkFactory(c.ResolveNamed<ISessionFactory>(databaseName), c.ResolveNamed<IContainer>(ContainerTag.Root)))
                       .Named<IUnitOfWorkFactory>(databaseName)
                       .SingleInstance();
                builder.Register(c => c.ResolveNamed<ISessionFactory>(databaseName).OpenSession())
                       .Named<ISession>(databaseName)
                       .InstancePerLifetimeScope()
                       .OnRelease(s => s.Dispose());
                builder.Register(c => new NHUnitOfWork(c.ResolveNamed<ISession>(databaseName), c.Resolve<IContainer>()))
                       .Named<IUnitOfWork>(databaseName)
                       .InstancePerLifetimeScope()
                       .OnRelease(s => s.Dispose());
                builder.Register(c => new QueryMachine(c.ResolveNamed<ISession>(databaseName)))
                       .Named<IQueryMachine>(databaseName)
                       .InstancePerLifetimeScope();

                _logger.Debug(() => $"Database configurated: {databaseName} -> Dialect: {databaseConfig.Properties["dialect"]}");
            }
        }

        internal Configuration GetORMConfig(string databaseName)
        {
            var cfg = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = _connectionProvider.GetConnectionString(databaseName);
                    db.Dialect<MsSql2012Dialect>();
                    db.IsolationLevel = IsolationLevel.ReadCommitted;
                })
                .SetProperty(Environment.SqlExceptionConverter, typeof(NHMsSqlExceptionConverter).AssemblyQualifiedName);

            cfg.AddAuxiliaryDatabaseObject(ORMConfigBase.GetDefaultAuxiliaryDatabaseObject());
            return cfg;
        }

        private IORMConfig[] GetConfigsFromAssemblyLocation()
        {
            var moduleTypes = new List<Type>();
            var moduleInstances = new List<IORMConfig>();
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (assemblyDirectory == null)
                throw new IOException("Error on loading ORM configuration. Unable to access the application folder.");

            var files = Directory.GetFiles(assemblyDirectory, "*.ORM.dll");

            foreach (var dllFilePathAndName in files)
            {
                try
                {
                    var assembly = Assembly.LoadFile(dllFilePathAndName);
                    var modulesInAssembly = assembly.GetTypes()
                        .Where(t => !t.IsInterface && !t.IsAbstract && typeof(IORMConfig).IsAssignableFrom(t) && typeof(IORMConfig) != t)
                        .ToArray();

                    moduleTypes.AddRange(modulesInAssembly);
                }
                catch (Exception e)
                {
                    if (e is FileLoadException || e is FileNotFoundException || e is BadImageFormatException)
                    {
                        _logger.Warn(() => $"Error loading ORM module at {dllFilePathAndName}", e);
                        continue;
                    }

                    throw;
                }
            }

            foreach (var moduleType in moduleTypes)
            {
                try
                {
                    moduleInstances.Add((IORMConfig) Activator.CreateInstance(moduleType));
                }
                catch (Exception e)
                {
                    _logger.Warn(() => $"Error loading ORM module type {moduleType.Name}", e);
                }
            }

            return moduleInstances.ToArray();
        }
    }
}