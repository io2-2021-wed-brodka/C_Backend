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

        public BikesController(IBikesService stationsService)
        {
            this.bikesService = stationsService;
        }

        // GET: api/<BikesController>
        [HttpGet]
        public ActionResult<IEnumerable<GetBikeResponse>> GetAllStations()
        {
            return Ok(bikesService.GetAllBikes()
                .Select(bike => new GetBikeResponse
                {
                    id = bike.Id.ToString(),
                    station = null,
                    user = null
                }));
        }

        // GET api/<BikesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}
