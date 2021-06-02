using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminUsersTests : TestsBase
    {
        public AdminUsersTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task AdminShouldBeAbleToBlockUsers()
        {
            var login = GetUniqueString();
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var user = await Api.SignUp(login, "234");

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminUsersPage = adminStationsPage.GoToUsers();
            adminUsersPage.BlockUser(login);
            adminUsersPage.IsUserBlocked(login).Should().BeTrue();
        }

        [Fact]
        public async Task AdminShouldBeAbleToUnblockUsers()
        {
            var login = GetUniqueString();
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var user = await Api.SignUp(login, "234");
            var userId = await Api.GetUserId(login, adminToken);
            await Api.BlockUser(userId, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminUsersPage = adminStationsPage.GoToUsers();
            adminUsersPage.UnblockUser(login);
            adminUsersPage.IsUserUnblocked(login).Should().BeTrue();
        }
    }
}
