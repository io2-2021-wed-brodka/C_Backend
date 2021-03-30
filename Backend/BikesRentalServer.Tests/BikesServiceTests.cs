using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using Moq;
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
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            var request = new Dtos.Requests.AddBikeRequest { StationId = context.Stations.FirstOrDefault().Id.ToString(), BikeDescription = "", BikeStatus = Models.BikeStatus.Working };
            var response = bikesService.Object.AddBike(request);

            Assert.True(response.Object != null);
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
        }

        [Fact]
        public void AddBikeWrongStationTest()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            var request = new Dtos.Requests.AddBikeRequest { StationId = "15", BikeDescription = "", BikeStatus = Models.BikeStatus.Working };
            var response = bikesService.Object.AddBike(request);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
        }

        [Fact]
        public void RemoveBlockedBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            context.Bikes.Add(new Bike() { Id = 1, Status = BikeStatus.Blocked, Station = context.Stations.FirstOrDefault() });
            context.SaveChanges();
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());

            var requestRemove = new Dtos.Requests.RemoveBikeRequest { BikeId = "1" };
            var responseRemove = bikesService.Object.RemoveBike(requestRemove);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
        }

        [Fact]
        public void RemoveWorkingBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            context.Bikes.Add(new Bike() { Id = 1, Status = BikeStatus.Working, Station = context.Stations.FirstOrDefault() });
            context.SaveChanges();
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());

            var requestRemove = new Dtos.Requests.RemoveBikeRequest { BikeId = "1" };
            var responseRemove = bikesService.Object.RemoveBike(requestRemove);

            // Working bikes should not be removed.
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
        }

        [Fact]
        public void RemoveNotExistingBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            context.Bikes.Add(new Bike() { Id = 1, Status = BikeStatus.Working, Station = context.Stations.FirstOrDefault() });
            context.SaveChanges();
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());

            var requestRemove = new Dtos.Requests.RemoveBikeRequest { BikeId = "5" };
            var responseRemove = bikesService.Object.RemoveBike(requestRemove);

            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
        }

        [Fact]
        public void RemoveRentedBike()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
            Assert.True(0 == context.Rentals.Count());

            Bike bike = new Bike() { Id = 1, Status = BikeStatus.Blocked, Station = null };
            Rental rental = new Rental() { Bike = bike };
            User user = new User() { Rentals = new List<Rental>() { rental } };
            rental.User = user;
            bike.User = user;

            context.Bikes.Add(bike);
            context.Users.Add(user);
            context.Rentals.Add(rental);
            context.SaveChanges();

            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
            Assert.True(1 == context.Rentals.Count());

            var requestRemove = new Dtos.Requests.RemoveBikeRequest { BikeId = bike.Id.ToString() };
            var responseRemove = bikesService.Object.RemoveBike(requestRemove);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
            Assert.True(0 == context.Rentals.Count());
        }

        [Fact]
        public void RemoveReservedBike()
        {
            var context = MockedDbFactory.GetContext();
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
            Assert.True(0 == context.Reservations.Count());

            Bike bike = new Bike() { Id = 1, Status = BikeStatus.Blocked, Station = context.Stations.FirstOrDefault() };
            User user = new User();
            Reservation reservation = new Reservation() { Bike = bike, User = user };
            user.Reservations = new List<Reservation>() { reservation };

            context.Bikes.Add(bike);
            context.Users.Add(user);
            context.Reservations.Add(reservation);
            context.SaveChanges();

            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
            Assert.True(1 == context.Reservations.Count());

            var requestRemove = new Dtos.Requests.RemoveBikeRequest { BikeId = bike.Id.ToString() };
            var responseRemove = bikesService.Object.RemoveBike(requestRemove);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());
            Assert.True(0 == context.Reservations.Count());
        }
    }
}
