using BikesRentalServer.DataAccess;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetAllBikes
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        
        public GetAllBikes()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesService = new Services.BikesService(_dbContext, new UserContext());
        }

        [Fact]
        public void GetAllBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            var result = _bikesService.GetAllBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }
        
        [Fact]
        public void GetAllBikesShouldReturnAllBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;

            var addedBikes = new []
            {
                new Bike
                {
                    Id = 1,
                    Station = station,
                    Description = "first one!",
                },
                new Bike
                {
                    Id = 2,
                    Station = station,
                    Description = "Another ONE",
                },
                new Bike
                {
                    Id = 4,
                    Station = station,
                    Description = "Skipped one",
                },
                new Bike
                {
                    Id = 7,
                    Station = station,
                    Description = string.Empty,
                },
            };
            _dbContext.Bikes.AddRange(addedBikes);
            _dbContext.SaveChanges();

            var result = _bikesService.GetAllBikes();
            
            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(addedBikes);
        }
    }
}
