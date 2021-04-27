using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class RemoveStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public RemoveStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void RemoveStationShouldDecrementStationCount()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            
            _dbContext.SaveChanges();

            var initialStationCount = _dbContext.Stations.Count();
            var result = _stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            _dbContext.Stations.Count().Should().Be(initialStationCount - 1);
        }

        [Fact]
        public void RemoveStationShouldReturnRemovedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void RemoveNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.RemoveStation("1");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void RemoveStationWithBikesShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "Al. Jerozolimskie",
                    Bikes = new List<Bike>
                    {
                        new Bike
                        {
                            Status = BikeStatus.Working,
                            Description = "bike",
                        },
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
