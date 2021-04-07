using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class RemoveBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        
        public RemoveBike()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesService = new Services.BikesService(_dbContext, new UserContext());
        }
        
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
    }
}
