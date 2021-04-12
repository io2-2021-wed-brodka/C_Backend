using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;

        public GetBike()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesService = new Services.BikesService(_dbContext, new UserContext());
        }

        [Fact]
        public void GetExistingBikeShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.Bikes.Add(new Bike
            {
                Station = station,
            });
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.GetBike(bike.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void GetNotExistingBikeShouldReturnEntityNotFound()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;
            _dbContext.Bikes.Add(new Bike
            {
                Id = 1,
                Station = station,
            });
            _dbContext.SaveChanges();

            var result = _bikesService.GetBike("4");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
