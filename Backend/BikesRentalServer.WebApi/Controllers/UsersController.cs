using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization;
using BikesRentalServer.WebApi.Authorization.Attributes;
using BikesRentalServer.WebApi.Dtos.Requests;
using BikesRentalServer.WebApi.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BikesRentalServer.WebApi.Controllers
{
    [Route("/users")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
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
                        Status = user.Status,
                    }),
            };
            return Ok(response);
        }

        [HttpPost("blocked")]
        [AdminAuthorization]
        public ActionResult<string> Block(BlockUserRequest request)
        {
            var response = _usersService.BlockUser(request.Id);
            return response.Status switch
            {
                Status.Success => Created($"/users/{response.Object.Id}", response.Object),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("blocked/{id}")]
        [AdminAuthorization]
        public ActionResult<string> Unblock(string id)
        {
            var response = _usersService.UnblockUser(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }
        
        [HttpPost("/login")]
        public ActionResult<LogInResponse> LogIn(LogInRequest request)
        {
            var response = _usersService.GetUserByUsernameAndPassword(request.Login, request.Password);
            return response.Status switch
            {
                Status.Success => Ok(new LogInResponse
                {
                    Role = response.Object.Role,
                    Token = _usersService.GenerateBearerToken(response.Object).Object,
                }),
                Status.EntityNotFound => Unauthorized("Bad credentials"),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost("/register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            var response = _usersService.AddUser(request.Login, request.Password);
            return response.Status switch
            {
                Status.Success => Ok(new RegisterResponse
                {
                    Token = _usersService.GenerateBearerToken(response.Object).Object,
                }),
                Status.InvalidState => Conflict("Conflicting registration data"),
                Status.EntityNotFound or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost("/logout")]
        public ActionResult<string> Logout()
        {
            return NoContent();
        }
    }
}
