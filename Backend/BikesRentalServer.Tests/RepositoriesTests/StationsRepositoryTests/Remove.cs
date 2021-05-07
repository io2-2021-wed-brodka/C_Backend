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
        public void RemoveUsingIdShouldRemoveStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            _stationsRepository.Remove(station.Id.ToString());

            _dbContext.Stations.Count().Should().Be(0);
            _dbContext.Stations.SingleOrDefault(x => x.Id == station.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingEntityShouldRemoveStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            _stationsRepository.Remove(station);

            _dbContext.Stations.Count().Should().Be(0);
            _dbContext.Stations.SingleOrDefault(x => x.Id == station.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdShouldReturnRemovedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsRepository.Remove(station.Id.ToString());

            result.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void RemoveUsingEntityShouldReturnRemovedStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsRepository.Remove(station);

            result.Should().BeEquivalentTo(station);
        }

        [Fact]
        public void RemoveUsingInvalidIdShouldRemoveNothingAndReturnNull()
        {
            const string id = "invalid id";
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

        [Fact]
        public void RemoveUsingIdOfNotExistingStationShouldRemoveNothingAndReturnNull()
        {
            const string id = "5";
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

        [Fact]
        public void RemoveNotExistingStationShouldRemoveNothingAndReturnNull()
        {
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
            var result = _stationsRepository.Remove(new Station
            {
                Id = 6,
                Name = "other",
            });

            _dbContext.Stations.Count().Should().Be(initialStationCount);
            result.Should().BeNull();
        }
    }
}
