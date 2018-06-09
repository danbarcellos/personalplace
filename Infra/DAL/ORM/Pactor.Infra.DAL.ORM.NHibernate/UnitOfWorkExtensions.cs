using NHibernate;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public static class UnitOfWorkExtensions
    {
        public static ISession GetEncapsulatedUnityOfWork(this IUnitOfWork nhUnitOfWork)
        {
            return (ISession)((NHUnitOfWork)nhUnitOfWork).GetEncapsulatedUnityOfWork();
        }

        public static global::NHibernate.Dialect.Dialect GetDialect(this IUnitOfWork nhUnitOfWork)
        {
            var session = GetEncapsulatedUnityOfWork(nhUnitOfWork);
            return session.GetSessionImplementation().Factory.Dialect;
        }
    }
}