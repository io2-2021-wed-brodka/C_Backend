using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class SetStatus
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public SetStatus()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void SetStatusShouldSetStatus()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                    Status = StationStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            _stationsRepository.SetStatus(station.Id, StationStatus.Active);

            station.Status.Should().Be(BikeStatus.Available);
        }

        [Fact]
        public void SetStatusShouldReturnChangedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                    Status = StationStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsRepository.SetStatus(station.Id, StationStatus.Active);

            result.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void SetStatusOfNotExistingBikeShouldReturnNull()
        {
            const int id = 7;
            
            var result = _stationsRepository.SetStatus(id, StationStatus.Blocked);

            result.Should().BeNull();
        }
    }
}
