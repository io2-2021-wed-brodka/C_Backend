using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class Get
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public Get()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void GetShouldReturnRequestedStationForValidId()
        {
            const int id = 1;
            var station = _dbContext.Stations.Add(new Station
                {
                    Id = id,
                    Name = "station"
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsRepository.Get(id.ToString());
            
            result.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void GetShouldReturnNullIfNoStationsWithProvidedId()
        {
            const string id = "1";

            var result = _stationsRepository.Get(id);

            result.Should().BeNull();
        }

        [Fact]
        public void GetShouldReturnNullForInvalidId()
        {
            const string id = "invalid id";

            var result = _stationsRepository.Get(id);

            result.Should().BeNull();
        }
    }
}
