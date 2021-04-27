using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class AddUser : UsersServiceTestsBase
    {
        public AddUser() : base()
        {
        }

        [Fact]
        public void AddUserShouldSucceedAndReturnCreatedUser()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";
            _usersRepository.Setup(r => r.Add(It.IsAny<User>())).Returns(new User
            {
                Username = username,
                Id = 3
            }).Verifiable();

            var usersService = GetUsersService();

            var response = usersService.AddUser(username, password);

            response.Status.Should().Be(Status.Success);
            response.Object.Username.Should().Be(username);
            response.Object.PasswordHash.Should().Be(Toolbox.ComputeHash(password));
            _usersRepository.Verify();
        }

        [Fact]
        public void AddUserWithTakenUsernameShouldReturnInvalidState()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";
            _usersRepository.Setup(r => r.Add(It.IsAny<User>())).Verifiable();
            _usersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(new User());

            var usersService = GetUsersService();

            var response = usersService.AddUser(username, password);

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
            _usersRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }
    }
}
