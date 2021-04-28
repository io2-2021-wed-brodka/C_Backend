using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class UnblockStation : StationsServiceTestsBase
    {
        [Fact]
        public void UnblockStationShouldReturnUnblockedStation()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Active,
                Id = 23,
            };
            var stationBlocked = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(stationBlocked);
            StationsRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<StationStatus>(s => s == StationStatus.Active))).Returns(station).Verifiable();

            var stationsService = GetStationsService();
            var result = stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Name.Should().Be(station.Name);
            result.Object.Status.Should().Be(StationStatus.Active);
            StationsRepository.Verify();
        }

        [Fact]
        public void UnblockNotExistingStationShouldReturnEntityNotFound()
        {
            var station = new Station
            {
                Id = 23,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

            var stationsService = GetStationsService();
            var result = stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void UnblockAlreadyUnblockedStationShouldReturnInvalidState()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Active,
                Id = 23,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();
            var result = stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
