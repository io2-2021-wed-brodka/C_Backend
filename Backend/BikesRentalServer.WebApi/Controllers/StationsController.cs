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
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
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
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
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
            var response = _stationsService.AddStation(request.Name);
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

        [HttpGet("active")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllStationsResponse> GetActiveStations()
        {
            var response = _stationsService.GetActiveStations();
            return new GetAllStationsResponse
            {
                Stations = response.Object.Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                }),
            };
        }

        [HttpPost("blocked")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> BlockStation(BlockStationRequest request)
        {
            var response = _stationsService.BlockStation(request.Id);
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

        [HttpDelete("blocked/{id}")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> UnblockStation(string id)
        {
            var response = _stationsService.UnblockStation(id);
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
    }
}
