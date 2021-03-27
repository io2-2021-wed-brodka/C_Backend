﻿using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationsService _stationsService;

        public StationsController(IStationsService stationsService)
        {
            _stationsService = stationsService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<GetStationResponse>> GetAllStations()
        {
            var response = _stationsService.GetAllStations()
                .Select(station => new GetStationResponse
                {
                    Id = station.Id,
                    Name = station.Name,
                    Description = station.Description
                });
            return Ok(response);
        }

        

        [HttpGet("{id}/bikes")]
        public ActionResult<IEnumerable<BikeResponse>> GetAllBikesAtStation(int id)
        {
            var response = _stationsService.GetAllBikesAtStation(id)
                 .Select(bike => new BikeResponse
                 {
                     Id = bike.Id.ToString(),

                     Station = bike.Station is null ? null : new BikeResponse.StationDto
                     {
                         Id = bike.Station.Id.ToString(),
                         Name = bike.Station.Location      //temporary Location instead of Name
                     },
                     User = bike.User is null ? null : new BikeResponse.UserDto
                     {
                         Id = bike.User.Id.ToString(),
                         Name = bike.User.Name
                     },
                     State = bike.State
                 });
            return Ok(response);
        }
    }
}
