using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BikesRentalServer.Controllers
{
    [Route("/")]
    [ApiController]
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
        public IActionResult Register()
        {
            throw new NotImplementedException();
        }
    }
}
