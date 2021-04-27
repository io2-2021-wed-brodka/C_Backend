using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class BlockBike : BikesServiceTestsBase
    {
        public BlockBike() : base()
        {
        }

        [Fact]
        public void BlockBikeShouldSucceed()
        {
            var bikeId = 123;
            var blockBikeRequest = new BlockBikeRequest
            {
                Id = bikeId.ToString()
            };
            BikesRepository.Setup(r => 
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working
                });

            var bikesService = GetBikesService();

            var result = bikesService.BlockBike(blockBikeRequest);

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Blocked);
        }

        [Fact]
        public void BlockBikeShouldChangeBikeStatusForBlocked()
        {
            var bikeId = 123;
            var blockBikeRequest = new BlockBikeRequest
            {
                Id = bikeId.ToString()
            };
            BikesRepository.Setup(r =>
                r.SetStatus(It.Is<string>(id => id == bikeId.ToString()), It.Is<BikeStatus>(b => b == BikeStatus.Blocked))
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working
                });

            var bikesService = GetBikesService();

            var result = bikesService.BlockBike(blockBikeRequest);

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void BlockNotExistingBikeShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var blockBikeRequest = new BlockBikeRequest
            {
                Id = bikeId.ToString()
            };
            BikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Bike)null);

            var bikesService = GetBikesService();

            var result = bikesService.BlockBike(blockBikeRequest);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact] 
        public void BlockAlreadyBlockedBikeShouldReturnInvalidState()
        {
            var bikeId = 123;
            var blockBikeRequest = new BlockBikeRequest
            {
                Id = bikeId.ToString()
            };
            BikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked
                });

            var bikesService = GetBikesService();

            var result = bikesService.BlockBike(blockBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact] 
        public void BlockRentedBikeShouldReturnInvalidState()
        {
            var bikeId = 123;
            var blockBikeRequest = new BlockBikeRequest
            {
                Id = bikeId.ToString()
            };
            BikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked // Should be rented, but BikeStatus.Rented is missing ;(
                });

            var bikesService = GetBikesService();

            var result = bikesService.BlockBike(blockBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }
    }
}
