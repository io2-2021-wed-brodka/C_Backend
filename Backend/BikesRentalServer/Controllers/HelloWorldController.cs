using BikesRentalServer.Dtos;
using BikesRentalServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BikesRentalServer.Controllers
{
    [Route("/")]
    public class HelloWorldController : ControllerBase
    {
        private IHelloWorldService helloWorldService;

        public HelloWorldController(IHelloWorldService helloWorldService)
        {
            this.helloWorldService = helloWorldService;
        }

        [HttpGet()]
        public ActionResult<HelloWorldDto> GetHello()
        {
            var helloModel = helloWorldService.GetHello();
            return Ok(new HelloWorldDto
            {
                Text = $"Hello {helloModel.Name}!"
            });
        }

        [HttpGet("{name}")]
        public ActionResult<HelloWorldDto> GetHello(string name)
        {
            var helloModel = helloWorldService.GetHello(name);
            return Ok(new HelloWorldDto
            {
                Text = $"Hello {helloModel.Name}!"
            });
        }
    }
}