using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
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

        [Fact]
        public void RemoveBikeShouldDecrementBikeCount()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.Success);
            _dbContext.Bikes.Count().Should().Be(initialBikeCount - 1);
        }

        [Fact]
        public void RemoveBikeShouldReturnRemovedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.RemoveBike("1");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void RemoveNotBlockedBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
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

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
