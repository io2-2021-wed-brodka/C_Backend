using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class RemoveBike : BikesServiceTestsBase
    {
        public RemoveBike() : base()
        {
        }

        [Fact]
        public void RemoveBikeShouldRemoveBike()
        {
            var bikeId = 1234;

            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Blocked
            });
            BikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            var bikeId = 1234;

            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            BikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Remove(It.IsAny<Bike>()), Times.Never);
        }

        [Fact]
        public void RemoveNotBlockedBikeShouldReturnInvalidState()
        {
            var bikeId = 1234;

            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Working
            });
            BikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Remove(It.IsAny<Bike>()), Times.Never);
        }
    }
}