using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class BlockUser
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;

        public BlockUser()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void BlockNotExistingUserShouldReturnEntityNotFound()
        {
            var response = _usersService.BlockUser("");

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockAlreadyBlockedUserShouldReturnInvalidState()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Id = 100,
                    Status = UserStatus.Banned,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var response = _usersService.BlockUser(user.Id.ToString());

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockUserShouldSetUserStateToBanned()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Id = 100,
                    Status = UserStatus.Active,
                })
                .Entity;
            _dbContext.SaveChanges();
            var response = _usersService.BlockUser(user.Id.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Status.Should().Be(UserStatus.Banned);
            response.Object.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void BlockUserWithReservationsShouldDeleteTheirReservationsAndNotDeleteOtherReservations()
        {
            var bike1 = new Bike();
            var bike2 = new Bike();
            var bike3 = new Bike();
            var notBlockedUser = _dbContext.Users.Add(new User
                {
                    Id = 2,
                    Status = UserStatus.Active,
                })
                .Entity;
            var blockedUser = _dbContext.Users.Add(new User
                {
                    Id = 100,
                    Status = UserStatus.Active,
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Bike = bike1,
                        },
                        new Reservation
                        {
                            Bike = bike2,
                        },
                        new Reservation
                        {
                            Bike = bike3,
                        },
                    },
                })
                .Entity;
            _dbContext.Reservations.Add(new Reservation
            {
                Bike = bike3,
                User = notBlockedUser,
            });
            _dbContext.SaveChanges();

            var initialReservationCount = _dbContext.Reservations.Count();
            var initialBlockedUserReservationCount = blockedUser.Reservations.Count;
            var response = _usersService.BlockUser(blockedUser.Id.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Should().BeEquivalentTo(blockedUser);
            _dbContext.Reservations.Should().HaveCount(initialReservationCount - initialBlockedUserReservationCount);
        }
    }
}
