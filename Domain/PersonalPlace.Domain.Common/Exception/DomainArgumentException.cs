namespace PersonalPlace.Domain.Common.Exception
{
    public class DomainArgumentException : DomainException
    {
        public DomainArgumentException()
        {
        }

        public DomainArgumentException(string message) : base(message)
        {
        }

        public DomainArgumentException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}