﻿using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class UnblockStation : StationsServiceTestsBase
    {
        public UnblockStation()
        {
        }

        [Fact]
        public void UnblockStationShouldReturnUnblockedStation()
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

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(stationBlocked);
            _stationsRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<StationStatus>(s => s == StationStatus.Working)))
                .Returns(station).Verifiable();
            var stationsService = GetStationsService();

            var result = stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Name.Should().Be(station.Name);
            result.Object.Status.Should().Be(StationStatus.Working);
            _stationsRepository.Verify();
        }

        [Fact]
        public void UnblockNotExistingStationShouldReturnEntityNotFound()
        {
            var station = new Station
            {
                Id = 23,
            };

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

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
                Status = StationStatus.Working,
                Id = 23,
            };

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();

            var result = stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
