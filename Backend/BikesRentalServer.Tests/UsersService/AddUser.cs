using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class AddUser
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;
        
        public AddUser()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void AddUserShouldSucceedAndReturnCreatedUser()
        {
            const string username = "test_user";
            const string password = "theBestTESTp4ssWd";
            var response = _usersService.AddUser(username, password);

            response.Status.Should().Be(Status.Success);
            response.Object.Username.Should().Be(username);
            response.Object.PasswordHash.Should().Be(Toolbox.ComputeHash(password));
        }

        [Fact]
        public void AddUserWithTakenUsernameShouldReturnInvalidState()
        {
            const string password = "theBestTESTp4ssWd";
            _dbContext.Users.Add(new User
            {
                Username = "test2",
            });
            _dbContext.SaveChanges();
            
            var response = _usersService.AddUser("test2", password);

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
        }
    }
}
