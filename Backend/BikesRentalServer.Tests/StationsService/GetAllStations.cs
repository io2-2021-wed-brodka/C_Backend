using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetAllStations : StationsServiceTestsBase
    {
        [Fact]
        public void GetAllStationsShouldReturnEmptyIEnumerableIfNoStations()
        {
            StationsRepository.Setup(r => r.GetAll()).Returns(new List<Station>());

            var stationsService = GetStationsService();
            var result = stationsService.GetAllStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllStationsShouldReturnAllStationsAndNoBlockedStations()
        {
            var workingStations = new[]
            {
                new Station
                {
                    Id = 7,
                    Status = StationStatus.Active,
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
                new Station
                {
                    Id = 9,
                    Status = StationStatus.Active,
                    Name = "station7",
                    Bikes = new List<Bike>(),
                },
                new Station
                {
                    Id = 69,
                    Status = StationStatus.Active,
                    Name = "station8",
                    Bikes = new List<Bike>(),
                },
            };
            StationsRepository.Setup(r => r.GetAll()).Returns(workingStations);

            var stationsService = GetStationsService();
            var result = stationsService.GetAllStations();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(workingStations);
        }
    }
}
