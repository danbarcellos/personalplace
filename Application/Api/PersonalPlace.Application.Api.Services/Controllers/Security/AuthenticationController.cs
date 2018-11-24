using Microsoft.AspNetCore.Mvc;
using PersonalPlace.Domain.Contract.Queries.Security;

namespace PersonalPlace.Application.Api.Services.Controllers.Security
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private const string InvalidAuthentication = "Invalid username or password";
        private readonly IUserByEmail _userByEmailQuery;

        public AuthenticationController(IUserByEmail userByEmailQuery)
        {
            _userByEmailQuery = userByEmailQuery;
        }

        [Route("~/api/authentication")]
        [HttpPost]
        public AuthencitationResult Post([FromBody] AuthenticationDto authenticationDto)
        {
            if (authenticationDto.Email == null 
                || string.IsNullOrWhiteSpace(authenticationDto.Email)
                ||string.IsNullOrWhiteSpace(authenticationDto.Password))
                return AuthencitationResult.Fail(InvalidAuthentication);

            _userByEmailQuery.Email = authenticationDto.Email;
            var user = _userByEmailQuery.Execute();

            return user.Password != authenticationDto.Password
                ? AuthencitationResult.Fail(InvalidAuthentication) 
                : AuthencitationResult.Pass();
        }
    }
}