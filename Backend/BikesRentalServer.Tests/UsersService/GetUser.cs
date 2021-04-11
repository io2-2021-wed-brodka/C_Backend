using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class GetUser
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;
        
        public GetUser()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void GetExistingUserShouldSucceed()
        {
            const string password = "Leć Adaś, leć!";
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.Tech,
                    State = UserState.Active,
                    Username = "adam_malysz",
                    PasswordHash = Toolbox.ComputeHash(password),
                })
                .Entity;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = _usersService.GetUserByUsernameAndPassword(user.Username, password);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetNotExistingUserShouldReturnEntityNotFound()
        {
            var result = _usersService.GetUserByUsernameAndPassword("user", "pass");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }
    }
}
