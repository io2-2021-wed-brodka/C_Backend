using BikesRentalServer.Authorization;
using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            var response = new GetAllStationsResponse
            {
                Stations = _stationsService.GetAllStations()
                .Select(station => new GetStationResponse
                {
                    Id = station.Id.ToString(),
                    Name = station.Name,
                }),
            };
            return Ok(response);
        }

        [HttpGet("{id}/bikes")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetAllBikesResponse> GetAllBikesAtStation(string id)
        {
            var bikes = _stationsService.GetAllBikesAtStation(id);
            if (bikes is null)
            {
                return NotFound("Station not found");
            }

            var response = new GetAllBikesResponse
            {
                Bikes = bikes
                 .Select(bike => new GetBikeResponse
                 {
                     Id = bike.Id.ToString(),
                 }),
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        [TechAuthorization]
        [AdminAuthorization]
        public ActionResult<GetStationResponse> GetStation(string id)
        {
            var response = _stationsService.GetStation(id);

            if (response is null)
                return NotFound("Station not found");
            return Ok(new GetStationResponse
            {
                Id = response.Id.ToString(),           
                Name = response.Name,
            });
        }
    }
}
