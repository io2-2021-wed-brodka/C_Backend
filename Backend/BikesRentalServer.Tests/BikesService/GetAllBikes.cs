using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
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
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Empty(result.Object);
        }
        
        [Fact]
        public void GetAllBikesShouldReturnAllBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
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
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(addedBikes.Length, result.Object.Count());
            Assert.True(addedBikes.OrderBy(b => b.Id).SequenceEqual(result.Object.OrderBy(b => b.Id)));
        }
    }
}
