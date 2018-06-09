using System;
using System.Runtime.Serialization;

namespace Pactor.Infra.DAL.ORM
{
    [Serializable]
    public class ConstraintViolationException : ApplicationException
    {
        private readonly string _constraintName;
        private readonly string _sql;

        public ConstraintViolationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ConstraintViolationException(string message, Exception innerException, string sql, string constraintName = null) : base(message, innerException)
        {
            _sql = sql;
            _constraintName = constraintName;
        }

        /// <summary> 
        /// Returns the text of the SQL command performed in violation of constraint, if known. 
        /// </summary>
        /// <returns> The text of the SQL command performed in violation of constraint, if known. </returns>
        public string Sql => _sql;

        /// <summary> 
        /// Returns the name of the violated constraint, if known. 
        /// </summary>
        /// <returns> The name of the violated constraint, or null if not known. </returns>
        public string ConstraintName => _constraintName;
    }
}