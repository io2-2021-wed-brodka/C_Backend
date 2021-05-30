using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class RemoveBike : BikesServiceTestsBase
    {
        [Fact]
        public void RemoveBikeShouldRemoveBike()
        {
            const int bikeId = 1234;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                });
            BikesRepository.Setup(r => r.Remove(It.IsAny<int>())).Returns(new Bike()).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void RemoveNotExistingBikeShouldReturnEntityNotFound()
        {
            const int bikeId = 1234;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            BikesRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RemoveNotBlockedBikeShouldReturnInvalidState()
        {
            const int bikeId = 1234;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available,
            });
            BikesRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.RemoveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RemoveBlockedAndRentedBikeShouldThrowInvalidOperationException()
        {
            var bike = new Bike
            {
                Id = 123,
                User = new User
                {
                    Username = "Adam",
                },
                Status = BikeStatus.Blocked,
            };
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);
            BikesRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();
            
            var bikesService = GetBikesService();
            Action action = () => bikesService.RemoveBike(bike.Id.ToString());

            action.Should().Throw<InvalidOperationException>();
            BikesRepository.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
        }
    }
}