using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class GetBlocked
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public GetBlocked()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void GetBlockedShouldReturnAllAndOnlyBlockedStations()
        {
            var blockedStations = new []
            {
                new Station
                {
                    Name = "station1",
                    Status = StationStatus.Blocked,
                },
                new Station
                {
                    Name = "station2",
                    Status = StationStatus.Blocked,
                },
                new Station
                {
                    Name = "station3",
                    Status = StationStatus.Blocked,
                },
            };
            _dbContext.Stations.AddRange(blockedStations);
            _dbContext.Stations.AddRange(new []
            {
                new Station
                {
                    Name = "active1",
                    Status = StationStatus.Active,
                },
                new Station
                {
                    Name = "active2",
                    Status = StationStatus.Active,
                },
            });
            _dbContext.SaveChanges();

            var result = _stationsRepository.GetBlocked();

            result.Should().BeEquivalentTo(blockedStations);
        }

        [Fact]
        public void GetBlockedShouldReturnEmptyIEnumerableIfNoBlockedStations()
        {
            _dbContext.Stations.AddRange(new []
            {
                new Station
                {
                    Name = "blocked",
                    Status = StationStatus.Active,
                },
            });
            _dbContext.SaveChanges();

            var result = _stationsRepository.GetBlocked();

            result.Should().BeEmpty();
        }
    }
}
