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
    public class GetBlockedStations
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public GetBlockedStations()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void GetBlockedStationsShouldReturnEmptyIEnumerableIfNoStations()
        {
            var result = _stationsService.GetBlockedStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlockedStationsShouldReturnAllBlockedStationsAndNoActiveStations()
        {
            var addedBlockedStations = new[]
            {
                new Station
                {
                    Id = 1,
                    Status = BikeStationStatus.Blocked,
                    Name = "First station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 3,
                    Status = BikeStationStatus.Blocked,
                    Name = "Second station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 4,
                    Status = BikeStationStatus.Blocked,
                    Name = "Third station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 6,
                    Status = BikeStationStatus.Blocked,
                    Name = "Forth station",
                    Bikes = new List<Bike>(),
                },
            };

            var addedWorkingStations = new[]
            {
                new Station
                {
                    Id = 7,
                    Status = BikeStationStatus.Working,
                    Name = "Working station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 8,
                    Status = BikeStationStatus.Working,
                    Name = "station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 9,
                    Status = BikeStationStatus.Working,
                    Name = "station7",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 69,
                    Status = BikeStationStatus.Working,
                    Name = "station8",
                    Bikes = new List<Bike>(),
                },
            };

            _dbContext.Stations.AddRange(addedBlockedStations);
            _dbContext.Stations.AddRange(addedWorkingStations);
            _dbContext.SaveChanges();

            var result = _stationsService.GetBlockedStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(addedBlockedStations);
            result.Object.Where(s => s.Status == BikeStationStatus.Working).Should().BeEmpty();
        }
    }
}
