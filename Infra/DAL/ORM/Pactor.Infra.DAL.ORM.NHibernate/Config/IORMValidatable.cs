using Autofac;

namespace Pactor.Infra.DAL.ORM.NHibernate.Config
{
    public interface IORMValidatable
    {
        void ConfigValidator(ContainerBuilder builder,IValidationConfigurator valCfg);
    }
}