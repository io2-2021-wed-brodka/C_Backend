using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;
        
        public GetStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext);
        }

        [Fact]
        public void GetExistingStationShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Id = 3,
                    Status = BikeStationStatus.Blocked,
                    Name = "Dom Adama",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.GetStation(station.Id.ToString());
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(station, result.Object);
        }

        [Fact]
        public void GetNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.GetStation("4");
            
            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }
    }
}
