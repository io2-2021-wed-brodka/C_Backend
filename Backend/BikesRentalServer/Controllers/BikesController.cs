using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BikesRentalServer.Controllers
{
    [Route("/[controller]")]
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
                            Name = bike.Station.Name,
                        },
                        User = bike.User is null ? null : new GetBikeResponse.UserDto
                        {
                            Id = bike.User.Id.ToString(),
                            Name = bike.User.FirstName,
                        },
                        Status = bike.Status,
                    }),
            };

            return Ok(response); 
        }

        [HttpGet("{id}")]
        public ActionResult<GetBikeResponse> GetBike(int id)
        {
            var response = _bikesService.GetBike(id.ToString());

            if (response is null)
                return NotFound("Bike not found");
            return Ok(new GetBikeResponse
            {
                Id = response.Id.ToString(),
                Status = response.Status,
                Station = response.Station is null ? null : new GetBikeResponse.StationDto
                {
                    Id = response.Station.Id.ToString(),
                    Name = response.Station.Name,
                },
                User = response.User is null ? null : new GetBikeResponse.UserDto
                {
                    Id = response.User.Id.ToString(),
                    Name = response.User.FirstName,
                },
            });
        }
    }
}
