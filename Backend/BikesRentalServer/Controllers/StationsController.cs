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
    [Route("[controller]")]
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
            var respose = stationsService.GetAllStations()
                .Select(station => new GetStationResponse
                {
                    Id = station.Id,
                    Name = station.Name,
                    Description = station.Description
                });
            return Ok(respose);
        }

        

        [HttpGet("{id}/bikes")]
        public ActionResult<IEnumerable<BikeResponse>> GetAllBikesAtStation(int id)
        {
            var response = stationsService.GetAllBikesAtStation(id)
                 .Select(bike => new BikeResponse
                 {
                     id = bike.Id.ToString(),

                     station = bike.Station != null ? new BikeResponse.Station
                     {
                         id = bike.Station.Id.ToString(),
                         name = bike.Station.Location      //temporary Location instead of Name
                     } : null,
                     user = bike.User != null ? new BikeResponse.User
                     {
                         id = bike.User.Id.ToString(),
                         name = bike.User.Name
                     } : null,
                     state = bike.State
                 });
            return Ok(response);
        }
    }
}
