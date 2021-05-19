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
    public class BikesController : ControllerBase
    {
        private readonly IBikesService _bikesService;

        public BikesController(IBikesService bikesService)
        {
            _bikesService = bikesService;
        }

        [HttpGet]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetAllBikes()
        {
            var response = _bikesService.GetAllBikes();
            return Ok(new GetAllBikesResponse
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
                    },
                    User = bike.User is null ? null : new GetUserResponse
                    {
                        Id = bike.User.Id.ToString(),
                        Name = bike.User.Username,
                    },
                    Status = bike.Status,
                }),
            });
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> AddBike(AddBikeRequest request)
        {
            var response = _bikesService.AddBike(request.StationId);
            return response.Status switch
            {
                Status.Success => Created($"/bikes/{response.Object.Id}", new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = new GetStationResponse
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                        Status = response.Object.Station.Status,
                        ActiveBikesCount = response.Object.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> GetBike(string id)
        {
            var response = _bikesService.GetBike(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = response.Object.Station is null ? null : new GetStationResponse
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                        Status = response.Object.Station.Status,
                        ActiveBikesCount = response.Object.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    },
                    User = response.Object.User is null ? null : new GetUserResponse
                    {
                        Id = response.Object.User.Id.ToString(),
                        Name = response.Object.User.Username,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("{id}")]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> RemoveBike(string id)
        {
            var response = _bikesService.RemoveBike(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("rented")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetRentedBikes()
        {
            var response = new GetAllBikesResponse
            {
                Bikes = _bikesService.GetRentedBikes().Object.Select(bike => new GetBikeResponse
                {
                    Id = bike.Id.ToString(),
                    User = new GetUserResponse
                    {
                        Id = bike.User.Id.ToString(),
                        Name = bike.User.Username,
                    },
                    Status = bike.Status,
                }),
            };
            return Ok(response);
        }

        [HttpPost("rented")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> RentBike(RentBikeRequest request)
        {
            var response = _bikesService.RentBike(request.Id);
            return response.Status switch
            {
                Status.Success => Created($"/rented/{response.Object.Id}", new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    User = new GetUserResponse
                    {
                        Id = response.Object.User.Id.ToString(),
                        Name = response.Object.User.Username,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked => Forbid(),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("blocked")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetBlockedBikes()
        {
            var response = new GetAllBikesResponse
            {
                Bikes = _bikesService.GetBlockedBikes().Object.Select(bike => new GetBikeResponse
                {
                    Id = bike.Id.ToString(),
                    Station = new GetStationResponse
                    {
                        Id = bike.Station.Id.ToString(),
                        Name = bike.Station.Name,
                        Status = bike.Station.Status,
                        ActiveBikesCount = bike.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    },
                    Status = bike.Status,
                }),
            };
            return Ok(response);
        }

        [HttpPost("blocked")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> BlockBike(BlockBikeRequest request)
        {
            var response = _bikesService.BlockBike(request.Id);
            return response.Status switch
            {
                Status.Success => Created($"/blocked/{response.Object.Id}", new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = new GetStationResponse
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                        Status = response.Object.Station.Status,
                        ActiveBikesCount = response.Object.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("blocked/{id}")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> UnblockBike(string id)
        {
            var response = _bikesService.UnblockBike(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost("reserved")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetReservedBikeResponse> ReserveBike(ReserveBikeRequest request)
        {
            var response = _bikesService.ReserveBike(request.Id);
            return response.Status switch
            {
                Status.Success => Created($"/reserved/{response.Object.Bike.Id}", new GetReservedBikeResponse
                {
                    Id = response.Object.Bike.Id.ToString(),
                    Station = new GetStationResponse
                    {
                        Id = response.Object.Bike.Station.Id.ToString(),
                        Name = response.Object.Bike.Station.Name,
                        Status = response.Object.Bike.Station.Status,
                        ActiveBikesCount = response.Object.Bike.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                    },
                    ReservedAt = response.Object.ReservationDate,
                    ReservedTill = response.Object.ExpirationDate,
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                Status.UserBlocked => Forbid(),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("reserved/{id}")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public IActionResult CancelReservation(string id)
        {
            var response = _bikesService.CancelBikeReservation(id);
            return response.Status switch
            {
                Status.Success => NoContent(),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException($"Unexpected result: {response.Status} - {response.Message}"),
            };
        }

        [HttpGet("reserved")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetReservedBikes()
        {
            var response = new GetAllBikesResponse
            {
                Bikes = _bikesService.GetReservedBikes().Object
                    .Select(bike => new GetBikeResponse
                    {
                        Id = bike.Id.ToString(),
                        Station = bike.Station is null ? null : new GetStationResponse
                        {
                            Id = bike.Station.Id.ToString(),
                            Name = bike.Station.Name,
                            Status = bike.Station.Status,
                            ActiveBikesCount = bike.Station.Bikes.Count(b => b.Status is BikeStatus.Available),
                        },
                        User = bike.User is null ? null : new GetUserResponse
                        {
                            Id = bike.User.Id.ToString(),
                            Name = bike.User.Username,
                        },
                        Status = bike.Status,
                    }),
            };

            return Ok(response);
        }
    }
}
