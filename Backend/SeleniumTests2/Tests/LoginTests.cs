using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SeleniumTests2
{
    public class LoginTests : TestsBase
    {
        public LoginTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void LogInWithWrongCredentialsShouldFail()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn("some-non-existing-user", "with-password");

            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public void LogInWithAdminCredentialsShouldSucceed()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn("admin", "admin");

            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            checkIfLoginSucceed();
        }

        [Fact]
        public async Task LogInWithCorrectCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await _api.SignUp(login, password);
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn(login, password);
            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            
            checkIfLoginSucceed();
        }

        [Fact]
        public async Task LogOutShouldSucceed()
        {
            var stationsPage = await LoginAsSomeUser();

            var loginPage = stationsPage.LogOut();
        }
    }
}
