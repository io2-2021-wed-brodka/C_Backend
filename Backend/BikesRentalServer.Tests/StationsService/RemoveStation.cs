using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class RemoveStation : StationsServiceTestsBase
    {
        public RemoveStation() : base()
        {
        }

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

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            _stationsRepository.Setup(r => r.Remove(It.Is<string>(id => id == station.Id.ToString())))
                .Returns(station).Verifiable();
            var stationsService = GetStationsService();

            var result = stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            _stationsRepository.Verify();
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

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);
            _stationsRepository.Setup(r => r.Remove(It.Is<string>(id => id == station.Id.ToString())))
                .Returns(station).Verifiable();

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

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();

            var result = stationsService.RemoveStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
