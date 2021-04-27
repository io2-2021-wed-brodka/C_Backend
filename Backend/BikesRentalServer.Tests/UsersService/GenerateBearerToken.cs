using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class GenerateBearerToken : UsersServiceTestsBase
    {
        public GenerateBearerToken() : base()
        {
        }

        [Fact]
        public void GenerateBearerTokenShouldSucceedAndReturnToken()
        {
            var usersService = GetUsersService();

            var result = usersService.GenerateBearerToken(new User 
            { 
                Username = "pudzian" 
            });

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
        }
    }
}
