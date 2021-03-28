using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<GetAllBikesResponse> GetAllBikes()
        {
            
            var response = new GetAllBikesResponse
            {
                Bikes = _bikesService.GetAllBikes()
                    .Select(bike => new GetBikeResponse
                    {
                        Id = bike.Id.ToString(),
                        Station = bike.Station is null ? null : new GetBikeResponse.StationDto 
                        {
                            Id = bike.Station.Id.ToString(),
                            Name = bike.Station.Name
                        },
                        User = bike.User is null ? null : new GetBikeResponse.UserDto
                        {
                            Id = bike.User.Id.ToString(),
                            Name = bike.User.Name
                        },
                        Status = bike.Status
                    })
            };

            return Ok(response); 
        }

        [HttpGet("{id}")]
        public ActionResult<GetBikeResponse> GetBike(int id)
        {
            var response = _bikesService.GetBike(id.ToString());

            if (response is null)
                return NotFound(new Error
                {
                    Message = "Bike not found"
                });
            return Ok(response);
        }
    }
}
