using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.Dtos.Requests;
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
        private readonly IBikesService _bikesService;

        public StationsController(IStationsService stationsService, IBikesService bikesService)
        {
            _stationsService = stationsService;
            _bikesService = bikesService;
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

        [HttpPost("{id}/bikes")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> GiveBikeBack(string id, GiveBikeBackRequest request)
        {
            var response = _bikesService.GiveBikeBack(request.Id, id);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Station = new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                    Status = response.Object.Status,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or _ => throw new InvalidOperationException("Invalid status"),
            };
        }

        [HttpDelete("{id}")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> RemoveStation(string id)
        {
            var response = _stationsService.RemoveStation(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetStationResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Name,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> AddStation(AddStationRequest request)
        {
            var response = _stationsService.AddStation(request);
            return response.Status switch
            {
                Status.Success => Ok(new GetStationResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Name,
                }),
                Status.EntityNotFound or Status.InvalidState or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("blocked")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllStationsResponse> GetBlockedStations()
        {
            var response = _stationsService.GetBlockedStations();
            return new GetAllStationsResponse
            {
                Stations = response.Object.Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                }),
            };
        }
    }
}
