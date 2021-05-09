using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class BlockUser : UsersServiceTestsBase
    {
        [Fact]
        public void BlockNotExistingUserShouldReturnEntityNotFound()
        {
            const string userId = "2";
            UsersRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((User)null);

            var usersService = GetUsersService();
            var response = usersService.BlockUser(userId);

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockAlreadyBlockedUserShouldReturnInvalidState()
        {
            const int userId = 2;
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = userId,
                    Status = UserStatus.Blocked,
                });

            var usersService = GetUsersService();
            var response = usersService.BlockUser(userId.ToString());

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockUserShouldSetUserStateToBanned()
        {
            const int userId = 2;
            UsersRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<UserStatus>(s => s == UserStatus.Blocked)))
                .Returns(new User
                {
                    Id = userId,
                    Status = UserStatus.Blocked,
                    Reservations = new List<Reservation>(),
                })
                .Verifiable();
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = 2,
                    Status = UserStatus.Active,
                    Reservations = new List<Reservation>(),
                });

            var usersService = GetUsersService();
            var response = usersService.BlockUser(userId.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Should().NotBeNull();
            response.Object.Status.Should().Be(UserStatus.Blocked);
            UsersRepository.Verify();
        }

        [Fact]
        public void BlockUserShouldRemoveAllReservationsOfBlockedUser()
        {
            const int userId = 2;
            var reservations = new List<Reservation>
            {
                new Reservation(),
                new Reservation(),
                new Reservation(),
                new Reservation(),
            };
            UsersRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<UserStatus>(s => s == UserStatus.Blocked)))
                .Returns(new User
                {
                    Id = userId,
                    Status = UserStatus.Blocked,
                    Reservations = new List<Reservation>(),
                });
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = userId,
                    Status = UserStatus.Active,
                    Reservations = reservations,
                });
            ReservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Returns(new Reservation()).Verifiable();

            var usersService = GetUsersService();
            var response = usersService.BlockUser(userId.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Should().NotBeNull();
            response.Object.Reservations.Should().BeEmpty();
            ReservationsRepository.Verify(r => r.Remove(It.IsAny<Reservation>()), Times.Exactly(reservations.Count));
        }
    }
}
