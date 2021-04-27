using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class BlockStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;

        public BlockStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext, new UserContext());
        }

        [Fact]
        public void BlockStationShouldReturnBlockedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.BlockStation(new BlockStationRequest
            {
                Id = station.Id.ToString(),
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void BlockStationShouldChangeStationStatusToBlocked()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.BlockStation(new BlockStationRequest
            {
                Id = station.Id.ToString(),
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Status.Should().Be(BikeStationStatus.Blocked);
        }

        [Fact]
        public void BlockNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.BlockStation(new BlockStationRequest
            {
                Id = "997",
            });

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void BlockAlreadyBlockedStationShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = BikeStationStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.BlockStation(new BlockStationRequest
            {
                Id = station.Id.ToString(),
            });

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
