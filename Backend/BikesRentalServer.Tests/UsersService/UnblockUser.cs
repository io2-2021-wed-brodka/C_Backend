using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class UnblockUser
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;

        public UnblockUser()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void UnblockUserThatDoesntExist()
        {
            var response = _usersService.UnblockUser("");

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().Be(null);
        }

        [Fact]
        public void UnblockUserAlreadyUnblocked()
        {
            int testId = 100;
            _dbContext.Users.Add(new User
            {
                Id = testId,
                State = UserState.Active,
            });
            _dbContext.SaveChanges();
            var response = _usersService.UnblockUser(testId.ToString());

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().Be(null);
        }

        [Fact]
        public void UnblockUserSimpleSuccess()
        {
            int testId = 100;
            _dbContext.Users.Add(new User
            {
                Id = testId,
                State = UserState.Banned,
            });
            _dbContext.SaveChanges();
            var response = _usersService.UnblockUser(testId.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Id.Should().Be(testId);
        }
    }
}
