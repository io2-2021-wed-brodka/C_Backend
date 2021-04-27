using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
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
                    Status = UserStatus.Active,
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
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void RentBikeShouldAssignBikeToUser()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.Success);
            result.Object.User.Should().Be(_user);
        }

        [Fact]
        public void RentNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = "27",
            });

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void RentBlockedBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            bike.User.Should().BeNull();
        }

        [Fact]
        public void RentAlreadyRentedBikeShouldReturnInvalidState()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
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

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            bike.User.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RentingAboveFourthBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            bike.User.Should().BeNull();
        }

        [Fact]
        public void RentBikeReservedByRequestingUserShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
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
                ExpirationDate = DateTime.Now.AddMinutes(20),
            });
            _dbContext.SaveChanges();

            var initialReservationCount = _user.Reservations.Count;
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
            _user.Reservations.Count.Should().Be(initialReservationCount - 1);
        }

        [Fact]
        public void RentBikeReservedByAnotherUserShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Riwiera",
                    Status = StationStatus.Working,
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
                    Status = UserStatus.Active,
                    Username = "another_one_that_does_not_bite_the_dust",
                })
                .Entity;
            _dbContext.Reservations.Add(new Reservation
            {
                User = user,
                Bike = bike,
                ReservationDate = DateTime.Now.AddMinutes(-10),
                ExpirationDate = DateTime.Now.AddMinutes(20),
            });
            _dbContext.SaveChanges();

            var initialReservationCount = user.Reservations.Count;
            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            user.Reservations.Count.Should().Be(initialReservationCount);
        }

        [Fact]
        public void RentBikeHavingExpiredReservationShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Riwiera",
                    Status = StationStatus.Working,
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
                    Status = UserStatus.Active,
                    Username = "another_one_that_does_not_bite_the_dust",
                })
                .Entity;
            _dbContext.Reservations.Add(new Reservation
            {
                User = user,
                Bike = bike,
                ReservationDate = DateTime.Now.AddMinutes(-40),
                ExpirationDate = DateTime.Now.AddMinutes(-10),
            });
            _dbContext.SaveChanges();

            var result = _bikesService.RentBike(new RentBikeRequest
            {
                Id = bike.Id.ToString(),
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
            result.Object.User.Should().Be(_user);
        }
    }
}
