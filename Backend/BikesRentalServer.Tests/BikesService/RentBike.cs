using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class RentBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        private readonly User _user;
        
        public RentBike()
        {
            _dbContext = MockedDbFactory.GetContext();
            _user = _dbContext.Users.Add(new User
                {
                    Username = "test_user",
                    State = UserState.Active,
                    Role = UserRole.Admin,
                    Reservations = new List<Reservation>(),
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var userContext = new UserContext();
            userContext.SetOnce(_user.Username, _user.Role);
            
            _bikesService = new Services.BikesService(_dbContext, userContext);
        }
        
        [Fact]
        public void RentBikeShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Description = "some text",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike, result.Object);
        }

        [Fact]
        public void RentNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = "27",
            });
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }

        [Fact]
        public void RentBlockedBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Description = "some text",
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
            Assert.Null(bike.User);
        }

        [Fact]
        public void RentAlreadyRentedBikeShouldReturnInvalidState()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    State = UserState.Active,
                    Username = "another_one_that_bites_the_dust",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    User = user,
                    Description = "some text",
                    Status = BikeStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
            Assert.Equal(user, bike.User);
        }

        [Fact]
        public void RentingAboveFourthBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "bike 1",
                    User = _user,
                },
                new Bike
                {
                    Description = "bike 2",
                    User = _user,
                },
                new Bike
                {
                    Description = "bike 3",
                    User = _user,
                },
                new Bike
                {
                    Description = "bike 4",
                    User = _user,
                },
            });
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "above 4!",
                    Station = station,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
            Assert.Null(bike.User);
        }

        [Fact]
        public void RentBikeReservedByRequestingUserShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "reserved.",
                    Station = station,
                })
                .Entity;
            _dbContext.Reservations.Add(new Reservation
            {
                User = _user,
                Bike = bike,
                ReservationDate = DateTime.Now.AddMinutes(-10),
                ExpiryDate = DateTime.Now.AddMinutes(20),
            });
            _dbContext.SaveChanges();

            var initialReservationCount = _user.Reservations.Count;
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike, result.Object);
            Assert.Equal(initialReservationCount - 1, _user.Reservations.Count);
        }

        [Fact]
        public void RentBikeReservedByAnotherUserShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Riwiera",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "reserved.",
                    Station = station,
                })
                .Entity;
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    State = UserState.Active,
                    Username = "another_one_that_does_not_bite_the_dust",
                })
                .Entity;
            _dbContext.Reservations.Add(new Reservation
            {
                User = user,
                Bike = bike,
                ReservationDate = DateTime.Now.AddMinutes(-10),
                ExpiryDate = DateTime.Now.AddMinutes(20),
            });
            _dbContext.SaveChanges();

            var initialReservationCount = user.Reservations.Count;
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
            Assert.Equal(initialReservationCount, user.Reservations.Count);
        }
    }
}
