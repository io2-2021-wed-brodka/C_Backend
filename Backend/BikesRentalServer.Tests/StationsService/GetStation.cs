using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;
        
        public GetStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void GetExistingStationShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Id = 3,
                    Status = StationStatus.Blocked,
                    Name = "Dom Adama",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.GetStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void GetNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.GetStation("4");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
