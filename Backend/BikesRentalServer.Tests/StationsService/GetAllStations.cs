using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetAllStations
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;
        
        public GetAllStations()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext);
        }

        [Fact]
        public void GetAllStationsShouldReturnEmptyIEnumerateIfNoStations()
        {
            var result = _stationsService.GetAllStations();
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Empty(result.Object);
        }

        [Fact]
        public void GetAllStationsShouldReturnAllStations()
        {
            var addedStations = new []
            {
                new Station
                {
                    Name = "Test Station 1",
                    Status = BikeStationStatus.Working
                },
                new Station
                {
                    Id = 5,
                    Name = "Test Station 3",
                    Status = BikeStationStatus.Blocked
                },
                new Station
                {
                    Name = "Test Station"
                },
            };
            _dbContext.Stations.AddRange(addedStations);
            _dbContext.SaveChanges();

            var result = _stationsService.GetAllStations();
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(addedStations.Length, result.Object.Count());
            Assert.True(addedStations.OrderBy(s => s.Id).SequenceEqual(result.Object.OrderBy(s => s.Id)));
        }
    }
}
