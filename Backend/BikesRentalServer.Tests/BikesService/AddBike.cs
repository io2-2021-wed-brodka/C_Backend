using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class AddBike : BikesServiceTestsBase
    {
        [Fact]
        public void AddBikeShouldAddBike()
        {
            var stationId = 123;
            var addBikeRequest = new AddBikeRequest
            {
                StationId = stationId.ToString()
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId
            });
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();
            
            var bikesService = GetBikesService();

            var result = bikesService.AddBike(addBikeRequest);

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void AddBikeShouldReturnCreatedBike()
        {
            var stationId = 123;
            var addBikeRequest = new AddBikeRequest
            {
                StationId = stationId.ToString()
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId
            });
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.AddBike(addBikeRequest);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Station.Should().NotBeNull();
        }

        [Fact]
        public void AddBikeToNotExistingStationShouldReturnEntityNotFound()
        {
            var stationId = 123;
            var addBikeRequest = new AddBikeRequest
            {
                StationId = stationId.ToString()
            };
            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Station)null);
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.AddBike(addBikeRequest);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
