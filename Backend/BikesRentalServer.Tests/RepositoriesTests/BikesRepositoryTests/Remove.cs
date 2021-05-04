using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void RemoveUsingIdShouldRemoveBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "deleted",
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesRepository.Remove(bike.Id.ToString());

            _dbContext.Bikes.Count().Should().Be(0);
            _dbContext.Bikes.SingleOrDefault(x => x.Id == bike.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingEntityShouldRemoveBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "deleted",
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesRepository.Remove(bike);

            _dbContext.Bikes.Count().Should().Be(0);
            _dbContext.Bikes.SingleOrDefault(x => x.Id == bike.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdShouldReturnRemovedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "deleted",
                    Station = station,
                    Status = BikeStatus.Reserved,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Remove(bike.Id.ToString());

            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void RemoveUsingEntityShouldReturnRemovedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "deleted",
                    Station = station,
                    Status = BikeStatus.Reserved,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Remove(bike);

            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void RemoveUsingInvalidIdShouldRemoveNothingAndReturnNull()
        {
            const string id = "invalid id";
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "duck",
                    Station = station,
                },
                new Bike
                {
                    Description = "cat",
                    Station = station,
                },
                new Bike
                {
                    Description = "dog",
                    Station = station,
                },
            });
            _dbContext.SaveChanges();

            var initialBikeCount = _dbContext.Bikes.Count();
            var result = _bikesRepository.Remove(id);

            _dbContext.Bikes.Count().Should().Be(initialBikeCount);
            result.Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdOfNotExistingBikeShouldRemoveNothingAndReturnNull()
        {
            const string id = "5";
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "duck",
                    Station = station,
                    Id = 2,
                },
                new Bike
                {
                    Description = "cat",
                    Station = station,
                    Id = 3,
                },
                new Bike
                {
                    Description = "dog",
                    Station = station,
                    Id = 4,
                },
            });
            _dbContext.SaveChanges();

            var initialBikeCount = _dbContext.Bikes.Count();
            var result = _bikesRepository.Remove(id);

            _dbContext.Bikes.Count().Should().Be(initialBikeCount);
            result.Should().BeNull();
        }

        [Fact]
        public void RemoveNotExistingBikeShouldRemoveNothingAndReturnNull()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "duck",
                    Station = station,
                    Id = 2,
                },
                new Bike
                {
                    Description = "cat",
                    Station = station,
                    Id = 3,
                },
                new Bike
                {
                    Description = "dog",
                    Station = station,
                    Id = 4,
                },
            });
            _dbContext.SaveChanges();

            var initialBikeCount = _dbContext.Bikes.Count();
            var result = _bikesRepository.Remove(new Bike
            {
                Id = 6,
                Description = "other",
            });

            _dbContext.Bikes.Count().Should().Be(initialBikeCount);
            result.Should().BeNull();
        }
    }
}
