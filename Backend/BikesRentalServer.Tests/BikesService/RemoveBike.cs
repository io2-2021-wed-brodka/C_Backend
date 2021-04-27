using BikesRentalServer.Models;
using BikesRentalServer.Services;
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

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Blocked
            });
            _bikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            var bikeId = 1234;

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            _bikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.Remove(It.IsAny<Bike>()), Times.Never);
        }

        [Fact]
        public void RemoveNotBlockedBikeShouldReturnInvalidState()
        {
            var bikeId = 1234;

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Working
            });
            _bikesRepository.Setup(r => r.Remove(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.Remove(It.IsAny<Bike>()), Times.Never);
        }
    }
}