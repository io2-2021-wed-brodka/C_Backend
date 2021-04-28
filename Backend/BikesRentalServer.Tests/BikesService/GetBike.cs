using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetBike : BikesServiceTestsBase
    {
        [Fact]
        public void GetExistingBikeShouldSucceed()
        {
            const int bikeId = 123;
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available
            };
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);

            var bikesService = GetBikesService();
            var result = bikesService.GetBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void GetNotExistingBikeShouldReturnEntityNotFound()
        {
            const string bikeId = "123";
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);

            var bikesService = GetBikesService();
            var result = bikesService.GetBike(bikeId);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
