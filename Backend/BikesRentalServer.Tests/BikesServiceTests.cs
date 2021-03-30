using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using Moq;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests
{
    public class BikesServiceTests
    {
        [Fact]
        public void AddBikeTest()
        {
            var context = MockedDbFactory.GetContext();
            
            Mock<BikesService> bikesService = new Mock<BikesService>(context);

            Assert.True(0 == bikesService.Object.GetAllBikes().Count());

            var request = new Dtos.Requests.AddBikeRequest { StationId = context.Stations.FirstOrDefault().Id.ToString(), BikeDescription = "", BikeStatus = Models.BikeStatus.Working };
            var response = bikesService.Object.AddBike(request);

            Assert.True(response.Object != null);
            Assert.True(1 == bikesService.Object.GetAllBikes().Count());
        }
    }
}
