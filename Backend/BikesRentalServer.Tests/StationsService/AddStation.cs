using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class AddStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public AddStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void AddStationShouldIncrementStationCount()
        {
            var initialStationCount = _dbContext.Stations.Count();
            var result = _stationsService.AddStation(new AddStationRequest
            {
                Name = "Al. Jerozolimskie",
            });

            result.Status.Should().Be(Status.Success);
            _dbContext.Stations.Count().Should().Be(initialStationCount + 1);
        }

        [Fact]
        public void AddStationShouldReturnCreatedStation()
        {
            var result = _stationsService.AddStation(new AddStationRequest
            {
                Name = "Al. Jerozolimskie",
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Name.Should().Be("Al. Jerozolimskie");
        }     
    }
}
