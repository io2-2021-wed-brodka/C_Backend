using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.StationsServiceTests
{
    public class GetBlockedStations : StationsServiceTestsBase
    {
        [Fact]
        public void GetBlockedStationsShouldReturnEmptyIEnumerableIfNoStations()
        {
            StationsRepository.Setup(r => r.GetActive()).Returns(new List<Station>());

            var stationsService = GetStationsService();
            var result = stationsService.GetActiveStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlockedStationsShouldReturnAllBlockedStationsAndNoActiveStations()
        {
            var blockedStations = new[]
            {
                new Station
                {
                    Id = 7,
                    Status = StationStatus.Blocked,
                    Name = "Working station",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 8,
                    Status = StationStatus.Blocked,
                    Name = "station",
                    Bikes = new List<Bike>(),
                },
            };
            StationsRepository.Setup(r => r.GetBlocked()).Returns(blockedStations);

            var stationsService = GetStationsService();
            var result = stationsService.GetBlockedStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(blockedStations);
        }
    }
}
