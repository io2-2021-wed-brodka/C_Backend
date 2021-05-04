using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class GetAll
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public GetAll()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void GetAllShouldReturnAllStations()
        {
            var stations = new []
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
                    Name = "station4",
                    Status = StationStatus.Blocked,
                },
            };
            _dbContext.Stations.AddRange(stations);
            _dbContext.SaveChanges();

            var result = _stationsRepository.GetAll();
            
            result.Should().BeEquivalentTo(stations);
        }

        [Fact]
        public void GetAllShouldReturnEmptyIEnumerableWhenNoBikes()
        {
            var result = _stationsRepository.GetAll();
            
            result.Should().BeEmpty();
        }
    }
}
