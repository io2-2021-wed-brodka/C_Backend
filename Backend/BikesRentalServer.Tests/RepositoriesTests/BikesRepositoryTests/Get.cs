using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class Get
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public Get()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void GetShouldReturnRequestedBikeForValidId()
        {
            const int id = 1;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Id = id,
                    Status = BikeStatus.Reserved,
                    Description = "enum",
                    Station = new Station
                    {
                        Name = "New York",
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Get(id.ToString());
            
            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void GetShouldReturnNullIfNoBikeWithProvidedId()
        {
            const string id = "1";

            var result = _bikesRepository.Get(id);

            result.Should().BeNull();
        }

        [Fact]
        public void GetShouldReturnNullForInvalidId()
        {
            const string id = "invalid id";

            var result = _bikesRepository.Get(id);

            result.Should().BeNull();
        }
    }
}
