using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class SetStatus
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public SetStatus()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void SetStatusShouldSetStatus()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "sheesh",
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesRepository.SetStatus(bike.Id.ToString(), BikeStatus.Available);

            bike.Status.Should().Be(BikeStatus.Available);
        }

        [Fact]
        public void SetStatusShouldReturnChangedBike()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "sheesh",
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.SetStatus(bike.Id.ToString(), BikeStatus.Available);

            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void SetStatusOfNotExistingBikeShouldReturnNull()
        {
            const string id = "7";
            
            var result = _bikesRepository.SetStatus(id, BikeStatus.Blocked);

            result.Should().BeNull();
        }
    }
}
