using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class GetAll
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public GetAll()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void GetAllShouldReturnAllBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bikes = new[]
            {
                new Bike
                {
                    Description = "one bike",
                    Status = BikeStatus.Available,
                    Station = station,
                },
                new Bike
                {
                    Description = "bike",
                    Status = BikeStatus.Blocked,
                    Station = station,
                },
                new Bike
                {
                    Description = "issued",
                    Status = BikeStatus.Blocked,
                    Station = station,
                },
                new Bike
                {
                    Status = BikeStatus.Rented,
                    Station = station,
                },
                new Bike
                {
                    Description = "issued",
                    Status = BikeStatus.Rented,
                    User = new User
                    {
                        Username = "piotr",
                    },
                },
            };
            _dbContext.Bikes.AddRange(bikes);
            _dbContext.SaveChanges();

            var result = _bikesRepository.GetAll();
            
            result.Should().BeEquivalentTo(bikes);
        }

        [Fact]
        public void GetAllShouldReturnEmptyIEnumerableWhenNoBikes()
        {
            var result = _bikesRepository.GetAll();
            
            result.Should().BeEmpty();
        }
    }
}
