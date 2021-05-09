using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class UnblockUser : UsersServiceTestsBase
    {
        [Fact]
        public void UnblockUserThatDoesntExist()
        {
            const string userId = "2";
            UsersRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((User)null);

            var usersService = GetUsersService();
            var response = usersService.UnblockUser(userId);

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void UnblockUserAlreadyUnblocked()
        {
            const string userId = "2";
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = 34,
                    Status = UserStatus.Active,
                });

            var usersService = GetUsersService();
            var response = usersService.UnblockUser(userId);

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void UnblockUserSimpleSuccess()
        {
            const string userId = "2";
            UsersRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<UserStatus>(s => s == UserStatus.Active)))
                .Returns(new User
                {
                    Id = 2,
                    Status = UserStatus.Active,
                })
                .Verifiable();
            UsersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = 2,
                    Status = UserStatus.Blocked,
                });

            var usersService = GetUsersService();
            var response = usersService.UnblockUser(userId);

            response.Status.Should().Be(Status.Success);
            response.Object.Should().NotBeNull();
            response.Object.Status.Should().Be(UserStatus.Active);
            UsersRepository.Verify();
        }
    }
}
