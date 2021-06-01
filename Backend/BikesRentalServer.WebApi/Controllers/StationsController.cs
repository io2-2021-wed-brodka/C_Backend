using BikesRentalServer.Models;
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
                    Status = station.Status,
                    ActiveBikesCount = station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = station.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = station.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = station.BikeLimit,
                }),
            });
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> AddStation(AddStationRequest request)
        {
            var response = _stationsService.AddStation(request.Name, request.BikesLimit ?? Station.DefaultBikeLimit);
            return response.Status switch
            {
                Status.Success => Created($"/stations/{response.Object.Id}", new GetStationResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Name,
                    Status = response.Object.Status,
                    ActiveBikesCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = response.Object.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = response.Object.BikeLimit,
                }),
                Status.EntityNotFound or Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("{id}")]
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
                    Status = response.Object.Status,
                    ActiveBikesCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = response.Object.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = response.Object.BikeLimit,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid status"),
            };
        }

        [HttpDelete("{id}")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> RemoveStation(string id)
        {
            var response = _stationsService.RemoveStation(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("{id}/bikes")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetAllBikesAtStation(string id)
        {
            var response = _stationsService.GetActiveBikesAtStation(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetAllBikesResponse
                {
                    Bikes = response.Object.Select(bike => new GetBikeResponse
                    {
                        Id = bike.Id.ToString(),
                        Station = bike.Station is null ? null : new GetStationResponse
                        {
                            Id = bike.Station.Id.ToString(),
                            Name = bike.Station.Name,
                            Status = bike.Station.Status,
                            ActiveBikesCount = bike.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                            MalfunctionsCount = bike.Station.Bikes.SelectMany(b => b.Malfunctions).Count(),
                            ReservationsCount = bike.Station.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                            BikesLimit = bike.Station.BikeLimit,
                        },
                        Status = bike.Status,
                    }),
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
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
                Status.Success => Created($"/bikes/{response.Object.Id}", new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Station = new GetStationResponse
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                        Status = response.Object.Station.Status,
                        ActiveBikesCount = response.Object.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                        MalfunctionsCount = response.Object.Station.Bikes.SelectMany(b => b.Malfunctions).Count(),
                        ReservationsCount = response.Object.Station.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                        BikesLimit = response.Object.Station.BikeLimit,
                    },
                    Status = response.Object.Status,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("active")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllStationsResponse> GetActiveStations()
        {
            var response = new GetAllStationsResponse
            {
                Stations = _stationsService.GetActiveStations().Object.Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                    Status = station.Status,
                    ActiveBikesCount = station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = station.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = station.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = station.BikeLimit,
                }),
            };
            return Ok(response);
        }

        [HttpGet("blocked")]
        [AdminAuthorization]
        public ActionResult<GetAllStationsResponse> GetBlockedStations()
        {
            var response = new GetAllStationsResponse
            {
                Stations = _stationsService.GetBlockedStations().Object.Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                    Status = station.Status,
                    ActiveBikesCount = station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = station.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = station.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = station.BikeLimit,
                }),
            };
            return Ok(response);
        }

        [HttpPost("blocked")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> BlockStation(BlockStationRequest request)
        {
            var response = _stationsService.BlockStation(request.Id);
            return response.Status switch
            {
                Status.Success => Created($"/bikes/{response.Object.Id}", new GetStationResponse
                {
                    Id = response.Object.Id.ToString(),
                    Name = response.Object.Name,
                    Status = response.Object.Status,
                    ActiveBikesCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Available),
                    MalfunctionsCount = response.Object.Bikes.SelectMany(b => b.Malfunctions).Count(),
                    ReservationsCount = response.Object.Bikes.Count(b => b.Status is BikeStatus.Reserved),
                    BikesLimit = response.Object.BikeLimit,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("blocked/{id}")]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> UnblockStation(string id)
        {
            var response = _stationsService.UnblockStation(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }
    }
}
