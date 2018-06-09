namespace PersonalPlace.Application.Api.Services.Controllers.Security
{
    public class AuthencitationResult
    {
        public static AuthencitationResult Fail(string message)
        {
            return new AuthencitationResult(false, message);
        }
        public static AuthencitationResult Pass()
        {
            return new AuthencitationResult(true, null);
        }

        private AuthencitationResult(bool sucess, string message)
        {
            Success = sucess;
            Message = message;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}