using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class GetActiveStations : StationsServiceTestsBase
    {
        public GetActiveStations() : base()
        {
        }

        [Fact]
        public void GetActiveStationsShouldReturnEmptyIEnumerableIfNoStations()
        {
            _stationsRepository.Setup(r => r.GetActive()).Returns(new List<Station>());

            var stationsService = GetStationsService();

            var result = stationsService.GetActiveStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetActiveStationsShouldReturnAllActiveStationsAndNoBlockedStations()
        {
            var workingStations = new[]
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

            _stationsRepository.Setup(r => r.GetActive()).Returns(workingStations);

            var stationsService = GetStationsService();

            var result = stationsService.GetActiveStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(workingStations);
        }
    }
}
