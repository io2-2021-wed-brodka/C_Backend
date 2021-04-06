using BikesRentalServer.Dtos.Requests;
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
            var bikesService = new Mock<BikesService>(context);

            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            var response = bikesService.Object.AddBike(new AddBikeRequest 
            { 
                StationId = context.Stations.First().Id.ToString(),
            });

            Assert.Equal(initialBikeCount + 1, bikesService.Object.GetAllBikes().Object.Count());
            Assert.NotNull(response.Object);
        }

        [Fact]
        public void AddBikeWrongStationTest()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            bikesService.Object.AddBike(new AddBikeRequest
            {
                StationId = "15",
            });

            Assert.Equal(initialBikeCount, bikesService.Object.GetAllBikes().Object.Count());
        }

        [Fact]
        public void RemoveBlockedBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            context.Bikes.Add(new Bike
            {
                Status = BikeStatus.Blocked,
                Station = context.Stations.FirstOrDefault(),
            });
            context.SaveChanges();
            
            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            bikesService.Object.RemoveBike(new RemoveBikeRequest
            {
                BikeId = "1",
            });

            Assert.Equal(initialBikeCount - 1, bikesService.Object.GetAllBikes().Object.Count());
        }

        [Fact]
        public void RemoveWorkingBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            context.Bikes.Add(new Bike
            {
                Id = 1,
                Status = BikeStatus.Working, 
                Station = context.Stations.FirstOrDefault(),
            });
            context.SaveChanges();

            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            bikesService.Object.RemoveBike(new RemoveBikeRequest
            {
                BikeId = "1",
            });

            Assert.Equal(initialBikeCount, bikesService.Object.GetAllBikes().Object.Count());
        }

        [Fact]
        public void RemoveNotExistingBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            context.Bikes.Add(new Bike
            {
                Id = 1,
                Status = BikeStatus.Working,
                Station = context.Stations.FirstOrDefault(),
            });
            context.SaveChanges();
            
            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            bikesService.Object.RemoveBike(new RemoveBikeRequest
            {
                BikeId = "5",
            });

            Assert.Equal(initialBikeCount, bikesService.Object.GetAllBikes().Object.Count());
        }

        [Fact]
        public void RemoveRentedBike()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            var bike = new Bike
            {
                Id = 1, 
                Status = BikeStatus.Blocked, 
                Station = null,
            };
            var rental = new Rental
            {
                Bike = bike,
            };
            var user = new User
            {
                Rentals = new List<Rental>
                {
                    rental,
                },
            };
            rental.User = user;
            bike.User = user;

            context.Bikes.Add(bike);
            context.Users.Add(user);
            context.Rentals.Add(rental);
            context.SaveChanges();

            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            var initialRentalCount = context.Rentals.Count();

            bikesService.Object.RemoveBike(new RemoveBikeRequest
            {
                BikeId = bike.Id.ToString(),
            });

            Assert.Equal(initialBikeCount - 1, bikesService.Object.GetAllBikes().Object.Count());
            Assert.Equal(initialRentalCount - 1, context.Rentals.Count());
        }

        [Fact]
        public void RemoveReservedBike()
        {
            var context = MockedDbFactory.GetContext();
            var bikesService = new Mock<BikesService>(context);

            var bike = new Bike
            {
                Id = 1,
                Status = BikeStatus.Blocked,
                Station = context.Stations.FirstOrDefault(),
            };
            var user = new User();
            var reservation = new Reservation
            {
                Bike = bike,
                User = user,
            };
            user.Reservations = new List<Reservation>
            {
                reservation,
            };
            context.Bikes.Add(bike);
            context.Users.Add(user);
            context.Reservations.Add(reservation);
            context.SaveChanges();

            var initialBikeCount = bikesService.Object.GetAllBikes().Object.Count();
            var initialReservationCount = context.Reservations.Count();

            bikesService.Object.RemoveBike(new RemoveBikeRequest
            {
                BikeId = bike.Id.ToString(),
            });

            Assert.Equal(initialBikeCount - 1, bikesService.Object.GetAllBikes().Object.Count());
            Assert.Equal(initialReservationCount - 1, context.Reservations.Count());
        }                                         
    }
}
