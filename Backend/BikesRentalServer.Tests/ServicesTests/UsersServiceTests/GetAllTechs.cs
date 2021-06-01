using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class GetAllTechs : UsersServiceTestsBase
    {
        [Fact]
        public void GetAllTechsShouldReturnEmptyListWhenNoTechs()
        {
            UsersRepository.Setup(r => r.GetAllTechs()).Returns(new List<User>());

            var usersService = GetUsersService();
            var result = usersService.GetAllTechs();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllTechsReturnsAllExistingTechs()
        {
            var techs = new[]
            {
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "wojtek",
                },
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "kasia",
                },
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "kacper",
                },
            };
            UsersRepository.Setup(r => r.GetAllTechs()).Returns(techs);

            var usersService = GetUsersService();
            var result = usersService.GetAllTechs();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(techs);
        }

        [Fact]
        public void GetAllTechsDontIncludeAdminAndNormalUsers()
        {
            var users = new[]
            {
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "wojtek",
                },
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "kasia",
                },
                new User
                {
                    Role = UserRole.Admin,
                    Status = UserStatus.Active,
                    Username = "wiesiek",
                },
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "maniek",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "kacper",
                },
            };
            UsersRepository.Setup(r => r.GetAllTechs()).Returns(users);

            var usersService = GetUsersService();
            var result = usersService.GetAllTechs();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().HaveCount(3);
        }
    }
}
