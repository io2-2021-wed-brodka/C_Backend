using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class GetActive
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public GetActive()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void GetActiveShouldReturnAllAndOnlyActiveStations()
        {
            var activeStations = new []
            {
                new Station
                {
                    Name = "station1",
                    Status = StationStatus.Active,
                },
                new Station
                {
                    Name = "station2",
                    Status = StationStatus.Active,
                },
                new Station
                {
                    Name = "station3",
                    Status = StationStatus.Active,
                },
            };
            _dbContext.Stations.AddRange(activeStations);
            _dbContext.Stations.AddRange(new []
            {
                new Station
                {
                    Name = "blocked1",
                    Status = StationStatus.Blocked,
                },
                new Station
                {
                    Name = "blocked2",
                    Status = StationStatus.Blocked,
                },
            });
            _dbContext.SaveChanges();

            var result = _stationsRepository.GetActive();

            result.Should().BeEquivalentTo(activeStations);
        }

        [Fact]
        public void GetActiveShouldReturnEmptyIEnumerableIfNoActiveStations()
        {
            _dbContext.Stations.AddRange(new []
            {
                new Station
                {
                    Name = "blocked",
                    Status = StationStatus.Blocked,
                },
            });
            _dbContext.SaveChanges();

            var result = _stationsRepository.GetActive();

            result.Should().BeEmpty();
        }
    }
}
