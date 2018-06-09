using System;
using System.Data.Common;
using NHibernate.Connection;

namespace Pactor.Infra.DAL.ORM.Tests.Base
{
    public class TestConnectionProvider : DriverConnectionProvider
    {
        [ThreadStatic]
        private static DbConnection Connection;

        public static void CloseDatabase()
        {
            Connection?.Dispose();
            Connection = null;
        }

        public override DbConnection GetConnection()
        {
            if (Connection == null)
            {
                Connection = Driver.CreateConnection();
                Connection.ConnectionString = ConnectionString;
                Connection.Open();
            }
            return Connection;
        }

        public override void CloseConnection(DbConnection conn) { }
    }
}
