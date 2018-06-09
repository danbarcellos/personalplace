using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Pactor.Infra.Crosscutting.Security;

namespace Pactor.Infra.DAL.ORM
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IDictionary<string, string> _databaseConnectionStrings;

        public ConnectionProvider(IConfiguration configuration)
        {
            _databaseConnectionStrings = new Dictionary<string, string>();
            ;
            foreach (var connectionString in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                _databaseConnectionStrings.Add(connectionString.Key, Concealment.Encryption.Decrypt(connectionString.Value));
            }
        }

        public virtual void SetConnection(string connectionStringName, string connectionString)
        {
            if (!_databaseConnectionStrings.ContainsKey(connectionStringName))
                _databaseConnectionStrings.Add(connectionStringName, connectionString);
        }

        public virtual string GetConnectionString(string databaseName)
        {
            return _databaseConnectionStrings.ContainsKey(databaseName) ? _databaseConnectionStrings[databaseName] : string.Empty;
        }
    }
}