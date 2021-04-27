using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class BlockBike
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;

        public BlockBike()
        {
            _dbContext = MockedDbFactory.GetContext();
            var user = _dbContext.Users.Add(new User
                {
                    Username = "test_admin",
                    Status = UserStatus.Active,
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
        public void BlockBikeShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Description = "some text",
                    Status = BikeStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.BlockBike(new BlockBikeRequest
            {
                Id = bike.Id.ToString(),
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void BlockBikeShouldChangeBikeStatusForBlocked()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
                })
              .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Description = "some text",
                    Status = BikeStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.BlockBike(new BlockBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            result.Status.Should().Be(Status.Success);
            result.Object.Status.Should().Be(BikeStatus.Blocked);
        }

        [Fact]
        public void BlockNotExistingBikeShouldReturnEntityNotFound()
        {
            var result = _bikesService.BlockBike(new BlockBikeRequest
            {
                Id = "27",
            });
            
            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact] 
        public void BlockAlreadyBlockedBikeShouldReturnInvalidState()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "DS Ustronie",
                    Status = StationStatus.Working,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Description = "some text",
                    Status = BikeStatus.Blocked,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.BlockBike(new BlockBikeRequest
            {
                Id = bike.Id.ToString()
            });
            
            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }

        [Fact] 
        public void BlockRentedBikeShouldReturnInvalidState()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "user_with_bike",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    User = user,
                    Description = "some text",
                    Status = BikeStatus.Working,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _bikesService.BlockBike(new BlockBikeRequest
            {
                Id = bike.Id.ToString(),
            });
            
            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
        }

    }
}
