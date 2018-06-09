namespace PersonalPlace.Domain.Common.Exception.Seguranca
{
    public class DomainSecurityException : DomainException
    {
        public DomainSecurityException()
        {
        }

        public DomainSecurityException(string message) : base(message)
        {
        }

        public DomainSecurityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}