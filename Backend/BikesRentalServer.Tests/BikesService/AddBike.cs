using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class AddBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        
        public AddBike()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesService = new Services.BikesService(_dbContext, new UserContext());
        }

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

            result.Status.Should().Be(Status.Success);
            _dbContext.Bikes.Count().Should().Be(initialBikeCount + 1);
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
            
            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Station.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void AddBikeToNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _bikesService.AddBike(new AddBikeRequest
            {
                StationId = "3",
            });
            
            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
