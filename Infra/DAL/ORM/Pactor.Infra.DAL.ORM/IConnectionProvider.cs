namespace Pactor.Infra.DAL.ORM
{
    public interface IConnectionProvider
    {
        void SetConnection(string connectionStringName, string connectionString);
        string GetConnectionString(string databaseName);
    }
}