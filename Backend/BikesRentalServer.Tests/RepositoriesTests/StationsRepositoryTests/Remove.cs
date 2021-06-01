using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.StationsRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly StationsRepository _stationsRepository;
        
        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsRepository = new StationsRepository(_dbContext);
        }

        [Fact]
        public void RemoveShouldRemoveStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            _stationsRepository.Remove(station.Id);

            _dbContext.Stations.Count().Should().Be(0);
            _dbContext.Stations.SingleOrDefault(x => x.Id == station.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveShouldReturnRemovedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsRepository.Remove(station.Id);

            result.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void RemoveUsingIdOfNotExistingStationShouldRemoveNothingAndReturnNull()
        {
            const int id = 5;
            _dbContext.Stations.AddRange(new []
            {
                new Station
                {
                    Name = "station1",
                },
                new Station
                {
                    Name = "station2",
                },
                new Station
                {
                    Name = "station3",
                },
                new Station
                {
                    Name = "station5",
                },
            });
            _dbContext.SaveChanges();

            var initialStationCount = _dbContext.Stations.Count();
            var result = _stationsRepository.Remove(id);

            _dbContext.Stations.Count().Should().Be(initialStationCount);
            result.Should().BeNull();
        }
    }
}
