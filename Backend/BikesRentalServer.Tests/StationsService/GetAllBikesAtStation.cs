using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetAllBikesAtStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;
        
        public GetAllBikesAtStation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _stationsService = new Services.StationsService(_dbContext);
        }

        [Fact]
        public void GetAllBikesAtStationShouldReturnEmptyIEnumerateIfNoBikesAtStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Id = 3,
                    Name = "Dom Adama",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllBikesAtStationShouldReturnAllBikesAtStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Id = 3,
                    Name = "Dom Adama",
                })
                .Entity;
            var addedBikes = new []
            {
                new Bike
                {
                    Description = "one",
                    Station = station,
                    Status = BikeStatus.Working,
                },
                new Bike
                {
                    Description = "more",
                    Station = station,
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Description = "bike",
                    Station = station,
                    Status = BikeStatus.Working,
                },
            };
            _dbContext.Bikes.AddRange(addedBikes);
            _dbContext.SaveChanges();

            var result = _stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Count().Should().Be(addedBikes.Length);
            result.Object.Should().BeEquivalentTo(addedBikes);
        }

        [Fact]
        public void GetAllBikesAtNotExistingStationShouldReturnEntityNotFound()
        {
            var result = _stationsService.GetAllBikesAtStation("4");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
