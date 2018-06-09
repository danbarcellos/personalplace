namespace PersonalPlace.Domain.Common.Exception
{
    public class DomainStateEntityException : DomainEntityException
    {
        public DomainStateEntityException()
        {
        }

        public DomainStateEntityException(string message) : base(message)
        {
        }

        public DomainStateEntityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}