using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class AddStation : StationsServiceTestsBase
    {
        public AddStation() : base()
        {
        }

        [Fact]
        public void AddStationShouldAddStation()
        {
            var station = new Station
            {
                Name = "Trailer Park"
            };
            var addStationRequest = new AddStationRequest
            {
                Name = station.Name
            };
            _stationsRepository.Setup(r => r.Add(It.IsAny<Station>())).Returns(station).Verifiable();
            var stationsService = GetStationsService();

            var result = stationsService.AddStation(addStationRequest);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Name.Should().Be(station.Name);
            _stationsRepository.Verify();
        }
    }
}
