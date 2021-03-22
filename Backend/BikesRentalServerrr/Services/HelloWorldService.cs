using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;

namespace BikesRentalServer.Services
{
    public class HelloWorldService : IHelloWorldService
    {
        public HelloWorldModel GetHello()
        {
            return new HelloWorldModel
            {
                Name = "fellow user",
                SomeOtherInternalStuff = new Random().Next()
            };
        }

        public HelloWorldModel GetHello(string name)
        {
            return new HelloWorldModel
            {
                Name = name,
                SomeOtherInternalStuff = new Random().Next()
            };
        }
    }
}