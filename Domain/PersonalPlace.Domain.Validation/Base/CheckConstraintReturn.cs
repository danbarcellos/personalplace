namespace PersonalPlace.Domain.Validation.Base
{
    public sealed class CheckConstraintReturn
    {
        public CheckConstraintReturn(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public string Property { get; private set; }
    }
}