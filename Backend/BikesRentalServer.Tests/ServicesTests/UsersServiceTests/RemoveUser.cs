using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class RemoveUser : UsersServiceTestsBase
    {
        [Fact]
        public void RemoveUserShouldRemoveUser()
        {
            const int id = 123;
            const UserRole role = UserRole.User;
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = id,
                    Role = role,
                    Status = UserStatus.Active,
                    Username = "Zenon",
                });
            UsersRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();

            var usersService = GetUsersService();
            var result = usersService.RemoveUser(id.ToString(), role);

            result.Status.Should().Be(Status.Success);
            UsersRepository.Verify();
        }

        [Fact]
        public void RemoveUserShouldReturnRemovedUser()
        {
            var user = new User
            {
                Id = 123,
                Role = UserRole.User,
                Status = UserStatus.Active,
                Username = "Zenon",
            };
            UsersRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(user);
            UsersRepository.Setup(r => r.Remove(It.IsAny<int>())).Returns(user);

            var usersService = GetUsersService();
            var result = usersService.RemoveUser(user.Id.ToString(), user.Role);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RemoveNotExistingUserShouldReturnEntityNotFound()
        {
            const string id = "123";
            UsersRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((User)null);
            UsersRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();

            var usersService = GetUsersService();
            var result = usersService.RemoveUser(id, UserRole.User);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            UsersRepository.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RemoveUserUsingWrongUserRoleShouldReturnEntityNotFound()
        {
            const int id = 123;
            const UserRole role = UserRole.Tech;
            const UserRole otherRole = UserRole.User;
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = id,
                    Role = otherRole,
                });
            UsersRepository.Setup(r => r.Remove(It.IsAny<int>())).Verifiable();

            var usersService = GetUsersService();
            var result = usersService.RemoveUser(id.ToString(), role);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            UsersRepository.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
        }
    }
}
