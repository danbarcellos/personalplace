using System;
using System.Runtime.Serialization;

namespace Pactor.Infra.DAL.ORM
{
    public class StaleObjectStateException : ApplicationException
    {
        public StaleObjectStateException(string message, string entityName, object identifier) : base(message)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        public StaleObjectStateException(string message, string entityName, object identifier, System.Exception innerException) : base(message, innerException)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        public StaleObjectStateException(string entityName, object identifier)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        protected StaleObjectStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string EntityName { get; protected set; }
        public object Identifier { get; protected set; }
    }
}