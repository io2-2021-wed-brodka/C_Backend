using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class GiveBackBike : BikesServiceTestsBase
    {
        public GiveBackBike() : base()
        {
        }

        [Fact]
        public void GiveBikeBackShouldSucceed()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Working);
        }

        [Fact]
        public void GiveBikeBackShouldAssignBikeToStation()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };

            BikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Station.Id.Should().Be(stationId);
        }

        [Fact]
        public void GiveBikeBackToNotExistingStationShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };

            BikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.IsAny<Station>())
            ).Verifiable();
            BikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Station)null);

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void GiveNotExistingBikeBackShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };

            BikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Bike)null);

            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void GiveNotOwnBikeBackShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var otherUser = new User
            {
                Id = 1,
                Username = "Czesiek"
            };

            BikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = otherUser,
                });

            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }
    }
}