using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BikesRentalServer.WebApi.Controllers
{
    [Route("/techs")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class TechsController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public TechsController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveTech(string id)
        {
            var response = _usersService.RemoveUser(id, UserRole.Tech);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }
    }
}
