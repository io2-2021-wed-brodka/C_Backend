﻿using BikesRentalServer.Authorization;
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
    public class GetRentedBikes
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.BikesService _bikesService;
        private readonly User _user;
        
        public GetRentedBikes()
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
        public void GetRentedBikesShouldReturnEmptyIEnumerableWhenNoRentals()
        {
            var result = _bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetRentedBikesShouldReturnAllRentedBikes()
        {
            var rentedBikes = new []
            {
                new Bike
                {
                    Description = "one",
                    User = _user,
                },
                new Bike
                {
                    Description = "two",
                    User = _user,
                },
                new Bike
                {
                    Description = "three",
                    User = _user,
                },
                new Bike
                {
                    Description = "four",
                    User = _user,
                },
                new Bike
                {
                    Description = "six",
                    User = _user,
                },
            };
            _dbContext.Bikes.AddRange(rentedBikes);
            _dbContext.SaveChanges();

            var result = _bikesService.GetRentedBikes();
            
            result.Status.Should().Be(Status.Success);
            result.Object.Count().Should().Be(rentedBikes.Length);
            result.Object.Should().BeEquivalentTo(rentedBikes);
        }
    }
}
