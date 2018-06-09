using System.Reflection;

namespace Pactor.Infra.DAL.ORM
{
    public interface ISqlResourceManager
    {
        string GetSqlScript(string resourceKey, Assembly assembly = null);
        void LoadResources(Assembly assembly);
    }
}