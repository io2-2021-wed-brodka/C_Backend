using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class BlockBike : BikesServiceTestsBase
    {
        [Fact]
        public void BlockBikeShouldSucceed()
        {
            const int bikeId = 123;
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                })
                .Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available
                });

            var bikesService = GetBikesService();
            var result = bikesService.BlockBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Blocked);
        }

        [Fact]
        public void BlockBikeShouldChangeBikeStatusForBlocked()
        {
            const int bikeId = 123;
            BikesRepository.Setup(r => r.SetStatus(It.Is<int>(id => id == bikeId), It.Is<BikeStatus>(b => b == BikeStatus.Blocked)))
                .Returns(new Bike())
                .Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available
                });

            var bikesService = GetBikesService();
            var result = bikesService.BlockBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void BlockNotExistingBikeShouldReturnEntityNotFound()
        {
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>())).Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);

            var bikesService = GetBikesService();
            var result = bikesService.BlockBike("123");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact] 
        public void BlockAlreadyBlockedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>())).Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                });

            var bikesService = GetBikesService();
            var result = bikesService.BlockBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact] 
        public void BlockRentedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>())).Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Rented,
                    User = new User
                    {
                        Id = 2,
                        Username = "some guy",
                    },
                });

            var bikesService = GetBikesService();
            var result = bikesService.BlockBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<int>(), It.IsAny<BikeStatus>()), Times.Never);
        }
    }
}
