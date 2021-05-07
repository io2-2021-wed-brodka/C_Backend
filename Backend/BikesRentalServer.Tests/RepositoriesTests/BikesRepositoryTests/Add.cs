using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class Add
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public Add()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void AddShouldAddBikeToDatabase()
        {
            _bikesRepository.Add(new Bike());

            _dbContext.Bikes.Count().Should().Be(1);
        }

        [Fact]
        public void AddShouldReturnAddedBike()
        {
            const string description = "somebody once rented this bike";
            const BikeStatus status = BikeStatus.Available;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "Test",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Add(new Bike
            {
                Description = description,
                Station = station,
                Status = status,
            });

            result.Should().NotBeNull();
            result.Id.Should().BePositive();
            result.Description.Should().Be(description);
            result.Station.Should().BeEquivalentTo(station);
            result.User.Should().BeNull();
            result.Status.Should().Be(status);
        }
    }
}
