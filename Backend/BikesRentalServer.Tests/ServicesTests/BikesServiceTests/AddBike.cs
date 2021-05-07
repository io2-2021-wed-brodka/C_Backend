using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class AddBike : BikesServiceTestsBase
    {
        [Fact]
        public void AddBikeShouldAddBike()
        {
            const int stationId = 123;
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId,
            });
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();
            
            var bikesService = GetBikesService();
            var result = bikesService.AddBike(stationId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void AddBikeShouldReturnCreatedBike()
        {
            const int stationId = 123;
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Station
            {
                Id = stationId,
            });
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>()))
                .Returns(new Bike
                {
                    Station = new Station
                    {
                        Id = stationId,
                    },
                })
                .Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.AddBike(stationId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Station.Should().NotBeNull();
            result.Object.Station.Id.Should().Be(stationId);
        }

        [Fact]
        public void AddBikeToNotExistingStationShouldReturnEntityNotFound()
        {
            const int stationId = 123;
            StationsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Station)null);
            BikesRepository.Setup(r => r.Add(It.IsAny<Bike>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.AddBike(stationId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
