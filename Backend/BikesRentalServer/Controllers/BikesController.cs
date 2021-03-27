using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly IBikesService _bikesService;

        public BikesController(IBikesService bikesService)
        {
            _bikesService = bikesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BikeResponse>> GetAllBikes()
        {
            
            var response = _bikesService.GetAllBikes()
                .Select(bike => new BikeResponse
                {
                    Id = bike.Id.ToString(),
                    Station = bike.Station is null ? null : new BikeResponse.StationDto 
                    {
                        Id = bike.Station.Id.ToString(),
                        Name = bike.Station.Location          // temporary Location instead of Name
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

        // GET <BikesController>/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<BikeResponse>> Get(int id)
        {
            var response = _bikesService.GetBike(id.ToString());
            return response is null ? NotFound($"Not found bike with id {id}") : Ok(response);
        }
    }
}
