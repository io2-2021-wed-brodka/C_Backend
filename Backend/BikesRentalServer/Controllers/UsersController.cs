using BikesRentalServer.Authorization;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikesRentalServer.Controllers
{
    [Route("/")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<LogInResponse> LogIn(LogInRequest request)
        {
            var user = _usersService.GetUser(request.Login, request.Password);
            if (user is null)
                return Unauthorized("Bad credentials");
            
            return new LogInResponse
            {
                Role = user.Role,
                Token = _usersService.GenerateBearerToken(user),
            };
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            var user = _usersService.AddUser(request.Login, request.Password);
            if (user is null)
                return Conflict("Conflicting registration data");

            return new RegisterResponse
            {
                Token = _usersService.GenerateBearerToken(user),
            };
        }
    }
}
