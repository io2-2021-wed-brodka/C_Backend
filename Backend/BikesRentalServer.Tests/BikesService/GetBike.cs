using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class GetBike : BikesServiceTestsBase
    {
        public GetBike() : base()
        {
        }

        [Fact]
        public void GetExistingBikeShouldSucceed()
        {
            var bikeId = "123";

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Bike)null);

            var bikesService = GetBikesService();

            var result = bikesService.GetBike(bikeId);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void GetNotExistingBikeShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Working
            };

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(bike);

            var bikesService = GetBikesService();

            var result = bikesService.GetBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }
    }
}
