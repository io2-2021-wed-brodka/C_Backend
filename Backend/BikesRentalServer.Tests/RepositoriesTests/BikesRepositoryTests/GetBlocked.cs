using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class GetBlocked
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public GetBlocked()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void GetBlockedShouldReturnAllAndOnlyBlockedBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station!",
                })
                .Entity;
            var blockedBikes = new []
            {
                new Bike
                {
                    Description = "blocked",
                    Station = station,
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Description = "also blocked",
                    Station = station,
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Description = "still blocked",
                    Station = station,
                    Status = BikeStatus.Blocked,
                },
            };
            _dbContext.Bikes.AddRange(blockedBikes);
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "available",
                    Station = station,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Description = "reserved",
                    Station = station,
                    Status = BikeStatus.Reserved,
                },
                new Bike
                {
                    Description = "rented",
                    User = new User
                    {
                        Username = "adam",
                    },
                    Status = BikeStatus.Rented,
                },
            });
            _dbContext.SaveChanges();

            var result = _bikesRepository.GetBlocked();

            result.Should().BeEquivalentTo(blockedBikes);
        }

        [Fact]
        public void GetBlockedShouldReturnEmptyIEnumerableIfNoBlockedBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station!",
                })
                .Entity;
            _dbContext.Bikes.AddRange(new []
            {
                new Bike
                {
                    Description = "available",
                    Station = station,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Description = "reserved",
                    Station = station,
                    Status = BikeStatus.Reserved,
                },
                new Bike
                {
                    Description = "rented",
                    User = new User
                    {
                        Username = "adam",
                    },
                    Status = BikeStatus.Rented,
                },
            });
            _dbContext.SaveChanges();

            var result = _bikesRepository.GetBlocked();

            result.Should().BeEmpty();
        }
    }
}
