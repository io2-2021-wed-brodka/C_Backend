using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
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
            
            Assert.Equal(Status.Success, result.Status);
            Assert.Equal(bike, result.Object);
            Assert.Null(bike.User);
            Assert.Equal(station, bike.Station);
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

            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
            Assert.Equal(_user, bike.User);
            Assert.Null(bike.Station);
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

            Assert.Equal(Status.EntityNotFound, result.Status);
            Assert.Null(result.Object);
        }
    }
}
