namespace Pactor.Infra.DAL.ORM
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork OpenUnitOfWork();
        IStatelessUnitOfWork OpenStatelessUnitOfWork();
    }
}