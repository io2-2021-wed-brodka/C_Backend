using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class GetUser : UsersServiceTestsBase
    {
        [Fact]
        public void GetExistingUserShouldSucceed()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";

            UsersRepository.Setup(r => r.GetByUsernameAndPassword(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .Returns(new User
                {
                    Username = username,
                    Id = 3,
                })
                .Verifiable();

            var usersService = GetUsersService();
            var response = usersService.GetUserByUsernameAndPassword(username, password);

            response.Status.Should().Be(Status.Success);
            response.Object.Username.Should().Be(username);
            UsersRepository.Verify();
        }

        [Fact]
        public void GetNotExistingUserShouldReturnEntityNotFound()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";

            UsersRepository.Setup(r => r.GetByUsernameAndPassword(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .Returns((User)null)
                .Verifiable();

            var usersService = GetUsersService();
            var response = usersService.GetUserByUsernameAndPassword(username, password);

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
            UsersRepository.Verify();
        }
    }
}
