using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface IHelloWorldService
    {
        HelloWorldModel GetHello();
        HelloWorldModel GetHello(string name);
    }
}