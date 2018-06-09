using System;
using System.Runtime.Serialization;

namespace Pactor.Infra.DAL.ORM
{
    [Serializable]
    public class DeadlockException : ApplicationException
    {
        public DeadlockException(string message, string entityName, object identifier) : base(message)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        public DeadlockException(string message, string entityName, object identifier, System.Exception innerException) : base(message, innerException)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        public DeadlockException(string entityName, object identifier)
        {
            EntityName = entityName;
            Identifier = identifier;
        }

        protected DeadlockException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string EntityName { get; protected set; }
        public object Identifier { get; protected set; }
    }
}