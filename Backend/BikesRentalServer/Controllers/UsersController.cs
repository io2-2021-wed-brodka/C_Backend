using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [AdminAuthorization]
        public ActionResult<GetAllUsersResponse> GetAllUsers()
        {
            var response = new GetAllUsersResponse
            {
                Users = _usersService.GetAllUsers().Object
                    .Select(user => new GetUserResponse
                    {
                        Id = user.Id.ToString(),
                        Name = user.Username,
                    }),
            };

            return Ok(response);
        }
    }
}
