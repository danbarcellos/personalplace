using NHibernate;
using Pactor.Infra.Crosscutting.IoC;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHUnitOfWorkFactory : IUnitOfWorkFactory
    {
        protected readonly ISessionFactory _sessionFactory;
        protected readonly IContainer _container;

        public NHUnitOfWorkFactory(ISessionFactory sessionFactory, IContainer container)
        {
            _sessionFactory = sessionFactory;
            _container = container;
        }

        public virtual IUnitOfWork OpenUnitOfWork()
        {
            return new NHUnitOfWork(_sessionFactory.OpenSession(), _container);
        }

        public IStatelessUnitOfWork OpenStatelessUnitOfWork()
        {
            return new NHStatelessUnitOfWork(_sessionFactory.OpenStatelessSession(), _container, _container.Resolve<IValidatorFactory>(), _container.ResolveAll<IPreInsertInterceptor>());
        }
    }
}
