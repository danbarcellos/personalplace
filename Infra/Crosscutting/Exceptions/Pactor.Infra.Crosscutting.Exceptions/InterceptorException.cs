using System;
using System.Runtime.Serialization;

namespace Pactor.Infra.Crosscutting.Exceptions
{
    [Serializable]
    public class InterceptorException : ApplicationException
    {
        public InterceptorException()
        {
        }

        public InterceptorException(string message) : base(message)
        {
        }

        public InterceptorException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InterceptorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}