using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class BlockStation : StationsServiceTestsBase
    {
        [Fact]
        public void BlockStationShouldReturnBlockedStation()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Working,
                Id = 23,
            };
            var stationBlocked = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
            };
            var blockStationRequest = new BlockStationRequest
            {
                Id = station.Id.ToString()
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            StationsRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<StationStatus>(s => s == StationStatus.Blocked)))
                .Returns(stationBlocked)
                .Verifiable();
            
            var stationsService = GetStationsService();
            var result = stationsService.BlockStation(blockStationRequest);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Name.Should().Be(station.Name);
            result.Object.Status.Should().Be(StationStatus.Blocked);
            StationsRepository.Verify();
        }

        [Fact]
        public void BlockNotExistingStationShouldReturnEntityNotFound()
        {
            var station = new Station
            {
                Id = 23,
            };
            var blockStationRequest = new BlockStationRequest
            {
                Id = station.Id.ToString(),
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

            var stationsService = GetStationsService();
            var result = stationsService.BlockStation(blockStationRequest);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void BlockAlreadyBlockedStationShouldReturnInvalidState()
        {
            var station = new Station
            {
                Name = "Trailer Park",
                Status = StationStatus.Blocked,
                Id = 23,
            };
            var blockStationRequest = new BlockStationRequest
            {
                Id = station.Id.ToString(),
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();
            var result = stationsService.BlockStation(blockStationRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
