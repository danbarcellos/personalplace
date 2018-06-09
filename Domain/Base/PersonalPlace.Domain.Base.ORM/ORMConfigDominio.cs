using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.NHibernate;
using Pactor.Infra.DAL.ORM.NHibernate.Config;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Base.Filters;
using PersonalPlace.Domain.Base.ORM.Map;
using PersonalPlace.Domain.Queries.Security;
using PersonalPlace.Domain.Validation.Base;
using Environment = NHibernate.Cfg.Environment;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace PersonalPlace.Domain.Base.ORM
{
    public class ORMConfigDominio : ORMConfigDominioBase, IORMValidatable
    {
        public override int Priority => 0;

        protected override void SetConfig(Configuration cfg)
        {
            cfg.DataBaseIntegration(db =>
            {
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.LogFormattedSql = true;
            })
            .SetProperty(Environment.CurrentSessionContextClass, "thread_static");


            cfg.DataBaseIntegration(x => x.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote);
            cfg.AddFilterDefinition(ScopeFilters.RootSegregation.Definition);
            cfg.AddFilterDefinition(ScopeFilters.DescendingSegregation.Definition);
            cfg.AddFilterDefinition(ScopeFilters.AscendingSegregation.Definition);
            cfg.AddFilterDefinition(ScopeFilters.ParenthoodSegregation.Definition);

            // ReSharper disable once CoVariantArrayConversion
            cfg.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new SessionDomainContextHandler() });
            cfg.AddAssembly(typeof(UserByEmail).Assembly);
        }

        protected override IEnumerable<HbmMapping> Map(ModelMapper mapper)
        {
            mapper.AddMappings(GetType().Assembly
                                        .GetExportedTypes()
                                        .Where(t => t.Namespace != null && t.Namespace.Contains(".ORM.Map.")));

            var hbmMaps = new List<HbmMapping>();
            foreach (var entidadeAssembly in ObterEntidadeAssemblies())
            {
                var entitiesFromAssembly = entidadeAssembly.GetExportedTypes().Where(t => t.IsClass && DomainMapperConventions.BaseEntityType.IsAssignableFrom(t));
                hbmMaps.Add(mapper.CompileMappingFor(StateRegisterFactory.RegisterEntitiesStateRecord(entidadeAssembly)));
                hbmMaps.Add(mapper.CompileMappingFor(entitiesFromAssembly));
            }
            return hbmMaps;
        }

        public void ConfigValidator(ContainerBuilder builder, IValidationConfigurator valCfg)
        {
            valCfg.RegisterFromAssembly(builder, typeof(Constraint<>).Assembly, val => val != typeof(Constraint<>) && val != typeof(CompositeConstraint<>));
        }

        protected override IEnumerable<IAuxiliaryDatabaseObject> GetAuxiliaryDatabaseObjects()
        {
            var auxiliaryDatabaseObjects = new List<IAuxiliaryDatabaseObject>();
            var databaseObjectSources = GetDatabaseObjectSources(new[] { GetType().Assembly, typeof(UserByEmail).Assembly });

            foreach (var databaseObjectSource in databaseObjectSources)
            {
                auxiliaryDatabaseObjects.AddRange(databaseObjectSource.GetDatabaseObjects());
            }
            return auxiliaryDatabaseObjects;
        }

        public override void IoCRegister(ContainerBuilder containerBuilder)
        {
            base.IoCRegister(containerBuilder);

            containerBuilder.Register(c => new DomainUnitOfWorkFactory(c.ResolveNamed<ISessionFactory>(Database), c.Resolve<IContainer>()))
                            .As<IUnitOfWorkFactory>()
                            .InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                var uow = c.Resolve<IUnitOfWork>();
                var session = uow.GetEncapsulatedUnityOfWork();
                NHibernate.Context.CurrentSessionContext.Bind(session);
                return session;
            })
                            .As<ISession>()
                            .InstancePerLifetimeScope()
                            .OnRelease(session =>
                            {
                                if (NHibernate.Context.CurrentSessionContext.HasBind(session.SessionFactory))
                                    NHibernate.Context.CurrentSessionContext.Unbind(session.SessionFactory);

                                session.Dispose();
                            });

            containerBuilder.Register(c => c.Resolve<IUnitOfWorkFactory>().OpenUnitOfWork())
                            .As<IUnitOfWork>()
                            .InstancePerLifetimeScope();

            containerBuilder.Register(c => new QueryMachine(c.Resolve<ISession>()))
                            .As<IQueryMachine>()
                            .InstancePerLifetimeScope();
        }

        private static IEnumerable<Assembly> ObterEntidadeAssemblies()
        {
            //todo: decidir qual o método de procura por entidades do domínio Alaris.
            return Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Where(x => x.Name.Contains(".Domain.Entities"))
                .Select(Assembly.Load).ToArray();
        }

        private class DomainUnitOfWorkFactory : IUnitOfWorkFactory
        {
            private readonly ISessionFactory _sessionFactory;
            private readonly IContainer _container;
            private readonly IDomainContext _domainContext;

            public DomainUnitOfWorkFactory(ISessionFactory sessionFactory, IContainer container)
            {
                _sessionFactory = sessionFactory;
                _container = container;
                _domainContext = container.Resolve<IDomainContext>();
            }

            public IUnitOfWork OpenUnitOfWork()
            {
                return GetNewScopedSession();
            }

            public IUnitOfWork OpenUnitOfWork(DbConnection connection)
            {
                return GetNewScopedSession(connection);
            }

            public IStatelessUnitOfWork OpenStatelessUnitOfWork()
            {
                return GetStatelessUnitOfWork();
            }

            public IStatelessUnitOfWork OpenStatelessUnitOfWork(DbConnection connection)
            {
                return GetStatelessUnitOfWork(connection);
            }

            private IUnitOfWork GetNewScopedSession(DbConnection connection = null)
            {
                var session = connection == null
                              ? _sessionFactory.OpenSession()
                              : _sessionFactory.WithOptions().Connection(connection).OpenSession();

                session.EnableFilter(ScopeFilters.DescendingSegregation.Name)
                       .SetParameter(ScopeFilters.DescendingSegregation.ParameterName, $"{_domainContext.ScopeTag}%");
                session.EnableFilter(ScopeFilters.AscendingSegregation.Name)
                       .SetParameterList(ScopeFilters.AscendingSegregation.ParameterName, _domainContext.ScopeTag.ToAscendingScopeTags());
                session.EnableFilter(ScopeFilters.ParenthoodSegregation.Name)
                       .SetParameter(ScopeFilters.DescendingSegregation.ParameterName, $"{_domainContext.ScopeTag}%")
                       .SetParameterList(ScopeFilters.ParenthoodSegregation.ParameterName, _domainContext.ScopeTag.ToParenthoodAscendingScopeTags());

                var disposable = SessionDomainContextHandler.SetSessionContext(session, _domainContext);

                return new NHUnitOfWork(session, _container.Resolve<IContainer>(), disposable);
            }

            private NHStatelessUnitOfWork GetStatelessUnitOfWork(DbConnection connection = null)
            {
                var session = connection == null
                              ? _sessionFactory.OpenStatelessSession()
                              : _sessionFactory.OpenStatelessSession(connection);

                var disposable = SessionDomainContextHandler.SetSessionContext(session, _domainContext);
                var statelessUnitOfWork = new NHStatelessUnitOfWork(session,
                                                                    _container.Resolve<IContainer>(),
                                                                    _container.Resolve<IValidatorFactory>(),
                                                                    _container.ResolveAll<IPreInsertInterceptor>(),
                                                                    disposable);
                return statelessUnitOfWork;
            }
        }

    }
}