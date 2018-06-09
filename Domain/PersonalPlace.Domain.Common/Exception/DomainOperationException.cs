namespace PersonalPlace.Domain.Common.Exception
{
    public class DomainOperationException : DomainException
    {
        public DomainOperationException()
        {
        }

        public DomainOperationException(string message) : base(message)
        {
        }

        public DomainOperationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}