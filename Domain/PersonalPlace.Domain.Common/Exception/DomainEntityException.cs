namespace PersonalPlace.Domain.Common.Exception
{
    public class DomainEntityException : DomainException
    {
        public DomainEntityException()
        {
        }

        public DomainEntityException(string message) : base(message)
        {
        }

        public DomainEntityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}