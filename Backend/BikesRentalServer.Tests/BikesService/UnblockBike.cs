using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class UnblockBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;

        public UnblockBike()
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

            _bikesService = new Services.BikesService(_dbContext, new UserContext());
        }

        [Fact]
        public void UnblockBikeShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "stacja",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.UnblockBike(bike.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }
        [Fact]
        public void UnblockBikeShouldChangeBikeStatusForWorking()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "stacja",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.UnblockBike(bike.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Status.Should().BeEquivalentTo(BikeStatus.Working);
        }
        [Fact]
        public void UnblockAlreadyWorkingBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Status = StationStatus.Working,
                    Name = "stacja",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.UnblockBike(bike.Id.ToString());
            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }
        [Fact]
        public void UnblockNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.UnblockBike("27");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
