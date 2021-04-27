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
            var response = new GetAllBikesResponse
            {
                Bikes = _bikesService.GetAllBikes().Object
                    .Select(bike => new GetBikeResponse
                    {
                        Id = bike.Id.ToString(),
                        Station = bike.Station is null ? null : new GetBikeResponse.StationDto 
                        {
                            Id = bike.Station.Id.ToString(),
                            Name = bike.Station.Name,
                        },
                        User = bike.User is null ? null : new GetBikeResponse.UserDto
                        {
                            Id = bike.User.Id.ToString(),
                            Name = bike.User.Username,
                        },
                        Status = bike.Status,
                    }),
            };

            return Ok(response); 
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
                    Station = response.Object.Station is null ? null : new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                    User = response.Object.User is null ? null : new GetBikeResponse.UserDto
                    {
                        Id = response.Object.User.Id.ToString(),
                        Name = response.Object.User.Username,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> AddBike(AddBikeRequest request)
        {
            var response = _bikesService.AddBike(request);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState or _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpDelete("{id}")]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> RemoveBike(string id)
        {
            var response = _bikesService.RemoveBike(id);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = response.Object.Station is null ? null : new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                    User = response.Object.User is null ? null : new GetBikeResponse.UserDto
                    {
                        Id = response.Object.User.Id.ToString(),
                        Name = response.Object.User.Username,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpPost("rented")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> RentBike(RentBikeRequest request)
        {
            var response = _bikesService.RentBike(request);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    User = new GetBikeResponse.UserDto
                    {
                        Id = response.Object.User.Id.ToString(),
                        Name = response.Object.User.Username,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("rented")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetRentedBikes()
        {
            var response = _bikesService.GetRentedBikes();
            return new GetAllBikesResponse
            {
                Bikes = response.Object.Select(bike => new GetBikeResponse
                {
                    Id = bike.Id.ToString(),
                    User = new GetBikeResponse.UserDto
                    {
                        Id = bike.User.Id.ToString(),
                        Name = bike.User.Username,
                    },
                    Status = bike.Status,
                }),
            };
        }

        [HttpPost("blocked")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetBikeResponse> BlockBike(BlockBikeRequest request)
        {
            var response = _bikesService.BlockBike(request);
            return response.Status switch
            {
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
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
                Status.Success => Ok(new GetBikeResponse
                {
                    Id = response.Object.Id.ToString(),
                    Status = response.Object.Status,
                    Station = response.Object.Station is null ? null : new GetBikeResponse.StationDto
                    {
                        Id = response.Object.Station.Id.ToString(),
                        Name = response.Object.Station.Name,
                    },
                }),
                Status.EntityNotFound => NotFound(response.Message),
                Status.InvalidState => UnprocessableEntity(response.Message),
                _ => throw new InvalidOperationException("Invalid state"),
            };
        }

        [HttpGet("blocked")]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetBlockedBikes()
        {
            var response = _bikesService.GetBlockedBikes();
            return new GetAllBikesResponse
            {
                Bikes = response.Object.Select(bike => new GetBikeResponse
                {
                    Id = bike.Id.ToString(),
                    Station = new GetBikeResponse.StationDto
                    {
                        Id = bike.Station.Id.ToString(),
                        Name = bike.Station.Name,
                    },
                    Status = bike.Status,
                }),
            };
        }
    }
}
