using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests
{
    public class BikesServiceTests
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesService _bikesService;
        
        public BikesServiceTests()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesService = new BikesService(_dbContext);
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
            var result = _bikesService.RemoveBike(new RemoveBikeRequest
            {
                BikeId = bike.Id.ToString(),
            });
            
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
            
            var result = _bikesService.RemoveBike(new RemoveBikeRequest
            {
                BikeId = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike.Id, result.Object.Id);
            Assert.Equal(bike.Description, result.Object.Description);
            Assert.Equal(bike.Station, result.Object.Station);
            Assert.Equal(bike.Status, result.Object.Status);
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.RemoveBike(new RemoveBikeRequest
            {
                BikeId = "1",
            });
            
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
            
            var result = _bikesService.RemoveBike(new RemoveBikeRequest
            {
                BikeId = bike.Id.ToString(),
            });
            
            Assert.Equal(Status.InvalidState, result.Status);
            Assert.Null(result.Object);
        }
        
        #endregion
    }
}
