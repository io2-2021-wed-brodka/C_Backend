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
    [Route("/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class MalfunctionsController : ControllerBase
    {
        private readonly IMalfunctionsService _malfunctionsService;

        public MalfunctionsController(IMalfunctionsService malfunctionsService)
        {
            _malfunctionsService = malfunctionsService;
        }
        
        [HttpDelete("{id}")]
        [AdminAuthorization]
        [TechAuthorization]
        public IActionResult RemoveMalfunction(string id)
        {
            var response = _malfunctionsService.RemoveMalfunction(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException(response.Message),
            };
        }

        [HttpGet]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllMalfunctionsResponse> GetAllMalfunctions()
        {
            var response = _malfunctionsService.GetAllMalfunctions();
            return Ok(new GetAllMalfunctionsResponse
            {
                Malfunctions = response.Object.Select(malfunction => new GetMalfunctionResponse
                {
                    Description = malfunction.Description,
                    Id = malfunction.Id.ToString(),
                    BikeId = malfunction.Bike.Id.ToString(),
                    ReportingUserId = malfunction.ReportingUser.Id.ToString(),
                }),
            });
        }
        

        [HttpPost]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetMalfunctionResponse> AddMalfunction(AddMalfunctionRequest request)
        {
            var response = _malfunctionsService.AddMalfunction(request.Id, request.Description);
            return response.Status switch
            {
                Status.Success => Created($"/malfunctions/{response.Object.Id}", new GetMalfunctionResponse
                {
                    Id = response.Object.Id.ToString(),
                    Description = response.Object.Description,
                    BikeId = response.Object.Bike.Id.ToString(),
                    ReportingUserId = response.Object.ReportingUser.Id.ToString()
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException($"Unexpected result: {response.Status} - {response.Message}")
            };
        }
    }
}
