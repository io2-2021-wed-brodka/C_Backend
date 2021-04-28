using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class GetAllBikesAtStation : StationsServiceTestsBase
    {
        public GetAllBikesAtStation() : base()
        {
        }

        [Fact]
        public void GetAllBikesAtStationShouldReturnEmptyIEnumerateIfNoBikesAtStation()
        {
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = new List<Bike>(),
                Status = StationStatus.Working
            };
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();

            var result = stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllBikesAtStationShouldReturnAllBikesAtStation()
        {
            var bikes = new List<Bike>()
            {
                new Bike(),
                new Bike(),
                new Bike(),
            };
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = bikes
            };
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();

            var result = stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bikes);
        }

        [Fact]
        public void GetAllBikesAtNotExistingStationShouldReturnEntityNotFound()
        {
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

            var stationsService = GetStationsService();

            var result = stationsService.GetAllBikesAtStation("4");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void GetAllBikesAtBlockedStationForUserShouldReturnInvalidState()
        {
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = new List<Bike>(),
                Status = StationStatus.Blocked
            };
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();

            var result = stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
