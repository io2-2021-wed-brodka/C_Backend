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
    public class GiveBikeBack
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        private readonly User _user;
        
        public GiveBikeBack()
        {
            _dbContext = MockedDbFactory.GetContext();
            _user = _dbContext.Users.Add(new User
                {
                    Username = "test_user",
                    State = UserState.Active,
                    Role = UserRole.Admin,
                    Reservations = new List<Reservation>(),
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var userContext = new UserContext();
            userContext.SetOnce(_user.Username, _user.Role);
            
            _bikesService = new Services.BikesService(_dbContext, userContext);
        }

        [Fact]
        public void GiveBikeBackShouldSucceed()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "Dworzec Centralny",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "whoosh",
                    User = _user,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.GiveBikeBack(bike.Id.ToString(), station.Id.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void GiveBikeBackShouldAssignBikeToStation()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "Dworzec Centralny",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "whoosh",
                    User = _user,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesService.GiveBikeBack(bike.Id.ToString(), station.Id.ToString());

            station.Bikes.Should().Contain(bike);
        }

        [Fact]
        public void GiveBikeBackToNotExistingStationShouldReturnEntityNotFound()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "whoosh",
                    User = _user,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.GiveBikeBack(bike.Id.ToString(), "1");
            
            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            bike.User.Should().Be(_user);
            bike.Station.Should().BeNull();
        }

        [Fact]
        public void GiveNotExistingBikeBackShouldReturnEntityNotFound()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "Dworzec Centralny",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesService.GiveBikeBack("1", station.Id.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
