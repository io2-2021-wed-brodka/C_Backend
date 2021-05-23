using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SeleniumTests2
{
    public class LoginTests : TestsBase
    {
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
        public void AdminLogInWithCorrectCredentialsShouldSucceed()
        {
            _driver.OpenAdminTab();
            var loginPage = new LoginPage(_driver, true);

            loginPage.LogIn("admin", "admin");
            Action checkIfLoginSucceed = () => new AdminStationsPage(_driver);
            
            checkIfLoginSucceed();
        }

        [Fact]
        public void AdminLogInWithWrongCredentialsShouldFail()
        {
            _driver.OpenAdminTab();
            var loginPage = new LoginPage(_driver, true);

            loginPage.LogIn("some-non-existing-user", "with-password");

            Action checkIfLoginSucceed = () => new AdminStationsPage(_driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public async Task AdminLogInWithUserCredentialsShouldFail()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await _api.SignUp(login, password);

            _driver.OpenAdminTab();
            var loginPage = new LoginPage(_driver, true);

            loginPage.LogIn(login, password);

            Action checkIfLoginSucceed = () => new AdminStationsPage(_driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

    }
}
