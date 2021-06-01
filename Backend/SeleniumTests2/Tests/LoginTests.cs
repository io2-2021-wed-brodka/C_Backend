using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SeleniumTests2.Tests
{
    public class LoginTests : TestsBase
    {
        public LoginTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void LogInWithWrongCredentialsShouldFail()
        {
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn("some-non-existing-user", "with-password");

            Action checkIfLoginSucceed = () => new StationsPage(Driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public void LogInWithAdminCredentialsShouldSucceed()
        {
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn("admin", "admin");

            Action checkIfLoginSucceed = () => new StationsPage(Driver);
            checkIfLoginSucceed();
        }

        [Fact]
        public async Task LogInWithCorrectCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await Api.SignUp(login, password);
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn(login, password);
            Action checkIfLoginSucceed = () => new StationsPage(Driver);
            
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
