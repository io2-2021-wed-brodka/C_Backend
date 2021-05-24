using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SeleniumTests2
{
    public class AdminLoginTests : TestsBase
    {
        public AdminLoginTests(ITestOutputHelper output) : base(output)
        {
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

        [Fact]
        public void LogOutShouldSucceed()
        {
            _driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            var loginPage = adminStationsPage.LogOut();
        }
    }
}
