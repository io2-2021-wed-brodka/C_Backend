using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class GenerateBearerToken
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;
        
        public GenerateBearerToken()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void GenerateBearerTokenShouldSucceedAndReturnToken()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.Admin,
                    State = UserState.Active,
                    Username = "test",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersService.GenerateBearerToken(user);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeAssignableTo(typeof(string));
        }
    }
}
