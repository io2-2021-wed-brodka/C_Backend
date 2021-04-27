using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
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

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
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

            _bikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
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

            _bikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.IsAny<Station>())
            ).Verifiable();
            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = thisUser,
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Station)null);

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            _bikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
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

            _bikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Bike)null);

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            _bikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
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

            _bikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == stationId))
            ).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, // Should be rented
                    User = otherUser,
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            _bikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }
    }
}