using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class UnblockBike : BikesServiceTestsBase
    {
        [Fact]
        public void UnlockBikeShouldSucceed()
        {
            const int bikeId = 123;

            BikesRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<BikeStatus>(s => s == BikeStatus.Working)))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                })
                .Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                });

            var bikesService = GetBikesService();
            var result = bikesService.UnblockBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Working);
        }

        [Fact]
        public void UnlockNotExistingBikeShouldReturnEntityNotFound()
        {
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())).Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);

            var bikesService = GetBikesService();
            var result = bikesService.UnblockBike("123");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void UnlockNotBlockedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())).Verifiable();
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                });

            var bikesService = GetBikesService();
            var result = bikesService.UnblockBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }
    }
}
