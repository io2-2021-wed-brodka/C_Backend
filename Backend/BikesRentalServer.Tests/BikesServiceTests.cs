using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests
{
    public class BikesServiceTests
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesService _bikesService;
        private readonly User _user;
        
        public BikesServiceTests()
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
            
            _bikesService = new BikesService(_dbContext, userContext);
        }

        #region GetAllBikes tests
        
        [Fact]
        public void GetAllBikesShouldReturnAllBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;

            var addedBikes = new []
            {
                new Bike
                {
                    Id = 1,
                    Station = station,
                    Description = "first one!",
                },
                new Bike
                {
                    Id = 2,
                    Station = station,
                    Description = "Another ONE",
                },
                new Bike
                {
                    Id = 4,
                    Station = station,
                    Description = "Skipped one",
                },
                new Bike
                {
                    Id = 7,
                    Station = station,
                    Description = string.Empty,
                },
            };
            _dbContext.Bikes.AddRange(addedBikes);
            _dbContext.SaveChanges();

            var result = _bikesService.GetAllBikes();
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(addedBikes.Length, result.Object.Count());
            Assert.True(addedBikes.OrderBy(b => b.Id).SequenceEqual(result.Object.OrderBy(b => b.Id)));
        }
        
        #endregion
        
        #region GetBike tests

        [Fact]
        public void GetExistingBikeShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.Bikes.Add(new Bike
            {
                Station = station,
            });
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.GetBike(bike.Id.ToString());
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike.Id, result.Object.Id);
            Assert.Equal(bike.Description, result.Object.Description);
            Assert.Equal(bike.Station, result.Object.Station);
            Assert.Equal(bike.Status, result.Object.Status);
            Assert.Equal(bike.Station, result.Object.Station);
        }

        [Fact]
        public void GetNotExistingBikeShouldReturnEntityNotFound()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.Bikes.Add(new Bike
            {
                Id = 1,
                Station = station,
            });
            _dbContext.SaveChanges();

            var result = _bikesService.GetBike("4");
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }
        
        #endregion
        
        #region AddBike tests

        [Fact]
        public void AddBikeShouldIncrementBikeCount()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.SaveChanges();

            var initialBikeCount = _dbContext.Bikes.Count();
            var result = _bikesService.AddBike(new AddBikeRequest
            {
                StationId = station.Id.ToString(),
            });
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(initialBikeCount + 1, _dbContext.Bikes.Count());
        }

        [Fact]
        public void AddBikeShouldReturnCreatedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.AddBike(new AddBikeRequest
            {
                StationId = station.Id.ToString(),
            });
            
            Assert.Equal(Status.Success, result.Status);
            Assert.NotNull(result.Object);
            Assert.Equal(station, result.Object.Station);
        }

        [Fact]
        public void AddBikeToNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _bikesService.AddBike(new AddBikeRequest
            {
                StationId = "3",
            });
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }
        
        #endregion
        
        #region RemoveBike tests

        [Fact]
        public void RemoveBikeShouldDecrementBikeCount()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var initialBikeCount = _dbContext.Bikes.Count();
            var result = _bikesService.RemoveBike(bike.Id.ToString());
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(initialBikeCount - 1, _dbContext.Bikes.Count());
        }

        [Fact]
        public void RemoveBikeShouldReturnRemovedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                    Description = "some things here",
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.RemoveBike(bike.Id.ToString());
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike.Id, result.Object.Id);
            Assert.Equal(bike.Description, result.Object.Description);
            Assert.Equal(bike.Station, result.Object.Station);
            Assert.Equal(bike.Status, result.Object.Status);
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.RemoveBike("1");
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }

        [Fact]
        public void RemoveNotBlockedBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Working,
                    Description = "some things here",
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.RemoveBike(bike.Id.ToString());
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
        }

        #endregion
        
        #region RentBike tests

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
        
        #endregion
    }
}
