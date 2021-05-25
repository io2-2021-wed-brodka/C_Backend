using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.StationsServiceTests
{
    public class GetAllBikesAtStation : StationsServiceTestsBase
    {
        [Fact]
        public void GetAllBikesAtStationShouldReturnEmptyIEnumerateIfNoBikesAtStation()
        {
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = new List<Bike>(),
                Status = StationStatus.Active,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService();
            var result = stationsService.GetActiveBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllBikesAtStationForAdminShouldReturnAllBikesAtStation()
        {
            var bikes = new List<Bike>
            {
                new Bike
                {
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Status = BikeStatus.Reserved,
                },
                new Bike
                {
                    Status = BikeStatus.Blocked,
                },
            };
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = bikes,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService("maklovitz", UserRole.Admin);
            var result = stationsService.GetActiveBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bikes);
        }

        [Fact]
        public void GetAllBikesAtStationForTechShouldReturnAllBikesAtStation()
        {
            var bikes = new List<Bike>
            {
                new Bike
                {
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Status = BikeStatus.Reserved,
                },
                new Bike
                {
                    Status = BikeStatus.Blocked,
                },
            };
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = bikes,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService("maklovitz", UserRole.Tech);
            var result = stationsService.GetActiveBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bikes);
        }

        [Fact]
        public void GetAllBikesAtStationForUserShouldReturnAllAvailableBikesAtStation()
        {
            var availableBikes = new []
            {
                new Bike
                {
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Status = BikeStatus.Available,
                },
            };
            var station = new Station
            {
                Name = "Szpital psychiatryczny",
                Id = 2,
                Bikes = availableBikes.Concat(new []
                {
                    new Bike
                    {
                        Status = BikeStatus.Blocked,
                    },
                    new Bike
                    {
                        Status = BikeStatus.Reserved,
                    },
                }).ToList(),
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);

            var stationsService = GetStationsService("maklovitz", UserRole.User);
            var result = stationsService.GetActiveBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(availableBikes);
        }

        [Fact]
        public void GetAllBikesAtNotExistingStationShouldReturnEntityNotFound()
        {
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);

            var stationsService = GetStationsService();
            var result = stationsService.GetActiveBikesAtStation("4");

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
                Status = StationStatus.Blocked,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            
            var stationsService = GetStationsService("user", UserRole.User);
            var result = stationsService.GetActiveBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
