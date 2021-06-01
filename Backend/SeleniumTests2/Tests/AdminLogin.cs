using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminLoginTests : TestsBase
    {
        public AdminLoginTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AdminLogInWithCorrectCredentialsShouldSucceed()
        {
            Driver.OpenAdminTab();
            var loginPage = new LoginPage(Driver, true);

            loginPage.LogIn("admin", "admin");
            Action checkIfLoginSucceed = () => new AdminStationsPage(Driver);
            
            checkIfLoginSucceed();
        }

        [Fact]
        public void AdminLogInWithWrongCredentialsShouldFail()
        {
            Driver.OpenAdminTab();
            var loginPage = new LoginPage(Driver, true);

            loginPage.LogIn("some-non-existing-user", "with-password");

            Action checkIfLoginSucceed = () => new AdminStationsPage(Driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public async Task AdminLogInWithUserCredentialsShouldFail()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await Api.SignUp(login, password);

            Driver.OpenAdminTab();
            var loginPage = new LoginPage(Driver, true);

            loginPage.LogIn(login, password);

            Action checkIfLoginSucceed = () => new AdminStationsPage(Driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public void LogOutShouldSucceed()
        {
            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            var loginPage = adminStationsPage.LogOut();
        }
    }
}
