using Autofac;
using NHibernate.Cfg;

namespace Pactor.Infra.DAL.ORM.NHibernate.Config
{
    public interface IORMConfig
    {
        string Database { get; }
        Configuration Config(Configuration cfg);
        void IoCRegister(ContainerBuilder containerBuilder);
        int Priority { get; }
    }
}
