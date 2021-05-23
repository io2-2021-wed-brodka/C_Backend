using BikesRentalServer.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.WebApi.Authorization.Attributes;
using BikesRentalServer.WebApi.Dtos.Requests;
using BikesRentalServer.WebApi.Dtos.Responses;

namespace BikesRentalServer.WebApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class MalfunctionsController : Controller
    {
        private readonly IMalfunctionsService _malfunctionsService;

        public MalfunctionsController(IMalfunctionsService malfunctionsService)
        {
            _malfunctionsService = malfunctionsService;
        }
        [HttpPost]
        [TechAuthorization]
        [AdminAuthorization]
        [UserAuthorization]
        public ActionResult <GetMalfunctionResponse> AddMalfunction(AddMalfunctionRequest request)
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
            };
        }

        [HttpGet("{id}")]
        [TechAuthorization]
        [AdminAuthorization]
        [UserAuthorization]
        public ActionResult<GetMalfunctionResponse> GetMalfunction(string id)
        {
            return null;
        }
    }
}
