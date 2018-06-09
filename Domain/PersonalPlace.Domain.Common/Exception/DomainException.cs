using System;

namespace PersonalPlace.Domain.Common.Exception
{
    public class DomainException : ApplicationException
    {
        public DomainException()
        {
        }

        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
