using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetBlockedBikes
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        
        public GetBlockedBikes()
        {
            _dbContext = MockedDbFactory.GetContext();
            var user = _dbContext.Users.Add(new User
                {
                    Username = "test_admin",
                    State = UserState.Active,
                    Role = UserRole.Admin,
                    Reservations = new List<Reservation>(),
                })
                .Entity;
            _dbContext.SaveChanges();

            var userContext = new UserContext();
            userContext.SetOnce(user.Username, user.Role);

            _bikesService = new Services.BikesService(_dbContext, userContext);
        }

        [Fact]
        public void GetBlocedBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            var result = _bikesService.GetBlockedBikes();
            
            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlockedBikesShouldReturnAllBlockedBikes()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = BikeStationStatus.Working,
                    Name = "Al. Jerozolimskie",
                })
                .Entity;

            var addedBlockedBikes = new[]
            {
                new Bike
                {
                    Id = 1,
                    Station = station,
                    Description = "first one!",
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Id = 2,
                    Station = station,
                    Description = "Another ONE",
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Id = 4,
                    Station = station,
                    Description = "Skipped one",
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Id = 7,
                    Station = station,
                    Description = string.Empty,
                    Status = BikeStatus.Blocked,
                },
            };

            var addedWorkingdBikes = new[]
            {
                new Bike
                {
                    Id = 11,
                    Station = station,
                    Description = "first working!",
                },
                new Bike
                {
                    Id = 12,
                    Station = station,
                    Description = "Another ONE",
                },
                new Bike
                {
                    Id = 14,
                    Station = station,
                    Description = "Skipped one",
                },
                new Bike
                {
                    Id = 17,
                    Station = station,
                    Description = string.Empty,
                },
            };

            _dbContext.Bikes.AddRange(addedBlockedBikes);
            _dbContext.Bikes.AddRange(addedWorkingdBikes);
            _dbContext.SaveChanges();

            var result = _bikesService.GetBlockedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(addedBlockedBikes);
        }
    }
}
