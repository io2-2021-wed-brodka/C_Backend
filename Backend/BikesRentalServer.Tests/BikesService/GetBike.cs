using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
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
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike.Id, result.Object.Id);
            Assert.Equal(bike.Description, result.Object.Description);
            Assert.Equal(bike.Station, result.Object.Station);
            Assert.Equal(bike.Status, result.Object.Status);
            Assert.Equal(bike.Station, result.Object.Station);
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
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }
    }
}
