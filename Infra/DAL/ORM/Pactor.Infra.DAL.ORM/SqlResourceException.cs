using System;

namespace Pactor.Infra.DAL.ORM
{
    internal class SqlResourceException : Exception
    {
        public SqlResourceException(string message) : base(message)
        {
        }

        public SqlResourceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}