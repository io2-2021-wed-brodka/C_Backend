using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GiveBikeBack : BikesServiceTestsBase
    {
        [Fact]
        public void GiveBikeBackShouldSucceed()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            var station = new Station
            {
                Id = 1,
                Status = StationStatus.Working,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    User = user,
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), station))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Station = station,
                });

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GiveBikeBack(bikeId.ToString(), station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Working);
        }

        [Fact]
        public void GiveBikeBackShouldAssignBikeToStation()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            var station = new Station
            {
                Id = 1,
                Status = StationStatus.Working
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    User = user,
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.Is<Station>(s => s.Id == station.Id)))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Station = station,
                })
                .Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GiveBikeBack(bikeId.ToString(), station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Station.Id.Should().Be(station.Id);
        }

        [Fact]
        public void GiveBikeBackToNotExistingStationShouldReturnEntityNotFound()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    User = user,
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>())).Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GiveBikeBack(bikeId.ToString(), "1");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void GiveNotExistingBikeBackShouldReturnEntityNotFound()
        {
            const int stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), station)).Verifiable();
            
            var bikesService = GetBikesService(thisUser.Username);
            var result = bikesService.GiveBikeBack("123", stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void GiveNotOwnBikeBackShouldReturnEntityNotFound()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            var otherUser = new User
            {
                Id = 1,
                Username = "Czesiek",
            };
            var station = new Station
            {
                Id = 1,
                Status = StationStatus.Working
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(station);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    User = otherUser,
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), station)).Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GiveBikeBack(bikeId.ToString(), station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void GiveBikeBackToBlockedStationShouldReturnInvalidState()
        {
            const int bikeId = 123;
            const int stationId = 2;
            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Status = StationStatus.Blocked,
                    Id = stationId,
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<User>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.GiveBikeBack(bikeId.ToString(), stationId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        }
    }
}