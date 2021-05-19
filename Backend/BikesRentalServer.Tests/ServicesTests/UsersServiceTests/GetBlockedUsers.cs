using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class GetBlockedUsers : UsersServiceTestsBase
    {
        [Fact]
        public void GetBlockedUsersShouldReturnEmptyListWhenNoBlockedUsers()
        {
            UsersRepository.Setup(r => r.GetBlockedUsers()).Returns(new List<User>());

            var usersService = GetUsersService();
            var result = usersService.GetBlockedUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlockedUsersShouldReturnAllBlockedUsers()
        {
            var users = new[]
            {
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Blocked,
                    Username = "wojtek",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Blocked,
                    Username = "kasia",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Blocked,
                    Username = "kacper",
                },
            };
            UsersRepository.Setup(r => r.GetBlockedUsers()).Returns(users);

            var usersService = GetUsersService();
            var result = usersService.GetBlockedUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(users);
        }
    }
}
