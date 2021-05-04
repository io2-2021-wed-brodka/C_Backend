using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class AddUser : UsersServiceTestsBase
    {
        [Fact]
        public void AddUserShouldSucceedAndReturnCreatedUser()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";
            var expectedPassword = Toolbox.ComputeHash(password);
            UsersRepository.Setup(r => r.Add(It.IsAny<User>()))
                .Returns(new User
                {
                    Username = username,
                    Id = 3,
                    PasswordHash = expectedPassword,
                })
                .Verifiable();

            var usersService = GetUsersService();
            var response = usersService.AddUser(username, password);

            response.Status.Should().Be(Status.Success);
            response.Object.Username.Should().Be(username);
            response.Object.PasswordHash.Should().Be(expectedPassword);
            UsersRepository.Verify();
        }

        [Fact]
        public void AddUserWithTakenUsernameShouldReturnInvalidState()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";
            UsersRepository.Setup(r => r.Add(It.IsAny<User>())).Verifiable();
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(new User());

            var usersService = GetUsersService();
            var response = usersService.AddUser(username, password);

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
            UsersRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }
    }
}
