using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class AddBike : BikesServiceTestsBase
    {
        public AddBike() : base()
        {
        }

        [Fact]
        public void AddBikeShouldAddBike()
        {
            var stationId = 123;
            var addBikeRequest = new AddBikeRequest
            {
                StationId = stationId.ToString()
            };
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId
            });
            _bikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();
            
            var bikesService = GetBikesService();

            var result = bikesService.AddBike(addBikeRequest);

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
        }

        [Fact]
        public void AddBikeShouldReturnCreatedBike()
        {
            var stationId = 123;
            var addBikeRequest = new AddBikeRequest
            {
                StationId = stationId.ToString()
            };
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId
            });
            _bikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();

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
            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Station)null);
            _bikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();

            var result = bikesService.AddBike(addBikeRequest);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
