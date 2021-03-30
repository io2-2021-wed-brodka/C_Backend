using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Tests.Mock;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests
{
    public class BikesServiceTests
    {
        [Fact]
        public void AddBikeTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;


            using (var context = new DatabaseContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Name = "Wojciech",
                    Lastname = "Szulc",
                    State = UserState.Active,
                    Rentals = new List<Rental>(),
                    Reservations = new List<Reservation>()
                });
                context.Users.Add(new User
                {
                    Id = 2,
                    Name = "Weronika",
                    Lastname = "Bomba",
                    State = UserState.Active,
                    Rentals = new List<Rental>(),
                    Reservations = new List<Reservation>()
                });
                context.Stations.Add(new Station
                {
                    Id = 0,
                    Status = BikeStationStatus.Working,
                    Bikes = new List<Bike>(),
                    Name = "Al. Jerozolimskie"
                });
                context.SaveChanges();

                // run your test here
                Mock<BikesService> bikesService = new Mock<BikesService>(context);

                // _stationsService.AddStation
                Assert.True(0 == bikesService.Object.GetAllBikes().Count());
                var request = new Dtos.Requests.AddBikeRequest { StationId = context.Stations.FirstOrDefault().Id.ToString(), BikeDescription = "", BikeStatus = Models.BikeStatus.Working };
                var response = bikesService.Object.AddBike(request);

                Assert.True(response.Object != null);
                Assert.True(1 == bikesService.Object.GetAllBikes().Count());
            }
        }
    }
}
