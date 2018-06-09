using NHibernate.Mapping;

namespace Pactor.Infra.DAL.ORM.NHibernate.Map
{
    public interface IDatabaseObjectSource
    {
        IAuxiliaryDatabaseObject[] GetDatabaseObjects();
    }
}