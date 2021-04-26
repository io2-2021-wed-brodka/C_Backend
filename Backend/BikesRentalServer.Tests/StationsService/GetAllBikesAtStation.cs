using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.StationsService
{
    public class GetAllBikesAtStation
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.StationsService _stationsService;
        private readonly User _user;

        public GetAllBikesAtStation()
        {
            _dbContext = MockedDbFactory.GetContext();

            _user = _dbContext.Users.Add(new User
            {
                Username = "test_user",
                State = UserState.Active,
                Role = UserRole.User,
                Reservations = new List<Reservation>(),
            })
            .Entity;
            _dbContext.SaveChanges();

            var userContext = new UserContext();
            userContext.SetOnce(_user.Username, _user.Role);

            _stationsService = new Services.StationsService(_dbContext, userContext);
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

        [Fact]
        public void GetAllBikesAtBlockedStationForUserShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
            {
                Id = 3,
                Name = "Buda Reksia",
                Status = BikeStationStatus.Blocked,
            })
            .Entity;
            _dbContext.SaveChanges();

            var result = _stationsService.GetAllBikesAtStation(station.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
    }
}
