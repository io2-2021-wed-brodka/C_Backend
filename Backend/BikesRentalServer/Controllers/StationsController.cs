using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BikesRentalServer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class StationsController : ControllerBase
    {
        private readonly IStationsService _stationsService;

        public StationsController(IStationsService stationsService)
        {
            _stationsService = stationsService;
        }

        [HttpGet]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllStationsResponse> GetAllStations()
        {
            var response = _stationsService.GetAllStations();
            return Ok(new GetAllStationsResponse
            {
                Stations = response.Object.Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                }),
            });
        }

        [HttpGet("{id}/bikes")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetAllBikesAtStation(string id)
        {
            var response = _stationsService.GetAllBikesAtStation(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetAllBikesResponse
                {
                    Bikes = response.Object.Select(bike => new GetBikeResponse
                    {
                        Id = bike.Id.ToString(),
                        Station = bike.Station is null ? null : new GetBikeResponse.StationDto
                        {
                            Id = bike.Station.Id.ToString(),
                            Name = bike.Station.Name,
                        },
                        Status = bike.Status,
                    }),
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or _ => throw new InvalidOperationException("Invalid status"),
            };
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> GetStation(string id)
        {
            var response = _stationsService.GetStation(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetStationResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Name,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or _ => throw new InvalidOperationException("Invalid status"),
            };
        }
    }
}
