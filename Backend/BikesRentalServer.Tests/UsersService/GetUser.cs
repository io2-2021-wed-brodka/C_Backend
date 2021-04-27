using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class GetUser : UsersServiceTestsBase
    {
        public GetUser() : base()
        {
        }

        [Fact]
        public void GetExistingUserShouldSucceed()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";

            _usersRepository.Setup(r => r.GetByUsernameAndPassword(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .Returns(new User
                {
                    Username = username,
                    Id = 3
                }).Verifiable();

            var usersService = GetUsersService();

            var response = usersService.GetUserByUsernameAndPassword(username, password);

            response.Status.Should().Be(Status.Success);
            response.Object.Username.Should().Be(username);
            response.Object.PasswordHash.Should().Be(Toolbox.ComputeHash(password));
            _usersRepository.Verify();
        }

        [Fact]
        public void GetNotExistingUserShouldReturnEntityNotFound()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";

            _usersRepository.Setup(r => r.GetByUsernameAndPassword(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .Returns((User)null).Verifiable();

            var usersService = GetUsersService();

            var response = usersService.GetUserByUsernameAndPassword(username, password);

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
            _usersRepository.Verify();
        }
    }
}
