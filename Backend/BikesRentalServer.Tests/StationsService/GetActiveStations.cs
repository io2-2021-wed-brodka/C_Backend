using BikesRentalServer.DataAccess;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetActiveStations
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public GetActiveStations()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void GetActiveStationsShouldReturnEmptyIEnumerableIfNoStations()
        {
            var result = _stationsService.GetActiveStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetActiveStationsShouldReturnAllActiveStationsAndNoBlockedStations()
        {
            var addedBlockedStations = new[]
            {
                new Station
                {
                    Id = 1,
                    Status = StationStatus.Blocked,
                    Name = "First station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 3,
                    Status = StationStatus.Blocked,
                    Name = "Second station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 4,
                    Status = StationStatus.Blocked,
                    Name = "Third station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 6,
                    Status = StationStatus.Blocked,
                    Name = "Forth station",
                    Bikes = new List<Bike>(),
                },
            };

            var addedWorkingStations = new[]
            {
                new Station
                {
                    Id = 7,
                    Status = StationStatus.Working,
                    Name = "Working station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 8,
                    Status = StationStatus.Working,
                    Name = "station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 9,
                    Status = StationStatus.Working,
                    Name = "station7",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 69,
                    Status = StationStatus.Working,
                    Name = "station8",
                    Bikes = new List<Bike>(),
                },
            };

            _dbContext.Stations.AddRange(addedBlockedStations);
            _dbContext.Stations.AddRange(addedWorkingStations);
            _dbContext.SaveChanges();

            var result = _stationsService.GetActiveStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(addedWorkingStations);
            result.Object.Where(s => s.Status == StationStatus.Blocked).Should().BeEmpty();
        }
    }
}
