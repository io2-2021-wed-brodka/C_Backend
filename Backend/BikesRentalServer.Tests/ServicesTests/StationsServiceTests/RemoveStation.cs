using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.StationsServiceTests
{
    public class RemoveStation : StationsServiceTestsBase
    {
        [Fact]
        public void RemoveStationShouldRemoveStation()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
                Bikes = new List<Bike>()
            };

            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            StationsRepository.Setup(r => r.Remove(It.Is<int>(id => id == station.Id))).Returns(station).Verifiable();
            
            var stationsService = GetStationsService();
            var result = stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            StationsRepository.Verify();
        }

        [Fact]
        public void RemoveNotExistingStationShouldReturnEntityNotFound()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
            };

            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);
            StationsRepository.Setup(r => r.Remove(It.Is<int>(id => id == station.Id))).Returns(station).Verifiable();

            var stationsService = GetStationsService();
            var result = stationsService.RemoveStation("3");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void RemoveStationWithBikesShouldReturnInvalidState()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
                Bikes = new List<Bike>() { new Bike() }
            };

            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();
            var result = stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
