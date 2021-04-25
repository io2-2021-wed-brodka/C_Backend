using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
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
        public ActionResult<LogInResponse> LogIn(LogInRequest request)
        {
            var response = _usersService.GetUserByUsernameAndPassword(request.Login, request.Password);
            if (response.Status is Status.EntityNotFound)
                return Unauthorized("Bad credentials");
            
            return new LogInResponse
            {
                Role = response.Object.Role,
                Token = _usersService.GenerateBearerToken(response.Object).Object,
            };
        }

        [HttpPost("register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            var response = _usersService.AddUser(request.Login, request.Password);
            if (response.Status is Status.InvalidState)
                return Conflict("Conflicting registration data");

            return new RegisterResponse
            {
                Token = _usersService.GenerateBearerToken(response.Object).Object,
            };
        }

        [HttpPost("logout")]
        public ActionResult<string> Logout()
        {
            return Ok("Logged out");
        }

        [HttpPost("{id}")]
        [AdminAuthorization]
        public ActionResult<string> Block(string id)
        {
            var response = _usersService.BlockUser(id);
            if (response.Status is Status.InvalidState)
                return Conflict("User already blocked.");
            if (response.Status is Status.EntityNotFound)
                return Conflict("User does not exist.");

            return Ok("Blocked User.");
        }

        [HttpPost("{id}")]
        [AdminAuthorization]
        public ActionResult<string> Unblock(string id)
        {
            var response = _usersService.UnblockUser(id);
            if (response.Status is Status.InvalidState)
                return Conflict("User already unblocked.");
            if(response.Status is Status.EntityNotFound)
                return Conflict("User does not exist.");

            return Ok("Unblocked User.");
        }
    }
}
