using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using BikesRentalServer.WebApi.Authorization.Attributes;
using BikesRentalServer.WebApi.Dtos.Requests;
using BikesRentalServer.WebApi.Dtos.Responses;

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

        [HttpPost]
        [AdminAuthorization]
        public ActionResult<GetTechResponse> AddTech(AddTechRequest request)
        {
            var response = _usersService.AddTech(request.Name, request.Password);
            return response.Status switch
            {
                Status.Success => Created($"/techs/{response.Object.Id}", new GetTechResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Username,
                }),
                Status.InvalidState => Conflict("Username already in use"),
                Status.EntityNotFound or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }
    }
}
