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
        public ActionResult<GetMalfunctionResponse> RemoveMalfunction(string id)
        {
            var response = _techsService.RemoveMalfunction(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetMalfunctionResponse
                {
                    Id = response.Object.Id.ToString(),
                    Bike = response.Object.Bike is null ? null : new GetBikeResponse()
                    {
                        Id = response.Object.Bike.Id.ToString(),
                        Station = response.Object.Bike.Station is null ? null : new GetStationResponse()
                        {
                            ActiveBikesCount = response.Object.Bike.Station.Bikes.Count,
                            Id = response.Object.Bike.Station.Id.ToString(),
                            Name = response.Object.Bike.Station.Name,
                            Status = response.Object.Bike.Station.Status,
                        },
                        Status = response.Object.Bike.Status,
                        User = response.Object.Bike.User is null ? null : new GetUserResponse()
                        {
                            Id = response.Object.Bike.User.Id.ToString(),
                            Name = response.Object.Bike.User.Username,
                        }
                    },
                    Description = response.Object.Description,
                    DetectionDate = response.Object.DetectionDate,
                    MalfunctionState = response.Object.State,
                    ReportingUser = response.Object.ReportingUser is null ? null : new GetUserResponse()
                    {
                        Id = response.Object.ReportingUser.Id.ToString(),
                        Name = response.Object.ReportingUser.Username,
                    }
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }
    }
}
