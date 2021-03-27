using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Dtos.Responses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikesRentalServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly IBikesService bikesService;

        public BikesController(IBikesService bikesService)
        {
            this.bikesService = bikesService;
        }

        // GET:/<BikesController>
        [HttpGet]
        public ActionResult<IEnumerable<BikeResponse>> GetAllBikes()
        {
            
            var response = bikesService.GetAllBikes()
                .Select(bike => new BikeResponse
                {
                    id = bike.Id.ToString(),
                    station = bike.Station!=null? new BikeResponse.Station 
                    {
                        id=bike.Station.Id.ToString(),
                        name=bike.Station.Location          //temporary Location instead of Name
                    } : null,
                    user = bike.User!=null? new BikeResponse.User
                    {
                        id = bike.User.Id.ToString(),
                        name = bike.User.Name
                    }: null,
                    state = bike.State
                });

            return Ok(response); 
        }

        // GET <BikesController>/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<BikeResponse>> Get(int id)
        {
            var response = bikesService.GetBike(id.ToString());
            if(response!=null)
            {
                return Ok(response);
            }
            return NotFound("Not found bike with id " + id.ToString());
        }

    }
}
