using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BikesRentalServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikesRentalServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationsService stationsService;

        public StationsController(IStationsService stationsService)
        {
            this.stationsService = stationsService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<GetStationResponse>> GetAllStations()
        {
            return Ok(stationsService.GetAllStations()
                .Select(station => new GetStationResponse
                {
                    Id = station.Id,
                    Name = station.Name,
                    Description = station.Description
                }));
        }

        [HttpGet("{id}")]
        public ActionResult<GetStationResponse> GetStation(string id, [FromQuery] string query) // ..../api/stations/123
        {
            if (id.Length > 3)
                return NotFound();
            return new GetStationResponse
            {
                Id = id,
                Name = "Test",
                Description = "Another test"
            };
        }

        [HttpPost]
        public IActionResult AddStation(AddStationRequest request)
        {
            Console.WriteLine(request.Status);
            return Ok(request);
        }
    }
}
