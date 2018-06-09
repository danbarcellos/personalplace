using Autofac;
using Microsoft.Extensions.Configuration;
using Pactor.Infra.Crosscutting.Log;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Pactor.Infra.DAL.ORM.Queries;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace Pactor.Infra.DAL.ORM.IoCM
{
    public class IoCModuleDALORM : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionProvider(c.Resolve<IConfiguration>()))
                .As<IConnectionProvider>()
                .SingleInstance();

            builder.Register(c => new QueryFactory(c.Resolve<IContainer>()))
                .As<IQueryFactory>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new SqlResourceManager(Dialect.SQLServer, typeof(IQueryMachine).Assembly))
                .As<ISqlResourceManager>()
                .InstancePerLifetimeScope();
        }
    }
}
