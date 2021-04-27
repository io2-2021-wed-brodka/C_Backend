using BikesRentalServer.DataAccess;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class UnblockStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public UnblockStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void UnblockStationShouldReturnUnblockedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void UnblockStationShouldChangeStationStatusToWorking()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Status.Should().Be(StationStatus.Working);
        }

        [Fact]
        public void UnblockNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.UnblockStation("997");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void UnblockNotBlockedStationShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.UnblockStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
