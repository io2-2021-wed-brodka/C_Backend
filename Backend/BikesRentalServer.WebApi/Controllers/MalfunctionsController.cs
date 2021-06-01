using System;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization;
using BikesRentalServer.WebApi.Authorization.Attributes;
using BikesRentalServer.WebApi.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BikesRentalServer.WebApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class MalfunctionsController : ControllerBase
    {
        private readonly ITechsService _techsService;

        public MalfunctionsController(ITechsService techsService)
        {
            _techsService = techsService;
        }
        
        [HttpDelete("{id}")]
        [AdminAuthorization]
        [TechAuthorization]
        public IActionResult RemoveMalfunction(string id)
        {
            var response = _techsService.RemoveMalfunction(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound or Status.InvalidState or Status.UserBlocked or _ => NotFound(response.Message),
            };
        }
    }
}
