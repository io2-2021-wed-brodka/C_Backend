using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class Add
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public Add()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void AddShouldAddStationToDatabase()
        {
            _stationsRepository.Add(new Station());

            _dbContext.Stations.Count().Should().Be(1);
        }

        [Fact]
        public void AddShouldReturnAddedStation()
        {
            const string name = "best station ever";
            const StationStatus status = StationStatus.Active;

            var result = _stationsRepository.Add(new Station
            {
                Name = name,
                Status = status,
            });

            result.Should().NotBeNull();
            result.Id.Should().BePositive();
            result.Name.Should().Be(name);
            result.Status.Should().Be(status);
        }
    }
}
