using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetStation : StationsServiceTestsBase
    {
        [Fact]
        public void GetExistingStationShouldSucceed()
        {
            var station = new Station
            {
                Name = "Palac Kultury",
                Id = 2,
                Bikes = new List<Bike>(),
                Status = StationStatus.Working
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();
            var result = stationsService.GetStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void GetNotExistingStationShouldReturnEntityNotFound()
        {
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

            var stationsService = GetStationsService();
            var result = stationsService.GetStation("7");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
